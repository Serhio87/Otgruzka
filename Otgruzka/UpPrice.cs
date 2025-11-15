using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class UpPrice : Form
    {
        private DataTable db;
        private BindingSource bindingSource = new BindingSource();

        public UpPrice()
        {
            InitializeComponent();
            LoadPrice();
            MaximizeBox = false;
        }

        private void LoadPrice()
        {
            using (OleDbConnection myConnection = new OleDbConnection(Metods.ConnectionString))
            {
                try
                {
                    string query = @"SELECT profil.Код, 
                                            profil.profil       AS [Профиль], 
                                            profil.price_pog_m  AS [Цена погонного метра] 
                                       FROM profil;";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, myConnection);
                    db = new DataTable();
                    adapter.Fill(db);
                    bindingSource.DataSource = db;
                    dataGridView1.DataSource = db;

                    //изменение шрифта
                    dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 14);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold); //заголовок

                    // Выравнивание текста в заголовках
                    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Скрываем столбцы по умолчанию
                    dataGridView1.Columns["Код"].Visible = false;

                    dataGridView1.RowHeadersVisible = false;

                    dataGridView1.Columns["Профиль"].ReadOnly = true;

                    //изменение цвета
                    dataGridView1.Columns[1].DefaultCellStyle.BackColor = SystemColors.Control;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            LoadPrice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                conn.Open();

                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Проверяем, что строка не новая и не пустая
                        if (!row.IsNewRow)
                        {
                            // Получаем код профиля и цену из ячеек
                            var kod = row.Cells["Код"].Value; // имя столбца с кодом
                            double price = Convert.ToDouble(row.Cells[2].Value); // Индекс 2 - это индекс столбца с ценой

                            string query = "UPDATE profil SET profil.price_pog_m = ? WHERE profil.Код = ?";

                            using (OleDbCommand commandIzd = new OleDbCommand(query, conn))
                            {
                                commandIzd.Parameters.AddWithValue("?", price);
                                commandIzd.Parameters.AddWithValue("?", kod);
                                commandIzd.ExecuteNonQuery();
                                Console.WriteLine($"Код: {kod}, Цена: {price}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    }
}
