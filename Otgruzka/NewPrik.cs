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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Otgruzka
{
    public partial class NewPrik: Form
    {
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";

        private int selectedKod; //код покуп
        private int kodPr;
        private int kodDl; 
        private string kodKl;
        private int tab_n;

        public NewPrik(int tab_nomer)
        {
            InitializeComponent();
            LoadPokup();
            LoadProd();

            tab_n = tab_nomer;
            FIO(tab_n);

            MaximizeBox = false;
        }

        private class Pokup
        {
            public int Код { get; set; }
            public string Nazvanie { get; set; }
            public string Gorod { get; set; }
            public string Strana { get; set; }

            public override string ToString()
            {
                return $"{Nazvanie,-10} | {Gorod,-10} -- {Strana,-10}"; // -20 и -15 для выравнивания влево
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

        private void LoadProd()
        {
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();

                string queryPr = "SELECT profil FROM profil";
                string queryDl = "SELECT dlina FROM dlina";
                string queryKl = "SELECT marka FROM klass";

                using (OleDbCommand command = new OleDbCommand(queryPr, connection))
                {
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBoxProf.Items.Add(reader["profil"]);
                    }
                    reader.Close();
                }

                using (OleDbCommand command = new OleDbCommand(queryDl, connection))
                {
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBoxDl.Items.Add(reader["dlina"]);
                    }
                    reader.Close();
                }

                using (OleDbCommand command = new OleDbCommand(queryKl, connection))
                {
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBoxCl.Items.Add(reader["marka"]);
                    }
                    reader.Close();
                }
            }
        }

        private void FIO(int tab_n)
        {
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                conn.Open();
                string query = @"SELECT tab_nomer, fam, im, otch 
                            FROM sotrudniki 
                            WHERE tab_nomer = @tab_n;";
                OleDbCommand command = new OleDbCommand(query, conn);
                command.Parameters.AddWithValue("@tab_n", tab_n);

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) //проверка есть ли результаты
                    {
                        
                        string fam = reader["fam"].ToString();
                        string im = reader["im"].ToString();
                        string otch = reader["otch"].ToString();
                        textBox2.Text = $"{fam} {im} {otch}";
                    }
                    else { MessageBox.Show("ошибка"); }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            NewPrik np = new NewPrik(tab_n);
            np.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxProf_SelectedIndexChanged(object sender, EventArgs e)
        {
                kodPr = Convert.ToInt32(comboBoxProf.SelectedItem); // Код профиля
        }

        private void comboBoxDl_SelectedIndexChanged(object sender, EventArgs e)
        {
                kodDl = Convert.ToInt32(comboBoxDl.SelectedItem); 
        }

        private void comboBoxCl_SelectedIndexChanged(object sender, EventArgs e)
        {
                kodKl = Convert.ToString(comboBoxCl.SelectedItem); 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokup select = (Pokup)comboBox1.SelectedItem;
            if (select != null)
            {
                selectedKod = select.Код;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    if (comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите покупателя.");
                        return;
                    }
                    int pokup = selectedKod;

                    if (comboBoxProf.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, заполните данные продукции.");
                        return;
                    }
                    int prof = kodPr;

                    if (comboBoxDl.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, заполните данные продукции.");
                        return;
                    }
                    int dlina = kodDl;

                    if (comboBoxCl.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, заполните данные продукции.");
                        return;
                    }
                    string klass = kodKl;

                    string date_prik = dateTimePicker1.Value.ToShortDateString();
                    
                    int ekon = tab_n;

                    int ves = Convert.ToInt32(textBoxVes.Text);
                
                    // Запрос INSERT
                    string query = @"INSERT INTO Prikaz (pokup, prof, dlina, klass, ves, date_prik, ekon)
                                                   VALUES (@pokup, @profil, @dlina, @klass, @ves, @date_prik, @ekon);";
                    
                //    string query = @"INSERT INTO Prikaz ( pokup, profil, dlina, klass, ves, date_prik )
                //                SELECT Prikaz.pokup, Prikaz.profil, Prikaz.dlina, Prikaz.klass, Prikaz.ves, Prikaz.date_prik
               //                 FROM Prikaz;";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@pokup", pokup);
                        command.Parameters.AddWithValue("@prof", prof);
                        command.Parameters.AddWithValue("@dlina", dlina);
                        command.Parameters.AddWithValue("@klass", klass);
                        command.Parameters.AddWithValue("@ves", ves);
                        command.Parameters.AddWithValue("@date_prik", date_prik);
                        command.Parameters.AddWithValue("@ekon", ekon);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Приказ создан!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            this.Close();
            NewPrik np = new NewPrik(tab_n);
            np.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewPrik pp = new ViewPrik();
            pp.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            newPok n = new newPok();
            n.DataUpdated += DataUpdated; // Подписка на событие
            n.Show();
        }

        private void DataUpdated()
        {
            comboBox1.Items.Clear();
            LoadPokup();
        }
    }
}
