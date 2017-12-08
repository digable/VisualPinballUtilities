using System;
using System.Collections.Generic;
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
                        //if dictionary name has 'visual' and 'pinball' or the name property has 'visual' and 'pinball' in it
                        foreach (string sectionName in pinballXProperties.Keys)
                        {
                            if (sectionName.ToLower().Contains("visual") && sectionName.ToLower().Contains("pinball"))
                            {
                                //this is a visual pinball config 
                                //move to a list of objects for visual pinball config
                                //or just pull the name, then generate the xml file location
                            }
                        }

                        //pull the visual pinball version from pinballx config file
                        //pull the vp table information from all vp versions
                        var vp9Item = PinballX_Utilities.Databases.Import.VisualPinball(@"C:\Emulators\PinballX\Databases\Visual Pinball\Visual Pinball.xml", PinballX_Utilities.Models.VisualPinballVersion.vp9);
                        
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
