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
        private List<string> FileExtensions { get; set; } = new List<string>();
        private List<string> FromFolders { get; set; } = new List<string>();
        private List<string> ToFolders { get; set; } = new List<string>();



        private string pMoveFile_enable = ConfigurationManager.AppSettings["move-file_enable"];
        private string pMoveFile_overwrite = ConfigurationManager.AppSettings["move-file_overwrite"];
        private string pMoveFile_extensions = ConfigurationManager.AppSettings["move-file_extensions"];
        private string pMoveFile_fromFolders = ConfigurationManager.AppSettings["move-file_fromFolders"];
        private string pMoveFile_toFolders = ConfigurationManager.AppSettings["move-file_toFolders"];

        public MoveFile(string logFile)
        {
            //Enabled
            try
            {
                this.Enabled = Convert.ToBoolean(pMoveFile_enable);
            }
            catch (Exception)
            {
                string details = "Enabled value '" + pMoveFile_enable + "' isn't a valid boolean.  Defaulting to '" + this.Enabled.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, logFile);
                details = null;
            }
            pMoveFile_enable = null;

            //Overwrite
            try
            {
                this.Enabled = Convert.ToBoolean(pMoveFile_overwrite);
            }
            catch (Exception)
            {
                string details = "Overwrite value '" + pMoveFile_overwrite + "' isn't a valid boolean.  Defaulting to '" + this.Overwrite.ToString() + "'.";
                bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.MoveFile, details, logFile);
                details = null;
            }
            pMoveFile_overwrite = null;

            //FileExtensions
            if (pMoveFile_extensions.Contains(";"))
            {
                FileExtensions = pMoveFile_extensions.Split(';').ToList();
            }
            pMoveFile_extensions = null;

            //FromFolders
            if (pMoveFile_fromFolders.Contains(";"))
            {
                FromFolders = pMoveFile_fromFolders.Split(';').ToList();
            }
            pMoveFile_fromFolders = null;

            //ToFolders
            if (pMoveFile_toFolders.Contains(";"))
            {
                ToFolders = pMoveFile_toFolders.Split(';').ToList();
            }
            pMoveFile_toFolders = null;

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

            List<int> folderSkipIndices = new List<int>();
            for (int i = 0; i < FromFolders.Count(); i++)
            {
                string fromFolder = FromFolders[i];
                if (!System.IO.Directory.Exists(fromFolder))
                {
                    //log it, but continue on
                    string details = "From folder '" + fromFolder + "' does not exist.";
                    bool b = Utilities.WriteToLogFile(Utilities.LoggingType.Error, Utilities.ApplicationFunction.MoveFile, details, logFile);
                    details = null;
                    if (!folderSkipIndices.Contains(i)) folderSkipIndices.Add(i);
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
                    if (!folderSkipIndices.Contains(i)) folderSkipIndices.Add(i);
                }
                toFolder = null;
            }
            //Stop --> Checks
        }
    }
}
