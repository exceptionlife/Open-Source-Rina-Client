using Cyclic.Redundancy.Check;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace Rina.Client.Source_files
{
  
    class Common
    {
        public static void ChangeStatus(string Key, params string[] Arguments)
        {

            Globals.pForm.Status.Text = Texts.GetText(Key, Arguments);
        }

        public static void UpdateCompleteProgress(long Value)
        {
            if (Value < 0 || Value > 100)
                return;

            Globals.pForm.completeProgress.Value = Convert.ToInt32(Value);
            Globals.pForm.completeProgressText.Text = Texts.GetText("COMPLETEPROGRESS", Value);
        }

        public static void UpdateCurrentProgress(long Value, double Speed)
        {
            if (Value < 0 || Value > 100)
                return;

            Globals.pForm.currentProgress.Value = Convert.ToInt32(Value);
            Globals.pForm.currentProgressText.Text = Texts.GetText("CURRENTPROGRESS", Value, Speed.ToString("0.00"));
        }

        public static string GetHash(string Name)
        {
            if (Name == string.Empty)
            {
                return string.Empty;
            }
            CRC crc = new CRC();
            string text = string.Empty;
            FileStream fileStream = new FileStream(Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 16777216);
            foreach (byte b in crc.ComputeHash(fileStream))
            {
                text += b.ToString("x2").ToLower();
            }
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
            crc.Dispose();
            return text;
        }

        public static void EnableStart()
        {
            Globals.pForm.Hide();

            Client form = new Client();
            form.Show();
            form.BringToFront();
        }
    }
}
