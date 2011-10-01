using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
namespace ArielDriver
{
    public delegate void ArielMarketDataResponseEventHandler(object sender, ArielMarketDataStatusEventArgs args);
    public delegate void ArielLoginResponseEventHandler(object sender, int LOGIN_STATUS);
    public delegate void ArielOrderResponseEventHandler(object sender, ArielOrderStatusEventArgs args);
    public enum LOGIN_STATUS { LOGIN_ACCEPTED = 1, LOGIN_REJECTED = 2, LOGIN_TERMINATED = 3, };
   
   
    public delegate void NoArgumetDelegate();

    
    public partial class ArielClientForm : Form
    {
        string SessionId = "";
       
        string marketsReqID = "";
        string accountsReqID = "";
        string dealReqID = "";
        string pricesReqId = "";

        public string SenderID = "";
        
        Hashtable htSymbols;
        ArrayList alAccounts;
        Hashtable htDealID;
        public event ArielMarketDataResponseEventHandler ArielMarketDataResponseEvent;
        public event ArielLoginResponseEventHandler ArielLoginResponseEvent;
        public event ArielOrderResponseEventHandler ArielOrderResponseEvent;

        NoArgumetDelegate delegateListMarktes; 
        NoArgumetDelegate delegateListAccounts;

        private bool isConnected = false;


        private bool isMDSubscribed = false;
        private bool doRaiseEvent = false;
        
        public ArielClientForm()
        {
            InitializeComponent();

            htSymbols = new Hashtable();
            htDealID = new Hashtable();

            Util.WriteDebugLogInfo("-------Initializing Ariel Client------------");
        }
       
        ~ArielClientForm()
        {
            axArielAPI1 = null;
        }
        
