using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualPinballUtilities
{
    class Report
    {
        public static void Process(List<string> reportNames)
        {
            foreach(string reportName in reportNames)
            {
                switch (reportName)
                {
                    case "PinballX --> list of media files w/o associations":
                        //pull all of the pinballx media folder info, companys and the visual pinball versions

                        Dictionary<string, Dictionary<string, object>> pinballXProperties = PinballX_Utilities.Databases.Import.PinballXConfig(@"C:\Emulators\PinballX\Config\PinballX.ini");

                        foreach (string sectionName in pinballXProperties.Keys)
                        {
                            //if dictionary name has 'visual' and 'pinball' or the name property has 'visual' and 'pinball' in it
                            if (sectionName.ToLower().Contains("visual") && sectionName.ToLower().Contains("pinball"))
                            {
                                string vpMediaConfigDirectory = string.Empty;
                                PinballX_Utilities.Models.VisualPinballVersion vpv = PinballX_Utilities.Models.VisualPinballVersion.UNKNOWN;
                                //this is a visual pinball config 
                                var vpp = pinballXProperties[sectionName];
                                if (Convert.ToBoolean(vpp["Enabled"]))
                                {
                                    if (sectionName.ToLower() == "visualpinball")
                                    {
                                        vpMediaConfigDirectory = "Visual Pinball";
                                        vpv = PinballX_Utilities.Models.VisualPinballVersion.VP9;
                                    }
                                    else
                                    {
                                        vpMediaConfigDirectory = vpp["Name"].ToString();
                                        if (vpMediaConfigDirectory.ToLower().Contains("physmod5")) vpv = PinballX_Utilities.Models.VisualPinballVersion.VP9PM5;
                                        else if (vpMediaConfigDirectory.ToLower().Contains(" x")) vpv = PinballX_Utilities.Models.VisualPinballVersion.VP10;
                                    }

                                    //get the vpItems
                                    //pull the vp table information from all vp versions
                                    var vpItem = PinballX_Utilities.Databases.Import.VisualPinball(@"C:\Emulators\PinballX\Databases\" + vpMediaConfigDirectory + @"\" + vpMediaConfigDirectory + @".xml", vpv);
                                    //description value is the prefix of the media files
                                    List<string> vpMediaFileDirectories = Directory.GetDirectories(@"C:\Emulators\PinballX\Media\" + vpMediaConfigDirectory, "*.*", SearchOption.AllDirectories).ToList();
                                    Dictionary<string, List<string>> allMediaFiles = new Dictionary<string, List<string>>();
                                    //find missing items as well as items that do not have a table associated with them
                                    foreach (string mediaFileDirectory in vpMediaFileDirectories)
                                    {
                                        string directoryName = mediaFileDirectory.Replace(@"C:\Emulators\PinballX\Media\" + vpMediaConfigDirectory + @"\", string.Empty);
                                        List<string> mediaFiles = Directory.GetFiles(mediaFileDirectory).ToList();
                                        allMediaFiles.Add(directoryName, mediaFiles);
                                        mediaFiles = null;
                                        directoryName = null;
                                    }
                                }
                                else
                                {
                                    //INFO: these are disabled setups
                                }
                                
                                //move to a list of objects for visual pinball config
                                //or just pull the name, then generate the xml file location
                            }
                        }

                        //pull the visual pinball version from pinballx config file
                        
                        
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
