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

namespace Otgruzka
{
    public partial class dob: Form
    {
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";

        // Определяем событие
        public event Action DataUpdated;

        private int dob_poz;
        private int poz;

        public dob(int dob_poz)
        {
            InitializeComponent();
            poz = dob_poz;
            ConfigDob();

            textBox1.Select(); // Устанавливаем фокус на textBox1
            textBox1.Focus(); // Убедитесь, что фокус установлен
        }

        private void ConfigDob()
        {
            switch (poz)
            {
                case 1:
                    this.label1.Text = $"Введите профиль:";
                    button1.Visible = true;
                    break;

                case 2:
                    this.label1.Text = $"Введите длину:";
                    button2.Visible = true;
                    break;

                case 3:
                    this.label1.Text = $"Введите класс стали:";
                    button3.Visible = true;
                    break;

                case 4:
                    this.label1.Text = $"Введите стандарт:";
                    button4.Visible = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    int profil = Convert.ToInt32(textBox1.Text);

                    // Запрос INSERT
                    string query = @"INSERT INTO profil (profil) VALUES (@profil);";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@profil", profil);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешно!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            // Вызываем событие перед закрытием формы
            DataUpdated?.Invoke();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    int dlina = Convert.ToInt32(textBox1.Text);

                    // Запрос INSERT
                    string query = @"INSERT INTO dlina (dlina) VALUES (@dlina);";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@dlina", dlina);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешно!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            // Вызываем событие перед закрытием формы
            DataUpdated?.Invoke();

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    string marka = textBox1.Text;

                    // Запрос INSERT
                    string query = @"INSERT INTO klass (marka) VALUES (@marka);";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@marka", marka);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешно!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            // Вызываем событие перед закрытием формы
            DataUpdated?.Invoke();

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    string standart = textBox1.Text;

                    // Запрос INSERT
                    string query = @"INSERT INTO standart (standart) VALUES (@standart);";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@standart", standart);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешно!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            // Вызываем событие перед закрытием формы
            DataUpdated?.Invoke();

            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
