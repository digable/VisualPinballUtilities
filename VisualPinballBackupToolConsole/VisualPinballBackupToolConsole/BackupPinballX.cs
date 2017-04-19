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
    public class BackupPinballX
    {
        public BackupPinballX()
        {
            List<string> processedItems = new List<string>();

            string dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd");

            string backupDirectory = @"C:\temp1";
            string backupFilename = "vpin-backup_" + dateTimeNow + ".zip";

            string emulatorsDirectory = @"C:\temp";//@"C:\Emulators";
            string emulatorsCompressedFile = "Emulators.zip";
            bool emulatorProcessing = ProcessDirectory(emulatorsDirectory, emulatorsCompressedFile, backupDirectory, backupFilename);
            if (emulatorProcessing) processedItems.Add(emulatorsCompressedFile);

            string procDirectory = @"C:\P-ROC";
            string procCompressedFile = "P-ROC.zip";

            string pinscapeDirectory = Environment.SpecialFolder.MyDocuments.ToString().TrimEnd('\\') + @"\" +  @"Pinscape";
            string pinscapeCompressedFile = "Pinscape.zip";

            if (!Directory.Exists(backupDirectory + @"\" + @"registry-exports"))
            {
                Directory.CreateDirectory(backupDirectory + @"\" + @"registry-exports");
            }
            //INFO: visual pinmame
            RegistryUtilities.VisualPinMame.Backup.All(backupDirectory + @"\" + @"registry-exports" + @"\" + "visual-pinmame_" + dateTimeNow + ".reg");

            string registryCompressedFile = "registry-exports.zip";

            //TODO: need to put the pindmd.dll and .ini file in the systemWOW64 folder
        }

        public bool ProcessDirectory(string sourceDirectory, string backupFilename, string backupDirectory, string masterBackupFilename)
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
