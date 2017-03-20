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

            Arguments a = ArgumentParser.Parse(args);

            switch(a.Action)
            {
                case ActionTypes.BackupPinballX:
                    BackupPinballX pbx = new  BackupPinballX();
                    break;
                default:
                    break;
            }
        }
    }
}
