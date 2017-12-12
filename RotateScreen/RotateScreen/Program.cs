using System;
using System.Configuration;
using System.Threading;


namespace RotateScreen
{
    class Program
    {
        private static int sleepTime = 1;

        private static string rotateScreen_enable = ConfigurationManager.AppSettings["rotate-screen_enable"].ToLower();
        private static string rotateScreen_appName = ConfigurationManager.AppSettings["rotate-screen_appName"].ToLower();
        private static int rotateScreen_monitor = 2;

        private static string appKill_enable = ConfigurationManager.AppSettings["app-kill_enable"].ToLower();
        private static string appKill_appName = ConfigurationManager.AppSettings["app-kill_appName"].ToLower();
        private static string appKill_watchApp = ConfigurationManager.AppSettings["app-kill_watchApp"].ToLower();
        //need to close out the longest running one if there are multiples.

        static void Main(string[] args)
        {
            bool bRotateScreen_enable = true;
            bool bAppKill_enable = true;

            try
            {
                sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["sleepTime"]) * 1000;
            }
            catch (Exception) { }

            try
            {
                rotateScreen_monitor = Convert.ToInt32(ConfigurationManager.AppSettings["rotate-screen_monitor"]);
            }
            catch (Exception) { }

            try
            {
                bRotateScreen_enable = Convert.ToBoolean(rotateScreen_enable);
            }
            catch (Exception ex) { }

            try
            {
                bAppKill_enable = Convert.ToBoolean(appKill_enable);
            }
            catch (Exception ex) { }

            while (true)
            {
                if (bRotateScreen_enable)
                {
                    Functions.MonitorOrientation orientation = Functions.CheckMonitorOrientation(rotateScreen_monitor);
                    if (Functions.CheckForRunningProcess(rotateScreen_appName))
                    {
                        if (orientation == Functions.MonitorOrientation.Portrait)
                        {
                            Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Landscape);
                        }
                        //Thread.Sleep(sleepTime);
                    }
                    else
                    {
                        if (orientation == Functions.MonitorOrientation.Landscape)
                        {
                            Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Portrait);
                        }
                        //Thread.Sleep(sleepTime);
                    }
                }

                if (bAppKill_enable)
                {
                    if (Functions.CheckForRunningProcess(appKill_appName))
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

                        if (Functions.CheckForRunningProcess(appKill_watchApp_clean) == appKillWatchAppBool)
                        {
                            bool b = Functions.KillRunningProcess(appKill_appName);
                        }

                    }
                }

                Thread.Sleep(sleepTime);
            }
        }
    }
}
