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
            this.textBox_backupDIrectory = new System.Windows.Forms.TextBox();
            this.button_browseBackupDIrectory = new System.Windows.Forms.Button();
            this.folderBrowserDialog_backupDirectory = new System.Windows.Forms.FolderBrowserDialog();
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
            this.label_backupDirectory.Size = new System.Drawing.Size(93, 13);
            this.label_backupDirectory.TabIndex = 5;
            this.label_backupDirectory.Text = "Backup DIrectory:";
            // 
            // textBox_backupDIrectory
            // 
            this.textBox_backupDIrectory.Location = new System.Drawing.Point(111, 135);
            this.textBox_backupDIrectory.Name = "textBox_backupDIrectory";
            this.textBox_backupDIrectory.Size = new System.Drawing.Size(335, 20);
            this.textBox_backupDIrectory.TabIndex = 6;
            // 
            // button_browseBackupDIrectory
            // 
            this.button_browseBackupDIrectory.Location = new System.Drawing.Point(452, 133);
            this.button_browseBackupDIrectory.Name = "button_browseBackupDIrectory";
            this.button_browseBackupDIrectory.Size = new System.Drawing.Size(75, 23);
            this.button_browseBackupDIrectory.TabIndex = 7;
            this.button_browseBackupDIrectory.Text = "Browse...";
            this.button_browseBackupDIrectory.UseVisualStyleBackColor = true;
            this.button_browseBackupDIrectory.Click += new System.EventHandler(this.button_browseBackupDIrectory_Click);
            // 
            // Form_visualPinballUtilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 228);
            this.Controls.Add(this.button_browseBackupDIrectory);
            this.Controls.Add(this.textBox_backupDIrectory);
            this.Controls.Add(this.label_backupDirectory);
            this.Controls.Add(this.button_backupVisualPinballData);
            this.Controls.Add(this.button_removeTable);
            this.Controls.Add(this.button_updateTable);
            this.Controls.Add(this.button_installTable);
            this.Controls.Add(this.button_packageTable);
            this.Name = "Form_visualPinballUtilities";
            this.Text = "Visual Pinball Utilities";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_packageTable;
        private System.Windows.Forms.Button button_installTable;
        private System.Windows.Forms.Button button_updateTable;
        private System.Windows.Forms.Button button_removeTable;
        private System.Windows.Forms.Button button_backupVisualPinballData;
        private System.Windows.Forms.Label label_backupDirectory;
        private System.Windows.Forms.TextBox textBox_backupDIrectory;
        private System.Windows.Forms.Button button_browseBackupDIrectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_backupDirectory;
    }
}

