namespace VisualPinballUtilities
{
    partial class Form_visualPinballUtilities
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_packageTable = new System.Windows.Forms.Button();
            this.button_installTable = new System.Windows.Forms.Button();
            this.button_updateTable = new System.Windows.Forms.Button();
            this.button_removeTable = new System.Windows.Forms.Button();
            this.button_backupVisualPinballData = new System.Windows.Forms.Button();
            this.label_backupDirectory = new System.Windows.Forms.Label();
            this.textBox_backupDirectory = new System.Windows.Forms.TextBox();
            this.button_browseBackupDirectory = new System.Windows.Forms.Button();
            this.folderBrowserDialog_backupDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.checkBox_compactDMD = new System.Windows.Forms.CheckBox();
            this.groupBox_registryUpdates = new System.Windows.Forms.GroupBox();
            this.checkBox_updateShowPinDMD = new System.Windows.Forms.CheckBox();
            this.checkBox_updateCompactDMD = new System.Windows.Forms.CheckBox();
            this.checkBox_showPinDMD = new System.Windows.Forms.CheckBox();
            this.button_registryUpdateAll = new System.Windows.Forms.Button();
            this.button_eloRating = new System.Windows.Forms.Button();
            this.button_runReports = new System.Windows.Forms.Button();
            this.checkedListBox_reports = new System.Windows.Forms.CheckedListBox();
            this.groupBox_reports = new System.Windows.Forms.GroupBox();
            this.button_findIPDBNumber = new System.Windows.Forms.Button();
            this.groupBox_registryUpdates.SuspendLayout();
            this.groupBox_reports.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_packageTable
            // 
            this.button_packageTable.Enabled = false;
            this.button_packageTable.Location = new System.Drawing.Point(12, 12);
            this.button_packageTable.Name = "button_packageTable";
            this.button_packageTable.Size = new System.Drawing.Size(94, 23);
            this.button_packageTable.TabIndex = 0;
            this.button_packageTable.Text = "Package Table";
            this.button_packageTable.UseVisualStyleBackColor = true;
            this.button_packageTable.Click += new System.EventHandler(this.button_packageTable_Click);
            // 
            // button_installTable
            // 
            this.button_installTable.Enabled = false;
            this.button_installTable.Location = new System.Drawing.Point(12, 41);
            this.button_installTable.Name = "button_installTable";
            this.button_installTable.Size = new System.Drawing.Size(94, 23);
            this.button_installTable.TabIndex = 1;
            this.button_installTable.Text = "Install Table";
            this.button_installTable.UseVisualStyleBackColor = true;
            // 
            // button_updateTable
            // 
            this.button_updateTable.Enabled = false;
            this.button_updateTable.Location = new System.Drawing.Point(12, 70);
            this.button_updateTable.Name = "button_updateTable";
            this.button_updateTable.Size = new System.Drawing.Size(94, 23);
            this.button_updateTable.TabIndex = 2;
            this.button_updateTable.Text = "Update Table";
            this.button_updateTable.UseVisualStyleBackColor = true;
            // 
            // button_removeTable
            // 
            this.button_removeTable.Enabled = false;
            this.button_removeTable.Location = new System.Drawing.Point(12, 99);
            this.button_removeTable.Name = "button_removeTable";
            this.button_removeTable.Size = new System.Drawing.Size(94, 23);
            this.button_removeTable.TabIndex = 3;
            this.button_removeTable.Text = "Remove Table";
            this.button_removeTable.UseVisualStyleBackColor = true;
            // 
            // button_backupVisualPinballData
            // 
            this.button_backupVisualPinballData.Location = new System.Drawing.Point(212, 161);
            this.button_backupVisualPinballData.Name = "button_backupVisualPinballData";
            this.button_backupVisualPinballData.Size = new System.Drawing.Size(149, 23);
            this.button_backupVisualPinballData.TabIndex = 4;
            this.button_backupVisualPinballData.Text = "Backup Visual Pinball Data";
            this.button_backupVisualPinballData.UseVisualStyleBackColor = true;
            this.button_backupVisualPinballData.Click += new System.EventHandler(this.button_backupVisualPinballData_Click);
            // 
            // label_backupDirectory
            // 
            this.label_backupDirectory.AutoSize = true;
            this.label_backupDirectory.Location = new System.Drawing.Point(12, 138);
            this.label_backupDirectory.Name = "label_backupDirectory";
            this.label_backupDirectory.Size = new System.Drawing.Size(92, 13);
            this.label_backupDirectory.TabIndex = 5;
            this.label_backupDirectory.Text = "Backup Directory:";
            // 
            // textBox_backupDirectory
            // 
            this.textBox_backupDirectory.Location = new System.Drawing.Point(111, 135);
            this.textBox_backupDirectory.Name = "textBox_backupDirectory";
            this.textBox_backupDirectory.Size = new System.Drawing.Size(335, 20);
            this.textBox_backupDirectory.TabIndex = 6;
            // 
            // button_browseBackupDirectory
            // 
            this.button_browseBackupDirectory.Location = new System.Drawing.Point(452, 133);
            this.button_browseBackupDirectory.Name = "button_browseBackupDirectory";
            this.button_browseBackupDirectory.Size = new System.Drawing.Size(75, 23);
            this.button_browseBackupDirectory.TabIndex = 7;
            this.button_browseBackupDirectory.Text = "Browse...";
            this.button_browseBackupDirectory.UseVisualStyleBackColor = true;
            this.button_browseBackupDirectory.Click += new System.EventHandler(this.Button_browseBackupDirectory_Click);
            // 
            // checkBox_compactDMD
            // 
            this.checkBox_compactDMD.AutoSize = true;
            this.checkBox_compactDMD.Checked = true;
            this.checkBox_compactDMD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_compactDMD.Location = new System.Drawing.Point(6, 19);
            this.checkBox_compactDMD.Name = "checkBox_compactDMD";
            this.checkBox_compactDMD.Size = new System.Drawing.Size(102, 17);
            this.checkBox_compactDMD.TabIndex = 8;
            this.checkBox_compactDMD.Text = "Compact DMD?";
            this.checkBox_compactDMD.UseVisualStyleBackColor = true;
            // 
            // groupBox_registryUpdates
            // 
            this.groupBox_registryUpdates.Controls.Add(this.checkBox_updateShowPinDMD);
            this.groupBox_registryUpdates.Controls.Add(this.checkBox_updateCompactDMD);
            this.groupBox_registryUpdates.Controls.Add(this.checkBox_showPinDMD);
            this.groupBox_registryUpdates.Controls.Add(this.button_registryUpdateAll);
            this.groupBox_registryUpdates.Controls.Add(this.checkBox_compactDMD);
            this.groupBox_registryUpdates.Location = new System.Drawing.Point(112, 12);
            this.groupBox_registryUpdates.Name = "groupBox_registryUpdates";
            this.groupBox_registryUpdates.Size = new System.Drawing.Size(415, 110);
            this.groupBox_registryUpdates.TabIndex = 9;
            this.groupBox_registryUpdates.TabStop = false;
            this.groupBox_registryUpdates.Text = "Registry Updates";
            // 
            // checkBox_updateShowPinDMD
            // 
            this.checkBox_updateShowPinDMD.AutoSize = true;
            this.checkBox_updateShowPinDMD.Location = new System.Drawing.Point(114, 42);
            this.checkBox_updateShowPinDMD.Name = "checkBox_updateShowPinDMD";
            this.checkBox_updateShowPinDMD.Size = new System.Drawing.Size(65, 17);
            this.checkBox_updateShowPinDMD.TabIndex = 12;
            this.checkBox_updateShowPinDMD.Text = "update?";
            this.checkBox_updateShowPinDMD.UseVisualStyleBackColor = true;
            // 
            // checkBox_updateCompactDMD
            // 
            this.checkBox_updateCompactDMD.AutoSize = true;
            this.checkBox_updateCompactDMD.Location = new System.Drawing.Point(114, 19);
            this.checkBox_updateCompactDMD.Name = "checkBox_updateCompactDMD";
            this.checkBox_updateCompactDMD.Size = new System.Drawing.Size(65, 17);
            this.checkBox_updateCompactDMD.TabIndex = 11;
            this.checkBox_updateCompactDMD.Text = "update?";
            this.checkBox_updateCompactDMD.UseVisualStyleBackColor = true;
            // 
            // checkBox_showPinDMD
            // 
            this.checkBox_showPinDMD.AutoSize = true;
            this.checkBox_showPinDMD.Checked = true;
            this.checkBox_showPinDMD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showPinDMD.Location = new System.Drawing.Point(6, 42);
            this.checkBox_showPinDMD.Name = "checkBox_showPinDMD";
            this.checkBox_showPinDMD.Size = new System.Drawing.Size(102, 17);
            this.checkBox_showPinDMD.TabIndex = 10;
            this.checkBox_showPinDMD.Text = "Show PinDMD?";
            this.checkBox_showPinDMD.UseVisualStyleBackColor = true;
            // 
            // button_registryUpdateAll
            // 
            this.button_registryUpdateAll.Location = new System.Drawing.Point(334, 81);
            this.button_registryUpdateAll.Name = "button_registryUpdateAll";
            this.button_registryUpdateAll.Size = new System.Drawing.Size(75, 23);
            this.button_registryUpdateAll.TabIndex = 9;
            this.button_registryUpdateAll.Text = "Update All!!!";
            this.button_registryUpdateAll.UseVisualStyleBackColor = true;
            this.button_registryUpdateAll.Click += new System.EventHandler(this.button_registryUpdateAll_Click);
            // 
            // button_eloRating
            // 
            this.button_eloRating.Enabled = false;
            this.button_eloRating.Location = new System.Drawing.Point(12, 161);
            this.button_eloRating.Name = "button_eloRating";
            this.button_eloRating.Size = new System.Drawing.Size(75, 23);
            this.button_eloRating.TabIndex = 10;
            this.button_eloRating.Text = "Elo Rating";
            this.button_eloRating.UseVisualStyleBackColor = true;
            this.button_eloRating.Click += new System.EventHandler(this.button_eloRating_Click);
            // 
            // button_runReports
            // 
            this.button_runReports.Location = new System.Drawing.Point(256, 119);
            this.button_runReports.Name = "button_runReports";
            this.button_runReports.Size = new System.Drawing.Size(84, 23);
            this.button_runReports.TabIndex = 0;
            this.button_runReports.Text = "Run Reports!";
            this.button_runReports.UseVisualStyleBackColor = true;
            this.button_runReports.Click += new System.EventHandler(this.button_runReports_Click);
            // 
            // checkedListBox_reports
            // 
            this.checkedListBox_reports.FormattingEnabled = true;
            this.checkedListBox_reports.Location = new System.Drawing.Point(6, 19);
            this.checkedListBox_reports.Name = "checkedListBox_reports";
            this.checkedListBox_reports.Size = new System.Drawing.Size(334, 94);
            this.checkedListBox_reports.TabIndex = 1;
            // 
            // groupBox_reports
            // 
            this.groupBox_reports.Controls.Add(this.button_runReports);
            this.groupBox_reports.Controls.Add(this.checkedListBox_reports);
            this.groupBox_reports.Location = new System.Drawing.Point(12, 190);
            this.groupBox_reports.Name = "groupBox_reports";
            this.groupBox_reports.Size = new System.Drawing.Size(346, 148);
            this.groupBox_reports.TabIndex = 12;
            this.groupBox_reports.TabStop = false;
            this.groupBox_reports.Text = "Reports";
            // 
            // button_findIPDBNumber
            // 
            this.button_findIPDBNumber.Location = new System.Drawing.Point(12, 344);
            this.button_findIPDBNumber.Name = "button_findIPDBNumber";
            this.button_findIPDBNumber.Size = new System.Drawing.Size(150, 23);
            this.button_findIPDBNumber.TabIndex = 13;
            this.button_findIPDBNumber.Text = "PinballY-Find IPDB Number";
            this.button_findIPDBNumber.UseVisualStyleBackColor = true;
            this.button_findIPDBNumber.Click += new System.EventHandler(this.button_findIPDBNumber_Click);
            // 
            // Form_visualPinballUtilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 415);
            this.Controls.Add(this.button_findIPDBNumber);
            this.Controls.Add(this.groupBox_reports);
            this.Controls.Add(this.button_eloRating);
            this.Controls.Add(this.groupBox_registryUpdates);
            this.Controls.Add(this.button_browseBackupDirectory);
            this.Controls.Add(this.textBox_backupDirectory);
            this.Controls.Add(this.label_backupDirectory);
            this.Controls.Add(this.button_backupVisualPinballData);
            this.Controls.Add(this.button_removeTable);
            this.Controls.Add(this.button_updateTable);
            this.Controls.Add(this.button_installTable);
            this.Controls.Add(this.button_packageTable);
            this.Name = "Form_visualPinballUtilities";
            this.Text = "Visual Pinball Utilities";
            this.groupBox_registryUpdates.ResumeLayout(false);
            this.groupBox_registryUpdates.PerformLayout();
            this.groupBox_reports.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Button_browseBackupDirectory_Click1(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button button_packageTable;
        private System.Windows.Forms.Button button_installTable;
        private System.Windows.Forms.Button button_updateTable;
        private System.Windows.Forms.Button button_removeTable;
        private System.Windows.Forms.Button button_backupVisualPinballData;
        private System.Windows.Forms.Label label_backupDirectory;
        private System.Windows.Forms.TextBox textBox_backupDirectory;
        private System.Windows.Forms.Button button_browseBackupDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_backupDirectory;
        private System.Windows.Forms.CheckBox checkBox_compactDMD;
        private System.Windows.Forms.GroupBox groupBox_registryUpdates;
        private System.Windows.Forms.Button button_registryUpdateAll;
        private System.Windows.Forms.Button button_eloRating;
        private System.Windows.Forms.Button button_runReports;
        private System.Windows.Forms.CheckedListBox checkedListBox_reports;
        private System.Windows.Forms.GroupBox groupBox_reports;
        private System.Windows.Forms.CheckBox checkBox_showPinDMD;
        private System.Windows.Forms.CheckBox checkBox_updateShowPinDMD;
        private System.Windows.Forms.CheckBox checkBox_updateCompactDMD;
        private System.Windows.Forms.Button button_findIPDBNumber;
    }
}

