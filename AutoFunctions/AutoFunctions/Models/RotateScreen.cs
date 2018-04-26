using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public RotateScreen(string logFile)
        {
            //Enabled
            try
            {
                this.Enabled = Convert.ToBoolean(P_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + P_enable + "' isn't a valid boolean.  Defaulting to '" + this.Enabled.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.RotateScreen, details, logFile);
                details = null;
            }
            P_enable = null;

            //Monitor
            this.Monitor = Functions.RunOnce.Get.RotateScreenMonitor(this.P_monitorString, logFile);
            P_monitorString = null;

            //WatchApplication
            if (this.WatchApplication.EndsWith("*"))
            {
                this.IsContains = true;
                this.WatchApplication = this.WatchApplication.TrimEnd('*');
            }
        }
    }
}
