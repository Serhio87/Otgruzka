using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class OtgrProd : Form
    {
        public event Action UpFirst;
        private int tab_n;
        private decimal SumPak = 0;
        private decimal kolich;
        int CountPak = 0;
        private List<int> kodBox; // Список для хранения кодов изделий
        private Metods metods;

        public OtgrProd(int tab_nomer)
        {
            InitializeComponent();
            tab_n = tab_nomer;
            kodBox = new List<int>(); // Инициализация списка
            metods = new Metods();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Enabled = false;
                using (OleDbConnection myConnection = new OleDbConnection(Metods.ConnectionString))
                {
                    if (!string.IsNullOrEmpty(textBox1.Text))
                    {
                        try
                        {
                            myConnection.Open();

                            string query = @"SELECT Prikaz.id_prik, pokupatel.nazvanie, pokupatel.strana, Prikaz.prof, Prikaz.dlina, Prikaz.klass, Prikaz.ves, Prikaz.kolich, Prikaz.ispoln
                                               FROM pokupatel INNER JOIN Prikaz ON pokupatel.Код = Prikaz.pokup
                                              WHERE Prikaz.id_prik= ? ;";

                            using (OleDbCommand command = new OleDbCommand(query, myConnection))
                            {
                                command.Parameters.AddWithValue("?", textBox1.Text);

                                // Создаем адаптер и заполняем DataTable
                                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                if (dataTable.Rows.Count == 0)
                                {
                                    MessageBox.Show("Приказ отсутствует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    textBox1.Enabled = true;
                                    textBox1.Focus();
                                    return;
                                }

                                DataRow row = dataTable.Rows[0];
                                int id_prik = Convert.ToInt32(row["id_prik"].ToString());
                                string pokup = row["nazvanie"].ToString();
                                string strana = row["strana"].ToString();
                                string prof = row["prof"].ToString();
                                string dlina = row["dlina"].ToString();
                                string klass = row["klass"].ToString();
                                int ves = Convert.ToInt32(row["ves"].ToString());
                                kolich = Convert.ToDecimal(row["kolich"].ToString());
                                string ispoln = row["ispoln"].ToString();

                                decimal Limit = (ves / 100) * 10 + ves; // 10% от значения
                                if (kolich < Limit)
                                {
                                    textBox2.Text = pokup;
                                    textBox3.Text = strana;
                                    textBox4.Text = prof;
                                    textBox5.Text = dlina;
                                    textBox6.Text = klass;
                                    textBox7.Text = Convert.ToString(ves);
                                    textBox13.Text = Convert.ToString(kolich);
                                }
                                else
                                {
                                    MessageBox.Show("Приказ отгружен" + ispoln + "!" + "\n" + "В объёме:" + kolich + " тонн.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    textBox1.Enabled = true;
                                    textBox1.Focus();
                                    return;
                                }
                            }
                            VidTr();
                            FIO(tab_n);
                            textBoxPlavka.Focus();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void VidTr()
        {
            Metods.LoadComboBoxData("SELECT Vid FROM VidTr", comboBoxVidTr);
            tbNomTrSr.Enabled = true;
            comboBoxVidTr.Enabled = true;
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
                textBox9.Text = result.Item1 + ", " + result.Item2;
            }
        }

        private void Plavka_SelectedPak(List<int> kodIzdList)
        {
            // Добавляем заголовки таблицы
            string header = $"{"Плавка",-10} {"Пакет",-10} {"Профиль",-10} {"Длина",-10} {"Марка",-34} {"Вес"}";
            textBox10.AppendText(header + Environment.NewLine);
            textBox10.AppendText(new string('*', header.Length) + Environment.NewLine); // Разделитель

            // Получаем значение из textBox7 и вычисляем 10%
            decimal limit = 0;
            if (!decimal.TryParse(textBox7.Text, out limit))
            {
                MessageBox.Show("Ошибка: неверное значение или отсутствует объем поставки.");
                textBox10.Clear();
                return;
            }

            decimal LimitKol = limit * 0.1m; // 10% от значения

            // Здесь вы получите список kodIzdList
            foreach (var kod_izd in kodIzdList)
            {
                Console.WriteLine($"Получен код изделия: {kod_izd}");

                using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT Izdelie.plavka, Izdelie.kod_izd, Izdelie.paket, Izdelie.ves_izd, profil.profil, dlina.dlina, klass.marka
                                       FROM profil INNER JOIN (klass INNER JOIN (dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) 
                                         ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
                                      WHERE Izdelie.kod_izd = ? ;";

                    using (OleDbCommand command = new OleDbCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("?", kod_izd);

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) //проверка есть ли результаты
                            {
                                string pl = reader["plavka"].ToString();
                                string pak = reader["paket"].ToString();
                                string prof = reader["profil"].ToString();
                                string dl = reader["dlina"].ToString();
                                string kl = reader["marka"].ToString();
                                decimal ves = Convert.ToDecimal(reader["ves_izd"]);

                                // Проверка на превышение 10%
                                if (SumPak + ves > limit + LimitKol)
                                {
                                    MessageBox.Show($"Ошибка: добавление данного пакета превышает допустимый лимит на 10%. " +
                                        $"Текущий вес: {SumPak}, добавляемый вес: {ves}, лимит: {limit + LimitKol}.");
                                    continue; // Пропускаем этот пакет
                                }

                                CountPak++;
                                textBox11.Text = $"{CountPak}";

                                SumPak += ves;
                                textBox8.Text = $"{SumPak}";

                                textBox14.Text = $"{kolich + SumPak}";

                                // Формируем строку с информацией о пакете
                                string info = $"{pl,-10} {pak,-14} {prof,-16} {dl,-10} {kl,-25} {ves,-40}";

                                // Добавляем информацию в TextBox
                                textBox10.AppendText(info);
                                textBox10.AppendText(new string('-', header.Length) + Environment.NewLine); // Разделитель

                                int kod = Convert.ToInt32(reader["kod_izd"]);
                                kodBox.Add(kod);
                                Console.WriteLine($"Записан код изделия: {kod}");

                                string queryUp = @"UPDATE Izdelie SET Izdelie.marker = Yes WHERE Izdelie.kod_izd= ?;";

                                using (OleDbCommand commUp = new OleDbCommand(queryUp, conn))
                                {
                                    commUp.Parameters.AddWithValue("?", kod);
                                    commUp.ExecuteNonQuery(); // Выполнение обновления
                                }
                            }
                            else { MessageBox.Show("Ошибка: информация не найдена для код_изд " + kod_izd); }
                        }
                    }
                }
            }
        }

        private void textBoxPlavka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Проверка на целое число
                if (!int.TryParse(textBoxPlavka.Text, out int plavka))
                {
                    MessageBox.Show("Введите корректный № плавки!", "ОШИБКА!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxPlavka.Clear();
                    return; // Выход из метода, если ввод некорректный
                }

                if (textBoxPlavka.Text.Length != 6)
                {
                    MessageBox.Show("Введите корректный № плавки из 6 чисел!", "ОШИБКА!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxPlavka.Clear();
                    return;
                }

                using (OleDbConnection myConnection = new OleDbConnection(Metods.ConnectionString))
                {
                    string query = @"SELECT Izdelie.plavka, klass.marka, dlina.dlina, profil.profil, Izdelie.date_prod
                                       FROM profil INNER JOIN (klass INNER JOIN (dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) 
                                         ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
                                      WHERE Izdelie.plavka = ? AND Izdelie.date_prod Is Null ;";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        command.Parameters.AddWithValue("?", plavka);

                        // Создаем адаптер и заполняем DataTable
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Проверка на наличие плавки в базе данных
                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Плавка не существует!", "ОШИБКА!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxPlavka.Clear();
                            return; // Выход из метода, если плавка не найдена
                        }

                        DataRow row = dataTable.Rows[0];
                        int plavkaDB = Convert.ToInt32(row["plavka"]);
                        int profilDB = Convert.ToInt32(row["profil"]);
                        int dlinaDB = Convert.ToInt32(row["dlina"]);
                        string klassDB = row["marka"].ToString();
                        int profil = Convert.ToInt32(textBox4.Text);
                        int dlina = Convert.ToInt32(textBox5.Text);
                        string klass = Convert.ToString(textBox6.Text);

                        if (profil != profilDB)
                        {
                            MessageBox.Show("Профиль данной плавки не соответствует приказу!", "ОШИБКА!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxPlavka.Clear();
                        }
                        else if (dlina != dlinaDB)
                        {
                            MessageBox.Show("Длина порезки данной плавки не соответствует приказу!", "ОШИБКА!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxPlavka.Clear();
                        }
                        else if (klass != klassDB)
                        {
                            MessageBox.Show("Класс стали данной плавки не соответствует приказу!", "ОШИБКА!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBoxPlavka.Clear();
                        }
                        else
                        {
                            Plavka pl = new Plavka(plavka, profil, dlina, klass);
                            // Подписываемся на событие
                            pl.SelectedPak += Plavka_SelectedPak;
                            pl.Show();
                            textBoxPlavka.Clear();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearData();
            MessageBox.Show("Форма очищена.");
        }

        private void ClearData()
        {
            // Снять маркеры с кодов изделий в базе данных
            RemoveMarkers(kodBox);

            textBox1.Enabled = true;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBoxPlavka.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox13.Clear();
            textBox14.Clear();

            comboBoxVidTr.Items.Clear();
            tbNomTrSr.Clear();

            SumPak = 0;

            tbNomTrSr.Enabled = false;
            comboBoxVidTr.Enabled = false;
        }

        private void RemoveMarkers(List<int> kodBox)
        {
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                conn.Open();
                foreach (var kod in kodBox)
                {
                    string queryDown = @"UPDATE Izdelie SET Izdelie.marker = No WHERE Izdelie.kod_izd = ?;";
                    using (OleDbCommand commDown = new OleDbCommand(queryDown, conn))
                    {
                        commDown.Parameters.AddWithValue("?", kod);
                        commDown.ExecuteNonQuery();
                    }
                }
            }
        }

        private void Prodazha(List<int> kodBox)
        {
            double price_pog;
            double dlina = Convert.ToDouble(textBox5.Text);

            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                conn.Open();

                string queryPog = @"SELECT profil.profil, profil.price_pog_m FROM profil WHERE profil.profil = ?;";
                using (OleDbCommand commandP = new OleDbCommand(queryPog, conn))
                {
                    commandP.Parameters.AddWithValue("?", textBox4.Text);

                    OleDbDataAdapter adapter = new OleDbDataAdapter(commandP);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Если данные найдены, извлекаем информацию
                    DataRow row = dataTable.Rows[0];
                    int profil = Convert.ToInt32(row[("profil")]);
                    price_pog = Convert.ToDouble(row["price_pog_m"].ToString());
                    Console.WriteLine($"Профиль: {profil}, Погонка: {price_pog}");
                }

                try
                {
                    int tab = tab_n;
                    string nom_tr = tbNomTrSr.Text;
                    int kod_vid = Metods.GetCode(@"SELECT Код FROM VidTr WHERE Vid = ?", comboBoxVidTr);
                    int id_prik = Convert.ToInt32(textBox1.Text);
                    string date_prod = dateTimePicker1.Value.ToShortDateString();
                    double ves_all = Convert.ToDouble(textBox14.Text);
                    double price_all = Math.Round(price_pog * dlina * ves_all / 1000, 2);

                    foreach (var kod in kodBox)
                    {
                        string queryProdazha = @"INSERT INTO Prodazha (kod_izd, kod_sotr, nomer_trst, kod_vidTr, id_prik) VALUES (?, ?, ?, ?, ?);";
                        string queryIzdelie = "UPDATE Izdelie SET date_prod = ? WHERE kod_izd = ?";

                        using (OleDbCommand commandIzd = new OleDbCommand(queryIzdelie, conn))
                        {
                            commandIzd.Parameters.AddWithValue("?", date_prod);
                            commandIzd.Parameters.AddWithValue("?", kod);
                            commandIzd.ExecuteNonQuery();
                            Console.WriteLine($"Дата: {date_prod}, Код изделия: {kod}");
                        }

                        using (OleDbCommand commandProd = new OleDbCommand(queryProdazha, conn))
                        {
                            // Добавление параметров
                            commandProd.Parameters.AddWithValue("?", kod);
                            commandProd.Parameters.AddWithValue("?", tab);
                            commandProd.Parameters.AddWithValue("?", nom_tr);
                            commandProd.Parameters.AddWithValue("?", kod_vid);
                            commandProd.Parameters.AddWithValue("?", id_prik);
                            commandProd.ExecuteNonQuery();
                            Console.WriteLine($"Код изделия: {kod}, Код сотрудника: {tab}, Номер: {nom_tr}, Код вида транспорта: {kod_vid}, ID приказа: {id_prik}");
                        }
                    }

                    string queryPrikaz = @"UPDATE Prikaz SET ispoln = ?, kolich = ?, price_all = ? WHERE id_prik = ?;";

                    using (OleDbCommand commandPrik = new OleDbCommand(queryPrikaz, conn))
                    {
                        commandPrik.Parameters.AddWithValue("?", date_prod);
                        commandPrik.Parameters.AddWithValue("?", ves_all);
                        commandPrik.Parameters.AddWithValue("?", price_all);
                        commandPrik.Parameters.AddWithValue("?", id_prik);
                        commandPrik.ExecuteNonQuery();
                        Console.WriteLine($"ID приказа: {id_prik}, Дата исполнения: {date_prod}, Количество: {ves_all}, Общая цена: {price_all}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxVidTr.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите вид транспорта.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbNomTrSr.Text))
            {
                MessageBox.Show("Введите номер транспорта перевозки.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Prodazha(kodBox);
            ClearData();
            UpFirst?.Invoke();
            this.Close();
            MessageBox.Show("Продукция успешно отгружена.", "ОТГРУЖЕНО!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearData();
            UpFirst?.Invoke();
            this.Close();
        }
    }
}
