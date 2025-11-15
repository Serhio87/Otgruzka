using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Otgruzka
{
    public partial class Nalich : Form
    {
        private DataView dv; // Поле для хранения отфильтрованных данных
        private int currentRowIndex = 0; // Текущий индекс строки для печати
        private DataTable dataTable; //для хранения данных для фильтрации

        private string query = @"SELECT profil.profil     AS [Профиль], 
                                        dlina.dlina       AS [Длина], 
                                        klass.marka       AS [Класс стали (Стандарт)], 
                                        Izdelie.plavka    AS [№ Плавки], 
                                        Izdelie.paket     AS [№ Пакета], 
                                        Izdelie.ves_izd   AS [Вес пакета], 
                                        Izdelie.date_post AS [Дата производства]
                                   FROM profil INNER JOIN (klass INNER JOIN ((dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) 
                              LEFT JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
                                  WHERE Izdelie.date_prod Is Null;";

        public Nalich()
        {
            InitializeComponent();
            LoadForm();

            // Подписка на события изменения выбора для всех комбобоксов
            comboBoxProf.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxDl.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            comboBoxKl.SelectedIndexChanged += comboBox_SelectedIndexChanged;
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
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                try
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

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

            // Применяем фильтр к DataTable
            if (dataTable != null)
            {
                dv = new DataView(dataTable);
                dv.RowFilter = filterExpression; // Устанавливаем фильтр
                dataGridView1.DataSource = dv; // Обновляем источник данных для DataGridView
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBoxProf.SelectedItem = "";
            comboBoxDl.SelectedItem = "";
            comboBoxKl.SelectedItem = "";
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
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Проверяем, есть ли примененные фильтры
            DataView dataView = dv ?? dataTable.DefaultView; // Используем dv, если оно не null, иначе используем DefaultView

            Font font = new Font("Arial", 12);
            Font headerFont = new Font("Arial", 14, FontStyle.Bold); // Жирный шрифт для заголовков
            float lineHeight = font.GetHeight(e.Graphics) + 4;
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top - 60;

            // Печатаем заголовки столбцов
            x = e.MarginBounds.Left - 70; // Сбрасываем x для заголовков
            List<float> columnWidths = new List<float>(); // Список для хранения ширин столбцов

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                string headerText = column.HeaderText;
                SizeF headerSize = e.Graphics.MeasureString(headerText, headerFont);
                float headerWidth = Math.Max(column.Width, headerSize.Width); // Устанавливаем минимальную ширину

                // Рисуем внутренние линии ячеек для заголовков
                e.Graphics.DrawRectangle(Pens.Black, x, y, headerWidth, lineHeight);
                // Печатаем текст в заголовке
                e.Graphics.DrawString(headerText, headerFont, Brushes.Black, x + 2, y + 2); // Добавляем небольшой отступ
                columnWidths.Add(headerWidth); // Сохраняем ширину столбца
                x += headerWidth; // Увеличиваем отступ между столбцами
            }
            y += lineHeight; // Переходим к следующей строке после заголовков

            // Печатаем строки данных
            while (currentRowIndex < dataView.Count)
            {
                DataRowView row = dataView[currentRowIndex];
                x = e.MarginBounds.Left - 70;

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    DataGridViewColumn column = dataGridView1.Columns[i];
                    string cellValue = row[column.Name]?.ToString() ?? string.Empty;

                    // Вычисляем ширину текста для ячейки с учетом многоточия
                    SizeF textSize = e.Graphics.MeasureString(cellValue, font);
                    float cellWidth = columnWidths[i]; // Используем сохраненную ширину столбца

                    // Рисуем внутренние линии ячеек
                    e.Graphics.DrawRectangle(Pens.Black, x, y, cellWidth, lineHeight);
                    // Печатаем текст в ячейке
                    e.Graphics.DrawString(cellValue, font, Brushes.Black, x + 2, y + 2); // Добавляем небольшой отступ
                    x += cellWidth; // Увеличиваем отступ между столбцами
                }

                currentRowIndex++; // Переходим к следующей строке
                y += lineHeight; // Увеличиваем высоту для следующей строки

                // Проверяем, помещается ли следующая строка на текущей странице
                if (y + lineHeight > e.MarginBounds.Bottom + 70)
                {
                    e.HasMorePages = true; // Указываем, что есть еще страницы для печати
                    return; // Завершаем текущую страницу
                }
            }
            // Если все строки напечатаны, указываем, что печать завершена
            e.HasMorePages = false;
        }

        private void предварительныйПросмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Устанавливаем альбомную ориентацию
            printDocument1.DefaultPageSettings.Landscape = true;

            // Устанавливаем размер окна
            printPreviewDialog1.Size = new Size(1200, 1024); // Установите нужный размер

            // Устанавливаем положение окна
            printPreviewDialog1.StartPosition = FormStartPosition.Manual;
            printPreviewDialog1.Location = new Point(0, 0);

            printPreviewDialog1.ShowIcon = false;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void печатьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Устанавливаем альбомную ориентацию
            printDocument1.DefaultPageSettings.Landscape = true;

            printDialog1.Document = printDocument1;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                // Установка выбранного принтера
                printDocument1.PrinterSettings = printDialog1.PrinterSettings;
                printDocument1.Print();
            }
        }

        private void сохранитьВФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true; // Отобразить Excel после создания файла

            // Создаем новую книгу и новый лист
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

            // Записываем заголовки столбцов
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }

            // Записываем данные строк
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    // Проверяем, не является ли ячейка пустой
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            MessageBox.Show("Передано в Excell! Не забудьте при необходимости сохранить файл!", "ВНИМАНИЕ!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

            // Освобождаем ресурсы
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
    }
}

