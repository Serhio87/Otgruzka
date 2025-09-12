using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Otgruzka.Prodazha;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Otgruzka
{
    public partial class Repo2: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private int tabNomer;
        private int pok;

        public Repo2()
        {
            InitializeComponent();
            LoadPokup();
            LoadSotr();

            radioButton3.Checked = false;
            radioButton4.Checked = false;

            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
        }

        private class Pokup
        {
            public int Код { get; set; }
            public string Nazvanie { get; set; }
            public string Gorod { get; set; }
            public string Strana { get; set; }

            public override string ToString()
            {
                return $"{Nazvanie,-10} | {Gorod,-10} "; // - для выравнивания влево
            }
        }
        private class Sotr
        {
            public int tab_nomer { get; set; }
            public string fam { get; set; }
            public string im { get; set; }
            public string otch { get; set; }

            public override string ToString()
            {
                return $"{fam,-10} {im,-10} {otch,-10}"; // - для выравнивания влево
            }
        }

        private void LoadPokup()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();
                string query = "SELECT Код, nazvanie, gorod, strana FROM pokupatel"; // Запрос на получение названий 
                using (OleDbCommand command = new OleDbCommand(query, myConnection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pokup pokup = new Pokup
                            {
                                Код = Convert.ToInt32(reader["Код"]),
                                Nazvanie = reader["nazvanie"].ToString(),
                                Gorod = reader["gorod"].ToString(),
                                Strana = reader["strana"].ToString()
                            };
                            comboBox1.Items.Add(pokup); // Добавляем названия в ComboBox
                        }
                    }
                }
            }
        }

        private void LoadSotr()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                string query = @"SELECT sotrudniki.tab_nomer, sotrudniki.fam, sotrudniki.im, sotrudniki.otch, dolzhnost.dolzhn
