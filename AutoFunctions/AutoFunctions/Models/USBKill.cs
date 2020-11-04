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
        public string WatchApplication { get; set; } = ConfigurationManager.AppSettings["usb-kill_watchApp"].ToLower().Trim();
        public string KillDeviceNamesString { get; set; } = ConfigurationManager.AppSettings["usb-kill_deviceNames"].ToLower().Trim();
        public List<Device> KillDevices { get; set; } = new List<Device>();
        //public string KillDeviceId { get; set; }
        //INFO: this is the default for LED-Wiz --> @"USB\VID_FAFA&PID_00F0\6&12A4013&0&2"
        public bool IsContains { get; set; } = false;

        private string P_enable = ConfigurationManager.AppSettings["usb-kill_enable"].ToLower();

        public USBKill(Global g)
        {
            //Enabled
            try
            {
                Enabled = Convert.ToBoolean(P_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + P_enable + "' isn't a valid boolean.  Defaulting to '" + Enabled.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.USBKill, details, g);
                details = null;
            }
            P_enable = null;

            //WatchApplication
            if (WatchApplication.EndsWith("*"))
            {
                IsContains = true;
                WatchApplication = WatchApplication.TrimEnd('*');
            }

            if (KillDeviceNamesString.Contains(';'))
            {
                string[] kdnss = KillDeviceNamesString.Split(';');
                foreach (string kdns in kdnss)
                {
                    Device d = new Device();
                    d.Name = kdns.Trim();
                    KillDevices.Add(d);
                    d = null;
                }
            }
            else
            {
                Device d = new Device();
                d.Name = KillDeviceNamesString.Trim();
                KillDevices.Add(d);
                d = null;
            }

            foreach (Device kd in KillDevices)
            {
                //KillDeviceId
                kd.Id = Utilities.GetUsbDeviceId(kd.Name);

                //Start --> Write device id to file
                //INFO: query to find the device, then find its device id
                if (kd.Id != string.Empty)
                {
                    //then detect it if its there, before you write to it again.
                    //write this to a file, save it somewhere for the first boot up.
                    if (!System.IO.File.Exists(g.ConfigFile))
                    {
                        //write to the config file
                        Utilities.WriteDeviceIdToConfig(kd.Name, kd.Id, g.ConfigFile);
                    }
                }
                else
                {
                    if (System.IO.File.Exists(g.ConfigFile))
                    {
                        //read from this file, get the id
                        kd.Id = Utilities.GetUsbDeviceIdFromFile(kd.Name, g.ConfigFile);
                    }
                    else
                    {
                        //INFO: there is no file and you need to reset your led wiz in Devices and Printers
                        string details = "No device id found for '" + kd.Name + "'.  Remove your device from device manager, then unplug and replug in to reset.";
                        Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.USBKill, details, g);
                        details = null;
                    }
                }
                //Stop --> Write device id to file
            }
            g = null;
        }

        public class Device
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }
    }
}
