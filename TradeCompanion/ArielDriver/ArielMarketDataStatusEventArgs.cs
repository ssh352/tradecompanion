using System;
using System.Collections.Generic;
using System.Text;

namespace ArielDriver
{
    public class ArielMarketDataStatusEventArgs : EventArgs
    {
        public AxAPILib._DArielAPIEvents_PriceChangeEvent _price;
        public ArielMarketDataStatusEventArgs(AxAPILib._DArielAPIEvents_PriceChangeEvent m)
        {
            this._price = m;
        }


        public String TimeStamp
        {
            get
            {
                return _price.timestamp;
            }
        }
        public String Symbol
        {
            get
            {
                return _price.market;
            }
        }
        
        public String BidPrice
        {
            
            get
            {
                return _price.bid;
            }
        }

        public String OfferPrice
        {
            get
            {
                return _price.ask;
            }
       
        }
    }
}
