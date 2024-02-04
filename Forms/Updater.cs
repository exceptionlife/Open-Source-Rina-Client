using Rina.Client.Source_files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace Rina.Client
{
    public partial class Updater : Form
    {
        string updaterPath = null;
        public Updater()
        {

            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            if (!isElevated)
            {
                MessageBox.Show("Client yönetici olarak çalışmadığı için başlatılamadı.");
                Environment.Exit(0);
            }

            Globals.Updater = this;
            InitializeComponent();
        }

        private void Updater_Load(object sender, EventArgs e)
        {
            updaterPath = Path.GetTempPath() + "Rina_Updater.exe";
        }

        public void StartUpdate()
        {
            if (File.Exists(updaterPath))
                File.Delete(updaterPath);

            Uri uri = new Uri(Globals.DownloadUpdaterURL);

            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", Globals.AllowedUseragent);
            wc.DownloadFileAsync(uri, updaterPath);
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(updaterPath);
                startInfo.Arguments = "\"" + Process.GetCurrentProcess().MainModule.FileName + "\"";
                Process.Start(startInfo);
                Environment.Exit(0);
            }
            else
            {
                MessageBox.Show("Güncelleme indirilemiyor, lütfen bağlantınızı kontrol edin.", "İndirilemedi");
                StartUpdate();
            }
        }

        private void Updater_Shown(object sender, EventArgs e)
        {
            string externalversion = Globals.RemoteClientSettings["version"];

            if (externalversion != Globals.CurrentVersion)
            {
                StartUpdate();
            }
            else
            {
              
                Globals.Updater.Hide();
                pForm form = new pForm();
                form.Show();
            }
        }
    }

}
