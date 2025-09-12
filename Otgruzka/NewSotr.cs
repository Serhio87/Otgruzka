using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class NewSotr : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        // Определяем событие
        public event Action DataUpdated;

        public NewSotr()
        {
            InitializeComponent();
            LoadDolzhnost();
        }

        private void LoadDolzhnost()
        {
            OleDbConnection myConnection = new OleDbConnection(connectString);
                myConnection.Open();
                string query = "SELECT dolzhn FROM dolzhnost"; // Запрос на получение названий должностей
                OleDbCommand command = new OleDbCommand(query, myConnection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["dolzhn"].ToString()); // Добавляем названия в ComboBox
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                myConnection.Open();

                try
                {
                    int tab_nomer = Convert.ToInt32(textBox1.Text);
                    string fam = textBox2.Text;
                    string im = textBox3.Text;
                    string otch = textBox4.Text;

                    if (comboBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Пожалуйста, выберите должность.");
                        return;
                    }
                    string dolzhn = comboBox1.SelectedItem.ToString();

                    int password = Convert.ToInt32(textBox6.Text);

                    // Запрос для получения кода должности
                    string queryDolzhn = @"SELECT Код FROM dolzhnost WHERE dolzhn = @dolzhn";
                    int dolzhnCode;

                    using (OleDbCommand CommDolzhn = new OleDbCommand(queryDolzhn, myConnection))
                    {
                        CommDolzhn.Parameters.AddWithValue("@dolzhn", dolzhn);
                        dolzhnCode = (int)CommDolzhn.ExecuteScalar(); // Получаем код должности
                    }

                    // Получаем путь к изображению
                    string imagePath = textBox_photo.Text;
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        MessageBox.Show("Пожалуйста, загрузите изображение.");
                        return;
                    }

                    // Получаем расширение файла
                    string extension = Path.GetExtension(imagePath);

                    // Копируем изображение в целевую директорию
                    string directory = @"C:\Users\user\Desktop\ДИПЛОМ\Otgruzka\Sotr";
                    string FilePath = Path.Combine(directory, $"{tab_nomer}{extension}"); //табельный номер как имя файла

                    // Если файл уже существует, можно переименовать его или перезаписать
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath); // Удаляем существующий файл
                    }
                    File.Copy(imagePath, FilePath); // Копируем файл

                    // Запрос INSERT
                    string query = @"INSERT INTO sotrudniki (tab_nomer, fam, im, otch, dolzhn, [password], photo)
                         VALUES (@tab_nomer, @fam, @im, @otch, @dolzhnCode, @password, @photo);";

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@tab_nomer", tab_nomer);
                        command.Parameters.AddWithValue("@fam", fam);
                        command.Parameters.AddWithValue("@im", im);
                        command.Parameters.AddWithValue("@otch", otch);
                        command.Parameters.AddWithValue("@dolzhnCode", dolzhnCode); // Используем код должности
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@photo", FilePath); // Сохраняем путь к изображению

                        command.ExecuteNonQuery();
                        MessageBox.Show("Ура! У нас новый сотрудник!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            // Вызываем событие перед закрытием формы
            DataUpdated?.Invoke();

            this.Close();
        }

        private void button_photo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.jfif"; // Фильтр для выбора только изображений
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Отображаем выбранное изображение в PictureBox
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    // Сохраняем путь к изображению в текстовое поле
                    textBox_photo.Text = openFileDialog.FileName;
                }
            }
        }
    }
}