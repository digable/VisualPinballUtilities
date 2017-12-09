using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisualPinballUtilities.PinballX_Utilities
{
    public class Models
    {
        public class VisualPinballTable
        {
            [XmlAttribute("name")]
            public string name { get; set; }
            public string description { get; set; }
            public string rom { get; set; }
            public string manufacturer { get; set; }
            public string year { get; set; }
            public string type { get; set; }
            public string hidedmd { get; set; }
            public string hidebackglass { get; set; }
            public string enabled { get; set; }
            public string rating { get; set; }
        }

        public class VisualPinballItem
        {
            public VisualPinballVersion version { get; set; }
            public List<VisualPinballTable> tables { get; set; } = new List<VisualPinballTable>();
        }

        public enum VisualPinballVersion
        {
            UNKNOWN = 0,
            VP9 = 1,
            VP9PM5 = 2,
            VP10 = 3
        }

        public class PinballXConfig
        {
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public string WorkingPath { get; set; }
            public string TablePath { get; set; }
            public string Executable { get; set; }
            public string Parameters { get; set; }
            public bool LaunchBeforeEnabled { get; set; }
            public string LaunchBeforeWorkingPath { get; set; }
            public string LaunchBeforeExecutable { get; set; }
            public bool LaunchBeforeHideWindow { get; set; }
            public bool LaunchBeforeWaitForExit { get; set; }
            public bool LaunchAfterEnabled { get; set; }
            public string LaunchAfterWorkingPath { get; set; }
            public string LaunchAfterExecutable { get; set; }
            public bool LaunchAfterHideWindow { get; set; }
            public bool LaunchAfterWaitForExit { get; set; }
        }
    }
}

