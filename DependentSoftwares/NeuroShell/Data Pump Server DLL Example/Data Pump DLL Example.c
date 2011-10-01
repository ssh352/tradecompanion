#include <windows.h>
#include <process.h>
#include <conio.h>
#include <stdio.h>
#include <ctype.h>
#include <string.h>
#include <math.h>

typedef struct  {
      char Ticker[10];
	  float barInterval;
	  double date;
      double Tickprice;
} FdData ;
 
typedef struct {
      char symbolTicker[10];
	  float type;
      double hisdate;
      double openPrice;
	  double closePrice;
	  double highPrice;
	  double lowPrice;
} FDHisData;

typedef struct {
      char symbol[20];
	  float barInt; 
} TkAr;

typedef enum BOOLEAN { false, true} blean;
	

#pragma comment(linker, "/SECTION:.shared,RWS")	//Shared Data segment 
#pragma data_seg(".shared")
FdData mfd[100] = {"qwe",0,0};
FDHisData hisData[300] = {"No", 0, 0, 0, 0, 0, 0};
TkAr sycn[20] = {"eur", 0};

char hisSymbol[10] = "No";

int front = 0;
int rear = 0;
int hisfront= 0;
int hisrear = 0;
int index = 0;
int Maxlength = 100;
int HISMAXLENGTH  =  300;

float hisBarInterval = 0;
double period  = 0;

blean statusFlag = false;
blean TCStatusFlag = false;
#pragma data_seg()


#define ASTERIX 3.4e38

void __stdcall UpdatePercent(long Percent);
void __stdcall RequireUserIDPassword();
void __stdcall GetUserIDPassword(char **UserID, char **Password);
long __stdcall NSTSETUPDATA(LPSTR Ticker, LPSTR TickerDescription, float BarInterval, long NumColumns, LPSTR Category, double FirstDateAvailable);
long __stdcall NSTSETLABEL(LPSTR Ticker, float BarInterval, long Col, LPSTR Label);
long __stdcall NSTSETVALUE(LPSTR Ticker, float BarInterval, long Col, double Val);
long __stdcall NSTCOMMITROW(LPSTR Ticker, float BarInterval);

DWORD WINAPI ThreadProc(LPVOID lpParam);
DWORD WINAPI HISThreadProc(LPVOID lpParam);

void DownloadHistoricalData();
int Addtoarr(char st[], float barIntValue);
int Removearr(char st[], float barIntValue);
int IndexOf(char st[]);

void * Mythread;
void * HISMythread;

char oticker[]="EUR/JPY";
int max = 50;
long Connected;
unsigned long dummy;

blean keepRunning = false;
blean keepRunningHisThread = false;
blean threadstate = false;


	/* 
	 * DLLMain() is the DllEntryPoint function for this DLL. 
	 */ 
BOOL _stdcall DLLMain(HINSTANCE hinstDLL,   /* DLL module handle  */ 
		DWORD fdwReason,      /* reason called           */ 
		LPVOID lpvReserved) { /* reserved                */ 
             	switch (fdwReason) {          
		// The DLL is attaching to a process due to process 
		// initialization or a call to LoadLibrary. 
		case DLL_PROCESS_ATTACH: 
			// this forces NeuroShell Trader to require a UserID/Password
			//MyVal=100;
			RequireUserIDPassword();
			break; 
			/* The attached process creates a new thread. */ 
		case DLL_THREAD_ATTACH: 
			break; 
			/* The thread of the attached process terminates. */ 
		case DLL_THREAD_DETACH: 
			break;
			// The DLL is detaching from a process due to
			// process termination or a call to FreeLibrary.
		case DLL_PROCESS_DETACH: 
			break; 
		default:
			break; 
	} 
	
	return TRUE;
}

_declspec (dllexport) long UserIDPasswordChanged(char *UserID, char *Password) {	
	
	char temp[200];
	
	strcpy(temp, UserID);
	strcat(temp, ",");
	strcat(temp, Password);
	
	return -1;
}

_declspec (dllexport) long RemoveTicker(char ticker[], float BarInterval) {	
	/* Stop Data Sink for this ticker/BarInterval combination */
	Removearr(ticker, BarInterval);
	return TRUE;
}

