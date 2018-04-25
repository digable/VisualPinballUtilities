using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Models
{
    public class Global
    {
        public bool IsServiceRunning { get; set; } = true; //this will always be true unless debugging
        public int SleepTime { get; set; } = 1 * 1000;
        public int OSVersion { get; set; } = 32;

        public string ConfigFile { get; set; } = ConfigurationManager.AppSettings["config-file"];
        public string LogFile { get; set; } = ConfigurationManager.AppSettings["log-file"];

        private string pSleepTime { get; set; } = ConfigurationManager.AppSettings["sleepTime"];
        private string pOSVersion { get; set; } = ConfigurationManager.AppSettings["os-version"];

        public Global()
        {
            //INFO: setup the full path for config and log files
            char[] trimChars = new char[] { '\\' };
            this.ConfigFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\" + this.ConfigFile;
            //INFO: remove URI format from file path
            this.ConfigFile = this.ConfigFile.Substring(6);

            this.LogFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\" + this.LogFile;
            //INFO: remove URI format from file path
            this.LogFile = this.LogFile.Substring(6);

            //SleepTime
            try
            {
                this.SleepTime = Convert.ToInt32(this.pSleepTime) * 1000;
            }
            catch (Exception)
            {
                string details = "SleepTime value '" + this.pSleepTime + "' isn't a valid integer.  Defaulting to '" + (this.SleepTime / 1000).ToString() + "' seconds.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, this.LogFile);
                details = null;
            }
            pSleepTime = null;

            //OSVersion
            try
            {
                this.OSVersion = Convert.ToInt32(this.pOSVersion);
            }
            catch (Exception)
            {
                string details = "OSVersion value '" + this.pOSVersion + "' isn't a valid boolean.  Defaulting to '" + this.OSVersion.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, this.LogFile);
                details = null;
            }
            pOSVersion = null;
        }
    }
}
