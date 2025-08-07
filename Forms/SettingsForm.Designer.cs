namespace ClipboardMasking.Win.Forms
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.GroupBox grpMaskingOptions;
        private System.Windows.Forms.CheckBox chkMaskNames;
        private System.Windows.Forms.CheckBox chkMaskURLs;
        private System.Windows.Forms.CheckBox chkMaskSSNs;
        private System.Windows.Forms.CheckBox chkMaskCreditCards;
        private System.Windows.Forms.CheckBox chkMaskPhones;
        private System.Windows.Forms.CheckBox chkMaskEmails;
        private System.Windows.Forms.CheckBox chkMaskIPs;

        private System.Windows.Forms.GroupBox grpCustomNames;
        private System.Windows.Forms.Button btnRemoveName;
        private System.Windows.Forms.Button btnAddName;
        private System.Windows.Forms.TextBox txtNewCustomName;
        private System.Windows.Forms.ListBox lstCustomNames;

        private System.Windows.Forms.GroupBox grpCustomPatterns;
        private System.Windows.Forms.DataGridView dgvCustomPatterns;

        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.CheckBox chkStartOnLaunch;

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpMaskingOptions = new System.Windows.Forms.GroupBox();
            this.chkMaskNames = new System.Windows.Forms.CheckBox();
            this.chkMaskURLs = new System.Windows.Forms.CheckBox();
            this.chkMaskSSNs = new System.Windows.Forms.CheckBox();
            this.chkMaskCreditCards = new System.Windows.Forms.CheckBox();
            this.chkMaskPhones = new System.Windows.Forms.CheckBox();
            this.chkMaskEmails = new System.Windows.Forms.CheckBox();
            this.chkMaskIPs = new System.Windows.Forms.CheckBox();
            this.grpCustomNames = new System.Windows.Forms.GroupBox();
            this.btnRemoveName = new System.Windows.Forms.Button();
            this.btnAddName = new System.Windows.Forms.Button();
            this.txtNewCustomName = new System.Windows.Forms.TextBox();
            this.lstCustomNames = new System.Windows.Forms.ListBox();
            this.grpCustomPatterns = new System.Windows.Forms.GroupBox();
            this.dgvCustomPatterns = new System.Windows.Forms.DataGridView();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.chkStartOnLaunch = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpMaskingOptions.SuspendLayout();
            this.grpCustomNames.SuspendLayout();
            this.grpCustomPatterns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomPatterns)).BeginInit();
            this.grpGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMaskingOptions
            // 
            this.grpMaskingOptions.Controls.Add(this.chkMaskNames);
            this.grpMaskingOptions.Controls.Add(this.chkMaskURLs);
            this.grpMaskingOptions.Controls.Add(this.chkMaskSSNs);
            this.grpMaskingOptions.Controls.Add(this.chkMaskCreditCards);
            this.grpMaskingOptions.Controls.Add(this.chkMaskPhones);
            this.grpMaskingOptions.Controls.Add(this.chkMaskEmails);
            this.grpMaskingOptions.Controls.Add(this.chkMaskIPs);
            this.grpMaskingOptions.Location = new System.Drawing.Point(12, 85);
            this.grpMaskingOptions.Name = "grpMaskingOptions";
            this.grpMaskingOptions.Size = new System.Drawing.Size(200, 200);
            this.grpMaskingOptions.TabIndex = 1;
            this.grpMaskingOptions.TabStop = false;
            this.grpMaskingOptions.Text = "Masking Options";
            // 
            // chkMaskNames
            // 
            this.chkMaskNames.AutoSize = true;
            this.chkMaskNames.Location = new System.Drawing.Point(15, 170);
            this.chkMaskNames.Name = "chkMaskNames";
            this.chkMaskNames.Size = new System.Drawing.Size(109, 19);
            this.chkMaskNames.TabIndex = 6;
            this.chkMaskNames.Text = "Custom Names";
            this.chkMaskNames.UseVisualStyleBackColor = true;
            // 
            // chkMaskURLs
            // 
            this.chkMaskURLs.AutoSize = true;
            this.chkMaskURLs.Location = new System.Drawing.Point(15, 145);
            this.chkMaskURLs.Name = "chkMaskURLs";
            this.chkMaskURLs.Size = new System.Drawing.Size(51, 19);
            this.chkMaskURLs.TabIndex = 5;
            this.chkMaskURLs.Text = "URLs";
            this.chkMaskURLs.UseVisualStyleBackColor = true;
            // 
            // chkMaskSSNs
            // 
            this.chkMaskSSNs.AutoSize = true;
            this.chkMaskSSNs.Location = new System.Drawing.Point(15, 120);
            this.chkMaskSSNs.Name = "chkMaskSSNs";
            this.chkMaskSSNs.Size = new System.Drawing.Size(155, 19);
            this.chkMaskSSNs.TabIndex = 4;
            this.chkMaskSSNs.Text = "Social Security Numbers";
            this.chkMaskSSNs.UseVisualStyleBackColor = true;
            // 
            // chkMaskCreditCards
            // 
            this.chkMaskCreditCards.AutoSize = true;
            this.chkMaskCreditCards.Location = new System.Drawing.Point(15, 95);
            this.chkMaskCreditCards.Name = "chkMaskCreditCards";
            this.chkMaskCreditCards.Size = new System.Drawing.Size(140, 19);
            this.chkMaskCreditCards.TabIndex = 3;
            this.chkMaskCreditCards.Text = "Credit Card Numbers";
            this.chkMaskCreditCards.UseVisualStyleBackColor = true;
            // 
            // chkMaskPhones
            // 
            this.chkMaskPhones.AutoSize = true;
            this.chkMaskPhones.Location = new System.Drawing.Point(15, 70);
            this.chkMaskPhones.Name = "chkMaskPhones";
            this.chkMaskPhones.Size = new System.Drawing.Size(111, 19);
            this.chkMaskPhones.TabIndex = 2;
            this.chkMaskPhones.Text = "Phone Numbers";
            this.chkMaskPhones.UseVisualStyleBackColor = true;
            // 
            // chkMaskEmails
            // 
            this.chkMaskEmails.AutoSize = true;
            this.chkMaskEmails.Location = new System.Drawing.Point(15, 45);
            this.chkMaskEmails.Name = "chkMaskEmails";
            this.chkMaskEmails.Size = new System.Drawing.Size(111, 19);
            this.chkMaskEmails.TabIndex = 1;
            this.chkMaskEmails.Text = "Email Addresses";
            this.chkMaskEmails.UseVisualStyleBackColor = true;
            // 
            // chkMaskIPs
            // 
            this.chkMaskIPs.AutoSize = true;
            this.chkMaskIPs.Location = new System.Drawing.Point(15, 20);
            this.chkMaskIPs.Name = "chkMaskIPs";
            this.chkMaskIPs.Size = new System.Drawing.Size(92, 19);
            this.chkMaskIPs.TabIndex = 0;
            this.chkMaskIPs.Text = "IP Addresses";
            this.chkMaskIPs.UseVisualStyleBackColor = true;
            // 
            // grpCustomNames
            // 
            this.grpCustomNames.Controls.Add(this.btnRemoveName);
            this.grpCustomNames.Controls.Add(this.btnAddName);
            this.grpCustomNames.Controls.Add(this.txtNewCustomName);
            this.grpCustomNames.Controls.Add(this.lstCustomNames);
            this.grpCustomNames.Location = new System.Drawing.Point(230, 12);
            this.grpCustomNames.Name = "grpCustomNames";
            this.grpCustomNames.Size = new System.Drawing.Size(442, 145);
            this.grpCustomNames.TabIndex = 2;
            this.grpCustomNames.TabStop = false;
            this.grpCustomNames.Text = "Custom Names";
            // 
            // btnRemoveName
            // 
            this.btnRemoveName.Location = new System.Drawing.Point(345, 50);
            this.btnRemoveName.Name = "btnRemoveName";
            this.btnRemoveName.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveName.TabIndex = 3;
            this.btnRemoveName.Text = "Remove";
            this.btnRemoveName.UseVisualStyleBackColor = true;
            this.btnRemoveName.Click += new System.EventHandler(this.btnRemoveName_Click);
            // 
            // btnAddName
            // 
            this.btnAddName.Location = new System.Drawing.Point(264, 50);
            this.btnAddName.Name = "btnAddName";
            this.btnAddName.Size = new System.Drawing.Size(75, 23);
            this.btnAddName.TabIndex = 2;
            this.btnAddName.Text = "Add";
            this.btnAddName.UseVisualStyleBackColor = true;
            this.btnAddName.Click += new System.EventHandler(this.btnAddName_Click);
            // 
            // txtNewCustomName
            // 
            this.txtNewCustomName.Location = new System.Drawing.Point(264, 21);
            this.txtNewCustomName.Name = "txtNewCustomName";
            this.txtNewCustomName.Size = new System.Drawing.Size(156, 23);
            this.txtNewCustomName.TabIndex = 1;
            // 
            // lstCustomNames
            // 
            this.lstCustomNames.FormattingEnabled = true;
            this.lstCustomNames.ItemHeight = 15;
            this.lstCustomNames.Location = new System.Drawing.Point(6, 22);
            this.lstCustomNames.Name = "lstCustomNames";
            this.lstCustomNames.Size = new System.Drawing.Size(252, 109);
            this.lstCustomNames.TabIndex = 0;
            // 
            // grpCustomPatterns
            // 
            this.grpCustomPatterns.Controls.Add(this.dgvCustomPatterns);
            this.grpCustomPatterns.Location = new System.Drawing.Point(230, 163);
            this.grpCustomPatterns.Name = "grpCustomPatterns";
            this.grpCustomPatterns.Size = new System.Drawing.Size(442, 172);
            this.grpCustomPatterns.TabIndex = 3;
            this.grpCustomPatterns.TabStop = false;
            this.grpCustomPatterns.Text = "Custom Patterns (Regex)";
            // 
            // dgvCustomPatterns
            // 
            this.dgvCustomPatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustomPatterns.Location = new System.Drawing.Point(3, 19);
            this.dgvCustomPatterns.Name = "dgvCustomPatterns";
            this.dgvCustomPatterns.Size = new System.Drawing.Size(436, 150);
            this.dgvCustomPatterns.TabIndex = 0;
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.chkStartOnLaunch);
            this.grpGeneral.Location = new System.Drawing.Point(12, 12);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(200, 56);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General";
            // 
            // chkStartOnLaunch
            // 
            this.chkStartOnLaunch.AutoSize = true;
            this.chkStartOnLaunch.Location = new System.Drawing.Point(15, 22);
            this.chkStartOnLaunch.Name = "chkStartOnLaunch";
            this.chkStartOnLaunch.Size = new System.Drawing.Size(167, 19);
            this.chkStartOnLaunch.TabIndex = 0;
            this.chkStartOnLaunch.Text = "Start monitoring on launch";
            this.chkStartOnLaunch.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(516, 341);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(597, 341);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(684, 376);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.grpCustomPatterns);
            this.Controls.Add(this.grpCustomNames);
            this.Controls.Add(this.grpMaskingOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.grpMaskingOptions.ResumeLayout(false);
            this.grpMaskingOptions.PerformLayout();
            this.grpCustomNames.ResumeLayout(false);
            this.grpCustomNames.PerformLayout();
            this.grpCustomPatterns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomPatterns)).EndInit();
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}