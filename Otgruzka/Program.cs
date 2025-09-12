using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        //    Application.Run(new enterBox());

                //вход без пароля мастером
            string FIO = "Мастер На Все Руки";
            string dolzhn = "Мастер";
            Application.Run(new First(dolzhn, FIO, 9));
        }
    }
}