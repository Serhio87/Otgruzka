using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Otgruzka
{
    public partial class PrintPrikaz: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        private int id;

        public PrintPrikaz(int id_prik)
        {
            InitializeComponent();

            id = id_prik;
            ShowReport();
            
            this.Text = $"Приказ на отгрузку №{id}";
        }

        
        private void PrintPrikaz_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();

                
                /* рабочий string query = @"SELECT Prikaz.id_prik, pokupatel.nazvanie, pokupatel.gorod, pokupatel.strana, Prikaz.prof, Prikaz.dlina, Prikaz.klass, Prikaz.ves, Prikaz.date_prik, sotrudniki.dolzhn, sotrudniki.fam, Prikaz.ispoln, Prikaz.kolich
FROM sotrudniki INNER JOIN(pokupatel INNER JOIN Prikaz ON pokupatel.Код = Prikaz.pokup) ON sotrudniki.tab_nomer = Prikaz.ekon;";*/
                string query = @"SELECT Prikaz.id_prik, pokupatel.nazvanie, pokupatel.gorod, pokupatel.strana, Prikaz.prof, Prikaz.dlina, Prikaz.klass, Prikaz.ves, Prikaz.date_prik, dolzhnost.dolzhn, sotrudniki.fam, Prikaz.ispoln, Prikaz.kolich
FROM (dolzhnost INNER JOIN sotrudniki ON dolzhnost.Код = sotrudniki.dolzhn) INNER JOIN (pokupatel INNER JOIN Prikaz ON pokupatel.Код = Prikaz.pokup) ON sotrudniki.tab_nomer = Prikaz.ekon
WHERE (((Prikaz.id_prik)= ? ));";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", id);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
        private void ShowReport()
        {
            DataTable data = GetData();
            ReportDataSource rds = new ReportDataSource("DataSet1", data); // имя набора данных
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }
    }
}
