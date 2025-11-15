using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class pokupatel : Form
    {
        private DataTable db;

        public pokupatel()
        {
            InitializeComponent();
            LoadForm();

            dataGridView1.RowHeadersVisible = false;
            MaximizeBox = false;

            //изменение шрифта
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 16);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold); //заголовок
        }

        private void LoadForm()
        {
            OleDbConnection conn = new OleDbConnection(Metods.ConnectionString);
            conn.Open();

            string query = @"SELECT nazvanie AS [Название], 
                                    gorod    AS [Город], 
                                    strana   AS [Страна] FROM pokupatel;";

            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            db = new DataTable();
            adapter.Fill(db);
            dataGridView1.DataSource = db;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newPok n = new newPok();
            n.DataUpdated += DataUpdated;
            n.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataUpdated()
        {
            LoadForm();
        }
    }
}
