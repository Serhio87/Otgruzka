using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class newPok : Form
    {
        // Определяем событие
        public event Action DataUpdated;

        public newPok()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                conn.Open();

                try
                {
                    string nazvanie = textBox1.Text;
                    string strana = textBox2.Text;
                    string gorod = textBox3.Text;

                    // Запрос INSERT
                    string query = @"INSERT INTO pokupatel (nazvanie, gorod, strana) VALUES (?, ?, ?);";
                    using (OleDbCommand command = new OleDbCommand(query, conn))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("?", nazvanie);
                        command.Parameters.AddWithValue("?", gorod);
                        command.Parameters.AddWithValue("?", strana);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Успешно добавлено.");
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
    }
}
