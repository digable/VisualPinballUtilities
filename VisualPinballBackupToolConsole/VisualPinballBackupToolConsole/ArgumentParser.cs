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
        public static ConsoleArguments Parse(string[] args)
        {
            ConsoleArguments ca = new ConsoleArguments();

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
                    string backupValue = @"C:\temp";
                    int backupValueIndex = Array.IndexOf(args, "-backup") + 1;
                    ca.Action = ActionType.BackupPinballX;

                    try
                    {
                        backupValue = args[backupValueIndex];
                        if (!Directory.Exists(backupValue))
                        {
                            //INFO: directory doesnt exit. Try again.
                            Console.WriteLine("The directory '" + backupValue + "' does not exist.");
                            Environment.Exit(0);
                        }
                    }
                    catch (Exception)
                    {
                        //TODO: there is no directory astated, need to default it to something
                        backupValue = @"C:\temp";
                    }

                    ca.Directory = backupValue;
                }
            }

            return ca;
        }
    }

    public class ConsoleArguments
    {
        public ActionType Action { get; set; }
        public string Directory { get; set; }
    }

    public enum ActionType
    {
        BackupPinballX = 1
    }
}
