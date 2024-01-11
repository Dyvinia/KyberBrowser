using System.Collections.Generic;

namespace KyberBrowser.Data {
    public class ModeData {
        public string Name { get; set; }
        public string[] Maps { get; set; }
        public Dictionary<string, string> MapOverrides { get; set; }

        public ModeData Clone() {
            return (ModeData)MemberwiseClone();
        }
    }
}