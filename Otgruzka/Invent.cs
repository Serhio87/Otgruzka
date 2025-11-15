using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class Invent : Form
    {
        private System.Data.DataTable dataTable;

        private string query_so_all = @"SELECT profil.profil        AS [Профиль], 
                                               klass.marka          AS [Класс стали], 
                                               dlina.dlina          AS [Длина], 
                                               Izdelie.plavka       AS [№ Плавки], 
                                               Count(Izdelie.paket) AS [Кол-во пакетов], 
                                               Sum(Izdelie.ves_izd) AS [Вес, тонн]
                                          FROM profil INNER JOIN (klass INNER JOIN (dlina INNER JOIN Izdelie 
                                            ON dlina.Код = Izdelie.dlina) ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
                                         WHERE Izdelie.date_prod Is Null
                                      GROUP BY profil.profil, klass.marka, dlina.dlina, Izdelie.plavka;";

        private string query_all = @"SELECT profil.profil   AS [Профиль], 
                                            dlina.dlina     AS [Длина], 
                                            klass.marka     AS [Класс стали], 
                                            Izdelie.plavka  AS [№ Плавки], 
                                            Izdelie.paket   AS [№ Пакета], 
                                            Izdelie.ves_izd AS [Вес, тонн]
                                       FROM profil INNER JOIN (klass INNER JOIN (dlina INNER JOIN Izdelie 
                                         ON dlina.Код = Izdelie.dlina) ON klass.Код = Izdelie.klass) ON profil.Код = Izdelie.profil
                                      WHERE Izdelie.date_prod Is Null 
                                   GROUP BY profil.profil, dlina.dlina, klass.marka, Izdelie.plavka, Izdelie.paket, Izdelie.ves_izd;";

        public Invent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(Metods.ConnectionString))
            {
                if (radioButton1.Checked)
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query_all, conn);
                    dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);
                    ExportToExcel(dataTable);
                }
                if (radioButton2.Checked)
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(query_so_all, conn);
                    dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);
                    ExportToExcel(dataTable);
                }
            }
            OpenWordDocument();
            this.Close();
            MessageBox.Show("Передано в Excell. Не забудьте при необходимости сохранить файл!", "ВНИМАНИЕ!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

        private void ExportToExcel(System.Data.DataTable dataTable)
        {
            // Создаем новый экземпляр Excel
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = true; // Сделать Excel видимым

            // Создаем новую книгу
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

            // Записываем заголовки столбцов
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dataTable.Columns[i].ColumnName;
            }

            // Записываем данные
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataTable.Rows[i][j];
                }
            }

            // Освобождаем ресурсы
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        public void OpenWordDocument()
        {
            string filePath = Metods.InventPrikFile;

            if (File.Exists(filePath))
            {
                // Открытие word
                Process.Start(filePath);
            }
        }
    }
}
