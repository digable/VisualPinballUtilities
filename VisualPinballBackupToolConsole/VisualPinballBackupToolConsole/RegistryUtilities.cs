using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace VisualPinballBackupToolConsole
{
    public class RegistryUtilities
    {
        public class Update
        {
            //TODO: this is only good for a specific subkey, need to verify this and refactor, onlt for roms
            public static bool Key(string subKey, string field, object newValue)
            {
                bool b = true;

                RegistryKey vpinmameKey = Registry.CurrentUser.OpenSubKey(subKey);

                string[] roms = vpinmameKey.GetSubKeyNames();

                foreach (string rom in roms)
                {
                    RegistryKey romKey = Registry.CurrentUser.OpenSubKey(subKey + @"\" + rom, true);
                    object value = 0;
                    try
                    {
                        value = (int)romKey.GetValue(field);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("'" + rom + "' doesn't have a '" + field + "' field.  Skipping...");
                    }

                    if (value != newValue)
                    {
                        romKey.SetValue(field, newValue);
                        Console.WriteLine("Successfully updated '" + field + "' value to 1 for rom '" + rom + "'.");
                    }
                    else
                    {
                        Console.WriteLine("'" + field + "' value is already set to 1 for rom '" + rom + "'.");
                    }

                    romKey = null;
                }

                roms = null;
                Console.WriteLine("Finished updating roms.");

                return b;
            }
        }
    }
}
