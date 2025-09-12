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
    public partial class First: Form
    {
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        private OleDbConnection myConnection;
        private BindingSource bindingSource = new BindingSource();
        private DataTable db;
        private string dol;
        private string FIO;
        private int tab_n;
        private string PozMenu;
        private int labelWidth;

        public First(string dolzhn, string fio, int tab_nomer)
        {
            InitializeComponent();
            Load_Ost();
            Runner();

            //  MaximizeBox = false;
            dol = dolzhn;
            FIO = fio;
            tab_n = tab_nomer;
            PozMenu = dol;

            ConfigMenu();

            dataGridView1.RowHeadersVisible = false;


            //      dataGridView1.Visible = false;
        }

        private void Load_Ost()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
      //          myConnection.Open();

                try
                {
                    string query = @"SELECT profil.profil AS [ПРОФИЛЬ], 
                                            klass.marka AS [КЛАСС СТАЛИ], 
                                            dlina.dlina AS [ДЛИНА], 
                                            Sum(Izdelie.ves_izd) AS [ВЕС ПРОДУКЦИИ, Тонн]
                                            FROM profil 
                                            INNER JOIN (klass 
                                            INNER JOIN ((dlina 
                                            INNER JOIN Izdelie 
                                            ON dlina.Код = Izdelie.dlina) 
                                            LEFT JOIN Prodazha 
                                            ON Izdelie.kod_izd = Prodazha.kod_izd) 
                                            ON klass.Код = Izdelie.klass) 
                                            ON profil.Код = Izdelie.profil
                                            GROUP BY profil.profil, klass.marka, dlina.dlina, Prodazha.date_prod
                                            HAVING (((Prodazha.date_prod) Is Null));";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, myConnection);
                    db = new DataTable();
                    adapter.Fill(db);
                    bindingSource.DataSource = db;
                    dataGridView1.DataSource = db;

                    //изменение шрифта
                    dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 12);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold); //заголовок

                    //округление в столбцах
                    //    DataGridViewColumn price = dataGridView1.Columns["Цена"];
                    //    price.DefaultCellStyle.Format = "0.00";
                    DataGridViewColumn ves = dataGridView1.Columns["ВЕС ПРОДУКЦИИ, Тонн"];
                    ves.DefaultCellStyle.Format = "0.000";



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void ConfigMenu()
        {
            switch (PozMenu)
            {
                case "Кладовщик":
                    this.Text = $"{dol} - {FIO}";
                    break;

                case "Бригадир":
                    this.Text = $"{dol} - {FIO}";
                    break;

                case "Экономист":
                    this.Text = $"{dol} - {FIO}";
                    break;

                case "Мастер":  //добавить др эл-ты управления
                    this.Text = $"{dol} - {FIO}";
                    break;
            }
        }

        private void First_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Nalich F1 = new Nalich();
            F1.Show();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Font = default;
            Load_Ost();
        }

        private void изменитьШрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // Установка шрифта для ячеек
            dataGridView1.DefaultCellStyle.Font = fontDialog1.Font;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Repo2 r = new Repo2();
            r.Show();
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
            label1.Text = "Текущее наличие продукции на складе";
            label1.AutoSize = true;
            labelWidth = label1.Width;
            // Устанавливаем таймер
            Timer timer = new Timer();
            timer.Interval = 30; // Интервал в миллисекундах
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void сменитьПарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pass newForm1 = new pass(tab_n);
            newForm1.Show();
        }

        private void вToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            enterBox newForm = new enterBox();
            newForm.Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 newBox = new AboutBox1();
            newBox.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            pokupatel p = new pokupatel();
            p.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Sotr s = new Sotr();
            s.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DobProd dp = new DobProd();
            dp.Show();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            NewPrik np = new NewPrik(tab_n);
            np.Show();
        }

        private void приказыНаОтгрузкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewPrik pp = new ViewPrik();
            pp.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            OtgrProd op = new OtgrProd(tab_n);
            op.Show();
        }
    }
}
