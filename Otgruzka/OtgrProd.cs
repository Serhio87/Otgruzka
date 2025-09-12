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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Otgruzka
{
    public partial class OtgrProd: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        private int tab_n;

        public OtgrProd(int tab_nomer)
        {
            InitializeComponent();
            tab_n = tab_nomer;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Enabled = false;
                using (OleDbConnection myConnection = new OleDbConnection(connectString))
                {
                    if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        try
                        {
                            myConnection.Open();

                            string query = @"SELECT Prikaz.id_prik, pokupatel.nazvanie, pokupatel.strana, Prikaz.prof, Prikaz.dlina, Prikaz.klass, Prikaz.ves, Prikaz.kolich, Prikaz.ispoln
FROM pokupatel INNER JOIN Prikaz ON pokupatel.Код = Prikaz.pokup
WHERE (((Prikaz.id_prik)=?));";

                            using (OleDbCommand command = new OleDbCommand(query, myConnection))
                            {
                                command.Parameters.AddWithValue("?", textBox1.Text);

                                // Создаем адаптер и заполняем DataTable
                                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                if (dataTable.Rows.Count == 0)
                                {
                                    MessageBox.Show("Приказ отсутствует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    textBox1.Enabled = true;
                                    textBox1.Focus();
                                    return;
                                }

                                DataRow row = dataTable.Rows[0];
                                int id_prik = row.Field<int>("id_prik");
                                string pokup = row["nazvanie"].ToString();
                                string strana = row["strana"].ToString();
                                string prof = row["prof"].ToString();
                                string dlina = row["dlina"].ToString();
                                string klass = row["klass"].ToString();
                                string ves = row["ves"].ToString();
                                int kolich = Convert.ToInt32(row["kolich"].ToString());
                                string ispoln = row["ispoln"].ToString();

                                if ((string.IsNullOrEmpty(ispoln) && kolich == 0))
                                {
                                    textBox2.Text = pokup;
                                    textBox3.Text = strana;
                                    textBox4.Text = prof;
                                    textBox5.Text = dlina;
                                    textBox6.Text = klass;
                                    textBox7.Text = ves;
                                    //textBox8.Text = kolich;
                                }
                                else
                                {
                                    MessageBox.Show("Приказ отгружен" + ispoln + "!" + "\n" + "В объёме:" + kolich + " тонн.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    textBox1.Enabled = true;
                                    textBox1.Focus();
                                    return;
                                }
                            }
                            VidTr();
                            FIO(tab_n);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void VidTr()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();
                string query = "SELECT Vid FROM VidTr"; // Запрос на получение названий
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBoxVidTr.Items.Add(reader["Vid"].ToString()); // Добавляем названия в ComboBox
                }
            }
            tbNomTrSr.Enabled = true;
            comboBoxVidTr.Enabled = true;
        }

        private void FIO(int tab_n)
        {
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                conn.Open();
                string query = @"SELECT sotrudniki.fam, sotrudniki.im, sotrudniki.otch, dolzhnost.dolzhn, sotrudniki.tab_nomer
FROM dolzhnost INNER JOIN sotrudniki ON dolzhnost.Код = sotrudniki.dolzhn
WHERE (((sotrudniki.tab_nomer)=?));";
                OleDbCommand command = new OleDbCommand(query, conn);
                command.Parameters.AddWithValue("?", tab_n);

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) //проверка есть ли результаты
                    {
                        string dol = reader["dolzhn"].ToString();
                        string fam = reader["fam"].ToString();
                        string im = reader["im"].ToString();
                        string otch = reader["otch"].ToString();
                        textBox9.Text = $"{dol} {fam} {im} {otch}";
                    }
                    else { MessageBox.Show("ошибка"); }
                }
            }
        }

        private void textBoxPlavka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int plavka = Convert.ToInt32(textBoxPlavka.Text);
                using (OleDbConnection myConnection = new OleDbConnection(connectString))
                {
                    if (!string.IsNullOrEmpty(textBoxPlavka.Text))
                    {
                        Plavka pl = new Plavka(plavka);
                        pl.Show();
                    }
                }
            }
        }
    }
}
