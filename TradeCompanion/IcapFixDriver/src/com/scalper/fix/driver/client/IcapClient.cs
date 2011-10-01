using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using Icap.com.scalper.util;

namespace Icap.com.scalper.fix.driver.client
{
    public delegate void IcapOrderResponseEventHandler(object sender, IcapOrderStatusEventArgs args);
    public delegate void IcapMarketDataResponseEventHandler(object sender, IcapMarketDataStatusEventArgs args);
    public delegate void IcapMarketDataSubResponseHandler();
    public delegate void IcapMarketSymbolStatusHandler(String symbol);    

    public class IcapClient : DefaultClient
    {
        //This is the event that is raised whenever a response is received from Ffastfill
        public /*static*/ event IcapOrderResponseEventHandler IcapOrderResponseEvent;
        public /*static*/ event IcapMarketDataResponseEventHandler IcapMarketDataResponseEvent;
        public /*static*/ event IcapMarketDataSubResponseHandler IcapMarketDataSubResponseEvent;
        public event IcapMarketSymbolStatusHandler IcapMarketSymbolStatusEvent;        

        public double pipValue = 0;
        public Boolean marketDataSubStatus = false;
        private string mdRequestID;
        private Hashtable htSymbols = new Hashtable();
        public Boolean stApplogon = false;
        private Boolean snipetflag = false;
        private object logonStatLock = new object();
        public IcapClient(System.String hostname, int port)
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

        /*--------------------------------------------------------------------
         * This function handles the three main application message and convey message 
         * back to the main window through their respective events
         * The messages UserResponse(BF), MarketDataIncrementalRefrechment(8)&
         * MarketDataSnapshotFullRefresh(W)
         * --------------------------------------------------------------------*/
        protected override void handleMsgReceived(Icap.com.scalper.fix.Message msg)
        {            
            if (msg.MsgTypeChar == Constants_Fields.MSGMarketDataIncrementalRefreshChar)
            {               
                CreatingMarketDataEvent(msg);             
            }
            else if (msg.MsgTypeChar == Constants_Fields.MSGExecutionChar)
            {
                if (IcapOrderResponseEvent != null)
                {
                    IcapOrderStatusEventArgs ev = new IcapOrderStatusEventArgs(msg);
                    //System.Console.WriteLine(msg.toString(false));
                    AddPip(ev);
                    IcapOrderResponseEvent(this, ev);
                }
            }            
            else if (msg.MsgTypeChar == Constants_Fields.MSGMarketDataSnapshotFullRefreshChar)
            {
                string symbol = msg.getStringFieldValue(Constants_Fields.TAGSymbol_i);
                MarketData md = new MarketData();
                htSymbols.Add(symbol, md);
                if (!marketDataSubStatus)
                {
                    marketDataSubStatus = true;
                    IcapMarketDataSubResponseEvent();
                }
                string[] str = msg.getValues(Constants_Fields.TAGQuoteCondition_i);
                if (!((str[0] == "1000") ||( str[0] == "1001")))  //1000 =  is for the "No market activity" for that currency pair
                {
                    CreatingMarketDataEvent(msg);
                    IcapMarketSymbolStatusEvent(symbol);
                }
            }
        }

        /*---------------------------------------------------
         * The response of the market data subscription is intimated the main 
         * window through the IcapMarketDataStatusEventArgs event.(incremtal refreshment)
         * ---------------------------------------------------*/
        private void CreatingMarketDataEvent(Message msg)
        {
            IcapMarketDataStatusEventArgs ev = new IcapMarketDataStatusEventArgs(msg);

            if (!htSymbols.ContainsKey(ev.Symbol))
            {
                MarketData md = new MarketData();
                md.Ask = ev.OfferPrice;
                md.Bid = ev.BidPrice;
                md.Symbol = ev.Symbol;
                htSymbols.Add(ev.Symbol, md);
                log.Debug("Ignoring Bad Ticks." + ev.Symbol);
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
                    //System.Console.WriteLine(msg.toString(false));
                    IcapMarketDataResponseEvent(this, ev);
                }
            }
        }

