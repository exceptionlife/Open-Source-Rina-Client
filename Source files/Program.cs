using System;
using System.Windows.Forms;
using System.Threading;
using Rina.Client.Source_files;

namespace Rina.Client
{
    static class Program
    {
        private static Mutex mutex = null;

        [STAThread]
        static void Main()
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            const string appName = "RinaRP";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            
            if (!createdNew)
            {
                MessageBox.Show("Bu program zaten açık.");
                return;
            }                           
          
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
                        
            try
            {
                Application.Run(new Updater());
            }
            catch (Exception e)
            {
                MessageBox.Show("Beklenmeyen Hata: " + e);
            }
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Hata Ayıklama: " + e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
    }
}
