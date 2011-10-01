using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TradeCompanion;
using TradingInterface;

namespace TestTC
{
    //[TestFixture]
    public class Ariel
    {
        Trader traderObj = new Trader();
        SettingsHome keys = SettingsHome.getInstance();
        AlertWatcher alertWatch = AlertWatcher.getInstance();
        AlertsHome alertHome = new AlertsHome();

        Boolean Status = false;
        Boolean marketStatus = true;
        Boolean orderStatus = false;
        Boolean keeprunning = true;

        private void SetTrader()
        {
            //traderObj.SetTestModeON();
            keys.ExchangeServer = "2";
            keys.ArielLoginUserID = "Test14";
            keys.ArielLoginPassword  = "MG1014";
            keys.ArielLoginUserName = "Test14";
            keys.setSettings();
            traderObj = EServerDependents.SetTrader(traderObj);
        }

        [Test]
        public void TestLogOn()
        {
            SetTrader();
            traderObj.Ae.Connected += new AlertExecution.ConnectedEventHandler(Ae_Connected);
            traderObj.Ae.Rejected += new AlertExecution.RejectedEventHandler(Ae_Rejected);
            Boolean logonStatus = traderObj.Logon();
            System.Threading.Thread.Sleep(50000);
            if (Status)
            {
                traderObj.logout();
            }
            Assert.AreEqual(false, Status);
        }

        void Ae_Rejected(string reason)
        {
            Console.WriteLine("Rejected event called");
            Status = false;
        }

        void Ae_Connected()
        {
            Console.WriteLine("Connected event called");
            Status = true;
        }


        [Test]
        public void TestLogOut()
        {
            SetTrader();
            traderObj.Ae.Connected +=new AlertExecution.ConnectedEventHandler(Ae_Connected);
            Boolean logonStatus = traderObj.Logon();
            System.Threading.Thread.Sleep(10000);
            if (Status)
            { 
                traderObj.Ae.Rejected +=new AlertExecution.RejectedEventHandler(Ae_Rejected);
                traderObj.logout();
            }

            Assert.AreEqual(false, Status);
        }

        [Test]
        public void TestSubscription()
        {
            SetTrader();
            traderObj.Ae.Connected += new AlertExecution.ConnectedEventHandler(Ae_Connected);
            Boolean logonStatus = traderObj.Logon();
            System.Threading.Thread.Sleep(10000);
            if (Status)
            {
                traderObj.Ae.MarketDataUpdated += new AlertExecution.MarketDataUpdatedEventHandler(Ae_MarketDataUpdated);
                keeprunning = true;
                traderObj.SubscribeMarketData();
                while (keeprunning)
                {
                    System.Threading.Thread.Sleep(10000);
                }

                traderObj.UnSubscribeMarketData();
                traderObj.logout();
            }

            Assert.AreEqual(true, marketStatus);
        }

        void Ae_MarketDataUpdated(FillMarketData mdata)
        {
            marketStatus = true;
            keeprunning = false;
            traderObj.Ae.MarketDataUpdated -= Ae_MarketDataUpdated;
        }

        [Test]
        public void TestOrderPlace()
        {
            SetTrader();
            Boolean logonStatus = traderObj.Logon();
            if (logonStatus)
            {
                traderObj.Ae.Executed += new AlertExecution.ExecutedEventHandler(Ae_Executed);
                traderObj.Ae.OrderPlaced += new AlertExecution.OrderPlacedEventHandler(Ae_OrderPlaced);
                AlertsManager.NewAlert newAlert = new AlertsManager.NewAlert();
                newAlert.senderID = "Test14";
                newAlert.contracts = 100000;
                newAlert.symbol = "EUR/USD";
                newAlert.currency = "EUR";
                newAlert.tradeType = 3;
                newAlert.actiontype = AlertsManager.ACTION_BUY;
                traderObj.send(newAlert);
            }
            traderObj.logout();
            Assert.AreEqual(true, orderStatus);
        }
      
        void Ae_OrderPlaced()
        {
            orderStatus = true;
            keeprunning = false;
            traderObj.Ae.OrderPlaced -= Ae_OrderPlaced;
        }
        void Ae_Executed()
        {
            orderStatus = true;
            keeprunning = false;
            traderObj.Ae.Executed -= Ae_Executed;
        }
    
    
    }
}
