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
                        //pull the visual pinball version from pinballx config file
                        //pull the vp table information from all vp versions
                        var vp9Item = PinballX_Utilities.Databases.Import.VisualPinball9(@"C:\Emulators\PinballX\Databases\Visual Pinball\Visual Pinball.xml");
                        //pull all of the pinballx media folder info, companys and the visual pinball versions
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
