using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace VisualPinballBackupToolConsole
{
    public class Backup
    {
        public class All
        {
            public All()
            {
                List<string> processedItems = new List<string>();

                //INFO: primary backup information
                string dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd");
                string backupDirectory = @"C:\temp";
                string backupFilename = "vpin-backup_" + dateTimeNow + ".zip";

                //INFO: main emulators folder
                string emulatorsDirectory = @"C:\Emulators";
                string emulatorsCompressedFile = "Emulators_" + dateTimeNow + ".zip";
                bool emulatorProcessing = Process.Folder(emulatorsDirectory, emulatorsCompressedFile, backupDirectory);
                if (emulatorProcessing) processedItems.Add(emulatorsCompressedFile);

                //INFO: p-roc folder
                string procDirectory = @"C:\P-ROC";
                string procCompressedFile = "P-ROC_" + dateTimeNow + ".zip";
                bool procProcessing = Process.Folder(procDirectory, procCompressedFile, backupDirectory);
                if (procProcessing) processedItems.Add(procCompressedFile);

                //INFO: pinscape folder
                string pinscapeDirectory = Environment.SpecialFolder.MyDocuments.ToString().TrimEnd('\\') + @"\" + @"Pinscape";
                string pinscapeCompressedFile = "Pinscape_" + dateTimeNow + ".zip";
                bool pinscapeProcessing = Process.Folder(pinscapeDirectory, pinscapeCompressedFile, backupDirectory);
                if (pinscapeProcessing) processedItems.Add(pinscapeCompressedFile);

                //INFO: registry items
                string registryDirectory = backupDirectory + @"\" + @"registry-exports";
                if (!Directory.Exists(registryDirectory))
                {
                    Directory.CreateDirectory(registryDirectory);
                }
                RegistryUtilities.B2S.Backup.All(registryDirectory + @"\" + "b2s_" + dateTimeNow + ".reg");
                RegistryUtilities.B2S_Software.Backup.All(registryDirectory + @"\" + "software-b2s_" + dateTimeNow + ".reg");
                RegistryUtilities.PinballX.Backup.All(registryDirectory + @"\" + "pinballx_" + dateTimeNow + ".reg");
                RegistryUtilities.SetDMD.Backup.All(registryDirectory + @"\" + "setdmd_" + dateTimeNow + ".reg");
                RegistryUtilities.UltraDMD.Backup.All(registryDirectory + @"\" + "ultradmd_" + dateTimeNow + ".reg");
                RegistryUtilities.VisualPinball.Backup.All(registryDirectory + @"\" + "visual-pinball_" + dateTimeNow + ".reg");
                RegistryUtilities.VisualPinMame.Backup.All(registryDirectory + @"\" + "visual-pinmame_" + dateTimeNow + ".reg");
                string registryCompressedFile = "registry-exports_" + dateTimeNow + ".zip";
                bool registryProcessing = Process.Folder(registryDirectory, registryCompressedFile, backupDirectory);
                if (registryProcessing) processedItems.Add(registryCompressedFile);

                //TODO: need to pull in all of the files from processed items and toss them into the backupfilename

                //TODO: need to copy the emulators\ultradmd\pindmd.dll and .ini file in the systemWOW64 folder
            }
        }

        class Process
        {
            public static bool Folder(string sourceDirectory, string backupFilename, string backupDirectory)
            {
                bool b = true;

                if (!Directory.Exists(backupDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(backupDirectory);
                    }
                    catch (Exception ex)
                    {
                        //TODO: couldnt create the backup directory
                        return false;
                    }
                }


                string zipPath = backupDirectory.TrimEnd('\\') + @"\" + backupFilename;
                //check for existing directory, if not, create it.
                //check to see if there is an existing file, if so, delete it
                //zip the directory and save
                //toss that file into the master zip file backup
                //need to check the zip and see if that file exists
                //should think about incremental backups

                if (File.Exists(zipPath))
                {
                    try
                    {
                        File.Delete(zipPath);
                    }
                    catch (Exception ex)
                    {
                        //TODO: couldn't remove current zip file
                        return false;
                    }
                }

                try
                {
                    ZipFile.CreateFromDirectory(sourceDirectory, zipPath);
                }
                catch (Exception ex)
                {
                    //couldnt zip directory
                    return false;
                }

                return b;
            }
        }
    }
}
