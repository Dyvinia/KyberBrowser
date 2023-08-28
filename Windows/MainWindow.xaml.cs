using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KyberBrowser.Dialogs;

namespace KyberBrowser {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly HostWindow hostWindow = new();

        public MainWindow() {
            InitializeComponent();

            MouseDown += (s, e) => FocusManager.SetFocusedElement(this, this);

            ModDataComboBox.ItemsSource = App.ModDataList;
            ModDataComboBox.SelectionChanged += (_, _) => {
                Config.Settings.SelectedModData = ((DirectoryInfo)ModDataComboBox.SelectedItem)?.FullName ?? Config.Settings.SelectedModData;
            };

            _ = GetServersAsync();
            UpdateModDataComboBox();

            StartInjectionPolling();
            StartRefreshLoop();
        }

        private async Task GetServersAsync() {
            JsonDocument serversJson = JsonDocument.Parse(await App.HttpClient.GetStringAsync("https://kyber.gg/api/servers"));
            int pageCount = serversJson.RootElement.GetProperty("pageCount").GetInt32();

            List<ServerData> servers = new();
            for (int i = 1; i <= pageCount; i++) {
                JsonDocument serverPageJson = JsonDocument.Parse(await App.HttpClient.GetStringAsync($"https://kyber.gg/api/servers?page={i}"));
                ServerData[] serverPage = JsonSerializer.Deserialize<ServerData[]>(serverPageJson.RootElement.GetProperty("servers"));
                servers.AddRange(serverPage);
            }

            ServerListBox.ItemsSource = servers;
        }

        private void UpdateModDataComboBox() {
            string path = Path.Combine(Path.GetDirectoryName(Config.Settings.BF2Path) ?? "", "ModData");

            Config.Settings.SelectedModData = ((DirectoryInfo)ModDataComboBox.SelectedItem)?.FullName ?? Config.Settings.SelectedModData;

            App.GetModData();

            ModDataComboBox.SelectedItem = App.ModDataList.FirstOrDefault(s => s.FullName == Config.Settings.SelectedModData);

            if (ModDataComboBox.SelectedItem is null)
                ModDataComboBox.SelectedIndex = 0;
        }

        private void ServerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ServerData server = (ServerData)ServerListBox.SelectedItem;

            if (server is null) {
                ServerInfoPane.Visibility = Visibility.Collapsed;
                EmptyInfoPane.Visibility = Visibility.Visible;
                return;
            }

            ServerInfoPane.Visibility = Visibility.Visible;
            EmptyInfoPane.Visibility = Visibility.Collapsed;

            InfoName.Text = server.Title;
            InfoName.ToolTip = server.Title;

            if (server.Official)
                InfoHosted.Text = $"Official Server";
            else
                InfoHosted.Text = $"Hosted by {server.Host}";

            InfoTime.Text = server.StartedAt.Replace(" ago", "");
            InfoTime.ToolTip = "Server started " + server.StartedAt;

            InfoDescription.Text = server.Description.Trim();

            InfoPasswordSection.Visibility = server.LockedVis;

            if (String.IsNullOrEmpty(InfoDescription.Text))
                InfoDescription.Visibility = Visibility.Collapsed;
            else
                InfoDescription.Visibility = Visibility.Visible;

            if (server.Tags.Count > 0) {
                InfoTags.Visibility = Visibility.Visible;
                InfoTags.ItemsSource = server.Tags;
            }
            else
                InfoTags.Visibility = Visibility.Collapsed;

            if (InfoDescription.Visibility == Visibility.Visible || InfoTags.Visibility == Visibility.Visible) {
                DescrSection.Visibility = Visibility.Visible;
            }
            else
                DescrSection.Visibility = Visibility.Collapsed;

            if (InfoDescription.Visibility == Visibility.Visible && InfoTags.Visibility == Visibility.Visible) {
                InfoDescTagSeparator.Visibility = Visibility.Visible;
            }
            else
                InfoDescTagSeparator.Visibility = Visibility.Collapsed;

            if (server.Mods.Length > 0) {
                InfoReqModSection.Visibility = Visibility.Visible;
                InfoRequiredMods.ItemsSource = server.Mods;
            }
            else {
                InfoReqModSection.Visibility = Visibility.Collapsed;
            }

            if (server.AutoBalance)
                FactionSelectSection.Visibility = Visibility.Collapsed;
            else
                FactionSelectSection.Visibility = Visibility.Visible;

