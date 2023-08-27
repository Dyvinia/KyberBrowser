using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KyberBrowser {
    public class ProxyData {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("ip")]
        public string IP { get; set; }

        public long Ping { get; set; }

        public string NameInfo => $"{Name} [{Ping}ms]";
    }
}
