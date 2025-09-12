using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otgruzka
{
    public partial class Plavka: Form
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\user\\Desktop\\ДИПЛОМ\\Otgruzka\\Otgruzka\\newBD.accdb";
        private DataTable dataTable;
        private int pl;
        

        public Plavka(int plavka)
        {
            pl = plavka;
            InitializeComponent();
            LoadForm();
        }

        private void LoadForm()
        {
            // Загрузка данных из БД в DataGridView
            using (OleDbConnection conn = new OleDbConnection(connectString))
            {
                try
                {
                    string query = @"SELECT Izdelie.plavka, profil.profil, klass.marka, dlina.dlina, 
Izdelie.paket, Izdelie.ves_izd, Izdelie.kod_izd, Izdelie.price, Prodazha.kod_prod
FROM (profil INNER JOIN (klass INNER JOIN (dlina INNER JOIN Izdelie ON dlina.Код = Izdelie.dlina) ON klass.Код = Izdelie.klass) 
ON profil.Код = Izdelie.profil) LEFT JOIN Prodazha ON Izdelie.kod_izd = Prodazha.kod_izd
WHERE (((Izdelie.plavka)=?) AND ((Prodazha.kod_prod) Is Null));";

                    
        OleDbCommand command = new OleDbCommand(query, conn);
                    command.Parameters.AddWithValue("?", pl);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine($"Ошибка выполнения запроса: {ex.Message}");
                    MessageBox.Show($"Ошибка выполнения запроса: {ex.Message}");
                }
            }
            //изменение шрифта
//            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
//            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13); //заголовок
        }
    }
}
