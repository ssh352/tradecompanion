using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AdminTC
{
    public partial class Orders : UserControl,IUserControl
    {
        private Janus.Windows.GridEX.GridEXLayout grdOrdersLayOut = null;
        DataSet dsOrders;
        private static Orders singleton_Orders = null;
        string loginID = "";
        //bool isRefresh;

        public Orders()
        {
            InitializeComponent();

            Janus.Windows.GridEX.GridEXFormatStyle style = new Janus.Windows.GridEX.GridEXFormatStyle();
            style.ForeColor = Color.LightGray;
            style.Key = "Accepted";
            grdOrders.FormatStyles.Add(style);
            //RefreshDataset();

            dtpStartDate.Value = DateTime.Now;
            dtpStartDate.Checked = true;
            dtpEndDate.Value = DateTime.Now;
            dtpEndDate.Checked = false;

            
            if (singleton_Orders == null)
                singleton_Orders = this;
           // isRefresh = false;
 
        }
       
        public static Orders GetSingletonOrderform()
        {
            return singleton_Orders;
        }

        public void ShowDetails(int id)
        {
            DataSet dsUsers = frmAdmin.GetSingletonAdminform().dsUsers;
            DataRow dr = dsUsers.Tables[0].Rows.Find(id);
            string loginID = (string)dr["LOGINID"];
            ShowOrders(loginID);
            
        }
     
        public void RefreshDataset()
        {
            //WSScalper.WebServicesScalper ws = new WSScalper.WebServicesScalper();
            //dsOrders = ws.GetOrdersDS();
            //dsOrders = ws.GetOrdersDSAsync();
           
            //panelData.Controls.Clear();
            //panelData.Controls.Add(grdOrders);
            //panelData.Controls.Clear();
            //panelData.Controls.Add(spinningProgress1);
           AsyncCallWebServices();
        }

        public void ShowOrders(string loginID)
        {
            try
            {
                this.loginID = loginID;
               // string querry = "";
                AsyncCallWebServices();
                //if (chkShowAll.Checked == false)
                //{
                //    querry = "TRADECOMPANIONID = '" + loginID + "'";
                //}
                
                //if (dtpStartDate.Checked)
                //{
                //    string val = (this.dtpStartDate.Value.GetDateTimeFormats('d'))[0];
                //    if(querry != "")
                //        querry = querry + " and TIMESTAMPS  >= #" + val + "#";
                //    else
                //        querry = "TIMESTAMPS  >= #" + val + "#";
                //}
                //if (dtpEndDate.Checked)
                //{
                //    string val = (this.dtpEndDate.Value.GetDateTimeFormats('d'))[0];
                //    if (querry != "")
                //         querry = querry + " and TIMESTAMPS  <= #" + val + " 23:59:59#";
                //    else
                //         querry =  "TIMESTAMPS  <= #" + val + " 23:59:59#";
                //}
                
    
            }
            catch//(Exception ex)
            {
                //MessageBox.Show(ex.Message,"TC Admin");
            }
        }
        private void ShowOrders()
        {
            DataView dv = dsOrders.Tables["Orders"].DefaultView;
            //dv.RowFilter = querry;// "TRADECOMPANIONID = '" + loginID + "'";
            grdOrders.DataSource = dv;
            
            grdOrders.RetrieveStructure();
            grdOrders.Tables[0].Columns["ID"].Visible = false;
            grdOrders.Tables[0].Columns["DateIDCustomer"].Caption = "Date/Time Customer";
            grdOrders.Tables[0].Columns["DateID"].Caption = "Date/Time Server";

            //Janus.Windows.GridEX.FilterMode..GridEXColumn.
            //grdOrders.Tables[0].Columns["TimeStamps"].FormatMode = Janus.Windows.GridEX.FormatMode.UseIFormattable;
            grdOrders.Tables[0].Columns["TimeStamps"].FormatString = "dd/MM/yyyy hh:mm:ss tt";
            grdOrders.Tables[0].Columns["DateIDCustomer"].FormatString = "dd/MM/yyyy hh:mm:ss tt";
            grdOrders.Tables[0].Columns["DateID"].FormatString = "dd/MM/yyyy hh:mm:ss tt";

            //grdOrders.Tables[0].Caption = "Executed Orders For " + loginID;
            grdOrders.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            grdOrders.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            grdOrders.AutoSizeColumns();

            //CurrenEx specific change
            grdOrders.Tables[0].Columns["MonthYear"].Visible = false;
            grdOrders.Tables[0].Columns["Exchange"].Visible = false;
            grdOrders.Tables[0].Columns["ExecOrderId"].Visible = false;
            //grdOrders.Tables[0].Columns["TRADECOMPANIONID"].Visible = false;
            grdOrders.Tables[0].Columns["CURRENEXID"].Visible = false;
            grdOrders.Tables[0].Columns["DateIDCustomer"].Position = 0;
            grdOrders.Tables[0].Columns["Price"].Position = 9;

            grdOrders.Refresh();
            Application.DoEvents();
            grdOrders.MoveLast();

            if (grdOrdersLayOut != null)
                grdOrders.LoadLayout(grdOrdersLayOut);
            if (chkShowAll.Checked == false)
                grdOrders.Tables[0].Caption = "Executed Orders For " + loginID;
            else
                grdOrders.Tables[0].Caption = "Executed Orders For All Customers";

        }

        private void grdOrders_GroupsChanged(object sender, Janus.Windows.GridEX.GroupsChangedEventArgs e)
        {
            grdOrdersLayOut = new Janus.Windows.GridEX.GridEXLayout();
            grdOrdersLayOut = grdOrders.DesignTimeLayout;
        }

        private void btnGetOrders_Click(object sender, EventArgs e)
        {
            ShowOrders(loginID);
        }

        private void grdOrders_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                //double d = ((DateTime)(e.Row.Cells["DateIDCustomer"].Value)).ToOADate();
                //e.Row.Cells["DateIDCustomer"].Text = DateTime.FromOADate(d).ToString();

                int actiontype = (int)e.Row.Cells["side"].Value;
                switch(actiontype)
                {
                    case 1:
                        e.Row.Cells["side"].Text = "BUY";
                        break;
                    case 2:
                        e.Row.Cells["side"].Text = "SELL";
                        break;
                }
            }
            catch//(Exception ex)
            {
               // MessageBox.Show(ex.Message, "TC Admin");
            }
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            //isRefresh = true;
            RefreshDataset();
            
            //ShowOrders(loginID);
        }

        public void AsyncCallWebServices()
        {

            WSScalper.WebServicesScalper wsServ = new WSScalper.WebServicesScalper();
            wsServ.GetOrdersFromQueryCompleted += new AdminTC.WSScalper.GetOrdersFromQueryCompletedEventHandler(wsServ_GetOrdersDSFromQueryCompleted);

            string filter = "";
            if (chkShowAll.Checked == false)
            {
                filter = "TRADECOMPANIONID = '" + loginID + "'";
            }
                        
            if (dtpStartDate.Checked)
            {
                string val = (this.dtpStartDate.Value.GetDateTimeFormats('d'))[0];
                if (filter != "")
                    filter = filter + " and TIMESTAMPS  >= '" + val + "'";
                else
                    filter = "TIMESTAMPS  >= '" + val + "'";
            }
            if (dtpEndDate.Checked)
            {
                string val = (this.dtpEndDate.Value.GetDateTimeFormats('d'))[0];
                if (filter != "")
                    filter = filter + " and TIMESTAMPS  <= '" + val + " 23:59:59'";
                else
                    filter = "TIMESTAMPS  <= '" + val + " 23:59:59'";
            }
            string querry = "select * from orders ";
            if (filter != "")
                querry = querry + "where " + filter;

            panelData.Controls.Clear();
            panelData.Controls.Add(spinningProgress1);

            wsServ.GetOrdersFromQueryAsync(querry);
        }

        // CallBack function
        public void wsServ_GetOrdersDSFromQueryCompleted(object sender, WSScalper.GetOrdersFromQueryCompletedEventArgs args)
        {
            if (args.Error != null)
                return;
            
            dsOrders = args.Result;
            panelData.Controls.Clear();
            panelData.Controls.Add(grdOrders);
            ShowOrders();
            //if (isRefresh)
            //    ShowOrders(loginID);
            //else
            //    frmAdmin.GetSingletonAdminform().ShowDefault();
            //isRefresh = false;

        }

        private void Orders_Load(object sender, EventArgs e)
        {
            int did = frmAdmin.GetSingletonAdminform().GetDefaultID();
            //frmAdmin.GetSingletonAdminform().RefreshDataset();
            ShowDetails(did);
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnExportOrders_Click(object sender, EventArgs e)
        {
            Janus.Windows.GridEX.GridEX grid = grdOrders;
            SaveFileDialog sfd = new SaveFileDialog(); 
            sfd.Filter = "Excel file (*.xls)|*.xls"; 
            if (sfd.ShowDialog() == DialogResult.OK) { 
                FileStream stream = null;
                try
                {
                    Janus.Windows.GridEX.Export.GridEXExporter gridExporter = new Janus.Windows.GridEX.Export.GridEXExporter();
                    gridExporter.IncludeFormatStyle = true;
                    gridExporter.GridEX = grid;
                    gridExporter.IncludeCollapsedRows = false;
                    gridExporter.IncludeHeaders = true;
                    gridExporter.GridEX.AutoSizeColumns();
                    
                    stream = new FileStream(sfd.FileName, FileMode.Create);
                    gridExporter.Export(stream);
                    MessageBox.Show("Orders Exported", "TC Admin");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (!((stream == null)))
                    {
                        stream.Close();
                        stream.Dispose();
                        stream = null;
                    }
                }
            }
            else {
                MessageBox.Show("Excel Export Cancel!", "TC Admin"); 
            }
        }

           
        }

    
}
