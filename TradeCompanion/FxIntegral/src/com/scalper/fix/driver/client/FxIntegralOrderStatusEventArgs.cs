using System;
using System.Collections.Generic;
using System.Text;

namespace FxIntegral.com.scalper.fix.driver
{
    public class FxIntegralOrderStatusEventArgs : EventArgs
    {
        public FxIntegralOrderStatusEventArgs(FxIntegral.com.scalper.fix.Message m)
        {
            this._fixMsg = m;
        }

        public String Text
        {
            get { return ((this._fixMsg != null) ? this._fixMsg.ToString() : ""); }
        }


        public String ClOrdID
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    retVal = this._fixMsg.getStringFieldValue(Constants_Fields.TAGClOrdID);
                }

                return (retVal != null) ? retVal : "";
            }
        }

        public String OrderID
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    retVal = this._fixMsg.getStringFieldValue(Constants_Fields.TAGOrderID);
                }

                return (retVal != null) ? retVal : "";
            }
        }

        public String ExchangeID
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    retVal = this._fixMsg.getStringFieldValue(Constants_Fields.TAGSecondaryOrderID);
                }

                return (retVal != null) ? retVal : "";
            }
        }

        public String OrderStatus
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    retVal = this._fixMsg.getStringFieldValue(Constants_Fields.TAGOrdStatus);
                }

                return (retVal != null) ? retVal : "";
            }
        }

        public String OrderStatusLong
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    retVal = this._fixMsg.getStringFieldValue(Constants_Fields.TAGText);
                }

                return (retVal != null) ? retVal : "";
            }
        }

        public String Instrument
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    retVal = this._fixMsg.getStringFieldValue(Constants_Fields.TAGSymbol);
                }

                return (retVal != null) ? retVal : "";
            }
        }

        public System.Int32 OrderedQty
        {
            get
            {
                System.Int32 retVal = 0;
                try {
                    if (this._fixMsg != null)
                        retVal = System.Int32.Parse(this._fixMsg.getStringFieldValue(Constants_Fields.TAGOrderQty));
                } catch //(Exception e)
                {
                    //Do Nothing
                }
                return retVal;
            }
        }
        public System.Int32 FilledQty
        {
            get
            {
                System.Int32 retVal = 0;
                try
                {
                    if (this._fixMsg != null)
                        retVal = System.Int32.Parse(this._fixMsg.getStringFieldValue(Constants_Fields.TAGLastShares));
                }
                catch //(Exception e)
                {
                    //Do Nothing
                }
                return retVal;
            }
        }

        public System.Double Price
        {
            get
            {
                System.Double price = 0;
                try
                {
                    if (this._fixMsg != null)
                        price = System.Double.Parse(this._fixMsg.getStringFieldValue(Constants_Fields.TAGAvgPx), util.BasicUtilities.getCulture());
                }
                catch //(Exception e)
                {
                    //Do Nothing
                }
                return price;

            }
        }

        public System.Int32 Side
        {
            get
            {
                System.Int32 side = 0;
                try
                {
                    if (this._fixMsg != null)
                        side = System.Int32.Parse(this._fixMsg.getStringFieldValue(Constants_Fields.TAGSide));
                }
                catch //(Exception e)
                {
                    //Do Nothing
                }
                return side;

            }
        }

        public String FillTime
        {
            get
            {
                string retVal = null;
                if (this._fixMsg != null)
                {
                    try
                    {
                        retVal = this._fixMsg.getStringFieldValue(Constants_Fields.TAGSendingTime);
                    }
                    catch /*(Exception e)*/ { }
                }

                return (retVal != null) ? retVal : "";
            }
        }

        public double totalAmount
        {
            get
            {
                double retValue = 0;
                if (_fixMsg != null)
                {
                    try
                    {
                        retValue = _fixMsg.getDoubleFieldValue(Constants_Fields.TAGNonPippedTotalPrice);
                    }
                    catch /*(Exception e)*/ { }
                }

                return retValue;
            }
        }

        public double pippedTotalAmount
        {
            get
            {
                double retValue = 0;
                if (_fixMsg != null)
                {
                    try
                    {
                        retValue = _fixMsg.getDoubleFieldValue(Constants_Fields.TAGPippedTotalPrice);
                    }
                    catch /*(Exception e)*/ { }
                }

                return retValue;
            }
        }

        public System.Double PipPrice
        {
            get
            {
                System.Double price = 0;
                try
                {
                    if (this._fixMsg != null)
                        price = this._fixMsg.getDoubleFieldValue(Constants_Fields.TAGPipPrice);
                }
                catch //(Exception e)
                {
                    //Do Nothing
                }
                return price;

            }
        }

        public System.String currency
        {
            get
            {
                System.String currency = "";
                try
                {
                    if (_fixMsg != null)
                        currency = _fixMsg.getStringFieldValue(Constants_Fields.TAGCurrency);
                }
                catch //(Exception e)
                {
                    //Do Nothing
                }
                return currency;
            }
        }

        public System.String Sender
        {
            get
            {
                System.String account = "";
                try
                {
                    if (_fixMsg != null)
                        account = _fixMsg.getStringFieldValue(Constants_Fields.TAGTargetCompID);
                }
                catch //(Exception e)
                {
                    //Do Nothing
                }
                return account;
            }
        }
        public FxIntegral.com.scalper.fix.Message _fixMsg;
    }
}
