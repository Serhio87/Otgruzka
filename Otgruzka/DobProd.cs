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
using System.IO;

namespace Otgruzka
{
    public partial class DobProd : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private int labelWidth;
     //   string prof = prof;

        public DobProd()
        {
            InitializeComponent();
            Runner();
            LoadProfil();
            LoadDlina();
            LoadKlass();
            LoadStand();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Сдвигаем текст влево
            label2.Left -= 3;

            // Если текст вышел за пределы формы, перезапускаем его
            if (label2.Right < 0)
            {
                label2.Left = this.ClientSize.Width; // Перемещаем текст обратно вправо
            }
        }
        private void Runner()
        {
            // Устанавливаем текст для бегущей строки
            label2.Text = "Если нужный выбор отсутствует - нажмите кнопку 'Добавить'";
            label2.AutoSize = true;
            labelWidth = label2.Width;
            // Устанавливаем таймер
            Timer timer = new Timer();
            timer.Interval = 65; // Интервал в миллисекундах
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void LoadProfil()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();
                string query = "SELECT profil FROM profil"; // Запрос на получение названий
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["profil"].ToString()); // Добавляем названия в ComboBox
                }
            }
        }
        private void LoadDlina()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
                {
                myConnection.Open();
                string query = "SELECT dlina FROM dlina"; // Запрос на получение названий
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["dlina"].ToString()); // Добавляем названия в ComboBox
                }
            }
        }
        private void LoadKlass()
        {
            OleDbConnection myConnection = new OleDbConnection(connectString);
            {
                myConnection.Open();
                string query = "SELECT marka FROM klass"; // Запрос на получение названий
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox3.Items.Add(reader["marka"].ToString()); // Добавляем названия в ComboBox
                }
            }
        }
        private void LoadStand()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
                {
                myConnection.Open();
                string query = "SELECT standart FROM standart"; // Запрос на получение названий
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox4.Items.Add(reader["standart"].ToString()); // Добавляем названия в ComboBox
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dob dob1 = new dob(1);
            dob1.DataUpdated += DataUpdated; // Подписка на событие
            dob1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dob dob2 = new dob(2);
            dob2.DataUpdated += DataUpdated;
            dob2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dob dob3 = new dob(3);
            dob3.DataUpdated += DataUpdated;
            dob3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dob dob4 = new dob(4);
            dob4.DataUpdated += DataUpdated;
            dob4.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DataUpdated();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    if (comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите профиль.");
                        return;
                    }
                    int profil = Convert.ToInt32(comboBox1.SelectedItem.ToString());
                    // Запрос для получения кода
                    string queryProfil = @"SELECT Код FROM profil WHERE profil = @profil";
                    int profilCode;

                    using (OleDbCommand CommProfil = new OleDbCommand(queryProfil, myConnection))
                    {
                        CommProfil.Parameters.AddWithValue("@profil", profil);
                        profilCode = (int)CommProfil.ExecuteScalar(); // Получаем код
                    }

                    if (comboBox2.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите длину.");
                        return;
                    }
                    int dlina = Convert.ToInt32(comboBox2.SelectedItem.ToString());
                    // Запрос для получения кода
                    string queryDlina = @"SELECT Код FROM dlina WHERE dlina = @dlina";
                    int dlinaCode;

                    using (OleDbCommand CommDlina = new OleDbCommand(queryDlina, myConnection))
                    {
                        CommDlina.Parameters.AddWithValue("@dlina", dlina);
                        dlinaCode = (int)CommDlina.ExecuteScalar(); // Получаем код
                    }

                    if (comboBox3.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите класс стали.");
                        return;
                    }
                    string klass = comboBox3.SelectedItem.ToString();
                    // Запрос для получения кода
                    string queryKlass = @"SELECT Код FROM klass WHERE marka = @marka";
                    int klassCode;

                    using (OleDbCommand CommKlass = new OleDbCommand(queryKlass, myConnection))
                    {
                        CommKlass.Parameters.AddWithValue("@marka", klass);
                        klassCode = (int)CommKlass.ExecuteScalar(); // Получаем код
                    }

                    if (comboBox4.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите стандарт.");
                        return;
                    }
                    string standart = comboBox4.SelectedItem.ToString();
                    // Запрос для получения кода
                    string queryStand = @"SELECT Код FROM standart WHERE standart = @standart";
                    int standCode;

                    using (OleDbCommand CommStand = new OleDbCommand(queryStand, myConnection))
                    {
                        CommStand.Parameters.AddWithValue("@standart", standart);
                        standCode = (int)CommStand.ExecuteScalar(); // Получаем код
                    }

                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show("Пожалуйста, введите вес.");
                        return;
                    }

                    double ves_izd;
                    if (!double.TryParse(textBox1.Text, out ves_izd))
                    {
                        MessageBox.Show("Введите корректное значение для веса.");
                        return;
                    }

                    double price;
                    if (!double.TryParse(textBox2.Text, out price))
                    {
                        MessageBox.Show("Введите корректное значение для цены.");
                        return;
                    }

                    string date_post = dateTimePicker1.Value.ToShortDateString();

                    string nomer_dok = textBox_dokum.Text;

                  //  double price = ves_izd * 10.12;

                    // Запрос INSERT
                    string query = @"INSERT INTO Izdelie (profil, dlina, klass, standart, price, ves_izd, date_post, nomer_dok)
                         VALUES (@profilCode, @dlinaCode, @klassCode, @standCode, @price, @ves_izd, @date_post, @nomer_dok);";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@profilCode", profilCode);
                        command.Parameters.AddWithValue("@dlina", dlinaCode);
                        command.Parameters.AddWithValue("@klass", klassCode);
                        command.Parameters.AddWithValue("@standart", standCode);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@ves_izd", ves_izd);
                        command.Parameters.AddWithValue("@date_post", date_post);
                        command.Parameters.AddWithValue("@nomer_dok", nomer_dok);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Добавлено");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            textBox1.Clear();
            textBox2.Clear();
        }

        private void DataUpdated()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            LoadProfil();
            LoadDlina();
            LoadKlass();
            LoadStand();
        }
    }
}
