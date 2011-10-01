//+------------------------------------------------------------------+
//|                                            TradeRelayLibrary.mq4 |
//|                           Copyright © 2007, Centauris Group inc. |
//|                                    http://www.centaurisgroup.com |
//+------------------------------------------------------------------+
#property copyright "Copyright © 2007, Centauris Group inc."
#property link      "http://www.centaurisgroup.com"
#property library

#import "stdlib.ex4"
string ErrorDescription(int error_code);
#import

// ----------------------------------
// TradeRelayOpenInit()
//  Returns   : 1 if initialized, 0 if already initialized, -1 if init failed.
//  Descripton: Ensures the library is initialized for open orders.
//  Use       : Call in init() of an expert advisor to ensure initialization and count.
// ----------------------------------
int TradeRelayOpenInit()
{
   // if not existing, create init'ed and count variables, then return
   if(!GlobalVariableCheck("g_tradeRelayOpenInitialized"))
   {
      Print("Adding global variables for open orders.");
      GlobalVariableSet("g_tradeRelayOpenInitialized", 1);              // init'ed
      GlobalVariableSet("g_tradeRelayOpenNextIndex", OrdersTotal());    // index to start processing open orders
      GlobalVariableSet("g_tradeRelayOpenCount", 1);                    // count of experts using the open orders of library
      GlobalVariableSet("g_tradeRelayOpenInUse", 0);                    // if equals 1 then writing req files in process, loop until 0 to continue
      Print("Initialized for open orders!");
      
      return(1);
   }
   else  // variable exists, increment count and return
   {
      GlobalVariableSet("g_tradeRelayOpenCount", GlobalVariableGet("g_tradeRelayOpenCount")+1);
      Print("Already initialized for open orders. Increment experts count using the open orders of library. (", GlobalVariableGet("g_tradeRelayOpenCount"), ")");
      
      return(0);
   }
   
   //TODO: add datetime return check on GlobalVariableSet()
   return(-1);
}

// ----------------------------------
// TradeRelayCloseInit()
//  Returns   : 1 if initialized, 0 if already initialized, -1 if init failed.
//  Descripton: Ensures the library is initialized for closed orders.
//  Use       : Call in init() of an expert advisor to ensure initialization and count.
// ----------------------------------
int TradeRelayCloseInit()
{
   // if not existing, create init'ed and count variables, then return
   if(!GlobalVariableCheck("g_tradeRelayCloseInitialized"))
   {
      Print("Adding global variables for close orders.");
      GlobalVariableSet("g_tradeRelayCloseInitialized", 1);                   // init'ed
      GlobalVariableSet("g_tradeRelayCloseNextIndex", OrdersHistoryTotal());  // index to start processing close orders
      GlobalVariableSet("g_tradeRelayCloseCount", 1);                         // count of experts using the close orders of library
      GlobalVariableSet("g_tradeRelayCloseInUse", 0);                         // if equals 1 then writing req files in process, loop until 0 to continue
      Print("Initialized for close orders!");
     
      return(1);
   }
   else  // variable exists, increment count and return
   {
      GlobalVariableSet("g_tradeRelayCloseCount", GlobalVariableGet("g_tradeRelayCloseCount")+1);
      Print("Already initialized for close orders. Increment experts count using the close orders of library. (", GlobalVariableGet("g_tradeRelayCloseCount"), ")");
      
      return(0);
   }
   
   //TODO: add datetime return check on GlobalVariableSet()
   return(-1);
}

