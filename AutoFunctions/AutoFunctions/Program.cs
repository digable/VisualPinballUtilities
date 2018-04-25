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
        //private static string usbKill_enable = ConfigurationManager.AppSettings["usb-kill_enable"].ToLower();
        //private static string usbKill_deviceName = ConfigurationManager.AppSettings["usb-kill_deviceName"].ToLower();
        //private static string usbKill_watchApp = ConfigurationManager.AppSettings["usb-kill_watchApp"].ToLower();

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
            //INFO: remove from memory app kill if its not enabled
            if (ak.Enabled == false) ak = null;

            Models.USBKill uk = new Models.USBKill(g.LogFile, g.ConfigFile);
            //INFO: remove from memory usb kill if its not enabled
            if (uk.Enabled == false) uk = null;

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
            //    bUsbKill_enable = Convert.ToBoolean(usbKill_enable);
            //}
            //catch (Exception) { }

            while (serviceIsRunning)
            {
                if (rs != null && rs.Enabled) Functions.RotateScreen.IsEnabled(rs.Monitor, rs.IsContains, rs.WatchApplication);

                if (ak != null && ak.Enabled) Functions.AppKill.IsEnabled(ak.IsContains, ak.WatchApplication, ak.KillApplication);

                if (uk != null && uk.Enabled) Functions.USBKill.IsEnabled(uk.IsContains, uk.WatchApplication, uk.KillDeviceId, uk.KillDeviceName, g.OSVersion);
  
                if (bMoveFile_enable)
                {
                    

                    //get extensions list, or do all files
                }

                Thread.Sleep(g.SleepTime);
            }
        }
    }
}
