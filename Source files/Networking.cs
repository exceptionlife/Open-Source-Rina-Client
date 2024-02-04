using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace Rina.Client.Source_files
{
    internal class Networking
    {
        public static void CheckNetwork()
        {
            Common.ChangeStatus("CONNECTING", Array.Empty<string>());
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += Networking.backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += Networking.backgroundWorker_RunWorkerCompleted;
            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show(Texts.GetText("UNKNOWNERROR", new object[]
                {
                    "CheckNetwork isBusy"
                }));
                Application.Exit();
                return;
            }
            backgroundWorker.RunWorkerAsync();
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                new WebClient
                {
                    Headers =
                    {
                        {
                            "user-agent",
                            Globals.AllowedUseragent
                        }
                    }
                }.OpenRead(Globals.ServerURL);
                e.Result = true;
            }
            catch
            {
                e.Result = false;
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!Convert.ToBoolean(e.Result))
            {
                MessageBox.Show(Texts.GetText("NONETWORK", Array.Empty<object>()));
                Application.Exit();
                return;
            }

            ListDownloader.DownloadList();
        }
    }
}
