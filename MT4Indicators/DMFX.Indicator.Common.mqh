//+-------------------------------------------------------------------------+
//|                                                  DMIndicator.mqh        |
//|                                       Copyright 2016 - 2020, DarkMindFX |
//|                                       https://www.darkmindfx.com        |
//+-------------------------------------------------------------------------+
#property copyright "Copyright 2016 - 2020, DarkMindFX"
#property link      "https://www.darkmindfx.com"
#property strict




enum ETimeFrame
{
   Daily = 1,
   Weekly,
   Monthly,
   Quarterly,
   Annually
};

//--- buffers
int         MaxColumnCount = 10;
string      Country = "US";
string      Ticker;
string      Name;
int         Timeframe = Weekly;

bool        Log = false;

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

void HandleError(int error)
{
    if(Log) Print(" [!] Error: " + error);
}

string GetRootFolder()
{
    return TerminalInfoString(TERMINAL_DATA_PATH) + (IsTesting() ? "\\tester\\Files\\" : "\\MQL4\\Files\\");
}

int InitSession(string accountKey)
{
	string rootFolder = GetRootFolder();
	int result = DMFXAccountsInitSession("http://localhost/", accountKey, rootFolder);
	
	if(Log) Print("DMFXAccountsInitSession: result = " + result);
	
	return result;
	
}

int InitBuffers()
{
   int length = ArraySize(Series);
   if(length > 0)
   {
      datetime dtNow = TimeCurrent();
      int result = DMFXTimeseriesGetTimeSeries(Country, Ticker, Timeframe, 0, dtNow);
      
      if(result == 0)
      {
         for(int i = 0; i < length; ++i)
         {
            int datesCount = GetTimeSeriesDatesCount(Ticker, Series[i].Name);
            if(datesCount > 0)
            {
               if(ArraySize(DateTimeValues) <= 0 || ArraySize(DateTimeValues) < datesCount)
               {
                  ArrayResize(DateTimeValues, datesCount);
               }
               
               SetIndexStyle(i, DRAW_LINE);
               SetIndexBuffer(i, Series[i].ExtMomBuffer);
               SetIndexLabel(i, Series[i].Name);
               
               ArrayResize(Series[i].ExtMomBuffer, datesCount);
               ArrayResize(Series[i].Values, datesCount);
               
               if(Log) Print(i + " - " + Series[i].Name + " buffer initialized");
            }
         }
      }
      else
      {
         if(Log) Print("DMFXTimeseriesGetTimeSeries failed - " + result);
      }
      
      SetIndexDrawBegin(0,0);
      
      return 0;
   }   
   else
   {   
      if(Log) Print("Series array not initialized");
      return 9999;
   }
}



int GetIndicatorInfo()
{
   int result = DMFXTimeseriesGetTimeSeriesInfo(Country, Ticker);
   if(result != 0)
   {
      if(Log) Print(GetLastErrorMessage());
   }
   else
   {
      SeriesCount = GetTimeSeriesCount(Ticker);
      if(Log) Print("Timeseries count: " + SeriesCount);
      
      
      ArrayResize(Series, SeriesCount);
      for(int i = 0; i < SeriesCount; ++i)
      {
         string name = GetTimeSeriesName(Ticker, i);
         if(name != 0)
         {
            Series[i].Name = name;
            if(Log) Print("Timeseries: " + Series[i].Name);
         }
         else
         {
            if(Log) Print("Null returned for ts index " + i);
            return 9999;
         }
      }
   }
	   
	return result;   
}

int ReadIndicatorData()
{
     
   int result = 0;
   
   int datesCount = ArraySize(DateTimeValues);
   for(int i = 0; i < SeriesCount; ++i)
   {
         CSeries series = Series[i];
         
         if(Log) Print("Reading data for " + series.Name);
         
         int dates[1];
         double values[1];
         ArrayResize(dates, datesCount);
         ArrayResize(values, datesCount);
         result = GetTimeSeriesValues(Ticker, i, dates, values);
         if(result == 0)
         {
            for(int s = 0; s < datesCount; ++s)
            {                       
                  datetime dt = dates[s];
                  double value = values[s];
                  
                  if(DateTimeValues[s] == 0)
                  {
                     DateTimeValues[s] = dt;
                  }
                  Series[i].Values[s] = value; 
                  
            }
            
            if(Log) Print(Series[i].Name + ": data read");
         }
         else
         {
            if(Log) Print("GetTimeSeriesValues failed for " + series.Name);
            HandleError(result);
            return result;
         }
   }
  
   return result;   
}

int GetDateIndex(datetime dt)
{
    int size = ArraySize(DateTimeValues);
    int index = -1;
    for(int i = 0; i < size && index == -1; ++i)
    {
        
        if(DateTimeValues[i] <= dt && (i + 1 >= size || dt < DateTimeValues[i+1]) )
        {
            index = i;
        }
    }
    
    return index;
}

void DrawBuffer(const int rates_total,
                const int prev_calculated,
                const datetime &time[])
{
    for(int i = 0; i < SeriesCount; ++i)
    {
        ArraySetAsSeries(Series[i].ExtMomBuffer, false);
    }
    
    for(int i = prev_calculated; i < rates_total; ++i)
    {
        datetime dt = time[i];
      
        int index = GetDateIndex(dt);
        
        if(index != -1)
        {
            for(int s = 0; s < SeriesCount; ++s)
            {
                //Print(Series[s].Values[index]);
                Series[s].ExtMomBuffer[rates_total-i-1] = Series[s].Values[index];
            }
        }
        
    }   
}