_declspec (dllexport) long RequestTicker(char ticker[], char description[], char category[], double startdate, float BarInterval, char ColumnNames[]) {	
	
	char *UserIDptr;
	char *Passwordptr;
	char *ColumnPtr1;
	char *ColumnPtr2;
	
	char UserID[21];
	char Password[21];

	long i;

	UserIDptr   = &UserID[0];
	Passwordptr = &Password[0];
	
	keepRunning			 = true;
    keepRunningHisThread = true;


	if(BarInterval > 0) {
		BarInterval = 1;
	}

	if (!Connected)	{	
		GetUserIDPassword(&UserIDptr, &Passwordptr);
		Connected = -1;
	}

	if(!TCStatusFlag) {
		MessageBox(NULL, "Historical Support is only for DBFX and Gain Server, You Must login in to TC", "Data Pump DLL",0);
	//	return false;
	} else {
		keepRunningHisThread = true;
		statusFlag = true;	
		period = startdate;
		strcpy(hisSymbol, ticker);
		hisBarInterval = BarInterval;
	}

	/*Calculate the number of columns */
	ColumnPtr1 = ColumnNames;
	ColumnPtr2 = ColumnPtr1;
	i = 0;
	while (*ColumnPtr1 != 0) {	
		i++;
		while (*ColumnPtr2 != ',' && *ColumnPtr2 != 0) {
			  ColumnPtr2++;
		}
		
		if (*ColumnPtr2 != 0) {
			*ColumnPtr2 = 0;
			ColumnPtr2++;
		}
	
		ColumnPtr1 = ColumnPtr2;
	} 

	i = NSTSETUPDATA(ticker, description, BarInterval, i, category, startdate);
	
	/* Set the Column Names (same code as above, but actually sets the names) */
	ColumnPtr1 = ColumnNames;
	ColumnPtr2 = ColumnPtr1;
	NSTSETLABEL(ticker, -(BarInterval), 1, ColumnPtr1);
	//i = 0;
	/*
	while (*ColumnPtr1 != 0) {
		i++;
		while (*ColumnPtr2 != ',' && *ColumnPtr2 != 0) {
			   ColumnPtr2++;			  			   
		}
		
		if (*ColumnPtr2 != 0) {	
			*ColumnPtr2 = 0; 
			ColumnPtr2++;
		}
	    MessageBox(NULL, ColumnPtr1, "Data Pump DLL",0);

		NSTSETLABEL(ticker, -(BarInterval), i, ColumnPtr1);
		ColumnPtr1 = ColumnPtr2;
	}*/
		
	/* This is to hold on display chart until finish downloading historical data */
	DownloadHistoricalData();

	Addtoarr(ticker, BarInterval);

	if (threadstate == false){
		Mythread = CreateThread(NULL, NULL,ThreadProc,NULL, 0,&dummy);
		threadstate = true;
	}	
	if(Mythread == NULL) {
	     MessageBox(NULL, "fail to create connection", "Data Pump DLL",0);
	     return FALSE;
	} 
		
	return TRUE;
}

void DownloadHistoricalData() {
		
	
	if(keepRunningHisThread) {

		 HISMythread = CreateThread(NULL, NULL,HISThreadProc,NULL, 0,&dummy);		

		  if(HISMythread == NULL) {
				 MessageBox(NULL, "fail to create connection", "Data Pump DLL",0);
				 exit(0);
		  } 
			
		  while(statusFlag) {
				Sleep(100);
		  }
			
		  keepRunningHisThread = false;
	}
}

_declspec (dllexport) void SetFlag() {
	statusFlag = false;
}

_declspec (dllexport) void SetTCStatus(blean status) {
	TCStatusFlag = status;
}

   	/*function to add data to queue from TC	*/
_declspec (dllexport) void setdata(BSTR st, double date, double tick) {

	int indx = IndexOf(st);

	if(indx != -1) {
		int t = (rear+1) % Maxlength;
			if(t != front) {
				rear = t;	     
				strcpy(mfd[rear].Ticker,st);
				mfd[rear].date        = date;
				mfd[rear].Tickprice   = tick;  
     			mfd[rear].barInterval = -(sycn[indx].barInt);
		
			}
	}
}

   /* thread function to read data from queue & send it to the NST */