// --------------------------------------------------------------------
// TradeRelayCheckOpenOrders()
//  Returns    : The number of orders that were processed in this pass.
//  Description: Checks the orders array and writes a .req file for each new open order since last check.
//  Use        : Call this method in the start() of an expert advisor.
// --------------------------------------------------------------------
int TradeRelayCheckOpenOrders()
{
   string dateYear, dateMonth, dateDay, dateHour, dateMinute;
   string orderType, dateString, timeString;
   int processed = 0;
   int handle = -1;
   int total = 0;
   double openPrice = 0;
   
   // verifies library was correctly initialized for open orders
   if(GlobalVariableGet("g_tradeRelayOpenInitialized") == 1)
   {
      // check if there is a lock, wait until unlocked
      while(!GlobalVariableSetOnCondition("g_tradeRelayOpenInUse", 1, 0))
         Sleep(1);
      
      // get all current orders
      total = OrdersTotal();
      
      // starting at the next index, process all additional orders
      for(int index = GlobalVariableGet("g_tradeRelayOpenNextIndex"); index < total; index++)
      {
         Print("Processing open order at index ", index);
         
         if(OrderSelect(index, SELECT_BY_POS))
         {
            handle = FileOpen(StringConcatenate(OrderSymbol(), OrderTicket(), ".req"), FILE_CSV|FILE_WRITE, ' ');
            
            if(handle < 1)  //error getting file handle, print error, continue with others
            {
               Print("Error getting file handle for ", OrderLots(), " lot(s) of ", OrderSymbol(), " at ", OrderOpenTime(), " [open time]. (message:", ErrorDescription(GetLastError()), ")");
            }
            else
            {
               if(OrderType() == OP_BUY)
               {
                  orderType = "BUY";
                  openPrice = 0;
               }
               else if(OrderType() == OP_SELL)
               {
                  orderType = "SELL";
                  openPrice = 0;
               }
               else if(OrderType() == OP_BUYLIMIT)
               {
                  orderType = "BUYLIMIT";
                  openPrice = OrderOpenPrice();
               }
               else if(OrderType() == OP_SELLLIMIT)
               {
                  orderType = "SELLLIMIT";
                  openPrice = OrderOpenPrice();
               }
               else if(OrderType() == OP_BUYSTOP)
               {
                  orderType = "BUYSTOP";
                  openPrice = OrderOpenPrice();
               }
               else if(OrderType() == OP_SELLSTOP)
               {
                  orderType = "SELLSTOP";
                  openPrice = OrderOpenPrice();
               }
               else continue;    // else, unexpected ordertype, so skip to process next order (should never happen)
               
               dateYear = TimeYear(OrderOpenTime())-1900;
               dateMonth = TimeMonth(OrderOpenTime());
               dateDay = TimeDay(OrderOpenTime());
               dateHour = TimeHour(OrderOpenTime());
               dateMinute = TimeMinute(OrderOpenTime());
               
               if(StringLen(dateMonth) == 1) dateMonth = StringConcatenate("0", dateMonth);
               if(StringLen(dateDay) == 1) dateDay = StringConcatenate("0", dateDay);
               if(StringLen(dateHour) == 1) dateHour = StringConcatenate("0", dateHour);
               if(StringLen(dateMinute) == 1) dateMinute = StringConcatenate("0", dateMinute);

               dateString = StringConcatenate(dateYear, dateMonth, dateDay);
               timeString = StringConcatenate(dateHour, dateMinute);
               
               // The data in the file is in the following order (We added the open price)
               //    1070423  1040  FOREX     AUDUSD  Sell     2       1.0000  1        Scalper1
               //    Date     Time  Exchange  Symbol  Action   Volume  Price   ChartID  WorspaceID
               FileWrite(handle, dateString, timeString, "FOREX", OrderSymbol(), orderType, OrderLots(), openPrice , "1", AccountName());
               FileClose(handle);
               
               // increment the return value (orders processed)
               processed += 1;
            }
         }
         else     // failed selecting current order
         {
            Print("Error selecting open order at index ", index, " (", ErrorDescription(GetLastError()), ")");
         }
      }
      
      // keep next index to process and unlock
      GlobalVariableSet("g_tradeRelayOpenNextIndex", total);
      GlobalVariableSet("g_tradeRelayOpenInUse", 0);
   }
   else
   {
      Alert("Library not initialized for open orders! See instructions in the readme.txt file of the TradeRelayLibrary.");
   }
   
   return(processed);
}

