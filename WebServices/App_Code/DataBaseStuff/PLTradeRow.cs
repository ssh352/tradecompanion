using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebServices.Scalper.DatabaseStuff
{
    /// <summary>
    /// Summary description for PLTrade
    /// </summary>
    public class PLTradeRow
    {
        int id;
        string orderID;
        //string exchange;
        string status;
        string symbol;
        int actions;
        int amount;
        string price;
        DateTime dateID;
        string execOrderId;
        int remaining;
        double pips;
        string senderID;
        DateTime serverDateTime;
        DateTime tradingServerDateTime;
        string tradeCompanionID;
        double marketPrice;

        public PLTradeRow()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string OrderID
        {
            get
            {
                return orderID;
            }
            set
            {
                orderID = value;
            }
        }
        //public string Exchange
        //{
        //    get
        //    {
        //        return exchange;
        //    }
        //    set
        //    {
        //        exchange = value;
        //    }
        //}
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                symbol = value;
            }
        }
        public int Actions
        {
            get
            {
                return actions;
            }
            set
            {
                actions = value;
            }
        }
        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }
        public string Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }
        public string ExecOrderId
        {
            get
            {
                return execOrderId;
            }
            set
            {
                execOrderId = value;
            }
        }

        public DateTime DateID
        {
            get
            {
                return dateID;
            }
            set
            {
                dateID = value;
            }
        }
        public int Remaining
        {
            get
            {
                return remaining;
            }
            set 
            {
                remaining = value;
            }

        }
        public double Pips
        {
            get 
            {
                return pips;
            }
            set
            {
                pips = value;
            }
        }
        public string SenderID
        {
            get
            {
                return senderID;
            }
            set
            {
                senderID = value;
            }
        }
        public DateTime ServerDateTime
        {
            get
            {
                return serverDateTime;
            }
            set
            {
                serverDateTime = value;
            }
        }
        public DateTime TradingServerDateTime
        {
            get 
            {
                return tradingServerDateTime;
            }
            set 
            {
                tradingServerDateTime = value;
            }
        }
        public string TradeCompanionID
        {
            get 
            {
                return tradeCompanionID;
            }
            set 
            {
                tradeCompanionID = value;
            }
        }
        public double MarketPrice
        {
            get
            {
                return marketPrice;
            }
            set
            {
                marketPrice = value;
            }
        }

    }
}