        private void AddPip(IcapOrderStatusEventArgs ev)
        {
            //2 -- Filled, 1 -- Partially filled.
            if (ev.OrderStatus.Equals("2") || ev.OrderStatus.Equals("1"))
            {
                long tradedQuantity = ev._fixMsg.getLongValue(Constants_Fields.TAGCumQty_i);
                double price = ev._fixMsg.getDoubleFieldValue(Constants_Fields.TAGPrice);
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
                double iFullPip = double.Parse(fullPip,BasicUtilities.getCulture());
                double actualPip = iFullPip * pipPercentage / 100;
                return actualPip;
            }
            else
                return 0;
        }

        protected override void handleMsgSent(Icap.com.scalper.fix.Message msg)
        {
            //System.Console.WriteLine("Message sent:" + msg);
        }

        /*--------------------------------------------------------------------
         * Construct the session logon message & send that to server 
         * If fail to login retruns false else Ture
        --------------------------------------------------------------------*/
        public bool logon(String senderCompID, String targetCompID)
        {
            if ((this.Session != null) && (this.Session.LoggedIn))
                throw new System.SystemException("Client: Already logged in.");

            if (senderCompID != null)
                this.SenderCompID = senderCompID;

            if (targetCompID != null)
                this.TargetCompID = targetCompID;

            Message msg = new Message(Constants_Fields.MSGLogon);

            msg.setStringFieldValue(Constants_Fields.TAGSenderCompID, this.SenderCompID);
            msg.setStringFieldValue(Constants_Fields.TAGTargetCompID, "ICAP_Ai_Server");//this.TargetCompID);
            
            DateTime t = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref t, false, true);

            msg.setStringFieldValue(Constants_Fields.TAGSendingTime, tempDateTime);
            msg.setStringFieldValue(Constants_Fields.TAGEncryptMethod, "0"); 
            msg.setStringFieldValue(Constants_Fields.TAGHeartBtInt, HeartBeatInterval.ToString());
            msg.setStringFieldValue(Constants_Fields.TAGDefaultApplVerID, "7");
            
            log.Debug("Sending the LOGON message to the FIX server.");
            sendMessage(msg);
            
            if (Session.waitUntilReady(10000) == false)
            {
                logout();
                return false;
            }
            
