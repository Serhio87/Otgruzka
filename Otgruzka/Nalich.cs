using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Otgruzka
{
    public partial class Nalich: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";

        private DataTable dataTable; //для хранения данных для фильтрации

        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";
    //    private OleDbConnection myConnection;
     //   private BindingSource bindingSource = new BindingSource();
    //    private DataTable db;
    private string query = @"SELECT profil.profil AS [Профиль], 
dlina.dlina AS [Длина], 
klass.marka AS [Класс стали (Стандарт)], 
Izdelie.plavka AS [№ Плавки], 
Izdelie.paket AS [№ Пакета], 
Izdelie.ves_izd AS [Вес пакета], 
Izdelie.date_post AS [Дата производства]
FROM profil INNER JOIN (klass INNER JOIN ((dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) 
LEFT JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
WHERE (((Prodazha.date_prod) Is Null))";

        public Nalich()
        {
            InitializeComponent();
            LoadForm();

            // Подписка на события изменения выбора для всех комбобоксов
            comboBoxProf.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxDl.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxKl.SelectedIndexChanged += comboBox_SelectedIndexChanged;
           // comboBoxSt.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxPlav.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxPak.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxVes.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxData.SelectedIndexChanged += comboBox_SelectedIndexChanged;

            dataGridView1.RowHeadersVisible = false;
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadForm()
        {
            // Загрузка данных из БД в DataGridView
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                try
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

                    //округление в столбцах
                    //      DataGridViewColumn ves = dataGridView1.Columns["Вес пакета"];
                    //      ves.DefaultCellStyle.Format = "0.000";

                    // Заполнение комбобоксов уникальными значениями
                    UpdateComboBoxes(dataTable);
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
                }
            }
            //изменение шрифта
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13); //заголовок
        }

        private void UpdateComboBoxes(DataTable dataTable)
        {
            // Проверяем, что DataTable не пустой
            if (dataTable == null || dataTable.Rows.Count == 0)
                return;

            // Для каждого столбца создаем HashSet для хранения уникальных значений
            HashSet<string>[] uniqueValues = new HashSet<string>[7];

            for (int i = 0; i < 7; i++)
            {
                uniqueValues[i] = new HashSet<string>();
            }

            // Проходим по всем строкам и собираем уникальные значения
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (i < dataTable.Columns.Count) // Проверка, что столбец существует
                    {
                        string value = row[i].ToString();
                        uniqueValues[i].Add(value);
                    }
                }
            }

            // Заполняем комбобоксы уникальными значениями и сортируем их
            for (int i = 0; i < 7; i++)
            {
                // Добавляем пустое значение в начало списка
                List<string> sortedValues = uniqueValues[i].ToList();
                sortedValues.Sort(); // Сортировка значений
                sortedValues.Insert(0, ""); // Вставка пустого значения

                // Установка источника данных для комбобокса
                switch (i)
                {
                    case 0:
                        comboBoxProf.DataSource = sortedValues;
                        break;
                    case 1:
                        comboBoxDl.DataSource = sortedValues;
                        break;
                    case 2:
                        comboBoxKl.DataSource = sortedValues;
                        break;
                    case 3:
                        comboBoxPlav.DataSource = sortedValues;
                        break;
                    case 4:
                        comboBoxPak.DataSource = sortedValues;
                        break;
                    case 5:
                        comboBoxVes.DataSource = sortedValues;
                        break;
                    case 6:
                        comboBoxData.DataSource = sortedValues;
                        break;
              //      case 7:
                        
                }
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Применяем фильтрацию на основе текущих значений комбобоксов
            FilterData(GetCurrentFilters());
        }

        private Dictionary<string, string> GetCurrentFilters()
        {
            var filters = new Dictionary<string, string>();

            // Изменяем ключи на соответствующие имена столбцов из SQL-запроса
            if (!string.IsNullOrEmpty(comboBoxProf.SelectedItem?.ToString()))
                filters["Профиль"] = comboBoxProf.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(comboBoxDl.SelectedItem?.ToString()))
                filters["Длина"] = comboBoxDl.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(comboBoxKl.SelectedItem?.ToString()))
                filters["[Класс стали (Стандарт)]"] = comboBoxKl.SelectedItem.ToString();
        //    if (!string.IsNullOrEmpty(comboBoxSt.SelectedItem?.ToString()))
        //        filters["Стандарт"] = comboBoxSt.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(comboBoxPlav.SelectedItem?.ToString()))
                filters["[№ Плавки]"] = comboBoxPlav.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(comboBoxPak.SelectedItem?.ToString()))
                filters["[№ Пакета]"] = comboBoxPak.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(comboBoxVes.SelectedItem?.ToString()))
                filters["[Вес пакета]"] = comboBoxVes.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(comboBoxData.SelectedItem?.ToString()))
                filters["[Дата производства]"] = comboBoxData.SelectedItem.ToString();

            return filters;
        }


        private void FilterData(Dictionary<string, string> filters)
        {
            // Здесь создаем строку фильтра на основе выбранных значений
            string filterExpression = "";

            foreach (var filter in filters)
            {
                if (!string.IsNullOrEmpty(filterExpression))
                    filterExpression += " AND "; // Добавляем оператор AND для последующих условий

                filterExpression += $"{filter.Key} = '{filter.Value}'"; // Формируем условие фильтрации
            }

            // Применяем фильтр к DataTable (предполагается, что у вас есть dataTable с данными)
            if (dataTable != null)
            {
                DataView dv = new DataView(dataTable);
                dv.RowFilter = filterExpression; // Устанавливаем фильтр
                dataGridView1.DataSource = dv; // Обновляем источник данных для DataGridView
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBoxProf.SelectedItem = "";
            comboBoxDl.SelectedItem = "";
            comboBoxKl.SelectedItem = "";
     //       comboBoxSt.SelectedItem = "";
            comboBoxPlav.SelectedItem = "";
            comboBoxPak.SelectedItem = "";
            comboBoxVes.SelectedItem = "";
            comboBoxData.SelectedItem = "";

            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        }

        private void изменитьШрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // Установка шрифта для ячеек
            dataGridView1.DefaultCellStyle.Font = fontDialog1.Font;

            // Автоматически рассчитываем высоту строк
           // dataGridView1.AutoResizeRows();
        }
    }
}

