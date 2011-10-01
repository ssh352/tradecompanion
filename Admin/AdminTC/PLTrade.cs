using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AdminTC
{
    // Profit and Loss calculation & display 
    public partial class PLTrade : UserControl,IUserControl
    {
        private Janus.Windows.GridEX.GridEXLayout grdPLTradeLayOut = null;
        DataSet dsPLTrade;
        string LoginID = "";
        Boolean first = true; //for checking the it is first time for that user
        public PLTrade()
        {
            InitializeComponent();

            Janus.Windows.GridEX.GridEXFormatStyle style = new Janus.Windows.GridEX.GridEXFormatStyle();
            style.ForeColor = Color.LightGray;
            style.Key = "Accepted";
            grdPLTrade.FormatStyles.Add(style);

            dateTimePickerFrom.Value = DateTime.Today;
            dateTimePickerFrom.Checked = true;
            dateTimePickerTo.Value = DateTime.Now;
            dateTimePickerTo.Checked = false;

        }

        public void ShowDetails(int id)
        {
            DataSet dsUsers = frmAdmin.GetSingletonAdminform().dsUsers;
            DataRow dr = dsUsers.Tables[0].Rows.Find(id);
            string loginID = (string)dr["LOGINID"];
            showPLTrade(loginID);
        }

        public void RefreshDataset()
        {
            AsyncCallWebServices();
        }

        public void showPLTrade(string loginid)
        {
            this.LoginID = loginid;
            first = true;
            AsyncCallWebServices();
        }

        public void showPLTrade() // to show the grid on from
        {
            DataSet dsProfitLoss = new DataSet();
            DataTable dtProfitLoss = new DataTable("ProfitLoss");
            dsProfitLoss.Tables.Add(dtProfitLoss);
            DataColumn symbol = new DataColumn();
            symbol.DataType = System.Type.GetType("System.String");
            symbol.ColumnName = "Symbol";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(symbol);
            DataColumn netccy1 = new DataColumn();
            netccy1.DataType = System.Type.GetType("System.String");
            netccy1.ColumnName = "NetCCY1";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(netccy1);
            DataColumn netccy2 = new DataColumn();
            netccy2.DataType = System.Type.GetType("System.String");
            netccy2.ColumnName = "NetCCY2";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(netccy2);
            DataColumn avgBuyRate = new DataColumn();
            avgBuyRate.DataType = System.Type.GetType("System.String");
            avgBuyRate.ColumnName = "AvgBuyRate";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(avgBuyRate);
            DataColumn avgSellRate = new DataColumn();
            avgSellRate.DataType = System.Type.GetType("System.String");
            avgSellRate.ColumnName = "AvgSellRate";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(avgSellRate);
            DataColumn allInRate = new DataColumn();
            allInRate.DataType = System.Type.GetType("System.String");
            allInRate.ColumnName = "AllInRate";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(allInRate);
            DataColumn openRate = new DataColumn();
            openRate.DataType = System.Type.GetType("System.String");
            openRate.ColumnName = "OpenRate";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(openRate);
            DataColumn realized = new DataColumn();
            realized.DataType = System.Type.GetType("System.String");
            realized.ColumnName = "Realized";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(realized);
            DataColumn realizedUSD = new DataColumn();
            realizedUSD.DataType = System.Type.GetType("System.Double");
            realizedUSD.ColumnName = "RealizedUSD";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(realizedUSD);
            DataColumn accountID = new DataColumn();
            accountID.DataType = System.Type.GetType("System.String");
            accountID.ColumnName = "AccountID";
            dsProfitLoss.Tables["ProfitLoss"].Columns.Add(accountID);
            
            string str = LoginID;

            if (first)
            {
                cmbAccountID.Items.Clear();
                Hashtable htSenderID = GetSenderIDForTCID(str);
                IDictionaryEnumerator enuSenderID = htSenderID.GetEnumerator();
                if (htSenderID.Count > 0)
                {
                    while (enuSenderID.MoveNext())
                        cmbAccountID.Items.Add(enuSenderID.Key.ToString());
                    cmbAccountID.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbAccountID.SelectedIndex = 0;
                    first = false;                   
                }

            }
          
            string senderID = cmbAccountID.Text;
            Hashtable ht = GetSymbolsTradedByAccount(senderID);
            IDictionaryEnumerator enu = ht.GetEnumerator();
            while (enu.MoveNext())
            {
                DataRow drPL = dsProfitLoss.Tables["ProfitLoss"].NewRow();
                try
                {
                    drPL["Symbol"] = Convert.ToString(enu.Key);
                    decimal rea = GetRealize(Convert.ToString(enu.Key), senderID);
                    decimal nccy1 = getNetccy1(Convert.ToString(enu.Key), senderID);
                    decimal nccy2 = getNetCCY2(Convert.ToString(enu.Key), senderID);
                    drPL["Realized"] = rea;
                    drPL["NetCCY1"] = nccy1;
                    drPL["NetCCY2"] = nccy2;
                    drPL["AvgBuyRate"] = getAvgBuyRate(Convert.ToString(enu.Key), senderID);
                    drPL["AvgSellRate"] = getAvgSellRate(Convert.ToString(enu.Key), senderID);
                    if (nccy1 != 0)
                    {
                        drPL["AllInRate"] = Decimal.Round((-nccy2 / nccy1), 9);
                        drPL["OpenRate"] = Decimal.Round((((-nccy2) + rea) / nccy1), 9);
                    }
                    if (rea != 0)
                    {
                        drPL["RealizedUSD"] = GetRealizedUSD(rea, Convert.ToString(enu.Key), senderID);
                    }
                    else
                    {
                        drPL["RealizedUSD"] = 0;
                    }
                    drPL["AccountID"] = senderID;
                    dsProfitLoss.Tables["ProfitLoss"].Rows.Add(drPL);
                }
                   
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (dsProfitLoss.Tables["ProfitLoss"].Rows.Count > 0)
            {
                DataRow drNet = dsProfitLoss.Tables["ProfitLoss"].NewRow();
                drNet["Symbol"] = "[NET]";
                drNet["RealizedUSD"] = dsProfitLoss.Tables["ProfitLoss"].Compute("Sum(RealizedUSD)", "");
                dsProfitLoss.Tables["ProfitLoss"].Rows.Add(drNet);
            }
            grdPLTrade.DataSource = dsProfitLoss;
            grdPLTrade.SetDataBinding(dsProfitLoss, "ProfitLoss");
            grdPLTrade.RetrieveStructure();
            grdPLTrade.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            grdPLTrade.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            grdPLTrade.Tables[0].Caption = "Profit and Loss for trade done by:-  " + LoginID;
            grdPLTrade.AutoSizeColumns();

            grdPLTrade.Refresh();
            Application.DoEvents();
            grdPLTrade.MoveLast();

            if (grdPLTradeLayOut != null)
                grdPLTrade.LoadLayout(grdPLTradeLayOut);
        }

        public void AsyncCallWebServices() 
        {
            WSScalper.WebServicesScalper wsServ = new AdminTC.WSScalper.WebServicesScalper();
            wsServ.GetPLTradeDSFromQueryCompleted += new AdminTC.WSScalper.GetPLTradeDSFromQueryCompletedEventHandler(wsServ_GetPLTradeDSFromQueryCompleted);
            string sql = "Select * From PLTrade Where TCID = '" + LoginID + "' and DateID >= '" + dateTimePickerFrom.Value.ToOADate() + "'and DateID<= '" + dateTimePickerTo.Value.ToOADate() + "'";
            panPLData.Controls.Clear();
            panPLData.Controls.Add(spinningProgress1);
            wsServ.GetPLTradeDSFromQueryAsync(sql);
                       
        }

        public void wsServ_GetPLTradeDSFromQueryCompleted(object sender, WSScalper.GetPLTradeDSFromQueryCompletedEventArgs args)
        {
            if (args.Error != null)
                return;

            dsPLTrade = args.Result;
            panPLData.Controls.Clear();
            panPLData.Controls.Add(grdPLTrade);
            showPLTrade();   
        }

        public Hashtable GetSenderIDForTCID(string TCID)
        {
            string filter = "";
            filter = "TCID = '" + TCID + "'";
            DataRow[] drs = dsPLTrade.Tables[0].Select(filter);
            return SelectDistinctSenderID(drs, "SenderID");                        
        }
        private Hashtable SelectDistinctSenderID(DataRow[] sourceRows, string sourceColumn)
        {//getting the distinnct senderid 
            try
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in sourceRows)
                {
                    if (!(ht.ContainsKey(dr[sourceColumn])))
                    {
                        ht.Add(dr[sourceColumn], null);
                    }
                }
                return ht;
        }
            catch //(Exception ex)
            {
                return null;
            }
        }

        public Hashtable GetSymbolsTradedByAccount(string accountID)
        {
            string filter = "";
            filter = "SenderID = '" + accountID + "'";
            DataRow[] drs = dsPLTrade.Tables[0].Select(filter);
            return SelectDistinct(drs, "Symbol");
        }

        private Hashtable SelectDistinct(DataRow[] sourceRows, string sourceColumn)
        {
            try
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow dr in sourceRows)
                {
                    if (!(ht.ContainsKey(dr[sourceColumn])))
                    {
                        ht.Add(dr[sourceColumn], null);
                    }
                }
                return ht;
            }
            catch //(Exception ex)
            {
                return null;
            }
        }

        public decimal GetRealize(string symbol, string senderID)
        {
            string filter = "";
            if ((symbol != ""))
            {
                filter = "Symbol = '" + symbol + "'AND SenderID = '" + senderID + "'";
            }
            decimal tRPips = 0;
            tRPips = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(Pips)", filter));
            return decimal.Round(Convert.ToDecimal(tRPips), 2);
        }

        private void PLTrade_Load(object sender, EventArgs e)
        {
            int did = frmAdmin.GetSingletonAdminform().GetDefaultID();
            ShowDetails(did);
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataset();
        }

        private void grdPLTrade_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                double pips;
                if (e.Row.Cells["Realized"].Value != DBNull.Value)
                {
                    pips = Convert.ToDouble(e.Row.Cells["Realized"].Value);
                    Janus.Windows.GridEX.GridEXFormatStyle f = new Janus.Windows.GridEX.GridEXFormatStyle();
                    if (pips > 0)
                    {
                        f.ForeColor = Color.Green;
                        e.Row.Cells["Realized"].FormatStyle = f;
                    }
                    else if (pips < 0)
                    {
                        f.ForeColor = Color.Red;
                        e.Row.Cells["Realized"].FormatStyle = f;
                    }
                }
                pips = Convert.ToDouble(e.Row.Cells["RealizedUSD"].Value);
                Janus.Windows.GridEX.GridEXFormatStyle f1 = new Janus.Windows.GridEX.GridEXFormatStyle();
                if (pips > 0)
                {
                    f1.ForeColor = Color.Green;
                    e.Row.Cells["RealizedUSD"].FormatStyle = f1;
                }
                else if (pips < 0)
                {
                    f1.ForeColor = Color.Red;
                    e.Row.Cells["RealizedUSD"].FormatStyle = f1;
                }
            }
            catch //(Exception ex)
            {

            }
        }
        private decimal getNetccy1(string symbol, string senderID)
        {//calculate the Net trade Quantity for the specified  symbol & snederid
            try
            {
                string filter = "";
                filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 1 ";
                decimal totalBuyAmount = 0;
                DataRow[] drs;
                drs = dsPLTrade.Tables[0].Select(filter);
                if (drs.Length != 0)
                    totalBuyAmount = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(Amount)", filter));
                decimal totalSellAmount = 0;
                filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 2 ";
                drs = dsPLTrade.Tables[0].Select(filter);
                if (drs.Length != 0)
                    totalSellAmount = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(Amount)", filter));
                return (totalBuyAmount - totalSellAmount);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public decimal getNetCCY2(string symbol, string senderID)
        {//Calculation of net amount for specified symbol & senderid
            try
            {
                string filter = "";
                filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 1 ";
                decimal totalBuyNetAmount = 0;
                DataRow[] drs;
                drs = dsPLTrade.Tables[0].Select(filter);
                if (drs.Length != 0)
                    totalBuyNetAmount = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(NetAmount)",filter));
                decimal totalSellNetAmount = 0;
                filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 2 ";
                drs = dsPLTrade.Tables[0].Select(filter);
                if (drs.Length != 0)
                    totalSellNetAmount = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(NetAmount)", filter));
                return (totalSellNetAmount - totalBuyNetAmount);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        public decimal getAvgBuyRate(string symbol, string senderID)
        {
            try
            {
                string filter = "";
                filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 1 ";
                DataRow[] drs = dsPLTrade.Tables[0].Select(filter);
                decimal totalAmount = 0;
                decimal avBR = 0;
                decimal sum = 0;
                if (drs.Length != 0)
                {
                    totalAmount = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(Amount)", filter));
                    sum = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(NetAmount)", filter));
                    avBR = sum / totalAmount;
                }
                return decimal.Round(avBR, 9);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public decimal getAvgSellRate(string symbol, string senderID)
        {
            try {
                string filter = "";
                filter = "Symbol = '" + symbol + "' AND SenderID = '" + senderID + "' AND Actions = 2 ";
                DataRow[] drs = dsPLTrade.Tables[0].Select(filter);
                decimal totalAmount = 0;
                decimal avSR = 0;
                if (drs.Length != 0) {
                    totalAmount = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(Amount)", filter));
                    decimal sum = Convert.ToDecimal(dsPLTrade.Tables[0].Compute("Sum(NetAmount)", filter));
                    avSR = sum / totalAmount;
                }
                return decimal.Round(avSR, 9);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            AsyncCallWebServices();
            showPLTrade();
        }
        private decimal GetRealizedUSD(decimal re, string symbol, string senderid)
        {
            string filter = "Symbol = '" + symbol + "' and SenderID = '" + senderid + "' AND USDMarketPrice > 0" ;
            DataRow[] drs = dsPLTrade.Tables[0].Select(filter,"DateID");
            decimal mr = 1;
            Int32 i = drs.Length;
            if (i > 0)
            {
                DataRow dr = drs[i - 1];
                if (dr["USDMarketPrice"] != DBNull.Value)
                    mr = Convert.ToDecimal(dr["USDMarketPrice"]);                
            }
            return Decimal.Round((re / mr), 2);
        }

        private void btnPLExport_Click(object sender, EventArgs e)
        {
            Janus.Windows.GridEX.GridEX grid = grdPLTrade;
            SaveFileDialog sfd  = new SaveFileDialog();
            sfd.Filter = "Excel file (*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream stream ;
                Janus.Windows.GridEX.Export.GridEXExporter gridexport = new Janus.Windows.GridEX.Export.GridEXExporter();
                gridexport.IncludeFormatStyle = true;
                gridexport.GridEX = grid;
                stream = new FileStream(sfd.FileName, FileMode.Create);
                gridexport.Export(stream);
            }

        }
                   
    }
}
