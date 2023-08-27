using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KyberBrowser.Dialogs;
using System.Text.RegularExpressions;

namespace KyberBrowser {
    /// <summary>
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window {

        private static readonly HttpClient httpClient = new();

        public HostWindow() {
            InitializeComponent();

            MouseDown += (s, e) => FocusManager.SetFocusedElement(this, this);

            GetModes();

            ModDataComboBox.ItemsSource = App.ModDataList;

            Loaded += (_, _) => {
                UpdatePingSiteComboBox();
                UpdateModDataComboBox();
            };
        }

        private void GetModes() {
            dynamic constants = JsonConvert.DeserializeObject<dynamic>(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("KyberBrowser.Resources.Constants.json")).ReadToEnd());

            List<dynamic> modes = constants.modes.ToObject<List<dynamic>>();

            ModeComboBox.ItemsSource = modes;
            ModeComboBox.SelectedIndex = 0;
        }

        private void GetMaps() {
            dynamic constants = JsonConvert.DeserializeObject<dynamic>(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("KyberBrowser.Resources.Constants.json")).ReadToEnd());

            List<dynamic> mapOverrides = ((dynamic)ModeComboBox.SelectedItem).mapOverrides?.ToObject<List<dynamic>>();

            List<dynamic> maps = constants.maps.ToObject<List<dynamic>>();


            List<string> selectedMapKeys = ((dynamic)ModeComboBox.SelectedItem).maps.ToObject<List<string>>();

            List<dynamic> selectedMaps = new();
            foreach (dynamic map in maps) {
                if (selectedMapKeys.Contains((string)map.map)) {
                    selectedMaps.Add(map);
                }
            }

            if (mapOverrides is not null) {
                foreach (dynamic selectedMap in selectedMaps) {
                    selectedMap.name = mapOverrides.FirstOrDefault(m => m.map == selectedMap.map)?.name ?? selectedMap.name;
                }
            }

            MapComboBox.ItemsSource = selectedMaps;
            MapComboBox.SelectedIndex = 0;
        }

        private void UpdatePingSiteComboBox() {
            App.GetProxies();
            PingSiteComboBox.ItemsSource = App.Proxies.Values.ToList().OrderBy(p => p.Ping);
            PingSiteComboBox.SelectedIndex = 0;
        }

        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            GetMaps(); // this code is such a mess
        }

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

        public void UpdateModDataComboBox() {
            string path = Path.Combine(Path.GetDirectoryName(Config.Settings.BF2Path) ?? "", "ModData");

            ModDataComboBox.SelectedItem = App.ModDataList.FirstOrDefault(s => s.FullName == Config.Settings.SelectedModData);

            if (ModDataComboBox.SelectedItem is null)
                ModDataComboBox.SelectedIndex = 0;
        }

        private async Task<bool> CreateServer(string name = null) {
            string serverName = name ?? NameTextBox.Text.Trim();

            Dictionary<string, object> selection = new() {
                { "autoBalanceTeams", AutoBalanceCheckbox.IsChecked == true },
                { "description", Regex.Replace(DescTextBox.Text, @"(\r\n){2,}", Environment.NewLine).Trim() },
                { "displayInBrowser", true },
                { "faction", LightFactionRadio.IsChecked == true ? 0 : 1 },
                { "kyberProxy", ((ProxyData)PingSiteComboBox.SelectedItem).IP },
                { "map", (string)((dynamic)MapComboBox.SelectedItem)?.map },
                { "maxPlayers", 40 },
                { "mode", (string)((dynamic)ModeComboBox.SelectedItem)?.mode },
                { "name", serverName },
                { "password", PassTextBox.Text }
            };

            HttpResponseMessage response = await httpClient.PostAsync("https://kyber.gg/api/config/host", new StringContent(System.Text.Json.JsonSerializer.Serialize(selection), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) {
                string message = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())?.message;

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
            DescTextBox.Text = Regex.Replace(DescTextBox.Text, @"(\r\n){2,}", Environment.NewLine).Trim();
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
