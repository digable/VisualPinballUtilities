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
        public bool IsEnabledLogging { get; set; } = true;

        private string P_sleepTime { get; set; } = ConfigurationManager.AppSettings["sleepTime"].Trim();
        private string P_osVersion { get; set; } = ConfigurationManager.AppSettings["os-version"].Trim();
        private string P_isEnabledLogging { get; set; } = ConfigurationManager.AppSettings["enable-logging"].Trim();

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

            //SleepTime
            try
            {
                SleepTime = Convert.ToInt32(P_sleepTime) * 1000;
            }
            catch (Exception)
            {
                string details = "SleepTime value '" + P_sleepTime + "' isn't a valid integer.  Defaulting to '" + (SleepTime / 1000).ToString() + "' seconds.";
               Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LogFile);
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
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LogFile);
                details = null;
            }
            P_osVersion = null;

            //IsEnabledLogging
            try
            {
                IsEnabledLogging = Convert.ToBoolean(P_isEnabledLogging);
            }
            catch (Exception)
            {
                string details = "IsEnabledLogging value '" + P_isEnabledLogging + "' isn't a valid boolean.  Defaulting to '" + IsEnabledLogging.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, LogFile);
                details = null;
            }
            P_isEnabledLogging = null;
        }
    }
}
