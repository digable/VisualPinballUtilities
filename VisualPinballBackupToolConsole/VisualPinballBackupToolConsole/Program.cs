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
            Arguments backup = ArgumentParser.Parse(args);

            switch(backup.Action)
            {
                case ActionTypes.BackupPinballX:
                    break;
                default:
                    break;
            }
        }
    }
}
