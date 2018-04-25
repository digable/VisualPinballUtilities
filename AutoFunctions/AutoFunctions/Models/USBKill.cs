using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Models
{
    class USBKill
    {
        public bool Enabled { get; set; } = false;
        public string WatchApplication { get; set; } = ConfigurationManager.AppSettings["usb-kill_watchApp"].ToLower();
        public string KillDeviceName { get; set; } = ConfigurationManager.AppSettings["usb-kill_deviceName"].ToLower();
        public string KillDeviceId { get; set; }
        //INFO: this is the default for LED-Wiz --> @"USB\VID_FAFA&PID_00F0\6&12A4013&0&2"
        public bool IsContains { get; set; } = false;

        private string pUSBKill_enable = ConfigurationManager.AppSettings["usb-kill_enable"].ToLower();

        public USBKill(string logFile, string configFile)
        {
            //Enabled
            try
            {
                this.Enabled = Convert.ToBoolean(pUSBKill_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + pUSBKill_enable + "' isn't a valid boolean.  Defaulting to '" + this.Enabled.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.USBKill, details, logFile);
                details = null;
            }
            pUSBKill_enable = null;

            //WatchApplication
            if (this.WatchApplication.EndsWith("*"))
            {
                this.IsContains = true;
                this.WatchApplication = this.WatchApplication.TrimEnd('*');
            }

            //KillDeviceId
            this.KillDeviceId = Utilities.GetUsbDeviceId(this.KillDeviceName);

            //Start --> Write device id to file
            //INFO: query to find the led wiz device, then find its device id
            if (this.KillDeviceId != string.Empty)
            {
                //then detect it if its there, before you write to it again.
                //write this to a file, save it somewhere for the first boot up.
                if (!System.IO.File.Exists(configFile))
                {
                    //write to the config file
                    bool b = Utilities.WriteDeviceIdToConfig(this.KillDeviceName, this.KillDeviceId, configFile);
                }
            }
            else
            {
                if (System.IO.File.Exists(configFile))
                {
                    //read from this file, get the id
                    this.KillDeviceId = Utilities.GetUsbDeviceIdFromFile(this.KillDeviceName, configFile);
                }
                else
                {
                    //INFO: there is no file and you need to reset your led wiz in Devices and Printers
                    string details = "No device id found for '" + this.KillDeviceName + "'";
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.USBKill, details, logFile);
                    details = null;
                }
            }
            //Stop --> Write device id to file
        }
    }
}
