//+-------------------------------------------------------------------------+
//|                                       COTFinFutNetPositions.mq4         |
//|                                       Copyright 2016 - 2020, DarkMindFX |
//|                                       https://www.darkmindfx.com        |
//+-------------------------------------------------------------------------+
#property copyright "Copyright 2016 - 2020, DarkMindFX"
#property link      "https://www.darkmindfx.com"
#property description "COT: Financials - Futures Only - Net Positions"
#property strict

#property indicator_level1 0

#property indicator_separate_window
#property indicator_buffers 10
#property indicator_color1 Red
#property indicator_color2 Green
#property indicator_color3 Blue
#property indicator_color4 DarkBlue
#property indicator_color5 Yellow
#property indicator_color6 Aqua
#property indicator_color7 LawnGreen
#property indicator_color8 HotPink
#property indicator_color9 DarkBlue


#import "..\DarkMindConnect.dll"
	int DMFXAccountsInitSession(string host, string accountKey, string folder);
	int DMFXAccountsCloseSession();
	int DMFXTimeseriesGetTimeSeriesInfo(string country, string ticker);
	int DMFXTimeseriesGetTimeSeries(string country, string ticker, int timeframe, int periodStart, int periodEnd);
	
	int GetTimeSeriesCount(string ticker);	
	string GetTimeSeriesName(string ticker, int index);
	int GetTimeSeriesDatesCount(string ticker, string tsName);
	int GetTimeSeriesValues(string ticker, int index, int& dates[], double& values[]);
	string GetLastErrorMessage();
#import

#include "..\DMFX.Indicator.Common.mqh"


