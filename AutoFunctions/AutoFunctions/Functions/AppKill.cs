using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class AppKill
    {
        public static void IsEnabled(Models.AppKill ak)
        {
            bool isRunning = false;
            if (ak.IsContains) isRunning = Utilities.CheckForRunningProcessContains(ak.KillApplication);
            else isRunning = Utilities.CheckForRunningProcess(ak.KillApplication);

            if (isRunning)
            {
                string appKill_watchApp_clean = ak.WatchApplication;
                bool appKillWatchAppBool = true;
                if (ak.WatchApplication.StartsWith("[-]"))
                {
                    appKillWatchAppBool = false;
                    appKill_watchApp_clean = ak.WatchApplication.Replace("[-]", string.Empty).Trim();
                }
                else if (ak.WatchApplication.StartsWith("[+]"))
                {
                    appKillWatchAppBool = true;
                    appKill_watchApp_clean = ak.WatchApplication.Replace("[+]", string.Empty).Trim();
                }

                if (Utilities.CheckForRunningProcess(appKill_watchApp_clean) == appKillWatchAppBool)
                {
                    bool b = Utilities.KillRunningProcess(ak.KillApplication);
                }
            }
        }
    }
}
