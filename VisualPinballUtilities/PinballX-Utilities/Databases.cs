using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VisualPinballUtilities.PinballX_Utilities
{
    public class Databases
    {
        private static T ConvertNode<T>(XmlNode node, XmlRootAttribute xRoot = null) where T : class
        {
            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.OuterXml);
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(T));
            if (xRoot != null) ser = new XmlSerializer(typeof(T), xRoot);
            T result = (ser.Deserialize(stm) as T);

            xRoot = null;
            node = null;

            return result;
        }

        public class Import
        {
            public static Models.VisualPinballItem VisualPinball(string file, Models.VisualPinballVersion version)
            {
                Models.VisualPinballItem vpItem = new Models.VisualPinballItem()
                {
                    version = version
                };

                //make sure the file exists
                if (!File.Exists(file))
                {
                    //TODO: error logging
                }
                else
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(file);
                    var rootNode = xdoc.FirstChild;

                    foreach (XmlNode game in rootNode.ChildNodes)
                    {
                        XmlRootAttribute xRoot = new XmlRootAttribute()
                        {
                            ElementName = "game",
                            IsNullable = true
                        };
                        Models.VisualPinballTable table = ConvertNode<Models.VisualPinballTable>(game, xRoot);
                        xRoot = null;
                        vpItem.tables.Add(table);
                        table = null;                        
                    }
                }

                return vpItem;
            }

            public static Dictionary<string, Dictionary<string, object>> PinballXConfig(string file)
            {
                Dictionary<string, Dictionary<string, object>> dProperties = new Dictionary<string, Dictionary<string, object>>();

                if (!File.Exists(file))
                {
                    //TODO: error logging
                }
                else
                {
                    StreamReader sr = new StreamReader(file);
                    string line;
                    
                    Dictionary<string, object> dSection = new Dictionary<string, object>();
                    string sectionName = string.Empty;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == string.Empty) continue;
                        if (line.StartsWith("#")) continue;

                        if (line.StartsWith("["))
                        {
                            if (sectionName != string.Empty) dProperties.Add(sectionName, dSection);
                            sectionName = line.Trim('[').Trim(']').Trim();
                            dSection = new Dictionary<string, object>();
                            //this is the start of a section
                            //read until you find the next '[' or the end of file
                        }
                        else
                        {
                            //these are the properties
                            string[] lineSplit = line.Split('=');
                            if (lineSplit.Length == 1)
                            {
                                dSection.Add(lineSplit[0].Trim(), null);
                            }
                            else if (lineSplit.Length > 1)
                            {
                                dSection.Add(lineSplit[0].Trim(), lineSplit[1].Trim());
                            }
                            //dSection.Add()
                        }
                    }
                    dProperties.Add(sectionName, dSection);
                    dSection = null;
                    sectionName = null;
                }

                file = null;

                return dProperties;
            }
        }
    }
}
