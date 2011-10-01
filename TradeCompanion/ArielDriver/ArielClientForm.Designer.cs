namespace ArielDriver
{
    partial class ArielClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArielClientForm));
            this.axArielAPI1 = new AxAPILib.AxArielAPI();
            ((System.ComponentModel.ISupportInitialize)(this.axArielAPI1)).BeginInit();
            this.SuspendLayout();
            // 
            // axArielAPI1
            // 
            this.axArielAPI1.Enabled = true;
            this.axArielAPI1.Location = new System.Drawing.Point(51, 0);
            this.axArielAPI1.Name = "axArielAPI1";
            this.axArielAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axArielAPI1.OcxState")));
            this.axArielAPI1.Size = new System.Drawing.Size(100, 50);
            this.axArielAPI1.TabIndex = 0;
            this.axArielAPI1.LostConnection += new AxAPILib._DArielAPIEvents_LostConnectionEventHandler(this.axArielAPI1_LostConnection);
            this.axArielAPI1.AccountList += new AxAPILib._DArielAPIEvents_AccountListEventHandler(this.axArielAPI1_AccountList);
            this.axArielAPI1.EndOfList += new AxAPILib._DArielAPIEvents_EndOfListEventHandler(this.axArielAPI1_EndOfList);
            this.axArielAPI1.MarketList += new AxAPILib._DArielAPIEvents_MarketListEventHandler(this.axArielAPI1_MarketList);
            this.axArielAPI1.LoginEvent += new AxAPILib._DArielAPIEvents_LoginEventHandler(this.axArielAPI1_LoginEvent);
            this.axArielAPI1.PriceChange += new AxAPILib._DArielAPIEvents_PriceChangeEventHandler(this.axArielAPI1_PriceChange);
            this.axArielAPI1.DealAccepted += new AxAPILib._DArielAPIEvents_DealAcceptedEventHandler(this.axArielAPI1_DealAccepted);
            // 
            // ArielClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 59);
            this.Controls.Add(this.axArielAPI1);
            this.Name = "ArielClientForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axArielAPI1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAPILib.AxArielAPI axArielAPI1;











    }
}