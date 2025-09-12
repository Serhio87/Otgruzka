using Microsoft.Reporting.WinForms;
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

namespace Otgruzka
{
    public partial class ViewPrik: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        private DataTable db;
        private BindingSource bindingSource = new BindingSource();

        public ViewPrik()
        {
            InitializeComponent();
            LoadData();
            Runner();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Сдвигаем текст влево
            label1.Left -= 2;

            // Если текст вышел за пределы формы, перезапускаем его
            if (label1.Right < 0)
            {
                label1.Left = this.ClientSize.Width; // Перемещаем текст обратно вправо
            }
        }
        private void Runner()
        {
            // Устанавливаем текст для бегущей строки
        //    label1.Text = "Текущее наличие продукции на складе";
            label1.AutoSize = true;
            int labelWidth = label1.Width;
            // Устанавливаем таймер
            Timer timer = new Timer();
            timer.Interval = 30; // Интервал в миллисекундах
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void LoadData()
        {
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                try
                {
                    string query = @"SELECT Prikaz.id_prik AS [№ Приказа], 
pokupatel.nazvanie AS [Покупатель], 
pokupatel.gorod AS [Город], 
pokupatel.strana AS [Страна], 
Prikaz.prof AS [Профиль], 
Prikaz.dlina AS [Длина], 
Prikaz.klass AS [Класс стали], 
Prikaz.ves AS [Объем поставки], 
Prikaz.date_prik AS [Дата составления], 
dolzhnost.dolzhn AS [Должность], 
sotrudniki.fam AS [Фамилия], 
Prikaz.ispoln AS [Дата отгрузки], 
Prikaz.kolich AS [Кол-во отгружено]
FROM dolzhnost 
INNER JOIN (sotrudniki 
INNER JOIN (pokupatel 
INNER JOIN Prikaz 
ON pokupatel.Код = Prikaz.pokup) 
ON sotrudniki.tab_nomer = Prikaz.ekon) 
ON dolzhnost.Код = sotrudniki.dolzhn;";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    db = new DataTable();
                    adapter.Fill(db);
                    bindingSource.DataSource = db;
                    dataGridView1.DataSource = db;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            //установка для выделения всей строки
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //изменение шрифта
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 10);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 12, FontStyle.Bold); //заголовок

            dataGridView1.RowHeadersVisible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
            int id_prik = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["№ Приказа"].Value);

            PrintPrikaz pp = new PrintPrikaz(id_prik);
            pp.Show();
        }
    }
}
