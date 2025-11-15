using System;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class NewSotr : Form
    {
        private Metods hashPass = new Metods();
        // Определяем событие
        public event Action DataUpdated;
        private string queryDol = "SELECT dolzhn FROM dolzhnost WHERE dolzhn Not In (\"ADMIN\");";

        public NewSotr()
        {
            InitializeComponent();
            Metods.LoadComboBoxData(queryDol, comboBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                conn.Open();

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
                    int dolzhnCode = Metods.GetCode("SELECT Код FROM dolzhnost WHERE dolzhn = ? ", comboBox1);

                    string password = hashPass.HashPassword(textBox6.Text);

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
                    string directory = Metods.DirectoryPhoto;
                    string FilePath = Path.Combine(directory, $"{tab_nomer}-{fam}{extension}"); //табельный номер и фамилия как имя файла

                    // Если файл уже существует, можно переименовать его или перезаписать
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath); // Удаляем существующий файл
                    }
                    File.Copy(imagePath, FilePath); // Копируем файл

                    // Запрос INSERT
                    string query = @"INSERT INTO sotrudniki (tab_nomer, fam, im, otch, dolzhn, [password], photo)
                                     VALUES (?, ?, ?, ?, ?, ?, ?);";

                    using (OleDbCommand command = new OleDbCommand(query, conn))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("?", tab_nomer);
                        command.Parameters.AddWithValue("?", fam);
                        command.Parameters.AddWithValue("?", im);
                        command.Parameters.AddWithValue("?", otch);
                        command.Parameters.AddWithValue("?", dolzhnCode); // Используем код должности
                        command.Parameters.AddWithValue("?", password);
                        command.Parameters.AddWithValue("?", FilePath); // Сохраняем путь к изображению

                        command.ExecuteNonQuery();
                        MessageBox.Show("Ура! У нас новый сотрудник!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Данный табельный номер уже используется!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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