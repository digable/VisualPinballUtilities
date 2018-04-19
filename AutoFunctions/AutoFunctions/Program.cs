using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

using System.Linq;

//TODO: have to so the playfield is the one in focus if pinballx is running and the direct b2s options are NOT open
//TODO: need to close out the longest running one if there are multiples.
//TODO: need to use the config file for other things, remove app config once that is completed.
//TODO: check to see if the app is already running, if so, kill it, post a message, set the monitor to landscape

namespace AutoFunctions
{
    class Program
    {
        private static bool serviceIsRunning = true;
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
        private static string moveFile_overwrite = ConfigurationManager.AppSettings["move-file_overwrite"];
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
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.RotateScreen, details, log_file);
                details = null;
            }

            //check to see if there is another one running
            Process[] processes = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location));

            if (processes.Length > 1)
            {
                //the application is already running

                //post a message saying you are killing the app
                System.Windows.Forms.MessageBox.Show("RotateScreen is turning off.  To enable again, relaunch the application.", "Killing RotateScreen", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                serviceIsRunning = false;

                //kill rotate screen
                var sorted = from p in processes orderby StartTimeNoException(p) ascending, p.Id select p;

                foreach (var p in sorted)
                {
                    //need to kill the first one, then rotate the screen, then kill the second one
                    p.Kill();
                    //rotate screen to landscape
                    Utilities.MonitorOrientation orientation = Utilities.CheckMonitorOrientation(rotateScreen_monitor);
                    if (orientation == Utilities.MonitorOrientation.Portrait)
                    {
                        Utilities.RotateMonitor(rotateScreen_monitor, Utilities.MonitorOrientation.Landscape);
                    }
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
            bool isContains_rotateScreen = false;
            string rotateScreen_watchApp_clean = rotateScreen_watchApp;
            if (rotateScreen_watchApp.EndsWith("*"))
            {
                isContains_rotateScreen = true;
                rotateScreen_watchApp_clean = rotateScreen_watchApp.TrimEnd('*');
            }

            bool bAppKill_enable = false;
            bool isContains_appKill = false;
            string appKill_appName_clean = appKill_appName;
            if (appKill_appName.EndsWith("*"))
            {
                isContains_appKill = true;
                appKill_appName_clean = appKill_appName.TrimEnd('*');
            }

            bool bUsbKill_enable = false;
            bool isContains_usbKill = false;
            string usbKill_deviceId = string.Empty;
            string usbKill_watchApp_clean = usbKill_watchApp;
            if (usbKill_watchApp.EndsWith("*"))
            {
                isContains_usbKill = true;
                usbKill_watchApp_clean = usbKill_watchApp.TrimEnd('*');
            }


            bool bMoveFile_enable = false;
            bool bMoveFile_overwrite = false;

            try
            {
                bMoveFile_enable = Convert.ToBoolean(moveFile_enable);
            }
            catch (Exception) { }

            try
            {
                bMoveFile_overwrite = Convert.ToBoolean(moveFile_overwrite);
            }
            catch (Exception) { }

            //get all from folders
            string[] fromFolders = new string[] { };
            if (moveFile_fromFolders.Contains(";"))
            {
                fromFolders = moveFile_fromFolders.Split(';');
            }
            //get all to folders
            string[] toFolders = new string[] { };
            if (moveFile_toFolders.Contains(";"))
            {
                toFolders = moveFile_toFolders.Split(';');
            }
            //check the counts on the 2 folder vars, they need to be the same or write to log and disable
            if (fromFolders.Length != toFolders.Length)
            {
                //INFO: the folders do not match counts, log it
                string details = "From folder count '" + fromFolders.Count().ToString() + "' and To folder count '" + toFolders.Count().ToString() + "' do not match.  Check and update the config file.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, log_file);
                bMoveFile_enable = false;
            }

            //get from folders list, make sure they are all folders that exist
            List<int> folderSkipIndices = new List<int>();
            for (int i = 0; i < fromFolders.Length; i++)
            {
                string fromFolder = fromFolders[i];
                if (!System.IO.Directory.Exists(fromFolder))
                {
                    //log it, but continue on
                    string details = "From folder '" + fromFolder + "' does not exist.";
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, log_file);
                    if (!folderSkipIndices.Contains(i)) folderSkipIndices.Add(i);
                }
                fromFolder = null;
            }

            for (int i = 0; i < toFolders.Length; i++)
            {
                string toFolder = toFolders[i];
                if (!System.IO.Directory.Exists(toFolder))
                {
                    //log it, but continue on
                    string details = "To folder '" + toFolder + "' does not exist.";
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, log_file);
                    if (!folderSkipIndices.Contains(i)) folderSkipIndices.Add(i);
                }
                toFolder = null;
            }

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

            while (serviceIsRunning)
            {
                if (bRotateScreen_enable)
                {
                    Utilities.MonitorOrientation orientation = Utilities.CheckMonitorOrientation(rotateScreen_monitor);

                    bool isRunning = false;
                    if (isContains_rotateScreen) isRunning = Utilities.CheckForRunningProcessContains(rotateScreen_watchApp_clean);
                    else isRunning = Utilities.CheckForRunningProcess(rotateScreen_watchApp_clean);

                    if (isRunning)
                    {
                        if (orientation == Utilities.MonitorOrientation.Portrait)
                        {
                            Utilities.RotateMonitor(rotateScreen_monitor, Utilities.MonitorOrientation.Landscape);
                        }
                    }
                    else
                    {
                        if (orientation == Utilities.MonitorOrientation.Landscape)
                        {
                            Utilities.RotateMonitor(rotateScreen_monitor, Utilities.MonitorOrientation.Portrait);
                        }
                    }
                }

                if (bAppKill_enable)
                {
                    bool isRunning = false;
                    if (isContains_appKill) isRunning = Utilities.CheckForRunningProcessContains(appKill_appName_clean);
                    else isRunning = Utilities.CheckForRunningProcess(appKill_appName_clean);

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

                        if (Utilities.CheckForRunningProcess(appKill_watchApp_clean) == appKillWatchAppBool)
                        {
                            bool b = Utilities.KillRunningProcess(appKill_appName_clean);
                        }

                    }
                }

                if (bUsbKill_enable)
                {
                    bool isRunning = false;
                    if (isContains_usbKill) isRunning = Utilities.CheckForRunningProcessContains(usbKill_watchApp_clean);
                    else isRunning = Utilities.CheckForRunningProcess(usbKill_watchApp_clean);

                    //get the device id before it's disabled...keep is safe.
                    if (usbKill_deviceId == string.Empty)
                    {
                        usbKill_deviceId = Utilities.GetUsbDeviceId(usbKill_deviceName);

                        //query to find the led wiz device, then find its device id
                        if (usbKill_deviceId != string.Empty)
                        {
                            //then detect it if its there, before you write to it again.
                            //write this to a file, save it somewhere for the first boot up.

                            if (!System.IO.File.Exists(configFile))
                            {
                                //write to the config file
                                bool b = Utilities.WriteDeviceIdToConfig(usbKill_deviceName, usbKill_deviceId, configFile);
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists(configFile))
                            {
                                //read from this file, get the id
                                usbKill_deviceId = Utilities.GetUsbDeviceIdFromFile(usbKill_deviceName, configFile);
                            }
                            else
                            {
                                //INFO: there is no file and you need to reset your led wiz in Devices and Printers
                                string details = "No device id found for '" + usbKill_deviceName + "'";
                                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.USBKill, details, log_file);
                                details = null;
                            }
                        }

                        //if (usbKill_deviceId == string.Empty) usbKill_deviceId = @"USB\VID_FAFA&PID_00F0\6&12A4013&0&2";
                    }

                    if (isRunning)
                    {
                        if (!Utilities.CheckForConnectedDevice(usbKill_deviceName))
                        {
                            //need to enable it
                            bool b = Utilities.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, true);
                        }
                    }
                    else
                    {
                        if (Utilities.CheckForConnectedDevice(usbKill_deviceName))
                        {
                            //need to disable it
                            bool b = Utilities.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, false);
                        }
                    }
                }

                if (bMoveFile_enable)
                {
                    

                    //get extensions list, or do all files
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
