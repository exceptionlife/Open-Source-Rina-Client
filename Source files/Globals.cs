using System;
using System.Collections.Generic;
using System.IO;
using DiscordRPC;

namespace Rina.Client.Source_files
{
    internal class Globals
    {
        public static Dictionary<string, string> RemoteClientSettings = Functions.GetRemoteSettings();

        public static string AllowedUseragent = "Keep-Delived";
        public static string ServerURL = "http://" + Globals.RemoteClientSettings["webserverip"] + "/data/";
        public static string PatchlistName = "patchlist.txt";
        public static string LoginKey = "";
        public static int AliveUpdateCount = 0;
        public static string LastName = "";
        public static string GetIPUrl = "http://" + Globals.RemoteClientSettings["webserverip"] + "/data/getip.php";
        public static string TimeURL = "http://" + Globals.RemoteClientSettings["webserverip"] + "/data/timestamp.php";
        public static string PostUpdateURL = "http://" + Globals.RemoteClientSettings["webserverip"] + "/data/r_alive.php";
        public static string DownloadUpdaterURL = "http://" + Globals.RemoteClientSettings["webserverip"] + "/data/Rina_Updater.exe";
        public static string CurrentVersion = "2024v1.0";

        public static pForm pForm;
        public static Client Client;
        public static Updater Updater;

        public static List<Globals.File> Files = new List<Globals.File>();

        public static List<string> OldFiles = new List<string>();

        public static long GTAProcessID = 0;
        public static int RealProcessID = 0;
        public static long PlayTime = 0;
        public static int ActiveChatLogs = 0;
        public static long Fullsize;
        public static long Completesize;
        public static int DiscordStateMode = -1;
        public static int HideDiscordName = 0;
        public static Timestamps DiscordPlayTime;
        public static bool BlacklistSuccess = false;

        public static string RootDrive = Path.GetPathRoot(Environment.SystemDirectory);
        public static string WindowsPath = Globals.RootDrive + "Windows\\System32";

        public static bool GameStartedByUser = false;
        public static int FindGameRetryCount = 0;
        public static bool ENBActivated = false;
        public static string GamePath = "";
        public static string SavedName = "";
        public static string RegID = "";

        public class ProgramData
        {
            public string ProcessName { get; set; }
            public string Path { get; set; }
        }

        public struct File
        {
            public string Name;
            public string SHA;
            public string Hash;
            public long Size;
        }
    }
}
