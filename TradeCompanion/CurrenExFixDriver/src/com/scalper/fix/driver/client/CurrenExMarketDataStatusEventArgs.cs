using System;
using System.Collections.Generic;
using System.Text;

namespace com.scalper.fix.driver
{
    public class CurrenExMarketDataStatusEventArgs : EventArgs
    {
        public com.scalper.fix.Message _fixMsg;
        private string bidPrice = "";
        private string offerPrice = "";
        private string symbol = "";
        public CurrenExMarketDataStatusEventArgs(com.scalper.fix.Message m)
        {
            this._fixMsg = m;

            symbol = this._fixMsg.getStringFieldValue(Constants_Fields.TAGSymbol_i);
            string[] retVal = this._fixMsg.getValues(Constants_Fields.TAGMDEntryType_i);//278            
            string[] retPrices = this._fixMsg.getValues(Constants_Fields.TAGMDEntryPx_i);            

            for (int i = 0; i < (retVal.Length); i++)
            {
                if (retVal[i] == "0")
                {
                    bidPrice = retPrices[i];
                }
                else if(retVal[i] == "1")
                {
                    offerPrice = retPrices[i];
                }
            }
            //did some change when the message from the server is chenged
            //if (retVal.Length > 0 && retPrices.Length > 0)
            //{
            //    if (retVal[0].Contains("OFFER"))
            //    {
            //        offerPrice = retPrices[0];
            //    }
            //    else if (retVal[0].Contains("BID"))
            //    {
            //        bidPrice = retPrices[0];
            //    }
            //}
            //if (retVal.Length > 1 && retPrices.Length > 1)
            //{
            //    if (retVal[1].Contains("OFFER"))
            //    {
            //        offerPrice = retPrices[1];
            //    }
            //    else if (retVal[1].Contains("BID"))
            //    {
            //        bidPrice = retPrices[1];
            //    }
            //}
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
