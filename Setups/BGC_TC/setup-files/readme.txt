=======================================================

	MetaTrader 4 TradeRelayLibrary Readme       

=======================================================

1. Description
2. Installing the library
3. Using the Library
4. Known Issues / Limitations



1. Description
---------------

This Library creates files for use with TradeCompanion.
It process all open and close orders made after the 
advisor, which uses this library, is added to a chart.
Any previous orders are bypassed, to avoid creating 
any duplicate orders.



2. Installing the Library
--------------------------

To install this library, put the "TradeRelayLibrary.ex4"
file in the libraries folder of your MetaTrader 4 
installation. By default, this folder is:
C:\Program Files\MetaTrader 4\experts\libraries



3. Using the Library
---------------------

To use this library, you must add import declarations and 
add some functions calls. Add the following declaration at 
the top of your Expert Advisor, below it's declarations and 
before the init() function:


 #import "TradeRelayLibrary.ex4"
    int TradeRelayOpenInit();
    int TradeRelayCheckOpenOrders();
    void TradeRelayOpenDeinit();
    int TradeRelayCloseInit();
    int TradeRelayCheckCloseOrders();
    void TradeRelayCloseDeinit();
 #import


 Then, for processing open orders:

 1. Add TradeRelayOpenInit() at the beginning of the init() function
    of your Expert Advisor, before your custom init() code.
    This method returns:
      1 if initialized at this call
      0 if already initialized
     -1 if initialization failed.
 
 2. Add TradeRelayCheckOpenOrders() at the end of the start() function
    of your expert advisor, after your custom start() code.
    This method returns the number of open orders processed at this call.
 
 3. Add TradeRelayOpenDeinit() at the end of the deinit() function
    of your expert advisor, after your deinit() code.
    This method returns nothing.


 And, for processing close orders:

 1. Add TradeRelayCloseInit() at the beginning of the init() function
    of your Expert Advisor, before your init() code.
    This method returns:
      1 if initialized at this call
      0 if already initialized
     -1 if initialization failed.
     
 2. Add TradeRelayCheckCloseOrders() at the end of the start() function
    of your expert advisor, after your strategy code.
    This method returns the number of close orders processed at this call.
    
 3. Add TradeRelayCloseDeinit() at the end of the deinit() function
    of your expert advisor, after your deinit() code.
    This method returns nothing.


 Additional information is available in this sample code.

	//+------------------------------------------------------------------+
	//|                                              MyExpertAdvisor.mq4 |
	//|                              Copyright © 2007, Company Name inc. |
	//|                                           http://www.website.com |
	//+------------------------------------------------------------------+
	#property copyright "Copyright © 2007, Company Name inc."
	#property link      "http://www.website.com"

	#import "TradeRelayLibrary.ex4"
	   int TradeRelayOpenInit();
	   int TradeRelayCheckOpenOrders();
	   void TradeRelayOpenDeinit();
	   int TradeRelayCloseInit();
	   int TradeRelayCheckCloseOrders();
	   void TradeRelayCloseDeinit();
	#import

	//+------------------------------------------------------------------+
	//| expert initialization function                                   |
	//+------------------------------------------------------------------+
	int init()
	{
	//----
	   int openInitOk = TradeRelayOpenInit();
	   int closeInitOk = TradeRelayCloseInit();
	   
	   // You should check the return values for the initialized status.
	   // if the library is not initialized correctly, the other functions of
	   // the library simply won't run their code. Initlialization is not
	   // supposed to be failing, but it's a good practice to check anyway.

	   // *** Add your custom init() code here ***

	//----
	   return(0);
	}
	
	//+------------------------------------------------------------------+
	//| expert deinitialization function                                 |
	//+------------------------------------------------------------------+
	int deinit()
	{
	//----

	   // *** Add your custom deinit() code here ***

	   TradeRelayOpenDeinit();
	   TradeRelayCloseDeinit();

	//----
	   return(0);
	}
	
	//+------------------------------------------------------------------+
	//| expert start function                                            |
	//+------------------------------------------------------------------+
	int start()
	{
	//----

	   // *** Add your custom start() code here ***

	   int openProcessed = TradeRelayCheckOpenOrders();
	   int closeProcessed = TradeRelayCheckCloseOrders();
	   
	   // You may also add code here that uses openProcessed and/or closeProcessed
	   // No obligation in setting the return value to a variable. You can simply call
	   // the method and be done with it.

	//----
	   return(0);
	}
	//+------------------------------------------------------------------+



4. Known Issues / Limitations
----------------------------

  A) MetaTrader 4 must be running 24 hours to correctly process all close orders.
 
   If any open positions are closed, while MetaTrader is not running, they 
   won't be processed by the Library when MetaTrader is re-opened. This ensures 
   historical orders do not generate files for older orders and avoid duplicates.


  B) All orders are processed, not only the ones of the current chart.

   When an advisor gets it's start() method called, all orders are
   processed, not only the orders of the chart in which the advisor
   is running. Automated and Manual orders are processed too.
   If another advisor gets it's start() method called, it will wait
   until any currently running advisor, using the library, has finished
   processing the new orders, then it will try processing new orders too.
   If it finds none, it will simply return 0.  This ensures orders do not 
   get processed multiple time and get processed as fast as possible.


  C) Library initialization sequence can fail to initialize correctly.

   If more than one expert advisor, using the library, is loaded and 
   the program gets closed, when the program is re-opened, it is possible 
   that the initialization sequence does not occur correctly. To verify
   if the initialization occured correctly, look in the Experts output
   page for these messages:

   (*)  "Adding global variables for open (or close) orders." 
   (**) "Already initialized for open (or close) orders. Increment experts count 
         using the open orders of library. (N)"

   *  : this message should appear only once for a correct initialization.
   ** : N is the count of experts using the library. If there is 4 experts using the
        library, the last message should state (4).
   
   If these messages do not apear as specified above, the initialization sequence 
   did not occur correctly. Remove all experts and add them again to the charts.
   This problem occurs randomly and mostly when the expert advisors are 
   disabled before the program is closed. You should leave the expert advisors enabled
   if you need to close the program, or, if you need to disable the advisors before
   closing the program, first remove them from the charts.

=======================================================
Readme Updated: 07-13-2007
=======================================================