enum COTCodes 
{
   COT0	 = 0	,	 //	CANADIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT1	 = 1	,	 //	SWISS FRANC - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT2	 = 2	,	 //	BRITISH POUND STERLING - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT3	 = 3	,	 //	JAPANESE YEN - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT4	 = 4	,	 //	EURO FX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT5	 = 5	,	 //	AUSTRALIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT6	 = 6	,	 //	EURO FX/BRITISH POUND XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT7	 = 7	,	 //	EURO FX/JAPANESE YEN XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT8	 = 8	,	 //	RUSSIAN RUBLE - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT9	 = 9	,	 //	MEXICAN PESO - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT10	 = 10	,	 //	BRAZILIAN REAL - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT11	 = 11	,	 //	NEW ZEALAND DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT12	 = 12	,	 //	SOUTH AFRICAN RAND - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT13	 = 13	,	 //	DJIA Consolidated - CHICAGO BOARD OF TRADE (Net Positions)
   COT14	 = 14	,	 //	DOW JONES INDUSTRIAL AVG- x $5 - CHICAGO BOARD OF TRADE (Net Positions)
   COT15	 = 15	,	 //	DOW JONES U.S. REAL ESTATE IDX - CHICAGO BOARD OF TRADE (Net Positions)
   COT16	 = 16	,	 //	S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT17	 = 17	,	 //	S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT18	 = 18	,	 //	E-MINI S&P CONSU STAPLES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT19	 = 19	,	 //	E-MINI S&P ENERGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT20	 = 20	,	 //	E-MINI S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT21	 = 21	,	 //	E-MINI S&P FINANCIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT22	 = 22	,	 //	E-MINI S&P HEALTH CARE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT23	 = 23	,	 //	E-MINI S&P INDUSTRIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT24	 = 24	,	 //	E-MINI S&P MATERIALS INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT25	 = 25	,	 //	E-MINI S&P TECHNOLOGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT26	 = 26	,	 //	E-MINI S&P UTILITIES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT27	 = 27	,	 //	S&P 500 TOTAL RETURN INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT28	 = 28	,	 //	NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT29	 = 29	,	 //	NASDAQ-100 STOCK INDEX (MINI) - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT30	 = 30	,	 //	E-MINI RUSSELL 2000 INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT31	 = 31	,	 //	NIKKEI STOCK AVERAGE - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT32	 = 32	,	 //	NIKKEI STOCK AVERAGE YEN DENOM - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT33	 = 33	,	 //	MSCI EAFE MINI INDEX - ICE FUTURES U.S. (Net Positions)
   COT34	 = 34	,	 //	MSCI EMERGING MKTS MINI INDEX - ICE FUTURES U.S. (Net Positions)
   COT35	 = 35	,	 //	E-MINI S&P 400 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT36	 = 36	,	 //	S&P 500 ANNUAL DIVIDEND INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT37	 = 37	,	 //	U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)
   COT38	 = 38	,	 //	ULTRA U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)
   COT39	 = 39	,	 //	2-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)
   COT40	 = 40	,	 //	10-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)
   COT41	 = 41	,	 //	ULTRA 10-YEAR U.S. T-NOTES - CHICAGO BOARD OF TRADE (Net Positions)
   COT42	 = 42	,	 //	5-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)
   COT43	 = 43	,	 //	30-DAY FEDERAL FUNDS - CHICAGO BOARD OF TRADE (Net Positions)
   COT44	 = 44	,	 //	3-MONTH EURODOLLARS - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT45	 = 45	,	 //	3-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT46	 = 46	,	 //	1-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT47	 = 47	,	 //	10 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)
   COT48	 = 48	,	 //	5 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)
   COT49	 = 49	,	 //	10 YEAR ERIS SWAP  - CHICAGO BOARD OF TRADE (Net Positions)
   COT50	 = 50	,	 //	5 YEAR ERIS SWAP - CHICAGO BOARD OF TRADE (Net Positions)
   COT51	 = 51	,	 //	BITCOIN-USD - CBOE FUTURES EXCHANGE (Net Positions)
   COT52	 = 52	,	 //	BITCOIN - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT53	 = 53	,	 //	U.S. DOLLAR INDEX - ICE FUTURES U.S. (Net Positions)
   COT54	 = 54	,	 //	VIX FUTURES - CBOE FUTURES EXCHANGE (Net Positions)
   COT55	 = 55	,	 //	BLOOMBERG COMMODITY INDEX - CHICAGO BOARD OF TRADE (Net Positions)
   COT56	 = 56	,	 //	E-MINI S&P CONSUMER DISC INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT57	 = 57	,	 //	EMINI RUSSELL 1000 VALUE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT58	 = 58	,	 //	RUSSELL 2000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)
   COT59	 = 59	,	 //	5-Year Eris Standard- Initial - ERIS FUTURES EXCHANGE (Net Positions)
   COT60	 = 60	,	 //	RUSSELL 1000 VALUE INDEX MINI - ICE FUTURES U.S. (Net Positions)
   COT61	 = 61	,	 //	S&P GSCI COMMODITY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT62	 = 62	,	 //	EURO FX/JAPANESE YEN - SMALL - ICE FUTURES U.S. (Net Positions)
   COT63	 = 63	,	 //	EURO FX/BRITISH POUND - SMALL - ICE FUTURES U.S. (Net Positions)
   COT64	 = 64	,	 //	DOW JONES INDUSTRIAL AVERAGE - CHICAGO BOARD OF TRADE (Net Positions)
   COT65	 = 65	,	 //	NASDAQ-100 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT66	 = 66	,	 //	Russell 2000 Stock Index Future - Chicago Mercantile Exchange (Net Positions)
   COT67	 = 67	,	 //	RUSSEL 1000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)
   COT68	 = 68	,	 //	Russell 2000 Stock Index - ICE Futures U.S. (Net Positions)
   COT69	 = 69	,	 //	E-MINI MSCI EAFE - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT70	 = 70	,	 //	E-MINI MSCI EMERGING MARKETS - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT71	 = 71	,	 //	S&P 400 MIDCAP STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT72	 = 72	,	 //	DTCC Repo-US Treasury <30 YR - NYSE LIFFE - NYPC (Net Positions)
   COT73	 = 73	,	 //	ONE-MONTH EURODOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)
   COT74	 = 74	,	 //	INTEREST RATE SWAPS 10YR - CHICAGO BOARD OF TRADE (Net Positions)
   COT75	 = 75	,	 //	INTEREST RATE SWAPS 5YR - CHICAGO BOARD OF TRADE (Net Positions)
   COT76	 = 76		 //	3-MO. EUROYEN TIBOR - CHICAGO MERCANTILE EXCHANGE (Net Positions)
};

