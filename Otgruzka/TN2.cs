using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class TN2 : Form
    {
        private string queryRaz = @"SELECT sotrudniki.fam, sotrudniki.im, sotrudniki.otch, dolzhnost.dolzhn
                                    FROM dolzhnost INNER JOIN sotrudniki ON dolzhnost.Код = sotrudniki.dolzhn
                                    WHERE dolzhnost.dolzhn='Мастер';";

        private string queryOtp = @"SELECT sotrudniki.fam, sotrudniki.im, sotrudniki.otch, dolzhnost.dolzhn
                                    FROM dolzhnost INNER JOIN sotrudniki ON dolzhnost.Код = sotrudniki.dolzhn
                                    WHERE dolzhnost.dolzhn='Бригадир' OR dolzhnost.dolzhn='Кладовщик';";

        private string docs, prop, NDS_str, Nds_sum_str, Sum_sNDS_str;
        private decimal NDS;
        private decimal Sum_sNDS, Nds_sum, pr;

        public TN2()
        {
            InitializeComponent();
            MaximizeBox = false;
        }

        private class Fams
        {
            public string fam { get; set; }
            public string im { get; set; }
            public string otch { get; set; }
            public string dol { get; set; }

            public override string ToString()
            {
                return $"{fam} {im} {otch}";
            }
        }

        private void LoadRaz()
        {
            Metods.LoadCombo(queryRaz, comboBox1, reader =>
            new Fams
            {
                fam = reader["fam"].ToString(),
                im = reader["im"].ToString(),
                otch = reader["otch"].ToString(),
                dol = reader["dolzhn"].ToString()
            });
        }

        private void LoadOtp()
        {
            Metods.LoadCombo(queryOtp, comboBox2, reader =>
            new Fams
            {
                fam = reader["fam"].ToString(),
                im = reader["im"].ToString(),
                otch = reader["otch"].ToString(),
                dol = reader["dolzhn"].ToString()
            });
        }

        private void TN2_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private DataTable GetData()
        {
            int prik = Convert.ToInt32(textBox1.Text);

            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(Metods.ConnectionString))
            {
                connection.Open();

                string query = @"SELECT Prikaz.date_prik, Prikaz.kolich, Prikaz.price_all, pokupatel.nazvanie, pokupatel.gorod, pokupatel.strana, Prikaz.usl_post, Prodazha.nomer_trst, Count(Prodazha.kod_izd) AS [Count], Prikaz.id_prik, Prikaz.klass, Prikaz.prof, VidTr.Vid
                                   FROM VidTr INNER JOIN ((pokupatel INNER JOIN Prikaz ON pokupatel.Код = Prikaz.pokup) INNER JOIN (Izdelie INNER JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd) ON Prikaz.id_prik = Prodazha.id_prik) ON VidTr.Код = Prodazha.kod_vidTr
                               GROUP BY Prikaz.date_prik, Prikaz.kolich, Prikaz.price_all, pokupatel.nazvanie, pokupatel.gorod, pokupatel.strana, Prikaz.usl_post, Prodazha.nomer_trst, Prikaz.id_prik, Prikaz.klass, Prikaz.prof, VidTr.Vid
                                 HAVING Prikaz.id_prik = ? ;";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    command.Parameters.AddWithValue("?", prik);
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Приказ отсутствует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Focus();
                        return null;
                    }
                    DataRow row = dataTable.Rows[0];

                    pr = Convert.ToDecimal(row["price_all"]);
                    NDS = Convert.ToDecimal(textBox3.Text);
                    Nds_sum = Math.Round(pr / 100 * NDS, 2);
                    Sum_sNDS = Math.Round(pr + Nds_sum, 2);
                }
            }
            return dataTable;
        }

        private void ShowReport()
        {
            DataTable data = GetData();
            ReportDataSource rds = new ReportDataSource("DataSet1", data); // имя набора данных
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fams select = (Fams)comboBox1.SelectedItem;
            if (select != null)
            {
                // Создаем параметр для отчета
                ReportParameter[] parameters = new ReportParameter[]
                {
                    new ReportParameter("DR", select.dol),
                    new ReportParameter("FR", select.fam), // имя параметра, используемое в отчете
                    new ReportParameter("IR", select.im),    // Имя
                    new ReportParameter("OR", select.otch), // Отчество
                };

                // Устанавливаем параметры в ReportViewer
                reportViewer1.LocalReport.SetParameters(parameters);

                // Обновляем отчет, чтобы отобразить изменения
                reportViewer1.RefreshReport();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fams select = (Fams)comboBox2.SelectedItem;
            if (select != null)
            {
                // Создаем параметр для отчета
                ReportParameter[] parameters = new ReportParameter[]
                {
                    new ReportParameter("DO", select.dol),
                    new ReportParameter("FO", select.fam), // имя параметра, используемое в отчете
                    new ReportParameter("IO", select.im),    // Имя
                    new ReportParameter("OO", select.otch), // Отчество
                };

                // Устанавливаем параметры в ReportViewer
                reportViewer1.LocalReport.SetParameters(parameters);

                // Обновляем отчет, чтобы отобразить изменения
                reportViewer1.RefreshReport();
            }
        }

        private void новаяНакладнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            tableLayoutPanel1.Visible = true;
            textBox1.Focus();
            оформитьToolStripMenuItem.Enabled = true;
            LoadRaz();
            LoadOtp();
        }

        private void оформитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите номер приказа.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Получаем данные
            DataTable result = GetData();

            // Проверяем, были ли получены данные
            if (result == null)
            {
                // Если данных нет, просто выходим из метода
                return; // Здесь можно также показать сообщение, если это необходимо
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите разрешающего!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox1.Focus();
                return;
            }

            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Выберите отправителя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBox2.Focus();
                return;
            }

            ShowReport();

            docs = textBox2.Text;
            NDS_str = NDS.ToString();
            Nds_sum_str = Nds_sum.ToString();
            Sum_sNDS_str = Sum_sNDS.ToString();
            prop = RuPropisNumber.Propis.CurrencyPhrase(Sum_sNDS, 643);

            ReportParameter[] parameters = new ReportParameter[]
                {
                    new ReportParameter("docs", docs), // имя параметра, используемое в отчете
                    new ReportParameter("NDS_str", NDS_str),
                    new ReportParameter("Nds_sum_str", Nds_sum_str),
                    new ReportParameter("Sum_sNDS_str", Sum_sNDS_str),
                    new ReportParameter("prop", prop)
                };

            // Устанавливаем параметры в ReportViewer
            reportViewer1.LocalReport.SetParameters(parameters);

            // Устанавливает режим страницы
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            // Обновляем отчет, чтобы отобразить изменения
            reportViewer1.RefreshReport();

            tableLayoutPanel7.Visible = true;
            tableLayoutPanel1.Visible = false;
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Text = 0.ToString();
            comboBox1.Text = "";
            comboBox2.Text = "";
            tableLayoutPanel7.Visible = false;
            tableLayoutPanel1.Visible = false;
            оформитьToolStripMenuItem.Enabled = false;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
