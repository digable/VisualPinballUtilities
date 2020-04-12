using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VisualPinballUtilities.PinballY_Utilities
{
    public class Models
    {
        [Serializable]
        //[XmlRoot(ElementName = "menu")]
        public class Database
        {
            public List<game> menu = new List<game>();

            [Serializable]
            public class game
            {
                [XmlAttribute]
                public string name { get; set; }

                public string description { get; set; }
                public string rom { get; set; }
                public string manufacturer { get; set; }
                public string year { get; set; }
                public string type { get; set; }
                public string hidedmd { get; set; }
                public string hidetopper { get; set; }
                public string hidebackglass { get; set; }
                public string enabled { get; set; }
                public string rating { get; set; }
                public string ipdbid { get; set; }
                public string alternateexe { get; set; }
            }
        }
    }
}
