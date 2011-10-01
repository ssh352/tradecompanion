//+------------------------------------------------------------------+
//|                                    TradeRelayLibraryStrategy.mq4 |
//|                           Copyright © 2007, Centauris Group inc. |
//|                                    http://www.centaurisgroup.com |
//+------------------------------------------------------------------+
#property copyright "Copyright © 2007, Centauris Group inc."
#property link      "http://www.centaurisgroup.com"

#import "TradeRelayLibrary.ex4"
   int TradeRelayOpenInit();
   int TradeRelayCheckOpenOrders();
   void TradeRelayOpenDeinit();
   int TradeRelayCloseInit();
   int TradeRelayCheckCloseOrders();
   void TradeRelayCloseDeinit();
   
#import "stdlib.ex4"
   string ErrorDescription(int error_code);
   
#import

#define RSI_OVERBOUGHT 51     // usually 70-80, is set lower for more trades
#define RSI_OVERSOLD 49       // usually 20-30, is set higher for more trades

//+------------------------------------------------------------------+
//| expert initialization function                                   |
//+------------------------------------------------------------------+
int init()
{
   Print("init()");
   
   // ensures initialization of the library for open and closed orders
   int openInit = TradeRelayOpenInit();
   int closeInit = TradeRelayCloseInit();
   
   return(0);  // auto-generated code
}

//+------------------------------------------------------------------+
//| expert start function                                            |
//+------------------------------------------------------------------+
int start()
{
   Print("start()");
   
   int ticket = 0;
   double cRSI = iRSI(NULL, 0, 14, PRICE_CLOSE, 0);
   double pRSI = iRSI(NULL, 0, 14, PRICE_CLOSE, 1);
   
   Print(" Current RSI:", cRSI, ", Previous RSI:", pRSI, ", Bid:", Bid, ", Ask:", Ask);
   
   if((cRSI >= RSI_OVERBOUGHT) || (cRSI <= RSI_OVERSOLD))
   {
      // if current bar rsi is above set limit, SELL
      if(cRSI >= RSI_OVERBOUGHT)
      {
         Print(" Sell Condition Reached.");
      
         if(IsTradeAllowed())
         {
            ticket = OrderSend(Symbol(), OP_SELL, 1, Bid, 2, Bid+10*Point, Bid-10*Point, "Market Sell Order", 0, 0, Green);
            if(ticket == -1)
               Print("   Market Order Sell failed (message:", ErrorDescription(GetLastError()),")");
            else
               Print("   Sold 1 Lot at ", Bid, ", stoploss at ", Bid+10*Point, ", take profit at ", Bid-10*Point);
         }
         else
         {
            Print("Trading disabled for this chart.");
         }
      }
      // if current bar rsi is less than set limit, BUY
      if(cRSI <= RSI_OVERSOLD)
      {
         Print(" Buy Condition Reached.");
      
         if(IsTradeAllowed())
         {
            ticket = OrderSend(Symbol(), OP_BUY, 1, Ask, 2, Ask-10*Point, Ask+10*Point, "Market Buy Order", 0, 0, Yellow);
            if(ticket == -1)
               Print("   Market Order Buy failed (message:", ErrorDescription(GetLastError()),")");
            else
               Print("   Bought 1 Lot at ", Ask, ", stoploss at ", Ask-10*Point, " take profit at ", Ask+10*Point);
         }
         else
         {
            Print("Trading disabled for this chart.");
         }
      }
   }
   else
   {
      Print(" No Buy/Sell Condition Reached");
   }
   
   // call relay library to process open and closed orders.
   Print("Processed ", TradeRelayCheckOpenOrders(), " open orders.");
   Print("Processed ", TradeRelayCheckCloseOrders(), " closed orders.");
   
   return(0);
}

//+------------------------------------------------------------------+
//| expert deinitialization function                                 |
//+------------------------------------------------------------------+
int deinit()
{
   Print("deinit()");
   
   TradeRelayOpenDeinit();
   TradeRelayCloseDeinit();
   
   return(0);
}
//+------------------------------------------------------------------+