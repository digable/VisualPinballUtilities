using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

using System.Linq;

//TODO: have to so the playfield is the one in focus if pinballx is running and the direct b2s options are NOT open
//TODO: need to close out the longest running one if there are multiples.
//TODO: need to use the config file for other things, remove app config once that is completed.
//TODO: check to see if the app is already running, if so, kill it, post a message, set the monitor to landscape

namespace RotateScreen
{
    class Program
    {
        private static int sleepTime = 1;
        private static int osVersion = 32;
        private static string config_File = ConfigurationManager.AppSettings["config-file"];
        private static string log_file = ConfigurationManager.AppSettings["log-file"];

        private static string rotateScreen_enable = ConfigurationManager.AppSettings["rotate-screen_enable"].ToLower();
        private static string rotateScreen_watchApp = ConfigurationManager.AppSettings["rotate-screen_watchApp"].ToLower();
        private static string rotateScreen_monitorString = ConfigurationManager.AppSettings["rotate-screen_monitor"].ToString();

        private static string appKill_enable = ConfigurationManager.AppSettings["app-kill_enable"].ToLower();
        private static string appKill_appName = ConfigurationManager.AppSettings["app-kill_appName"].ToLower();
        private static string appKill_watchApp = ConfigurationManager.AppSettings["app-kill_watchApp"].ToLower();

        private static string usbKill_enable = ConfigurationManager.AppSettings["usb-kill_enable"].ToLower();
        private static string usbKill_deviceName = ConfigurationManager.AppSettings["usb-kill_deviceName"].ToLower();
        private static string usbKill_watchApp = ConfigurationManager.AppSettings["usb-kill_watchApp"].ToLower();

        private static string moveFile_enable = ConfigurationManager.AppSettings["move-file_enable"];
        private static string moveFile_extensions = ConfigurationManager.AppSettings["move-file_extensions"];
        private static string moveFile_fromFolders = ConfigurationManager.AppSettings["move-file_fromFolders"];
        private static string moveFile_toFolders = ConfigurationManager.AppSettings["move-file_toFolders"];

        static void Main(string[] args)
        {
            int rotateScreen_monitor = 1;
            try
            {
                rotateScreen_monitor = Convert.ToInt32(rotateScreen_monitorString);
            }
            catch (Exception ex)
            {
                string details = "Monitor number '" + rotateScreen_monitorString + "' isn't a valid integer.  Defaulting to 1.";
                bool b = Functions.WriteToLogFile(Functions.LoggingType.Warning, details, log_file);
                details = null;
            }

            //check to see if there is another one running
            Process[] processes = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location));

            if (processes.Length > 1)
            {
                //the application is already running

                //post a message saying you are killing the app
                System.Windows.Forms.MessageBox.Show("RotateScreen is turning off.  To enable again, relaunch the application.", "Killing RotateScreen", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                //rotate screen to landscape
                Functions.MonitorOrientation orientation = Functions.CheckMonitorOrientation(rotateScreen_monitor);
                if (orientation == Functions.MonitorOrientation.Portrait)
                {
                    Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Landscape);

                    Thread.Sleep(1000);
                }

                //kill rotate screen
                var sorted = from p in processes orderby StartTimeNoException(p) ascending, p.Id select p;

                foreach (var p in sorted)
                {
                    p.Kill();
                }
                //this kills this instance, it shouldn't make it this far
                Process.GetCurrentProcess().Kill();
            }
            processes = null;

            //setup the full path for config and log files
            char[] trimChars = new char[] { '\\' };
            string configFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\" + config_File;
            //remove URI format from file path
            configFile = configFile.Substring(6);
            
            string logFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\" + log_file;
            //remove URI format from file path
            logFile = logFile.Substring(6);

            bool bRotateScreen_enable = false;
            bool bAppKill_enable = false;
            bool bUsbKill_enable = false;
            bool bMoveFile_enable = false;

            string usbKill_deviceId = string.Empty;

