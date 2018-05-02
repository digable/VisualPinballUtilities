using System;
using System.Configuration;

namespace AutoFunctions.Models
{
    public class RotateScreen
    {
        public bool Enabled { get; set; } = false;
        public string WatchApplication { get; set; } = ConfigurationManager.AppSettings["rotate-screen_watchApp"].ToLower().Trim();
        public int Monitor { get; set; } = 1;
        public bool IsContains { get; set; } = false;

        private string P_enable = ConfigurationManager.AppSettings["rotate-screen_enable"].ToLower().Trim();
        private string P_monitorString { get; set; } = ConfigurationManager.AppSettings["rotate-screen_monitor"].Trim();

        public RotateScreen(Global g)
        {
            //Enabled
            try
            {
                Enabled = Convert.ToBoolean(P_enable);
            }
            catch (Exception)
            {
                if (g.LoggingEnabled)
                {
                    string details = "Enabled value '" + P_enable + "' isn't a valid boolean.  Defaulting to '" + Enabled.ToString() + "'.";
                    Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.RotateScreen, details, g.LogFile);
                    details = null;
                }
            }
            P_enable = null;

            //Monitor
            Monitor = Functions.RunOnce.Get.RotateScreenMonitor(P_monitorString, g.LogFile);
            P_monitorString = null;

            //WatchApplication
            if (WatchApplication.EndsWith("*"))
            {
                IsContains = true;
                WatchApplication = WatchApplication.TrimEnd('*');
            }

            g = null;
        }
    }
}
