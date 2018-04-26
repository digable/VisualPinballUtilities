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
        //private static string moveFile_enable = ConfigurationManager.AppSettings["move-file_enable"];
        //private static string moveFile_overwrite = ConfigurationManager.AppSettings["move-file_overwrite"];
        //private static string moveFile_extensions = ConfigurationManager.AppSettings["move-file_extensions"];
        //private static string moveFile_fromFolders = ConfigurationManager.AppSettings["move-file_fromFolders"];
        //private static string moveFile_toFolders = ConfigurationManager.AppSettings["move-file_toFolders"];

        static void Main(string[] args)
        {
            //INFO: setup globals for service
            Models.Global g = new Models.Global();

            //INFO: get rotate screen parameters
            Models.RotateScreen rs = new Models.RotateScreen(g.LogFile);

            //INFO: check to see if there is another one running, kill them all
            bool serviceIsRunning = Functions.RunOnce.CheckInstances(rs.Monitor);
            //INFO: remove from memory rotate screen if its not enabled
            if (!rs.Enabled) rs = null;

            Models.AppKill ak = new Models.AppKill(g.LogFile);
            //INFO: remove from memory app kill if its not enabled
            if (!ak.Enabled) ak = null;

            Models.USBKill uk = new Models.USBKill(g.LogFile, g.ConfigFile);
            //INFO: remove from memory usb kill if its not enabled
            if (!uk.Enabled) uk = null;

            Models.MoveFile mf = new Models.MoveFile(g.LogFile);
            //INFO: remove from memory move file if its not enabled
            if (!mf.Enabled) mf = null;

            //INFO: this is the service
            while (g.IsServiceRunning)
            {
                if (rs != null && rs.Enabled) Functions.RotateScreen.IsEnabled(rs);

                if (ak != null && ak.Enabled) Functions.AppKill.IsEnabled(ak);

                if (uk != null && uk.Enabled) Functions.USBKill.IsEnabled(uk, g.OSVersion);

                if (mf != null && mf.Enabled) Functions.MoveFile.IsEnabled(mf, g.LogFile);

                //INFO: log if there are no services running, close out the application
                if (rs == null && ak == null && uk == null && mf == null)
                {
                    g.IsServiceRunning = false;
                    string details = "None of the functions are enabled, please check your config.";
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, g.LogFile);
                    details = null;
                }

                Thread.Sleep(g.SleepTime);
            }
        }
    }
}
