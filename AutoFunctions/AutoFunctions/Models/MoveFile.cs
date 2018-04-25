using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFunctions.Models
{
    class MoveFile
    {
        public bool Enabled { get; set; } = false;
        public bool Overwrite { get; set; } = false;
        public List<string> FileExtensions { get; set; } = new List<string>();
        public List<string> FromFolders { get; set; } = new List<string>();
        public List<string> ToFolders { get; set; } = new List<string>();
        public List<int> SkipFolderIndices { get; set; } = new List<int>();

        private string p_enable = ConfigurationManager.AppSettings["move-file_enable"];
        private string p_overwrite = ConfigurationManager.AppSettings["move-file_overwrite"];
        private string p_extensions = ConfigurationManager.AppSettings["move-file_extensions"];
        private string p_fromFolders = ConfigurationManager.AppSettings["move-file_fromFolders"];
        private string p_toFolders = ConfigurationManager.AppSettings["move-file_toFolders"];

        public MoveFile(string logFile)
        {
            //Enabled
            try
            {
                this.Enabled = Convert.ToBoolean(p_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + p_enable + "' isn't a valid boolean.  Defaulting to '" + this.Enabled.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, logFile);
                details = null;
            }
            p_enable = null;

            //Overwrite
            try
            {
                this.Overwrite = Convert.ToBoolean(p_overwrite);
            }
            catch (Exception)
            {
                string details = "Overwrite value '" + p_overwrite + "' isn't a valid boolean.  Defaulting to '" + this.Overwrite.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, logFile);
                details = null;
            }
            p_overwrite = null;

            //FileExtensions
            if (p_extensions.Contains(";"))
            {
                FileExtensions = p_extensions.Split(';').ToList();
            }
            p_extensions = null;

            //FromFolders
            if (p_fromFolders.Contains(";"))
            {
                FromFolders = p_fromFolders.Split(';').ToList();
            }
            p_fromFolders = null;

            //ToFolders
            if (p_toFolders.Contains(";"))
            {
                ToFolders = p_toFolders.Split(';').ToList();
            }
            p_toFolders = null;

            //Start --> Checks
            //INFO: check the counts on the 2 folder vars, they need to be the same or write to log and disable
            if (FromFolders.Count() != ToFolders.Count())
            {
                //INFO: the folders do not match counts, log it
                string details = "From folder count '" + FromFolders.Count().ToString() + "' and To folder count '" + ToFolders.Count().ToString() + "' do not match.  Check and update the config file.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, logFile);
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
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, logFile);
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
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, logFile);
                    details = null;
                    if (!SkipFolderIndices.Contains(i)) SkipFolderIndices.Add(i);
                }
                toFolder = null;
            }
            //Stop --> Checks
        }
    }
}
