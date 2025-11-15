using System;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class logo : Form
    {
        private string dol;
        private string FIO;
        private int tab_n;
        private int progressValue = 0;

        public logo(string dolzhn, string fio, int tab_nomer)
        {
            InitializeComponent();

            dol = dolzhn;
            FIO = fio;
            tab_n = tab_nomer;
        }

        private void logo_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 250; // Максимальное значение (продолжительность заставки)
            progressBar1.Value = 50; // Начальное значение прогресс-бара
            timer1.Interval = 10; //чем меньше - тем выше скорость
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressValue < progressBar1.Maximum)
            {
                progressValue++;
                progressBar1.Value = progressValue;
            }
            else
            {
                timer1.Stop(); // Останавливаем таймер
                First f = new First(dol, FIO, tab_n);
                f.Show();
                this.Close(); // Закрываем текущую форму
            }
        }
    }
}
