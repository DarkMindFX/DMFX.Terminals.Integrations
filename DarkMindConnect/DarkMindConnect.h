// DarkMindConnect.h

#pragma once

#include "StringDefs.h"

#ifdef _WINDLL
    #define MT4ACCESS __declspec(dllexport) 
#else
    #define MT4ACCESS __declspec(dllimport) 
#endif

// /api/accounts/InitSession
MT4ACCESS int DMFXAccountsInitSession(MqlString host, MqlString accountKey, MqlString folder);

// /api/accounts/CloseSession
MT4ACCESS int DMFXAccountsCloseSession();

// /api/timeseries/GetTickerList
MT4ACCESS int DMFXTimeseriesGetTickerList(MqlString country, MqlString regulator);

// /api/timeseries/GetTimeSeriesInfo
MT4ACCESS int DMFXTimeseriesGetTimeSeriesInfo(MqlString country, MqlString ticker);

// /api/timeseries/GetTimeSeries
MT4ACCESS int DMFXTimeseriesGetTimeSeries(MqlString country, MqlString ticker, int timeframe, time_t periodStart, time_t periodEnd);

// returns number of timeseries for given ticker
MT4ACCESS int GetTimeSeriesCount(MqlString ticker);

// returns number of timeseries for given ticker
MT4ACCESS int GetTimeSeriesDatesCount(MqlString ticker, MqlString tsName);

// returns name of the time series with the given index
MT4ACCESS MqlString GetTimeSeriesName(MqlString ticker, int index);

// returns single time series with the given index
MT4ACCESS int GetTimeSeriesValues(MqlString ticker, int index, int* dates, double* values);

// returns last error message text
MT4ACCESS MqlString GetLastErrorMessage();

