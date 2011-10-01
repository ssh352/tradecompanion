using System;
using System.Collections.Generic;
using System.Text;

namespace com.scalper.fix.driver.client
{
    public class MarketData
    {
        String bid = "";
        String ask = "";
        String symbol = "";
        bool isBidBadTick = true;
        bool isAskBadTick = true;
    
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

        public bool IsAskBadTick
        {
            get { return isAskBadTick; }
            set { isAskBadTick  = value; }
        }

        public bool IsBidBadTick
        {
            get { return isBidBadTick; }
            set { isBidBadTick = value; }
        }

    }
}
