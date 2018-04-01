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
        public static bool CheckForRunningProcess(string processName) => Process.GetProcesses().Any<Process>(p => p.ProcessName.ToLower().Equals(processName.ToLower()));//.Contains(processName.ToLower()));
        public static bool CheckForRunningProcessContains(string processName) => Process.GetProcesses().Any<Process>(p => p.ProcessName.ToLower().Contains(processName.ToLower()));//.Contains(processName.ToLower()));
        public static bool CheckForConnectedDevice(string deviceName) => USBLib.USB.GetConnectedDevices().Any<USBLib.USB.USBDevice>(d => d.Product.ToLower().Equals(deviceName.ToLower()));

        public static bool KillRunningProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process p in processes)
            {
                p.Kill();
            }

            return true;
        }

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

        public static string GetUsbDeviceId(string deviceName)
        {
            string s = string.Empty;

            s = USBLib.USB.GetConnectedDevices().Where(d => d.Product.ToLower().Equals(deviceName.ToLower())).Select(d => d.InstanceID).FirstOrDefault();

            //HACK
            if (s == null) s = string.Empty;

            return s;
        }

        public static bool ChangeStatusOfUSBDevice(string usbDeviceId, int osVersion, bool enable)
        {
            bool b = true;

            string enDisable = "enable";
            if (!enable) enDisable = "disable";

            string[] commandArray = new string[] { "/C devcon" + osVersion + ".exe " + enDisable + " \"@" + usbDeviceId + "\"" };
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

            return b;
        }

        public static bool WriteDeviceIdToConfig(string deviceName, string deviceId, string configFile)
        {
            bool b = true;

            StreamWriter sw = new StreamWriter(configFile);
            sw.WriteLine(deviceName + "|" + deviceId);
            sw.Close();
            sw.Dispose();

            return b;
        }

        public static string GetUsbDeviceIdFromFile(string deviceName, string configFile)
        {
            string s = string.Empty;

            string line;
            StreamReader sr = new StreamReader(configFile);
            
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("#")) continue; //these are comments

                if (line.ToLower().Contains('|'))
                {
                    string[] lineSplit = line.Split('|');
                    if (lineSplit[0].Trim().ToLower() == deviceName.Trim().ToLower())
                    {
                        s = lineSplit[1].Trim();
                        break;
                    }
                }
            }

            return s;
        }

        public static bool WriteToLogFile(LoggingType loggingType, string details, string logFile)
        {
            bool b = true;

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine(loggingType.ToString() + "|" + DateTime.Now.ToString() + "|" + details);
            sw.Close();
            sw.Dispose();

            return b;
        }

        public enum LoggingType
        {
            Information = 1,
            Warning = 2,
            Error = 3
        }
    }
}
