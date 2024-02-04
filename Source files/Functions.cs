using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Management;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace Rina.Client.Source_files
{
    public class MessageBoxWrapper
    {
        public static bool IsOpen { get; set; }

        public static Thread thr;
        public static void Show(string messageBoxText, string caption)
        {
            if (!IsOpen)
            {
                Globals.Client.ClientiGoster();
                Globals.Client.BringToFront();

                IsOpen = true;

                thr = new Thread(() =>
                    {
                        DialogResult result = MessageBox.Show(messageBoxText, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            
                        if (result == DialogResult.OK)
                        {   
                            IsOpen = false;
                            thr.Abort();
                        }
                    }
                );

                thr.Start();
            }
        }
    }

    public class Functions
    {
        public static Thread ccDelete;
        public static FolderBrowserDialog Klasor = new FolderBrowserDialog();

        private static readonly Random getrandomsym = new Random();
        private static readonly object syncLock = new object();

        public static Dictionary<string, string> GetRemoteSettings()
        {
            Dictionary<string, string> settingsDict = new Dictionary<string, string>();

            string url = "http://www.rina-roleplay.com/client/sampclient.conf";

            try
            {
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(url + "?" + Functions.GetRandomSymbols(8));
                StreamReader streamReader = new StreamReader(stream);

                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    settingsDict.Add(line.Split('=')[0], line.Split('=')[1]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Client bilgileri yüklenirken bir bağlantı problemi oluştu.", "Version Instance Controller");
                Application.Exit();
            }
            return settingsDict;
        }

        public static string ReadTextFromUrl(string url)
        {
            var result = string.Empty;
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = cookieContainer;
            req.UserAgent = Globals.AllowedUseragent ;
       

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(res.CharacterSet));
            result = sr.ReadToEnd();
            return result;
        }

        public static void AppendProcessID(string key)
        {
            try {
                var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

                var GAMEYR = view64.OpenSubKey("Software\\WOW6432Node\\Microsoft\\ASP.NET", true);

                if (GAMEYR == null)
                {
                    GAMEYR = view64.CreateSubKey("Software\\WOW6432Node\\Microsoft\\ASP.NET");
                }

                DateTime dt = DateTime.Now;
                int asilsayi = (dt.Day * 2) + (dt.Month * 2) + (dt.Hour * 4) + 14;
                Random _random = new Random();
                for (int i = 0; i < 200; i++)
                {
                    if (i == asilsayi)
                    {
                        GAMEYR.SetValue("RootKey" + i, UtilityHash(key) );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void PutProcessID(int gtapid)
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

            var GAMEYR = view64.OpenSubKey("Software\\WOW6432Node\\Microsoft\\ASP.NET", true);
            if (GAMEYR == null)
            {
                GAMEYR = view64.CreateSubKey("Software\\WOW6432Node\\Microsoft\\ASP.NET");
            }

            string za = "";

            if (Globals.ENBActivated)
                za = UtilityHash(gtapid.ToString());
            else
                za = UtilityHash("0");

            GAMEYR.SetValue("MainKeyF", UtilityHash(gtapid.ToString()), RegistryValueKind.String);
            GAMEYR.SetValue("MainKeyH", za, RegistryValueKind.String);
        }

        public static string Base64Decode(string sifreliMetin)
        {
            byte[] bytes = Convert.FromBase64String(sifreliMetin);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ToBase64(string input)
        {
            string result = "";
            try
            {
                result = Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
            }
            catch
            {
            }
            return result;
        }

        public static string UtilityHash(string text, bool encrypt = true)
        {
            string key = "ml";

            if (encrypt)
            {
                var result = new StringBuilder();
                for (int c = 0; c < text.Length; c++)
                    result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

                return ToBase64(result.ToString());
            }
            else
            {
                text = Base64Decode(text);

                var result = new StringBuilder();
                for (int c = 0; c < text.Length; c++)
                    result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

                return result.ToString();
            }
        }

        public static void SetRegeditName(string name)
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var reg = view64.OpenSubKey("Software\\Microsoft\\gameyr", true);
            if (reg == null)
            {
                reg = view64.CreateSubKey("Software\\Microsoft\\gameyr");
            }
            reg.SetValue("name", name);
        }

        public static void SetDiscordHide(int hide)
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

            var reg = view64.OpenSubKey("Software\\Microsoft\\gameyr", true);
            if (reg == null)
            {
                reg = view64.CreateSubKey("Software\\Microsoft\\gameyr");
            }

            Globals.HideDiscordName = hide;
            reg.SetValue("discordrpchidename", hide);
        }

        public static void SetChatLogStatus(int hide)
        {
            var view64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var reg = view64.OpenSubKey("Software\\Microsoft\\gameyr", true);
            if (reg == null)
            {
                reg = view64.CreateSubKey("Software\\Microsoft\\gameyr");
            }

            Globals.ActiveChatLogs = hide;
            reg.SetValue("activechatlogs", hide);
        }

        public static string GetIPAddress()
        {
            string ipad = ReadTextFromUrl(Globals.GetIPUrl);
            return ipad;
        }

        public static string GetRandomSymbols(int count = 8)
        {
            string text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] array = new char[count];
            for (int i = 0; i < array.Length; i++)
            {
                object obj = Functions.syncLock;
                lock (obj)
                {
                    array[i] = text[Functions.getrandomsym.Next(text.Length)];
                }
            }
            return new string(array);
        }

        public bool UrlIsValid(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) //Good requests
                {
                    return true;
                }
                else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                {

                    return false;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                {
                    return false;
                }

            }

            return false;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static int GetParentProcessID(int pid)
        {
            try
            {
                using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = " + pid.ToString()))
                {
                    using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
                    {
                        ManagementObject managementObject = managementObjectCollection.Cast<ManagementObject>().FirstOrDefault<ManagementObject>();
                        if (managementObject != null)
                        {
                            return (int)managementObject["ParentProcessId"];
                        }
                    }
                }
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public static string GetComputerName()
        {
            return Environment.MachineName;
        }

        public static string GetOsName()
        {
            return Environment.OSVersion.VersionString;
        }

        public static void LogWrite(string text)
        {
            try
            {
                using (StreamWriter w = File.AppendText(Globals.RootDrive + @"Rina_Client.log"))
                {
                    w.WriteLine("["+ DateTime.Now.ToString("yyyy-MM-dd.HH:mm:ss")  +"] " + text);
                   
                }

                Functions.PutDebugText(text);
            }
            catch
            {
            }
        }

        public static void PutDebugText(string text)
        {
            try
            {
                if (Globals.Client.listBox1.Items.Count >= 100)
                {
                    Globals.Client.listBox1.Items.Clear();
                    Globals.Client.listBox1.Items.Add("[" + DateTime.Now.ToString("yyyy-MM-dd.HH:mm:ss") + "] Liste otomatik olarak temizlendi.");
                }

                Globals.Client.listBox1.Items.Add("[" + DateTime.Now.ToString("yyyy-MM-dd.HH:mm:ss") + "] " + text);
            }
            catch
            {
            }
        }

        public static bool ProcIsRunning(string process)
        {
            foreach (Process p in Process.GetProcesses())
                if (p.ProcessName == process)
                    return true;

            return false;

        }
                
        public static int GameClose(int code)
        {
            Process[] processList = Process.GetProcesses(".");
            foreach (Process p in processList)
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        if (p.ProcessName == "gta_sa" || p.MainWindowTitle.Contains("SA-MP 0.3") || p.MainWindowTitle.Contains("GTA: San Andreas") || p.MainWindowTitle.Contains("GTA:SA:MP") || p.MainWindowTitle.Contains("GTA Attention") )
                        {
                            Globals.ENBActivated = false;

                            try { p.Kill(); } catch { }
                        }
                    }
                }
                catch { }
            }

            ccDelete = new Thread(() =>
            {
                RemoveClientSession(code);
                ccDelete.Abort();
            });
            ccDelete.Start();
            return 1;
        }
       
        public static int RemoveClientSession(int code)
        {
            Globals.GameStartedByUser = false;

            try
            {
                Thread thr = new Thread(() =>
                {
                    Functions.ReadTextFromUrl(string.Concat(new string[]
                    {
                        "http://",
                        Globals.RemoteClientSettings["webserverip"],
                        "/data/se.php?loginkey=",
                        Globals.LoginKey,
                        "&ip=",
                        Functions.GetIPAddress(),
                        "&code=",
                        code.ToString()
                    }));
                });
                thr.Start();
            }
            catch
            {
                
            }
            return 1;
        }

        public static string GenerateNumber()
        {
            Random random = new Random();
            string text = "";
            for (int i = 1; i < 20; i++)
            {
                text += random.Next(0, 9).ToString();
            }
            return text;
        }


        public static void LoadLocalSettings()
        {
            try
            {
                using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\\\SAMP", true))
                {
                    if (registryKey != null)
                    {
                        object value = registryKey.GetValue("gta_sa_exe");
                        if (value != null)
                        {
                            Globals.GamePath = value.ToString();
                        }
                        else
                        {
                            registryKey.SetValue("gta_sa_exe", "NULL");
                            registryKey.SetValue("PlayerName", "NULL");
                        }
                        Globals.GamePath = Globals.GamePath.Replace("\\gta_sa.exe", "");
                        RegistryKey registryKey2 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        RegistryKey registryKey3 = registryKey2.OpenSubKey("Software\\Microsoft\\gameyr", true);
                        if (registryKey3 == null)
                        {
                            registryKey3 = registryKey2.CreateSubKey("Software\\Microsoft\\gameyr");
                        }
                        if (registryKey3.GetValue("serial") == null)
                        {
                            registryKey3.SetValue("serial", GenerateNumber());
                        }
                        else
                        {
                            Globals.RegID = registryKey3.GetValue("serial").ToString();
                        }
                        if (registryKey3.GetValue("discordrpchidename") == null)
                        {
                            registryKey3.SetValue("discordrpchidename", 0);
                        }
                        else
                        {
                            Globals.HideDiscordName = Convert.ToInt32(registryKey3.GetValue("discordrpchidename"));
                        }
                        if (registryKey3.GetValue("activechatlogs") == null)
                        {
                            registryKey3.SetValue("activechatlogs", 0);
                        }
                        else
                        {
                            Globals.ActiveChatLogs = Convert.ToInt32(registryKey3.GetValue("activechatlogs"));
                        }
                        if (registryKey3.GetValue("file") == null)
                        {
                            registryKey3.SetValue("file", Globals.GamePath);
                        }
                        if (registryKey3.GetValue("name") == null)
                        {
                            Globals.SavedName = "xxx";
                        }
                        else
                        {
                            Globals.SavedName = (string)registryKey3.GetValue("name");
                        }
                        if (registryKey2.OpenSubKey("Software\\WOW6432Node\\Microsoft\\ASP.NET", true) == null)
                        {
                            registryKey3 = registryKey2.CreateSubKey("Software\\WOW6432Node\\Microsoft\\ASP.NET");
                        }
                    }
                    else
                    {
                        Registry.CurrentUser.CreateSubKey("Software\\\\SAMP");
                        LoadLocalSettings();
                    }
                }
            }
            catch
            {
            }
        }
        
        public static void CheckPath()
        {
            if (Directory.Exists(Globals.GamePath) == false)
            {
                Klasor.Description = "GTA-SA'nın kurulu olduğu klasörü seçiniz;";
                Klasor.ShowDialog();
                Globals.GamePath = Klasor.SelectedPath;

                RegistryKey rkTest = Registry.CurrentUser.OpenSubKey(@"Software\SAMP\", true);
                rkTest.SetValue("gta_sa_exe", Klasor.SelectedPath + @"\gta_sa.exe");

                CheckPath();
            }
            else if (File.Exists(Globals.GamePath + "/samp.exe") == false)
            {
                Klasor.Description = "SA-MP'nın kurulu olduğu klasörü seçiniz;";
                Klasor.ShowDialog();
                Globals.GamePath = Klasor.SelectedPath;
                
                RegistryKey rkTest = Registry.CurrentUser.OpenSubKey(@"Software\SAMP\", true);
                rkTest.SetValue("gta_sa_exe", Klasor.SelectedPath + @"\gta_sa.exe");
                
                CheckPath();
            }
           
            else if (File.Exists(Globals.GamePath + "/gta_sa.exe") == false)
            {

                Klasor.Description = "GTA-SA'nın kurulu olduğu klasörü seçiniz;";
                Klasor.ShowDialog();

                Globals.GamePath = Klasor.SelectedPath;
                

                RegistryKey rkTest = Registry.CurrentUser.OpenSubKey(@"Software\SAMP\", true);
                rkTest.SetValue("gta_sa_exe", Klasor.SelectedPath + @"\gta_sa.exe");

                CheckPath();

            }
            else
            {

            }
        }       
    }
}
