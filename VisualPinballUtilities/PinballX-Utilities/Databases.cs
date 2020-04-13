using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VisualPinballUtilities.PinballX_Utilities
{
    public class Databases
    {
        public static T ConvertNode<T>(XmlNode node, XmlRootAttribute xRoot = null) where T : class
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

            public static void GetIPDBData()
            {
                IPDB.Utilities.Cache.SetIPDBCache();
                System.Net.Http.HttpClient client = IPDB.Utilities.GetClient();

                foreach (var listsItem in IPDB.Utilities.listsItems)
                {
                    //if (listsItem.Id != IPDB.Models.ListsValue.top300) continue;

                    //make the subsequent web requests using the same HttpClient object
                    string url = @"https://www.ipdb.org/lists.cgi?puid=" + IPDB.Utilities.puid + "&list=" + listsItem.Id;
                    System.Net.Http.HttpResponseMessage response = null;
                    string content = string.Empty;

                    Task.Run(async () =>
                    {
                        response = await client.GetAsync(url);
                    }).Wait();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Task.Run(async () =>
                        {
                            content = await client.GetStringAsync(url);
                        }).Wait();

                        //write this to json
                        System.Text.RegularExpressions.Regex checkTableTags = new System.Text.RegularExpressions.Regex(@"(<table[^>]*>)([\s\S\n\r\t]*?)(</table>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        //var tables = ConvertFromHTMLTableToJSON.GetPropertyValueStringXML(content, "table");
                        System.Text.RegularExpressions.MatchCollection mc = checkTableTags.Matches(content);
                        int tableCounter = 0;
                        foreach (var table in mc)
                        {
                            List<Dictionary<string, string>> data = ConvertFromHTMLTable.ExtractData(table.ToString(), listsItem.Id);
                            //string stringData = System.Text.Json.JsonSerializer.Serialize(data);
                            string filenameExtra = string.Empty;
                            if (tableCounter > 0) filenameExtra = "_" + tableCounter;
                            string filename = @"C:\temp\".TrimEnd('\\') + @"\" + listsItem.Name.Replace(@"/", "-").Replace(@"\", "-") + filenameExtra + ".txt";

                            if (!System.IO.Directory.Exists(@"C:\temp\")) System.IO.Directory.CreateDirectory(@"C:\temp\");
                            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
                            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                            sw.Write(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));//
                                                                                                                                                                                                          //sw.Write(JsonSerializer.Serialize(objectToSerialize, new JsonSerializerOptions { WriteIndented = true }));
                            sw.Close();
                            sw.Dispose();
                            tableCounter++;
                        }

                    }
                    else
                    {
                        //TODO: write to log, figure out what is happening
                    }
                }

                //TOSO: parse the content
                //}
                client.Dispose();
            }
        }

        public class ConvertFromHTMLTable
        {
            public static List<Dictionary<string, string>> ExtractData(string tableString, IPDB.Models.ListsValue listsValue)
            {
                List<Dictionary<string, string>> table = new List<Dictionary<string, string>>();

                
                //TODO: need to check file for start and end <table> tags, must have these
                System.Text.RegularExpressions.Regex checkTableTags = new System.Text.RegularExpressions.Regex(@"^(<table[^>]*>)([\s\S\n\r\t]*?)(</table>)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if(checkTableTags.IsMatch(tableString))
                {
                    //get rows
                    //System.Text.RegularExpressions.Regex checkTRTags = new System.Text.RegularExpressions.Regex(@"(<tr[^>]*>)([\s\S\n\r\t]*?)(</tr>)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    //System.Text.RegularExpressions.MatchCollection rows = checkTRTags.Matches(tableString);
                    var rows = GetPropertyValueStringXML(tableString, "tr");
                    //<tr> start of new row
                    //assume first row is the header, if <th> then 100%, if <td> then its a guess
                    //check for <th> in every row, create a new table if it's found after the first row

                    bool isFirst = true;
                    List<string> header = new List<string>();
                    foreach (string row in rows)
                    {
                        Dictionary<string, string> tableRow = new Dictionary<string, string>();
                        //var cells = GetPropertyValueStringXML(row, "th"); //INFO: found coding issues in site, a th tag was terminated with a td
                        System.Text.RegularExpressions.Regex checkTHTags = new System.Text.RegularExpressions.Regex(@"(<th[^>]*>)([\s\S\n\r\t]*?)((</th>)|(</td>))", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        System.Text.RegularExpressions.MatchCollection mc = checkTHTags.Matches(row);
                        List<string> cells = new List<string>();
                        cells = mc.OfType<System.Text.RegularExpressions.Match>().Select(a => a.Groups[2].Value.ToString()).ToList();

                        if (cells.Count > 0)
                        {
                            //this is the header
                            if (!isFirst)
                            {
                                table.Add(tableRow);
                            }
                            tableRow = new Dictionary<string, string>();
                            header = new List<string>();
                            foreach(var cell in cells)
                            {
                                var thisCell = cell.Replace("(click to view)", string.Empty)
                                    .Replace("(click to search for)", string.Empty)
                                                    .Replace("&nbsp;", string.Empty)
                                                    .Replace("<br>", string.Empty)
                                                    .Trim();
                                header.Add(thisCell);
                            }
                            
                            isFirst = false;
                        }
                        else
                        {
                            cells = GetPropertyValueStringXML(row, "td");

                            if (cells.Count > 0)
                            {
                                if (isFirst)
                                {
                                    header = new List<string>();
                                    foreach (var cell in cells)
                                    {
                                        var thisCell = cell .Replace("(click to view)", string.Empty)
                                            .Replace("(click to search for)", string.Empty)
                                                            .Replace("&nbsp;", string.Empty)
                                                            .Replace("<br>", string.Empty)
                                                            .Trim();
                                        header.Add(thisCell);
                                    }
                                    isFirst = false;
                                }
                                else
                                {
                                    if (header.Count >= cells.Count)
                                    {
                                        for (int i = 0; i < header.Count; i++)
                                        {
                                            string cell = cells[i].Replace("&nbsp;", string.Empty).Trim();
                                            System.Text.RegularExpressions.Regex checkATags = new System.Text.RegularExpressions.Regex(@"<a\s+(?:[^>]*?\s+)?href=([""'])(.*?)\1", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                            if (checkATags.IsMatch(cell))
                                            {
                                                var values = GetPropertyValueStringXML(cell, "a");

                                                tableRow.Add(header[i], values[0]);
                                                var aTagMatchCollection = checkATags.Matches(cell);
                                                var aTagMatch = aTagMatchCollection[0];
                                                string linkURL = aTagMatch.Groups[2].Value.ToString();
                                                tableRow.Add(header[i] + "_link", @"https://www.ipdb.org/" + linkURL);

                                                switch(listsValue)
                                                {
                                                    case IPDB.Models.ListsValue.games:
                                                    case IPDB.Models.ListsValue.abbrev:
                                                        string ipdbId = System.Web.HttpUtility.ParseQueryString(linkURL.Substring(linkURL.IndexOf('?'))).Get("gid");
                                                        tableRow.Add("IPDBId", ipdbId);
                                                        break;
                                                    case IPDB.Models.ListsValue.mfg1:
                                                    case IPDB.Models.ListsValue.mfg2:
                                                        string mfgId = System.Web.HttpUtility.ParseQueryString(linkURL.Substring(linkURL.IndexOf('?'))).Get("mfgid");
                                                        tableRow.Add("IPDBMFGId", mfgId);
                                                        break;
                                                    case IPDB.Models.ListsValue.mpu:
                                                        string mpuId = System.Web.HttpUtility.ParseQueryString(linkURL.Substring(linkURL.IndexOf('?'))).Get("mpu");
                                                        tableRow.Add("IPDBMPUId", mpuId);
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                tableRow.Add(header[i], cells[i]);
                                            }
                                        }



                                        table.Add(tableRow);
                                    }
                                }
                            }
                        }
                    }
                }
                
                //if any subsequent row has <th>, start a new table
                //<td> each one is a cell, grab all of it, add it to the array
                //maybe check for <a> tags, for links, extract and add as a new column for that row <td>[field name with link]</td> --> [field name], [field name]_anchor
                //check for more meta data as you need it
                return table;
            }

            public static List<string> GetPropertyValueStringXML(string xml, string propertyName, bool getLastMatch = false, bool onlyDistinct = false)
            {
                return GetPropertyValueStringXML(xml, new string[] { propertyName }, getLastMatch, onlyDistinct);
            }

            public static List<string> GetPropertyValueStringXML(string xml, string[] propertyNames, bool getLastMatch = false, bool onlyDistinct = false)
            {
                List<string> l = new List<string>();

                //var xml = IDT.Web.CommonUtilities.Util.Xml.XmlHelper.Serialize(objectToCheck);
                //xml = xml.Replace(" xmlns=\"http://tempuri.org/\"", "");
                //INFO: search xml for property names, grab the values
                var propRegex = new System.Text.RegularExpressions.Regex(@"(<" + propertyNames[0] + @"[^>]*>)([\s\S\n\r\t]*?)(</" + propertyNames[0] + @">)");
                System.Text.RegularExpressions.MatchCollection mc = propRegex.Matches(xml);

                if (getLastMatch)
                {
                    System.Text.RegularExpressions.Match m = mc[mc.Count - 1];
                    l.Add(m.Groups[2].ToString());
                    m = null;
                }
                else
                {
                    foreach (System.Text.RegularExpressions.Match m in mc)
                    {
                        if (onlyDistinct) if (l.Contains(m.Groups[2].ToString())) continue;

                        l.Add(m.Groups[2].ToString());
                    }
                }

                mc = null;
                xml = null;
                //objectToCheck = null;

                return l;
            }
        }
    }
}
