using System;
using System.Collections.Generic;
using System.Text;

namespace ArielDriver
{
    public class ArielOrderStatusEventArgs : EventArgs
    {
        public AxAPILib._DArielAPIEvents_DealAcceptedEvent _deal;
        private string _ClOrdID;
        private string _Instrument;
        int  _Qty;
        private int _Side;
        private string _Currency;
        private string _Sender;
        public ArielOrderStatusEventArgs(AxAPILib._DArielAPIEvents_DealAcceptedEvent m)
        {
            this._deal = m;
        }

        public String ClOrdID
        {
            get
            {
                return this._ClOrdID;
            }
            set
            {
                this._ClOrdID = value;
            }
        }

        public String Currency
        {
            get
            {
                return this._Currency;
            }
            set
            {
                this._Currency = value;
            }
        }
        
        public String Sender
        {
            get
            {
                return this._Sender;
            }
            set
            {
                this._Sender = value;
            }
        }

        public String OrderID
        {
            get
            {
               return  this._deal.dealNumber.ToString();
            }
        }

        public String OrderStatus
        {
            get
            {
                if (this._deal.accepted == 1)
                    return "Order Accepted";
                else
                    return "Order Cancelled";
            }
        }
        public String OrderStatusLong
        {
            get
            {
                return this._deal.dealerMessage;
            }
        }


        public String Instrument
        {
            get
            {
                return this._Instrument;
            }
            set
            {
                this._Instrument = value;
            }
        }

        public System.Int32 OrderedQty
        {
            get
            {
                return this._Qty;
            }
            set
            {
                this._Qty = value;
            }
        }
        public System.Int32 FilledQty
        {
            get
            {
                return this._Qty;
            }
            set
            {
                this._Qty = value;
            }
        }

        public System.Double Price
        {
            get
            {
                return double.Parse(this._deal.price);
            }
        }

        public System.Int32 Side
        {
            get
            {
                return this._Side;
            }
            set
            {
                this._Side = value;
            }
        }

        public String FillTime
        {
            get
            {
                return this._deal.acceptTime;
            }
        }

         
    }
}
