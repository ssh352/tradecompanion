using System;
using System.Collections.Generic;
using System.Text;

namespace EspeedDriver
{
    class Constants
    {
        //CONSTANTS
        //Return code for success 
        public readonly static int CFETI_SUCCESS = 0;

        //Commands issued by eSpeed API 
        public readonly static int CFETI_LOGIN_ACCEPTED = 0x1;
        public readonly static int CFETI_LOGIN_REJECTED = 0x2;
        public readonly static int CFETI_LOGIN_TERMINATED = 0x3;
        public readonly static int CFETI_SET_PASSWORD_ACCEPTED = 0x43;
        public readonly static int CFETI_SET_PASSWORD_REJECTED = 0x44;
        public readonly static int CFETI_STATUS = 0x1d;
        public readonly static int CFETI_LOGOUT_ACCEPTED = 0x1e;
        public readonly static int CFETI_LOGOUT_REJECTED = 0x1f;
        public readonly static int CFETI_CONNECTION_ACCEPTED = 0x18;
        public readonly static int CFETI_CONNECTION_REJECTED = 0x19;
        public readonly static int  CFETI_CONNECTION_TERMINATED = 0x1a;
        public readonly static int  CFETI_DISCONNECT_ACCEPTED = 0x1b;
        public readonly static int  CFETI_DISCONNECT_REJECTED = 0x1c;
        public readonly static int  CFETI_REFRESH_COMPLETE = 0x24;
        public readonly static int  CFETI_ORDER_QUEUED = 0xa;
        public readonly static int  CFETI_ORDER_REJECTED = 0xb;
        public readonly static int  CFETI_ORDER_EXECUTED = 0xc;
        public readonly static int  CFETI_ORDER_CANCELLED = 0xd;
        public readonly static int  CFETI_TRADE_CONFIRM = 0xe;
        public readonly static int  CFETI_ORDER_EXECUTING = 0xb7; /**< An execution will be delivered for this order at this or a better price for the size indicated in the SIZE_DONE field */
        public readonly static int  CFETI_SUBSCRIBE_ACCEPTED = 0x13;
        public readonly static int  CFETI_SUBSCRIBE_REJECTED = 0x14;
        public readonly static int  CFETI_UNSUBSCRIBE_ACCEPTED = 0x15;
        public readonly static int  CFETI_UNSUBSCRIBE_REJECTED = 0x16;
        public readonly static int  CFETI_SUBSCRIBE_STATUS = 0x20;
        public readonly static int  CFETI_MKT_CREATED = 0x23;

        //eSpeed API command status and error codes
        public readonly static int  CFETI_INSTRUMENT_LOST = 0x14;
        public readonly static int  CFETI_INSTRUMENT_RESTORED = 0x15;
        public readonly static int  CFETI_MARKET_FEED_DOWN = 0x16;
        public readonly static int  CFETI_MARKET_FEED_RESTORED = 0x17;

        //Commands issued by user
        public readonly static int  CFETC_SUBMIT_ORDER = 0x3;
        public readonly static int  CFETC_SUBSCRIBE = 0x6;
        public readonly static int  CFETC_UNSUBSCRIBE = 0x7;
        public readonly static int  CFETC_DD_REGISTER = 0x1c;
        public readonly static int  CFETC_DD_DEREGISTER = 0x1d;

        //Market and Order indicators
        const byte CFETI_ORDER_BUY = 0x4;
        const byte CFETI_ORDER_TAK = CFETI_ORDER_BUY;
        const byte CFETI_ORDER_SELL = 0x3;
        const byte CFETI_ORDER_HIT = CFETI_ORDER_SELL;

        //Enumerated market retain rule
        public readonly static int  CFETI_RETAIN_DEFAULT = 0x0;

        //Enumerated market priority follow rule 
        public readonly static int  CFETI_PRIORITY_FOLLOW_DEFAULT = 0x0;

        //Trade control flags
        public readonly static int  CFETI_DIRECT_DEALING_ENABLED = 0x200;

        /*
        * Values for order info type component of CFETI_ORDER_DESC structure 
        */
        public readonly static int  CFETI_ORDERINFO_NOT_SPECIFIED = 0;
        public readonly static int  CFETI_ORDERINFO_BLOCK_TRADE = 1; /**< Block trade support */
        public readonly static int  CFETI_ORDERINFO_EXTENDED_PROPERTIES = 4;
        public readonly static int  CFETI_ORDERINFO_ENERGY_TRADE = 5;
        public readonly static int  CFETI_ORDERINFO_FX_TRADE = 6;
        public readonly static int  CFETI_ORDERINFO_TSWAP = 8;
        public readonly static int  CFETI_ORDERINFO_PV01_LOCK = CFETI_ORDERINFO_TSWAP; /**< @Indicates that the orderInfo pointer is of type CFETI_PV01_LOCK_DESC */
        public readonly static int  CFETI_ORDERINFO_MMTS = 9;
        public readonly static int  CFETI_ORDERINFO_FX_OPTION_TRADE = 10;
        public readonly static int  CFETI_ORDERINFO_CANTOR_REPO = 11;
        public readonly static int  CFETI_ORDERINFO_IRD_TRADE = 12;
        public readonly static int  CFETI_ORDERINFO_ESPD_REPO_TRADE = 13;
        public readonly static int  CFETI_ORDERINFO_ESPD_REPO = 14;
        public readonly static int  CFETI_ORDERINFO_CDS_TRADE = 15;
        public readonly static int  CFETI_ORDERINFO_CDS_INDEX_OPTION_TRADE = 16;
        public readonly static int  CFETI_ORDERINFO_CDS_INDEX_TRANCHE_TRADE = 17;
    }
}
