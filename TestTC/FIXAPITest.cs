using System;
using System.Collections.Generic;
using System.Text;

using TradeCompanion;
using TradingInterface;
using NUnit.Framework;
using System.Configuration; 

namespace TestTC
{
    
    public class FIXAPITest 
    {
        Trader traderObj;
        protected SettingsHome keys = SettingsHome.getInstance();
        AlertWatcher alertWatch = AlertWatcher.getInstance();
        AlertsHome alertHome = new AlertsHome();

        Boolean Status;
        Boolean keepRunning;
        Boolean orderStatus;
        Boolean marketSataus;
        Boolean marketDataLogin;


        #region Set Settings 

        private void SetTrader()
        {
            ReadXML();
            traderObj = new Trader();
            traderObj.SetTestMode();

            if (marketDataLogin && (keys.ExchangeServer == "1" || keys.ExchangeServer == "7" || keys.ExchangeServer == "8"))
            {
                traderObj = EServerDependents.SetTraderMarketData(traderObj);
            }
            else
            {
                traderObj = EServerDependents.SetTrader(traderObj);
            }
            keepRunning = true;
            Status = false;
            orderStatus = false;
        }

        private void ReadXML()
        {
            
            //ConfigurationManager.RefreshSection("appSettings");
            String exServer = ConfigurationManager.AppSettings["ExchangeServer"].ToString();
            //keys.ExchangeServer = exServer;
            switch (exServer)
            {
                case "FxIntegral":
                    Console.WriteLine("Test case for FxIntegral");
                    keys.ExchangeServer = "8";
                    keys.FxIntLoginPassword = GetAppsetting("FxIntLoginPassword");
                    keys.FxIntLoginPort = GetAppsetting("FxIntLoginPort");
                    keys.FxIntLoginSendercompId = GetAppsetting("FxIntLoginSendercompId");
                    keys.FxIntLoginTargetcompId = GetAppsetting("FxIntLoginTargetcompId");
                    keys.FxIntLoginUserName = GetAppsetting("FxIntLoginUserName");
                    keys.FxIntLoginLegalEntity = GetAppsetting("FxIntLoginLegalEntity");

                    keys.FxIntegralMarkLegalEntity = GetAppsetting("FxIntegralMarkLegalEntity");
                    keys.FxIntegralIPMarketData = GetAppsetting("FxIntegralIPMarketData");
                    keys.FxIntegralPortMarketData = GetAppsetting("FxIntegralPortMarketData");
                    keys.FxIntegralSenderMarketData = GetAppsetting("FxIntegralSenderMarketData");
                    keys.FxIntegralTargetMarketData = GetAppsetting("FxIntegralTargetMarketData");
                    keys.FxIntegralUserNameMarketData = GetAppsetting("FxIntegralUserNameMarketData");
                    keys.FxIntLoginPassword = GetAppsetting("FxIntLoginPassword");
                    
                    break;
                case "Dukascopy":
                    Console.WriteLine("Test case for Dukascopy");
                    keys.ExchangeServer = "7";
                    keys.DUKASLoginIP = GetAppsetting("DUKASLoginIP");
                    keys.DUKASLoginIPMarketData = GetAppsetting("DUKASLoginIPMarketData");
                    keys.DUKASLoginPassword = GetAppsetting("DUKASLoginPassword");
                    keys.DUKASLoginPort = GetAppsetting("DUKASLoginPort");
                    keys.DUKASLoginPortMarketData = GetAppsetting("DUKASLoginPortMarketData");
                    keys.DUKASLoginSender = GetAppsetting("DUKASLoginSender");
                    keys.DUKASLoginSenderMarketData = GetAppsetting("DUKASLoginSenderMarketData");
                    keys.DUKASLoginTarget = GetAppsetting("DUKASLoginTarget");
                    keys.DUKASLoginTargetMarketData = GetAppsetting("DUKASLoginTargetMarketData");
                    keys.DUKASLoginUserName = GetAppsetting("DUKASLoginUserName");
                    keys.DUKASLoginUserNameMarketData = GetAppsetting("DUKASLoginUserNameMarketData");
                    break;
                case "Icap":
                    Console.WriteLine("Test case for Icap");
                    keys.ExchangeServer = "6";
                    keys.IcapIP = GetAppsetting("IcapIP"); ;
                    keys.IcapPassword = GetAppsetting("IcapPassword"); ;
                    keys.IcapPort = GetAppsetting("IcapPort"); ;
                    keys.IcapUserName = GetAppsetting("IcapUserName"); ;
                    break;
                case "Gain":
                    Console.WriteLine("Test case for Gain");
                    keys.ExchangeServer = "5";
                    keys.GainBrand = GetAppsetting("GainBrand");
                    keys.GainMDHost = GetAppsetting("GainMDHost");
                    keys.GainMDPort = Convert.ToInt32(GetAppsetting("GainMDPort"));
                    keys.GainPassword = GetAppsetting("GainPassword");
                    keys.GainPlatform = Convert.ToInt32(GetAppsetting("GainPlatform"));
                    keys.GainUserName = GetAppsetting("GainUserName"); ;

                    break;
                case "DBFX":
                    Console.WriteLine("Test case for DBFX");
                    keys.ExchangeServer = "4";
                    keys.DBFXAccountType = GetAppsetting("DBFXAccountType");
                    keys.DBFXPassword = GetAppsetting("DBFXPassword");
                    keys.DBFXURL = GetAppsetting("DBFXURL");
                    keys.DBFXUserName = GetAppsetting("DBFXUserName");
                    break;
                case "Espeed":
                    Console.WriteLine("Test case for Espeed");
                    keys.ExchangeServer = "3";
                    break;
                case "Airel":
                    Console.WriteLine("Test case for Airel");
                    keys.ExchangeServer = "2";
                    keys.ArielClient = GetAppsetting("ArielClient");
                    keys.ArielLoginPassword = GetAppsetting("ArielLoginPassword");
                    keys.ArielLoginUserID = GetAppsetting("ArielLoginUserID");
                    keys.ArielLoginUserName = GetAppsetting("ArielLoginUserName");
                    break;
                case "CurrenEx":
                    Console.WriteLine("Test case for CurrenEx");
                    keys.ExchangeServer = "1";
                    keys.CXLoginIP = GetAppsetting("CXLoginIP");
                    keys.CXLoginPassword = GetAppsetting("CXLoginPassword");
                    keys.CXLoginPort = GetAppsetting("CXLoginPort");
                    keys.CXLoginSender = GetAppsetting("CXLoginSender");
                    keys.CXLoginTarget = GetAppsetting("CXLoginTarget");
                    keys.LoginIPMarketData = GetAppsetting("LoginIPMarketData");
                    keys.LoginPortMarketData = GetAppsetting("LoginPortMarketData");
                    keys.LoginSenderMarketData = GetAppsetting("LoginSenderMarketData");
                    keys.LoginTargetMarketData = GetAppsetting("LoginTargetMarketData");
                    
                    break;
            }
        }

