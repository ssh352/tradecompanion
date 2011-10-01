using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using Dukascopy.com.scalper.util;

namespace Dukascopy.com.scalper.fix.driver.client
{
    public delegate void DukasCopyOrderResponseEventHandler(object sender, DukascopyOrderStatusEventArgs args);
    public delegate void DukasCopyMarketDataResponseEventHandler(object sender, DukascopyMarketDataStatusEventArgs args);
    public delegate void DukasCopyOpenPositionEventHandler(object sender, DukascopyOpenPositionEventArgs args);

    public class DukascopyClient : DefaultClient
    {
        //This is the event that is raised whenever a response is received from Ffastfill
        public /*static*/ event DukasCopyOrderResponseEventHandler dukasCopyOrderResponseEvent;
        public /*static*/ event DukasCopyMarketDataResponseEventHandler dukasCopyMarketDataResponseEvent;
        public event DukasCopyOpenPositionEventHandler dukascopyOpenPositonEvent;

        public double pipValue = 0;
        private Hashtable htSymbols = new Hashtable();
        public DukascopyClient(System.String hostname, int port)
            : base(hostname, port)
        {
        }

        public bool setPip(String pipFilePath)
        {
            if (!File.Exists(pipFilePath))
                return false;
            String encryptedData = readData(pipFilePath);

            if (encryptedData != null)
            {
                String decryptedData = EncDec.Decrypt(encryptedData.Trim(), "S7isCool!", "FrancoRocks!", "SHA1",
                             2, "S7smetsysScalper", 256);
                if (decryptedData != null)
                {
                    try
                    {
                        pipValue = float.Parse(decryptedData);
                        return true;
                    }
                    catch //(Exception e)
                    {
                        pipValue = 100;
                    }
                }
            }
            return false;
        }

