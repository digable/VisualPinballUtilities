using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AutoFunctions.Models
{
    class MoveFile
    {
        public bool Enabled { get; set; } = false;
        public string WatchApplication { get; set; } = ConfigurationManager.AppSettings["move-file_watchApp"].ToLower().Trim();
        public bool Overwrite { get; set; } = false;
        public List<string> FileExtensions { get; set; } = new List<string>();
        public List<string> FromFolders { get; set; } = new List<string>();
        public List<string> ToFolders { get; set; } = new List<string>();
        public List<int> SkipFolderIndices { get; set; } = new List<int>();
        public bool IsContains { get; set; } = false;

        private string P_enable = ConfigurationManager.AppSettings["move-file_enable"];
        private string P_overwrite = ConfigurationManager.AppSettings["move-file_overwrite"];
        private string P_extensions = ConfigurationManager.AppSettings["move-file_extensions"];
        private string P_fromFolders = ConfigurationManager.AppSettings["move-file_fromFolders"];
        private string P_toFolders = ConfigurationManager.AppSettings["move-file_toFolders"];

        public MoveFile(Global g)
        {
            //Enabled
            try
            {
                Enabled = Convert.ToBoolean(P_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + P_enable + "' isn't a valid boolean.  Defaulting to '" + Enabled.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, g);
                details = null;
            }
            P_enable = null;

            //WatchApplication
            if (WatchApplication.EndsWith("*"))
            {
                IsContains = true;
                WatchApplication = WatchApplication.TrimEnd('*');
            }

            //Overwrite
            try
            {
                Overwrite = Convert.ToBoolean(P_overwrite);
            }
            catch (Exception)
            {
                string details = "Overwrite value '" + P_overwrite + "' isn't a valid boolean.  Defaulting to '" + Overwrite.ToString() + "'.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, g);
                details = null;
            }
            P_overwrite = null;

            //FileExtensions
            if (P_extensions.Contains(";"))
            {
                FileExtensions = P_extensions.Split(';').ToList();
            }
            FileExtensions.Add(P_extensions);
            FileExtensions.Remove(string.Empty);
            P_extensions = null;

            //FromFolders
            if (P_fromFolders.Contains(";"))
            {
                FromFolders = P_fromFolders.Split(';').ToList();
            }
            else FromFolders.Add(P_fromFolders);
            FromFolders.Remove(string.Empty);
            P_fromFolders = null;

            //ToFolders
            if (P_toFolders.Contains(";"))
            {
                ToFolders = P_toFolders.Split(';').ToList();
            }
            else ToFolders.Add(P_toFolders);
            ToFolders.Remove(string.Empty);
            P_toFolders = null;

            //Start --> Checks
            //INFO: check the counts on the 2 folder vars, they need to be the same or write to log and disable
            if (FromFolders.Count() != ToFolders.Count())
            {
                //INFO: the folders do not match counts, log it
                string details = "From folder count '" + FromFolders.Count().ToString() + "' and To folder count '" + ToFolders.Count().ToString() + "' do not match.  Check and update the config file.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, g);
                details = null;
                Enabled = false;
            }

            //INFO: check for missing folders, skip if not found
            for (int i = 0; i < FromFolders.Count(); i++)
            {
                string fromFolder = FromFolders[i];
                if (!System.IO.Directory.Exists(fromFolder))
                {
                    //log it, but continue on
                    string details = "From folder '" + fromFolder + "' does not exist.";
                    Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, g);
                    details = null;
                    if (!SkipFolderIndices.Contains(i)) SkipFolderIndices.Add(i);
                }
                fromFolder = null;
            }

            for (int i = 0; i < ToFolders.Count(); i++)
            {
                string toFolder = ToFolders[i];
                if (!System.IO.Directory.Exists(toFolder))
                {
                    //log it, but continue on
                    string details = "To folder '" + toFolder + "' does not exist.";
                    Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, g);
                    details = null;
                    if (!SkipFolderIndices.Contains(i)) SkipFolderIndices.Add(i);
                }
                toFolder = null;
            }

            if (FromFolders.Count() == SkipFolderIndices.Count())
            {
                //INFO: if we are skipping all of the folders, disable the service
                string details = "From folder count '" + FromFolders.Count().ToString() + "' and Skip Folder Indices count '" + SkipFolderIndices.Count().ToString() + "' match.  You shouldn't skip all folders you want to monitor.";
                Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, g);
                details = null;
                Enabled = false;
            }
            //Stop --> Checks

            g = null;
        }
    }
}
