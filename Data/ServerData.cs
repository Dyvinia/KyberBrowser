using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using Newtonsoft.Json;

namespace KyberBrowser {
    public class ServerData {
        public string Title {
            get {
                string title = Name;
                foreach (Match match in Regex.Matches(Name, @"\[(.*?)\]")) {
                    title = title.Replace(match.Value, "");
                }
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
                dynamic constants = JsonConvert.DeserializeObject<dynamic>(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("KyberBrowser.Resources.Constants.json")).ReadToEnd());

                string mapName = ((List<dynamic>)constants.maps.ToObject<List<dynamic>>()).FirstOrDefault(m => m.map == Map).name;
                string modeName = ((List<dynamic>)constants.modes.ToObject<List<dynamic>>()).FirstOrDefault(m => m.mode == Mode).name;

                // BF+ Override for custom 'Bounty Hunt' gamemode, replacing Mode6 (Hero Showdown)
                if (Mods.Any(m => m.Name.Contains("Battlefront Plus - Kyber")) && Mode == "Mode6") {
                    modeName = "Bounty Hunt";
                }

                if (Official)
                    return $"{modeName} - {mapName}";
                else
                    return $"{modeName} - {mapName} [{Host}]";
            }
        }

        public string Thumbnail {
            get {
                return $"https://kyber.gg/static/images/maps/{Map.Replace('/', '-')}.jpg";
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
                    if (Uri.IsWellFormedUriString(Link, UriKind.Absolute) && (Link.Contains("https://www.nexusmods.com") || Link.Contains("https://www.moddb.com/")))
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