FROM dolzhnost INNER JOIN sotrudniki ON dolzhnost.Код = sotrudniki.dolzhn
WHERE (((dolzhnost.dolzhn)=""Бригадир"")) OR (((dolzhnost.dolzhn)=""Мастер""));"; // Запрос на получение

                using (OleDbCommand command = new OleDbCommand(query, myConnection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sotr sotr = new Sotr
                            {
                                tab_nomer = Convert.ToInt32(reader["tab_nomer"]),
                                fam = reader["fam"].ToString(),
                                im = reader["im"].ToString(),
                                otch = reader["otch"].ToString()
                            };
                            comboBox2.Items.Add(sotr); // Добавляем в ComboBox
                        }
                    }
                }
            }
        }

        private void Check()
        {
            if (radioButton3.Checked)
            {
                label4.Visible = true;
                label5.Visible = false;
                button1.Visible = false;
                button2.Visible = true;
               // label1.Visible = true;
                comboBox1.Visible = true;
              //  label4.Visible = false;
                comboBox2.Visible = false;
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                dateTimePicker1.ResetText();
                dateTimePicker2.ResetText();
            }
            else if (radioButton4.Checked)
            {
                label4.Visible = false;
                label5.Visible = true;
                button1.Visible = true;
                button2.Visible = false;
              //  label1.Visible = false;
                comboBox1.Visible = false;
              //  label4.Visible = true;
                comboBox2.Visible = true;
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                dateTimePicker1.ResetText();
                dateTimePicker2.ResetText();
            }
            if (radioButton2.Checked)
            {
                groupBox1.Visible = true;
            }
            else { groupBox1.Visible = false; }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Check();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Check();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Check();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Check();
        }

        private void Repo2_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reportViewer2.Visible = false;
            groupBox1.Visible = false;
            reportViewer1.Visible = true;
            ShowReport();
            dateTimePicker1.ResetText();
            dateTimePicker2.ResetText();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reportViewer1.Visible = false;
            groupBox1.Visible = false;
            reportViewer2.Visible = true;
            ShowReport1();
            dateTimePicker1.ResetText();
            dateTimePicker2.ResetText();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sotr select = (Sotr)comboBox2.SelectedItem;
            if (select != null)
            {
                tabNomer = select.tab_nomer;
            }
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Pokup select = (Pokup)comboBox1.SelectedItem;
            if (select != null)
            {
                pok = select.Код;
                Console.WriteLine($"Выбран код покупателя: {pok}");
            }
        }

        private DataTable GetData()
        {
            DataTable dataTable1 = new DataTable();

            if (radioButton2.Checked == true)
            {
                using (OleDbConnection connection = new OleDbConnection(connectString))
                {
                    connection.Open();

                    //      string D_ot = dateTimePicker1.Value.ToShortDateString();
                    //      string D_do = dateTimePicker2.Value.ToShortDateString();
                    // Получаем даты из DateTimePicker
                    DateTime D_ot = dateTimePicker1.Value.Date; // Дата начала
                    DateTime D_do = dateTimePicker2.Value.Date; // Дата окончания

                    string query = @"
            SELECT sotrudniki.fam, sotrudniki.im, sotrudniki.otch, Izdelie.ves_izd, Izdelie.price, Prodazha.date_prod
            FROM sotrudniki 
            INNER JOIN (Izdelie INNER JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) 
            ON sotrudniki.tab_nomer = Prodazha.kod_sotr
            WHERE Prodazha.date_prod >= ? 
            AND Prodazha.date_prod <= ? 
            AND sotrudniki.tab_nomer = ?;"; // Используем позиционные параметры

                    Console.WriteLine(query);
                    Console.WriteLine($"tabNomer: {tabNomer}, date_ot: {D_ot}, date_do: {D_do}");


                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                        command.Parameters.AddWithValue("?", D_ot);
                        command.Parameters.AddWithValue("?", D_do.AddDays(1));
                        command.Parameters.AddWithValue("?", tabNomer);

                        adapter.Fill(dataTable1);

                        int rowCount = dataTable1.Rows.Count;
                        Console.WriteLine($"Количество строк в dataTable: {rowCount}");
                    }
                }
            }
            else if (radioButton1.Checked == true)
            {
                using (OleDbConnection connection = new OleDbConnection(connectString))
                {
                    connection.Open();

                    string query = @"
            SELECT sotrudniki.fam, sotrudniki.im, sotrudniki.otch, Izdelie.ves_izd, Izdelie.price, Prodazha.date_prod
FROM sotrudniki 
INNER JOIN (Izdelie INNER JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) 
ON sotrudniki.tab_nomer = Prodazha.kod_sotr
WHERE sotrudniki.tab_nomer = ?;";

                    Console.WriteLine(query);
                    Console.WriteLine($"tabNomer: {tabNomer}");

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                        
                        command.Parameters.AddWithValue("?", tabNomer);

                        adapter.Fill(dataTable1);

                        int rowCount = dataTable1.Rows.Count;
                        Console.WriteLine($"Количество строк в dataTable: {rowCount}");
                    }
                }
            }
                return dataTable1;
        }
        private DataTable GetData1()
        {
            DataTable dataTable1 = new DataTable();

            if (radioButton2.Checked == true)
            {
                using (OleDbConnection connection = new OleDbConnection(connectString))
                {
                    connection.Open();

                    //      string D_ot = dateTimePicker1.Value.ToShortDateString();
                    //      string D_do = dateTimePicker2.Value.ToShortDateString();
                    // Получаем даты из DateTimePicker
                    DateTime D_ot = dateTimePicker1.Value.Date; // Дата начала
                    DateTime D_do = dateTimePicker2.Value.Date; // Дата окончания

                    string query1 = @"
            SELECT pokupatel.nazvanie, Prodazha.date_prod, profil.profil, dlina.dlina, klass.marka, Izdelie.ves_izd
            FROM klass INNER JOIN (dlina INNER JOIN (profil INNER JOIN (Izdelie INNER JOIN (pokupatel INNER JOIN Prodazha ON pokupatel.Код = Prodazha.kod_pokup) ON Izdelie.kod_izd = Prodazha.kod_izd) ON profil.Код = Izdelie.profil) ON dlina.Код = Izdelie.dlina) ON klass.Код = Izdelie.klass
            WHERE Prodazha.date_prod >= ? 
            AND Prodazha.date_prod <= ? 
            AND pokupatel.Код = ?;"; // Используем позиционные параметры

                    string query = @"SELECT pokupatel.nazvanie, Prodazha.date_prod, profil.profil, dlina.dlina, klass.marka, Izdelie.ves_izd
FROM profil INNER JOIN (pokupatel INNER JOIN (klass INNER JOIN ((dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) INNER JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON klass.Код = Izdelie.klass) ON pokupatel.Код = Prodazha.kod_pokup) ON profil.Код = Izdelie.profil
WHERE pokupatel.nazvanie = ? 
AND Prodazha.date_prod>=?
AND Prodazha.date_prod<=?;";

                    Console.WriteLine(query);
                    Console.WriteLine($"pokup: {pok}, date_ot: {D_ot}, date_do: {D_do}");


                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                        command.Parameters.AddWithValue("?", D_ot);
                        command.Parameters.AddWithValue("?", D_do.AddDays(1));
                        command.Parameters.AddWithValue("?", pok);

                        adapter.Fill(dataTable1);

                        int rowCount = dataTable1.Rows.Count;
                        Console.WriteLine($"Количество строк в dataTable: {rowCount}");
                    }
                }
            }
            else if (radioButton1.Checked == true)
            {
                using (OleDbConnection connection = new OleDbConnection(connectString))
                {
                    connection.Open();

                    string query1 = @"
            SELECT pokupatel.nazvanie, Prodazha.date_prod, profil.profil, dlina.dlina, klass.marka, Izdelie.ves_izd
            FROM klass INNER JOIN (dlina INNER JOIN (profil INNER JOIN (Izdelie INNER JOIN (pokupatel 
INNER JOIN Prodazha ON pokupatel.Код = Prodazha.kod_pokup) ON Izdelie.kod_izd = Prodazha.kod_izd) 
ON profil.Код = Izdelie.profil) ON dlina.Код = Izdelie.dlina) ON klass.Код = Izdelie.klass
            WHERE pokupatel.Код = ?;";

                    string query = @"SELECT pokupatel.nazvanie, Prodazha.date_prod, profil.profil, dlina.dlina, klass.marka, Izdelie.ves_izd, pokupatel.Код
                    FROM profil INNER JOIN(pokupatel INNER JOIN(klass INNER JOIN((dlina 
INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) 
INNER JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) 
ON klass.Код = Izdelie.klass) ON pokupatel.Код = Prodazha.kod_pokup) 
ON profil.Код = Izdelie.profil
GROUP BY pokupatel.nazvanie, Prodazha.date_prod, profil.profil, dlina.dlina, klass.marka, Izdelie.ves_izd, pokupatel.Код;";


                    Console.WriteLine(query);
                    Console.WriteLine($"pokup: {pok}");

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);


                        command.Parameters.AddWithValue("?", pok);

                        adapter.Fill(dataTable1);

                        int rowCount = dataTable1.Rows.Count;
                        Console.WriteLine($"Количество строк в dataTable: {rowCount}");
                    }
                }
            }
            return dataTable1;
        }

        private void ShowReport()
        {
            DataTable data = GetData();
            ReportDataSource rds = new ReportDataSource("DataSetSotr1", data); // имя набора данных
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }
        private void ShowReport1()
        {
            DataTable data = GetData1();
            ReportDataSource rds = new ReportDataSource("DataSetPok", data); // имя набора данных
            reportViewer2.LocalReport.DataSources.Clear();
            reportViewer2.LocalReport.DataSources.Add(rds);
            reportViewer2.RefreshReport();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
