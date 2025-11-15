using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class pass : Form
    {
        private Metods hashPass = new Metods(); // Создаем экземпляр класса HashPass
        private int tab_nom;

        public pass(int tab_n)
        {
            InitializeComponent();
            tab_nom = tab_n;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pass = textBox1.Text;
            string new_pass = textBox2.Text;
            string new_pass1 = textBox3.Text;

            // Проверяем, что пароли не пустые
            if (string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(new_pass) || string.IsNullOrWhiteSpace(new_pass1))
            {
                MessageBox.Show("Пароль не может быть пустым.");
                return;
            }
            if (new_pass != new_pass1)
            {
                MessageBox.Show("Новые пароли не совпадают.");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                conn.Open();

                try
                {
                    // Запрос для проверки текущего пароля
                    string query = @"SELECT password FROM sotrudniki WHERE tab_nomer = ?";

                    using (OleDbCommand command = new OleDbCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("?", tab_nom);
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                        DataTable db = new DataTable();
                        adapter.Fill(db);

                        if (db.Rows.Count > 0)
                        {
                            string storedHash = db.Rows[0]["password"].ToString(); // Получаем хеш

                            // Проверка введенного пароля
                            if (hashPass.VerifyPassword(pass, storedHash))
                            {
                                // Обновление пароля
                                string newHash = hashPass.HashPassword(new_pass);
                                string query1 = @"UPDATE sotrudniki SET [password] = ? WHERE tab_nomer = ?";
                                using (OleDbCommand command1 = new OleDbCommand(query1, conn))
                                {
                                    command1.Parameters.AddWithValue("?", newHash);
                                    command1.Parameters.AddWithValue("?", tab_nom);
                                    command1.ExecuteNonQuery();
                                    MessageBox.Show("Пароль успешно изменен!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неверный текущий пароль.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
