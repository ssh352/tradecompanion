namespace AdminTC
{
    partial class frmAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdmin));
            this.panelFormDisplay = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.spinningProgress1 = new CircularProgress.SpinningProgress.SpinningProgress();
            this.dgDetails = new System.Windows.Forms.DataGridView();
            this.ColumnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLoginID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEmailID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPhoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnActive = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnLoggedIn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRegistrationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTrial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLastupdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPLTrade = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.panelFormDisplay.SuspendLayout();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetails)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFormDisplay
            // 
            this.panelFormDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormDisplay.Controls.Add(this.webBrowser1);
            this.panelFormDisplay.Location = new System.Drawing.Point(194, 3);
            this.panelFormDisplay.Name = "panelFormDisplay";
            this.panelFormDisplay.Size = new System.Drawing.Size(830, 537);
            this.panelFormDisplay.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(828, 535);
            this.webBrowser1.TabIndex = 0;
            // 
            // panelGrid
            // 
            this.panelGrid.BackColor = System.Drawing.Color.White;
            this.panelGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGrid.Controls.Add(this.spinningProgress1);
            this.panelGrid.Controls.Add(this.dgDetails);
            this.panelGrid.Location = new System.Drawing.Point(1, 546);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(1025, 176);
            this.panelGrid.TabIndex = 1;
            // 
            // spinningProgress1
            // 
            this.spinningProgress1.AutoIncrementFrequency = 100;
            this.spinningProgress1.Location = new System.Drawing.Point(491, 67);
            this.spinningProgress1.Name = "spinningProgress1";
            this.spinningProgress1.Size = new System.Drawing.Size(40, 40);
            this.spinningProgress1.TabIndex = 1;
            this.spinningProgress1.TransistionSegment = 10;
            // 
            // dgDetails
            // 
            this.dgDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnID,
            this.ColumnLoginID,
            this.ColumnUserName,
            this.ColumnEmailID,
            this.ColumnPhoneNo,
            this.ColumnActive,
            this.ColumnLoggedIn,
            this.ColumnAddress,
            this.ColumnRegistrationDate,
            this.ColumnTrial,
            this.ColumnLastupdated,
            this.Version});
            this.dgDetails.Location = new System.Drawing.Point(-2, -1);
            this.dgDetails.Name = "dgDetails";
            this.dgDetails.Size = new System.Drawing.Size(1022, 168);
            this.dgDetails.TabIndex = 0;
            this.dgDetails.Click += new System.EventHandler(this.dgDetails_Click);
            // 
            // ColumnID
            // 
            this.ColumnID.DataPropertyName = "ID";
            this.ColumnID.HeaderText = "";
            this.ColumnID.Name = "ColumnID";
            this.ColumnID.Visible = false;
            // 
            // ColumnLoginID
            // 
            this.ColumnLoginID.DataPropertyName = "LoginID";
            this.ColumnLoginID.HeaderText = "Login ID";
            this.ColumnLoginID.Name = "ColumnLoginID";
            // 
            // ColumnUserName
            // 
            this.ColumnUserName.DataPropertyName = "Username";
            this.ColumnUserName.HeaderText = "Name";
            this.ColumnUserName.Name = "ColumnUserName";
            // 
            // ColumnEmailID
            // 
            this.ColumnEmailID.DataPropertyName = "Emailid";
            this.ColumnEmailID.HeaderText = "Email";
            this.ColumnEmailID.Name = "ColumnEmailID";
            // 
            // ColumnPhoneNo
            // 
            this.ColumnPhoneNo.DataPropertyName = "Phoneno";
            this.ColumnPhoneNo.HeaderText = "Phone";
            this.ColumnPhoneNo.Name = "ColumnPhoneNo";
            // 
            // ColumnActive
            // 
            this.ColumnActive.DataPropertyName = "Actives";
            this.ColumnActive.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.ColumnActive.HeaderText = "Active";
            this.ColumnActive.Name = "ColumnActive";
            this.ColumnActive.ReadOnly = true;
            this.ColumnActive.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColumnLoggedIn
            // 
            this.ColumnLoggedIn.DataPropertyName = "LoggedIn";
            this.ColumnLoggedIn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.ColumnLoggedIn.HeaderText = "Logged In";
            this.ColumnLoggedIn.Name = "ColumnLoggedIn";
            this.ColumnLoggedIn.ReadOnly = true;
            // 
            // ColumnAddress
            // 
            this.ColumnAddress.DataPropertyName = "ADDRESS";
            this.ColumnAddress.HeaderText = "Address";
            this.ColumnAddress.Name = "ColumnAddress";
            // 
            // ColumnRegistrationDate
            // 
            this.ColumnRegistrationDate.DataPropertyName = "REGISTRATIONDATE";
            this.ColumnRegistrationDate.HeaderText = "Registration Date";
            this.ColumnRegistrationDate.Name = "ColumnRegistrationDate";
            // 
            // ColumnTrial
            // 
            this.ColumnTrial.DataPropertyName = "TRIAL";
            this.ColumnTrial.HeaderText = "Trial";
            this.ColumnTrial.Name = "ColumnTrial";
            // 
            // ColumnLastupdated
            // 
            this.ColumnLastupdated.DataPropertyName = "LASTUPDATED";
            this.ColumnLastupdated.HeaderText = "Lastupdated";
            this.ColumnLastupdated.Name = "ColumnLastupdated";
            // 
            // Version
            // 
            this.Version.DataPropertyName = "Version";
            this.Version.HeaderText = "Version";
            this.Version.Name = "Version";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnPLTrade);
            this.panel3.Controls.Add(this.btnOrders);
            this.panel3.Controls.Add(this.btnUsers);
            this.panel3.Location = new System.Drawing.Point(2, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(186, 537);
            this.panel3.TabIndex = 2;
            // 
            // btnPLTrade
            // 
            this.btnPLTrade.Location = new System.Drawing.Point(5, 78);
            this.btnPLTrade.Name = "btnPLTrade";
            this.btnPLTrade.Size = new System.Drawing.Size(166, 29);
            this.btnPLTrade.TabIndex = 2;
            this.btnPLTrade.Text = "PLTrade";
            this.btnPLTrade.UseVisualStyleBackColor = true;
            // 
            // btnOrders
            // 
            this.btnOrders.Location = new System.Drawing.Point(5, 43);
            this.btnOrders.Name = "btnOrders";
            this.btnOrders.Size = new System.Drawing.Size(166, 29);
            this.btnOrders.TabIndex = 1;
            this.btnOrders.Text = "Orders";
            this.btnOrders.UseVisualStyleBackColor = true;
            // 
            // btnUsers
            // 
            this.btnUsers.Location = new System.Drawing.Point(5, 8);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(166, 29);
            this.btnUsers.TabIndex = 0;
            this.btnUsers.Text = "Users";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // frmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 723);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.panelFormDisplay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAdmin";
            this.Text = "TC Admin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAdmin_Load);
            this.panelFormDisplay.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetails)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFormDisplay;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgDetails;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnOrders;
        private CircularProgress.SpinningProgress.SpinningProgress spinningProgress1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLoginID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEmailID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPhoneNo;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnActive;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnLoggedIn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRegistrationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTrial;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLastupdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.Button btnPLTrade;
    }
}

