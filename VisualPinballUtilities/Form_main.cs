using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VisualPinballBackupToolConsole;

namespace VisualPinballUtilities
{
    public partial class Form_visualPinballUtilities : Form
    {
        public Form_visualPinballUtilities()
        {
            InitializeComponent();
            textBox_backupDirectory.Text = string.Empty;
        }

        private void button_packageTable_Click(object sender, EventArgs e)
        {
            //check visual pinball version, 9,10,pm5
            //need info for front end software, using pinball x
            //need to figure out config settings for tables, roms, media files, etc
            //need to figure out xml config for pinball x
            //need to figure out the versions of the scripts, locations, vpinmame and visual pinball 10
            //maybe use their registery

            //get table file
            //get rom file, if required
            //Try to read the vb script for the rom name
            //get backglass, either file or direct b2s
            //get pinballx config xml created
            //get pinballx media files
            //package up all the files into a compressed file structure.
        }

        private void button_browseBackupDIrectory_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog_backupDirectory.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox_backupDirectory.Text = folderBrowserDialog_backupDirectory.SelectedPath;
            }
        }

        private void button_backupVisualPinballData_Click(object sender, EventArgs e)
        {
            if (textBox_backupDirectory.Text.Trim() != string.Empty && Directory.Exists(textBox_backupDirectory.Text))
            {
                string[] consoleArgs = new string[] { "-backup", textBox_backupDirectory.Text };
                ConsoleArguments ca = ArgumentParser.Parse(consoleArgs);
                

            }
            else MessageBox.Show("The directory '" + textBox_backupDirectory.Text + "' is not valid.  Please try another.", "Invalid Backup Directory Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button_registryUpdate_Click(object sender, EventArgs e)
        {
            //process all the updates for the registry

            //INFO: 'dmd_compact' update
            string field = "dmd_compact";
            int newValue = 1;
            if (checkBox_compactDMD.Checked) newValue = 1;
            else newValue = 0;

            bool b = RegistryUtilities.VisualPinMame.Update.Roms.All.For(field, newValue);
        }

        private void button_eloRating_Click(object sender, EventArgs e)
        {
            Rating.EloRating.Matchup m = new Rating.EloRating.Matchup();
            m.User1Score = 1000;
            m.User2Score = 1000;

            Rating.EloRating.UpdateScores(m, true);
        }
    }
}