DWORD WINAPI ThreadProc(LPVOID lpParam) {
	float liveBarInt;
	
	while(keepRunning == true) {
		if (rear != front) {
			front +=1;
			front %= Maxlength;
		
		    strcpy(oticker,mfd[front].Ticker);
		    liveBarInt = mfd[front].barInterval;
		
			NSTSETVALUE(oticker, liveBarInt, 1, mfd[front].date);
	 	    NSTSETVALUE(oticker, liveBarInt, 2, mfd[front].Tickprice);
		    
			NSTCOMMITROW(oticker, liveBarInt);	

		}
		Sleep(100);			
	}
	return 0;
}

	/*set historical data
	 function to add historical data to queue from TC */
_declspec (dllexport) void sethisdata(BSTR st, double date, double open, double close, double high, double low, float type) { 

	int t = (hisrear+1) % HISMAXLENGTH;
		if(t != hisfront) {
			hisrear = t;	     
			strcpy(hisData[hisrear].symbolTicker, st);
			hisData[hisrear].hisdate    = date;
			hisData[hisrear].openPrice  = open;
			hisData[hisrear].closePrice = close;
			hisData[hisrear].highPrice  = high;
			hisData[hisrear].lowPrice   = low;
			hisData[hisrear].type       = type; 
		}
}

    /* thread function to read data from queue & send it to the NST */
DWORD WINAPI HISThreadProc(LPVOID lpParam) { 

	while(keepRunningHisThread == true)	{
		if (hisrear != hisfront) {
		     char symb[10];
			 float barIntval;
			 
			 
			 hisfront +=1;
		     hisfront %= HISMAXLENGTH;
			 
			 barIntval = hisData[hisfront].type;
			 strcpy(symb, hisData[hisfront].symbolTicker);
		    
			 NSTSETVALUE(symb, barIntval, 1, hisData[hisfront].hisdate);
			 NSTSETVALUE(symb, barIntval, 2, hisData[hisfront].openPrice);
			
			 if(hisData[hisfront].type) {
				 NSTSETVALUE(symb, barIntval, 3, hisData[hisfront].highPrice);
				 NSTSETVALUE(symb, barIntval, 4, hisData[hisfront].lowPrice);
				 NSTSETVALUE(symb, barIntval, 5, hisData[hisfront].closePrice);
			 }

			 NSTSETVALUE(symb, barIntval, 6, 0);
			 NSTCOMMITROW(symb, barIntval);

		}
		Sleep(5);
	}
	return 0;
}


_declspec (dllexport) char getHistoricalSymbol() {
    return hisSymbol;
}


_declspec (dllexport) double getHistoricalDataTime() {
	strcpy(hisSymbol,"No"); /*This to make the historical symbol empty or set to normal stat*/
	return period;
}


_declspec (dllexport) float getHistoricalBarInterval() {
	return hisBarInterval;
}

int IndexOf(char st[]) {
     int i ;
     if(index !=0) {
		 for (i = 0; i < index; i ++) {
			if(strcmp(sycn[i].symbol, st) == 0) {
				return i;
			}
		 }
     }
	return -1;
}

int Addtoarr(char st[], float barIntValue) {

	int indx = IndexOf(st);
	 if (index < max) {
		 if(indx != -1) {
			 if(barIntValue < sycn[indx].barInt) {
				  	   sycn[indx].barInt = barIntValue;
					   return 0;
			 } 
		 } 
		 else {
			     strcpy(sycn[index].symbol, st);
				 sycn[index].barInt = barIntValue;
				 index++;		 
		}
	}
	return -1;
}	
	
int Removearr(char st[], float barIntValue) {
	int i = 0, j = 0;
	i = IndexOf(st);

    if(i == 0 || i  == max -1) {
      index--;
      return i;
    } else if(i != -1) {
		for (j = i; j < index; j ++) {
		   strcpy(sycn[j].symbol, sycn[j+1].symbol);
		   sycn[j].barInt = sycn[j+1].barInt;
		}
		index--;
		return i;
    }
    return -1;
}

