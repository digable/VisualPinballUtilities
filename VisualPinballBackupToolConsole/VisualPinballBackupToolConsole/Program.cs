using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualPinballBackupToolConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //DEBUG- START
            args = new string[] { "-backup" };
            //DEBUG -STOP

            ConsoleArguments ca = ArgumentParser.Parse(args);

            switch(ca.Action)
            {
                case ActionType.BackupPinballX:
                    Backup pbx = new  Backup();
                    break;
                default:
                    break;
            }
        }
    }
}
