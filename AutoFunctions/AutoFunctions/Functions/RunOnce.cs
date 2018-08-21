using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class RunOnce
    {
        public class Get
        {
            public static int RotateScreenMonitor(string rotateScreen_monitorString, bool loggingEnabled, string logFile)
            {
                int rotateScreen_monitor = 1;
                try
                {
                    int temp_rotateScreen_monitor = Convert.ToInt32(rotateScreen_monitorString);
                    if (temp_rotateScreen_monitor <= 0)
                    {
                        string details = "Monitor number '" + rotateScreen_monitorString + "' must be greater than 0.  Defaulting to 1.";
                        Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.RotateScreen, details, loggingEnabled, logFile);
                        details = null;
                    }
                    else if (temp_rotateScreen_monitor > 99)
                    {
                        string details = "Monitor number '" + rotateScreen_monitorString + "' must be less than 99.  Defaulting to 1.";
                        Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.RotateScreen, details, loggingEnabled, logFile);
                        details = null;
                    }
                    else rotateScreen_monitor = temp_rotateScreen_monitor;
                }
                catch (Exception)
                {
                    string details = "Monitor number '" + rotateScreen_monitorString + "' isn't a valid integer.  Defaulting to 1.";
                    Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.RotateScreen, details, loggingEnabled, logFile);
                    details = null;
                }

                rotateScreen_monitorString = null;
                logFile = null;

                return rotateScreen_monitor;
            }
        }

        public static void CheckInstances(int rotateScreen_monitor, bool bypassCountRestriction = false)
        {
            //bool serviceIsRunning = true;
            Process[] processes = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location));

            if (processes.Length > 1 || bypassCountRestriction)
            {
                //the application is already running

                //post a message saying you are killing the app
                System.Windows.Forms.MessageBox.Show("RotateScreen is turning off.  To enable again, relaunch the application.", "Killing RotateScreen", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                //serviceIsRunning = false;

                //kill rotate screen
                var sorted = from p in processes orderby StartTimeNoException(p) ascending, p.Id select p;
                processes = null;

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
                sorted = null;
                //this kills this instance, it shouldn't make it this far
                Process.GetCurrentProcess().Kill();
            }
            
            //return serviceIsRunning;
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
