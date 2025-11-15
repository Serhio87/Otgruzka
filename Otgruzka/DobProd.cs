using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class DobProd : Form
    {
        public event Action UpFirst;
        private int tab_n;
        private Metods metods;

        public DobProd(int tab_nomer)
        {
            tab_n = tab_nomer;

            InitializeComponent();
            Runner();
            LoadProd();

            metods = new Metods();
            FIO(tab_n);
        }

        private void FIO(int tab_n)
        {
            Tuple<string, string> result = metods.GetFIO(tab_n);

            if (result == null) // Проверяем на null (ошибку)
            {
                MessageBox.Show("Ошибка: нет данных или сотрудник не найден.");
            }
            else
            {
                // Вставляем только ФИО без должности
                textBox4.Text = result.Item1 + ", " + result.Item2;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Сдвигаем текст влево
            label2.Left -= 3;
            // Если текст вышел за пределы формы, перезапускаем его
            if (label2.Right < 0)
            {
                label2.Left = this.ClientSize.Width; // Перемещаем текст обратно вправо
            }
        }

        private void Runner()
        {
            int labelWidth;
            // Устанавливаем текст для бегущей строки
            label2.Text = "Если нужный выбор отсутствует - нажмите кнопку 'Добавить'";
            label2.AutoSize = true;
            labelWidth = label2.Width;
            // Устанавливаем таймер
            Timer timer = new Timer();
            timer.Interval = 65; // Интервал в миллисекундах
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void LoadProd()
        {
            string queryPr = "SELECT profil FROM profil";
            string queryKl = "SELECT marka FROM klass";
            string queryDl = "SELECT dlina FROM dlina";
            Metods.LoadComboBoxData(queryPr, comboBox1);
            Metods.LoadComboBoxData(queryDl, comboBox2);
            Metods.LoadComboBoxData(queryKl, comboBox3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dob dob1 = new dob(1);
            dob1.DataUpdated += DataUpdated; // Подписка на событие
            dob1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dob dob2 = new dob(2);
            dob2.DataUpdated += DataUpdated;
            dob2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dob dob3 = new dob(3);
            dob3.DataUpdated += DataUpdated;
            dob3.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpFirst?.Invoke();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DataUpdated();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(Metods.ConnectionString))
            {
                myConnection.Open();

                try
                {
                    if (comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите профиль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int profilCode = Metods.GetCode("SELECT Код FROM profil WHERE profil = ?", comboBox1);

                    if (comboBox2.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите длину.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int dlinaCode = Metods.GetCode("SELECT Код FROM dlina WHERE dlina = ?", comboBox2); ;

                    if (comboBox3.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите класс стали.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int klassCode = Metods.GetCode("SELECT Код FROM klass WHERE marka = ?", comboBox3);

                    int plavka;
                    if (string.IsNullOrWhiteSpace(textBox3.Text) || textBox3.Text.Length != 6 || !int.TryParse(textBox3.Text, out plavka))
                    {
                        MessageBox.Show("Введите корректный номер плавки. Шесть цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int paket;
                    if (string.IsNullOrWhiteSpace(textBox2.Text) || !int.TryParse(textBox2.Text, out paket))
                    {
                        MessageBox.Show("Введите корректный номер пакета.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    double ves_izd;
                    if (string.IsNullOrWhiteSpace(textBox1.Text) || !double.TryParse(textBox1.Text, out ves_izd))
                    {
                        MessageBox.Show("Введите корректное значение для веса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string date_post = dateTimePicker1.Value.ToShortDateString();

                    int dokum;
                    if (string.IsNullOrWhiteSpace(textBox_dokum.Text) || !int.TryParse(textBox_dokum.Text, out dokum))
                    {
                        MessageBox.Show("Введите номер накладной. Только цифры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int id_sotr = tab_n;

                    // Запрос INSERT
                    string query = @"INSERT INTO Izdelie (plavka, profil, dlina, klass, paket, ves_izd, date_post, id_sotr, dokum)
                                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?);";

                    Console.WriteLine($"plavka: {plavka}, profilCode: {profilCode}, dlinaCode: {dlinaCode}, klassCode: {klassCode}, paket: {paket}, ves_izd: {ves_izd}, date_post: {date_post}, id_sotr: {id_sotr}, dokum: {dokum}");

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("?", plavka);
                        command.Parameters.AddWithValue("?", profilCode);
                        command.Parameters.AddWithValue("?", dlinaCode);
                        command.Parameters.AddWithValue("?", klassCode);
                        command.Parameters.AddWithValue("?", paket);
                        command.Parameters.AddWithValue("?", ves_izd);
                        command.Parameters.AddWithValue("?", date_post);
                        command.Parameters.AddWithValue("?", id_sotr);
                        command.Parameters.AddWithValue("?", dokum);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Успешно", "Добавлено");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
            DialogResult result = MessageBox.Show(
            "Необходимо добавить еще пакет в данную плавку?",
            " ",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);


            if (result == DialogResult.Yes)
            {
                DobPak();
                textBox2.Focus();
                this.Activate();
            }
            else
            {
                UpFirst?.Invoke();
                this.Close();
            }
        }

        private void DataUpdated()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox_dokum.Clear();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";

            comboBox1.Enabled = true;
            button1.Enabled = true;
            comboBox2.Enabled = true;
            button2.Enabled = true;
            comboBox3.Enabled = true;
            button3.Enabled = true;
            textBox_dokum.Enabled = true;
            textBox3.Enabled = true;

            LoadProd();
        }

        private void DobPak()
        {
            comboBox1.Enabled = false;
            button1.Enabled = false;
            comboBox2.Enabled = false;
            button2.Enabled = false;
            comboBox3.Enabled = false;
            button3.Enabled = false;
            textBox_dokum.Enabled = false;
            textBox3.Enabled = false;
            textBox2.Clear();
            textBox1.Clear();
        }
    }
}
