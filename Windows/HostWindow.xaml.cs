using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KyberBrowser.Dialogs;
using KyberBrowser.Data;

namespace KyberBrowser {
    /// <summary>
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window {

        [GeneratedRegex("(\\r\\n){2,}")]
        private static partial Regex NewlineRegex();

        public HostWindow() {
            InitializeComponent();

            MouseDown += (s, e) => FocusManager.SetFocusedElement(this, this);

            ModeComboBox.ItemsSource = App.Modes;
            ModeComboBox.SelectedIndex = 0;

            App.GetModData();
            ModDataComboBox.ItemsSource = App.ModDataList.ToList();

            try { CheckKyberConfig(); }
            catch { }
        }

        private async void CheckKyberConfig() {
            JsonDocument configJson = JsonDocument.Parse(await App.HttpClient.GetStringAsync("https://kyber.gg/api/config/"));
            if (configJson.RootElement.GetProperty("KYBER_MODE").GetString() != "SERVER")
                return;

            JsonElement serverOptions = configJson.RootElement.GetProperty("SERVER_OPTIONS");

            AutoBalanceCheckbox.IsChecked = serverOptions.GetProperty("AUTO_BALANCE_TEAMS").GetBoolean();

            NameTextBox.Text = serverOptions.GetProperty("NAME").GetString();

            PassTextBox.Text = serverOptions.GetProperty("PASSWORD").GetString();

            DescTextBox.Text = Encoding.UTF8.GetString(Convert.FromBase64String(serverOptions.GetProperty("DESCRIPTION").GetString()));

            ModeComboBox.SelectedIndex = ((Dictionary<string, dynamic>)ModeComboBox.ItemsSource).Keys.ToList().FindIndex(m => m == serverOptions.GetProperty("MODE").GetString());

            MapComboBox.SelectedIndex = ((Dictionary<string, string>)MapComboBox.ItemsSource).Keys.ToList().FindIndex(m => m == serverOptions.GetProperty("MAP").GetString());

            MaxPlayersTextBox.Text = serverOptions.GetProperty("MAX_PLAYERS").GetInt32().ToString();
        }

        private void GetMaps() {
            if (ModeComboBox.SelectedItem is null) return;

            Dictionary<string, string> mapOverrides = ((dynamic)ModeComboBox.SelectedItem).Value.MapOverrides?.ToObject<Dictionary<string, string>>();
            string[] selectedMapKeys = ((dynamic)ModeComboBox.SelectedItem).Value.Maps.ToObject<string[]>();

            Dictionary<string, string> selectedMaps = App.Maps.Where(m => selectedMapKeys.Contains(m.Key)).ToDictionary(k => k.Key, k => k.Value) ?? new();
            foreach (KeyValuePair<string, string> selectedMap in selectedMaps)
                selectedMaps[selectedMap.Key] = mapOverrides?.FirstOrDefault(m => m.Key == selectedMap.Key).Value ?? selectedMap.Value;

            string selectedValue = String.Empty;
            if (MapComboBox.SelectedItem is not null)
                selectedValue = ((KeyValuePair<string, string>)MapComboBox.SelectedItem).Value;
            MapComboBox.ItemsSource = selectedMaps;
            MapComboBox.SelectedItem = selectedMaps.FirstOrDefault(m => m.Value == selectedValue);

            if (MapComboBox.SelectedItem is null)
                MapComboBox.SelectedIndex = 0;
        }

        public void UpdatePingSiteComboBox() {
            string selectedIP = ((ProxyData)PingSiteComboBox.SelectedItem)?.IP;
            App.GetProxies();

            IOrderedEnumerable<ProxyData> proxies = App.Proxies.Values.OrderBy(p => p.Ping);
            PingSiteComboBox.ItemsSource = proxies;
            PingSiteComboBox.SelectedItem = proxies.FirstOrDefault(s => s.IP == selectedIP);

            if (PingSiteComboBox.SelectedItem is null)
                PingSiteComboBox.SelectedIndex = 0;
        }

        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => GetMaps();

        private void MaxPlayersTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            TextBox tBox = (TextBox)sender;
            e.Handled = !int.TryParse(tBox.Text + e.Text, out int _);
        }

        private void MaxPlayersTextBox_LostFocus(object sender, RoutedEventArgs e) {
            TextBox tBox = (TextBox)sender;
            int val = int.Parse(tBox.Text);
            if (val > 64)
                tBox.Text = "64";
            else if (val < 2)
                tBox.Text = "2";
        }

