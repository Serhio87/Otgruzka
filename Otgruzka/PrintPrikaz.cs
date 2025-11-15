using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class PrintPrikaz : Form
    {
        private int id;

        public PrintPrikaz(int id_prik)
        {
            InitializeComponent();

            id = id_prik;
            ShowReport();

            this.Text = $"Печать приказа на отгрузку №{id}";
        }

        private void PrintPrikaz_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(Metods.ConnectionString))
            {
                connection.Open();

                string query = @"SELECT Prikaz.id_prik, pokupatel.nazvanie, pokupatel.gorod, pokupatel.strana, Prikaz.prof, Prikaz.dlina, Prikaz.klass, Prikaz.ves, Prikaz.date_prik, dolzhnost.dolzhn, sotrudniki.fam, Prikaz.ispoln, Prikaz.kolich
                                   FROM (dolzhnost INNER JOIN sotrudniki ON dolzhnost.Код = sotrudniki.dolzhn) INNER JOIN (pokupatel INNER JOIN Prikaz ON pokupatel.Код = Prikaz.pokup) ON sotrudniki.tab_nomer = Prikaz.ekon
                                  WHERE Prikaz.id_prik = ? ;";

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
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.RefreshReport();
        }
    }
}
