using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class Plavka : Form
    {
        private DataTable dataTable;
        private int pl, pr, dl;
        private string kl;
        public event Action<List<int>> SelectedPak;
        private List<int> kodIzdList = new List<int>();

        public Plavka(int plavka, int profil, int dlina, string klass)
        {
            pl = plavka;
            pr = profil;
            dl = dlina;
            kl = klass;
            InitializeComponent();
            LoadForm();

            this.Text = $"Плавка {pl}";
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
        }

        private void LoadForm()
        {
            // Загрузка данных из БД в DataGridView
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                try
                {
                    string query = @"SELECT Izdelie.plavka  AS [№ плавки], 
                                            profil.profil   AS [Профиль], 
                                            klass.marka     AS [Класс стали], 
                                            dlina.dlina     AS [Длина], 
                                            Izdelie.paket   AS [№ пакета], 
                                            Izdelie.ves_izd AS [Вес], 
                                            Izdelie.kod_izd,  
                                            Izdelie.date_prod
                                       FROM profil INNER JOIN (klass INNER JOIN (dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) 
                                         ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
                                      WHERE Izdelie.plavka = ? AND profil.profil = ? AND dlina.dlina = ? AND klass.marka = ? AND Izdelie.date_prod Is Null AND Izdelie.marker = NO;";

                    OleDbCommand command = new OleDbCommand(query, conn);
                    command.Parameters.AddWithValue("?", pl);
                    command.Parameters.AddWithValue("?", pr);
                    command.Parameters.AddWithValue("?", dl);
                    command.Parameters.AddWithValue("?", kl);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine($"Ошибка выполнения запроса: {ex.Message}");
                    MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
                }
            }
            dataGridView1.RowHeadersVisible = false;

            // Скрываем столбцы по умолчанию
            dataGridView1.Columns["kod_izd"].Visible = false;
            dataGridView1.Columns["date_prod"].Visible = false;

            //изменение шрифта
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13); //заголовок
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Очищаем список перед добавлением новых значений
                kodIzdList.Clear();

                // Проходим по всем выбранным строкам
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // Проверяем, что значение не null и является целым числом
                    if (row.Cells["kod_izd"].Value != null && int.TryParse(row.Cells["kod_izd"].Value.ToString(), out int kodIzd))
                    {
                        // Добавляем kod_izd в список
                        kodIzdList.Add(kodIzd);
                        Console.WriteLine($"Выбрана строка: {kodIzd}");
                    }
                }
                // Вызываем событие только если список не пуст
                if (kodIzdList.Count > 0)
                {
                    SelectedPak?.Invoke(kodIzdList);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите необходимые пакеты.");
            }
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //кол-во и сумма
            decimal Ves = 0;
            int Count = 1;

            // Проходим по всем выбранным строкам
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                // Проверяем значение в столбце Вес
                if (row.Cells["Вес"].Value != null && decimal.TryParse(row.Cells["Вес"].Value.ToString(), out decimal weight))
                {
                    Ves += weight; // Добавляем вес к общей сумме
                    textBox2.Text = Convert.ToString(Ves);
                    int c = Count++; // Увеличиваем счетчик выбранных пакетов
                    textBox1.Text = Convert.ToString(c);
                }
            }
        }
    }
}
