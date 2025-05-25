using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gherkinizer
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class AppSettings
    {
        public string TestPrefix { get; set; }
        public string MindmapPrefix { get; set; }
        public string ReqPrefix { get; set; }
        public List<string> DomainList { get; set; }
        public string ParentNode { get; set; }
        public string FeatureFindRoot { get; set; }
        public string MindmapFindRoot { get; set; }
        public int SequenceStart { get; set; }
        public int ScenarioLevel { get; set; }
        public bool ClearTextArea { get; set; }

        public static AppSettings Load(string filePath)
        {
            var settings = new AppSettings();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                var parts = line.Split('=', 2);
                if (parts.Length != 2) continue;

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                switch (key)
                {
                    case "TEST_PREFIX":
                        settings.TestPrefix = value;
                        break;
                    case "REQ_PREFIX":
                        settings.ReqPrefix = value;
                        break;
                    case "MINDMAP_PREFIX":
                        settings.MindmapPrefix = value;
                        break;
                    case "DOMAIN_LIST":
                        settings.DomainList = new List<string>(value.Split(','));
                        break;
                    case "PARENT_NODE":
                        settings.ParentNode = value;
                        break;
                    case "FEATURE_FIND_ROOT":
                        settings.FeatureFindRoot = value;
                        break;
                    case "MINDMAP_FIND_ROOT":
                        settings.MindmapFindRoot = value;
                        break;
                    case "SEQUENCE_START":
                        settings.SequenceStart = int.TryParse(value, out var seq) ? seq : 1;
                        break;
                    case "SCENARIO_LEVEL":
                        settings.ScenarioLevel = int.TryParse(value, out var lvl) ? lvl : 1;
                        break;
                    case "CLEAR_TEXT_AREA":
                        settings.ClearTextArea = value.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
                        break;
                }
            }

            return settings;
        }
    }

}
