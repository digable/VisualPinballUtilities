﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AutoFunctions
{
    class Utilities
    {
        public static bool CheckForRunningProcess(string processName)
        {
            //return Process.GetProcesses().Any<Process>(p => p.ProcessName.ToLower().Equals(processName.ToLower()));
            Process[] processes = Process.GetProcesses();
            bool b = processes.Any<Process>(p => p.ProcessName.ToLower().Equals(processName.ToLower()));
            processes = null;
            processName = null;
            return b;
        }

        public static bool CheckForRunningProcessContains(string processName)
        {
            //return Process.GetProcesses().Any<Process>(p => p.ProcessName.ToLower().Contains(processName.ToLower()));
            Process[] processes = Process.GetProcesses();
            bool b = processes.Any<Process>(p => p.ProcessName.ToLower().Contains(processName.ToLower()));
            processes = null;
            processName = null;
            return b;
        }

        public static bool CheckForConnectedDevice(string deviceName)
        {
            //return USBLib.USB.GetConnectedDevices().Any<USBLib.USB.USBDevice>(d => d.Product.ToLower().Equals(deviceName.ToLower()));
            List<USBLib.USB.USBDevice> l = USBLib.USB.GetConnectedDevices();
            bool b = l.Any<USBLib.USB.USBDevice>(d => d.Product.ToLower().Equals(deviceName.ToLower()));
            l = null;
            deviceName = null;
            return b;
        }

        public static string[] RunningProcesses()
        {
            string[] s = new string[] { };

            //s = Process.GetProcesses().Select(p => p.ProcessName.ToLower()).ToArray();
            Process[] processes = Process.GetProcesses();
            s = processes.Select(p => p.ProcessName.ToLower()).ToArray();
            processes = null;

            return s;
        }

        public static void KillRunningProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process p in processes)
            {
                p.Kill();
            }
            processes = null;
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
            char[] trimChars = new char[] { '\\' };

            int num = 0;
            if (orientation == MonitorOrientation.Portrait) num = 90; 

            string[] commandArray = new string[] { "/C display.exe /rotate:", num.ToString(), " /device:", deviceIndex.ToString(), " /toggle" };
            string str = string.Concat(commandArray);
            commandArray = null;
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\",
                    Arguments = str,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            str = null;
            trimChars = null;
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

            deviceName = null;
            //HACK
            if (s == null) s = string.Empty;

            return s;
        }

        public static void ChangeStatusOfUSBDevice(string usbDeviceId, int osVersion, bool enable)
        {
            char[] trimChars = new char[] { '\\' };

            string enDisable = "enable";
            if (!enable) enDisable = "disable";

            string[] commandArray = new string[] { "/C devcon" + osVersion + ".exe " + enDisable + " \"@" + usbDeviceId + "\"" };
            enDisable = null;
            string str = string.Concat(commandArray);
            commandArray = null;
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).TrimEnd(trimChars) + @"\",
                    Arguments = str,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            str = null;
            trimChars = null;
            process.Start();
        }

        public static void WriteDeviceIdToConfig(string deviceName, string deviceId, string configFile)
        {
            StreamWriter sw = new StreamWriter(configFile);
            sw.WriteLine(deviceName + "|" + deviceId);
            sw.Close();
            sw.Dispose();

            deviceName = null;
            deviceId = null;
            configFile = null;
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
                    lineSplit = null;
                }
            }

            sr.Close();
            sr.Dispose();

            deviceName = null;
            configFile = null;

            return s;
        }

        public static void WriteToLogFile(LoggingType loggingType, ApplicationFunction appFunction, string details, Models.Global g)
        {
            WriteToLogFile(loggingType, appFunction, details, g.LoggingEnabled, g.LogFile);
            details = null;
            g = null;
        }

        public static void WriteToLogFile(LoggingType loggingType, ApplicationFunction appFunction, string details, bool loggingEnabled, string logFile)
        {
            if (loggingEnabled)
            {
                StreamWriter sw = new StreamWriter(logFile, true);
                sw.WriteLine(loggingType.ToString() + "|" + DateTime.Now.ToString() + "|" + appFunction + "|" + details);
                sw.Close();
                sw.Dispose();
            }
            details = null;
            logFile = null;
        }

        public enum ApplicationFunction
        {
            Global = 0,
            RotateScreen = 1,
            AppKill = 2,
            USBKill = 3,
            MoveFile = 4
        }

        public enum LoggingType
        {
            Information = 1,
            Warning = 2,
            Error = 3
        }
    }
}
