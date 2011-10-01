using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TradeCompanion;
using TradingInterface;
using System.Windows.Forms;

namespace TestTC
{
    //[TestFixture]
    public class GainTest
    {
        Trader traderObj = new Trader();
        SettingsHome keys = SettingsHome.getInstance();
        AlertWatcher alertWatch = AlertWatcher.getInstance();
        AlertsHome alertHome = new AlertsHome();

        Boolean Status = false;
        Boolean marketStatus = true;
        Boolean orderStatus = false;
        Boolean keeprunning = true; 

        public void SetTrader()
        {
            //traderObj.SetTestModeON();
            keys.ExchangeServer = "5";
            keys.GainBrand = "demo";
            keys.GainMDPort = 3020;
            keys.GainMDHost = "DemoSecondary.efxnow.com";
            keys.GainPassword = "gaindemo";
            keys.GainUserName = "gain3@gmail.com";
            keys.GainPlatform = 0;
            keys.setSettings();
            traderObj = EServerDependents.SetTrader(traderObj);
        }

        [Test]
        public void TestLogOn()
        {
            SetTrader();
            traderObj.Ae.Connected += new AlertExecution.ConnectedEventHandler(Ae_Connected);
            Boolean logonStatus = traderObj.Logon();
            if (logonStatus)
            {
                traderObj.mDoNotConnect = true;
                traderObj.logout();
            }
            Assert.AreEqual(true, Status);
        }

        void Ae_Connected()
        {
            Status = true;
            traderObj.Ae.Connected -= Ae_Connected;
        }

        [Test]
        public void TestLogOut()
        {
            SetTrader();
            traderObj.Ae.Rejected += new AlertExecution.RejectedEventHandler(Ae_Rejected);
            Boolean logonStatus = traderObj.Logon();
            if (logonStatus)
            {
                traderObj.logout();
            }
            Assert.AreEqual(true, Status);
        }

        void Ae_Rejected(string reason)
        {
            Status = true;
            traderObj.Ae.Rejected -= Ae_Rejected; 
        }

        [Test]
        public void TestSubscribeMarketData()
        {
            SetTrader();
            traderObj.Ae.Connected +=new AlertExecution.ConnectedEventHandler(Ae_Connected); 
            Boolean logonStatus = traderObj.Logon();
            if (Status)
            {
                traderObj.Ae.MarketDataUpdated += new AlertExecution.MarketDataUpdatedEventHandler(Ae_MarketDataUpdated);
                traderObj.SubscribeMarketData();
            }
            while (keeprunning)
            {
                System.Threading.Thread.Sleep(1000);
            }
            traderObj.UnSubscribeMarketData();
            traderObj.logout();
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
                newAlert.senderID = "gain3@gmail.com";
                newAlert.contracts = 600000;
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
        [Test]
        public void TestAlertWatcher()
        {
            SetTrader();
            Boolean logonStatus = traderObj.Logon();
            if (logonStatus)
            {
                orderStatus = false;
                alertWatch.NewAlert += new AlertWatcher.NewAlertEventHandler(alertWatch_NewAlert);
                keys.ExecutionMode = 2;
                alertWatch.InitializeMonitorPath(keys.Platform);

                while (keeprunning)
                {
                    System.Threading.Thread.Sleep(5000);
                }
            }
            Assert.AreEqual(true, orderStatus);
        }
        void alertWatch_NewAlert(AlertsManager.NewAlert execute)
        {
            try
            {
                String senderID = "";
                if(execute.timestamp.Equals("simulated")) 
                {
                    senderID = alertHome.MapTSID(execute.senderID);
                }
                else
                {
                    senderID = execute.senderID;
                }
                if (senderID.Equals(""))
                {
                    Console.WriteLine("Something wrong with the alert senderID");
                    keeprunning = false;
                }
                else
                {
                    traderObj.Ae.Executed += new AlertExecution.ExecutedEventHandler(Ae_Executed);
                    traderObj.Ae.OrderPlaced += new AlertExecution.OrderPlacedEventHandler(Ae_OrderPlaced);
                    if (SettingsHome.getInstance().ExchangeServer.Equals("4"))
                    {
                        execute.senderID = senderID;
                        traderObj.send(execute);
                    }
                    else
                    {
                        traderObj.send(execute);
                    }
                }

                alertWatch.NewAlert -= alertWatch_NewAlert; 
                
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                keeprunning = false;
            }
            
        }
    }
}
