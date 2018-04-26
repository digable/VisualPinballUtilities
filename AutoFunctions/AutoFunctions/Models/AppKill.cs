using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Models
{
    class AppKill
    {
        public bool Enabled { get; set; } = false;
        public string WatchApplication { get; set; } = ConfigurationManager.AppSettings["app-kill_watchApp"].ToLower();
        public string KillApplication { get; set; } = ConfigurationManager.AppSettings["app-kill_appName"].ToLower();
        public bool IsContains { get; set; } = false;

        private string P_enable = ConfigurationManager.AppSettings["app-kill_enable"].ToLower();

        public AppKill(string logFile)
        {
            //Enabled
            try
            {
                this.Enabled = Convert.ToBoolean(P_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + P_enable + "' isn't a valid boolean.  Defaulting to '" + this.Enabled.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.AppKill, details, logFile);
                details = null;
            }
            P_enable = null;

            //WatchApplication
            if (this.WatchApplication.EndsWith("*"))
            {
                this.IsContains = true;
                this.WatchApplication = this.WatchApplication.TrimEnd('*');
            }
        }
    }
}
