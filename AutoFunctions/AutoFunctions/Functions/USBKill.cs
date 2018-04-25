using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class USBKill
    {
        public static void IsEnbaled(bool isContains_usbKill, string usbKill_watchApp_clean, string usbKill_deviceId, string usbKill_deviceName, string configFile, string logFile, int osVersion)
        {
            bool isRunning = false;
            if (isContains_usbKill) isRunning = Utilities.CheckForRunningProcessContains(usbKill_watchApp_clean);
            else isRunning = Utilities.CheckForRunningProcess(usbKill_watchApp_clean);

            //get the device id before it's disabled...keep is safe.
            if (usbKill_deviceId == string.Empty)
            {
                usbKill_deviceId = Utilities.GetUsbDeviceId(usbKill_deviceName);

                //query to find the led wiz device, then find its device id
                if (usbKill_deviceId != string.Empty)
                {
                    //then detect it if its there, before you write to it again.
                    //write this to a file, save it somewhere for the first boot up.

                    if (!System.IO.File.Exists(configFile))
                    {
                        //write to the config file
                        bool b = Utilities.WriteDeviceIdToConfig(usbKill_deviceName, usbKill_deviceId, configFile);
                    }
                }
                else
                {
                    if (System.IO.File.Exists(configFile))
                    {
                        //read from this file, get the id
                        usbKill_deviceId = Utilities.GetUsbDeviceIdFromFile(usbKill_deviceName, configFile);
                    }
                    else
                    {
                        //INFO: there is no file and you need to reset your led wiz in Devices and Printers
                        string details = "No device id found for '" + usbKill_deviceName + "'";
                        bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.USBKill, details, logFile);
                        details = null;
                    }
                }

                //if (usbKill_deviceId == string.Empty) usbKill_deviceId = @"USB\VID_FAFA&PID_00F0\6&12A4013&0&2";
            }

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