        private void ModDataComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ModDataComboBox.SelectedItem is null) return;

            Dictionary<string, dynamic> selectedModes = App.Modes.ToDictionary(m => m.Key, m => m.Value.DeepClone());

            DirectoryInfo selectedModData = ModDataComboBox.SelectedItem as DirectoryInfo;
            string selectedModsJson = Path.Combine(selectedModData.FullName, "patch", "mods.json");
            if (File.Exists(selectedModsJson)) {
                // reset modes
                selectedModes = App.Modes.ToDictionary(m => m.Key, m => m.Value.DeepClone());
                ClientModData[] mods = JsonSerializer.Deserialize<ClientModData[]>(File.ReadAllText(selectedModsJson));

                Dictionary<string, string> modOverrides = App.ModOverrides.FirstOrDefault(m => mods.Any(o => o.Name.Contains(m.Key))).Value ?? new();
                foreach (var modOverride in modOverrides) {
                    if (selectedModes.TryGetValue(modOverride.Key, out dynamic value))
                        value.Name = modOverride.Value;
                }
            }

            // refresh combobox
            int selectedIndex = ModeComboBox.SelectedIndex;
            ModeComboBox.ItemsSource = selectedModes;
            ModeComboBox.SelectedIndex = selectedIndex;
        }

        public void UpdateModDataComboBox() {
            string path = Path.Combine(Path.GetDirectoryName(Config.Settings.BF2Path) ?? "", "ModData");

            ModDataComboBox.SelectedItem = App.ModDataList.FirstOrDefault(s => s.FullName == Config.Settings.SelectedModData);

            if (ModDataComboBox.SelectedItem is null)
                ModDataComboBox.SelectedIndex = 0;
        }

        private async Task<bool> CreateServer() => await CreateServer(null);

        private async Task<bool> CreateServer(string name) {
            string serverName = name ?? NameTextBox.Text.Trim();

            if (String.IsNullOrWhiteSpace(serverName)) {
                MessageBoxDialog.Show("The Server must have a name", this.Title, MessageBoxButton.OK, DialogSound.Error);
                return false;
            }

            Dictionary<string, object> selection = new() {
                { "autoBalanceTeams", AutoBalanceCheckbox.IsChecked == true },
                { "description", NewlineRegex().Replace(DescTextBox.Text, Environment.NewLine).Trim() },
                { "displayInBrowser", true },
                { "faction", LightFactionRadio.IsChecked == true ? 0 : 1 },
                { "kyberProxy", ((ProxyData)PingSiteComboBox.SelectedItem).IP },
                { "map", ((KeyValuePair<string, string>)MapComboBox.SelectedItem).Key },
                { "maxPlayers", Int32.Parse(MaxPlayersTextBox.Text) },
                { "mode", ((KeyValuePair<string, dynamic>)ModeComboBox.SelectedItem).Key },
                { "name", serverName },
                { "password", PassTextBox.Text }
            };

            HttpResponseMessage response = await App.HttpClient.PostAsync("https://kyber.gg/api/config/host", new StringContent(System.Text.Json.JsonSerializer.Serialize(selection), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) {
                string message = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("message").GetString();

                if (message == "A server with this name already exists.") {
                    return await CreateServer(serverName + " ");
                }

                MessageBoxDialog.Show("Unable To Create Server:\n" + message, this.Title, MessageBoxButton.OK, DialogSound.Error);
                return false;
            }
            return true;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e) {
            _ = CreateServer();
        }

        private async void LaunchFrostyButton_Click(object sender, RoutedEventArgs e) {
            if (await CreateServer()) {
                Process.Start(new ProcessStartInfo(Config.Settings.FrostyPath) {
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(Config.Settings.FrostyPath),
                });
            }
        }

        private async void LaunchButton_Click(object sender, RoutedEventArgs e) {
            if (await CreateServer()) {
                LaunchGame();
            }
        }

        private void LaunchGame() {
            if (ModDataComboBox.SelectedItem is null || ((DirectoryInfo)ModDataComboBox.SelectedItem).Name == "Vanilla")
                Process.Start(new ProcessStartInfo(Config.Settings.BF2Path));
            else {
                string args = $"-dataPath \"{Path.Combine("ModData", ((DirectoryInfo)ModDataComboBox.SelectedItem).Name)}\"";

                // Steam
                string steamAppIdPath = Path.Combine(Path.GetDirectoryName(Config.Settings.BF2Path), "steam_appid.txt");
                if (File.Exists(steamAppIdPath)) {
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

        private void DescTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            int charCount = DescTextBox.Text.Length;
            int charMax = DescTextBox.MaxLength;
            DescTextBoxCounter.Text = $"{charCount}/{charMax}";
        }

        private void DescTextBox_LostFocus(object sender, RoutedEventArgs e) {
            DescTextBox.Text = NewlineRegex().Replace(DescTextBox.Text, Environment.NewLine).Trim();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);

            if (e.Key == Key.F5) {
                UpdatePingSiteComboBox();
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }
    }
}
