namespace AdminTC
{
    partial class Orders
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnGetOrders = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.panelData = new System.Windows.Forms.Panel();
            this.spinningProgress1 = new CircularProgress.SpinningProgress.SpinningProgress();
            this.grdOrders = new Janus.Windows.GridEX.GridEX();
            this.chkShowAll = new System.Windows.Forms.CheckBox();
            this.btnExportOrders = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            this.panelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "MM/dd/yyyy";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(69, 13);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(149, 20);
            this.dtpStartDate.TabIndex = 1;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "MM/dd/yyyy";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(299, 13);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(141, 20);
            this.dtpEndDate.TabIndex = 2;
            // 
            // btnGetOrders
            // 
            this.btnGetOrders.Location = new System.Drawing.Point(557, 12);
            this.btnGetOrders.Name = "btnGetOrders";
            this.btnGetOrders.Size = new System.Drawing.Size(80, 21);
            this.btnGetOrders.TabIndex = 3;
            this.btnGetOrders.Text = "Get";
            this.btnGetOrders.UseVisualStyleBackColor = true;
            this.btnGetOrders.Click += new System.EventHandler(this.btnGetOrders_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "From Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To Date";
            // 
            // picRefresh
            // 
            this.picRefresh.Image = global::AdminTC.Properties.Resources._ref;
            this.picRefresh.Location = new System.Drawing.Point(765, 3);
            this.picRefresh.Name = "picRefresh";
            this.picRefresh.Size = new System.Drawing.Size(57, 43);
            this.picRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRefresh.TabIndex = 8;
            this.picRefresh.TabStop = false;
            this.picRefresh.Click += new System.EventHandler(this.picRefresh_Click);
            // 
            // panelData
            // 
            this.panelData.BackColor = System.Drawing.Color.White;
            this.panelData.Controls.Add(this.spinningProgress1);
            this.panelData.Controls.Add(this.grdOrders);
            this.panelData.Location = new System.Drawing.Point(3, 46);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(819, 481);
            this.panelData.TabIndex = 10;
            // 
            // spinningProgress1
            // 
            this.spinningProgress1.AutoIncrementFrequency = 100;
            this.spinningProgress1.Location = new System.Drawing.Point(396, 190);
            this.spinningProgress1.Name = "spinningProgress1";
            this.spinningProgress1.Size = new System.Drawing.Size(40, 40);
            this.spinningProgress1.TabIndex = 2;
            this.spinningProgress1.TransistionSegment = 8;
            // 
            // grdOrders
            // 
            this.grdOrders.Location = new System.Drawing.Point(3, 3);
            this.grdOrders.Name = "grdOrders";
            this.grdOrders.SaveSettings = false;
            this.grdOrders.Size = new System.Drawing.Size(813, 475);
            this.grdOrders.TabIndex = 0;
            // 
            // chkShowAll
            // 
            this.chkShowAll.AutoSize = true;
            this.chkShowAll.Location = new System.Drawing.Point(467, 15);
            this.chkShowAll.Name = "chkShowAll";
            this.chkShowAll.Size = new System.Drawing.Size(67, 17);
            this.chkShowAll.TabIndex = 11;
            this.chkShowAll.Text = "Show All";
            this.chkShowAll.UseVisualStyleBackColor = true;
            // 
            // btnExportOrders
            // 
            this.btnExportOrders.Location = new System.Drawing.Point(654, 12);
            this.btnExportOrders.Name = "btnExportOrders";
            this.btnExportOrders.Size = new System.Drawing.Size(75, 21);
            this.btnExportOrders.TabIndex = 12;
            this.btnExportOrders.Text = "Export";
            this.btnExportOrders.UseVisualStyleBackColor = true;
            this.btnExportOrders.Click += new System.EventHandler(this.btnExportOrders_Click);
            // 
            // Orders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnExportOrders);
            this.Controls.Add(this.chkShowAll);
            this.Controls.Add(this.panelData);
            this.Controls.Add(this.picRefresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGetOrders);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Name = "Orders";
            this.Size = new System.Drawing.Size(847, 530);
            this.Load += new System.EventHandler(this.Orders_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            this.panelData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnGetOrders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picRefresh;
        private System.Windows.Forms.Panel panelData;
        private CircularProgress.SpinningProgress.SpinningProgress spinningProgress1;
        private System.Windows.Forms.CheckBox chkShowAll;
        private Janus.Windows.GridEX.GridEX grdOrders;
        private System.Windows.Forms.Button btnExportOrders;
    }
}