            return true;

        }

        public String cancelOrder(System.String stockSymbol, String origClOrderId, String orderId, System.Int32 side, String text)
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                throw new System.SystemException("Client: Cannot place orders before logging in.");

            DateTime DtTime = DateTime.Now;
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
            return placeOrder(stockSymbol, orderQty, side, "0", curency, orderType);
        }

        public String placeOrder(System.String stockSymbol, System.Int32 orderQty, System.Int32 side, string orderPrice, String curency, System.Int32 orderType)
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                throw new System.SystemException("Client: Cannot place orders before logging in.");

            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String clientOrderID = SenderCompID + tempDateTime.Substring(4);
            clientOrderID = clientOrderID.ToUpper();
            clientOrderID = clientOrderID.Replace("-", "");
            clientOrderID = clientOrderID.Replace(".", "");
            clientOrderID = clientOrderID.Replace(":", "");

            return placeOrder(stockSymbol, orderQty, side, orderPrice, curency, orderType, clientOrderID);
        }

        public String placeOrder(System.String stockSymbol, System.Int32 orderQty, System.Int32 side, string orderPrice, String curency, System.Int32 orderType, String clOrderID)
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                throw new System.SystemException("Client: Cannot place orders before logging in.");

            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            Message msg = new Message(Constants_Fields.MSGOrder);
            msg.setStringFieldValue(Constants_Fields.TAGClOrdID, clOrderID);

            msg.setStringFieldValue(Constants_Fields.TAGSymbol, stockSymbol);
            msg.setStringFieldValue(Constants_Fields.TAGCFICode, "RCSXXX");
            msg.setStringFieldValue(Constants_Fields.TAGSettlmntTyp, "0");
            msg.setStringFieldValue(Constants_Fields.TAGSide, side.ToString());
            msg.setStringFieldValue(Constants_Fields.TAGOrdType, "2");
            msg.setStringFieldValue(Constants_Fields.TAGTimeInForce, orderType.ToString());

            // if (orderPrice != 0)
            {
                msg.setStringFieldValue(Constants_Fields.TAGPrice, orderPrice.ToString());
            }
            msg.setStringFieldValue(Constants_Fields.TAGOrderQty, orderQty.ToString());
            msg.setStringFieldValue(Constants_Fields.TAGTransactTime, tempDateTime);
            sendMessage(msg);
            return clOrderID;
        }

        public void  subscribeMarketData(String[] symbols)
        {
            if ((this.Session == null) || (!this.Session.LoggedIn))
                throw new System.SystemException("Client: Cannot get market data before logging in.");
            
            htSymbols = new Hashtable();

            Message msg = new Message(Constants_Fields.MSGMarketDataRequest);

            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            mdRequestID = SenderCompID + tempDateTime.Substring(4);
            msg.addValue(Constants_Fields.TAGMDReqID, SenderCompID + tempDateTime.Substring(4)); 
            //	Must be unique, or the ID of previous Market Data Request to disable if SubscriptionRequestType = Disable previous Snapshot  + Updates Request (2).
            
            msg.addValue(Constants_Fields.TAGSubscriptionRequestType, "1");
            //	SubcriptionRequestType indicates to the other party what type of response is expected. A snapshot request only asks for current information. A subscribe request asks for updates as the status changes. Unsubscribe will cancel any future update messages from the counter party.
            msg.addValue(Constants_Fields.TAGMarketDepth, "0");
            msg.addValue(Constants_Fields.TAGMDUpdateType_i, '1');          
            msg.addValue(Constants_Fields.TAGNoMDEntryTypes_i, '1');
            msg.addValue(Constants_Fields.TAGMDEntryType_i, '*');            
            msg.addValue(Constants_Fields.TAGNoRelatedSym_i, (symbols.Length - 1));
            for (int i = 0; i < symbols.Length - 1; i++)
            {
                msg.addValue(Constants_Fields.TAGSymbol, symbols[i]);
                msg.addValue(Constants_Fields.TAGCFICode_i, "RCSXXX");
                msg.addValue(Constants_Fields.TAGSettlmntTyp_i, "0");
            }
            sendMessage(msg);
            
        }

        public void TradeCaptureReportRequest(String clOrderID)
        {
            Message msg = new Message(Constants_Fields.MSGOrderStatusRequest);
            msg.addValue(Constants_Fields.TAGOrderID, "OPEN_ORDER");
            msg.addValue(Constants_Fields.TAGClOrdID, clOrderID);
            msg.addValue(Constants_Fields.TAGSymbol, "EUR/USD");
            msg.addValue(Constants_Fields.TAGSide, "1");

            sendMessage(msg);
        }

        public void PasswordReset(String oldPass, String newPass)
        {
            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String userRequestId = SenderCompID + tempDateTime.Substring(4);
            userRequestId = userRequestId.ToUpper();
            userRequestId = userRequestId.Replace("-", "");
            userRequestId = userRequestId.Replace(".", "");
            userRequestId = userRequestId.Replace(":", "");

            Message msg = new Message(Constants_Fields.MSGUserRequest);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestID, userRequestId);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestType, "3");
            msg.setStringFieldValue(Constants_Fields.TAGUserName, SenderCompID);
            msg.setStringFieldValue(Constants_Fields.TAGPassword, oldPass);
            msg.setStringFieldValue(Constants_Fields.TAGNewPassword, newPass);

            sendMessage(msg);
        }

        /*--------------------------------------------------
        * Addes the tags & values to construct the application
        * logon message.
        *--------------------------------------------------*/
        public void AppLogon(string userId , string pass)
        {
            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String userRequestId = SenderCompID + tempDateTime.Substring(4);
            userRequestId = userRequestId.ToUpper();
            userRequestId = userRequestId.Replace("-", "");
            userRequestId = userRequestId.Replace(".", "");
            userRequestId = userRequestId.Replace(":", "");

            Message msg = new Message(Constants_Fields.MSGUserRequest);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestID, userRequestId);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestType, "1");
            msg.setStringFieldValue(Constants_Fields.TAGUserName, userId);
            msg.setStringFieldValue(Constants_Fields.TAGPassword, pass);
            msg.setStringFieldValue(Constants_Fields.TAGCstmApplVerID, "1.0");
            msg.setStringFieldValue(Constants_Fields.TAGICNoUserData, "1");
            msg.setStringFieldValue(Constants_Fields.TAGICUserDataName, "PriceCheck");
            msg.addValue(Constants_Fields.TAGICUserDataValue, 'N');

            sendMessage(msg);
            try
            {                
                //lock(logonStatLock)
                //{
                //    if (System.Threading.Monitor.Wait(logonStatLock, TimeSpan.FromMilliseconds(10000)))
                //    {
                //        Console.WriteLine("reacquired before ");
                //    }
                //}
                System.Threading.Thread.Sleep(8000);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + ex.StackTrace);
            }

        }


        /*------------------------------------------------------------------------
         * This method constructs the Application logout messge and send to the server.
         * ----------------------------------------------------------------------*/
        public void AppLogout()
        {
            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String userRequestId = SenderCompID + tempDateTime.Substring(4);
            userRequestId = userRequestId.ToUpper();
            userRequestId = userRequestId.Replace("-", "");
            userRequestId = userRequestId.Replace(".", "");
            userRequestId = userRequestId.Replace(":", "");

            Message msg = new Message(Constants_Fields.MSGUserRequest);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestID, userRequestId);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestType, "2");
            msg.setStringFieldValue(Constants_Fields.TAGUserName, SenderCompID);
            sendMessage(msg);            
        }

        /*------------------------------------------------------------------------
         * This method constructs the market data unsubscribe messge and send to the server.
         * ----------------------------------------------------------------------*/
        public void UnSubscribeMarketData()
        {
            Message msg = new Message(Constants_Fields.MSGMarketDataRequest);
            msg.setStringFieldValue(Constants_Fields.TAGMDReqID, mdRequestID);
            msg.setStringFieldValue(Constants_Fields.TAGSubscriptionRequestType, "2");
            msg.setStringFieldValue(Constants_Fields.TAGMarketDepth, "0");
            msg.setStringFieldValue(Constants_Fields.TAGNoMDEntryTypes, "1");
            msg.setStringFieldValue(Constants_Fields.TAGMDEntryType, "*");
            msg.setStringFieldValue(Constants_Fields.TAGNoRelatedSym, htSymbols.Count.ToString());
            ICollection ie = htSymbols.Keys;
            foreach (string st in ie)
            {
                msg.setStringFieldValue(Constants_Fields.TAGSymbol, st);
                msg.setStringFieldValue(Constants_Fields.TAGCFICode, "RCSXXX");
                msg.setStringFieldValue(Constants_Fields.TAGSettlmntTyp, "0");
            }
            sendMessage(msg);
        }

        public void CancelDuplicateSession(string userRequestId)
        {
            Message msg = new Message(Constants_Fields.MSGUserRequest);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestID, userRequestId);
            msg.setStringFieldValue(Constants_Fields.TAGUserRequestType, "1000");
            msg.setStringFieldValue(Constants_Fields.TAGUserName, SenderCompID);
            msg.setStringFieldValue(Constants_Fields.TAGICNoUserData, "1");
            msg.setStringFieldValue(Constants_Fields.TAGICUserDataName, "AutoCancelDuplSession");
            msg.addValue(Constants_Fields.TAGICUserDataValue, 'Y');
            sendMessage(msg);
        }

    }
}
