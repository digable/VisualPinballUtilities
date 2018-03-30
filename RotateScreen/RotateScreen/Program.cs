﻿using System;
using System.Configuration;
using System.Threading;

//TODO: have to so the playfield is the one in focus if pinballx is running and the direct b2s options are NOT open
//TODO: need to close out the longest running one if there are multiples.

namespace RotateScreen
{
    class Program
    {
        private static int sleepTime = 1;
        private static int osVersion = 32;

        private static string rotateScreen_enable = ConfigurationManager.AppSettings["rotate-screen_enable"].ToLower();
        private static string rotateScreen_watchApp = ConfigurationManager.AppSettings["rotate-screen_watchApp"].ToLower();
        private static int rotateScreen_monitor = 2;

        private static string appKill_enable = ConfigurationManager.AppSettings["app-kill_enable"].ToLower();
        private static string appKill_appName = ConfigurationManager.AppSettings["app-kill_appName"].ToLower();
        private static string appKill_watchApp = ConfigurationManager.AppSettings["app-kill_watchApp"].ToLower();

        private static string usbKill_enable = ConfigurationManager.AppSettings["usb-kill_enable"].ToLower();
        private static string usbKill_deviceName = ConfigurationManager.AppSettings["usb-kill_deviceName"].ToLower();
        private static string usbKill_watchApp = ConfigurationManager.AppSettings["usb-kill_watchApp"].ToLower();

        static void Main(string[] args)
        {
            bool bRotateScreen_enable = false;
            bool bAppKill_enable = false;
            bool bUsbKill_enable = false;

            //isContains
            bool isContains_rotateScreen = false;
            if (rotateScreen_watchApp.EndsWith("*"))
            {
                isContains_rotateScreen = true;
                rotateScreen_watchApp = rotateScreen_watchApp.TrimEnd('*');
            }

            bool isContains_appKill = false;
            if (appKill_appName.EndsWith("*"))
            {
                isContains_appKill = true;
                appKill_appName = appKill_appName.TrimEnd('*');
            }

            bool isContains_usbKill = false;
            if (usbKill_watchApp.EndsWith("*"))
            {
                isContains_usbKill = true;
                usbKill_watchApp = usbKill_watchApp.TrimEnd('*');
            }

            string usbKill_deviceId = string.Empty;

            try
            {
                sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["sleepTime"]) * 1000;
            }
            catch (Exception) { }

            try
            {
                osVersion = Convert.ToInt32(ConfigurationManager.AppSettings["os-version"]);
            }
            catch (Exception) { }
            try
            {
                rotateScreen_monitor = Convert.ToInt32(ConfigurationManager.AppSettings["rotate-screen_monitor"]);
            }
            catch (Exception) { }

            try
            {
                bRotateScreen_enable = Convert.ToBoolean(rotateScreen_enable);
            }
            catch (Exception) { }

            try
            {
                bAppKill_enable = Convert.ToBoolean(appKill_enable);
            }
            catch (Exception) { }

            try
            {
                bUsbKill_enable = Convert.ToBoolean(usbKill_enable);
            }
            catch (Exception) { }

            while (true)
            {
                if (bRotateScreen_enable)
                {
                    Functions.MonitorOrientation orientation = Functions.CheckMonitorOrientation(rotateScreen_monitor);

                    bool isRunning = false;
                    if (isContains_rotateScreen) isRunning = Functions.CheckForRunningProcessContains(rotateScreen_watchApp);
                    else isRunning = Functions.CheckForRunningProcess(rotateScreen_watchApp);

                    if (isRunning)
                    {
                        if (orientation == Functions.MonitorOrientation.Portrait)
                        {
                            Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Landscape);
                        }
                    }
                    else
                    {
                        if (orientation == Functions.MonitorOrientation.Landscape)
                        {
                            Functions.RotateMonitor(rotateScreen_monitor, Functions.MonitorOrientation.Portrait);
                        }
                    }
                }

                if (bAppKill_enable)
                {
                    //bool isRunning = false;
                    //if (isContains_appKill) isRunning = Functions.CheckForRunningProcessContains(appKill_appName);
                    //else isRunning = Functions.CheckForRunningProcess(appKill_appName);

                    //if (isRunning)
                    //{
                    //    string appKill_watchApp_clean = appKill_watchApp;
                    //    bool appKillWatchAppBool = true;
                    //    if (appKill_watchApp.StartsWith("[-]"))
                    //    {
                    //        appKillWatchAppBool = false;
                    //        appKill_watchApp_clean = appKill_watchApp.Replace("[-]", string.Empty).Trim();
                    //    }
                    //    else if (appKill_watchApp.StartsWith("[+]"))
                    //    {
                    //        appKillWatchAppBool = true;
                    //        appKill_watchApp_clean = appKill_watchApp.Replace("[+]", string.Empty).Trim();
                    //    }

                    //    if (Functions.CheckForRunningProcess(appKill_watchApp_clean) == appKillWatchAppBool)
                    //    {
                    //        bool b = Functions.KillRunningProcess(appKill_appName);
                    //    }

                    //}
                }

                if (bUsbKill_enable)
                {
                    bool isRunning = false;
                    if (isContains_usbKill) isRunning = Functions.CheckForRunningProcessContains(usbKill_watchApp);
                    else isRunning = Functions.CheckForRunningProcess(usbKill_watchApp);

                    //get the device id before it's disabled...keep is safe.
                    if (usbKill_deviceId == string.Empty)
                    {
                        usbKill_deviceId = Functions.GetUsbDeviceId(usbKill_deviceName);
                    }

                    if (isRunning)
                    {
                        if (!Functions.CheckForConnectedDevice(usbKill_deviceName))
                        {
                            //need to enable it
                            bool b = Functions.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, true);
                        }
                    }
                    else
                    {
                        if (Functions.CheckForConnectedDevice(usbKill_deviceName))
                        {
                            //need to disable it
                            bool b = Functions.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, false);
                        }
                    }
                }

                Thread.Sleep(sleepTime);
            }
        }
    }
}
