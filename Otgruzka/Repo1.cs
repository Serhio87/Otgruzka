using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Otgruzka
{
    public partial class Repo1: Form
    {
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\UNIVER\\3session\\SVPP\\KP2025\\Otgruzka\\Otgruzka\\newBD.accdb";
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        //      private int labelWidth;

        public Repo1()
        {
            InitializeComponent();
            Runner();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (label2.Left > -label2.Width)
            {
                label2.Left -= 5;
            }
            else
            {
                label2.Left = panel1.Width;
            }
        }
        private void Runner()
        {
            label2.AutoSize = true;
            // Устанавливаем таймер
            Timer timer = new Timer();
            timer.Interval = 100; // Интервал в миллисекундах
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Repo1_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
            this.reportViewer3.RefreshReport();
            this.reportViewer4.RefreshReport();
        }

        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();
                string query = "SELECT pokupatel.nazvanie, profil.profil, Sum(Izdelie.ves_izd) AS Sumvesizd, Sum(Izdelie.price) AS Sumprice\r\nFROM profil INNER JOIN (pokupatel INNER JOIN (Izdelie INNER JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON pokupatel.Код = Prodazha.kod_pokup) ON profil.Код = Izdelie.profil\r\nGROUP BY pokupatel.nazvanie, profil.profil;\r\n"; 
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
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

        private DataTable GetData1()
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();
                string query = "SELECT sotrudniki.fam, sotrudniki.im, sotrudniki.otch, Sum(Izdelie.ves_izd) AS [Sumvesizd]\r\nFROM sotrudniki INNER JOIN (profil INNER JOIN ((dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) INNER JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON profil.Код = Izdelie.profil) ON sotrudniki.tab_nomer = Prodazha.kod_sotr\r\nGROUP BY sotrudniki.fam, sotrudniki.im, sotrudniki.otch;";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
        private void ShowReport1()
        {
            DataTable data = GetData1();
            ReportDataSource rds = new ReportDataSource("DataSet2", data); // имя набора данных
            reportViewer2.LocalReport.DataSources.Clear();
            reportViewer2.LocalReport.DataSources.Add(rds);
            reportViewer2.RefreshReport();
        }

        private DataTable GetData4()
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();
                string query = "SELECT profil.profil, dlina.dlina, klass.marka, Sum(Izdelie.ves_izd) AS Sumvesizd\r\nFROM standart INNER JOIN (profil INNER JOIN (klass INNER JOIN ((dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) LEFT JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil) ON standart.Код = Izdelie.standart\r\nGROUP BY profil.profil, dlina.dlina, klass.marka, Prodazha.date_prod\r\nHAVING (((Prodazha.date_prod) Is Null));\r\n";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
        private void ShowReport4()
        {
            DataTable data = GetData4();
            ReportDataSource rds = new ReportDataSource("DataSet4", data); // имя набора данных
            reportViewer3.LocalReport.DataSources.Clear();
            reportViewer3.LocalReport.DataSources.Add(rds);
            reportViewer3.RefreshReport();
        }

        private DataTable GetData5()
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();
                string query = "SELECT profil.profil, dlina.dlina, klass.marka, Sum(Izdelie.ves_izd) AS Sumvesizd\r\nFROM standart INNER JOIN (profil INNER JOIN (klass INNER JOIN ((dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) LEFT JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil) ON standart.Код = Izdelie.standart\r\nGROUP BY profil.profil, dlina.dlina, klass.marka, Prodazha.date_prod\r\nHAVING (((Prodazha.date_prod) Is Not Null));\r\n";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
        private void ShowReport5()
        {
            DataTable data = GetData5();
            ReportDataSource rds = new ReportDataSource("DataSet5", data); // имя набора данных
            reportViewer4.LocalReport.DataSources.Clear();
            reportViewer4.LocalReport.DataSources.Add(rds);
            reportViewer4.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            label1.Visible = false;
            reportViewer2.Visible = false;
            reportViewer1.Visible = true;
            reportViewer3.Visible = false;
            reportViewer4.Visible = false;
            ShowReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            label1.Visible = false;
            reportViewer1.Visible = false;
            reportViewer2.Visible = true;
            reportViewer3.Visible = false;
            reportViewer4.Visible = false;
            ShowReport1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            label1.Visible = false;
            reportViewer2.Visible = false;
            reportViewer1.Visible = false;
            reportViewer3.Visible = true;
            reportViewer4.Visible = false;
            ShowReport4();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label2.Visible = true;
            label1.Visible = false;
            reportViewer2.Visible = false;
            reportViewer1.Visible = false;
            reportViewer3.Visible = false;
            reportViewer4.Visible = true;
            ShowReport5();
        }
    }
}
