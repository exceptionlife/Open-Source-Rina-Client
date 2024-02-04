using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Rina.Client.Source_files
{
    class FileDownloader
    {
        private static int      curFile;
        private static long     lastBytes;
        private static long     currentBytes;

        private static Stopwatch stopWatch = new Stopwatch();

        public static void DownloadFile()
        {            
            if (Globals.OldFiles.Count <= 0)
            {
                Common.ChangeStatus("CHECKCOMPLETE");
                Common.EnableStart();     
                return;
            }

            if (curFile >= Globals.OldFiles.Count)
            {
                Common.ChangeStatus("DOWNLOADCOMPLETE");
                Common.EnableStart();
                return;
            }

            Functions.LoadLocalSettings();
            Functions.CheckPath();

            if (Globals.OldFiles[curFile].Contains("/"))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Globals.GamePath+ @"\" + Globals.OldFiles[curFile]));
            }

            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", Globals.AllowedUseragent);
            webClient.DownloadProgressChanged   += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted     += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            stopWatch.Start();            
            webClient.DownloadFileAsync(new Uri(Globals.ServerURL + Globals.OldFiles[curFile]), Globals.GamePath + @"\"+  Globals.OldFiles[curFile]);
        }

        private static void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            currentBytes = lastBytes + e.BytesReceived;
            Common.ChangeStatus("DOWNLOADFILE", Globals.OldFiles[curFile], Computer.ComputeDownloadSize(e.BytesReceived).ToString("0.00") + " MB ", Computer.ComputeDownloadSize(e.TotalBytesToReceive).ToString("0.00") + " MB");
            Common.UpdateCompleteProgress(Computer.Compute(Globals.Completesize + currentBytes));
            Common.UpdateCurrentProgress(e.ProgressPercentage, Computer.ComputeDownloadSpeed(e.BytesReceived, stopWatch));
        }

        private static void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            lastBytes = currentBytes;
            Common.UpdateCurrentProgress(100, 0);
            curFile++;
            stopWatch.Reset();
            DownloadFile();
        }
    }
}
