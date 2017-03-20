using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualPinballBackupToolConsole
{
    public class ArgumentParser
    {
        public static Arguments Parse(string[] args)
        {
            Arguments a = new Arguments();

            List<string> lArgs = args.ToList<string>();

            if (args.Length == 0 || args.Contains("-h") || args.Contains("?"))
            {
                //INFO: write out the help file
                Console.WriteLine("\t-backup [location to save the backup file]");
                //Console.WriteLine("\t-restore [file path to backup file]");
            }
            else
            {
                //INFO: parse the actual arguments
                if (args.Contains("-backup"))
                {
                    int backupValueIndex = lArgs.IndexOf("-backup") + 1;
                    string backupValue = lArgs[backupValueIndex];
                    if (!Directory.Exists(backupValue))
                    {
                        //INFO: directory doesnt exit. Try again.
                        Console.WriteLine("The directory '" + backupValue + "' does not exist.");
                    }
                    else
                    {
                        a.Action = ActionTypes.BackupPinballX;
                        a.Directory = backupValue;
                    }
                }
            }

            return a;
        }
    }

    public class Arguments
    {
        public ActionTypes Action { get; set; }
        public string Directory { get; set; }
    }

    public enum ActionTypes
    {
        BackupPinballX = 1
    }
}