        private string GetAppsetting(String tag)
        {
            return ConfigurationManager.AppSettings[tag].ToString();
        }

        #endregion 

        #region Methods that can be called by the testing class

        public Boolean LogOnAndLogOut()
        {
            Boolean logonStatus = FIXLogOn();
            if (logonStatus)
            {
                Console.WriteLine("Login completed, calling for logout user");
                FIXLogOut();
            }
            return Status;
        }

        public Boolean SubscribeMarketData()
        {
            marketDataLogin = true;
            Boolean logonStat = FIXLogOn();
            if (logonStat)
            {
                FIXMarketdataSubscription();
                FIXLogOut();
            }
            return marketSataus;
        }

        public Boolean PlaceOrderBUY()
        {
            Boolean logonStat = FIXLogOn();
            if (logonStat)
            {
                FIXPlaceOrder(1);
                FIXLogOut();
            }
            return orderStatus;
        }

        public Boolean PlaceOrderSELL()
        {
            Boolean logonStat = FIXLogOn();
            if (logonStat)
            {
                FIXPlaceOrder(2);
                FIXLogOut();
            }
            return orderStatus;
        }

        #endregion

        #region TC Trader class methods

            private Boolean FIXLogOn()
            {
                SetTrader();
                traderObj.Ae.Connected += new AlertExecution.ConnectedEventHandler(Ae_Connected);
                Boolean logonStatus = traderObj.Logon();
                return logonStatus;
            }

            private void FIXLogOut()
            {
                traderObj.Ae.Rejected += new AlertExecution.RejectedEventHandler(Ae_Rejected);
                traderObj.logout();
            }

            private void FIXMarketdataSubscription()
            {
                traderObj.Ae.MarketDataUpdated += new AlertExecution.MarketDataUpdatedEventHandler(Ae_MarketDataUpdated);
                traderObj.SubscribeMarketData();
                while(keepRunning) { System.Threading.Thread.Sleep(500); }
            }

            private void FIXPlaceOrder(int actionType)
            {
                traderObj.Ae.Executed += new AlertExecution.ExecutedEventHandler(Ae_Executed);
                traderObj.Ae.OrderPlaced += new AlertExecution.OrderPlacedEventHandler(Ae_OrderPlaced);
                AlertsManager.NewAlert newAlert = new AlertsManager.NewAlert();
                newAlert.senderID = traderObj.ConnectionId;
                newAlert.contracts = 100000;
                newAlert.symbol = "EUR/USD";
                newAlert.currency = "EUR";
                newAlert.tradeType = 3;
                newAlert.timestamp = "simulated";
                newAlert.actiontype = actionType;
                traderObj.send(newAlert);
            }

        #endregion

        #region Event handlers

            void Ae_OrderPlaced()
            {
                Console.WriteLine("Order placed"); 
                orderStatus = true;
            }

            void Ae_Executed()
            {
                Console.WriteLine("Order executed"); 
                orderStatus = true;
            }

            void Ae_MarketDataUpdated(FillMarketData mdata)
            {
                marketSataus = true;
                Console.WriteLine("symbol" + mdata.Symbol);
                keepRunning = false;
            }

            void Ae_Connected()
            {
                Console.WriteLine("Connnection event raise");
            }

            void Ae_Rejected(string reason)
        {
            Console.WriteLine("DisConnnection event raise");
            Status = true;
            keepRunning = false;
        }
        
        #endregion

    }
}