        public void ListMarkets()
        {
            try
            {
                if (SessionId != "")
                {
                    htSymbols = new Hashtable();
                    Util.WriteDebugLogInfo("SEND:>>List Markets");
                    marketsReqID = axArielAPI1.ListMarkets(SessionId, 0);
                   
                }
            }
            catch (Exception ex)
            {
                Util.WriteDebugLogInfo("Error: ListMarkets " + ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }

        public void ListAccounts()
        {
            try
            {
                if (SessionId != "")
                {
                    alAccounts = new ArrayList();
                    Util.WriteDebugLogInfo("SEND:>>List Accounts " + SessionId);
                    accountsReqID = axArielAPI1.ListAccounts(SessionId);
                    
                }
            }
            catch (Exception ex)
            {
                Util.WriteDebugLogInfo("Error ListAccounts " + ex.Message);
            }
        }
        
        public bool Login(string userid, string username, string password)
        {
            try
            {
                Util.WriteDebugLogInfo("--------Login Ariel----------");
                Util.WriteDebugLogInfo("SEND:>>Username " + username + " Userid " + userid);
                SenderID = userid;

                short Address = 0;   // connect to default (main) server
                short Language = 9;  // server messages in English

                if (SessionId == "")
                {
                    SessionId = axArielAPI1.Login(username, password, userid, Address, Language);

                    //this will display the connecting event on the GUI
                    ArielLoginResponseEvent(this, (int)LOGIN_STATUS.LOGIN_REJECTED);
                }

            }
            catch (Exception ex)
            {
                Util.WriteDebugLogInfo("Error Login: " + ex.Message);
                //MessageBox.Show(ex.Message);
            }

            return false;
        }
        
        public void Logout()
        {
            Util.WriteDebugLogInfo("SEND:>>Logout Ariel " + SessionId);
            if (SessionId != "" && isConnected)
            {
                try
                {
                    UnSubscribeMarketData();
                    axArielAPI1.Logout(SessionId);
                }
                catch (Exception ex)
                {
                    Util.WriteDebugLogInfo("Error Logout " + ex.Message);
                }
            }
            SessionId = "";
            this.axArielAPI1.LoginEvent -= new AxAPILib._DArielAPIEvents_LoginEventHandler(this.axArielAPI1_LoginEvent);
            this.axArielAPI1.LostConnection -= new AxAPILib._DArielAPIEvents_LostConnectionEventHandler(this.axArielAPI1_LostConnection);
          
        }
        
        public String PlaceOrder(System.String stockSymbol, System.Int32 orderQty, System.Int32 side,System.String currency)
        {

            int marketno;
            short buySell = -1;
            string account = (string)alAccounts[0];
        
            if (htSymbols.ContainsKey(stockSymbol))
                marketno = (int)htSymbols[stockSymbol];
            else
                return "-1"; //symbol not found

            if (side == 1) buySell = 0; // BUY
            else if (side == 2) buySell = 1; //SELL
           

            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String clientOrderID = account + tempDateTime.Substring(4);
            clientOrderID = clientOrderID.ToUpper();
            clientOrderID = clientOrderID.Replace("-", "");
            clientOrderID = clientOrderID.Replace(".", "");
            clientOrderID = clientOrderID.Replace(":", "");

            OrderMessage om = new OrderMessage();
            om.ClientOrderID = clientOrderID;
            om.OrderQty = orderQty;
            om.Side = side;
            om.StockSymbol = stockSymbol;
            om.Currency = currency;
            om.Sender = account;

            Util.WriteDebugLogDebug("SEND:>>Place Order: " + dealReqID + " " + SessionId + " " + marketno + " " + stockSymbol + " " + orderQty + " " + buySell + " " + account);
            dealReqID = axArielAPI1.RequestDeal(SessionId, marketno, orderQty.ToString(), "", 0, "", buySell, account, "");
            htDealID.Add(dealReqID, om);

            return clientOrderID;            
        }
        public bool SubscribeMarketData()
        {
            Util.WriteDebugLogInfo("SEND:>>Subscribe MarketData");
            doRaiseEvent = true;
            if (SessionId != "")
            {
                if (!isMDSubscribed)
                {
                    isMDSubscribed = true;
                    pricesReqId = axArielAPI1.RequestPrices(SessionId, 0);
                    Util.WriteDebugLogInfo("Successfully subscribed MarketData");
                }
                    return true;
            }
            else
            {
                Util.WriteDebugLogInfo("Error Subscribe MarketData: Not Connected to Server");
                return false;
            }
         }
        
        public void UnSubscribeMarketData()
        {
            try
            {
                doRaiseEvent = false;
                Util.WriteDebugLogInfo("UnSubscribe MarketData");
            }
            catch(Exception ex)
            {
                Util.WriteDebugLogInfo("Error UnSubscribeMarketData " + ex.Message);
            }
        }

        private void axArielAPI1_LoginEvent(object sender, AxAPILib._DArielAPIEvents_LoginEvent e)
        {

            Util.WriteDebugLogInfo("RECV:<<Login Response: " + e.sessionId + " " + e.accepted  + " " + e.failureCode + " " + e.failureMessage);
            if (e.accepted == 1)
            {
                SessionId = e.sessionId;

                delegateListMarktes = new NoArgumetDelegate(this.ListMarkets);
                delegateListAccounts = new NoArgumetDelegate(this.ListAccounts);

                delegateListAccounts.BeginInvoke(null, null);
                delegateListMarktes.BeginInvoke(null, null);

                Util.WriteDebugLogInfo("Login Accepted");
                ArielLoginResponseEvent(this, (int)LOGIN_STATUS.LOGIN_ACCEPTED);
                isConnected = true;

            }
            else
            {
                isConnected = false;
                Util.WriteDebugLogInfo("Login Rejected");
                ArielLoginResponseEvent(this, (int)LOGIN_STATUS.LOGIN_REJECTED);
            }
        }

        private void axArielAPI1_LostConnection(object sender, AxAPILib._DArielAPIEvents_LostConnectionEvent e)
        {
            Util.WriteDebugLogInfo("RECV:<<Lost Connection: " + e.sessionId);
            ArielLoginResponseEvent(this, (int)LOGIN_STATUS.LOGIN_TERMINATED);
            isConnected = false;
        }

        private void axArielAPI1_PriceChange(object sender, AxAPILib._DArielAPIEvents_PriceChangeEvent e)
        {
            //Util.WriteDebugLogInfo("RECV:<<Price Changed:" + e.market + " " + e.marketNo + " " + e.bid + " " + e.ask);
            if (doRaiseEvent)
            {
                 if (!htSymbols.ContainsKey(e.market)) htSymbols.Add(e.market, e.marketNo);
                ArielMarketDataStatusEventArgs ev = new ArielMarketDataStatusEventArgs(e);
                ArielMarketDataResponseEvent(this, ev);
            }
        }

        private void axArielAPI1_DealAccepted(object sender, AxAPILib._DArielAPIEvents_DealAcceptedEvent e)
        {

            Util.WriteDebugLogInfo("RECV:<<Order Status " + e.accepted + " " + e.requestId + " " + e.price + " " + e.sessionId + " " + e.dealerMessage);
           
            OrderMessage om = (OrderMessage)htDealID[e.requestId];
            ArielOrderStatusEventArgs ev = new ArielOrderStatusEventArgs(e);
            ev.ClOrdID = om.ClientOrderID;
            ev.FilledQty = om.OrderQty;
            ev.Instrument = om.StockSymbol;
            ev.OrderedQty = om.OrderQty;
            ev.Side = om.Side;
            ev.Currency = om.Currency;
            ev.Sender = om.Sender;

            ArielOrderResponseEvent(this, ev);
        }

        private void axArielAPI1_EndOfList(object sender, AxAPILib._DArielAPIEvents_EndOfListEvent e)
        {
            //if (e.requestId == accountsReqID)
            //{
            //    aseAccounts.Set();
            //}
            //else if (e.requestId == marketsReqID)
            //{
            //    aseMarkets.Set();
            //}
        }
   
        private void axArielAPI1_MarketList(object sender, AxAPILib._DArielAPIEvents_MarketListEvent e)
        {
            //Util.WriteDebugLogInfo("RECV:<<MarketList" + e.market + e.marketNo);
            if (e.requestId == marketsReqID)
                htSymbols.Add(e.market, e.marketNo);
        }

        private void axArielAPI1_AccountList(object sender, AxAPILib._DArielAPIEvents_AccountListEvent e)
        {
            Util.WriteDebugLogInfo("RECV:<<AccountList" + e .name);
            if (e.requestId == accountsReqID)
                alAccounts.Add(e.name);
        }

    }
}