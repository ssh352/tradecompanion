using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;

namespace EspeedDriver
{
    public delegate void ESpeedLoginResponseEventHandler(object sender, int args);
    public delegate void ESpeedOrderResponseEventHandler(object sender, EspeedOrderStatusEventArgs args);
    public delegate void ESpeedMarketDataResponseEventHandler(object sender, EspeedMarketDataStatusEventArgs args);
    
    public class EspeedClient
    {
        //Events
        public event ESpeedLoginResponseEventHandler eSpeedLoginResponseEvent;
        public event ESpeedOrderResponseEventHandler eSpeedOrderResponseEvent;
        public event ESpeedMarketDataResponseEventHandler eSpeedMarketDataResponseEvent;
   
        //CALLBACK FUNCTIONS
        public delegate void SystemCallback(int cmd, ref CFETI_CMD_STATUS cmdStatus, IntPtr cmdData, IntPtr CFETI_UD);
        public delegate void ConnectCallback(int cmd, ref CFETI_CMD_STATUS cmdStatus, IntPtr cmdData, IntPtr CFETI_UD);

        //DLL IMPORT
        [DllImport("libESPD.dll")]
        public static extern int CFETIOpenSession(string primary, string secondary, ref CFETI_IDENTIFICATION_DESC oIdent);
        [DllImport("libESPD.dll")]
        public static extern string CFETIGetLastError();
        [DllImport("libESPD.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CFETILogin(string userName, string userPassword, int systemPreferences,         SystemCallback systemCallback, Object userData);
        [DllImport("libESPD.dll")]
        public static extern string CFETIVersion();
        [DllImport("libESPD.dll")]
        public static extern int CFETISelectConnectionMode(string szConnectionMode);
        [DllImport("libESPD.dll")]
        public static extern void CFETIMessageLoop();
        [DllImport("libESPD.dll")]
        public static extern int CFETILogout(String sessId, uint systemPreferences);
        [DllImport("libESPD.dll")]
        public static extern void CFETICloseSession();
        [DllImport("libESPD.dll")]
        public static extern int CFETIConnect(String sessId, String userPassword, uint tradeSys, uint tradingSysPreferences, ConnectCallback tradingSysCallback, Object userData, ref CFETI_TRADE_SETTINGS_DESC pTradeSettings);
        [DllImport("libESPD.dll")]
        public static extern int CFETIDisconnect(string sessId, uint trdSysSessId, uint tradingSysPreferences);
        [DllImport("libESPD.dll")]
        public static extern int CFETIPostMessage(string sessId, uint trdSysSessId, uint cmd, IntPtr cmdData, uint cmdPreferences);


        //CONSTANTS
        //Return code for success 
        const int CFETI_SUCCESS = 0;
        const int CFETI_BASE_ERROR = 0x80;
        const int CFETI_SESSMGR_SPEC = CFETI_BASE_ERROR + 0xe;
        const int CFETI_CONNECT_FAIL = CFETI_BASE_ERROR + 0xf;
        //const int CFETI_DIRECTORY_READ_ERROR = 

        //Commands issued by eSpeed API 
        public const int CFETI_LOGIN_ACCEPTED = 0x1;
        public const int CFETI_LOGIN_REJECTED = 0x2;
        public const int CFETI_LOGIN_TERMINATED = 0x3;
        public const int CFETI_SET_PASSWORD_ACCEPTED = 0x43;
        public const int CFETI_SET_PASSWORD_REJECTED = 0x44;
        public const int CFETI_STATUS = 0x1d;
        public const int CFETI_LOGOUT_ACCEPTED = 0x1e;
        public const int CFETI_LOGOUT_REJECTED = 0x1f;
        public const int CFETI_CONNECTION_ACCEPTED = 0x18;
        public const int CFETI_CONNECTION_REJECTED = 0x19;
        public const int CFETI_CONNECTION_TERMINATED = 0x1a;
        public const int CFETI_DISCONNECT_ACCEPTED = 0x1b;
        public const int CFETI_DISCONNECT_REJECTED = 0x1c;
        public const int CFETI_REFRESH_COMPLETE = 0x24;
        public const int CFETI_ORDER_QUEUED = 0xa;
        public const int CFETI_ORDER_REJECTED = 0xb;
        public const int CFETI_ORDER_EXECUTED = 0xc;
        public const int CFETI_ORDER_CANCELLED = 0xd;
        public const int CFETI_TRADE_CONFIRM = 0xe;
        public const int CFETI_ORDER_EXECUTING = 0xb7; /**< An execution will be delivered for this order at this or a better price for the size indicated in the SIZE_DONE field */
        public const int CFETI_SUBSCRIBE_ACCEPTED = 0x13;
        public const int CFETI_SUBSCRIBE_REJECTED = 0x14;
        public const int CFETI_UNSUBSCRIBE_ACCEPTED = 0x15;
        public const int CFETI_UNSUBSCRIBE_REJECTED = 0x16;
        public const int CFETI_SUBSCRIBE_STATUS = 0x20;
        public const int CFETI_MKT_CREATED = 0x23;
        public const int CFETI_UPDATE = 0x17;        
        public const int CFETI_SESSION_LOST = 0x12;
        public const int CFETI_SESSION_RESTORED = 0x13;

        //eSpeed API command status and error codes
        public const int CFETI_INSTRUMENT_LOST = 0x14;
        public const int CFETI_INSTRUMENT_RESTORED = 0x15;
        public const int CFETI_MARKET_FEED_DOWN = 0x16;
        public const int CFETI_MARKET_FEED_RESTORED = 0x17;

        //Commands issued by user
        public const int CFETC_SUBMIT_ORDER = 0x3;
        public const int CFETC_SUBSCRIBE = 0x6;
        public const int CFETC_UNSUBSCRIBE = 0x7;
        public const int CFETC_DD_REGISTER = 0x1c;
        public const int CFETC_DD_DEREGISTER = 0x1d;

        //Market and Order indicators
        public const byte CFETI_ORDER_BUY = 0x4;
        public const byte CFETI_ORDER_TAK = CFETI_ORDER_BUY;
        public const byte CFETI_ORDER_SELL = 0x3;
        public const byte CFETI_ORDER_HIT = CFETI_ORDER_SELL;
        public const uint CFETI_TRADE_AT_MARKET_PRICE = 0x40;

        //Enumerated market retain rule
        public const int CFETI_RETAIN_DEFAULT = 0x0;

        //Enumerated market priority follow rule 
        public const int CFETI_PRIORITY_FOLLOW_DEFAULT = 0x0;

        //Trade control flags
        public const int CFETI_DIRECT_DEALING_ENABLED = 0x200;

        /*
        * Values for order info type component of CFETI_ORDER_DESC structure 
        */
        public const int CFETI_ORDERINFO_NOT_SPECIFIED = 0;
        public const int CFETI_ORDERINFO_BLOCK_TRADE = 1; /**< Block trade support */
        public const int CFETI_ORDERINFO_EXTENDED_PROPERTIES = 4;
        public const int CFETI_ORDERINFO_ENERGY_TRADE = 5;
        public const int CFETI_ORDERINFO_FX_TRADE = 6;
        public const int CFETI_ORDERINFO_TSWAP = 8;
        public const int CFETI_ORDERINFO_PV01_LOCK = CFETI_ORDERINFO_TSWAP; /**< @Indicates that the orderInfo pointer is of type CFETI_PV01_LOCK_DESC */
        public const int CFETI_ORDERINFO_MMTS = 9;
        public const int CFETI_ORDERINFO_FX_OPTION_TRADE = 10;
        public const int CFETI_ORDERINFO_CANTOR_REPO = 11;
        public const int CFETI_ORDERINFO_IRD_TRADE = 12;
        public const int CFETI_ORDERINFO_ESPD_REPO_TRADE = 13;
        public const int CFETI_ORDERINFO_ESPD_REPO = 14;
        public const int CFETI_ORDERINFO_CDS_TRADE = 15;
        public const int CFETI_ORDERINFO_CDS_INDEX_OPTION_TRADE = 16;
        public const int CFETI_ORDERINFO_CDS_INDEX_TRANCHE_TRADE = 17;

        /*
        * eSpeed Market Data Fields. Valid field identifiers are in the range 0x0001
        * to 0xff00 (1 to 65280). 
        */
        const int CFETF_BID_1 = 0x1;
        const int CFETF_ASK_1 = 0x10;

        /*
        * Field data types 
        */
        const int CFETI_FIELDTYPE_INT8 = 0x1;
        const int CFETI_FIELDTYPE_BYTE = 0x2;
        const int CFETI_FIELDTYPE_INT16 = 0x3;
        const int CFETI_FIELDTYPE_UINT16 = 0x4;
        const int CFETI_FIELDTYPE_INT32 = 0x5;
        const int CFETI_FIELDTYPE_UINT32 = 0x6;
        const int CFETI_FIELDTYPE_DATETIME = 0x7;
        const int CFETI_FIELDTYPE_STRING = 0x8;
        const int CFETI_FIELDTYPE_BUFFER = 0x9;
        const int CFETI_FIELDTYPE_DECIMAL = 0xa;
        const int CFETI_FIELDTYPE_BYTESTREAM = 0xf;
        const int CFETI_FIELDTYPE_EMPTY = 0x10;

        /*
         * Market and Order preferences - bitmask 
         */
        const uint CFETI_ONLY_AT_BEST = 0x80000000; /**< Market or order shall work only at the best price available */
        const uint CFETI_MARKET_GOOD_TILL_CANCEL = 0x8000;


        String currentSession = "";
        uint currentTradingSession = 0;
        
        AutoResetEvent autoEvent;
        SystemCallback mySCB;
        ConnectCallback myCCB;
        CFETI_IDENTIFICATION_DESC oIdent;
        CFETI_TRADE_SETTINGS_DESC tradeSettings;
        uint currentTradingSystem = 0;
        String hostName = "";
        int port = -1;
        String SenderCompID;
        String TargetCompID;
        Thread trd;
        int sequentialOrderId = 0;
        public Hashtable htOrders;
        private String[] espeedSymbols;
        private bool isMDSubscribed = false;

        Hashtable htMarketData = new Hashtable();

        public EspeedClient(System.String hostname, int port)
        {
            this.hostName = hostname;
            this.port = port;
            autoEvent = new AutoResetEvent(false);
            htOrders = new Hashtable();
            Util.WriteDebugLogDebug("********************************");
            Util.WriteDebugLogDebug("Initializing Espeed Client");
            Util.WriteDebugLogDebug("Host : " + hostname);
            Util.WriteDebugLogDebug("Port : " + port);

            //Start MessageLoop
            Util.WriteDebugLogDebug("Starting Message Loop");
            trd = new Thread(new ThreadStart(CFETIMessageLoop));
            trd.IsBackground = true;
            trd.Start();
        }

        public bool Logon(String sender, String target)
        {
            if (currentSession == "")
            {                
                Util.WriteDebugLogDebug("Login to Espeed");
                if (sender != null)
                    this.SenderCompID = sender;

                if (target != null)
                    this.TargetCompID = target;

                //Open Session
                Util.WriteDebugLogDebug("Open Session");
                oIdent = new CFETI_IDENTIFICATION_DESC();
                oIdent.Company = "espeed";
                oIdent.Application = "simple";
                oIdent.Version = "1.0.0";
                String primaryID = hostName + ":" + port;
                String secondaryID = hostName + ":" + port;
                //CFETICloseSession();
              
                //fxprov11@training.espeed.co.uk tradingSession=1
                //CFETIDisconnect("fxprov12@training.espeed.co.uk",1, 0);
                //CFETILogout("fxprov12@training.espeed.co.uk", 0);
                //CFETICloseSession();

                int result = CFETIOpenSession(primaryID, secondaryID, ref oIdent);
                switch (result)
                {
                    case CFETI_SUCCESS:
                        Util.WriteDebugLogDebug("CFETIOpenSession successsful");
                        break;
                    case CFETI_SESSMGR_SPEC:
                        eSpeedLoginResponseEvent(this, CFETI_LOGIN_TERMINATED);
                        Util.WriteDebugLogDebug("Invalid session manager name specification in call to CFETIOpenSession.");
                        break;
                    case CFETI_CONNECT_FAIL:
                        eSpeedLoginResponseEvent(this, CFETI_LOGIN_TERMINATED);
                        Util.WriteDebugLogDebug("Connection to the session manager(s) specified in the primary and secondary session manager names was not successful.");
                        break;
                    default:
                        eSpeedLoginResponseEvent(this, CFETI_LOGIN_TERMINATED);
                        Util.WriteDebugLogDebug("CFETIOpenSession Failed : Error : " + result + " : " + CFETIGetLastError());
                        break;
                }
                
                //Login
                Util.WriteDebugLogDebug("Login as username = " + sender + " password = " + target);
                String username = sender;
                String password = target;
                mySCB = new SystemCallback(MySystemCallback);
                
                result = CFETILogin(username, password, 0, mySCB, 0);
                switch (result)
                {
                    case CFETI_SUCCESS:
                        Util.WriteDebugLogDebug("Login Successful");
                        break;
                    default:
                        Util.WriteDebugLogDebug("CFETILogin Failed : Error : " + result + " : " + CFETIGetLastError());
                        return false;
                }

                Util.WriteDebugLogDebug("Wait till we get the trading system");
                autoEvent.WaitOne(5000, true);

                //Connect to trading session
                tradeSettings = new CFETI_TRADE_SETTINGS_DESC();
                tradeSettings.retainRule = CFETI_RETAIN_DEFAULT;
                tradeSettings.priorityFollowRule = CFETI_PRIORITY_FOLLOW_DEFAULT;
                uint pref = 0;
                myCCB = new ConnectCallback(MyConnectCallback);
                result = CFETIConnect(currentSession, password, currentTradingSystem, pref, myCCB, 0, ref tradeSettings);                
                switch (result)
                {
                    case CFETI_SUCCESS:
                        Util.WriteDebugLogDebug("Successfully connected to trading system : " + currentTradingSystem);
                        return true;
                    default:
                        Util.WriteDebugLogDebug("CFETIConnect to " + currentTradingSystem + " failed : Error " + result + " Reason : " + CFETIGetLastError());
                        return false;
                }
            }
            return false;
        }

        public void logout()
        {
            if (currentSession != "" && currentTradingSession != 0)
            {
                //Disconnect from the Trading system
                int result = CFETIDisconnect(currentSession, currentTradingSession, 0);
                switch (result)
                {
                    case CFETI_SUCCESS:
                        Util.WriteDebugLogDebug("Successfully disconnected, session=" + currentSession + " tradingSession=" + currentTradingSession);
                        break;
                    default:
                        Util.WriteDebugLogDebug("CFETIDisconnect Failed : Error=" + result + " Reason=" + CFETIGetLastError());
                        break;
                }

                //Logout
                result = CFETILogout(currentSession, 0);
                switch (result)
                {
                    case CFETI_SUCCESS:
                        Util.WriteDebugLogDebug("Successfully logged out from session=" + currentSession);
                        break;
                    default:
                        Util.WriteDebugLogDebug("CFETILogout Failed : Error=" + result + " : Reason=" + CFETIGetLastError());
                        break;
                }

                //Close Session
                Util.WriteDebugLogDebug("Close Session");
                CFETICloseSession();
            }
        }


        
        public String placeOrder(String stockSymbol, System.Int32 orderQty, System.Int32 side, System.Double orderPrice, String curency, System.Int32 orderType)
        {

            uint command = CFETC_SUBMIT_ORDER;
            byte flag = (byte)0;
            if (side == 1)
                flag = CFETI_ORDER_TAK;
            else if (side == 2)
                flag = CFETI_ORDER_HIT;

            DateTime DtTime = DateTime.Now;
            string tempDateTime = Message.buildDateString(ref DtTime, false, true);
            String clientOrderID = SenderCompID + tempDateTime.Substring(4);
            clientOrderID = clientOrderID.ToUpper();
            clientOrderID = clientOrderID.Replace("-", "");
            clientOrderID = clientOrderID.Replace(".", "");
            clientOrderID = clientOrderID.Replace(":", "");

            CFETI_ORDER_DESC order = new CFETI_ORDER_DESC();
            order.tradeInstrument = stockSymbol;
            order.price = orderPrice; //will be executed at the market ask/bid price
            order.size = orderQty;
            order.indicator = flag;
            order.id = 0;
            order.requestId = 0;
            order.tradeId = "";
            order.userData = IntPtr.Zero;
            order.userDataSize = 0;
            order.appUserData = IntPtr.Zero;
            order.appUserDataSize = 0;
            order.shortCode = "";
            order.toPrice = 0;

            sequentialOrderId++;
            UserData userData = new UserData(clientOrderID, SenderCompID);
            htOrders.Add(sequentialOrderId.ToString(), userData);
            
            try{
                order.userDataSize = 8; //Maximum allowed is 8 bytes
                order.userData = Marshal.StringToCoTaskMemAnsi(sequentialOrderId.ToString());
            }
            catch(Exception ex)
            {
                Util.WriteDebugLogError("Error in placeOrder --" + ex.Message + ex.StackTrace); //Console.WriteLine(ex.StackTrace);
            }

            uint pref = 0;
            /*if(orderType == 3)
                pref = CFETI_ONLY_AT_BEST;
            else if(orderType == 1)
                pref = CFETI_MARKET_GOOD_TILL_CANCEL;*/
            pref = CFETI_TRADE_AT_MARKET_PRICE;
            

            Util.WriteDebugLogDebug("SEND>>> Place Order : clientOrderID=" + clientOrderID + "symbol=" + stockSymbol + ",orderQty=" + orderQty + ",side=" + side + ",orderPrice=" + orderPrice + ",curreny=" + curency + ",orderType=" + orderType);
            int size = Marshal.SizeOf(typeof(CFETI_ORDER_DESC));
            IntPtr orderPtr = IntPtr.Zero;
            orderPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(order, orderPtr, false);
            int result = CFETIPostMessage(currentSession, currentTradingSession, command, orderPtr, pref);
            Marshal.FreeHGlobal(orderPtr);
            switch (result)
            {
                case CFETI_SUCCESS:
                    Util.WriteDebugLogDebug("Successfully order has been sent");
                    break;
                default:
                    Util.WriteDebugLogDebug("CFETIPostMessage Failed : Error=" + result + " : Reason=" + CFETIGetLastError());
                    break;
            }
            return clientOrderID;
        }

        public void SubscribeMarketData(String[] symbols)
        {
            isMDSubscribed = true;
            espeedSymbols = symbols;
            for (int i = 0; i < espeedSymbols.Length - 1; i++)
            {
                if (espeedSymbols[i] != null)
                {
                    uint command = CFETC_SUBSCRIBE;
                    int result = CFETIPostMessage(currentSession, currentTradingSession, command, Marshal.StringToHGlobalAnsi(espeedSymbols[i]), 0);
                    switch (result)
                    {
                        case CFETI_SUCCESS:
                            Util.WriteDebugLogDebug("Successfully subscribed symbol " + espeedSymbols[i]);
                            break;
                        default:
                            Util.WriteDebugLogDebug("Subscribe failed for symbol=" + espeedSymbols[i] + ",Reason=" + CFETIGetLastError());
                            break;
                    }
                }
            }
        }

        public void UnSubscribeMarketData()
        {
            isMDSubscribed = false;
            if (espeedSymbols != null)
            {
                for (int i = 0; i < espeedSymbols.Length - 1; i++)
                {
                    if (espeedSymbols[i] != null)
                    {
                        uint cmd = CFETC_UNSUBSCRIBE;
                        int result = CFETIPostMessage(currentSession, currentTradingSession, cmd, Marshal.StringToHGlobalAnsi(espeedSymbols[i]), 0);
                        switch (result)
                        {
                            case CFETI_SUCCESS:
                                Util.WriteDebugLogDebug("Successfully unsubscribed symbol " + espeedSymbols[i]);
                                break;
                            default:
                                Util.WriteDebugLogDebug("Unsubscribe failed for symbol=" + espeedSymbols[i] + ",Reason=" + CFETIGetLastError());
                                break;
                        }
                    }
                }
            }
        }

        public System.String getSender()
        {
            return SenderCompID;
        }

        public void setSender(String value)
        {
            this.SenderCompID = value;
        }

        public System.String getTarget()
        {
            return TargetCompID;
        }

        public void setTarget(String value)
        {
            this.TargetCompID = value;
        }

        //Print message
        public static void Print(String msg)
        {
            System.Console.WriteLine(msg);
        }

        //Equivalent to SystemCallback
        public void MySystemCallback(int cmd, ref CFETI_CMD_STATUS cmdStatus, IntPtr cmdData, IntPtr CFETI_UD)
        {
            if (cmdStatus.statusText != null && cmdStatus.statusText != "")
                Util.WriteDebugLogDebug("RECV<<< Systemcallback status : " + cmdStatus.statusText.ToUpper());
            switch (cmd)
            {
                case CFETI_LOGIN_ACCEPTED:
                    {
                        // Point in unmanaged memory.
                        CFETI_LOGIN_INFO l = (CFETI_LOGIN_INFO)Marshal.PtrToStructure(cmdData, typeof(CFETI_LOGIN_INFO));

                        Util.WriteDebugLogDebug("RECV<<< Login accepted - session id " + l.sessionId + " connection mode " + l.szConnectionMode);
                        currentSession = l.sessionId;

                        int size = Marshal.SizeOf(l.ts);
                        int offset = 0;
                        IntPtr temp = IntPtr.Zero;
                        do
                        {
                            temp = Marshal.AllocCoTaskMem(size);
                            temp = Marshal.ReadIntPtr(l.ts, offset);
                            if ((int)temp == 0)
                                break;
                            CFETI_TRADING_SYS_DESC tmpTs = (CFETI_TRADING_SYS_DESC)Marshal.PtrToStructure(temp, typeof(CFETI_TRADING_SYS_DESC));
                            Util.WriteDebugLogDebug("RECV<<< Trading System : " + tmpTs.tsId + " " + tmpTs.tsDescription);
                            if (tmpTs.tsDescription == "BGC FXPRO")
                                currentTradingSystem = tmpTs.tsId;
                            Marshal.FreeCoTaskMem(temp);
                            offset += size;
                        } while ((int)temp != 0);
                        Marshal.FreeCoTaskMem(temp);
                        autoEvent.Set();                        
                    }
                    break;

                case CFETI_LOGIN_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< Login rejected : user " + (String)Marshal.PtrToStringAnsi(cmdData));
                    eSpeedLoginResponseEvent(this, CFETI_LOGIN_REJECTED);
                    break;
                case CFETI_LOGIN_TERMINATED:
                    Util.WriteDebugLogDebug("RECV<<< Login terminated : session " + (String)Marshal.PtrToStringAnsi(cmdData));
                    eSpeedLoginResponseEvent(this, CFETI_LOGIN_TERMINATED);
                    break;
                case CFETI_STATUS:
                    currentSession = (String)Marshal.PtrToStringAnsi(cmdData);
                    Util.WriteDebugLogDebug("RECV<<< Status " + cmdStatus.cmdStatus);
                    switch (cmdStatus.cmdStatus)
                    {
                        case CFETI_SESSION_LOST:
                            eSpeedLoginResponseEvent(this, CFETI_SESSION_LOST);
                            break;
                        case CFETI_SESSION_RESTORED:
                            eSpeedLoginResponseEvent(this, CFETI_SESSION_RESTORED);
                            break;
                    }
                    break;
                case CFETI_LOGOUT_ACCEPTED:
                    Util.WriteDebugLogDebug("RECV<<< Logout accepted : session " + (String)Marshal.PtrToStringAnsi(cmdData));
                    eSpeedLoginResponseEvent(this, CFETI_LOGOUT_ACCEPTED);
                    break;
                case CFETI_LOGOUT_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< Logout rejected : session " + (String)Marshal.PtrToStringAnsi(cmdData));
                    break;
                case CFETI_SET_PASSWORD_ACCEPTED:
                    Util.WriteDebugLogDebug("RECV<<< Set password accepted: session " + (String)Marshal.PtrToStringAnsi(cmdData));
                    break;
                case CFETI_SET_PASSWORD_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< Set password rejected: session " + (String)Marshal.PtrToStringAnsi(cmdData));
                    break;
                
                default:
                    Util.WriteDebugLogDebug("RECV<<< Default command" + cmd);
                    break;
            }
        }

        
        //Equivalent to ConnectCallback
        public void MyConnectCallback(int cmd, ref CFETI_CMD_STATUS cmdStatus, IntPtr cmdData, IntPtr CFETI_UD)
        {
            if (cmd != CFETI_STATUS && (cmdStatus.statusText != null && cmdStatus.statusText != ""))
                Util.WriteDebugLogDebug("RECV<<< ConnectCallback status : " + cmdStatus.statusText.ToUpper());
            CFETI_CONNECT_INFO c = new CFETI_CONNECT_INFO();
            CFETI_ORDER_DESC orderData = new CFETI_ORDER_DESC();
            EspeedOrderStatusEventArgs orderArgs = new EspeedOrderStatusEventArgs();
            String key = "";
            UserData ud = new UserData();
            switch (cmd)
            {
                case CFETI_CONNECTION_ACCEPTED:
                    {
                        try
                        {
                            c = (CFETI_CONNECT_INFO)Marshal.PtrToStructure(cmdData, typeof(CFETI_CONNECT_INFO));

                            Util.WriteDebugLogDebug("RECV<<< Connection Accepted - trading session id " + c.sessionId);
                            currentTradingSession = c.sessionId;

                            //TODO Should not post msg for Direct Trading bit it does here
                            //If this connection is Direct Dealing enabled, send an Direct Dealing Registration Message
                            /*if (c.tradeFlags > 0) 
                            {
                                int result = CFETIPostMessage(currentSession, currentTradingSession, CFETC_DD_REGISTER, 0, 0);
                                if (result != CFETI_SUCCESS)
                                {
                                    Print("\tDirect Dealing Registration Failed");
                                }
                            }*/
                            eSpeedLoginResponseEvent(this, CFETI_CONNECTION_ACCEPTED);
                        }
                        catch (Exception ex)
                        {
                            Util.WriteDebugLogError("Error in MyConnectCallback --" + ex.Message + ex.StackTrace); // System.Console.WriteLine(ex.Message);
                        }
                    }
                    break;
                case CFETI_CONNECTION_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< Connection Rejected for session " + (String)Marshal.PtrToStringAnsi(cmdData));
                    break;
                case CFETI_CONNECTION_TERMINATED:
                    Util.WriteDebugLogDebug("RECV<<< FETI_CONNECTION_TERMINATED");
                    //c = (CFETI_CONNECT_INFO)cmdData;
                    //Util.WriteDebugLogDebug("RECV>>> Connection terminated - trading session id " +  c.tradingSystem.tsId + " : " + c.sessionId);
                    break;
                case CFETI_DISCONNECT_ACCEPTED:
                    Util.WriteDebugLogDebug("RECV<<< CFETI_DISCONNECT_ACCEPTED");
                    //c = (CFETI_CONNECT_INFO)Marshal.PtrToStructure(cmdData, typeof(CFETI_CONNECT_INFO));
                    //Util.WriteDebugLogDebug("RECV>>> Disconnect accepted - trading session id " + c.tradingSystem.tsId + " : " + c.sessionId);
                    //eSpeedLoginResponseEvent(this, CFETI_DISCONNECT_ACCEPTED);
                    break;
                case CFETI_DISCONNECT_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< CFETI_DISCONNECT_REJECTED");
                    //c = (CFETI_CONNECT_INFO)cmdData;
                    //Util.WriteDebugLogDebug("RECV>>> Disconnect rejected - trading session id " + c.tradingSystem.tsId + " : " + c.sessionId);
                    break;
                case CFETI_REFRESH_COMPLETE:
                    //Util.WriteDebugLogDebug("CFETI_REFRESH_COMPLETE");
                    //TODO Need proper check
                    if (cmdStatus.cmdStatus == 0)
                        Util.WriteDebugLogDebug("RECV<<< Refresh Complete -> " + cmdStatus.cmdStatus + " : " + cmdStatus.statusText);
                    else
                        Util.WriteDebugLogDebug("RECV<<< Refresh Complete");
                    if (isMDSubscribed)
                        SubscribeMarketData(espeedSymbols);
                    break;
                case CFETI_ORDER_QUEUED:
                    Util.WriteDebugLogDebug("RECV<<< Order queued");
                    orderData = (CFETI_ORDER_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_ORDER_DESC));
                    key = (String)Marshal.PtrToStringAnsi(orderData.userData);
                    ud = (UserData)htOrders[key];
                    orderArgs.updateData(ud, orderData, CFETI_ORDER_QUEUED, cmdStatus.statusText);
                    eSpeedOrderResponseEvent(this, orderArgs);
                    OutputOrder(orderData);
                    break;
                case CFETI_ORDER_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< Order rejected");
                    orderData = (CFETI_ORDER_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_ORDER_DESC));
                    key = (String)Marshal.PtrToStringAnsi(orderData.userData);
                    ud = (UserData)htOrders[key];
                    orderArgs.updateData(ud, orderData, CFETI_ORDER_REJECTED, cmdStatus.statusText);
                    eSpeedOrderResponseEvent(this, orderArgs);
                    OutputOrder(orderData);
                    break;
                case CFETI_ORDER_EXECUTED:
                    Util.WriteDebugLogDebug("RECV<<< Order executed");
                    orderData = (CFETI_ORDER_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_ORDER_DESC));
                    key = (String)Marshal.PtrToStringAnsi(orderData.userData);
                    ud = (UserData)htOrders[key];
                    orderArgs.updateData(ud, orderData, CFETI_ORDER_EXECUTED, cmdStatus.statusText);
                    eSpeedOrderResponseEvent(this, orderArgs);
                    OutputOrder(orderData);
                    break;
                case CFETI_ORDER_EXECUTING:
                    Util.WriteDebugLogDebug("RECV<<< Order executing");
                    orderData = (CFETI_ORDER_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_ORDER_DESC));
                    key = (String)Marshal.PtrToStringAnsi(orderData.userData);
                    ud = (UserData)htOrders[key];
                    orderArgs.updateData(ud, orderData, CFETI_ORDER_EXECUTING, cmdStatus.statusText);
                    eSpeedOrderResponseEvent(this, orderArgs);
                    OutputOrder(orderData);
                    break;
                case CFETI_ORDER_CANCELLED:
                    Util.WriteDebugLogDebug("RECV<<< Order Cancelled");
                    orderData = (CFETI_ORDER_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_ORDER_DESC));
                    key = (String)Marshal.PtrToStringAnsi(orderData.userData);
                    ud = (UserData)htOrders[key];
                    orderArgs.updateData(ud, orderData, CFETI_ORDER_CANCELLED, cmdStatus.statusText);
                    eSpeedOrderResponseEvent(this, orderArgs);
                    OutputOrder(orderData);
                    break;
                case CFETI_TRADE_CONFIRM:
                    Util.WriteDebugLogDebug("RECV<<< Trade Confirm");
                    orderData = (CFETI_ORDER_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_ORDER_DESC));
                    /*key = (String)Marshal.PtrToStringAnsi(orderData.userData);
                    ud = (UserData)htOrders[key];
                    orderArgs.updateData(ud, orderData, CFETI_TRADE_CONFIRM, cmdStatus.statusText);
                    eSpeedOrderResponseEvent(this, orderArgs);*/
                    OutputOrder(orderData);
                    break;
                case CFETI_SUBSCRIBE_ACCEPTED:
                    CFETI_INSTRUMENT_DATA_DESC instData = (CFETI_INSTRUMENT_DATA_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_INSTRUMENT_DATA_DESC));
                    Util.WriteDebugLogDebug("RECV<<< Subscribe Accepted : " + instData.instName);
                    GetInstrumentData(instData);
                    break;
                case CFETI_UPDATE:
                    CFETI_INSTRUMENT_DATA_DESC instData2 = (CFETI_INSTRUMENT_DATA_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_INSTRUMENT_DATA_DESC));
                    GetInstrumentData(instData2);
                    break;
                case CFETI_SUBSCRIBE_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< Subscribe Rejected : " + (String)Marshal.PtrToStringAnsi(cmdData));
                    break;
                case CFETI_SUBSCRIBE_STATUS:
                    switch (cmdStatus.cmdStatus)
                    {
                        case CFETI_INSTRUMENT_LOST:
                            Util.WriteDebugLogDebug("RECV<<< Subscribe Status - Instrument Lost : " + (String)Marshal.PtrToStringAnsi(cmdData));
                            break;
                        case CFETI_INSTRUMENT_RESTORED:
                            Util.WriteDebugLogDebug("RECV<<< Subscribe Status - Instrument Restored : " + (String)Marshal.PtrToStringAnsi(cmdData));
                            break;
                        case CFETI_MARKET_FEED_DOWN:
                            Util.WriteDebugLogDebug("RECV<<< Subscribe Status - Market Data Feed Down (Trading system)");
                            //Output((uint)cmdData);
                            break;
                        case CFETI_MARKET_FEED_RESTORED:
                            Util.WriteDebugLogDebug("RECV<<< Subscribe Status - Market Data Feed Restored (Trading system)");
                            //Output((uint)cmdData);
                            break;
                        default:
                            Util.WriteDebugLogDebug("RECV<<< Subscribe Status " + cmdStatus.cmdStatus);
                            break;
                    }
                    break;
                case CFETI_UNSUBSCRIBE_ACCEPTED:
                    Util.WriteDebugLogDebug("RECV<<< Unsubscribe Accepted : " + (String)Marshal.PtrToStringAnsi(cmdData));
                    break;
                case CFETI_UNSUBSCRIBE_REJECTED:
                    Util.WriteDebugLogDebug("RECV<<< Unsubscribe Rejected : " + (String)Marshal.PtrToStringAnsi(cmdData));
                    break;
                case CFETI_MKT_CREATED:
                    Util.WriteDebugLogDebug("RECV<<< Market Created : No Action taken");
                    //CFETI_ORDER_DESC orderData = (CFETI_ORDER_DESC)Marshal.PtrToStructure(cmdData, typeof(CFETI_ORDER_DESC));
                    //OutputMarket((CFETI_MARKET)cmdData);
                    break;
                case CFETI_STATUS:
                    switch(cmdStatus.cmdStatus)
                    {
                        case CFETI_SESSION_LOST:
                            //eSpeedLoginResponseEvent(this, CFETI_SESSION_LOST);            
                        break;
                        case CFETI_SESSION_RESTORED:
                            //eSpeedLoginResponseEvent(this, CFETI_SESSION_RESTORED);
                            break;
                    }
                    break;
                default:
                    Util.WriteDebugLogDebug("RECV<<< default command : " + cmd);
                    break;
            }
        }


        public void GetInstrumentData(CFETI_INSTRUMENT_DATA_DESC instData)
        {
            
            CFETI_FIELD_DESC[] manArray = new CFETI_FIELD_DESC[instData.numFields];
            IntPtr current = instData.fieldTable; //outArray;
            MarketData mdata;
            if (!htMarketData.ContainsKey(instData.instName))
            {
                mdata = new MarketData();
                mdata.Symbol = instData.instName;
                htMarketData.Add(instData.instName,mdata);
            }
            else
            {
                mdata = (MarketData)htMarketData[instData.instName];
            }

            //double askPrice = 0;
            //double bidPrice = 0;
            for (int i = 0; i < instData.numFields; i++)
            {
                manArray[i] = new CFETI_FIELD_DESC();
                Marshal.PtrToStructure(current, manArray[i]);

                Marshal.DestroyStructure(current, typeof(CFETI_FIELD_DESC));
                current = (IntPtr)((int)current + Marshal.SizeOf(manArray[i]));
                switch (manArray[i].fieldType)
                {
                    case CFETI_FIELDTYPE_DECIMAL:
                        {
                            if (manArray[i].fieldId == CFETF_BID_1)
                            {
                                if (manArray[i].fieldValue.Cdecimal != 0)
                                    mdata.Bid = manArray[i].fieldValue.Cdecimal.ToString();

                            }
                            else if (manArray[i].fieldId == CFETF_ASK_1)
                            {
                                if (manArray[i].fieldValue.Cdecimal != 0)
                                    mdata.Ask = manArray[i].fieldValue.Cdecimal.ToString();
                            }
                        }
                        break;
                }

                //if (bidPrice != 0 && askPrice != 0)
                //{
                //    //Util.WriteDebugLogDebug(">>>>>MarketData : " + instData.instName + " " + bidPrice.ToString()+ " " + askPrice.ToString());
                //    break;
                //}
            }

            //if (bidPrice != 0 || askPrice != 0)
            //{
                Util.WriteDebugLogDebug(">>>>>MarketData : " + mdata.Symbol + " " + mdata.Bid + " " + mdata.Ask);
                DateTime now = new DateTime();
                EspeedMarketDataStatusEventArgs mdArgs = new EspeedMarketDataStatusEventArgs(mdata.Symbol, mdata.Bid, mdata.Ask, now.ToString());
                eSpeedMarketDataResponseEvent(this, mdArgs);
            //}
        }

        // Print Order Data
        public void OutputOrder(CFETI_ORDER_DESC order)
        {
            Util.WriteDebugLogDebug("=====PRINT ORDER DETAILS=====");
            //char buf1[32], buf2[32];
            Util.WriteDebugLogDebug("Instrument\t [" + order.tradeInstrument + "]");
            Util.WriteDebugLogDebug("Trading system\t [" + order.tsId + "]");
            Util.WriteDebugLogDebug("Price\t [" + order.price + "]");
            Util.WriteDebugLogDebug("Size\t [" + order.size + "]");
            Util.WriteDebugLogDebug("Pref\t [" + order.preferences + " : " + order.preferences2 + "]");
            //Print("Side\t [" + (order.indicator == CFETI_ORDER_HIT || order.indicator == CFETI_ORDER_SELL)  ? "Sell" : (order.indicator == CFETI_ORDER_BUY ? "Buy" : "???");
            Util.WriteDebugLogDebug("Id, Req Id\t [" + order.id + " , " + order.requestId + "]");

            if (order.yield != 0.0)
                Util.WriteDebugLogDebug(" Yield\t\t" + order.yield);
            //if (order.preferences2 & CFETI_USE_RESERVE_SIZE)
            //  Output("Reserve Size Min [%f] Max [%f] Initial [%f]", order->reserveMinSize, order->reserveMaxSize, order->reserveInitialSize);
            if (order.tradeComments != null)
                Util.WriteDebugLogDebug("Trade comments\t[" + order.tradeComments + "]");
            if (order.toPrice != 0)
                Util.WriteDebugLogDebug("To Price\t\t[" + order.toPrice + "]");
            if (order.tradeId != null && order.tradeId != "")
            {
                Util.WriteDebugLogDebug("Trade Id\t[" + order.tradeId + "]");
                Util.WriteDebugLogDebug("Trade Size\t[" + order.tradeSize + "]");
                Util.WriteDebugLogDebug("Time\t[" + order.tradeTime + "]");
            }
            if (order.userName != null)
                Util.WriteDebugLogDebug("Trader\t\t[" + order.userName + "]");
            if (order.userDataSize > 0)
            {
                Util.WriteDebugLogDebug("User Data\t[" + order.userDataSize + " : " + order.userData + "]");
            }
            if (order.appUserDataSize > 0)
                Util.WriteDebugLogDebug("App User Data\t[" + order.appUserDataSize + " : " + order.appUserData + "]");
            if (order.shortCode != null && order.shortCode != "")
                Util.WriteDebugLogDebug("Short code\t\t[" + order.shortCode + "]");
            if (order.counterpartyName != null)
                Util.WriteDebugLogDebug("Counterparty name\t[" + order.counterpartyName + "]");
            if (order.settlementMethod != null)
                Util.WriteDebugLogDebug("Settlement Method\t[" + order.settlementMethod + "]");
            if (order.brokerage > 0)
                Util.WriteDebugLogDebug("Brokerage\t\t[" + order.brokerage + "]");
            if (order.instProperties > 0)
                Util.WriteDebugLogDebug("Properties\t\t[" + order.instProperties + "]");

            /*sprintf(buf1, "%f", order->tradePrice);
            if (order->tradeSettlement != 0)
                sprintf(buf2, "%s", ctime(&order->tradeSettlement));
            else
                buf2[0] = '\0';
            
            switch(order->tradeSide)
            {
            case CFETI_TRADE_PASSIVE:
                Output("Trade\t\t[PASSIVE, %s %.24s]", buf1, buf2);
                break;
            case CFETI_TRADE_ACTIVE:
                Output("Trade\t\t[ACTIVE, %s %.24s]", buf1, buf2);
                break;
            default:
                break;
            }*/

            /*if (order.pPIBenefit > 0)
            {
                Output("PI Benefit Straight\t\t[%f]", order->pPIBenefit->straight);
                Output("PI Benefit Held\t\t[%f]", order->pPIBenefit->held);
                Output("PI Benefit Better Fill\t\t[%f]", order->pPIBenefit->betterFilled);
            }*/

            switch (order.orderInfoType)
            {
                case CFETI_ORDERINFO_NOT_SPECIFIED:
                    break;
                case CFETI_ORDERINFO_TSWAP:
                    Util.WriteDebugLogDebug("\tOrderInfoType\t[" + order.orderInfoType + "]");
                    //OutputTSwapDetails((CFETI_TSWAP)order->orderInfo);
                    break;
                case CFETI_ORDERINFO_MMTS:
                    Util.WriteDebugLogDebug("\tOrderInfoType\t[" + order.orderInfoType + "]");
                    //OutputMMTSDetails((CFETI_MMTS_ORDER)order->orderInfo);
                    break;
                case CFETI_ORDERINFO_FX_TRADE:
                    Util.WriteDebugLogDebug("\tOrderInfoType\t[" + order.orderInfoType + "]");
                    //OutputFXTradeDetails((CFETI_FX_TRADE_DESC*)order->orderInfo);
                    break;
                case CFETI_ORDERINFO_FX_OPTION_TRADE:
                    Util.WriteDebugLogDebug("\tOrderInfoType\t[" + order.orderInfoType + "]");
                    //OutputFXOptionTradeDetails((CFETI_FX_OPTION_TRADE_DESC*)order->orderInfo);
                    break;
                case CFETI_ORDERINFO_CDS_TRADE:
                    Util.WriteDebugLogDebug("\tOrderInfoType\t[" + order.orderInfoType + "]");
                    //OutputCdsTradeDetails((CFETI_CDS_TRADE_DESC*)order->orderInfo);
                    break;
                case CFETI_ORDERINFO_CDS_INDEX_OPTION_TRADE:
                    Util.WriteDebugLogDebug("\tOrderInfoType\t[" + order.orderInfoType + "]");
                    //OutputCdsIndexOptionTradeDetails((CFETI_CDS_INDEX_OPTION_TRADE_DESC*)order->orderInfo);
                    break;
                default:
                    break;
            }
            //if (order->instrumentData)
            //OutputInstrumentData(order->instrumentData);
            Util.WriteDebugLogDebug("=============================");
        }



        //STRUCTURES
        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_IDENTIFICATION_DESC
        {
            public String Company;
            public String Application;
            public String Version;
            public String Platform;
            public String OperatingSystem;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_CMD_STATUS
        {
            [MarshalAs(UnmanagedType.I4)]
            public int cmdStatus;
            [MarshalAs(UnmanagedType.LPStr)]
            public String statusText;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_LOGIN_INFO
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string sessionId;
            public IntPtr ts;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szConnectionMode;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szUserId;         /**eSpeed user identity */

            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct)]
            //public CFETI_TRADING_SYS_DESC[] ts;            
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_TRADING_SYS_DESC
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint tsId;
            [MarshalAs(UnmanagedType.LPStr)]
            public String tsDescription;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_TRADE_SETTINGS_DESC
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint retainRule;
            [MarshalAs(UnmanagedType.U4)]
            public uint priorityFollowRule;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_CONNECT_INFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint sessionId;
            [MarshalAs(UnmanagedType.Struct)]
            public CFETI_TRADING_SYS_DESC tradingSystem;
            [MarshalAs(UnmanagedType.U4)]
            public uint tradeFlags;
            [MarshalAs(UnmanagedType.U4)]
            public uint tradeFlags2;
            [MarshalAs(UnmanagedType.Struct)]
            public CFETI_TRADE_SETTINGS_DESC tradeSettings;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_INSTRUMENT_DATA_DESC
        {
            public String instName;
            public uint tsId;
            public ushort numFields;
            public IntPtr fieldTable;            
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CFETI_FIELD_DESC
        {
            public ushort fieldId;
            public byte fieldType;
            public CFETI_FIELD_VALUE fieldValue;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_FIELD_VALUE
        {
            public double Cdecimal;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_PI_BENEFIT_DESC
        {
            public double straight;     /**< The amount of PI Benefit */
            public double held;         /**< The amount of PI Held Benefit */
            public double betterFilled; /**< The amount of Better Filled Benefit */
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_ORDER_DESC
        {
            public String tradeInstrument;
            public double price;
            public double size;
            //public char indicator;
            public byte indicator;
            public uint preferences;          /**< 32 bit mask for order preferences */
            public uint preferences2;		  /**< 32 bit mask for additional order preferences */
            public String tradeId;
            public double tradeSize;
            public /*Int64*/ int tradeTime;
            //public char tradeSide;
            public byte tradeSide;
            public double tradePrice;
            public /*Int64*/ int tradeSettlement;
            public uint settlementDate; /**< CCYYMMDD settlement date */
            public String tradeReference;		   /**< Trade reference common to all legs of a trade */
            public ushort tradeConfirmOperation; /**< Enumerated trade confirm operation */
            public uint legId;				   /**< Trade leg number for trade confirms  consisting of multiple legs */
            public uint legCount;			   /**< The number of legs in this trade */
            public uint recordVersion;		   /**< Trade confirmation version number */
            public uint id;
            public uint subsystem;
            public /*Object*/ IntPtr userData;
            public ushort userDataSize;
            public /*Object*/ IntPtr appUserData;
            public ushort appUserDataSize;
            public String userName;
            public String shortCode;
            public double toPrice; /**< Deprecated. Replaced by altPrice1 */
            public double altPrice1;
            public double altPrice2;
            public uint requestId;
            public /*Int64*/ int tradeRepoEndDate; /**< Deprecated. Replaced by endDate */
            public /*Int64*/ int endDate;
            public uint instrumentIdType;
            public String instrumentId;
            public String tradeComments;
            public uint tradeInfoType;
            public String tradeInfo;
            public uint settlementType;
            public uint orderInfoType;
            public /*Object*/ IntPtr orderInfo;
            public String counterpartyName;
            public uint counterpartyID;
            public String contactName;
            public String contactTelephoneNumber;
            public uint rejectionId;
            public uint tsId;
            public String settlementMethod;
            public double brokerage;
            public /*Int64*/ int paymentDate;
            public uint instProperties;
            public uint tradeProperties;
            public uint priceImprovement;
            public uint checkoutPermissions;
            public String reserved3;        /**< Reserved for future feature */
            public uint basketId;
            public uint basketActions;
            public String requestorId;
            public String originatorId;
            //public CFETI_INSTRUMENT_DATA_DESC instrumentData;
            public IntPtr instrumentData;
            public String clearerTradeId;
            //public CFETI_PI_BENEFIT_DESC pPIBenefit; /**< PI Benefit Data (trade confirms only) */
            public IntPtr pPIBenefit; /**< PI Benefit Data (trade confirms only) */
            public /*Int64*/ int creationTime; /**< Order creation time (responses & notifications only) */
            public String allocationInfo; /**< User specified allocation instructions (free text) */
            public uint dealStructure; /**< Enumerated deal structure */
            public uint tradeType; /**< Enumerated trade type */
            public uint pricingMethod; /**< Enumerated pricing method */
            public double executionPrice; /**< Execution price */
            public uint timeOffset; /**< No. of minutes for which order is valid when pref is GOOD_UNTIL_TIME */
            //public CFETI_TRADE_SETTINGS_DESC tradeSettings; /**< Trade settings */
            public IntPtr tradeSettings; /**< Trade settings */
            public double assetSwapLevel; /**< The corresponding asset swap level at the time of the trade */
            public double reserveMinSize; /**< Min clip size for reserve order */
            public double reserveMaxSize; /**< Max clip size for reserve order */
            public double reserveInitialSize; /**< Initial clip size for reserve order */
            public double yield; /**< Yield corresponding to screen price CFETI_ORDER_DESC::price (where available in trade confirmations) */
            public String bicCode; /**< BIC code (where available in trade confirmations) */
            public String contraTradeReference; /**< Trade indentifier (common to opposing trade confirmations) */
            public String firstPayerDtccId; /**< DTCC 8 character Id of First Payer */
            public String buyerDtccId; /**< DTCC 8 character Id of Buyer */
            public String sellerDtccId; /**< DTCC 8 character Id of Seller */
            public String brokerDtccId; /**< DTCC 8 character Id of Broker */
        }
    }
}
