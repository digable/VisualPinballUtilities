﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class USBKill
    {
        public static void IsEnabled(Models.USBKill uk, int osVersion, string[] runningProcesses)
        {
            bool isRunning = true;
            if (uk.IsContains)
            {
                string watchProcessName = runningProcesses.Where(p => p.Contains(uk.WatchApplication)).FirstOrDefault();
                if (watchProcessName != null) isRunning = true;
                else isRunning = false;
                watchProcessName = null;
            }
            else isRunning = runningProcesses.Contains(uk.WatchApplication);
            runningProcesses = null;

            if (isRunning)
            {
                if (!Utilities.CheckForConnectedDevice(uk.KillDeviceName))
                {
                    //need to enable it
                    Utilities.ChangeStatusOfUSBDevice(uk.KillDeviceId, osVersion, true);
                }
            }
            else
            {
                if (Utilities.CheckForConnectedDevice(uk.KillDeviceName))
                {
                    //need to disable it
                    Utilities.ChangeStatusOfUSBDevice(uk.KillDeviceId, osVersion, false);
                }
            }

            uk = null;
        }
    }
}
