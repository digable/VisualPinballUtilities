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

                string dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd");

                string backupDirectory = @"C:\temp";
                string backupFilename = "vpin-backup_" + dateTimeNow + ".zip";

                string emulatorsDirectory = @"C:\Emulators";//@"C:\Emulators";
                string emulatorsCompressedFile = "Emulators_" + dateTimeNow + ".zip";
                bool emulatorProcessing = ProcessDirectory(emulatorsDirectory, emulatorsCompressedFile, backupDirectory);
                if (emulatorProcessing) processedItems.Add(emulatorsCompressedFile);

                string procDirectory = @"C:\P-ROC";
                string procCompressedFile = "P-ROC_" + dateTimeNow + ".zip";
                bool procProcessing = ProcessDirectory(procDirectory, procCompressedFile, backupDirectory);
                if (procProcessing) processedItems.Add(procCompressedFile);

                string pinscapeDirectory = Environment.SpecialFolder.MyDocuments.ToString().TrimEnd('\\') + @"\" + @"Pinscape";
                string pinscapeCompressedFile = "Pinscape_" + dateTimeNow + ".zip";
                bool pinscapeProcessing = ProcessDirectory(pinscapeDirectory, pinscapeCompressedFile, backupDirectory);
                if (pinscapeProcessing) processedItems.Add(pinscapeCompressedFile);

                if (!Directory.Exists(backupDirectory + @"\" + @"registry-exports"))
                {
                    Directory.CreateDirectory(backupDirectory + @"\" + @"registry-exports");
                }
                //INFO: visual pinmame
                RegistryUtilities.VisualPinMame.Backup.All(backupDirectory + @"\" + @"registry-exports" + @"\" + "visual-pinmame_" + dateTimeNow + ".reg");

                string registryCompressedFile = "registry-exports_" + dateTimeNow + ".zip";

                //TODO: need to pull in all of the files from processed items and toss them into the backupfilename
            }
        }
        private static bool ProcessDirectory(string sourceDirectory, string backupFilename, string backupDirectory)
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
