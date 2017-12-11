using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RotateScreen
{
    class Functions
    {
        public static bool CheckForRunningProcess(string processName) => Process.GetProcesses().Any<Process>(p => p.ProcessName.ToLower().Contains(processName.ToLower()));

        public static MonitorOrientation CheckMonitorOrientation(int deviceIndex = 1)
        {
            MonitorOrientation mo = MonitorOrientation.Landscape;
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.DeviceName.EndsWith(deviceIndex.ToString()))
                {
                    if (screen.Bounds.Height > screen.Bounds.Width)
                    {
                        return MonitorOrientation.Portrait;
                    }
                    return MonitorOrientation.Landscape;
                }
            }
            return mo;
        }

        public static void RotateMonitor(int deviceIndex = 1, MonitorOrientation orientation = MonitorOrientation.Landscape)
        {
            int num = 0;
            if (orientation == MonitorOrientation.Portrait)
            {
                num = 90;
            }
            string[] commandArray = new string[] { "/C display.exe /rotate:", num.ToString(), " /device:", deviceIndex.ToString(), " /toggle" };
            string str = string.Concat(commandArray);
            Process process = new Process
            {
                StartInfo = { FileName = "cmd.exe" }
            };
            char[] trimChars = new char[] { '\\' };
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\";
            process.StartInfo.Arguments = str;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
        }

        public enum MonitorOrientation
        {
            Landscape = 1,
            Portrait = 2
        }
    }
}
