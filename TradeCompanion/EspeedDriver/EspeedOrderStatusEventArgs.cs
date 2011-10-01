using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace EspeedDriver
{
    public class EspeedOrderStatusEventArgs
    {
        int command;
        String status;
        String exchID;
        String symbol;
        int side;
        String currency = "";
        String orderId;
        String senderId;
        double qty = 0;
        double price = 0;
        String timestamp;

        public const int CFETI_ORDER_QUEUED = 0xa;
        public const int CFETI_ORDER_REJECTED = 0xb;
        public const int CFETI_ORDER_EXECUTED = 0xc;
        public const int CFETI_ORDER_CANCELLED = 0xd;
        public const int CFETI_TRADE_CONFIRM = 0xe;
        public const int CFETI_ORDER_EXECUTING = 0xb7;

        public EspeedOrderStatusEventArgs()
        {                    
        }

        public void updateData(UserData ud, EspeedClient.CFETI_ORDER_DESC orderData, int command, String status)
        {
            try
            {
                this.command = command;
                this.status = status;
                exchID = orderData.tsId.ToString();
                symbol = orderData.tradeInstrument;

                if (orderData.indicator == EspeedClient.CFETI_ORDER_SELL)
                    side = 2;
                if (orderData.indicator == EspeedClient.CFETI_ORDER_BUY)
                    side = 1;
                if (command == CFETI_ORDER_QUEUED || command == CFETI_ORDER_REJECTED || command == CFETI_ORDER_CANCELLED)
                {
                    qty = orderData.size;
                    price = orderData.price;
                }

                if (command == CFETI_ORDER_EXECUTED  || command == CFETI_TRADE_CONFIRM || command == CFETI_ORDER_EXECUTING)
                {
                    qty = orderData.tradeSize;
                    price = orderData.executionPrice;                    
                }

                if (orderData.tradeTime == 0)
                    timestamp = orderData.tradeTime.ToString();
                else
                {
                    timestamp = orderData.tradeTime.ToString();
                    
                    int year = (orderData.tradeTime / 31557600);
                    int month = (int)((orderData.tradeTime - (year * 31557600)) / 2592000);
                    int day = (int)((orderData.tradeTime - (year * 31557600) - (month * 2592000)) / 86400);
                    int fmtTime = orderData.tradeTime - (year * 31557600) - (month * 2592000) - (day * 86400);
                    int HH = fmtTime / 3600;
                    int MM = (fmtTime - (HH * 3600)) / 60;
                    int SS = (fmtTime - (HH * 3600)) % 60;
                    timestamp = ((1970 + year).ToString()) +
                    (((1 + month) < 9) ? ("0" + (1 + month).ToString()) : (1 + month).ToString()) +
                    (((day) < 9) ? ("0" + (day).ToString()) : (day).ToString()) + "-" +
                    ((HH < 10) ? ("0" + HH.ToString()) : HH.ToString()) + ":" +
                    ((MM < 10) ? ("0" + MM.ToString()) : MM.ToString()) + ":" +
                    ((SS < 10) ? ("0" + SS.ToString()) : SS.ToString());
                }
                orderId = ud.clientOrderId;
                senderId = ud.senderId;
            }
            catch (Exception e)
            {
                Util.WriteDebugLogError("Error in updateData -- " + e.Message + e.StackTrace);//Console.Write(e.StackTrace);
            }
        }

        public int Command
        {
            get { return command; }
            set { command = value; }
        }

        public String Status
        {
            get { return status; }
            set { status = value; }
        }


        public String ExchID
        {
            get { return exchID; }
            set { exchID = value; }
        }


        public String Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }


        public int Side
        {
            get { return side; }
            set { side = value; }
        }


        public String Currency
        {
            get { return currency; }
            set { currency = value; }
        }


        public String OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }


        public String SenderId
        {
            get { return senderId; }
            set { senderId = value; }
        }


        public double Qty
        {
            get { return qty; }
            set { qty = value; }
        }


        public double Price
        {
            get { return price; }
            set { price = value; }
        }


        public String Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
    }
}
