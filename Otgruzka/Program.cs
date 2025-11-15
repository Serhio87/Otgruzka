using System;
using System.Windows.Forms;

namespace Otgruzka
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new enterBox());

            //вход без пароля для ADMIN
            string FIO = "ADMIN";
            string dolzhn = "ADMIN";
            Application.Run(new First(dolzhn, FIO, 1));
        }
    }
}