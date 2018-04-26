using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Functions
{
    class MoveFile
    {
        public static void IsEnabled(Models.MoveFile mf, string logFile)
        {
            //INFO: get the files that need to be moved
            bool allExtensions = false;
            if (mf.FileExtensions.Contains("*")) allExtensions = true;

            Dictionary<string, List<string>> dMovingFiles = new Dictionary<string, List<string>>();
            for (int i = 0; i < mf.FromFolders.Count(); i++)
            {
                if (mf.SkipFolderIndices.Contains(i)) continue;
                else
                {
                    if (allExtensions)
                    {
                        //TODO: maybe log this
                        string[] fromFiles = Directory.GetFiles(mf.FromFolders[i], "*.*", SearchOption.TopDirectoryOnly);
                        dMovingFiles.Add(mf.ToFolders[i], fromFiles.ToList());
                        fromFiles = null;
                    }
                    else
                    {
                        foreach (string extension in mf.FileExtensions)
                        {
                            //TODO: maybe log this
                            string[] fromFiles = Directory.GetFiles(mf.FromFolders[i], "*." + extension.TrimStart('.'), SearchOption.TopDirectoryOnly);
                            dMovingFiles.Add(mf.ToFolders[i], fromFiles.ToList());
                            fromFiles = null;
                        }
                    }
                }
            }

            //INFO: move the files
            foreach(string toFolder in dMovingFiles.Keys)
            {
                foreach (string fromFile in dMovingFiles[toFolder])
                {
                    string toFile = toFolder.TrimEnd('\\') + @"\" + Path.GetFileName(fromFile);

                    if (File.Exists(toFile))
                    {
                        string details = "File '" + Path.GetFileName(fromFile) + "' already exists in To Folder.";
                        bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Information, Utilities.ApplicationFunction.MoveFile, details, logFile);
                        details = null;

                        if (mf.Overwrite)
                        {
                            details = "Deleting '" + Path.GetFileName(fromFile) + "' in To Folder.";
                            b = Utilities.WriteToLogFile(Utilities.LoggingType.Information, Utilities.ApplicationFunction.MoveFile, details, logFile);
                            details = null;
                            File.Delete(toFile);
                            File.Move(fromFile, toFile);
                        }
                        else
                        {
                            details = "Overwrite is set to '" + mf.Overwrite + "'.  File will not be removed.";
                            b = Utilities.WriteToLogFile(Utilities.LoggingType.Information, Utilities.ApplicationFunction.MoveFile, details, logFile);
                            details = null;
                        }
                    }
                    else
                    {
                        File.Move(fromFile, toFile);
                    }

                    toFile = null;
                }
            }
            dMovingFiles = null;
        }
    }
}
