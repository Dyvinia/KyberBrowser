using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KyberBrowser.Dialogs;
using Microsoft.Win32;

namespace KyberBrowser {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingsWindow : Window {

        private readonly List<RadioButton> launchFixRadioButtons;

        public SettingsWindow() {
            InitializeComponent();

            Title = $"KyberBrowser {App.Version} " + (App.IsAdmin ? "[ADMIN]" : String.Empty);

            MouseDown += (s, e) => FocusManager.SetFocusedElement(this, this);

            ResetSettingsButton.Click += (s, e) => ResetSettings();
            WinDefButton.Click += (s, e) => WindowsDefenderExclusion();

            InitfsFixButton.Click += (s, e) => InstallInitfs();

            launchFixRadioButtons = new() {
                LFixNone,
                LFixSteam,
                LFixDP
            };

            foreach (RadioButton rb in launchFixRadioButtons)
                rb.Checked += (s, e) => Config.Settings.LaunchFixMethod = launchFixRadioButtons.FirstOrDefault(b => b.IsChecked == true).Content.ToString();

            Loaded += (_, _) => {
                UpdateModDataComboBox();
                UpdateLaunchFixUI();
            };

            DataContext = Config.Settings;
        }

        private static void WindowsDefenderExclusion() {
            string dllPath = Path.GetDirectoryName(App.KyberPath);
            string exePath = Assembly.GetExecutingAssembly().Location;

            ProcessStartInfo startInfo = new() {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C powershell -inputformat none -outputformat none -NonInteractive -Command \"Add-MpPreference -ExclusionPath \'" + dllPath + "'\"" + "&powershell -inputformat none -outputformat none -NonInteractive -Command \"Add-MpPreference -ExclusionPath \'" + exePath + "'\"",
                Verb = "runas",
                UseShellExecute = true
            };
            Process process = new() { StartInfo = startInfo };
            process.Start();
        }

        private void UpdateLaunchFixUI() {
            RadioButton selectedFix = launchFixRadioButtons.FirstOrDefault(b => b.Content.ToString() == Config.Settings.LaunchFixMethod);
            if (selectedFix is not null)
                selectedFix.IsChecked = true;
        }

        private void ResetSettings() {
            Config.Reset();
            DataContext = null;
            DataContext = Config.Settings;
            Config.Save();
        }

        private void RedownloadButton_Click(object sender, RoutedEventArgs e) {
            App.DownloadKyber();
            MessageBoxDialog.Show("Kyber DLL has been redownloaded", "KyberBrowser", MessageBoxButton.OK, DialogSound.Notify);
        }

        private void UpdateModDataComboBox() {
            string path = Path.Combine(Path.GetDirectoryName(Config.Settings.BF2Path) ?? "", "ModData");

            ModDataComboBox.ItemsSource = App.ModDataList.Where(m => !m.FullName.Contains("Vanilla"));

            ModDataComboBox.SelectedItem = App.ModDataList.FirstOrDefault(s => s.FullName == Config.Settings.SelectedModData);

            if (ModDataComboBox.SelectedItem is null)
                ModDataComboBox.SelectedIndex = 0;
        }

        private void InstallInitfs() {
            if (MessageBoxDialog.Show("Installing InitfsFix overwrites any custom Graphics changes\nyou may have made to initfs_Win32.\n\n[If you don't know what this means press OK]", "KyberBrowser", MessageBoxButton.OKCancel, DialogSound.Notify) == MessageBoxResult.Cancel)
                return;


            string initfsPath = Path.Combine(((DirectoryInfo)ModDataComboBox.SelectedItem).FullName, "Data", "initfs_Win32");
            if (!File.Exists(initfsPath)) {
                MessageBoxDialog.Show($"InitfsFix has not been installed\n\\{((DirectoryInfo)ModDataComboBox.SelectedItem).Name}\\Data\\initfs_Win32 file not found", "KyberBrowser", MessageBoxButton.OK, DialogSound.Error);
                return;
            }

            string initfsFixDirectory = Path.Combine(Path.GetDirectoryName(Config.FilePath), "InitfsFix");
            Directory.CreateDirectory(initfsFixDirectory);

            string initfsEditPath = Path.Combine(initfsFixDirectory, "InitfsEdit.exe");
            string graphicsLuaPath = Path.Combine(initfsFixDirectory, "Graphics.lua");
            string graphicsLuaPayloadName = "Scripts/UserOptions/Options/Graphics.lua";

            // Create InitfsEdit.exe
            using Stream s = GetType().Assembly.GetManifestResourceStream("KyberBrowser.Resources.InitfsEdit.exe");
            using FileStream f = File.OpenWrite(initfsEditPath);
            s.CopyTo(f);
            s.Close();
            f.Close();

            // Create Graphics.lua if does not exist
            if (!File.Exists(graphicsLuaPath)) {
                File.WriteAllText(graphicsLuaPath, new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("KyberBrowser.Resources.Graphics.lua")).ReadToEnd());
            }

            // run InitfsEdit on selected initfs
            ProcessStartInfo startInfo = new() {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = initfsEditPath,
                Arguments = $"\"{initfsPath}\" \"{graphicsLuaPath}\" {graphicsLuaPayloadName}",
                UseShellExecute = true
            };
            Process.Start(startInfo).WaitForExit();

            MessageBoxDialog.Show("InitfsFix has been installed", "KyberBrowser", MessageBoxButton.OK, DialogSound.Notify);

            // Only delete the exe, just incase user wants to edit Graphics.lua
            File.Delete(initfsEditPath);
        }

        private void FrostyOpenButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new() {
                Title = "Select Frosty",
                Filter = "Frosty Executable (*.exe) |*.exe",
                FilterIndex = 2,
                InitialDirectory = Path.GetDirectoryName(Config.Settings.FrostyPath),
            };
            if (dialog.ShowDialog() == true) {
                Config.Settings.FrostyPath = dialog.FileName;
                DataContext = null;
                DataContext = Config.Settings;
                Config.Save();
            }
        }

        private void GameOpenButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new() {
                Title = "Select STAR WARS Battlefront II",
                Filter = "SWBF2 Executable (*.exe) |*.exe",
                FilterIndex = 2,
                InitialDirectory = Path.GetDirectoryName(Config.Settings.BF2Path),
            };
            if (dialog.ShowDialog() == true) {
                Config.Settings.BF2Path = dialog.FileName;
                DataContext = null;
                DataContext = Config.Settings;
                Config.Save();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);

            if (e.Key == Key.F8)
                Process.Start("explorer.exe", $"/select, {App.KyberPath}");

            if (e.Key == Key.F12)
                Process.Start("explorer.exe", $"/select, {Config.FilePath}");
        }
    }
}
