using System;
using System.Configuration;
using System.Threading;


namespace RotateScreen
{
    class Program
    {
        private static int sleepTime = 1;

        private static string rotateScreen_appName = ConfigurationManager.AppSettings["rotate-screen_appName"].ToLower();
        private static int rotateScreen_monitor = 2;

        private static string appKill_appName = ConfigurationManager.AppSettings["app-kill_appName"].ToLower();
        private static string appKill_watchApp = ConfigurationManager.AppSettings["app-kill_watchApp"].ToLower();

        static void Main(string[] args)
        {
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

            while (true)
            {
                Functions.MonitorOrientation orientation = Functions.CheckMonitorOrientation(rotateScreen_monitor);
                if (Functions.CheckForRunningProcess(rotateScreen_appName))
                {
                    if (orientation == Functions.MonitorOrientation.Portrait)
                    {
                        Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Landscape);
                    }
                    Thread.Sleep(sleepTime);
                }
                else
                {
                    if (orientation == Functions.MonitorOrientation.Landscape)
                    {
                        Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Portrait);
                    }
                    Thread.Sleep(sleepTime);
                }
            }
        }
    }
}
