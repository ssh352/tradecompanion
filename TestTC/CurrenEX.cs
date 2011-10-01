using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TradeCompanion;
using TradingInterface; 

namespace TestTC
{
    //[TestFixture]
    public class CurrenEX
    {
        Trader traderObj = new Trader();
        SettingsHome keys = SettingsHome.getInstance();
        AlertWatcher alertWatch = AlertWatcher.getInstance();
        AlertsHome alertHome = new AlertsHome();

        Boolean Status;
        Boolean keepRunning;
        Boolean orderStatus;

        void SetTrader()
        {
            //traderObj.SetTestModeON();
            keys.ExchangeServer = "1";
            keys.CXLoginPassword = "test1234";
            keys.setSettings();
            traderObj = EServerDependents.SetTrader(traderObj);
            keepRunning = true;
        }
        void Ae_Rejected(string reason)
        {
            Console.WriteLine("Me Logon rejectd");
            Status = false;
            keepRunning = false;

        }
        void Ae_Connected()
        {
            Console.WriteLine("Me Logon Connected");
            Status = true;
            keepRunning = false;
        }

        [Test]
        public void TestLogOn()
        {
            Console.WriteLine("TestLogOn()");
            SetTrader();
            traderObj.Ae.Connected += new AlertExecution.ConnectedEventHandler(Ae_Connected);
            traderObj.Ae.Rejected += new AlertExecution.RejectedEventHandler(Ae_Rejected);
            Boolean logonStatus = traderObj.Logon();
            while (keepRunning)
            {
                System.Threading.Thread.Sleep(40000);
            }
            if (logonStatus)
            {
                System.Threading.Thread.Sleep(20000);
                traderObj.logout();
            }

            Assert.AreEqual(false, keepRunning);

        }

        [Test]
        public void TestLogOut()
        {
            Console.WriteLine("TestLogOut()");
            SetTrader();
            traderObj.Ae.Connected +=new AlertExecution.ConnectedEventHandler(Ae_Connected);
            Boolean logonStatus = traderObj.Logon();
            while (keepRunning)
            {
                System.Threading.Thread.Sleep(40000);
            }
            if (logonStatus)
            {
                traderObj.Ae.Rejected += new AlertExecution.RejectedEventHandler(Ae_Rejected);
                traderObj.logout();
            }
            System.Threading.Thread.Sleep(10000);
            Assert.AreEqual(false, Status);
        }

        [Test]
        public void TestSubscribeMarketData()
        {
            Console.WriteLine("TestSubscribeMarketData()");
            SetMarketTrader();
            traderObj.Ae.MarketDataUpdated += new AlertExecution.MarketDataUpdatedEventHandler(Ae_MarketDataUpdated);
            Boolean logonStatus = traderObj.Logon();
            traderObj.SubscribeMarketData();
            while (keepRunning)
            {
                System.Threading.Thread.Sleep(1000);
            }
            traderObj.UnSubscribeMarketData();

            System.Threading.Thread.Sleep(10000);
            Assert.AreEqual(false, keepRunning);
        }

        private void SetMarketTrader()
        {
            //traderObj.SetTestModeON();
            keys.ExchangeServer = "1";
            keys.CXLoginPassword = "test1234";
            keys.setSettings();
            traderObj = EServerDependents.SetTraderMarketData(traderObj);
            keepRunning = true;
        }

        void Ae_MarketDataUpdated(FillMarketData mdata)
        {
            Console.WriteLine("Symbol - " + mdata.Symbol + " BidPrice - " + mdata.BidPrice + " OfferPrice - " + mdata.OfferPrice);
            keepRunning = false;
            traderObj.Ae.MarketDataUpdated -= Ae_MarketDataUpdated;
        }

        [Test]
        public void TestOrderPlace()
        {
            Console.WriteLine("TestOrderPlace()");
            SetTrader();
            Boolean logonStatus = traderObj.Logon();
            if (logonStatus)
            {
                traderObj.Ae.Executed += new AlertExecution.ExecutedEventHandler(Ae_Executed);
                traderObj.Ae.OrderPlaced += new AlertExecution.OrderPlacedEventHandler(Ae_OrderPlaced);
                AlertsManager.NewAlert newAlert = new AlertsManager.NewAlert();
                newAlert.senderID = "scalpercust1u6trade";
                newAlert.contracts = 100000;
                newAlert.symbol = "EUR/USD";
                newAlert.currency = "EUR";
                newAlert.tradeType = 3;
                newAlert.actiontype = AlertsManager.ACTION_BUY;
                traderObj.send(newAlert);
            }
            //while (keepRunning)
            //{
                System.Threading.Thread.Sleep(5000);
            //}
            traderObj.logout();

            System.Threading.Thread.Sleep(10000);
            Assert.AreEqual(true, orderStatus);
        }

        void Ae_OrderPlaced()
        {
            orderStatus = true;
            keepRunning = false;
            traderObj.Ae.OrderPlaced -= Ae_OrderPlaced;
        }
        void Ae_Executed()
        {
            orderStatus = true;
            keepRunning = false;
            traderObj.Ae.Executed -= Ae_Executed;
        }

    }
}
