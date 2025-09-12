using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Otgruzka
{
    public partial class Sotr : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private OleDbConnection myConnection;
        private BindingSource bindingSource = new BindingSource();
        private DataTable db;
        private List<string> fams; // Для хранения фамилий

        public Sotr()
        {
            InitializeComponent();
     //       myConnection = new OleDbConnection(connectString);
     //       myConnection.Open();
            LoadData();
            InitFams(); // Инициализируем список фамилий
  //          this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);

        }

        private void LoadData()
        {
            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                string query = @"SELECT sotrudniki.tab_nomer AS [Табельный номер],
                                        sotrudniki.fam AS [Фамилия],
                                        sotrudniki.im AS [Имя],
                                        sotrudniki.otch AS [Отчество],
                                        dolzhnost.dolzhn AS [Должность], 
                                        sotrudniki.password AS [Пароль]
                            FROM        dolzhnost 
                            INNER JOIN  sotrudniki 
                            ON          dolzhnost.Код = sotrudniki.dolzhn
                            ORDER BY    sotrudniki.fam;";

                OleDbDataAdapter adapter = new OleDbDataAdapter(query, myConnection);
                db = new DataTable();
                adapter.Fill(db);

                bindingSource.DataSource = db;
                dataGridView1.DataSource = db;

                //изменение шрифта
                dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 14);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Corbel", 16, FontStyle.Bold); //заголовок

                //установка для выделения всей строки
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                //текст по центру в ячейке
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //в заголовке

                // Пример настройки отступов для самого DataGridView
             //   dataGridView1.Padding = new Padding(0, 50, 0, 100); // 10px слева/справа, 5px сверху/снизу

                dataGridView1.RowHeadersVisible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewSotr newS = new NewSotr();
            newS.DataUpdated += DataUpdated;
            newS.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем tab_nomer
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int tab_nomer = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["Табельный номер"].Value);

                // Запрашиваем подтверждение у пользователя
                DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // Удаляем запись из базы данных
                    DeleteRecord(tab_nomer);

                    // Удаляем строку из DataGridView
                    dataGridView1.Rows.RemoveAt(selectedRowIndex);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
            }
        }

        private void DeleteRecord(int tab_nomer)
        {
            using (OleDbConnection connection = new OleDbConnection(connectString))
            {
                connection.Open();

                using (OleDbCommand command = new OleDbCommand("DELETE FROM sotrudniki WHERE tab_nomer = @tab_nomer", connection))
                {
                    command.Parameters.AddWithValue("@tab_nomer", tab_nomer);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Data(string columnName, System.Windows.Forms.ComboBox comboBox)
        {
            var uniqueValues = db.AsEnumerable()
                         .Select(row => row.Field<object>(columnName)?.ToString())
                         .Where(value => value != null)
                         .Distinct()
                         .OrderBy(value => value)
                         .ToList();

            comboBox.Items.Clear();
            comboBox.Items.Add(""); // Добавляем опцию " "

            foreach (var value in uniqueValues)
            {
                comboBox.Items.Add(value);
            }

            comboBox.SelectedIndexChanged += FilterDGV; // Подписываемся на событие изменения выбора
        }

        private void FilterDGV(object sender, EventArgs e)
        {
            string filter = "";
            // Проверяем каждое значение комбобокса и формируем строку фильтра
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все")
                filter += $"[Должность] = '{comboBox1.SelectedItem}'";

            // Убираем последний AND, если он есть
            if (filter.EndsWith(" AND "))
                filter = filter.Substring(0, filter.Length - 5);

            // Применяем фильтр к DataGridView
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        private void DataUpdated()
        {
            comboBox1.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            listBox1.Visible = false;
            button4.Visible = false;
            dataGridView1.DefaultCellStyle = null;
            pictureBox1.Image = null;
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем tab_nomer
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int tab_nomer = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["Табельный номер"].Value);

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.jfif"; // Фильтр для выбора только изображений
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //путь к файлу
                        string imagePath = openFileDialog.FileName;
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

                        using (OleDbConnection myConnection = new OleDbConnection(connectString))
                        {
                            myConnection.Open();

                            try
                            {
                                // Обновление фото
                                string query = @"UPDATE sotrudniki SET [photo] = ? WHERE tab_nomer = ?";

                                using (OleDbCommand command = new OleDbCommand(query, myConnection))
                                {
                                    command.Parameters.AddWithValue("?", FilePath); // Новое фото
                                    command.Parameters.AddWithValue("?", tab_nomer);  // Табельный номер
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Фото изменено!");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка: " + ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private void Sotr_FormClosing(object sender, FormClosingEventArgs e)
        {
    //        myConnection.Close();
        }

        private string SearchOption = "";

        private void button4_Click(object sender, EventArgs e)
        {
            if (SearchOption == "Tab")
            {
                SearchTab();
                textBox2.Clear();
            }
            else if (SearchOption == "Fam")
            {
                SearchFam();
                textBox1.Clear();
            }
        }
        
        private void SearchTab()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
            if (int.TryParse(textBox2.Text, out int tab))
            {
                string query = @"SELECT     sotrudniki.tab_nomer AS [Табельный номер],
                                        sotrudniki.fam AS [Фамилия],
                                        sotrudniki.im AS [Имя],
                                        sotrudniki.otch AS [Отчество],
                                        dolzhnost.dolzhn AS [Должность], 
                                        sotrudniki.password AS [Пароль]
                            FROM        dolzhnost 
                            INNER JOIN  sotrudniki 
                            ON          dolzhnost.Код = sotrudniki.dolzhn
                            WHERE tab_nomer LIKE " + "'%" + tab + "%';";
                //       string query = "SELECT * FROM sotrudniki WHERE fam LIKE " + "'%" + tab + "%'";
                OleDbDataAdapter command = new OleDbDataAdapter(query, myConnection);
                DataTable db = new DataTable();
                command.Fill(db);
                dataGridView1.DataSource = db;
            }
            else
            {
                MessageBox.Show("Табельный номер - это число!");
            }
            myConnection.Close();
        }

        private void SearchFam()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();

            string fam = textBox1.Text;
            string query = @"SELECT     sotrudniki.tab_nomer AS [Табельный номер],
                                        sotrudniki.fam AS [Фамилия],
                                        sotrudniki.im AS [Имя],
                                        sotrudniki.otch AS [Отчество],
                                        dolzhnost.dolzhn AS [Должность], 
                                        sotrudniki.password AS [Пароль]
                            FROM        dolzhnost 
                            INNER JOIN  sotrudniki 
                            ON          dolzhnost.Код = sotrudniki.dolzhn
                            WHERE fam LIKE " + "'%" + fam + "%';";
            OleDbDataAdapter command = new OleDbDataAdapter(query, myConnection);
            DataTable db = new DataTable();
            command.Fill(db);
            dataGridView1.DataSource = db;
            
            myConnection.Close();
        }

        private void InitFams()
        {
            fams = db.AsEnumerable()
                     .Select(row => row.Field<string>("Фамилия"))
                     .Distinct()
                     .ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                listBox1.Visible = false; // Скрываем список, если текст пустой
                return;
            }

            var filterFam = fams.Where(s => s.ToLower().Contains(searchText)).ToList();

            if (filterFam.Count > 0)
            {
                listBox1.DataSource = filterFam;
                listBox1.Visible = true; // Показываем список совпадений
            }
            else
            {
                listBox1.Visible = false; // Скрываем, если нет совпадений
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
                textBox1.SelectionStart = textBox1.Text.Length; // Устанавливаем курсор в конец
                listBox1.Visible = false; // Скрываем список после выбора
            }
        }

        private void button_tab_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            textBox1.Visible = false;
            listBox1.Visible = false;
            comboBox1.Visible = false;
            textBox2.Visible = true;
            button4.Visible = true;
            LoadData();
            SearchOption = "Tab";
        }

        private void button_fam_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            textBox2.Visible = false;
            comboBox1.Visible = false;
            textBox1.Visible = true;
            button4.Visible = true;
            LoadData();
            SearchOption = "Fam";
        }

        private void button_dol_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
            comboBox1.Visible = true;
            textBox1.Visible = false;
            listBox1.Visible = false;
            textBox2.Visible = false;
            button4.Visible = false;
            LoadData();
            Data("Должность", comboBox1);
         //   SearchOption = "Dol";
            FilterDGV(sender, e);
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataUpdated();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Проверяем, что кликнули по строке, а не заголовку
            {
                dataGridView1.Rows[e.RowIndex].Selected = true; // Выделяем строку
                LoadPhoto();
            }
        }

        private void LoadPhoto()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int tab_nomer = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["Табельный номер"].Value);

                try
                {
                    using (OleDbConnection myConnection = new OleDbConnection(connectString))
                    {
                        myConnection.Open();
                        string query = "SELECT photo FROM sotrudniki WHERE tab_nomer = ?";
                        OleDbCommand com = new OleDbCommand(query, myConnection);
                        com.Parameters.AddWithValue("?", tab_nomer);

                        using (OleDbDataReader myReader = com.ExecuteReader())
                        {
                            if (myReader.Read())
                            {
                                if (!myReader.IsDBNull(myReader.GetOrdinal("photo")))
                                {
                                    string PhotoPath = myReader.GetString(myReader.GetOrdinal("photo"));
                                    string photoDirectory = @"C:UsersuserDesktop\ДИПЛОМOtgruzkaSotr";
                                    string fullPhotoPath = Path.Combine(photoDirectory, PhotoPath);

                                    if (File.Exists(fullPhotoPath))
                                    {
                                        this.pictureBox1.Image?.Dispose(); // Освобождаем предыдущее изображение
                                        this.pictureBox1.Image = Image.FromFile(fullPhotoPath);
                                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Файл изображения не найден по указанному пути: " + fullPhotoPath);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Изображение не найдено для данного сотрудника.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
        }
    }
}
