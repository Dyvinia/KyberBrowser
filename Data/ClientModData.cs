using System.Text.Json.Serialization;

namespace KyberBrowser.Data {
    public class ClientModData {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; }
    }
}
