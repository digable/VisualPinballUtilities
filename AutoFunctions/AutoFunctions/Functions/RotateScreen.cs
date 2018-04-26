using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class RotateScreen
    {
        public static void IsEnabled(Models.RotateScreen rs, string[] runningProcesses)
        {
            Utilities.MonitorOrientation orientation = Utilities.CheckMonitorOrientation(rs.Monitor);

            bool isRunning = false;
            if (rs.IsContains)
            {
                var watchProcessName = runningProcesses.Where(p => p.Contains(rs.WatchApplication)).FirstOrDefault();
                if (watchProcessName != null) isRunning = true;
                watchProcessName = null;
            }
            else isRunning = runningProcesses.Contains(rs.WatchApplication);
            runningProcesses = null;

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

            rs = null;
        }
    }
}
