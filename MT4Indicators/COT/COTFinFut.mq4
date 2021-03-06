//+-------------------------------------------------------------------------+
//|                                       COTFinFutNetPositions.mq4         |
//|                                       Copyright 2016 - 2020, DarkMindFX |
//|                                       https://www.darkmindfx.com        |
//+-------------------------------------------------------------------------+
#property copyright "Copyright 2016 - 2020, DarkMindFX"
#property link      "https://www.darkmindfx.com"
#property description "COT: Financials - Futures Only"
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
   COT0	 = 0	,	 //	CANADIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE
   COT1	 = 1	,	 //	SWISS FRANC - CHICAGO MERCANTILE EXCHANGE
   COT2	 = 2	,	 //	BRITISH POUND STERLING - CHICAGO MERCANTILE EXCHANGE
   COT3	 = 3	,	 //	JAPANESE YEN - CHICAGO MERCANTILE EXCHANGE
   COT4	 = 4	,	 //	EURO FX - CHICAGO MERCANTILE EXCHANGE
   COT5	 = 5	,	 //	AUSTRALIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE
   COT6	 = 6	,	 //	EURO FX/BRITISH POUND XRATE - CHICAGO MERCANTILE EXCHANGE
   COT7	 = 7	,	 //	EURO FX/JAPANESE YEN XRATE - CHICAGO MERCANTILE EXCHANGE
   COT8	 = 8	,	 //	RUSSIAN RUBLE - CHICAGO MERCANTILE EXCHANGE
   COT9	 = 9	,	 //	MEXICAN PESO - CHICAGO MERCANTILE EXCHANGE
   COT10	 = 10	,	 //	BRAZILIAN REAL - CHICAGO MERCANTILE EXCHANGE
   COT11	 = 11	,	 //	NEW ZEALAND DOLLAR - CHICAGO MERCANTILE EXCHANGE
   COT12	 = 12	,	 //	SOUTH AFRICAN RAND - CHICAGO MERCANTILE EXCHANGE
   COT13	 = 13	,	 //	DJIA Consolidated - CHICAGO BOARD OF TRADE
   COT14	 = 14	,	 //	DOW JONES INDUSTRIAL AVG- x $5 - CHICAGO BOARD OF TRADE
   COT15	 = 15	,	 //	DOW JONES U.S. REAL ESTATE IDX - CHICAGO BOARD OF TRADE
   COT16	 = 16	,	 //	S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE
   COT17	 = 17	,	 //	S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE
   COT18	 = 18	,	 //	E-MINI S&P CONSU STAPLES INDEX - CHICAGO MERCANTILE EXCHANGE
   COT19	 = 19	,	 //	E-MINI S&P ENERGY INDEX - CHICAGO MERCANTILE EXCHANGE
   COT20	 = 20	,	 //	E-MINI S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE
   COT21	 = 21	,	 //	E-MINI S&P FINANCIAL INDEX - CHICAGO MERCANTILE EXCHANGE
   COT22	 = 22	,	 //	E-MINI S&P HEALTH CARE INDEX - CHICAGO MERCANTILE EXCHANGE
   COT23	 = 23	,	 //	E-MINI S&P INDUSTRIAL INDEX - CHICAGO MERCANTILE EXCHANGE
   COT24	 = 24	,	 //	E-MINI S&P MATERIALS INDEX - CHICAGO MERCANTILE EXCHANGE
   COT25	 = 25	,	 //	E-MINI S&P TECHNOLOGY INDEX - CHICAGO MERCANTILE EXCHANGE
   COT26	 = 26	,	 //	E-MINI S&P UTILITIES INDEX - CHICAGO MERCANTILE EXCHANGE
   COT27	 = 27	,	 //	S&P 500 TOTAL RETURN INDEX - CHICAGO MERCANTILE EXCHANGE
   COT28	 = 28	,	 //	NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE
   COT29	 = 29	,	 //	NASDAQ-100 STOCK INDEX (MINI) - CHICAGO MERCANTILE EXCHANGE
   COT30	 = 30	,	 //	E-MINI RUSSELL 2000 INDEX - CHICAGO MERCANTILE EXCHANGE
   COT31	 = 31	,	 //	NIKKEI STOCK AVERAGE - CHICAGO MERCANTILE EXCHANGE
   COT32	 = 32	,	 //	NIKKEI STOCK AVERAGE YEN DENOM - CHICAGO MERCANTILE EXCHANGE
   COT33	 = 33	,	 //	MSCI EAFE MINI INDEX - ICE FUTURES U.S.
   COT34	 = 34	,	 //	MSCI EMERGING MKTS MINI INDEX - ICE FUTURES U.S.
   COT35	 = 35	,	 //	E-MINI S&P 400 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE
   COT36	 = 36	,	 //	S&P 500 ANNUAL DIVIDEND INDEX - CHICAGO MERCANTILE EXCHANGE
   COT37	 = 37	,	 //	U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE
   COT38	 = 38	,	 //	ULTRA U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE
   COT39	 = 39	,	 //	2-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE
   COT40	 = 40	,	 //	10-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE
   COT41	 = 41	,	 //	ULTRA 10-YEAR U.S. T-NOTES - CHICAGO BOARD OF TRADE
   COT42	 = 42	,	 //	5-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE
   COT43	 = 43	,	 //	30-DAY FEDERAL FUNDS - CHICAGO BOARD OF TRADE
   COT44	 = 44	,	 //	3-MONTH EURODOLLARS - CHICAGO MERCANTILE EXCHANGE
   COT45	 = 45	,	 //	3-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE
   COT46	 = 46	,	 //	1-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE
   COT47	 = 47	,	 //	10 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE
   COT48	 = 48	,	 //	5 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE
   COT49	 = 49	,	 //	10 YEAR ERIS SWAP  - CHICAGO BOARD OF TRADE
   COT50	 = 50	,	 //	5 YEAR ERIS SWAP - CHICAGO BOARD OF TRADE
   COT51	 = 51	,	 //	BITCOIN-USD - CBOE FUTURES EXCHANGE
   COT52	 = 52	,	 //	BITCOIN - CHICAGO MERCANTILE EXCHANGE
   COT53	 = 53	,	 //	U.S. DOLLAR INDEX - ICE FUTURES U.S.
   COT54	 = 54	,	 //	VIX FUTURES - CBOE FUTURES EXCHANGE
   COT55	 = 55	,	 //	BLOOMBERG COMMODITY INDEX - CHICAGO BOARD OF TRADE
   COT56	 = 56	,	 //	E-MINI S&P CONSUMER DISC INDEX - CHICAGO MERCANTILE EXCHANGE
   COT57	 = 57	,	 //	EMINI RUSSELL 1000 VALUE INDEX - CHICAGO MERCANTILE EXCHANGE
   COT58	 = 58	,	 //	RUSSELL 2000 MINI INDEX FUTURE - ICE FUTURES U.S.
   COT59	 = 59	,	 //	5-Year Eris Standard- Initial - ERIS FUTURES EXCHANGE
   COT60	 = 60	,	 //	RUSSELL 1000 VALUE INDEX MINI - ICE FUTURES U.S.
   COT61	 = 61	,	 //	S&P GSCI COMMODITY INDEX - CHICAGO MERCANTILE EXCHANGE
   COT62	 = 62	,	 //	EURO FX/JAPANESE YEN - SMALL - ICE FUTURES U.S.
   COT63	 = 63	,	 //	EURO FX/BRITISH POUND - SMALL - ICE FUTURES U.S.
   COT64	 = 64	,	 //	DOW JONES INDUSTRIAL AVERAGE - CHICAGO BOARD OF TRADE
   COT65	 = 65	,	 //	NASDAQ-100 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE
   COT66	 = 66	,	 //	Russell 2000 Stock Index Future - Chicago Mercantile Exchange
   COT67	 = 67	,	 //	RUSSEL 1000 MINI INDEX FUTURE - ICE FUTURES U.S.
   COT68	 = 68	,	 //	Russell 2000 Stock Index - ICE Futures U.S.
   COT69	 = 69	,	 //	E-MINI MSCI EAFE - CHICAGO MERCANTILE EXCHANGE
   COT70	 = 70	,	 //	E-MINI MSCI EMERGING MARKETS - CHICAGO MERCANTILE EXCHANGE
   COT71	 = 71	,	 //	S&P 400 MIDCAP STOCK INDEX - CHICAGO MERCANTILE EXCHANGE
   COT72	 = 72	,	 //	DTCC Repo-US Treasury <30 YR - NYSE LIFFE - NYPC
   COT73	 = 73	,	 //	ONE-MONTH EURODOLLAR - CHICAGO MERCANTILE EXCHANGE
   COT74	 = 74	,	 //	INTEREST RATE SWAPS 10YR - CHICAGO BOARD OF TRADE
   COT75	 = 75	,	 //	INTEREST RATE SWAPS 5YR - CHICAGO BOARD OF TRADE
   COT76	 = 76		 //	3-MO. EUROYEN TIBOR - CHICAGO MERCANTILE EXCHANGE
};

