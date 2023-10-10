using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows;

namespace KyberBrowser {
    public class ServerData {
        public string Title {
            get {
                string title = Name;

                foreach (Match match in Regex.Matches(Name, @"\[(.*?)\]").Cast<Match>())
                    title = title.Replace(match.Value, "");

                if (String.IsNullOrWhiteSpace(title))
                    title = "Server";

                return title.Trim();
            }
        }

        public List<string> Tags {
            get {
                return Regex.Matches(Name, @"\[(.*?)\]").Select(m => m.Value.Replace("[", "").Replace("]", "").Trim()).ToList();
            }
        }

        public string TitleTags { 
            get {
                string[] tags = Regex.Matches(Name, @"\[(.*?)\]").Select(m => m.Value).ToArray();
                if (tags.Length > 0 && Config.Settings.ShowTagsInBrowser)
                    return String.Join(' ', tags) + " ";
                else return String.Empty;
            }
        }

        public string Subtitle {
            get {
                if (!App.Maps.TryGetValue(Map, out string mapName))
                    mapName = "Custom Mode";

                string modeName = "Custom Mode";
                if (App.Modes.TryGetValue(Mode, out dynamic mode))
                    modeName = mode.Name;

                Dictionary<string, string> modOverrides = App.ModOverrides.FirstOrDefault(m => Mods.Any(o => o.Name.Contains(m.Key))).Value ?? new();
                if (modOverrides.TryGetValue(Mode, out string moddedMode))
                    modeName = moddedMode;

                if (Official)
                    return $"{modeName} - {mapName}";
                else
                    return $"{modeName} - {mapName} [{Host}]";
            }
        }

        public string Thumbnail {
            get {
                string thumbUrl = $"https://kyber.gg/static/images/maps/{Map.Replace('/', '-')}.jpg";
                HttpResponseMessage response = App.HttpClient.GetAsync(thumbUrl).Result;
                if (response.Content.Headers.ContentType.ToString().StartsWith("image/"))
                    return thumbUrl;
                else return "https://kyber.gg/bg.png";
            }
        }

        public string PlayerCounter {
            get {
                return $"{Players}/{MaxPlayers}";
            }
        }


        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }


        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("map")]
        public string Map { get; set; }


        [JsonPropertyName("users")]
        public int Players { get; set; }

        [JsonPropertyName("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonPropertyName("autoBalanceTeams")]
        public bool AutoBalance { get; set; }


        [JsonPropertyName("startedAtPretty")]
        public string StartedAt { get; set; }

        [JsonPropertyName("requiresPassword")]
        public bool Locked { get; set; } = false;

        public Visibility LockedVis => Locked ? Visibility.Visible : Visibility.Collapsed;

        [JsonPropertyName("official")]
        public bool Official { get; set; }

        public Visibility OfficialVis => Official ? Visibility.Visible : Visibility.Collapsed;

        [JsonPropertyName("mods")]
        public ModData[] Mods { get; set; }

        public class ModData {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("link")]
            public string Link { get; set; }

            public string LinkVerified { 
                get {
                    if (Name.Contains("Battlefront Plus - Kyber"))
                        return "https://battlefront.plus/";
                    else if (Uri.IsWellFormedUriString(Link, UriKind.Absolute) && (Link.Contains("https://www.nexusmods.com") || Link.Contains("https://www.moddb.com/")))
                        return Link;
                    return "";
                } 
            }
        }

        [JsonPropertyName("proxy")]
        public ServerProxyData ServerProxy { get; set; }

        public class ServerProxyData {
            [JsonPropertyName("ip")]
            public string IP { get; set; }
        }


        public ProxyData Proxy {
            get {
                App.Proxies.TryGetValue(ServerProxy.IP, out ProxyData value);
                return value;
            }
        }
    }
}
