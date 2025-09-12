using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;

namespace Otgruzka
{
    public partial class newPok: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

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
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    string nazvanie = textBox1.Text;
                    string strana = textBox2.Text;
                    string gorod = textBox3.Text;

                    // Запрос INSERT
                    string query = @"INSERT INTO pokupatel (nazvanie, gorod, strana) VALUES (@nazvanie, @gorod, @strana);";
                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@nazvanie", nazvanie);
                        command.Parameters.AddWithValue("@gorod", gorod);
                        command.Parameters.AddWithValue("@strana", strana);
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