            if (File.Exists(Config.Settings.FrostyPath))
                LaunchFrostyButton.IsEnabled = true;
            else
                LaunchFrostyButton.IsEnabled = false;
        }

        private async Task<bool> SelectServer() {
            ServerData server = (ServerData)ServerListBox.SelectedItem;

            Dictionary<string, object> selection = new() {
                { "id", server.ID },
                { "faction", LightFactionRadio.IsChecked == true ? 0 : 1 },
                { "password", PasswordTextBox.Text }
            };

            HttpResponseMessage response = await App.HttpClient.PostAsync("https://kyber.gg/api/config/play", new StringContent(JsonSerializer.Serialize(selection), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) {
                string message = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())?.message;
                MessageBoxDialog.Show("Unable To Select Server:\n" + message, this.Title, MessageBoxButton.OK, DialogSound.Error);
                return false;
            }
            return true;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e) {
            _ = SelectServer();
        }
        
        private async void LaunchFrostyButton_Click(object sender, RoutedEventArgs e) {
            if (await SelectServer()) {
                Process.Start(new ProcessStartInfo(Config.Settings.FrostyPath) {
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(Config.Settings.FrostyPath),
                });
            }
        }

        private async void LaunchButton_Click(object sender, RoutedEventArgs e) {
            if (await SelectServer()) {
                LaunchGame();
            }
        }

        private void LaunchGame() {
            if (ModDataComboBox.SelectedItem is null || ((DirectoryInfo)ModDataComboBox.SelectedItem).Name == "Vanilla")
                Process.Start(new ProcessStartInfo(Config.Settings.BF2Path));
            else {
                string args = $"-dataPath \"{Path.Combine("ModData", ((DirectoryInfo)ModDataComboBox.SelectedItem).Name)}\"";

                // Steam
                string steamAppIdPath = Path.Combine(Path.GetDirectoryName(Config.Settings.BF2Path) ?? "", "steam_appid.txt");
                if (File.Exists(steamAppIdPath) && Config.Settings.UseSteamFix) {
                    string steamAppId = File.ReadAllLines(steamAppIdPath).First();
                    string url = Uri.EscapeDataString(args);
                    Process.Start(new ProcessStartInfo($"steam://run/{steamAppId}//{url}/") { UseShellExecute = true });
                }
                // EAD/Origin
                else {
                    Process.Start(new ProcessStartInfo(Config.Settings.BF2Path, args));
                }
            }
        }

        private async void RefreshServerList() {
            // Remember selected server
            string selectedID = ((ServerData)ServerListBox.SelectedItem)?.ID;

            await GetServersAsync();
            UpdateModDataComboBox();

            ServerListBox.SelectedItem = ((List<ServerData>)ServerListBox.ItemsSource).FirstOrDefault(s => s.ID == selectedID);
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            string selectedID = ((ServerData)ServerListBox.SelectedItem)?.ID;

            ServerListBox.Items.Filter = o => { return ((ServerData)o).Name.ToLower().Contains(FilterTextBox.Text.ToLower()); };

            ServerListBox.SelectedItem = ((List<ServerData>)ServerListBox.ItemsSource).FirstOrDefault(s => s.ID == selectedID);
        }

        private void FilterClearButton_Click(object sender, RoutedEventArgs e) {
            FilterTextBox.Text = String.Empty;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) {
            RefreshServerList();
        }

        private void ReqModItem_Click(object sender, RoutedEventArgs e) {
            ServerData.ModData mod = ((FrameworkElement)sender).DataContext as ServerData.ModData;

            if (Uri.IsWellFormedUriString(mod.LinkVerified, UriKind.Absolute)) {
                Process.Start(new ProcessStartInfo(mod.Link) { UseShellExecute = true });
            }
        }

        private void TagItem_Click(object sender, RoutedEventArgs e) {
            string tag = ((FrameworkElement)sender).DataContext as string;

            FilterTextBox.Text = tag;
        }

        public async void StartRefreshLoop() {
            while (true) {
                await Task.Delay(TimeSpan.FromMinutes(5));
                RefreshServerList();
            }
        }

        public async void StartInjectionPolling() {
            List<int> injectedTo = new();
            while (true) {
                Process[] game = Process.GetProcessesByName("starwarsbattlefrontii");
                for (int i = 0; i < injectedTo.Count; i++) {
                    bool found = false;
                    for (int j = 0; j < game.Length; j++) {
                        if (game[j].Id == injectedTo[i]) {
                            found = true;
                            _ = Dispatcher.BeginInvoke(() => Injected.Visibility = Visibility.Visible);
                        }
                    }
                    if (!found) {
                        injectedTo.RemoveAt(i);
                        _ = Dispatcher.BeginInvoke(() => Injected.Visibility = Visibility.Hidden);
                    }
                }
                if (game.Length == 0) {
                    await Task.Delay(250);
                    continue;
                }
                if (injectedTo.Count > 0) {
                    await Task.Delay(2000);
                    continue;
                }
                DLLInjector.Inject(game.ElementAt(0).Id, App.KyberPath);
                injectedTo.Add(game.ElementAt(0).Id);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            SettingsWindow settingsWindow = new();
            settingsWindow.ShowDialog();
            Config.Save();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            LaunchGame();
        }

        private void HostButton_Click(object sender, RoutedEventArgs e) {
            hostWindow.Show();
            hostWindow.Focus();
            hostWindow.UpdateModDataComboBox();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);

            if (e.Key == Key.F5) {
                RefreshServerList();
            }

            if (e.Key == Key.F12) {
                SettingsWindow settingsWindow = new();
                settingsWindow.ShowDialog();
                Config.Save();
            }
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Config.Save();
            Application.Current.Shutdown();
        }
    }
}
