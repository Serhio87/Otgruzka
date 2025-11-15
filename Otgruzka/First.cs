using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class First : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private DataTable db;
        private string dol;
        private string FIO;
        private int tab_n;
        private string PozMenu;

        public First(string dolzhn, string fio, int tab_nomer)
        {
            InitializeComponent();
            Load_Ost();
            Runner();

            dol = dolzhn;
            FIO = fio;
            tab_n = tab_nomer;
            PozMenu = dol;
            dataGridView1.RowHeadersVisible = false;

            ConfigMenu();
        }

        private void Load_Ost()
        {
            using (OleDbConnection myConnection = new OleDbConnection(Metods.ConnectionString))
            {
                try
                {
                    string query = @"SELECT profil.profil        AS [ПРОФИЛЬ], 
                                            klass.marka          AS [КЛАСС СТАЛИ], 
                                            dlina.dlina          AS [ДЛИНА], 
                                            Sum(Izdelie.ves_izd) AS [ВЕС ПРОДУКЦИИ, Тонн]
                                       FROM profil INNER JOIN (klass INNER JOIN (dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) 
                                         ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
                                      WHERE Izdelie.date_prod Is Null
                                   GROUP BY profil.profil, klass.marka, dlina.dlina;";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, myConnection);
                    db = new DataTable();
                    adapter.Fill(db);
                    bindingSource.DataSource = db;
                    dataGridView1.DataSource = db;

                    //изменение шрифта
                    dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 14);
                    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 16, FontStyle.Bold); //заголовок

                    //округление в столбцах
                    dataGridView1.Columns["ВЕС ПРОДУКЦИИ, Тонн"].DefaultCellStyle.Format = "0.000";

                    //столбец не сортируется, ошибка
                    dataGridView1.Columns["ВЕС ПРОДУКЦИИ, Тонн"].SortMode = DataGridViewColumnSortMode.NotSortable;
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
                    this.Height = 700;
                    toolStripButton1.Visible = false;
                    toolStripButton2.Visible = false;
                    toolStripButton6.Visible = false;
                    toolStripButton7.Visible = false;
                    toolStripButton8.Visible = false;
                    toolStripButton9.Visible = false;
                    ПрикНаОтгрToolStripMenuItem.Visible = false;
                    break;

                case "Бригадир":
                    this.Text = $"{dol} - {FIO}";
                    this.Height = 700;
                    toolStripButton1.Visible = false;
                    toolStripButton2.Visible = false;
                    toolStripButton7.Visible = false;
                    toolStripButton8.Visible = false;
                    toolStripButton9.Visible = false;
                    break;

                case "Экономист":
                    this.Text = $"{dol} - {FIO}";
                    this.Height = 800;
                    toolStripButton1.Visible = false;
                    toolStripButton4.Visible = false;
                    toolStripButton6.Visible = false;
                    toolStripButton10.Visible = false;
                    break;

                case "Мастер":
                    this.Text = $"{dol} - {FIO}";
                    this.Height = 900;
                    toolStripButton7.Visible = false;
                    toolStripButton9.Visible = false;
                    break;

                case "ADMIN":
                    this.Text = $"{dol} - {FIO}";
                    break;
            }
        }

        private void First_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void UpFirst()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Font = default;
            Load_Ost();
            this.Activate();
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
            int labelWidth;
            // Устанавливаем текст для бегущей строки
            label1.Text = "Текущее наличие продукции на складе";
            label1.AutoSize = true;
            labelWidth = label1.Width;
            // Устанавливаем таймер
            Timer timer = new Timer
            {
                Interval = 30 // Интервал в миллисекундах
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Sotr s = new Sotr();
            s.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            pokupatel p = new pokupatel();
            p.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DobProd dp = new DobProd(tab_n);
            dp.UpFirst += UpFirst;
            dp.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Nalich F1 = new Nalich();
            F1.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            OtgrProd op = new OtgrProd(tab_n);
            op.UpFirst += UpFirst;
            op.Show();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            NewPrik np = new NewPrik(tab_n);
            np.Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Invent inv = new Invent();
            inv.Show();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            UpPrice up = new UpPrice();
            up.Show();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            TN2 tn2 = new TN2();
            tn2.Show();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpFirst();
        }

        private void изменитьШрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // Установка шрифта для ячеек
            dataGridView1.DefaultCellStyle.Font = fontDialog1.Font;
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
        
        private void приказыНаОтгрузкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewPrik pp = new ViewPrik();
            pp.Show();
        }

        private void изменитьЦенуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpPrice up = new UpPrice();
            up.Show();
        }

        private void печатьНакладнойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TN2 tn2 = new TN2();
            tn2.Show();
        }
    }
}
