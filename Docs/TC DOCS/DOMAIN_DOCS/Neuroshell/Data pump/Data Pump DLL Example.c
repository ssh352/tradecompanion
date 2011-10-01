#include <windows.h>
#include <process.h>
#include <conio.h>
#include <stdio.h>
#include <ctype.h>
#include <string.h>
#include <math.h>

typedef struct 
{
      char Ticker[10];
      double date;
      double Tickprice;
} FdData ;

typedef struct 
{
      char symbol[20];
} TkAr;

typedef enum BOOLEAN { false, true} blean;
	

#pragma comment(linker, "/SECTION:.shared,RWS")	//Shared Data segment 
#pragma data_seg(".shared")
FdData mfd[100] = {"qwe",0,0};
int front = 0;
int rear = 0;
int Maxlength = 100;
TkAr sycn[20] = {"eur"};
int index = 0;
#pragma data_seg()



#define ASTERIX 3.4e38

void __stdcall UpdatePercent(long Percent);
long __stdcall NSTSETUPDATA(LPSTR Ticker, LPSTR TickerDescription, float BarInterval, long NumColumns, LPSTR Category, double FirstDateAvailable);
long __stdcall NSTSETLABEL(LPSTR Ticker, float BarInterval, long Col, LPSTR Label);
long __stdcall NSTSETVALUE(LPSTR Ticker, float BarInterval, long Col, double Val);
long __stdcall NSTCOMMITROW(LPSTR Ticker, float BarInterval);
void __stdcall RequireUserIDPassword();
void __stdcall GetUserIDPassword(char **UserID, char **Password);

DWORD WINAPI ThreadProc(LPVOID lpParam);

int Addtoarr(char st[]);
int Removearr(char st[]);
int IndexOf(char st[]);

long Connected;
char oticker[]="EUR/JPY";
float oBarInterval;
double odate_time;
double tickprice;
unsigned long dummy;
void * Mythread;
blean keepRunning = false;
blean threadstate = false;
int max = 20;



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

_declspec (dllexport) long UserIDPasswordChanged(char *UserID, char *Password)
{	char temp[200];
	strcpy(temp, UserID);
	strcat(temp, ",");
	strcat(temp, Password);
	//MessageBox(NULL, temp, "Data Pump DLL Example", 0);

	return -1;
}

_declspec (dllexport) long RemoveTicker(char ticker[], float BarInterval)
{	// Stop Data Sink for this ticker/BarInterval combination
	Removearr(ticker);
	return TRUE;
}

_declspec (dllexport) long RequestTicker(char ticker[], char description[], char category[], double startdate, float BarInterval, char ColumnNames[])
{	long i;
	
	char *UserIDptr;
	char *Passwordptr;
	char UserID[21];
	char Password[21];

	double date;

	char *ColumnPtr1;
	char *ColumnPtr2;
	oBarInterval=BarInterval;
	strcpy(oticker,ticker);
	odate_time=startdate;
	UserIDptr = &UserID[0];
	Passwordptr = &Password[0];
	keepRunning = true;

	if (!Connected)
	{	GetUserIDPassword(&UserIDptr, &Passwordptr);
		Connected = -1;
	}

	// Calculate the number of columns
	ColumnPtr1 = ColumnNames;
	ColumnPtr2 = ColumnPtr1;
	i = 0;
	while (*ColumnPtr1 != 0)
	{	i++;
		while (*ColumnPtr2 != ',' && *ColumnPtr2 != 0) ColumnPtr2++;
		
		if (*ColumnPtr2 != 0)
		{	*ColumnPtr2 = 0;
			ColumnPtr2++;
		}
		ColumnPtr1 = ColumnPtr2;
	}

	i = NSTSETUPDATA(ticker, description, BarInterval, i, category, startdate);
	
	// Set the Column Names (same code as above, but actually sets the names)
	ColumnPtr1 = ColumnNames;
	ColumnPtr2 = ColumnPtr1;
	i = 0;
	while (*ColumnPtr1 != 0)
	{	i++;
		while (*ColumnPtr2 != ',' && *ColumnPtr2 != 0) ColumnPtr2++;
		
		if (*ColumnPtr2 != 0)
		{	*ColumnPtr2 = 0;
			ColumnPtr2++;
		}
		
		NSTSETLABEL(ticker, BarInterval, i, ColumnPtr1);
		ColumnPtr1 = ColumnPtr2;
	}
		
	Addtoarr(ticker);
	if (threadstate == false){
		Mythread = CreateThread(NULL, NULL,ThreadProc,NULL, 0,&dummy);
		threadstate = true;
	}	
	if(Mythread == NULL)
	{
	     MessageBox(NULL, "fail to create connection", "Data Pump DLL",0);
	     return FALSE;
	} 
	return TRUE;
}


_declspec (dllexport) void setdata(BSTR st, double date, double tick)    //function to add data to queue from TC
{
	int t;
	if(IndexOf(st) != -1{
	t = (rear+1) % Maxlength;
	if(t != front)
	{
	    rear = t;	     
	    strcpy(mfd[rear].Ticker,st);
	    mfd[rear].date = date;
	    mfd[rear].Tickprice = tick;       	      
   	}}	
}


DWORD WINAPI ThreadProc(LPVOID lpParam)    // thread function to read data from queue & send it to the NST
{
	
	while(keepRunning == true)
	{
		if (rear != front)
		{
		     front +=1;
		     front %= Maxlength;
		    strcpy(oticker,mfd[front].Ticker);
		    odate_time = mfd[front].date;
	     	    tickprice = mfd[front].Tickprice;
		    NSTSETVALUE(oticker, 0, 1, odate_time);
	 	    NSTSETVALUE(oticker, 0, 2, tickprice);
		    NSTCOMMITROW(oticker,0);	
		}
		Sleep(100);			
	}
	return 0;
}

int IndexOf(char st[])
{
     int i ;
     if(index !=0){
     for (i = 0; i < index; i ++)
     {
	if(strcmp(sycn[i].symbol, st) == 0)
              {
	      return i;
	}
     }
     }
	return -1;
}

int Addtoarr(char st[])
{
      if (index < max){
               strcpy(sycn[index].symbol, st);
               index++;
               return 0;
        }
        return -1;
}		
int Removearr(char st[])
{
       int i = 0, j = 0;
       char s[20];
       i = IndexOf(st);
        if(i == 0 || i  == max -1){
                 index--;
                return i;
        }
       else if(i != -1){
           for (j = i; j < index; j ++){
               strcpy(s, sycn[j].symbol);
               strcpy(sycn[j].symbol, sycn[j+1].symbol);
               strcpy(sycn[j+1].symbol, s);
           }
           index--;
           return i;
        }
       return -1;
}	

 