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
    /// Summary description for OrderRow
    /// </summary>
    public class OrderRow
    {

        int id;
        string orderID;
        string exchange;
        string status;
        string symbol;
        string monthYear;
        int side;
        int quantity;
        string price;
        DateTime timeStamps;
        string tradeCurrency;
        string execOrderId;
        DateTime dateID;
        string tradeCompanionID;
        string currenExID;
        DateTime dateIDcustomer;
        string senderID;
        double marketprice;
       

        public OrderRow()
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
        public string Exchange
        {
            get
            {
                return exchange;
            }
            set
            {
                exchange = value;
            }
        }
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
        public string MonthYear
        {
            get
            {
                return monthYear;
            }
            set
            {
                monthYear = value;
            }
        }
        public int Side
        {
            get
            {
                return side;
            }
            set
            {
                side = value;
            }
        }
        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
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

        public DateTime TimeStamp
        {
            get
            {
                return timeStamps;
            }
            set
            {
                timeStamps = value;
            }
        }
        public string TradeCurrency
        {
            get
            {
                return tradeCurrency;
            }
            set
            {
                tradeCurrency = value;
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
        public string CurrenExID
        {
            get
            {
                return currenExID;
            }
            set
            {
                currenExID = value;
            }
        }

        public DateTime DateIDCustomer
        {
            get
            {
                return dateIDcustomer;
            }
            set
            {
                dateIDcustomer = value;
            }
        }
        public string SenderId
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
        public double MarketPrice
        {
            get
            {
                return marketprice;
            }
            set
            {
                marketprice = value;
            }
        }
              
    }
}
