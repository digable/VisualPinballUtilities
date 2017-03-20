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
            textBox_backupDIrectory.Text = string.Empty;
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
                textBox_backupDIrectory.Text = folderBrowserDialog_backupDirectory.SelectedPath;
            }
        }

        private void button_backupVisualPinballData_Click(object sender, EventArgs e)
        {
            if (textBox_backupDIrectory.Text.Trim() != string.Empty && Directory.Exists(textBox_backupDIrectory.Text))
            {
                string[] consoleArgs = new string[] { "-backup", textBox_backupDIrectory.Text };
                Arguments a = ArgumentParser.Parse(consoleArgs);


            }
            else MessageBox.Show("The directory '" + textBox_backupDIrectory.Text + "' is not valid.  Please try another.", "Invalid Backup Directory Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


    }
}