string codes[][2] = 
{
   {	"COT.FinFut.090741.Net",		"CANADIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.092741.Net",		"SWISS FRANC - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.096742.Net",		"BRITISH POUND STERLING - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.097741.Net",		"JAPANESE YEN - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.099741.Net",		"EURO FX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.232741.Net",		"AUSTRALIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.299741.Net",		"EURO FX/BRITISH POUND XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.399741.Net",		"EURO FX/JAPANESE YEN XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.089741.Net",		"RUSSIAN RUBLE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.095741.Net",		"MEXICAN PESO - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.102741.Net",		"BRAZILIAN REAL - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.112741.Net",		"NEW ZEALAND DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.122741.Net",		"SOUTH AFRICAN RAND - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.12460+.Net",		"DJIA Consolidated - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.124603.Net",		"DOW JONES INDUSTRIAL AVG- x $5 - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.124606.Net",		"DOW JONES U.S. REAL ESTATE IDX - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.13874+.Net",		"S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.138741.Net",		"S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.138748.Net",		"E-MINI S&P CONSU STAPLES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.138749.Net",		"E-MINI S&P ENERGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874A.Net",		"E-MINI S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874C.Net",		"E-MINI S&P FINANCIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874E.Net",		"E-MINI S&P HEALTH CARE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874F.Net",		"E-MINI S&P INDUSTRIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874H.Net",		"E-MINI S&P MATERIALS INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874I.Net",		"E-MINI S&P TECHNOLOGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874J.Net",		"E-MINI S&P UTILITIES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.13874N.Net",		"S&P 500 TOTAL RETURN INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.20974+.Net",		"NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.209742.Net",		"NASDAQ-100 STOCK INDEX (MINI) - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.239742.Net",		"E-MINI RUSSELL 2000 INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.240741.Net",		"NIKKEI STOCK AVERAGE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.240743.Net",		"NIKKEI STOCK AVERAGE YEN DENOM - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.244041.Net",		"MSCI EAFE MINI INDEX - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.244042.Net",		"MSCI EMERGING MKTS MINI INDEX - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.33874A.Net",		"E-MINI S&P 400 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.43874A.Net",		"S&P 500 ANNUAL DIVIDEND INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.020601.Net",		"U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.020604.Net",		"ULTRA U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.042601.Net",		"2-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.043602.Net",		"10-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.043607.Net",		"ULTRA 10-YEAR U.S. T-NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.044601.Net",		"5-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.045601.Net",		"30-DAY FEDERAL FUNDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.132741.Net",		"3-MONTH EURODOLLARS - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.134741.Net",		"3-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.134742.Net",		"1-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.246605.Net",		"10 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.246606.Net",		"5 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.343601.Net",		"10 YEAR ERIS SWAP  - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.344601.Net",		"5 YEAR ERIS SWAP - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.1330E1.Net",		"BITCOIN-USD - CBOE FUTURES EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.133741.Net",		"BITCOIN - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.098662.Net",		"U.S. DOLLAR INDEX - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.1170E1.Net",		"VIX FUTURES - CBOE FUTURES EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.221602.Net",		"BLOOMBERG COMMODITY INDEX - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.138747.Net",		"E-MINI S&P CONSUMER DISC INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.239744.Net",		"EMINI RUSSELL 1000 VALUE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.23977A.Net",		"RUSSELL 2000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.ZB9105.Net",		"5-Year Eris Standard- Initial - ERIS FUTURES EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.23977C.Net",		"RUSSELL 1000 VALUE INDEX MINI - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.256741.Net",		"S&P GSCI COMMODITY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.299661.Net",		"EURO FX/JAPANESE YEN - SMALL - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.599661.Net",		"EURO FX/BRITISH POUND - SMALL - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.124601.Net",		"DOW JONES INDUSTRIAL AVERAGE - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.209741.Net",		"NASDAQ-100 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.239741.Net",		"Russell 2000 Stock Index Future - Chicago Mercantile Exchange (Net Positions)"	},
   {	"COT.FinFut.239772.Net",		"RUSSEL 1000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)"	},
   {	"COT.FinFut.239777.Net",		"Russell 2000 Stock Index - ICE Futures U.S. (Net Positions)"	},
   {	"COT.FinFut.244741.Net",		"E-MINI MSCI EAFE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.244742.Net",		"E-MINI MSCI EMERGING MARKETS - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.338741.Net",		"S&P 400 MIDCAP STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.045L2T.Net",		"DTCC Repo-US Treasury <30 YR - NYSE LIFFE - NYPC (Net Positions)"	},
   {	"COT.FinFut.032741.Net",		"ONE-MONTH EURODOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
   {	"COT.FinFut.246602.Net",		"INTEREST RATE SWAPS 10YR - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.247602.Net",		"INTEREST RATE SWAPS 5YR - CHICAGO BOARD OF TRADE (Net Positions)"	},
   {	"COT.FinFut.597741.Net",		"3-MO. EUROYEN TIBOR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	}
};

class COTMarketSymbol
{
public:
    string Name;
    string Code;
};

COTMarketSymbol Symbols[];
string COTNamePrefix = "COT: Financials - Futures Only";

input string   AccountKey = "[Your Account Key]";
input COTCodes COTMarketCode = 4;
input bool     PrintLog = true;
input string   DataSource = "http://darkmindfx.com/";


//+------------------------------------------------------------------+
//| Custom indicator initialization function                         |
//+------------------------------------------------------------------+


void InitSymbolsCollection()
{
    
    int count = ArraySize(codes)/2;
    
    ArrayResize(Symbols, count);
    for(int i = 0; i < count; ++i)
    {
        Symbols[i].Code = codes[i][0];
        Symbols[i].Name = codes[i][1];
 
    }  
  
}

string GetNameByCode(string code)
{
    string result = "";
    
    for(int i = 0; i < ArraySize(Symbols) && StringLen(result) == 0; ++i)
    {
        if(Symbols[i].Code == code)
        {            
            result = Symbols[i].Name;
        }
    }
    
    return result;
}


int OnInit(void)
{
   Log = PrintLog;
   Timeframe = Weekly;
   int result = InitSession(AccountKey);

   if(result != 0)
	{
	   if(PrintLog) Print(GetLastErrorMessage());	   
	}
	else
	{
	   if(PrintLog) Print("Session for " + AccountKey + " initiated");
	   InitSymbolsCollection();
	   Ticker = codes[COTMarketCode][0];
	   Name = GetNameByCode(Ticker);
	   
	   IndicatorSetString(INDICATOR_SHORTNAME, COTNamePrefix + ": " + Name);
	   
	   if(PrintLog) Print("Init: " + Ticker + ", " + Name);	
	   
	   result = GetIndicatorInfo(); 
	   if(result == 0)
	   {
	      result = InitBuffers();
	      if(result == 0)
	      {
	         result = ReadIndicatorData();
	         if(result != 0)
	         {
	            HandleError("Data was not read - " + result);
	         }
	      }
	      else
	      {
	         HandleError("Buffers not initialized - " + result);
	      }   
	   }   
	   
	}
   return 0;
}

void OnDeinit(const int reason)
{
}
//+------------------------------------------------------------------+
//| Custom indicator iteration function                              |
//+------------------------------------------------------------------+
int OnCalculate(const int rates_total,
                const int prev_calculated,
                const datetime &time[],
                const double &open[],
                const double &high[],
                const double &low[],
                const double &close[],
                const long &tick_volume[],
                const long &volume[],
                const int &spread[])
{
//---
   DrawBuffer(rates_total, prev_calculated, time);
//--- return value of prev_calculated for next call
   return(rates_total);

}

