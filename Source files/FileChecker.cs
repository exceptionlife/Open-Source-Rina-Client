using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Rina.Client.Source_files
{
    class FileChecker
    {
        enum State
        {
            REPORT_NAME     = 0,
            REPORT_PROGRESS = 1
        }

        private static BackgroundWorker backgroundWorker = new BackgroundWorker();

        public static void CheckFiles()
        {
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.DoWork             += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged    += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show(Texts.GetText("UNKNOWNERROR", "CheckFiles isBusy"));
                Application.Exit();
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Functions.LoadLocalSettings();
            Functions.CheckPath();

            foreach (Globals.File file in Globals.Files)
            {
                Globals.Fullsize += file.Size;

                backgroundWorker.ReportProgress((int)State.REPORT_NAME, Path.GetFileName(file.Name));

                if (!File.Exists(Globals.GamePath + @"\" + file.Name) || Common.GetHash(Globals.GamePath + @"\" + file.Name) != file.Hash)
                {
                    Globals.OldFiles.Add(file.Name);
                }
                else
                {
                    Globals.Completesize += file.Size;
                    backgroundWorker.ReportProgress((int)State.REPORT_PROGRESS);
                }
            }
        }

        private static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == (int)State.REPORT_NAME)
            {
                Common.ChangeStatus("CHECKFILE", e.UserState.ToString());
            }
            else
            {
                Common.UpdateCompleteProgress(Computer.Compute(Globals.Completesize));
            }
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileDownloader.DownloadFile();
        }
    }
}
