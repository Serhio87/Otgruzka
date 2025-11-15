using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class dob : Form
    {
        // Определяем событие
        public event Action DataUpdated;
        private int poz;

        public dob(int dob_poz)
        {
            InitializeComponent();
            poz = dob_poz;
            ConfigDob();
            // Устанавливаем фокус на textBox1
            textBox1.Select();
            textBox1.Focus();
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
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string queryPr = @"INSERT INTO profil (profil) VALUES (?);";
            dobProd(queryPr);
            DataUpdated?.Invoke();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string queryDl = @"INSERT INTO dlina (dlina) VALUES (?);";
            dobProd(queryDl);
            DataUpdated?.Invoke();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string queryKl = @"INSERT INTO klass (marka) VALUES (?);";
            dobProd(queryKl);
            // Вызываем событие перед закрытием формы
            DataUpdated?.Invoke();
            this.Close();
        }

        private void dobProd(string query)
        {
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                conn.Open();
                try
                {
                    string text = textBox1.Text;

                    using (OleDbCommand command = new OleDbCommand(query, conn))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("?", text);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешно!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}