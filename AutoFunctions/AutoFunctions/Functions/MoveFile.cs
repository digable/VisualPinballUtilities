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
        public static void IsEnabled(Models.MoveFile mf, string[] runningProcesses, string logFile)
        {
            bool isRunning = true;
            if (mf.WatchApplication != null && mf.WatchApplication != string.Empty)
            {
                if (mf.IsContains)
                {
                    var watchProcessName = runningProcesses.Where(p => p.Contains(mf.WatchApplication)).FirstOrDefault();
                    if (watchProcessName != null) isRunning = true;
                    watchProcessName = null;
                }
                else isRunning = runningProcesses.Contains(mf.WatchApplication);
            }
            runningProcesses = null;

            if (isRunning)
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
                            if (fromFiles.Length > 0) dMovingFiles.Add(mf.ToFolders[i], fromFiles.ToList());
                            fromFiles = null;
                        }
                        else
                        {
                            foreach (string extension in mf.FileExtensions)
                            {
                                //TODO: maybe log this
                                string[] fromFiles = Directory.GetFiles(mf.FromFolders[i], "*." + extension.TrimStart('.'), SearchOption.TopDirectoryOnly);
                                if (fromFiles.Length > 0) dMovingFiles.Add(mf.ToFolders[i], fromFiles.ToList());
                                fromFiles = null;
                            }
                        }
                    }
                }

                //INFO: move the files
                foreach (string toFolder in dMovingFiles.Keys)
                {
                    foreach (string fromFile in dMovingFiles[toFolder])
                    {
                        string toFile = toFolder.TrimEnd('\\') + @"\" + Path.GetFileName(fromFile);

                        if (File.Exists(toFile))
                        {
                            string details = "File '" + Path.GetFileName(fromFile) + "' already exists in '" + toFolder + "'.";
                            Utilities.WriteToLogFile(Utilities.LoggingType.Information, Utilities.ApplicationFunction.MoveFile, details, logFile);
                            details = null;

                            if (mf.Overwrite)
                            {
                                details = "Deleting '" + Path.GetFileName(fromFile) + "' from '" + toFolder + "'.";
                                Utilities.WriteToLogFile(Utilities.LoggingType.Information, Utilities.ApplicationFunction.MoveFile, details, logFile);
                                details = null;
                                File.Delete(toFile);
                                File.Move(fromFile, toFile);
                            }
                            else
                            {
                                details = "Overwrite is set to '" + mf.Overwrite + "'.  File will not be removed.";
                                Utilities.WriteToLogFile(Utilities.LoggingType.Information, Utilities.ApplicationFunction.MoveFile, details, logFile);
                                details = null;
                            }
                        }
                        else
                        {
                            File.Move(fromFile, toFile);
                            string details = "Moved '" + Path.GetFileName(fromFile) + "' to '" + toFolder + "'.";
                            Utilities.WriteToLogFile(Utilities.LoggingType.Information, Utilities.ApplicationFunction.MoveFile, details, logFile);
                            details = null;
                        }

                        toFile = null;
                    }
                }
                dMovingFiles = null;
            }

            mf = null;
            logFile = null;
        }
    }
}
