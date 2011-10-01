using System;
using System.Collections.Generic;
using System.Text;

namespace Icap.com.scalper.fix.driver
{
    public class IcapMarketDataStatusEventArgs : EventArgs
    {
        public Icap.com.scalper.fix.Message _fixMsg;
        private string bidPrice = "";
        private string offerPrice = "";
        private string symbol = "";
        public IcapMarketDataStatusEventArgs(Icap.com.scalper.fix.Message m)
        {
            this._fixMsg = m;

            symbol = this._fixMsg.getStringFieldValue(Constants_Fields.TAGSymbol_i);
            string[] retVal = this._fixMsg.getValues(Constants_Fields.TAGICMDElementName_i);
            string[] retPrices = this._fixMsg.getValues(Constants_Fields.TAGMDEntryPx_i);
            string[] retType = this._fixMsg.getValues(Constants_Fields.TAGMDEntryType_i);
            for (int i = 0; i < retVal.Length; i++)
            {
                if (retType[i] == "0") // entryType "0" = Bid
                {
                    if (retVal[i] == "45") // 45 = dealable_bid 
                    {
                        bidPrice = retPrices[i];
                        //Console.WriteLine("Bid" + bidPrice);
                    }
                }
                else if (retType[i] == "1") // entryType "1" = Offer
                {
                    if (retVal[i] == "46") // 46 = dealable_offer
                    {
                        offerPrice = retPrices[i];
                        //Console.WriteLine("offer" + offerPrice);
                    }
                }
            }
        }

        public String Text
        {
            get { return ((this._fixMsg != null) ? this._fixMsg.ToString() : ""); }
        }


        public String TimeStamp
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    retVal = this._fixMsg.getStringFieldValue(52);
                }

                return (retVal != null) ? retVal : "";
            }
        }
        public String Symbol
        {
            get { return (symbol != null) ? symbol : ""; }
            set { symbol = value; }
        }
        
        public String BidPrice
        {

            get { return bidPrice; }
            set { bidPrice = value; }
        }

        public String OfferPrice
        {
            get { return offerPrice; }
            set { offerPrice = value; }
       
        }
    }
}
