using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Otgruzka
{
    public partial class Form1 : Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        //public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|newBD.accdb";

        private OleDbConnection myConnection;
        private BindingSource bindingSource = new BindingSource();
        private DataTable db;
        private string PozMenu;
        private string dol;
        private string FIO;
        private int tab_n;

        public Form1()
        {
            InitializeComponent();

          //  dol = dolzhn;
        //    PozMenu = dol;
        //    FIO = fio;
        //    tab_n = tab_nomer;

        //    ConfigMenu();

         //   MaximizeBox = false;
            dataGridView1.RowHeadersVisible = false;
        //    dataGridView1.CellFormatting += DGV_Format;

            LoadForm();
        //    InitializeComboBoxes();
        }

        private void LoadForm()
        {
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();

       /*     string query = @"SELECT     profil.profil           AS [Профиль], 
                                        dlina.dlina             AS [Длина], 
                                        klass.marka             AS [Класс стали], 
                                        standart.standart       AS [Стандарт], 
                                        Izdelie.price           AS [Цена], 
                                        Izdelie.ves_izd         AS [Вес], 
                                        Izdelie.date_post       AS [Дата производства], 
                                        Prodazha.date_prod      AS [Дата продажи], 
                                        Prodazha.nomer_trst     AS [Номер транспорта]
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
                            ORDER BY    Izdelie.date_post DESC;";
       */
            string query = @"SELECT profil.profil AS [Профиль], 
dlina.dlina AS [Длина], 
klass.marka AS [Класс стали], 
standart.standart AS [Стандарт], 
Izdelie.plavka AS [№ Плавки], 
Izdelie.paket AS [№ Пакета], 
Izdelie.ves_izd AS [Вес пакета], 
Izdelie.date_post AS [Дата производства]
FROM standart 
INNER JOIN (profil 
INNER JOIN (klass 
INNER JOIN ((dlina 
INNER JOIN Izdelie 
ON dlina.Код = Izdelie.dlina) 
LEFT JOIN Prodazha 
ON Izdelie.kod_izd = Prodazha.kod_izd) 
ON klass.Код = Izdelie.klass) 
ON profil.Код = Izdelie.profil) 
ON standart.Код = Izdelie.standart
WHERE (((Prodazha.date_prod) Is Null));";

            OleDbDataAdapter command = new OleDbDataAdapter(query, myConnection);
            db = new DataTable();
            command.Fill(db);
            bindingSource.DataSource = db;
            dataGridView1.DataSource = db;

            // Скрываем столбцы по умолчанию
        //    dataGridView1.Columns["Дата продажи"].Visible = false;
        //    dataGridView1.Columns["Номер транспорта"].Visible = false;
        //    dataGridView1.Columns["Вид транспорта"].Visible = false;
        //    comboBoxDProd.Visible = false;
        //    comboBoxTrsr.Visible = false;

            //округление в столбцах
          //  DataGridViewColumn price = dataGridView1.Columns["Цена"];
          //  price.DefaultCellStyle.Format = "0.00";
            DataGridViewColumn ves = dataGridView1.Columns["Вес пакета"];
            ves.DefaultCellStyle.Format = "0.000";

            // Подписываемся на событие CheckedChanged
    //        checkBox1.CheckedChanged += checkBox1_CheckedChanged;

        }
        private void LoadForm1()
                {
                    myConnection = new OleDbConnection(connectString);
                    myConnection.Open();

                    string query = @"SELECT     profil.profil           AS [Профиль], 
                                                dlina.dlina             AS [Длина], 
                                                klass.marka             AS [Класс стали], 
                                                standart.standart       AS [Стандарт], 
                                                Izdelie.price           AS [Цена], 
                                                Izdelie.ves_izd         AS [Вес], 
                                                Izdelie.date_post       AS [Дата производства],
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
                                    WHERE       Prodazha.date_prod IS NULL
                                    ORDER BY    Izdelie.date_post DESC;";

                    


            OleDbDataAdapter command = new OleDbDataAdapter(query, myConnection);
                    db = new DataTable();

                    // Заполняем DataTable асинхронно
                    //await Task.Run(() => command.Fill(db));

                    command.Fill(db);
                    bindingSource.DataSource = db;
                    dataGridView1.DataSource = db;

                    // Скрываем столбцы по умолчанию
                    dataGridView1.Columns["Дата продажи"].Visible = false;
                //    dataGridView1.Columns["Номер транспорта"].Visible = false;
               //     comboBoxDProd.Visible = false;
               //     comboBoxTrsr.Visible = false;

                    //округление в столбцах
                    DataGridViewColumn price = dataGridView1.Columns["Цена"];
                    price.DefaultCellStyle.Format = "0.00";
                    DataGridViewColumn ves = dataGridView1.Columns["Вес"];
                    ves.DefaultCellStyle.Format = "0.000";

                    // Подписываемся на событие CheckedChanged
    //                checkBox1.CheckedChanged += checkBox1_CheckedChanged;
                }

        private void DGV_Format(object sender, DataGridViewCellFormattingEventArgs e) //Окрашивание строки
        {
            int targetColumnIndex = 10; // Индекс столбца проверки
            if (e.RowIndex >= 0 && e.ColumnIndex == targetColumnIndex) // Проверяем, что это не заголовок столбца
            {
                var cellValue = e.Value as string; // Получаем значение ячейки
                if (!string.IsNullOrEmpty(cellValue)) dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red; // Устанавливаем цвет текста
            }
        }

        private void InitializeComboBoxes()
        {
            // Заполнение комбобоксов уникальными значениями
            Data("Профиль", comboBoxProfil);
            Data("Длина", comboBoxDlina);
            Data("Класс стали", comboBoxKlass);
            Data("Стандарт", comboBoxStand);
         //   Data("Цена", comboBoxPrice);
            Data("Вес пакета", comboBoxVes);
            Data("Дата производства", comboBoxDPost);
     //       Data("Дата продажи", comboBoxDProd);
      //      Data("Номер транспорта", comboBoxTrsr);
        }

        private void Data(string columnName, System.Windows.Forms.ComboBox comboBox)
        {
            //       DataTable db = ((DataTable)bindingSource.DataSource);
            /*         var uniqueValues = db.AsEnumerable()
                                  .Select(row => row.Field<object>(columnName)?.ToString())
                                  .Where(value => value != null)
                                  .Distinct()
                                  .OrderBy(value => value)
                                  .ToList();
            */
            var uniqueValues = dataGridView1.Rows
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
            if (dataGridView1.DataSource is DataTable dataTable && dataTable.Rows.Count > 0)
            {
                var filterBuilder = new StringBuilder();

                // Проверяем каждое значение комбобокса и формируем строку фильтра
                AddFilter(comboBoxProfil, "Профиль", filterBuilder);
                AddFilter(comboBoxDlina, "Длина", filterBuilder);
                AddFilter(comboBoxKlass, "[Класс стали]", filterBuilder);
                AddFilter(comboBoxStand, "Стандарт", filterBuilder);
     //           AddFilter(comboBoxPrice, "Цена", filterBuilder);
                AddFilter(comboBoxVes, "[Вес пакета]", filterBuilder);
                AddFilter(comboBoxDPost, "[Дата производства]", filterBuilder);

                // Специальная обработка для комбобоксов с датами
          //      AddDateFilter(comboBoxDProd, "[Дата продажи]", filterBuilder);
         //       AddTransportFilter(comboBoxTrsr, "[Номер транспорта]", filterBuilder);

                // Убираем последний AND, если он есть
                if (filterBuilder.Length > 0)
                {
                    string filter = filterBuilder.ToString();
                    if (filter.EndsWith(" AND "))
                        filter = filter.Substring(0, filter.Length - 5);

                    // Применяем фильтр к DataGridView
                    dataTable.DefaultView.RowFilter = filter;
                }

                /*       if (comboBoxProfil.SelectedItem != null && comboBoxProfil.SelectedItem.ToString() != "Все")
                           filter += $"Профиль = '{comboBoxProfil.SelectedItem}' AND ";

                       if (comboBoxDlina.SelectedItem != null && comboBoxDlina.SelectedItem.ToString() != "Все")
                           filter += $"Длина = '{comboBoxDlina.SelectedItem}' AND ";

                       if (comboBoxKlass.SelectedItem != null && comboBoxKlass.SelectedItem.ToString() != "Все")
                           filter += $"[Класс стали] = '{comboBoxKlass.SelectedItem}' AND ";

                       if (comboBoxStand.SelectedItem != null && comboBoxStand.SelectedItem.ToString() != "Все")
                           filter += $"Стандарт = '{comboBoxStand.SelectedItem}' AND ";

                       if (comboBoxPrice.SelectedItem != null && comboBoxPrice.SelectedItem.ToString() != "Все")
                           filter += $"Цена = '{comboBoxPrice.SelectedItem}' AND ";

                       if (comboBoxVes.SelectedItem != null && comboBoxVes.SelectedItem.ToString() != "Все")
                           filter += $"Вес = '{comboBoxVes.SelectedItem}' AND ";

                       if (comboBoxDPost.SelectedItem != null && comboBoxDPost.SelectedItem.ToString() != "Все")
                           filter += $"[Дата производства] = '{comboBoxDPost.SelectedItem}' AND ";

                       if (comboBoxDProd.SelectedItem != null)
                       {
                           if (comboBoxDProd.SelectedItem.ToString() == "Все") { }
                           else if (string.IsNullOrEmpty(comboBoxDProd.SelectedItem.ToString()))
                           { filter += $"[Дата продажи] IS NULL AND "; }
                           else
                           { filter += $"[Дата продажи] = '{comboBoxDProd.SelectedItem}' AND "; }
                       }

                       if (comboBoxTrsr.SelectedItem != null)
                       {
                           if (comboBoxTrsr.SelectedItem.ToString() == "Все") { }
                           else if (string.IsNullOrEmpty(comboBoxTrsr.SelectedItem.ToString()))
                           { filter += $"[Номер транспорта] IS NULL OR [Номер транспорта] = '' AND "; }
                           else
                           { filter += $"[Номер транспорта] = '{comboBoxTrsr.SelectedItem}' AND "; }
                       }

                       // Убираем последний AND, если он есть
                       if (filter.EndsWith(" AND "))
                           filter = filter.Substring(0, filter.Length - 5);

                       // Применяем фильтр к DataGridView
                       (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
                */
            }
        }

        private void AddFilter(System.Windows.Forms.ComboBox comboBox, string columnName, StringBuilder filterBuilder)
        {
            if (comboBox.SelectedItem != null && comboBox.SelectedItem.ToString() != "Все")
            {
                filterBuilder.Append($"{columnName} = '{comboBox.SelectedItem}' AND ");
            }
        }
        private void AddDateFilter(System.Windows.Forms.ComboBox comboBox, string columnName, StringBuilder filterBuilder)
        {
            if (comboBox.SelectedItem != null)
            {
                if (comboBox.SelectedItem.ToString() == "Все")
                    return;

                if (string.IsNullOrEmpty(comboBox.SelectedItem.ToString()))
                {
                    filterBuilder.Append($"{columnName} IS NULL AND ");
                }
                else
                {
                    filterBuilder.Append($"{columnName} = '{comboBox.SelectedItem}' AND ");
                }
            }
        }
        private void AddTransportFilter(System.Windows.Forms.ComboBox comboBox, string columnName, StringBuilder filterBuilder)
        {
            if (comboBox.SelectedItem != null)
            {
                if (comboBox.SelectedItem.ToString() == "Все")
                    return;

                if (string.IsNullOrEmpty(comboBox.SelectedItem.ToString()))
                {
                    filterBuilder.Append($"{columnName} IS NULL OR {columnName} = '' AND ");
                }
                else
                {
                    filterBuilder.Append($"{columnName} = '{comboBox.SelectedItem}' AND ");
                }
            }
        }
        private void FilterDGV7(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource is DataTable dataTable && dataTable.Rows.Count > 0)
            {
                var filterBuilder = new StringBuilder();
                //string filter = "";

                // Проверяем каждое значение комбобокса и формируем строку фильтра
                AddFilter(comboBoxProfil, "Профиль", filterBuilder);
                AddFilter(comboBoxDlina, "Длина", filterBuilder);
                AddFilter(comboBoxKlass, "[Класс стали]", filterBuilder);
                AddFilter(comboBoxStand, "Стандарт", filterBuilder);
         //       AddFilter(comboBoxPrice, "Цена", filterBuilder);
                AddFilter(comboBoxVes, "[Вес пакета]", filterBuilder);
                AddFilter(comboBoxDPost, "[Дата производства]", filterBuilder);

                // Убираем последний AND, если он есть
                if (filterBuilder.Length > 0)
                {
                    string filter = filterBuilder.ToString();
                    if (filter.EndsWith(" AND "))
                        filter = filter.Substring(0, filter.Length - 5);

                    // Применяем фильтр к DataGridView
                    dataTable.DefaultView.RowFilter = filter;
                }

                /*        string filter = "";

                        // Проверяем каждое значение комбобокса и формируем строку фильтра
                        if (comboBoxProfil.SelectedItem != null && comboBoxProfil.SelectedItem.ToString() != "Все")
                            filter += $"Профиль = '{comboBoxProfil.SelectedItem}' AND ";

                        if (comboBoxDlina.SelectedItem != null && comboBoxDlina.SelectedItem.ToString() != "Все")
                            filter += $"Длина = '{comboBoxDlina.SelectedItem}' AND ";

                        if (comboBoxKlass.SelectedItem != null && comboBoxKlass.SelectedItem.ToString() != "Все")
                            filter += $"[Класс стали] = '{comboBoxKlass.SelectedItem}' AND ";

                        if (comboBoxStand.SelectedItem != null && comboBoxStand.SelectedItem.ToString() != "Все")
                            filter += $"Стандарт = '{comboBoxStand.SelectedItem}' AND ";

                        if (comboBoxPrice.SelectedItem != null && comboBoxPrice.SelectedItem.ToString() != "Все")
                            filter += $"Цена = '{comboBoxPrice.SelectedItem}' AND ";

                        if (comboBoxVes.SelectedItem != null && comboBoxVes.SelectedItem.ToString() != "Все")
                            filter += $"Вес = '{comboBoxVes.SelectedItem}' AND ";

                        if (comboBoxDPost.SelectedItem != null && comboBoxDPost.SelectedItem.ToString() != "Все")
                            filter += $"[Дата производства] = '{comboBoxDPost.SelectedItem}' AND ";

                        //     if (comboBoxDProd.SelectedItem != null && comboBoxDProd.SelectedItem.ToString() != "Все")
                        //         filter += $"[Дата продажи] = '{comboBoxDProd.SelectedItem}' AND ";

                        //     if (comboBoxTrsr.SelectedItem != null && comboBoxTrsr.SelectedItem.ToString() != "Все")
                        //         filter += $"[Номер транспорта] = '{comboBoxTrsr.SelectedItem}' AND ";

                        // Убираем последний AND, если он есть
                        if (filter.EndsWith(" AND "))
                            filter = filter.Substring(0, filter.Length - 5);

                        // Применяем фильтр к DataGridView
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
                */
            }
        }

        private void comboBoxProfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV7(sender, e);
        }
        private void comboBoxDlina_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV7(sender, e);
        }
        private void comboBoxKlass_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV7(sender, e);
        }
        private void comboBoxStand_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV7(sender, e);
        }
        private void comboBoxPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV7(sender, e);
        }
        private void comboBoxVes_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV7(sender, e);
        }
        private void comboBoxDPost_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV7(sender, e);
        }
        private void comboBoxDProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV(sender, e);
        }
        private void comboBoxTrsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDGV(sender, e);
        }

        private void ConfigMenu()
        {
     //       отчетыToolStripMenuItem.Visible = false;
    //        персToolStripMenuItem.Visible = false;
     //       ДобПродToolStripMenuItem.Visible = false;
     //       новприкToolStripMenuItem.Visible = false;
    //        ПокупToolStripMenuItem.Visible = false;

            switch (PozMenu)
            {
                case "Кладовщик":
     //               ДобПродToolStripMenuItem.Visible = true;
                    this.Text = $"{ dol } - {FIO}";
                break;

                case "Бригадир":
                    this.Text = $"{dol} - {FIO}";
    //                новприкToolStripMenuItem.Visible = true;
                break;

          /*      case "Экономист":
                    
                    button1.Visible = true;
                    обнЭконToolStripMenuItem1.Visible = true;
                    обновитьToolStripMenuItem.Visible = false;
                    LoadFormEkon();
                    dataGridView2.Visible = true;
                    dataGridView2.Width = 680;  //ширина столбца
                    this.Width = 1000; //ширина окна
            //        this.Height = 600;  //длина
                    новприкToolStripMenuItem.Visible = true;
             //       ПокупToolStripMenuItem.Visible = true;
                    изменитьЦенуToolStripMenuItem.Visible = true;
                    dataGridView1.Visible = false;
                    comboBoxDPost.Visible = false;
                    comboBoxDProd.Visible = false;
                    comboBoxTrsr.Visible = false;
                    comboBoxProfil.Visible = false;
                    comboBoxDlina.Visible = false;
                    comboBoxKlass.Visible = false;
                    comboBoxStand.Visible = false;
                    comboBoxPrice.Visible = false;
                    comboBoxVes.Visible = false;
                    comboBox1.Visible = true;
                    comboBox2.Visible = true;
                    comboBox3.Visible = true;
                    comboBox4.Visible = true;
                    comboBox5.Visible = true;

                    break;
          */
                case "Мастер":  //добавить др эл-ты управления
       //             отчетыToolStripMenuItem.Visible = true;
       //             новприкToolStripMenuItem.Visible = true;
       //             персToolStripMenuItem.Visible = true;
       //             ДобПродToolStripMenuItem.Visible = true;
       //             ПокупToolStripMenuItem.Visible = true;
                    this.Text = $"{dol} - {FIO}";
                break;
            }
        }



        /*      private void Form1_FormClosing(object sender, FormClosingEventArgs e)
              {
                  this.Close();
              }

              private void Form1_FormClosed(object sender, FormClosedEventArgs e)
              {
                  myConnection.Close();
              }
        */
        private void персToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sotr newForm = new Sotr();
            newForm.Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 newBox = new AboutBox1();
            newBox.ShowDialog();
        }

        private void сменаПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            enterBox newForm = new enterBox();
            newForm.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pass newForm1 = new pass(tab_n);
            newForm1.Show();
        }

        private void ДобПродToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DobProd newForm = new DobProd();
            newForm.Show();
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //          comboBoxTrsr.SelectedItem = "Все";
            //          comboBoxDProd.SelectedItem = "Все";
            dataGridView1.DataSource = null;
            LoadForm();
            InitializeComboBoxes();
   //         checkBox1.Checked = false;
        }

  /*      private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                LoadForm1();
             //   dataGridView1.Columns["Дата продажи"].Visible = false;
                //      comboBoxDProd.Location = new Point(1034,24);
             //   comboBoxDProd.Size = new Size(148, 21);
              //  dataGridView1.Columns["Номер транспорта"].Visible = false;
                //       comboBoxTrsr.Location = new Point(1182, 24);
             //   comboBoxTrsr.Size = new Size(148, 21);
                comboBoxDProd.Visible = false;
                comboBoxTrsr.Visible = false;

                comboBoxProfil.Location = new Point(0, 24);
                comboBoxDlina.Location = new Point(190, 24);
                comboBoxKlass.Location = new Point(380, 24);
                comboBoxStand.Location = new Point(570, 24);
                //comboBoxPrice.Location = new Point(760, 24);
                comboBoxVes.Location = new Point(950, 24);
                comboBoxDPost.Location = new Point(1140, 24);

                comboBoxProfil.Size = new Size(190, 21);
                comboBoxDlina.Size = new Size(190, 21);
                comboBoxKlass.Size = new Size(190, 21);
                comboBoxStand.Size = new Size(190, 21);
                //comboBoxPrice.Size = new Size(190, 21);
                comboBoxVes.Size = new Size(190, 21);
                comboBoxDPost.Size = new Size(190, 21);
            }

            else
            {
                LoadForm();
                Data("Дата продажи", comboBoxDProd);
                Data("Номер транспорта", comboBoxTrsr);

                dataGridView1.Columns["Дата продажи"].Visible = true;
                //      comboBoxDProd.Location = new Point(1034,24);
                comboBoxDProd.Size = new Size(148, 21);
                dataGridView1.Columns["Номер транспорта"].Visible = true;
                //       comboBoxTrsr.Location = new Point(1182, 24);
                comboBoxTrsr.Size = new Size(148, 21);
                comboBoxDProd.Visible = true;
                comboBoxTrsr.Visible = true;

                comboBoxProfil.Location = new Point(0, 24);
                comboBoxDlina.Location = new Point(148, 24);
                comboBoxKlass.Location = new Point(296, 24);
                comboBoxStand.Location = new Point(444, 24);
                //comboBoxPrice.Location = new Point(592, 24);
                comboBoxVes.Location = new Point(740, 24);
                comboBoxDPost.Location = new Point(888, 24);
                comboBoxProfil.Size = new Size(148, 21);
                comboBoxDlina.Size = new Size(148, 21);
                comboBoxKlass.Size = new Size(148, 21);
                comboBoxStand.Size = new Size(148, 21);
                //comboBoxPrice.Size = new Size(148, 21);
                comboBoxVes.Size = new Size(148, 21);
                comboBoxDPost.Size = new Size(148, 21);
            }
        }
  */
        private void ПокупToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            pokupatel po = new pokupatel();
            po.Show();
        }

  /*      private void button1_Click(object sender, EventArgs e)
        {
            pokupatel p = new pokupatel();
            p.Show();
        }
  */
        private void новприкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Prodazha pp = new Prodazha(tab_n);
            pp.Show();
        }

  /*      private void отчетыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
  */
        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Repo2 r2 = new Repo2();
            r2.Show();
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Repo1 re = new Repo1();
            re.Show();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*      private void обновитьToolStripMenuItem_MouseEnter(object sender, EventArgs e)
              {
                  обновитьToolStripMenuItem.Text = "Обновить таблицу";
              }
              private void обновитьToolStripMenuItem_MouseLeave(object sender, EventArgs e)
              {
                  обновитьToolStripMenuItem.Text = "Обновить";
              }
        */
    }
}