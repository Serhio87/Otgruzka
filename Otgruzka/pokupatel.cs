using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Otgruzka
{
    public partial class pokupatel: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private OleDbConnection myConnection;
        private BindingSource bindingSource = new BindingSource();
        private DataTable db;

        public pokupatel()
        {
            InitializeComponent();
            LoadForm();

            dataGridView1.RowHeadersVisible = false;
            //    dataGridView1.ColumnHeadersHeight = 150;
            MaximizeBox = false;

            //изменение шрифта
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 16);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif",20, FontStyle.Bold); //заголовок
        }

        private void LoadForm()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();

            string query = @"SELECT     nazvanie           AS [Название], 
                                        gorod             AS [Город], 
                                        strana             AS [Страна]
                            FROM        pokupatel;";

            OleDbDataAdapter adapter = new OleDbDataAdapter(query, myConnection);
            db = new DataTable();
            adapter.Fill(db);
       //     bindingSource.DataSource = db;
            dataGridView1.DataSource = db;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newPok n = new newPok();
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
    }
}
