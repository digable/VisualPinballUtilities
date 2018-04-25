using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class AppKill
    {
        public static void IsEnabled(bool isContains_appKill, string appKill_watchApp, string appKill_appName_clean)
        {
            bool isRunning = false;
            if (isContains_appKill) isRunning = Utilities.CheckForRunningProcessContains(appKill_appName_clean);
            else isRunning = Utilities.CheckForRunningProcess(appKill_appName_clean);

            if (isRunning)
            {
                string appKill_watchApp_clean = appKill_watchApp;
                bool appKillWatchAppBool = true;
                if (appKill_watchApp.StartsWith("[-]"))
                {
                    appKillWatchAppBool = false;
                    appKill_watchApp_clean = appKill_watchApp.Replace("[-]", string.Empty).Trim();
                }
                else if (appKill_watchApp.StartsWith("[+]"))
                {
                    appKillWatchAppBool = true;
                    appKill_watchApp_clean = appKill_watchApp.Replace("[+]", string.Empty).Trim();
                }

                if (Utilities.CheckForRunningProcess(appKill_watchApp_clean) == appKillWatchAppBool)
                {
                    bool b = Utilities.KillRunningProcess(appKill_appName_clean);
                }
            }
        }
    }
}
