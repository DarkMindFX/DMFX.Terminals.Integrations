// DMFX.Test.ConsoleClient.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <windows.h>

#include "..\..\DarkMindConnect\StringDefs.h"
#include "..\..\DarkMindConnect\StringUtils.h"
#include "..\..\DarkMindConnect\DarkMindConnect.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace ServiceStack;
using namespace ServiceStack::ServiceClient::Web;
using namespace System::IO;
using namespace DMFX::Service;
using namespace DMFX::Interfaces;


ref class IndicatorSeriesData
{
public:
	property String^ Name;
	property Dictionary<DateTime, Decimal>^ Values;

	IndicatorSeriesData()
	{
		Values = gcnew Dictionary<DateTime, Decimal>();
	}
};

ref class IndicatorData
{
public:

	property String^ Name;
	property String^ Code;
	property Dictionary<String^, IndicatorSeriesData^>^ Series;
	property Dictionary<int, String^>^ SeriesNames;
	property DateTime LastUpdated;

	IndicatorData()
	{
		Series = gcnew Dictionary<String^, IndicatorSeriesData^>();
		SeriesNames = gcnew Dictionary<int, String^>();
		LastUpdated = DateTime::Now;
	}
};

ref class Globals
{
public:
	static JsonServiceClient^ Client = nullptr;
	static String^ SessionToken = nullptr;
	static Dictionary<String^, IndicatorData^>^ Indicators = nullptr;
	static int UpdateFrequency = 1; // update frequency in minutes
	static int UpdateRecordsCount = 10; // request last N records
	static Object^ LastError = nullptr;
};



template<class TResponse, class TRequest>
TResponse^ Post(String^ method, TRequest^ request)
{
	TResponse^ response = Globals::Client->Post<TResponse^>(method, request);

	return response;
}

// /api/accounts/InitSession
int DMFXAccountsInitSession(MqlString host, MqlString accountKey, MqlString folder)
{
	try
	{
		String^ sHost = ToManagedString(host);
		String^ sKey = ToManagedString(accountKey);
		String^ sFolder = ToManagedString(folder);

		Globals::Client = gcnew JsonServiceClient(sHost);

		// Init

		DTO::InitSession^ reqInit = gcnew DTO::InitSession();
		reqInit->RequestID = Guid::NewGuid().ToString();
		reqInit->AccountKey = sKey;

		DTO::InitSessionResponse^ resInit = Post<DTO::InitSessionResponse>("/api/accounts/InitSession", reqInit);

		if (resInit != nullptr && resInit->Success)
		{
			Globals::SessionToken = resInit->SessionToken;
			Globals::Indicators = gcnew Dictionary<String^, IndicatorData^>();

		}
		else
		{
			return (int)(resInit->Errors[0]->Code);
		}

		return (int)EErrorCodes::Success;
	}
	catch (WebServiceException ^ webEx)
	{
		Globals::LastError = webEx;
		return (int)EErrorCodes::GeneralError;
	}
	catch (Exception ^ ex)
	{
		Globals::LastError = ex;
		return (int)EErrorCodes::GeneralError;
	}
	return (int)EErrorCodes::Success;
}

 int DMFXAccountsCloseSession()
{
	return 0;
}

// /api/timeseries/GetTickerList
 int DMFXTimeseriesGetTickerList(MqlString country, MqlString regulator)
{
	return 0;
}

// /api/timeseries/GetTimeSeriesInfo
 int DMFXTimeseriesGetTimeSeriesInfo(MqlString country, MqlString ticker)
{
	return 0;
}

// /api/timeseries/GetTimeSeries
 int DMFXTimeseriesGetTimeSeries(MqlString country, MqlString ticker, int timeframe, time_t periodStart, time_t periodEnd)
{
	return 0;
}

// returns number of timeseries 
 int GetTimeSeriesCount()
{
	return 0;
}

// returns name of the time series with the given index
 MqlString GetTimeSeriesName(int index)
{
	return 0;
}

// returns single time series with the given index
 int GetTimeSeriesValues(int index, int* dates, double* values)
{
	return 0;
}

 MqlString GetLastErrorMessage()
{
	if (Globals::LastError != nullptr)
	{
		if (Globals::LastError->GetType()->Equals(WebServiceException::typeid))
		{
			return ToMqlString(((WebServiceException^)Globals::LastError)->ErrorMessage);

		}
		else
		{
			return ToMqlString(((Exception^)Globals::LastError)->Message);
		}
	}
	else
	{
		return  ToMqlString(String::Empty);
	}
}

int main()
{
	int result = DMFXAccountsInitSession(ToMqlString("http://localhost/"), ToMqlString("123"), ToMqlString(""));

    std::cout << "Hello World!\n";
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
