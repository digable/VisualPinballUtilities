using System;
using System.Configuration;

namespace AutoFunctions.Models
{
    public class Global
    {
        public bool IsServiceRunning { get; set; } = true; //this will always be true unless debugging
        public int SleepTime { get; set; } = 1 * 1000;
        public int OSVersion { get; set; } = 32;

        public string ConfigFile { get; set; } = ConfigurationManager.AppSettings["config-file"].Trim();
        public string LogFile { get; set; } = ConfigurationManager.AppSettings["log-file"].Trim();
        public bool LoggingEnabled { get; set; } = true;
        public bool RDPKillEnabled { get; set; } = true;

        private string P_sleepTime { get; set; } = ConfigurationManager.AppSettings["sleepTime"].Trim();
        private string P_osVersion { get; set; } = ConfigurationManager.AppSettings["os-version"].Trim();
        private string P_loggingEnabled { get; set; } = ConfigurationManager.AppSettings["logging_enabled"].Trim();
        private string P_RDPkillEnabled { get; set; } = ConfigurationManager.AppSettings["RDP-kill_enable"].Trim();

        public Global()
        {
            //INFO: setup the full path for config and log files
            char[] trimChars = new char[] { '\\' };
            ConfigFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\" + ConfigFile;
            //INFO: remove URI format from file path
            ConfigFile = ConfigFile.Substring(6);

            LogFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\" + LogFile;
            //INFO: remove URI format from file path
            LogFile = LogFile.Substring(6);
            trimChars = null;

            //LoggingEnabled
            try
            {
                LoggingEnabled = Convert.ToBoolean(P_loggingEnabled);
            }
            catch (Exception)
            {
                string details = "EnabledLogging value '" + P_loggingEnabled + "' isn't a valid boolean.  Defaulting to '" + LoggingEnabled.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LoggingEnabled, LogFile);
                details = null;
            }
            P_loggingEnabled = null;

            //RDPKillEnabled
            try
            {
                RDPKillEnabled = Convert.ToBoolean(P_RDPkillEnabled);
            }
            catch (Exception)
            {
                string details = "RDPKillEnabled value '" + P_RDPkillEnabled + "' isn't a valid boolean.  Defaulting to '" + RDPKillEnabled.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, RDPKillEnabled, LogFile);
                details = null;
            }
            P_RDPkillEnabled = null;

            //SleepTime
            try
            {
                int temp_SleepTime = Convert.ToInt32(P_sleepTime) * 1000;
                if (temp_SleepTime <= 0)
                {
                    string details = "SleepTime value '" + P_sleepTime + "' isn't greater than zero.  Defaulting to '" + (SleepTime / 1000).ToString() + "' seconds.";
                    Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LoggingEnabled, LogFile);
                    details = null;
                }
                else if (temp_SleepTime > 86400)
                {
                    string details = "SleepTime value '" + P_sleepTime + "' is more than 24 hours.  Defaulting to '" + (SleepTime / 1000).ToString() + "' seconds.";
                    Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LoggingEnabled, LogFile);
                    details = null;
                }
                else SleepTime = temp_SleepTime;
            }
            catch (Exception)
            {
                string details = "SleepTime value '" + P_sleepTime + "' isn't a valid integer.  Defaulting to '" + (SleepTime / 1000).ToString() + "' seconds.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LoggingEnabled, LogFile);
                details = null;
            }
            P_sleepTime = null;

            //OSVersion
            try
            {
                OSVersion = Convert.ToInt32(P_osVersion);
            }
            catch (Exception)
            {
                string details = "OSVersion value '" + P_osVersion + "' isn't a valid integer.  Defaulting to '" + OSVersion.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LoggingEnabled, LogFile);
                details = null;
            }
            P_osVersion = null;
        }
    }
}
