using System.Threading;

using System.Linq;

//TODO: have to so the playfield is the one in focus if pinballx is running and the direct b2s options are NOT open
//TODO: need to use the config file for other things, remove app config once that is completed.
//TODO: need special rules for apps, like pinballx adding '_1' and '_2' at the end of a file if it exists.

namespace AutoFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            //INFO: setup globals for service
            Models.Global g = new Models.Global();

            //INFO: get rotate screen parameters
            Models.RotateScreen rs = new Models.RotateScreen(g);

            //INFO: check to see if there is another one running, kill them all
            bool serviceIsRunning = Functions.RunOnce.CheckInstances(rs.Monitor);
            //INFO: remove from memory if its not enabled
            if (!rs.Enabled) rs = null;

            Models.AppKill ak = new Models.AppKill(g);
            //INFO: remove from memory if its not enabled
            if (!ak.Enabled) ak = null;

            Models.USBKill uk = new Models.USBKill(g);
            //INFO: remove from memory if its not enabled
            if (!uk.Enabled) uk = null;

            Models.MoveFile mf = new Models.MoveFile(g);
            //INFO: remove from memory if its not enabled
            if (!mf.Enabled) mf = null;

            //INFO: this is the service
            while (g.IsServiceRunning)
            {
                string[] rp = Utilities.RunningProcesses();

                if (rs != null && rs.Enabled) Functions.RotateScreen.IsEnabled(rs, rp);

                if (ak != null && ak.Enabled) Functions.AppKill.IsEnabled(ak);

                if (uk != null && uk.Enabled) Functions.USBKill.IsEnabled(uk, g.OSVersion, rp);

                if (mf != null && mf.Enabled) Functions.MoveFile.IsEnabled(mf, rp, g.LoggingEnabled, g.LogFile);

                //INFO: log if there are no services running, close out the application
                if (rs == null && ak == null && uk == null && mf == null)
                {
                    g.IsServiceRunning = false;
                    string details = "None of the functions are enabled, please check your config.";
                    Utilities.WriteToLogFile(Utilities.LoggingType.Warning, Utilities.ApplicationFunction.Global, details, g);
                    details = null;
                }

                rp = null;
                Thread.Sleep(g.SleepTime);
            }

            //need to dispose of notification icon here
        }
    }
}
