using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;

namespace EspeedDriver
{
    class MarketData
    {
        String bid = "";
        String ask = "";
        String symbol = "";
        CultureInfo cul = new CultureInfo("en-US");
        public MarketData()
        {
         Thread.CurrentThread.CurrentCulture = cul;
        }

        public String Bid
        {
            get { return bid; }
            set { bid = value; }
        }
        

        public String Ask
        {
            get { return ask; }
            set { ask = value; }
        }
       

        public String Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
    }
}
