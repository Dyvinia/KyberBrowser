﻿using System.Diagnostics;
using System.Media;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Navigation;

namespace DyviniaUtils.Dialogs {
    /// <summary>
    /// Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Window {
        private bool result = false;

        public UpdateDialog(string repoAuthor, string repoName) {
            InitializeComponent();

            Topmost = true;
            Hide();

            InstallButton.Click += OnClose;
            IgnoreButton.Click += OnClose;

            WebpageButton.Click += (_, _) => Process.Start(new ProcessStartInfo($"https://github.com/{repoAuthor}/{repoName}/releases/latest") { UseShellExecute = true });

            GetUpdateInfo(repoAuthor, repoName);

            Loaded += (_, _) => {
                Focus();
            };

            SystemSounds.Exclamation.Play();
        }

        private void GetUpdateInfo(string repoAuthor, string repoName) {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "request");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.html");
            JsonDocument github = JsonDocument.Parse(client.GetStringAsync($"https://api.github.com/repos/{repoAuthor}/{repoName}/releases/latest").Result);
            string htmlString = $"<head><style>body{{line-height: 1.25; background-color: #000000; color: rgb(230, 237, 243); font-family: -apple-system, BlinkMacSystemFont, \"Segoe UI\", \"Noto Sans\", Helvetica, Arial, sans-serif, \"Apple Color Emoji\", \"Segoe UI Emoji\"}} a{{color: rgb(220, 220, 220);}} h1, h2, h3, h4, h5, h6{{line-height: 0.125;}}</style></head><body>{github.RootElement.GetProperty("body_html").GetString()}</body>";

            Header.Text = github.RootElement.GetProperty("name").GetString();
            Browser.NavigateToString(htmlString);
            Browser.Visibility = Visibility.Visible;
        }

        public static bool Show(string repoAuthor, string repoName) {
            bool result = false;
            Application.Current.Dispatcher.Invoke(() => {
                UpdateDialog window = new(repoAuthor, repoName);
                window.ShowDialog();
                result = window.result;
            });
            return result;
        }

        private void OnClose(object sender, RoutedEventArgs e) {
            if (sender == InstallButton)
                result = true;
            else
                result = false;
            Close();
        }

        private void Browser_Navigating(object sender, NavigatingCancelEventArgs e) {
            if (e.Uri is null) return;

            if (e.Uri.ToString().StartsWith("http")) {
                e.Cancel = true;
                Process.Start(new ProcessStartInfo(e.Uri.ToString()) { UseShellExecute = true });
            }
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e) {
            int height = ((dynamic)Browser.Document).Body.scrollHeight;

            if (height < 300) {
                Browser.Height = height;
                Browser.InvokeScript("execScript", new object[] { "document.body.style.overflow ='hidden'", "JavaScript" });
            }
        }
    }
}
