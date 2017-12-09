using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RotateScreen
{
    class Program
    {
        private static string appName = ConfigurationManager.AppSettings["appName"].ToLower();
        private static int monitor = 2;
        private static int sleepTime = 1;

        private static bool CheckForRunningProcess(string processName) =>
            Process.GetProcesses().Any<Process>(p => p.ProcessName.ToLower().Contains(processName.ToLower()));

        private static string CheckMonitorOrientation(int deviceIndex = 1)
        {
            string str = string.Empty;
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.DeviceName.EndsWith(deviceIndex.ToString()))
                {
                    if (screen.Bounds.Height > screen.Bounds.Width)
                    {
                        return "portrait";
                    }
                    return "landscape";
                }
            }
            return str;
        }

        static void Main(string[] args)
        {
            try
            {
                sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["sleepTime"]) * 1000;
            }
            catch (Exception)
            {
            }
            try
            {
                monitor = Convert.ToInt32(ConfigurationManager.AppSettings["monitor"]);
            }
            catch (Exception)
            {
            }
            while (true)
            {
                string str = CheckMonitorOrientation(monitor);
                if (CheckForRunningProcess(appName))
                {
                    if (str == "portrait")
                    {
                        RotateMonitor(monitor, "landscape");
                    }
                    Thread.Sleep(sleepTime);
                }
                else
                {
                    if (str == "landscape")
                    {
                        RotateMonitor(monitor, "portrait");
                    }
                    Thread.Sleep(sleepTime);
                }
            }
        }

        private static void RotateMonitor(int deviceIndex = 1, string orientation = "landscape")
        {
            int num = 0;
            if (orientation.ToLower() == "portrait")
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
    }
}
