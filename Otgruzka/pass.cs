using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class pass : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private int tab_nom; 

        public pass(int tab_n) 
        {
            InitializeComponent();
            tab_nom = Convert.ToInt32(tab_n); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pass; 
            int new_pass;
            int new_pass1;

            // Проверяем, что введенные значения можно преобразовать в int
            if (!int.TryParse(textBox1.Text, out pass) ||
                !int.TryParse(textBox2.Text, out new_pass) ||
                !int.TryParse(textBox3.Text, out new_pass1))
            {
                MessageBox.Show("Пароль - целое число.");
                return;
            }

            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    // Запрос для проверки текущего пароля
                    string query = @"SELECT password FROM sotrudniki WHERE tab_nomer = ?";
                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        command.Parameters.AddWithValue("?", tab_nom); 
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                        DataTable db = new DataTable();
                        adapter.Fill(db);

                        if (db.Rows.Count > 0)
                        {
                            DataRow row = db.Rows[0];
                            int storedPassword = Convert.ToInt32(row["password"]); // Приводим к int

                            // Проверка введенного пароля
                            if (pass == storedPassword && new_pass == new_pass1)
                            {
                                // Обновление пароля
                                string query1 = @"UPDATE sotrudniki SET [password] = ? WHERE tab_nomer = ?";
                                using (OleDbCommand command1 = new OleDbCommand(query1, myConnection))
                                {
                                    command1.Parameters.AddWithValue("?", new_pass); // Новый пароль
                                    command1.Parameters.AddWithValue("?", tab_nom);  // Табельный номер
                                    command1.ExecuteNonQuery();
                                    MessageBox.Show("Пароль успешно изменен!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неверный текущий пароль или пароли не совпадают.");
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
                }
            }
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void textBox3_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true; // Предотвращаем звуковой сигнал при нажатии Enter
            }
        }
    }
}
