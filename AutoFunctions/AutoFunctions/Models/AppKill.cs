using System;
using System.Configuration;

namespace AutoFunctions.Models
{
    class AppKill
    {
        public bool Enabled { get; set; } = false;
        public string WatchApplication { get; set; } = ConfigurationManager.AppSettings["app-kill_watchApp"].ToLower().Trim();
        public string KillApplication { get; set; } = ConfigurationManager.AppSettings["app-kill_appName"].ToLower().Trim();
        public bool IsContains { get; set; } = false;

        private string P_enable = ConfigurationManager.AppSettings["app-kill_enable"].ToLower();

        public AppKill(Global g)
        {
            //Enabled
            try
            {
                Enabled = Convert.ToBoolean(P_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + P_enable + "' isn't a valid boolean.  Defaulting to '" + Enabled.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.AppKill, details, g);
                details = null;
            }
            P_enable = null;

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
