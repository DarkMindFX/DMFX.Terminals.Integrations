// This is the main DLL file.

#include "stdafx.h"

#ifdef _MSC_VER

#pragma comment (lib,"uuid.lib")

#endif

#include "DarkMindConnect.h"
#include "StringDefs.h"
#include "StringUtils.h"

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

	void SetColumns(IList<String^>^ columns)
	{
		Series->Clear();
		SeriesNames->Clear();
		for (int i = 0; i < columns->Count; ++i)
		{
			IndicatorSeriesData^ data = gcnew IndicatorSeriesData();
			data->Name = columns[i];
			Series->Add(columns[i], data);
			SeriesNames->Add(i, columns[i]);
		}
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
MT4ACCESS int DMFXAccountsInitSession(MqlString host, MqlString accountKey, MqlString folder)
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
			return (int)resInit->Errors[0]->Code;
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

MT4ACCESS int DMFXAccountsCloseSession()
{
	try
	{
		DTO::CloseSession^ reqInit = gcnew DTO::CloseSession();
		reqInit->RequestID = Guid::NewGuid().ToString();
		reqInit->SessionToken = Globals::SessionToken;

		DTO::CloseSessionResponse^ resInit = Post<DTO::CloseSessionResponse>("/api/accounts/CloseSession", reqInit);
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

// /api/timeseries/GetTickerList
MT4ACCESS int DMFXTimeseriesGetTickerList(MqlString country, MqlString regulator)
{
	return 0;
}

// /api/timeseries/GetTimeSeriesInfo
MT4ACCESS int DMFXTimeseriesGetTimeSeriesInfo(MqlString country, MqlString ticker)
{
	try
	{
		DTO::GetTimeSeriesInfo^ req = gcnew DTO::GetTimeSeriesInfo();
		req->RequestID = Guid::NewGuid().ToString();
		req->SessionToken = Globals::SessionToken;
		req->CountryCode = ToManagedString(country);
		req->Ticker = ToManagedString(ticker);

		DTO::GetTimeSeriesInfoResponse^ resp = Post<DTO::GetTimeSeriesInfoResponse>("/api/timeseries/GetTimeSeriesInfo", req);
		if (resp->Success)
		{
			// creating new records if needed
			IndicatorData^ indData = nullptr;
			if (!Globals::Indicators->TryGetValue(resp->Payload->Ticker, indData))
			{
				indData = gcnew IndicatorData();
				indData->Code = resp->Payload->Ticker;
				indData->Name = resp->Payload->Name;
				Globals::Indicators->Add(resp->Payload->Ticker, indData);
			}

			// filling column details
			indData->SetColumns(resp->Payload->Columns);

			return (int)EErrorCodes::Success;
		}
		else
		{
			return (int)resp->Errors[0]->Code;
		}
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

}

// /api/timeseries/GetTimeSeries
MT4ACCESS int DMFXTimeseriesGetTimeSeries(MqlString country, MqlString ticker, int timeframe, int periodStart, int periodEnd)
{
	try
	{
		String^ sTicker = ToManagedString(ticker);
		DTO::GetTimeSeries^ req = gcnew DTO::GetTimeSeries();
		req->RequestID = Guid::NewGuid().ToString();
		req->SessionToken = Globals::SessionToken;
		req->CountryCode = ToManagedString(country);
		req->Ticker = sTicker;
		req->TimeFrame = (DMFX::QuotesInterfaces::ETimeFrame)timeframe;
		req->PeriodStart = periodStart > 0 ? DateTime::Parse("01/01/1970") + TimeSpan::FromSeconds(periodStart) : DateTime::Parse("01/01/1970");
		req->PeriodEnd = periodEnd > 0 ? DateTime::Parse("01/01/1970") + TimeSpan::FromSeconds(periodEnd) : DateTime::Now;

		DTO::GetTimeSeriesResponse^ resp = Post<DTO::GetTimeSeriesResponse>("/api/timeseries/GetTimeSeries", req);
		if (resp->Success)
		{
			// creating new records if needed
			IndicatorData^ indData = nullptr;
			if (Globals::Indicators->TryGetValue(sTicker, indData))
			{
				for (int i = 0; i < resp->Payload->Values->Quotes->Count; ++i)
				{
					DTO::QuoteRecord^ rec = resp->Payload->Values->Quotes[i];
					for (int v = 0; v < rec->Values->Count; ++v)
					{
						String^ sTsName = indData->SeriesNames[v];
						IndicatorSeriesData^ indTsData = nullptr;
						if (indData->Series->TryGetValue(sTsName, indTsData))
						{
							indTsData->Values->Add(
								rec->Time,
								rec->Values[v]
							);
						}
					}
				}
			}

			return (int)EErrorCodes::Success;
		}
		else
		{
			return (int)resp->Errors[0]->Code;
		}
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
}

// returns number of timeseries 
MT4ACCESS int GetTimeSeriesCount(MqlString ticker)
{
	IndicatorData^ indData = nullptr;
	String^ sTicker = ToManagedString(ticker);
	if (Globals::Indicators->TryGetValue(sTicker, indData))
	{
		return indData->Series->Count;
	}
	else
	{
		return 0;
	}
}

MT4ACCESS int GetTimeSeriesDatesCount(MqlString ticker, MqlString tsName)
{
	IndicatorData^ indData = nullptr;
	String^ sTicker = ToManagedString(ticker);
	String^ sTsName = ToManagedString(tsName);
	if (Globals::Indicators->TryGetValue(sTicker, indData) && indData->Series->Count > 0)
	{
		return indData->Series[sTsName]->Values->Count;
	}
	else
	{
		return 0;
	}
}

// returns name of the time series with the given index
MT4ACCESS MqlString GetTimeSeriesName(MqlString ticker, int index)
{
	String^ sTicker = ToManagedString(ticker);
	IndicatorData^ indData = nullptr;
	if (Globals::Indicators->TryGetValue(sTicker, indData))
	{
		return ToMqlString(indData->SeriesNames[index]);
	}

	return 0;
}

// returns single time series with the given index
MT4ACCESS int GetTimeSeriesValues(MqlString ticker, int index, int* dates, double* values)
{
	try
	{
		DateTime dtStart(1970, 1, 1);

		String^ sTicker = ToManagedString(ticker);
		IndicatorData^ indData = nullptr;
		if (Globals::Indicators->TryGetValue(sTicker, indData))
		{
			String^ sName = indData->SeriesNames[index];

			IndicatorSeriesData^ indSeriesData = indData->Series[sName];

			int i = 0;
			for each (DateTime dt in indSeriesData->Values->Keys)
			{
				dates[i] = (dt - dtStart).TotalSeconds;
				values[i] = double(indSeriesData->Values[dt]);
				++i;
			}

		}
		else
		{
			return (int)EErrorCodes::QuotesNotFound;
		}
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
	return 0;
}


MT4ACCESS MqlString GetLastErrorMessage()
{
	if (Globals::LastError != nullptr)
	{
		if (Globals::LastError->GetType()->Equals( WebServiceException::typeid ))
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



