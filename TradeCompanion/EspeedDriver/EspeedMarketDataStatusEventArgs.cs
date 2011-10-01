using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;

namespace EspeedDriver
{
    public class EspeedMarketDataStatusEventArgs
    {
        String bid = "";
        String ask = "";
        String timestamp = "";
        String symbol = "";
        
        public EspeedMarketDataStatusEventArgs(String symbol, String bid, String ask, String timestamp)
        {
            CultureInfo cul = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cul;
            this.symbol = symbol;
            if(!bid.Equals(""))
                this.bid = bid;
            if (!ask.Equals(""))
                this.ask = ask;
            this.timestamp = timestamp;
        }

        public String TimeStamp
        {
            get
            {
                return timestamp;
            }
        }

        public String Symbol
        {
            get
            {
                return symbol;
            }
        }

        public String BidPrice
        {
            get
            {
                return bid;
            }
        }

        public String OfferPrice
        {
            get
            {
                return ask;
            }
        }
    }
}