string codes[][2] = 
{
   {	"COT.FinFut.090741",		"CANADIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.092741",		"SWISS FRANC - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.096742",		"BRITISH POUND STERLING - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.097741",		"JAPANESE YEN - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.099741",		"EURO FX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.232741",		"AUSTRALIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.299741",		"EURO FX/BRITISH POUND XRATE - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.399741",		"EURO FX/JAPANESE YEN XRATE - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.089741",		"RUSSIAN RUBLE - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.095741",		"MEXICAN PESO - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.102741",		"BRAZILIAN REAL - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.112741",		"NEW ZEALAND DOLLAR - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.122741",		"SOUTH AFRICAN RAND - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.12460+",		"DJIA Consolidated - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.124603",		"DOW JONES INDUSTRIAL AVG- x $5 - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.124606",		"DOW JONES U.S. REAL ESTATE IDX - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.13874+",		"S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.138741",		"S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.138748",		"E-MINI S&P CONSU STAPLES INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.138749",		"E-MINI S&P ENERGY INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874A",		"E-MINI S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874C",		"E-MINI S&P FINANCIAL INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874E",		"E-MINI S&P HEALTH CARE INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874F",		"E-MINI S&P INDUSTRIAL INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874H",		"E-MINI S&P MATERIALS INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874I",		"E-MINI S&P TECHNOLOGY INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874J",		"E-MINI S&P UTILITIES INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.13874N",		"S&P 500 TOTAL RETURN INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.20974+",		"NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.209742",		"NASDAQ-100 STOCK INDEX (MINI) - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.239742",		"E-MINI RUSSELL 2000 INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.240741",		"NIKKEI STOCK AVERAGE - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.240743",		"NIKKEI STOCK AVERAGE YEN DENOM - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.244041",		"MSCI EAFE MINI INDEX - ICE FUTURES U.S."	},
   {	"COT.FinFut.244042",		"MSCI EMERGING MKTS MINI INDEX - ICE FUTURES U.S."	},
   {	"COT.FinFut.33874A",		"E-MINI S&P 400 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.43874A",		"S&P 500 ANNUAL DIVIDEND INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.020601",		"U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.020604",		"ULTRA U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.042601",		"2-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.043602",		"10-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.043607",		"ULTRA 10-YEAR U.S. T-NOTES - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.044601",		"5-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.045601",		"30-DAY FEDERAL FUNDS - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.132741",		"3-MONTH EURODOLLARS - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.134741",		"3-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.134742",		"1-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.246605",		"10 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.246606",		"5 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.343601",		"10 YEAR ERIS SWAP  - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.344601",		"5 YEAR ERIS SWAP - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.1330E1",		"BITCOIN-USD - CBOE FUTURES EXCHANGE"	},
   {	"COT.FinFut.133741",		"BITCOIN - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.098662",		"U.S. DOLLAR INDEX - ICE FUTURES U.S."	},
   {	"COT.FinFut.1170E1",		"VIX FUTURES - CBOE FUTURES EXCHANGE"	},
   {	"COT.FinFut.221602",		"BLOOMBERG COMMODITY INDEX - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.138747",		"E-MINI S&P CONSUMER DISC INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.239744",		"EMINI RUSSELL 1000 VALUE INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.23977A",		"RUSSELL 2000 MINI INDEX FUTURE - ICE FUTURES U.S."	},
   {	"COT.FinFut.ZB9105",		"5-Year Eris Standard- Initial - ERIS FUTURES EXCHANGE"	},
   {	"COT.FinFut.23977C",		"RUSSELL 1000 VALUE INDEX MINI - ICE FUTURES U.S."	},
   {	"COT.FinFut.256741",		"S&P GSCI COMMODITY INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.299661",		"EURO FX/JAPANESE YEN - SMALL - ICE FUTURES U.S."	},
   {	"COT.FinFut.599661",		"EURO FX/BRITISH POUND - SMALL - ICE FUTURES U.S."	},
   {	"COT.FinFut.124601",		"DOW JONES INDUSTRIAL AVERAGE - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.209741",		"NASDAQ-100 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.239741",		"Russell 2000 Stock Index Future - Chicago Mercantile Exchange"	},
   {	"COT.FinFut.239772",		"RUSSEL 1000 MINI INDEX FUTURE - ICE FUTURES U.S."	},
   {	"COT.FinFut.239777",		"Russell 2000 Stock Index - ICE Futures U.S."	},
   {	"COT.FinFut.244741",		"E-MINI MSCI EAFE - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.244742",		"E-MINI MSCI EMERGING MARKETS - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.338741",		"S&P 400 MIDCAP STOCK INDEX - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.045L2T",		"DTCC Repo-US Treasury <30 YR - NYSE LIFFE - NYPC"	},
   {	"COT.FinFut.032741",		"ONE-MONTH EURODOLLAR - CHICAGO MERCANTILE EXCHANGE"	},
   {	"COT.FinFut.246602",		"INTEREST RATE SWAPS 10YR - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.247602",		"INTEREST RATE SWAPS 5YR - CHICAGO BOARD OF TRADE"	},
   {	"COT.FinFut.597741",		"3-MO. EUROYEN TIBOR - CHICAGO MERCANTILE EXCHANGE"	}
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

