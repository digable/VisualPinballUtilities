using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class USBKill
    {
        public static void IsEnabled(Models.USBKill uk, int osVersion)
        {
            bool isRunning = false;
            if (uk.IsContains) isRunning = Utilities.CheckForRunningProcessContains(uk.WatchApplication);
            else isRunning = Utilities.CheckForRunningProcess(uk.WatchApplication);

            if (isRunning)
            {
                if (!Utilities.CheckForConnectedDevice(uk.KillDeviceName))
                {
                    //need to enable it
                    bool b = Utilities.ChangeStatusOfUSBDevice(uk.KillDeviceId, osVersion, true);
                }
            }
            else
            {
                if (Utilities.CheckForConnectedDevice(uk.KillDeviceName))
                {
                    //need to disable it
                    bool b = Utilities.ChangeStatusOfUSBDevice(uk.KillDeviceId, osVersion, false);
                }
            }
        }
    }
}
