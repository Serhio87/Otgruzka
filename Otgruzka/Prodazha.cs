using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class Prodazha: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private int selectedKod; //код покуп
        private int kod; //код прод
        private int tab_n;

        public Prodazha(int tab_nomer)
        {
            InitializeComponent();
            LoadPokup();
            LoadProf();
            tab_n = tab_nomer;
            FIO(tab_n);
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
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text; // Это то, что будет отображаться в ComboBox
            }
        }

        private void LoadPokup()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();
                string query = "SELECT Код, nazvanie, gorod, strana FROM pokupatel"; // Запрос на получение названий 
                using (OleDbCommand command = new OleDbCommand(query, myConnection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pokup pokup = new Pokup
                            {
                                Код = Convert.ToInt32(reader["Код"]),
                                Nazvanie = reader["nazvanie"].ToString(),
                                Gorod = reader["gorod"].ToString(),
                                Strana = reader["strana"].ToString()
                            };
                            comboBox1.Items.Add(pokup); // Добавляем названия в ComboBox
                        }
                    }
                }
            }
        }

        private void LoadProf()
        {
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();

                string query = "SELECT Код, profil FROM profil";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    OleDbDataReader reader = command.ExecuteReader();

                   while (reader.Read())
                   {
                        var item = new ComboBoxItem
                        {
                            Text = reader["profil"].ToString(),
                            Value = reader["Код"]
                        };
                        comboBoxProf.Items.Add(item);
                    }
                   reader.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pokup select = (Pokup)comboBox1.SelectedItem;
            if (select != null)
            { 
                selectedKod = select.Код;   
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProf.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedCode = selectedItem.Value; // Здесь хранится Код
                LoadDl(Convert.ToInt32(selectedCode)); // Загружаем длины на основе выбранного профиля
            }
            comboBoxCl.Items.Clear(); // Очистка классов
            comboBoxV.Items.Clear(); // Очистка веса
        }

        private void LoadDl(int selectedCode)
        {
            comboBoxDl.Items.Clear();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();
                string query = @"SELECT DISTINCT dlina.Код, dlina.dlina
                    FROM Izdelie
                    INNER JOIN dlina ON Izdelie.dlina = dlina.Код
                    WHERE Izdelie.profil = @selectedCode;";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@selectedCode", selectedCode);
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = new ComboBoxItem
                    {
                        Text = reader["dlina"].ToString(),
                        Value = reader["Код"]
                    };
                    comboBoxDl.Items.Add(item);
                }
                reader.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDl.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedDlCode = selectedItem.Value; // Код длины
                if (comboBoxProf.SelectedItem is ComboBoxItem profileItem)
                {
                    var selectedCode = profileItem.Value; // Код профиля
                    LoadClass(Convert.ToInt32(selectedDlCode), Convert.ToInt32(selectedCode)); // Загружаем классы
                }
            }
            comboBoxV.Items.Clear(); // Очистка веса
        }

        private void LoadClass(int selectedDlCode, int selectedCode)
        {
            comboBoxCl.Items.Clear();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();
                string query = @"SELECT DISTINCT klass.Код, klass.marka
                    FROM Izdelie INNER JOIN klass ON Izdelie.klass = klass.Код
                    WHERE Izdelie.dlina = @dlinaCode AND Izdelie.profil = @profilCode;";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@dlinaCode", selectedDlCode);
                command.Parameters.AddWithValue("@profilCode", selectedCode);
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var item = new ComboBoxItem
                    {
                        Text = reader["marka"].ToString(),
                        Value = reader["Код"]
                    };
                    comboBoxCl.Items.Add(item);
                }
                reader.Close();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCl.SelectedItem is ComboBoxItem selectedKlItem)
            {
                var klassCode = selectedKlItem.Value; // Код класса
                if (comboBoxDl.SelectedItem is ComboBoxItem selectedDlItem)
                {
                    var selectedDlCode = selectedDlItem.Value; // Код длины
                    if (comboBoxProf.SelectedItem is ComboBoxItem profileItem)
                    {
                        var selectedCode = profileItem.Value; // Код профиля
                        LoadVes(Convert.ToInt32(selectedDlCode), Convert.ToInt32(selectedCode), Convert.ToInt32(klassCode)); // Загружаем классы
                    }
                }
            }
        }

        private void LoadVes(int selectedDlCode, int selectedCode, int klassCode)
        {
            comboBoxV.Items.Clear();
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();

                string query = @"SELECT DISTINCT Izdelie.kod_izd, ves_izd 
                     FROM Izdelie 
                     LEFT JOIN Prodazha
                     ON Izdelie.kod_izd = Prodazha.kod_izd
                     WHERE klass = @klassCode 
                     AND dlina = @dlinaCode 
                     AND profil = @profilCode 
                     AND Prodazha.date_prod IS NULL;";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@klassCode", klassCode);
                    command.Parameters.AddWithValue("@dlinaCode", selectedDlCode);
                    command.Parameters.AddWithValue("@profilCode", selectedCode);

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Такой продукции нет.");
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    var item = new ComboBoxItem
                                    {
                                        Text = reader["ves_izd"].ToString(),
                                        Value = reader["kod_izd"]
                                    };
                                    comboBoxV.Items.Add(item);
                                }
                            }
                        }
                }
            }
        }

        private void comboBoxV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxV.SelectedItem is ComboBoxItem selectedItem)
            {
                var kod_izd = selectedItem.Value; // Здесь хранится Код
                KOD(Convert.ToInt32(kod_izd)); 
            }
        }

        private void KOD(int kod_izd)
        {
            kod = kod_izd;//получаем код изделия
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Prodazha p = new Prodazha(tab_n);
            p.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FIO(int tab_n)
        {
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                conn.Open();
                string query = @"SELECT tab_nomer, fam, im, otch 
                            FROM sotrudniki 
                            WHERE tab_nomer = @tab_n;";
                OleDbCommand command = new OleDbCommand(query, conn);
                command.Parameters.AddWithValue("@tab_n", tab_n);

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) //проверка есть ли результаты
                    {
                        string fam = reader["fam"].ToString();
                        string im = reader["im"].ToString();
                        string otch = reader["otch"].ToString();
                        textBox2.Text = $"{fam} {im} {otch}";
                    }
                    else { MessageBox.Show("ошибка"); }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    if (comboBox1.SelectedItem == null)
                    {
                         MessageBox.Show("Пожалуйста, выберите покупателя.");
                         return;
                    }
                    int kod_pokup = selectedKod;

                    if ((comboBoxProf.SelectedItem == null) || (comboBoxDl.SelectedItem ==null) || (comboBoxCl.SelectedItem ==null) || (comboBoxV.SelectedItem == null))
                    {
                         MessageBox.Show("Пожалуйста, заполните данные продукции.");
                         return;
                    }
                    int kod_izd = kod;

                    string date_prod = dateTimePicker1.Value.ToShortDateString();

                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("Пожалуйста, заполните поле транспорта.");
                        return;
                    }
                    string nomer_trst = textBox1.Text;

                    int kod_sotr = tab_n;

                    // Запрос INSERT
                    string query = @"INSERT INTO Prodazha (date_prod, kod_pokup, kod_izd, kod_sotr, nomer_trst)
                                                   VALUES (@date_prod, @kod_pokup, @kod_izd, @kod_sotr, @nomer_trst);";
                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@date_prod", date_prod);
                        command.Parameters.AddWithValue("@kod_pokup", kod_pokup);
                        command.Parameters.AddWithValue("@kod_izd", kod_izd);
                        command.Parameters.AddWithValue("@kod_sotr", kod_sotr);
                        command.Parameters.AddWithValue("@nomer_trst", nomer_trst);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Продано!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            this.Close();
        }
    }
}