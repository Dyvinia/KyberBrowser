using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Principal;
using System.Net.Http;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using KyberBrowser.Dialogs;
using DyviniaUtils;

namespace KyberBrowser {

    [GlobalConfig]
    public class Config : SettingsManager<Config> {
        public bool UpdateChecker { get; set; } = true;
        public bool RunAsAdmin { get; set; } = true;
        public bool ShowTagsInBrowser { get; set; } = true;

        public string KyberChannel { get; set; } = "stable";
        public string BF2Path { get; set; } = String.Empty;
        public string FrostyPath { get; set; } = String.Empty;
        public string LaunchFixMethod { get; set; } = "SteamFix";

        public string SelectedModData { get; set; } = "Vanilla";
    }


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public static readonly string Version = "v" + Assembly.GetExecutingAssembly().GetName()?.Version?.ToString()[..5];

        public static readonly bool IsAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        public static readonly string KyberPath = Path.Combine(Path.GetDirectoryName(Config.FilePath), "Kyber.dll");

        public static readonly ObservableCollection<DirectoryInfo> ModDataList = new();

        public static readonly HttpClient HttpClient = new();

        public static Dictionary<string, ProxyData> Proxies { get; private set; } = new();


        public App() {
            Config.Load();

            DispatcherUnhandledException += Application_DispatcherUnhandledException;

            DownloadKyber();
            GetProxies();
        }

        protected override async void OnStartup(StartupEventArgs e) {
            if (Config.Settings.RunAsAdmin && !IsAdmin) {
                Process.Start(new ProcessStartInfo { 
                    FileName = Environment.ProcessPath, 
                    UseShellExecute = true, 
                    Verb = "runas" 
                });
                Config.Save();
                Environment.Exit(0);
            }
            else {
                foreach (Process existingProcess in Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
                    if (existingProcess.Id != Environment.ProcessId)
                        existingProcess.Kill();
            }

            MainWindow window = new();
            window.Show();

            if (!File.Exists(Config.Settings.BF2Path)) {
                if (!FindBattlefrontPath()) {
                    _ = Current.Dispatcher.BeginInvoke(() => {
                        MessageBoxDialog.Show("Unable To Locate STAR WARS Battlefront II", "KyberBrowser", MessageBoxButton.OK, DialogSound.Error);
                        new SettingsWindow().ShowDialog();
                        Config.Save();
                    });
                }
            }

            if (Config.Settings.UpdateChecker)
                await GitHub.CheckAndInstall("Dyvinia", "KyberBrowser");
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            e.Handled = true;
            ExceptionDialog.Show(e.Exception, "KyberBrowser", true);
        }

        public static async void DownloadKyber() {
            try {
                string hash = File.Exists(KyberPath) ? SHA256CheckSum(KyberPath) : "";
                string serverHash = await HttpClient.GetStringAsync($"https://kyber.gg/api/hashes/distributions/{Config.Settings.KyberChannel}/dll");
                if (hash != serverHash) {
                    await Downloader.DownloadFile($"https://kyber.gg/api/downloads/distributions/{Config.Settings.KyberChannel}/dll", KyberPath);
                }
            }
            catch (Exception e) {
                ExceptionDialog.Show(e, "KyberBrowser", false, "Unable to download Kyber:");
                Config.Settings.KyberChannel = "stable";
                Config.Save();
            }
        }

        private static bool FindBattlefrontPath() {
            string regPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\EA Games\STAR WARS Battlefront II")?.GetValue("Install Dir")?.ToString();
            regPath ??= Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EA Games\STAR WARS Battlefront II")?.GetValue("Install Dir")?.ToString();

            if (regPath is not null && File.Exists(Path.Combine(regPath, "starwarsbattlefrontii.exe"))) {
                Config.Settings.BF2Path = Path.Combine(regPath, "starwarsbattlefrontii.exe");
                Config.Save();
                return true;
            }
            return false;
        }

        public static void GetModData() {
            string path = Path.Combine(Path.GetDirectoryName(Config.Settings.BF2Path) ?? "", "ModData");
            ModDataList.Clear();

            ModDataList.Add(new("Vanilla"));
            if (Directory.Exists(path))
                foreach (string dir in Directory.GetDirectories(path))
                    ModDataList.Add(new(dir));
        }

        public static async void GetProxies() {
            ProxyData[] proxies = JsonSerializer.Deserialize<ProxyData[]>(await HttpClient.GetStringAsync("https://kyber.gg/api/proxies"));

            Parallel.ForEach(proxies, proxy => {
                PingReply ping = new Ping().Send(proxy.IP, 1500);

                if (ping.Status == IPStatus.Success) {
                    proxy.Ping = ping.RoundtripTime / 2;
                }
                else {
                    proxy.Ping = 999;
                }
            });

            Proxies = proxies.ToDictionary(p => p.IP, p => p);
        }

        private static string ToHex(byte[] bytes, bool upperCase) {
            StringBuilder result = new(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            return result.ToString();
        }

        private static string SHA256CheckSum(string filePath) {
            using FileStream fileStream = File.OpenRead(filePath);
            return ToHex(SHA256.Create().ComputeHash(fileStream), false);
        }
    }
}
