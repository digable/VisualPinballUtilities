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
        public string WatchApplication { get; set; } = ConfigurationManager.AppSettings["rotate-screen_watchApp"].ToLower();
        public int Monitor { get; set; } = 1;
        public bool IsContains { get; set; } = false;

        private string p_enable = ConfigurationManager.AppSettings["rotate-screen_enable"].ToLower();
        private string p_monitorString { get; set; } = ConfigurationManager.AppSettings["rotate-screen_monitor"];

        public RotateScreen(string logFile)
        {
            //Enabled
            try
            {
                this.Enabled = Convert.ToBoolean(p_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + p_enable + "' isn't a valid boolean.  Defaulting to '" + this.Enabled.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.RotateScreen, details, logFile);
                details = null;
            }
            p_enable = null;

            //Monitor
            this.Monitor = Functions.RunOnce.Get.RotateScreenMonitor(this.p_monitorString, logFile);
            p_monitorString = null;

            //WatchApplication
            if (this.WatchApplication.EndsWith("*"))
            {
                this.IsContains = true;
                this.WatchApplication = this.WatchApplication.TrimEnd('*');
            }
        }
    }
}
