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
            public static Models.VisualPinballItem VisualPinball9(string file)
            {
                Models.VisualPinballItem vpItem = new Models.VisualPinballItem();
                vpItem.version = Models.VisualPinballVersion.vp9;

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
                        XmlRootAttribute xRoot = new XmlRootAttribute();
                        xRoot.ElementName = "game";
                        xRoot.IsNullable = true;

                        Models.VisualPinballTable table = ConvertNode<Models.VisualPinballTable>(game, xRoot);
                        xRoot = null;
                        vpItem.tables.Add(table);
                        table = null;                        
                    }
                }

                return vpItem;
            }

            public static void VisualPinballPhysmod5(string file)
            {

            }

            public static void VisualPinball10(string file)
            {

            }
        }
    }
}
