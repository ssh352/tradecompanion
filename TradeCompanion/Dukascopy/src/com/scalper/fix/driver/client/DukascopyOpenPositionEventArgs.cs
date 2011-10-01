using System;
using System.Collections.Generic;
using System.Text;

namespace Dukascopy.com.scalper.fix.driver
{
    public class DukascopyOpenPositionEventArgs : EventArgs 
    {
        public Dukascopy.com.scalper.fix.Message fixMsg;
        private string openPositionAmount = "";
        private string symbol = "";
        private string senderID = "";

        public string OpenPosition
        {
            get { return openPositionAmount; }
            set { openPositionAmount = value; }
        }
        
        public string SenderID
        {
            get { return senderID; }
            set { senderID = value; }
        }

        public string Symbol
        {
            get { return (symbol != null) ? symbol : ""; }
            set { symbol = value; }
        }

        public DukascopyOpenPositionEventArgs(Dukascopy.com.scalper.fix.Message msg)
        {
            this.fixMsg = msg;
            symbol = this.fixMsg.getStringFieldValue(Constants_Fields.TAGSymbol_i);
            senderID = this.fixMsg.getStringFieldValue(Constants_Fields.TAGDKAccountName_i);
            openPositionAmount = this.fixMsg.getStringFieldValue(Constants_Fields.TAGDKAmount_i);
        }
    }
}
