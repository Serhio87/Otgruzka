using System;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Otgruzka
{
    public class Metods
    {
        public static string ConnectionString { get; set; }
        public static string DirectoryPhoto { get; set; }
        public static string InventPrikFile { get; set; }

        //строка подключения к БД и указание расположений
        static Metods()
        {
            ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
            //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD2.accdb";
            DirectoryPhoto = @"C:\Users\user\Desktop\ДИПЛОМ\Otgruzka\Sotr"; //папка для фото сотрудников
            InventPrikFile = @"C:\Users\user\Desktop\ДИПЛОМ\Otgruzka\Otgruzka\bin\Debug\Resources\Prikaz_o_provedenii_inventarizacii_aktivov_i_obyazatelstv.doc"; //размещение приказа
        }

        // Метод для хеширования пароля с использованием PBKDF2
        public string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Объединяем соль и хеш для хранения
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        // Метод для проверки пароля
        public bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        //Метод для получения должности и ФИО пользователя
        public Tuple<string, string> GetFIO(int tab_n)
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT sotrudniki.fam, sotrudniki.im, sotrudniki.otch, dolzhnost.dolzhn, sotrudniki.tab_nomer
                                   FROM dolzhnost 
                             INNER JOIN sotrudniki ON dolzhnost.Код = sotrudniki.dolzhn
                                  WHERE sotrudniki.tab_nomer = ? ;";

                OleDbCommand command = new OleDbCommand(query, conn);
                command.Parameters.AddWithValue("?", tab_n);

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // проверка есть ли результаты
                    {
                        string dol = reader["dolzhn"].ToString();
                        string fam = reader["fam"].ToString();
                        string im = reader["im"].ToString();
                        string otch = reader["otch"].ToString();

                        // Возвращаем объект Tuple с должностью и полной строкой ФИО
                        return new Tuple<string, string>(dol, $"{fam} {im} {otch}");
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        //заполнение комбобокса в соответствии с классом, определенным в форме
        public static void LoadCombo<T>(string query, ComboBox comboBox, Func<OleDbDataReader, T> createItem)
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();

                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T item = createItem(reader); // Создаем объект типа T
                            comboBox.Items.Add(item); // Добавляем объект в ComboBox
                        }
                    }
                }
            }
        }

        //получение кода из наименования
        public static int GetCode(string query, ComboBox comboBox)
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();

                string name = comboBox.SelectedItem.ToString();
                int kod;
                using (OleDbCommand com = new OleDbCommand(query, conn))
                {
                    com.Parameters.AddWithValue("?", name);
                    kod = (int)com.ExecuteScalar(); // Получаем код	
                }
                return kod;
            }
        }

        //загрузка ComboBox с помощью запроса
        public static void LoadComboBoxData(string query, ComboBox comboBox)
        {
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                conn.Open();
                using (OleDbCommand command = new OleDbCommand(query, conn))
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader[0].ToString()); // Добавляем 1е поле результата в ComboBox
                    }
                }
            }
        }
    }
}
