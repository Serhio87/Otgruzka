using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Otgruzka
{
    public partial class Ekon: Form
    {
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\UNIVER\\3session\\SVPP\\KP2025\\Otgruzka\\Otgruzka\\newBD.accdb";
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private OleDbConnection myConnection;
        private BindingSource bindingSource = new BindingSource();
        private DataTable db;
        private string dol;
        private string FIO;
        private int tab_n;
  //      private int CodeProfil;
  //      private int CodeDl;
  //      private int CodeKlass;

        public Ekon(string dolzhn, string fio, int tab_nomer)
        {
            InitializeComponent();
            dol = dolzhn;
            FIO = fio;
            this.Text = $"{dol} - {FIO}";
            LoadForm();

       /*     // Создаем и отображаем заставку
            using (logo2 splash = new logo2())
            {
                splash.Show();
                Thread.Sleep(3000); // Задержка для демонстрации
                splash.Close();
            }
       */
        }

        private void LoadForm()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();

            string query = @"SELECT     Izdelie.kod_izd AS [Код],
                                        profil.profil           AS [Профиль], 
                                        dlina.dlina             AS [Длина], 
                                        klass.marka             AS [Класс стали], 
                                        Izdelie.price           AS [Цена], 
                                        Izdelie.ves_izd         AS [Вес], 
                                        Prodazha.date_prod      AS [Дата продажи]
                            FROM        standart 
                            INNER JOIN  (profil 
                            INNER JOIN  (klass 
                            INNER JOIN  (dlina 
                            INNER JOIN  (Izdelie 
                            LEFT JOIN   Prodazha 
                            ON          Izdelie.kod_izd = Prodazha.kod_izd) 
                            ON          dlina.Код = Izdelie.dlina) 
                            ON          klass.Код = Izdelie.klass) 
                            ON          profil.Код = Izdelie.profil) 
                            ON          standart.Код = Izdelie.standart
                            WHERE       Prodazha.date_prod IS NULL;";

            OleDbDataAdapter command = new OleDbDataAdapter(query, myConnection);
            db = new DataTable();
            command.Fill(db);
            bindingSource.DataSource = db;
            dataGridView2.DataSource = db;
            dataGridView2.Columns["Код"].Visible = false;
            dataGridView2.Columns["Дата продажи"].Visible = false;

            Data("Профиль", comboBox1);
            Data("Длина", comboBox2);
            Data("Класс стали", comboBox3);
            Data("Цена", comboBox4);
            Data("Вес", comboBox5);
        }

        private void Data(string columnName, System.Windows.Forms.ComboBox comboBox)
        {
            var uniqueValues = dataGridView2.Rows
                 .Cast<DataGridViewRow>()
                 .Select(row => row.Cells[columnName].Value?.ToString())
                 .Where(value => value != null)
                 .Distinct()
                 .OrderBy(value => value)
                 .ToList();

            comboBox.Items.Clear();
            comboBox.Items.Add("Все"); // Добавляем опцию "Все"
            comboBox.SelectedIndex = 0;

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
                filter += $"Профиль = '{comboBox1.SelectedItem}' AND ";

            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() != "Все")
                filter += $"Длина = '{comboBox2.SelectedItem}' AND ";

            if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() != "Все")
                filter += $"[Класс стали] = '{comboBox3.SelectedItem}' AND ";

            if (comboBox4.SelectedItem != null && comboBox4.SelectedItem.ToString() != "Все")
                filter += $"Цена = '{comboBox4.SelectedItem}' AND ";

            if (comboBox5.SelectedItem != null && comboBox5.SelectedItem.ToString() != "Все")
                filter += $"Вес = '{comboBox5.SelectedItem}' AND ";

            // Убираем последний AND, если он есть
            if (filter.EndsWith(" AND "))
                filter = filter.Substring(0, filter.Length - 5);

            // Применяем фильтр к DataGridView
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void сменитьПарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pass newForm1 = new pass(tab_n);
            newForm1.Show();
        }

        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            enterBox newForm = new enterBox();
            newForm.Show();
        }

        private void Ekon_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pokupatel po = new pokupatel();
            po.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Repo1 R1 = new Repo1();
            R1.Show();
        }

 /*       private void поToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pokupatel po = new pokupatel();
            po.Show();
        }
 */
        private void button3_Click(object sender, EventArgs e)
        {
            Repo2 R2 = new Repo2();
            R2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Проверяем, выбрана ли строка
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Получаем 
                int selectedRowIndex = dataGridView2.SelectedRows[0].Index;
                int kod_izd = Convert.ToInt32(dataGridView2.Rows[selectedRowIndex].Cells["Код"].Value);

                // Открываем диалоговое окно для ввода новой цены
                newPrice f = new newPrice(kod_izd);
                //  f.ShowDialog();
                f.Show();
                LoadForm();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения цены.");
            }
        }
    }
}
