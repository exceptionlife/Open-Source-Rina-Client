using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Rina.Client.Source_files
{
    class ListDownloader
    {
        public static void DownloadList()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            Common.ChangeStatus("LISTDOWNLOAD");

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show(Texts.GetText("UNKNOWNERROR", "DownloadList isBusy"));
                Application.Exit();
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }
        }
        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("user-agent", Globals.AllowedUseragent);

                Stream stream = webClient.OpenRead(Globals.ServerURL + Globals.PatchlistName);
                
                StreamReader streamReader = new StreamReader(stream);

                while (!streamReader.EndOfStream)
                {
                    ListProcessor.AddFile(streamReader.ReadLine());
                }
            }
            catch (WebException ex)
            {
                var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                MessageBox.Show("İndirme sunucusuna bağlanılamadı. Hata sizden veya sunucudan\nkaynaklı olabilir lütfen bir yetkiliye ulaşınız.\n\n\nHata kodu: " + statusCode, "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
                
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileChecker.CheckFiles();
        }
    }
}
