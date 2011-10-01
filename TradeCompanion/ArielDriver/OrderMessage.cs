using System;
using System.Collections.Generic;
using System.Text;

namespace ArielDriver
{
    class OrderMessage
    {

        
        
        System.String currency;
        public System.String Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        System.String sender;

        public System.String Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        System.String stockSymbol;
        public System.String StockSymbol
        {
            get { return stockSymbol; }
            set { stockSymbol = value; }
        }
        System.Int32 orderQty;

        public System.Int32 OrderQty
        {
            get { return orderQty; }
            set { orderQty = value; }
        }
        System.Int32 side;

        public System.Int32 Side
        {
            get { return side; }
            set { side = value; }
        }
        System.String clientOrderID;

        public System.String ClientOrderID
        {
            get { return clientOrderID; }
            set { clientOrderID = value; }
        }

    }
}