        private String readData(String filePath)
        {
            if (filePath != null && filePath.Trim().Length > 0)
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                StringBuilder sb = new StringBuilder();
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line);
                }
                sr.Close();
                fs.Close();
                return sb.ToString();
            }
            else
                return null;
        }

        protected override void handleMsgReceived(Dukascopy.com.scalper.fix.Message msg)
        {
            //System.Console.WriteLine("Message received:" + msg);
            if (msg.MsgTypeChar == Constants_Fields.MSGMarketDataSnapshotFullRefreshChar)
            {
                DukascopyMarketDataStatusEventArgs ev = new DukascopyMarketDataStatusEventArgs(msg);

                if (!htSymbols.ContainsKey(ev.Symbol))
                {
                    MarketData md = new MarketData();
                    md.Ask = ev.OfferPrice;
                    md.Bid = ev.BidPrice;
                    md.Symbol = ev.Symbol;
                    htSymbols.Add(ev.Symbol, md);
                    log.Debug("Ignoring Bad Ticks.");
                    return;
                }
                else
                {
                    MarketData md = (MarketData)htSymbols[ev.Symbol];
                    if (ev.BidPrice != "")
                    {
                        md.Bid = ev.BidPrice;
                        md.IsBidBadTick = false;
                    }
                    else
                    {
                        if (!md.IsBidBadTick)
                            ev.BidPrice = md.Bid;
                    }
                    if (ev.OfferPrice != "")
                    {
                        md.Ask = ev.OfferPrice;
                        md.IsAskBadTick = false;
                    }
                    else
                    {
                        if (!md.IsAskBadTick)
                            ev.OfferPrice = md.Ask;
                    }

                    if (!md.IsAskBadTick && !md.IsBidBadTick)
                    {
                        dukasCopyMarketDataResponseEvent(this, ev);
                    }
                }


            }
            else if (msg.MsgTypeChar == Constants_Fields.MSGExecutionChar)
            {
                if (dukasCopyOrderResponseEvent != null)
                {
                    DukascopyOrderStatusEventArgs ev = new DukascopyOrderStatusEventArgs(msg);
                    //DukasCopyOrderResponseEvent(this, ev);                    dukasCopyOrderResponseEvent(this, ev);
                }
            }
            else if (msg.MsgType == Constants_Fields.MSGInstrumentPositionInfo)
            {
                if (dukascopyOpenPositonEvent != null)
                {
                    DukascopyOpenPositionEventArgs openPositionEvent = new DukascopyOpenPositionEventArgs(msg);
                    dukascopyOpenPositonEvent(this, openPositionEvent);
                }
            }
            else if (msg.MsgTypeChar == Constants_Fields.MSGLogoutChar)
            {
            }
        }

        private void AddPip(DukascopyOrderStatusEventArgs ev)
        {
            //2 -- Filled, 1 -- Partially filled.
            if (ev.OrderStatus.Equals("2") || ev.OrderStatus.Equals("1"))
            {
                long tradedQuantity = ev._fixMsg.getLongValue(Constants_Fields.TAGCumQty_i);
                double price = ev._fixMsg.getDoubleFieldValue(Constants_Fields.TAGAvgPx);
                double tradedAmount = System.Math.Round(tradedQuantity * price, 4);
                ev._fixMsg.setValue(Constants_Fields.TAGNonPippedTotalPrice_i, tradedAmount);
                double pipPrice = price + getPippedPrice(price, pipValue);
                ev._fixMsg.setValue(Constants_Fields.TAGPipPrice_i, pipPrice);
                double pippedTradedAmount = System.Math.Round(tradedQuantity * pipPrice, 4);
                ev._fixMsg.setValue(Constants_Fields.TAGPippedTotalPrice_i, pippedTradedAmount);
            }
        }

        private double getPippedPrice(double price, double pipPercentage)
        {
            String curPrice = price.ToString(BasicUtilities.getCulture());
            String decString = curPrice.Substring(curPrice.IndexOf('.') + 1);
            StringBuilder sbPip = new StringBuilder().Append('.');
            for (int i = 0; i < (decString.Length - 1); i++)
            {
                sbPip.Append(0);
            }
            sbPip.Append(1);
            String fullPip = sbPip.ToString();
            if (fullPip != null && fullPip.Length > 0)
            {
                double iFullPip = double.Parse(fullPip, BasicUtilities.getCulture());
                double actualPip = iFullPip * pipPercentage / 100;
                return actualPip;
            }
            else
                return 0;
        }

        public bool logon(String senderCompID, String targetCompID, String userName, String Password)
        {
            log.Debug("logon came for id " + senderCompID);
            if ((this.Session != null) && (this.Session.LoggedIn))
                throw new System.SystemException("Client: Already logged in.");

            if (senderCompID != null)
                this.SenderCompID = senderCompID;

            if (targetCompID != null)
                this.TargetCompID = targetCompID;

            Message msg = new Message(Constants_Fields.MSGLogon);

            msg.setStringFieldValue(Constants_Fields.TAGSenderCompID, this.SenderCompID);
            msg.setStringFieldValue(Constants_Fields.TAGTargetCompID, this.TargetCompID);

            DateTime t = DateTime.Now.ToUniversalTime(); //Converted to GMT time
            string tempDateTime = Message.buildDateString(ref t, false, true);

            msg.setStringFieldValue(Constants_Fields.TAGResetSeqNumFlag, "Y");
            msg.setStringFieldValue(Constants_Fields.TAGSendingTime, tempDateTime);
            msg.setStringFieldValue(Constants_Fields.TAGEncryptMethod, "0");
            msg.setStringFieldValue(Constants_Fields.TAGHeartBtInt, HeartBeatInterval.ToString());
            msg.setStringFieldValue(Constants_Fields.TAGCUPassword, Password); //pwreset
            msg.setStringFieldValue(Constants_Fields.TAGUserName, userName);

            log.Debug("Sending the LOGON message to the FIX server.");

            sendMessage(msg);

            if (Session.waitUntilReady(15000) == false)
            {
                logout();
                return false;
            }
            log.Debug("logon connected for id " + senderCompID);
            return true;

        }

        public String cancelOrder(System.String stockSymbol, String origClOrderId, String orderId, System.Int32 side, String text)
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                throw new System.SystemException("Client: Cannot place orders before logging in.");

            DateTime DtTime = DateTime.Now.ToUniversalTime();//Vm_ Fix converting to GMT time
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String clientOrderID = SenderCompID + tempDateTime.Substring(4);
            clientOrderID = clientOrderID.ToUpper();
            clientOrderID = clientOrderID.Replace("-", "");
            clientOrderID = clientOrderID.Replace(".", "");
            clientOrderID = clientOrderID.Replace(":", "");

            Message msg = new Message(Constants_Fields.MSGOrderCancelRequest);
            msg.setStringFieldValue(Constants_Fields.TAGClOrdID, clientOrderID);
            msg.setStringFieldValue(Constants_Fields.TAGOrigClOrdID, origClOrderId);
            msg.setStringFieldValue(Constants_Fields.TAGOrderID, orderId);
            msg.setStringFieldValue(Constants_Fields.TAGSendingTime, tempDateTime);
            msg.setStringFieldValue(Constants_Fields.TAGSymbol, stockSymbol);
            msg.setStringFieldValue(Constants_Fields.TAGTransactTime, tempDateTime);
            msg.setStringFieldValue(Constants_Fields.TAGSide, side.ToString());

            sendMessage(msg);

            return clientOrderID;
        }

        public String placeOrder(System.String stockSymbol, System.Int32 orderQty, System.Int32 side, String curency, System.Int32 orderType)
        {
            return placeOrder(stockSymbol, orderQty, side, 0, curency, orderType);
        }

        public String placeOrder(System.String stockSymbol, System.Int32 orderQty, System.Int32 side, System.Double orderPrice, String curency, System.Int32 orderType)
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                throw new System.SystemException("Client: Cannot place orders before logging in.");

            DateTime DtTime = DateTime.Now.ToUniversalTime();//Vm_ Fix converting to GMT time
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String clientOrderID = SenderCompID + tempDateTime.Substring(4);
            clientOrderID = clientOrderID.ToUpper();
            clientOrderID = clientOrderID.Replace("-", "");
            clientOrderID = clientOrderID.Replace(".", "");
            clientOrderID = clientOrderID.Replace(":", "");

            Message msg = new Message(Constants_Fields.MSGOrder);
            msg.setStringFieldValue(Constants_Fields.TAGClOrdID, clientOrderID);

            msg.setStringFieldValue(Constants_Fields.TAGSymbol, stockSymbol);
            msg.setStringFieldValue(Constants_Fields.TAGSendingTime, tempDateTime);
            msg.setStringFieldValue(Constants_Fields.TAGOrderQty, orderQty.ToString());
            msg.setStringFieldValue(Constants_Fields.TAGTransactTime, tempDateTime);
            msg.setStringFieldValue(Constants_Fields.TAGSide, side.ToString());

            msg.setStringFieldValue(Constants_Fields.TAGHandlInst, "1");
            msg.setStringFieldValue(Constants_Fields.TAGCurrency, curency);
            msg.setStringFieldValue(Constants_Fields.TAGOrdType, "1");
            //msg.setStringFieldValue(Constants_Fields.TAGTimeInForce, "3");
            msg.setStringFieldValue(Constants_Fields.TAGTimeInForce, orderType.ToString());
            //Price is optional. It is not needed for Market orders when placing the order.
            if (orderPrice != 0)
            {
                msg.setStringFieldValue(Constants_Fields.TAGPrice, orderPrice.ToString());
            }

            sendMessage(msg);

            return clientOrderID;
        }

        public void SubscribeMarketData(String[] symbols)
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                throw new System.SystemException("Client: Cannot get market data before logging in.");

            htSymbols = new Hashtable();
            for (int i = 0; i < symbols.Length - 1; i++)
            {
                Message msg = new Message(Constants_Fields.MSGMarketDataRequest);

                DateTime DtTime = DateTime.Now.ToUniversalTime();//Vm_ Fix converting to GMT time
                string tempDateTime = Message.buildDateString(ref DtTime, false, true);

                msg.addValue(Constants_Fields.TAGMDReqID, SenderCompID + tempDateTime.Substring(4)); //	Must be unique, or the ID of previous Market Data Request to disable if SubscriptionRequestType = Disable previous Snapshot  + Updates Request (2).
                msg.addValue(Constants_Fields.TAGSubscriptionRequestType, "1");//	SubcriptionRequestType indicates to the other party what type of response is expected. A snapshot request only asks for current information. A subscribe request asks for updates as the status changes. Unsubscribe will cancel any future update messages from the counter party.
                msg.addValue(Constants_Fields.TAGMarketDepth, "1");
                msg.addValue(Constants_Fields.TAGMDUpdateType_i, '0');
                msg.addValue(Constants_Fields.TAGNoMDEntryTypes_i, '2');
                msg.addValue(Constants_Fields.TAGMDEntryType_i, '0');//Bid
                msg.addValue(Constants_Fields.TAGMDEntryType_i, '1');//Offer
                msg.addValue(Constants_Fields.TAGNoRelatedSym_i, '1');
                msg.addValue(Constants_Fields.TAGSymbol, symbols[i]);

                sendMessage(msg);
            }
        }

        // This method constructs the market data unsubscribe messge and send to the server.

        public void UnSubscribeMarketData()
        {
            ICollection ie = htSymbols.Keys;
            foreach (string st in ie)
            {
                Message msg = new Message(Constants_Fields.MSGMarketDataRequest);

                DateTime DtTime = DateTime.Now.ToUniversalTime();//Vm_ Fix converting to GMT time
                string tempDateTime = Message.buildDateString(ref DtTime, false, true);

                msg.addValue(Constants_Fields.TAGMDReqID, SenderCompID + tempDateTime.Substring(4)); //	Must be unique, or the ID of previous Market Data Request to disable if SubscriptionRequestType = Disable previous Snapshot  + Updates Request (2).

                msg.addValue(Constants_Fields.TAGSubscriptionRequestType, "2");//	SubcriptionRequestType indicates to the other party what type of response is expected. A snapshot request only asks for current information. A subscribe request asks for updates as the status changes. Unsubscribe will cancel any future update messages from the counter party.
                msg.addValue(Constants_Fields.TAGMarketDepth, "1");
                msg.addValue(Constants_Fields.TAGMDUpdateType_i, '0');
                msg.addValue(Constants_Fields.TAGNoMDEntryTypes_i, '2');
                msg.addValue(Constants_Fields.TAGMDEntryType_i, '0');//Bid
                msg.addValue(Constants_Fields.TAGMDEntryType_i, '1');//Offer
                msg.addValue(Constants_Fields.TAGNoRelatedSym_i, '1');
                msg.setStringFieldValue(Constants_Fields.TAGSymbol, st);

                sendMessage(msg);
            }
            logout();
        }
    }
}