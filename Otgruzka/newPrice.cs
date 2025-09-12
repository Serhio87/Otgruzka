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
    public partial class newPrice: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private int kod;
        public newPrice(int kod_izd)
        {
            InitializeComponent();
            kod = kod_izd;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double newPr))
            {
                using (OleDbConnection myConnection = new OleDbConnection(connectString))
                {
                    myConnection.Open();

                    try
                    {
                        // Обновление
                        string query1 = @"UPDATE Izdelie SET price = ? WHERE kod_izd = ?";
                        using (OleDbCommand command1 = new OleDbCommand(query1, myConnection))
                        {
                            command1.Parameters.AddWithValue("?", newPr); // Новая цена
                            command1.Parameters.AddWithValue("?", kod);
                            command1.ExecuteNonQuery();
                            MessageBox.Show("Цена изменена.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите корректное значение цены."); // Сообщение об ошибке ввода
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true; // Предотвращаем звуковой сигнал при нажатии Enter
            }
        }
    }
}
