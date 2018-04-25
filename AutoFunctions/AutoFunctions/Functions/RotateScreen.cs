using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class RotateScreen
    {
        public static void IsEnabled(int rotateScreen_monitor, bool isContains_rotateScreen, string rotateScreen_watchApp_clean)
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
    }
}
