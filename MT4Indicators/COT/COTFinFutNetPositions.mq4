//+-------------------------------------------------------------------------+
//|                                       COTFinFutNetPositions.mq4         |
//|                                       Copyright 2016 - 2020, DarkMindFX |
//|                                       https://www.darkmindfx.com        |
//+-------------------------------------------------------------------------+
#property copyright "Copyright 2016 - 2020, DarkMindFX"
#property link      "https://www.darkmindfx.com"
#property strict

#define     PathDarkMindConnectDll "..\DarkMindConnect.dll"

#import PathDarkMindConnectDll
	int DMFXAccountsInitSession(string host, string accountKey, string folder);
#import

//--- buffers
int         MaxColumnCount = 10;


class CSeries
{
public:
     double ExtMomBuffer[];
     double Values[];
     string Name;
};

datetime    DateTimeValues[];
CSeries     Series[];
int         SeriesCount = 0;

int ErrorSuccess = 0;
int ErrorIndicatorNotFound = 1;
int ErrorIndicatorSeriesNotFound = 2;
int ErrorSubscriptionNotValid = 3;
int ErrorSubscriptionExpired = 4;
int ErrorUnknownError = 10000;
//+------------------------------------------------------------------+
//| Custom indicator initialization function                         |
//+------------------------------------------------------------------+
void HandleError(int error)
{
    Print(" [!] Error: " + error);
}

string GetRootFolder()
{
    return TerminalInfoString(TERMINAL_DATA_PATH) + (IsTesting() ? "\\tester\\Files\\" : "\\MQL4\\Files\\");
}

void InitSession(string accountKey)
{
	string rootFolder = GetRootFolder();
	int result = DMFXAccountsInitSession(accountKey, "http://localhost/api/accounts/")
	Print("DMFXAccountsInitSession: result = " + result);
}
