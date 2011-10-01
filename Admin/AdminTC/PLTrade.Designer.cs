namespace AdminTC
{
    partial class PLTrade
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
            this.grdPLTrade = new Janus.Windows.GridEX.GridEX();
            this.panPLData = new System.Windows.Forms.Panel();
            this.spinningProgress1 = new CircularProgress.SpinningProgress.SpinningProgress();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.cmbAccountID = new System.Windows.Forms.ComboBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.lblaccount = new System.Windows.Forms.Label();
            this.btnPLExport = new System.Windows.Forms.Button();
            this.lblTodate = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.grdPLTrade)).BeginInit();
            this.panPLData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // grdPLTrade
            // 
            this.grdPLTrade.ColumnAutoResize = true;
            this.grdPLTrade.Location = new System.Drawing.Point(3, 3);
            this.grdPLTrade.Name = "grdPLTrade";
            this.grdPLTrade.SaveSettings = false;
            this.grdPLTrade.Size = new System.Drawing.Size(820, 469);
            this.grdPLTrade.TabIndex = 0;
            this.grdPLTrade.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdPLTrade_FormattingRow);
            // 
            // panPLData
            // 
            this.panPLData.BackColor = System.Drawing.Color.White;
            this.panPLData.Controls.Add(this.spinningProgress1);
            this.panPLData.Controls.Add(this.grdPLTrade);
            this.panPLData.Location = new System.Drawing.Point(3, 52);
            this.panPLData.Name = "panPLData";
            this.panPLData.Size = new System.Drawing.Size(826, 475);
            this.panPLData.TabIndex = 1;
            // 
            // spinningProgress1
            // 
            this.spinningProgress1.AutoIncrementFrequency = 100;
            this.spinningProgress1.BehindTransistionSegmentIsActive = false;
            this.spinningProgress1.Location = new System.Drawing.Point(402, 224);
            this.spinningProgress1.Name = "spinningProgress1";
            this.spinningProgress1.Size = new System.Drawing.Size(40, 40);
            this.spinningProgress1.TabIndex = 3;
            this.spinningProgress1.TransistionSegment = 9;
            // 
            // picRefresh
            // 
            this.picRefresh.Image = global::AdminTC.Properties.Resources._ref;
            this.picRefresh.Location = new System.Drawing.Point(742, 4);
            this.picRefresh.Name = "picRefresh";
            this.picRefresh.Size = new System.Drawing.Size(54, 45);
            this.picRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRefresh.TabIndex = 2;
            this.picRefresh.TabStop = false;
            this.picRefresh.Click += new System.EventHandler(this.picRefresh_Click);
            // 
            // cmbAccountID
            // 
            this.cmbAccountID.FormattingEnabled = true;
            this.cmbAccountID.Location = new System.Drawing.Point(312, 22);
            this.cmbAccountID.Name = "cmbAccountID";
            this.cmbAccountID.Size = new System.Drawing.Size(158, 21);
            this.cmbAccountID.TabIndex = 3;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(505, 23);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(85, 21);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Get";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // lblaccount
            // 
            this.lblaccount.AutoSize = true;
            this.lblaccount.Location = new System.Drawing.Point(309, 7);
            this.lblaccount.Name = "lblaccount";
            this.lblaccount.Size = new System.Drawing.Size(58, 13);
            this.lblaccount.TabIndex = 5;
            this.lblaccount.Text = "AccountID";
            // 
            // btnPLExport
            // 
            this.btnPLExport.Location = new System.Drawing.Point(605, 22);
            this.btnPLExport.Name = "btnPLExport";
            this.btnPLExport.Size = new System.Drawing.Size(75, 21);
            this.btnPLExport.TabIndex = 6;
            this.btnPLExport.Text = "Export";
            this.btnPLExport.UseVisualStyleBackColor = true;
            this.btnPLExport.Click += new System.EventHandler(this.btnPLExport_Click);
            // 
            // lblTodate
            // 
            this.lblTodate.AutoSize = true;
            this.lblTodate.Location = new System.Drawing.Point(160, 7);
            this.lblTodate.Name = "lblTodate";
            this.lblTodate.Size = new System.Drawing.Size(46, 13);
            this.lblTodate.TabIndex = 9;
            this.lblTodate.Text = "To Date";
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(20, 7);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(56, 13);
            this.lblFromDate.TabIndex = 10;
            this.lblFromDate.Text = "From Date";
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.CustomFormat = "MM/dd/yyyy";
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(163, 23);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.ShowCheckBox = true;
            this.dateTimePickerTo.Size = new System.Drawing.Size(111, 20);
            this.dateTimePickerTo.TabIndex = 4;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.CustomFormat = "MM/dd/yyyy";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(23, 23);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.ShowCheckBox = true;
            this.dateTimePickerFrom.Size = new System.Drawing.Size(107, 20);
            this.dateTimePickerFrom.TabIndex = 4;
            // 
            // PLTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.lblTodate);
            this.Controls.Add(this.btnPLExport);
            this.Controls.Add(this.lblaccount);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.cmbAccountID);
            this.Controls.Add(this.picRefresh);
            this.Controls.Add(this.panPLData);
            this.Name = "PLTrade";
            this.Size = new System.Drawing.Size(832, 530);
            this.Load += new System.EventHandler(this.PLTrade_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdPLTrade)).EndInit();
            this.panPLData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.GridEX.GridEX grdPLTrade;
        private System.Windows.Forms.Panel panPLData;
        private CircularProgress.SpinningProgress.SpinningProgress spinningProgress1;
        private System.Windows.Forms.PictureBox picRefresh;
        private System.Windows.Forms.ComboBox cmbAccountID;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label lblaccount;
        private System.Windows.Forms.Button btnPLExport;
        private System.Windows.Forms.Label lblTodate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
    }
}
