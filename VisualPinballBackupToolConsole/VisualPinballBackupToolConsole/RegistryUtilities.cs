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

                            string[] roms = vpinmameKey.GetSubKeyNames();

                            foreach (string rom in roms)
                            {
                                RegistryKey romKey = Registry.CurrentUser.OpenSubKey(SubKeyFreewareVisualPinMame + @"\" + rom, true);
                                object value = 0;
                                try
                                {
                                    value = (int)romKey.GetValue(field);
                                }
                                catch (Exception)
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