// --------------------------------------------------------------------
// TradeRelayCheckCloseOrders()
//  Returns    : The number of closed orders that were processed in this pass.
//  Description: Checks the closed orders array and writes a .req file for each new close order since last check.
//  Use        : Call this method in the start() of an expert advisor.
// --------------------------------------------------------------------
int TradeRelayCheckCloseOrders()
{
   string dateYear, dateMonth, dateDay, dateHour, dateMinute;
   string orderType, dateString, timeString;
   int processed = 0;
   int handle = -1;
   int total = 0;
   double openPrice = 0;
   
   // verifies library was correctly initialized for close orders
   if(GlobalVariableGet("g_tradeRelayOpenInitialized") == 1)
   {
      // check if there is a lock, wait until unlocked
      while(!GlobalVariableSetOnCondition("g_tradeRelayCloseInUse", 1, 0))
         Sleep(1);
      
      // get all current close orders
      total = OrdersHistoryTotal();
      
      // starting at the next index, process all new closed orders
      for(int index = GlobalVariableGet("g_tradeRelayCloseNextIndex"); index < total; index++)
      {
         Print("Processing closed order at index ", index);
         
         if(OrderSelect(index, SELECT_BY_POS, MODE_HISTORY))
         {
            handle = FileOpen(StringConcatenate(OrderSymbol(), OrderTicket(), ".req"), FILE_CSV|FILE_WRITE, ' ');
            
            if(handle < 1)  //error getting file handle, print error, continue with others
            {
               Print("Error getting file handle for ", OrderLots(), " lot(s) of ", OrderSymbol(), " at  ", OrderCloseTime(), " [close time] (message:", ErrorDescription(GetLastError()), ")");
            }
            else
            {
               if(OrderType() == OP_BUY)
               {
                  orderType = "BUY";
                  //openPrice = 0;
               }
               else if(OrderType() == OP_SELL)
               {
                  orderType = "SELL";
                  //openPrice = 0;
               }
               else if(OrderType() == OP_BUYLIMIT)
               {
                  orderType = "BUYLIMIT";
                  //openPrice = OrderOpenPrice();
               }
               else if(OrderType() == OP_SELLLIMIT)
               {
                  orderType = "SELLLIMIT";
                  //openPrice = OrderOpenPrice();
               }
               else if(OrderType() == OP_BUYSTOP)
               {
                  orderType = "BUYSTOP";
                  //openPrice = OrderOpenPrice();
               }
               else if(OrderType() == OP_SELLSTOP)
               {
                  orderType = "SELLSTOP";
                  //openPrice = OrderOpenPrice();
               }
               else continue;    // else, unexpected ordertype, so skip to process next order (should never happen)
               
               dateYear = TimeYear(OrderCloseTime())-1900;
               dateMonth = TimeMonth(OrderCloseTime());
               dateDay = TimeDay(OrderCloseTime());
               dateHour = TimeHour(OrderCloseTime());
               dateMinute = TimeMinute(OrderCloseTime());
               
               if(StringLen(dateMonth) == 1) dateMonth = StringConcatenate("0", dateMonth);
               if(StringLen(dateDay) == 1) dateDay = StringConcatenate("0", dateDay);
               if(StringLen(dateHour) == 1) dateHour = StringConcatenate("0", dateHour);
               if(StringLen(dateMinute) == 1) dateMinute = StringConcatenate("0", dateMinute);
               
               dateString = StringConcatenate(dateYear, dateMonth, dateDay);
               timeString = StringConcatenate(dateHour, dateMinute);
               
               // The data in the file is in the following order (We added the Price, 0 for closed orders)
               //    1070423  1040  FOREX     AUDUSD  Sell     2       0      1        Scalper1
               //    Date     Time  Exchange  Symbol  Action   Volume  Price  ChartID  WorspaceID
               FileWrite(handle, dateString, timeString, "FOREX", OrderSymbol(), orderType, OrderLots(), openPrice, "1", AccountName());
               FileClose(handle);
               
               // increment return value (orders processed)
               processed += 1;
            }
         }
         else     // failed selecting current order
         {
            Print("Error selecting close order at index ", index, " (", ErrorDescription(GetLastError()), ")");
         }
      }
      
      // set next index to process and unlock
      GlobalVariableSet("g_tradeRelayCloseNextIndex", total);
      GlobalVariableSet("g_tradeRelayCloseInUse", 0);
   }
   else
   {
      Alert("Library not initialized for close orders! See instructions in the readme.txt file of the TradeRelayLibrary.");
   }
   
   return(processed);
}

// ----------------------------------
// TradeRelayOpenDeinit()
//  Descripton: Ensures a valid experts count still using the library for open orders.
//  Use       : Call in deinit() of an expert advisor.
// ----------------------------------
void TradeRelayOpenDeinit()
{
   GlobalVariableSet("g_tradeRelayOpenCount", GlobalVariableGet("g_tradeRelayOpenCount")-1);
   
   if(GlobalVariableGet("g_tradeRelayOpenCount") <= 0)
   {
      Print("No more experts uses the library for open orders, removing global variables.");
      // no more experts using the open orders of library, cleanup global variables.
      GlobalVariableDel("g_tradeRelayOpenCount");
      GlobalVariableDel("g_tradeRelayOpenInitialized");
      GlobalVariableDel("g_tradeRelayOpenNextIndex");
      GlobalVariableDel("g_tradeRelayOpenInUse");
   }
}

// ----------------------------------
// TradeRelayCloseDeinit()
//  Descripton: Ensures a valid experts count still using the library for close orders.
//  Use       : Call in deinit() of an expert advisor.
// ----------------------------------
void TradeRelayCloseDeinit()
{
   GlobalVariableSet("g_tradeRelayCloseCount", GlobalVariableGet("g_tradeRelayCloseCount")-1);
   
   if(GlobalVariableGet("g_tradeRelayCloseCount") <= 0)
   {
      Print("No more experts uses the library for close orders, removing global variables.");
      // no more experts using the close orders of library, cleanup global variables.
      GlobalVariableDel("g_tradeRelayCloseCount");
      GlobalVariableDel("g_tradeRelayCloseInitialized");
      GlobalVariableDel("g_tradeRelayCloseNextIndex");
      GlobalVariableDel("g_tradeRelayCloseInUse");
   }
}

