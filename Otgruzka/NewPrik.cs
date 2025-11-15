using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class NewPrik : Form
    {
        private int tab_n;
        private Metods metods = new Metods();


        public NewPrik(int tab_nomer)
        {
            InitializeComponent();
            LoadPokup();
            LoadProd();
            LoadPost();

            tab_n = tab_nomer;
            FIO(tab_n);

            MaximizeBox = false;
        }

        private class Pokup
        {
            public int Код { get; set; }
            public string Nazvanie { get; set; }
            public string Gorod { get; set; }
            public string Strana { get; set; }

            public override string ToString()
            {
                return $"{Nazvanie,-10} | {Gorod,-10} -- {Strana,-10}"; // -20 и -15 для выравнивания влево
            }
        }

        private class Post
        {
            public string post { get; set; }
            public string usl_post { get; set; }

            public override string ToString()
            {
                return $"{post,-5} - {usl_post,-5} ";
            }
        }

        private void LoadPokup()
        {
            string query = "SELECT Код, nazvanie, gorod, strana FROM pokupatel";
            Metods.LoadCombo(query, comboBox1, reader =>
            new Pokup
            {
                Код = Convert.ToInt32(reader["Код"]),
                Nazvanie = reader["nazvanie"].ToString(),
                Gorod = reader["gorod"].ToString(),
                Strana = reader["strana"].ToString()
            });
        }

        private void LoadPost()
        {
            string query = "SELECT post, usl_post FROM postavka";
            Metods.LoadCombo(query, comboBox2, reader =>
            new Post
            {
                post = reader["post"].ToString(),
                usl_post = reader["usl_post"].ToString()
            });
        }

        private void LoadProd()
        {
            string queryPr = "SELECT profil FROM profil";
            string queryDl = "SELECT dlina FROM dlina";
            string queryKl = "SELECT marka FROM klass";

            Metods.LoadComboBoxData(queryPr, comboBoxProf);
            Metods.LoadComboBoxData(queryDl, comboBoxDl);
            Metods.LoadComboBoxData(queryKl, comboBoxCl);
        }

        private void FIO(int tab_n)
        {
            // Вызываем новый метод, который возвращает кортеж (Tuple)
            Tuple<string, string> result = metods.GetFIO(tab_n);
            if (result == null) // Проверяем на null (ошибку)
            {
                MessageBox.Show("Ошибка: нет данных или сотрудник не найден.");
            }
            else
            {
                textBox2.Text = result.Item2; // Item2 содержит "Фамилия Имя Отчество"
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "";
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            comboBoxProf.Items.Clear();
            comboBoxProf.Text = "";
            comboBoxDl.Items.Clear();
            comboBoxDl.Text = "";
            comboBoxCl.Items.Clear();
            comboBoxCl.Text = "";

            textBoxVes.Clear();

            LoadPokup();
            LoadProd();
            LoadPost();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(Metods.ConnectionString))
            {
                myConnection.Open();

                try
                {
                    if (comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите покупателя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Pokup selectedPokup = (Pokup)comboBox1.SelectedItem;
                    int pokup = selectedPokup.Код;

                    if (comboBox2.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите условие поставки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Post selectedPost = (Post)comboBox2.SelectedItem;
                    string post = selectedPost.post; 

                    if (comboBoxProf.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, заполните данные продукции.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int prof = Convert.ToInt32(comboBoxProf.SelectedItem);

                    if (comboBoxDl.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, заполните данные продукции.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int dlina = Convert.ToInt32(comboBoxDl.SelectedItem);

                    if (comboBoxCl.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, заполните данные продукции.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string klass = comboBoxCl.SelectedItem.ToString();

                    string date_prik = dateTimePicker1.Value.ToShortDateString();

                    int ekon = tab_n;

                    int ves;
                    if (string.IsNullOrWhiteSpace(textBoxVes.Text) || !int.TryParse(textBoxVes.Text, out ves))
                    {
                        MessageBox.Show("Введите требуемый объем поставки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Console.WriteLine($"pokup: {pokup}, prof: {prof}, dlina: {dlina}, klass: {klass}, ves: {ves}, date_prik: {date_prik}, ekon: {ekon}, post: {post}");

                    // Запрос INSERT
                    string query = @"INSERT INTO Prikaz (pokup, prof, dlina, klass, ves, date_prik, ekon, usl_post)
                                     VALUES (?, ?, ?, ?, ?, ?, ?, ?);";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("?", pokup);
                        command.Parameters.AddWithValue("?", prof);
                        command.Parameters.AddWithValue("?", dlina);
                        command.Parameters.AddWithValue("?", klass);
                        command.Parameters.AddWithValue("?", ves);
                        command.Parameters.AddWithValue("?", date_prik);
                        command.Parameters.AddWithValue("?", ekon);
                        command.Parameters.AddWithValue("?", post);

                        command.ExecuteNonQuery();

                        // Получение последнего вставленного идентификатора
                        command.CommandText = "SELECT @@IDENTITY";
                        int Id = Convert.ToInt32(command.ExecuteScalar());
                        MessageBox.Show($"Приказ создан! Номер приказа: {Id}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            UpdateData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewPrik pp = new ViewPrik();
            pp.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            newPok n = new newPok();
            n.DataUpdated += DataUpdated; // Подписка на событие
            n.Show();
        }

        private void DataUpdated()
        {
            comboBox1.Items.Clear();
            LoadPokup();
        }
    }
}