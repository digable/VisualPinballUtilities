using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class USBKill
    {
        public static void IsEnabled(bool isContains_usbKill, string usbKill_watchApp_clean, string usbKill_deviceId, string usbKill_deviceName, int osVersion)
        {
            bool isRunning = false;
            if (isContains_usbKill) isRunning = Utilities.CheckForRunningProcessContains(usbKill_watchApp_clean);
            else isRunning = Utilities.CheckForRunningProcess(usbKill_watchApp_clean);

            if (isRunning)
            {
                if (!Utilities.CheckForConnectedDevice(usbKill_deviceName))
                {
                    //need to enable it
                    bool b = Utilities.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, true);
                }
            }
            else
            {
                if (Utilities.CheckForConnectedDevice(usbKill_deviceName))
                {
                    //need to disable it
                    bool b = Utilities.ChangeStatusOfUSBDevice(usbKill_deviceId, osVersion, false);
                }
            }
        }
    }
}
