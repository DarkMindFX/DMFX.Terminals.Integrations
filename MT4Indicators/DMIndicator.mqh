//+------------------------------------------------------------------+
//|                                                  DMIndicator.mqh |
//|                                       Copyright 2016, DarkMindFX |
//|                                       https://www.darkmindfx.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, DarkMindFX"
#property link      "https://www.darkmindfx.com"
#property strict



#import PathDarkMindConnectDll
    int	    DarkMindInit(string host, string subscrKey, string folder);
    int	    DarkMindDeinit();
    int     DarkMindReadIndicator(string indicator, string code, int& seriesCount, int& datesCount);
    int	    DarkMindGetSeries(string indicator, string code, string seriesName, int& dates[], double& values[]);
    string  DarkMindGetSeriesName(string indicator, string code, int seriesIndex, int& error);
    string  DarkMindGetIndicatorName(string code, int& error);
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

void ReadIndicatorData()
{
    //Print(GetRootFolder());
    int error = DarkMindInit(SerivceHost, AccountKey, GetRootFolder());
    
    Print("DarkMindInit: " + error);
    if( error == ErrorSuccess )
    {
    
        int seriesCount = 0;
        int datesCount = 0;
        
        error = DarkMindReadIndicator(IndicatorName, IndicatorCode, seriesCount, datesCount);
        Print("DarkMindReadIndicator: " + error);
        if( error == ErrorSuccess )
        {
            Print("seriesCount = "+ seriesCount + ",  datesCount = " + datesCount);
            // preparing arrays
            SeriesCount = seriesCount;
            ArrayResize(Series, seriesCount);
            ArrayResize(DateTimeValues, datesCount);
            for(int i = 0; i < seriesCount; ++i)
            {
                string header = DarkMindGetSeriesName(IndicatorName, IndicatorCode, i, error);
                Print(header);
                ArrayResize(Series[i].Values, datesCount);
                Series[i].Name = header;
            }
            
            // reading series values
            for(int i = 0; i < SeriesCount; ++i) 
            {
                int dates[1];
                double values[1];
                ArrayResize(dates, datesCount);
                ArrayResize(values, datesCount);
                error = DarkMindGetSeries(IndicatorName, IndicatorCode, Series[i].Name, dates, values);
                if(error == ErrorSuccess)
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
                    
                }
                else
                {
                    HandleError(error);
                }
            }
        }
        else
        {
            HandleError(error);
        }
        
    }
    else
    {
        HandleError(error);
    }
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

string GetIndicatorName(string code)
{
    int error = 0;
    string name = DarkMindGetIndicatorName(IndicatorCode, error);
    Print("[GetIndicatorName] Code: " + code + "  Name: " + name);
    return name;
}

void InitBuffers()
{
//--- indicator buffers mapping
    ReadIndicatorData();
    
    IndicatorBuffers(SeriesCount);
    
    Print("Series Count: " + SeriesCount);
//--- indicator line
    for(int i = 0; i < SeriesCount; ++i)
    {
        SetIndexStyle(i, DRAW_LINE);
        SetIndexBuffer(i, Series[i].ExtMomBuffer);
        SetIndexLabel(i, Series[i].Name);
    }
//--- name for DataWindow and indicator subwindow label

    string short_name = IndicatorName;
    IndicatorShortName(short_name);
   
//--- check for input parameter
  

    SetIndexDrawBegin(0,0);
    
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

