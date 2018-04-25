using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class RotateScreen
    {
        public static void IsEnabled(Models.RotateScreen rs)
        {
            Utilities.MonitorOrientation orientation = Utilities.CheckMonitorOrientation(rs.Monitor);

            bool isRunning = false;
            if (rs.IsContains) isRunning = Utilities.CheckForRunningProcessContains(rs.WatchApplication);
            else isRunning = Utilities.CheckForRunningProcess(rs.WatchApplication);

            if (isRunning)
            {
                if (orientation == Utilities.MonitorOrientation.Portrait)
                {
                    Utilities.RotateMonitor(rs.Monitor, Utilities.MonitorOrientation.Landscape);
                }
            }
            else
            {
                if (orientation == Utilities.MonitorOrientation.Landscape)
                {
                    Utilities.RotateMonitor(rs.Monitor, Utilities.MonitorOrientation.Portrait);
                }
            }
        }
    }
}
