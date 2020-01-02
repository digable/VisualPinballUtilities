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
        public class PinballX
        {
            const string SubKey = @"SOFTWARE\PinballX";

            public class Backup
            {
                public static bool All(string savePath)
                {
                    bool b = true;

                    RegistryUtilities.ExportKey(SubKey, savePath);

                    return b;
                }
            }
        }

        public class B2S
        {
            //need to to both sets
            const string SubKey = @"B2S";
            public class Backup
            {
                public static bool All(string savePath)
                {
                    bool b = true;

                    RegistryUtilities.ExportKey(SubKey, savePath);

                    return b;
                }
            }
        }

        public class B2S_Software
        {
            //need to to both sets
            const string SubKey = @"SOFTWARE\B2S";
            public class Backup
            {
                public static bool All(string savePath)
                {
                    bool b = true;

                    RegistryUtilities.ExportKey(SubKey, savePath);

                    return b;
                }
            }
        }

        public class VisualPinball
        {
            const string SubKey = @"SOFTWARE\Visual Pinball";

            public class Backup
            {
                public static bool All(string savePath)
                {
                    bool b = true;

                    RegistryUtilities.ExportKey(SubKey, savePath);

                    return b;
                }
            }
        }

        public class UltraDMD
        {
            const string SubKey = @"SOFTWARE\UltraDMD";

            public class Backup
            {
                public static bool All(string savePath)
                {
                    bool b = true;

                    RegistryUtilities.ExportKey(SubKey, savePath);

                    return b;
                }
            }
        }

        public class SetDMD
        {
            const string SubKey = @"SOFTWARE\SetDMD";

            public class Backup
            {
                public static bool All(string savePath)
                {
                    bool b = true;

                    RegistryUtilities.ExportKey(SubKey, savePath);

                    return b;
                }
            }
        }
        public class VisualPinMame
        {
            //INFO: these are the roms
            const string SubKeyFreewareVisualPinMame = @"SOFTWARE\Freeware\Visual PinMame";
            public class Backup
            {
                public static bool All(string savePath)
                {
                    bool b = true;

                    RegistryUtilities.ExportKey(SubKeyFreewareVisualPinMame, savePath);

                    return b;
                }
            }
            public class Update
            {
                public class Roms
                {
                    public class All
                    {
                        public static bool For(string field, object newValue)
                        {
                            bool b = true;

                            RegistryKey vpinmameKey = Registry.CurrentUser.OpenSubKey(SubKeyFreewareVisualPinMame);
                            if (vpinmameKey == null)
                            {
                                Console.WriteLine("It appears Visual PinMAME is not installed on this computer.  Exiting...");
                                return false;
                            }

                            string[] romNames = vpinmameKey.GetSubKeyNames();

                            foreach (string rom in romNames)
                            {
                                RegistryKey romKey = Registry.CurrentUser.OpenSubKey(SubKeyFreewareVisualPinMame + @"\" + rom, true);
                                object value = 0;
                                try
                                {
                                    //TODO: this might not always been an integer
                                    value = (int)romKey.GetValue(field);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("'" + rom + "' doesn't have a '" + field + "' field.  Skipping...");
                                }

                                //TODO: this check will not be equal unless the object types match, need to figure that out
                                if (value != newValue)
                                {
                                    romKey.SetValue(field, newValue);
                                    Console.WriteLine("Successfully updated '" + field + "' value to " + newValue + " for rom '" + rom + "'.");
                                }
                                else
                                {
                                    Console.WriteLine("'" + field + "' value is already set to " + newValue + " for rom '" + rom + "'.");
                                }

                                romKey = null;
                            }

                            romNames = null;
                            Console.WriteLine("Finished updating roms.");

                            return b;
                        }
                    }

                    public static bool Specific(string romName, string field, object newValue)
                    {
                        string[] romNames = new string[] { romName };
                        string[] fields = new string[] { field };
                        object[] newValues = new object[] { newValue };

                        bool b = Specific(romNames, fields, newValues);

                        newValues = null;
                        fields = null;
                        romNames = null;
                        newValue = null;
                        field = null;
                        romName = null;

                        return b;
                    }

                    public static bool Specific(string[] romNames, string[] fields, object[] newValues)
                    {
                        bool b = true;

                        if (fields.Length != newValues.Length)
                        {
                            Console.WriteLine("'Field and value array lengths are not the same.  Exiting...");
                            return false;
                        }
                        else
                        {
                            RegistryKey vpinmameKey = null;
                            try
                            {
                                vpinmameKey = Registry.CurrentUser.OpenSubKey(SubKeyFreewareVisualPinMame);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("It appears Visual PinMAME is not installed on this computer.  Exiting...");
                                return false;
                            }

                            foreach (string rom in romNames)
                            {
                                RegistryKey romKey = null;
                                try
                                {
                                    romKey = Registry.CurrentUser.OpenSubKey(SubKeyFreewareVisualPinMame + @"\" + rom, true);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Rom '" + rom + "' doesn't exist in the registry. Skipping to next rom.");
                                    continue;
                                }

                                for (int i = 0; i < fields.Length; i++)
                                {
                                    object value = 0;
                                    try
                                    {
                                        //TODO: this might not always been an integer
                                        value = (int)romKey.GetValue(fields[i]);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("'" + rom + "' doesn't have a '" + fields[i] + "' field.  Skipping...");
                                    }

                                    //TODO: this check will not be equal unless the object types match, need to figure that out
                                    if (value != newValues[i])
                                    {
                                        romKey.SetValue(fields[i], newValues[i]);
                                        Console.WriteLine("Successfully updated '" + fields[i] + "' value to " + newValues[i] + " for rom '" + rom + "'.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("'" + fields[i] + "' value is already set to " + newValues[i] + " for rom '" + rom + "'.");
                                    }
                                }
                                romKey = null;
                            }
                        }
                        romNames = null;
                        fields = null;
                        newValues = null;
                        Console.WriteLine("Finished updating roms.");

                        return b;
                    }
                }
            }
        }

        private static void ExportKey(string registryKey, string exportPath)
        {
            var proc = new System.Diagnostics.Process();
            try
            {
                proc.StartInfo.FileName = "regedit.exe";
                proc.StartInfo.UseShellExecute = false;
                proc = System.Diagnostics.Process.Start("regedit.exe", "/e " + exportPath + " " + registryKey + "");

                if (proc != null) proc.WaitForExit();
            }
            finally
            {
                if (proc != null) proc.Dispose();
            }

        }
    }
}
