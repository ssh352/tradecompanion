#include <windows.h>

#define ASTERIX 3.4e38

void __stdcall UpdatePercent(long Percent);
long __stdcall NSTSETUPDATA(LPSTR Ticker, LPSTR TickerDescription, float BarInterval, long NumColumns, LPSTR Category, double FirstDateAvailable);
long __stdcall NSTSETLABEL(LPSTR Ticker, float BarInterval, long Col, LPSTR Label);
long __stdcall NSTSETVALUE(LPSTR Ticker, float BarInterval, long Col, double Val);
long __stdcall NSTCOMMITROW(LPSTR Ticker, float BarInterval);
void __stdcall RequireUserIDPassword();
void __stdcall GetUserIDPassword(char **UserID, char **Password);

long Connected;
#pragma (.Myshare)
char MyVal[40] = " First vlaue";
#pragma Dataseg()

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
	MessageBox(NULL, temp, "Data Pump DLL Example", 0);

	return -1;
}

_declspec (dllexport) long RemoveTicker(char *ticker, float BarInterval)
{	// Stop Data Sink for this ticker/BarInterval combination
	//		there is no easy way to do this so we'll pretend that we removed the
	//      ticker/BarInterval combination from the 'DataSink' requests

	return TRUE;
}

_declspec (dllexport) long RequestTicker(char *ticker, char *description, char *category, double startdate, float BarInterval, char *ColumnNames)
{	long i;
	
	char *UserIDptr;
	char *Passwordptr;
	char UserID[21];
	char Password[21];

	double date;

	char *ColumnPtr1;
	char *ColumnPtr2;
	
	UserIDptr = &UserID[0];
	Passwordptr = &Password[0];

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
	
	//NSTSETLABEL(ticker, BarInterval, 1, "Date");
	//NSTSETLABEL(ticker, BarInterval, 2, "Open");
	//NSTSETLABEL(ticker, BarInterval, 3, "High");
	//NSTSETLABEL(ticker, BarInterval, 4, "Low");
	//NSTSETLABEL(ticker, BarInterval, 5, "Close");
	//NSTSETLABEL(ticker, BarInterval, 6, "Volume");
	

	// Download history
	for (i = 1; i <= 100; i++)
	{	date = startdate + (float) i * (BarInterval / (24 * 60));
		
		NSTSETVALUE(ticker, BarInterval, 1, date);		//date
		NSTSETVALUE(ticker, BarInterval, 2, i);			//open
		NSTSETVALUE(ticker, BarInterval, 3, i+0.5);		//high
		NSTSETVALUE(ticker, BarInterval, 4, i-0.5);		//low
		NSTSETVALUE(ticker, BarInterval, 5, i);			//close
		NSTSETVALUE(ticker, BarInterval, 6, i * 100);	//volume

		NSTCOMMITROW(ticker, BarInterval);

		UpdatePercent(i);
	}

	// Start Data Sink
	//		there is no easy way to do this so we'll pretend that we set the
	//      'DataSink' procedure up as a data sink

	return TRUE;
}

// This is an example of how a DataSink might work.
//

_declspec (dllexport) long DataSink(char *ticker, long BarInterval, double date_time, float open, float high, float low, float close, float volume)
{	NSTSETVALUE(ticker, BarInterval, 1, date_time);
	NSTSETVALUE(ticker, BarInterval, 2, open);
	NSTSETVALUE(ticker, BarInterval, 3, high);
	NSTSETVALUE(ticker, BarInterval, 4, low);
	NSTSETVALUE(ticker, BarInterval, 5, close);
	NSTSETVALUE(ticker, BarInterval, 6, volume);

	NSTCOMMITROW(ticker, BarInterval);
}

_declspec (dllexport) void SetData(char tickPrice[])
{
	strcpy(MyVal, tickPrice);
}