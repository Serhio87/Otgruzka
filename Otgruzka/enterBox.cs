using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Otgruzka
{
    public partial class enterBox : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        public enterBox()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("До новых встреч!");
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    try
                    {
                        myConnection.Open();

                        // Используем параметризованный запрос для предотвращения SQL-инъекций
                        string query = @"SELECT     sotrudniki.tab_nomer,
                                                    sotrudniki.fam,
                                                    sotrudniki.im,
                                                    sotrudniki.otch,
                                                    dolzhnost.dolzhn, 
                                                    sotrudniki.password
                                        FROM        dolzhnost 
                                        INNER JOIN  sotrudniki 
                                        ON          dolzhnost.Код = sotrudniki.dolzhn
                                        WHERE       tab_nomer = ?;";

                        using (OleDbCommand command = new OleDbCommand(query, myConnection))
                        {
                            command.Parameters.AddWithValue("?", textBox1.Text);

                            // Создаем адаптер и заполняем DataTable
                            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            if (dataTable.Rows.Count == 0)
                            {
                                MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (dataTable.Rows.Count > 0)
                            {
                                // Если данные найдены, извлекаем информацию
                                DataRow row = dataTable.Rows[0];
                                int tab_nomer = row.Field<int>("tab_nomer");
                                string fam = row["fam"].ToString();
                                string im = row["im"].ToString();
                                string otch = row["otch"].ToString();
                                string dolzhn = row["dolzhn"].ToString();
                                string password = row["password"]?.ToString(); // Используем оператор ?. для безопасного доступа

                                // Проверяем введенный пароль, если он был введен
                                if (!string.IsNullOrEmpty(textBox2.Text))
                                {
                                    if (password != null && password == textBox2.Text)
                                    {
                                        // Если пароль совпадает, продолжаем вход
                                        MessageBox.Show(fam + "\n" + im + "\n" + otch, "Добро пожаловать!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        string FIO = $"{fam} {im} {otch}";
                                        Prodazha p = new Prodazha(tab_nomer);
                                        if (dolzhn == "Экономист")
                                        {
                                            //Ekon ek = new Ekon(dolzhn, FIO, tab_nomer);
                                            //ek.Show();
                                            logo logo = new logo(dolzhn, FIO, tab_nomer);
                                            logo.Show();
                                        }
                                        else
                                        {   // Если вход успешен, показываем форму заставки
                                            logo logo = new logo(dolzhn, FIO, tab_nomer);
                                            logo.Show();
                                        }

                                        }
                                        this.Hide();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Неверный пароль. Попробуйте еще раз.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        
                    }
                    finally
                    {
                        myConnection.Close(); // Закрываем соединение
                    }
                }

            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            { 
                button1.PerformClick(); 
                e.SuppressKeyPress = true; // Предотвращаем звуковой сигнал при нажатии Enter
            }
        }
    }
}
