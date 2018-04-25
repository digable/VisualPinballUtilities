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
            //INFO: setup globals for service
            Models.Global g = new Models.Global();

            //INFO: get rotate screen parameters
            Models.RotateScreen rs = new Models.RotateScreen(g.LogFile);

            //INFO: check to see if there is another one running, kill them all
            bool serviceIsRunning = Functions.RunOnce.CheckInstances(rs.Monitor);
            //INFO: remove from memory rotate screen if its not enabled
            if (rs.Enabled == false) rs = null;

            Models.AppKill ak = new Models.AppKill(g.LogFile);
            //INFO: remove from memory rotate screen if its not enabled
            if (ak.Enabled == false) ak = null;

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
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, g.LogFile);
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
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, g.LogFile);
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
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, g.LogFile);
                    if (!folderSkipIndices.Contains(i)) folderSkipIndices.Add(i);
                }
                toFolder = null;
            }

            //try
            //{
            //    bAppKill_enable = Convert.ToBoolean(appKill_enable);
            //}
            //catch (Exception) { }

            try
            {
                bUsbKill_enable = Convert.ToBoolean(usbKill_enable);
            }
            catch (Exception) { }

            while (serviceIsRunning)
            {
                if (rs != null && rs.Enabled) Functions.RotateScreen.IsEnabled(rs.Monitor, rs.IsContains, rs.WatchApplication);

                if (ak != null && ak.Enabled) Functions.AppKill.IsEnabled(ak.IsContains, ak.WatchApplication, ak.KillApplication);

                if (bUsbKill_enable) Functions.USBKill.IsEnbaled(isContains_usbKill, usbKill_watchApp_clean, usbKill_deviceId, usbKill_deviceName, g.ConfigFile, g.LogFile, g.OSVersion);
                //{
                    //bool isRunning = false;
                    //if (isContains_usbKill) isRunning = Utilities.CheckForRunningProcessContains(usbKill_watchApp_clean);
                    //else isRunning = Utilities.CheckForRunningProcess(usbKill_watchApp_clean);

                    ////get the device id before it's disabled...keep is safe.
                    //if (usbKill_deviceId == string.Empty)
                    //{
                    //    usbKill_deviceId = Utilities.GetUsbDeviceId(usbKill_deviceName);

                    //    //query to find the led wiz device, then find its device id
                    //    if (usbKill_deviceId != string.Empty)
                    //    {
                    //        //then detect it if its there, before you write to it again.
                    //        //write this to a file, save it somewhere for the first boot up.

                    //        if (!System.IO.File.Exists(configFile))
                    //        {
                    //            //write to the config file
                    //            bool b = Utilities.WriteDeviceIdToConfig(usbKill_deviceName, usbKill_deviceId, configFile);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (System.IO.File.Exists(configFile))
                    //        {
                    //            //read from this file, get the id
                    //            usbKill_deviceId = Utilities.GetUsbDeviceIdFromFile(usbKill_deviceName, configFile);
                    //        }
                    //        else
                    //        {
                    //            //INFO: there is no file and you need to reset your led wiz in Devices and Printers
                    //            string details = "No device id found for '" + usbKill_deviceName + "'";
                    //            bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.USBKill, details, log_file);
                    //            details = null;
                    //        }
                    //    }

                    //    //if (usbKill_deviceId == string.Empty) usbKill_deviceId = @"USB\VID_FAFA&PID_00F0\6&12A4013&0&2";
                    //}

                    //if (isRunning)
                    //{
                    //    if (!Utilities.CheckForConnectedDevice(usbKill_deviceName))
                    //    {
                    //        //need to enable it
                    //        bool b = Utilities.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, true);
                    //    }
                    //}
                    //else
                    //{
                    //    if (Utilities.CheckForConnectedDevice(usbKill_deviceName))
                    //    {
                    //        //need to disable it
                    //        bool b = Utilities.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, false);
                    //    }
                    //}
                //}

                if (bMoveFile_enable)
                {
                    

                    //get extensions list, or do all files
                }

                Thread.Sleep(g.SleepTime);
            }
        }

        //private static DateTime StartTimeNoException(System.Diagnostics.Process p)
        //{
        //    try
        //    {
        //        return p.StartTime;
        //    }
        //    catch
        //    {
        //        return DateTime.MinValue;
        //    }
        //}
    }
}