            try
            {
                sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["sleepTime"]) * 1000;
            }
            catch (Exception) { }

            try
            {
                osVersion = Convert.ToInt32(ConfigurationManager.AppSettings["os-version"]);
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
            catch (Exception) { }

            try
            {
                bAppKill_enable = Convert.ToBoolean(appKill_enable);
            }
            catch (Exception) { }

            try
            {
                bUsbKill_enable = Convert.ToBoolean(usbKill_enable);
            }
            catch (Exception) { }

            try
            {
                bMoveFile_enable = Convert.ToBoolean(moveFile_enable);
            }
            catch (Exception) { }

            while (true)
            {
                if (bRotateScreen_enable)
                {
                    bool isContains_rotateScreen = false;

                    string rotateScreen_watchApp_clean = rotateScreen_watchApp;
                    if (rotateScreen_watchApp.EndsWith("*"))
                    {
                        isContains_rotateScreen = true;
                        rotateScreen_watchApp_clean = rotateScreen_watchApp.TrimEnd('*');
                    }

                    Functions.MonitorOrientation orientation = Functions.CheckMonitorOrientation(rotateScreen_monitor);

                    bool isRunning = false;
                    if (isContains_rotateScreen) isRunning = Functions.CheckForRunningProcessContains(rotateScreen_watchApp_clean);
                    else isRunning = Functions.CheckForRunningProcess(rotateScreen_watchApp_clean);

                    if (isRunning)
                    {
                        if (orientation == Functions.MonitorOrientation.Portrait)
                        {
                            Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Landscape);
                        }
                    }
                    else
                    {
                        if (orientation == Functions.MonitorOrientation.Landscape)
                        {
                            Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Portrait);
                        }
                    }
                }

                if (bAppKill_enable)
                {
                    bool isContains_appKill = false;

                    string appKill_appName_clean = appKill_appName;
                    if (appKill_appName.EndsWith("*"))
                    {
                        isContains_appKill = true;
                        appKill_appName_clean = appKill_appName.TrimEnd('*');
                    }

                    bool isRunning = false;
                    if (isContains_appKill) isRunning = Functions.CheckForRunningProcessContains(appKill_appName_clean);
                    else isRunning = Functions.CheckForRunningProcess(appKill_appName_clean);

                    if (isRunning)
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
                            bool b = Functions.KillRunningProcess(appKill_appName_clean);
                        }

                    }
                }

                if (bUsbKill_enable)
                {
                    bool isContains_usbKill = false;

                    string usbKill_watchApp_clean = usbKill_watchApp;
                    if (usbKill_watchApp.EndsWith("*"))
                    {
                        isContains_usbKill = true;
                        usbKill_watchApp_clean = usbKill_watchApp.TrimEnd('*');
                    }

                    bool isRunning = false;
                    if (isContains_usbKill) isRunning = Functions.CheckForRunningProcessContains(usbKill_watchApp_clean);
                    else isRunning = Functions.CheckForRunningProcess(usbKill_watchApp_clean);

                    //get the device id before it's disabled...keep is safe.
                    if (usbKill_deviceId == string.Empty)
                    {
                        usbKill_deviceId = Functions.GetUsbDeviceId(usbKill_deviceName);

                        //query to find the led wiz device, then find its device id
                        if (usbKill_deviceId != string.Empty)
                        {
                            //then detect it if its there, before you write to it again.
                            //write this to a file, save it somewhere for the first boot up.

                            if (!System.IO.File.Exists(configFile))
                            {
                                //write to the config file
                                bool b = Functions.WriteDeviceIdToConfig(usbKill_deviceName, usbKill_deviceId, configFile);
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists(configFile))
                            {
                                //read from this file, get the id
                                usbKill_deviceId = Functions.GetUsbDeviceIdFromFile(usbKill_deviceName, configFile);
                            }
                            else
                            {
                                //INFO: there is no file and you need to reset your led wiz in Devices and Printers
                                string details = "No device id found for '" + usbKill_deviceName + "'";
                                bool b = Functions.WriteToLogFile(Functions.LoggingType.Warning, details, log_file);
                                details = null;
                            }
                        }

                        //if (usbKill_deviceId == string.Empty) usbKill_deviceId = @"USB\VID_FAFA&PID_00F0\6&12A4013&0&2";
                    }

                    if (isRunning)
                    {
                        if (!Functions.CheckForConnectedDevice(usbKill_deviceName))
                        {
                            //need to enable it
                            bool b = Functions.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, true);
                        }
                    }
                    else
                    {
                        if (Functions.CheckForConnectedDevice(usbKill_deviceName))
                        {
                            //need to disable it
                            bool b = Functions.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, false);
                        }
                    }
                }

                if (bMoveFile_enable)
                {
                    //get extensions list, or do all files
                    //get from folders list, make sure they are all folders that exist
                    //get to folders list, verify they all exist.
                    //if they dont, need to log it
                }

                Thread.Sleep(sleepTime);
            }
        }

        private static DateTime StartTimeNoException(System.Diagnostics.Process p)
        {
            try
            {
                return p.StartTime;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
