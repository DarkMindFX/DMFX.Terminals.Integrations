/*
using DMFX.Interfaces;
using DMFX.QuotesInterfaces;
using DMFX.Service.DTO;
using ServiceStack.ServiceClient.Web;
*/
using DMFX.Interfaces;
using DMFX.QuotesInterfaces;
using DMFX.Service.DTO;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.DarkMindConnect
{
    public class DMFXClient
    {
        
        private string Host = null;
        private string AccountKey = null;
        private string SessionToken = null;
        private JsonServiceClient Client = null;
        private Dictionary<string, IndicatorData> Indicators = null;
        private Exception _lastError = null;

        public EErrorCodes InitSession(string host, string accountKey)
        {
            EErrorCodes result = EErrorCodes.GeneralError;

            try
            {
                if (Client == null)
                {
                    Host = host;
                    Client = new JsonServiceClient(Host);
                }

                if (SessionToken == null || AccountKey != accountKey)
                {
                    AccountKey = accountKey;
                    InitSession reqInit = new InitSession();
                    reqInit.RequestID = System.Guid.NewGuid().ToString();
                    reqInit.AccountKey = AccountKey;

                    InitSessionResponse resInit = Post<InitSession, InitSessionResponse>("/api/accounts/InitSession", reqInit);

                    if (resInit.Success)
                    {
                        SessionToken = resInit.SessionToken;
                        Indicators = new Dictionary<string, IndicatorData>();
                        result = EErrorCodes.Success;
                    }
                    else
                    {
                        result = resInit.Errors[0].Code;
                    }
                }
                else
                {
                    return EErrorCodes.Success;
                }
            }
            catch (Exception ex)
            {
                _lastError = ex;
                result = EErrorCodes.GeneralError;
            }

            return result;
        }

        public EErrorCodes CloseSession()
        {
            EErrorCodes result = EErrorCodes.GeneralError;

            try
            {
                if (SessionToken != null)
                {
                    CloseSession reqInit = new CloseSession();
                    reqInit.RequestID = System.Guid.NewGuid().ToString();
                    reqInit.SessionToken = SessionToken;

                    CloseSessionResponse resInit = Post<CloseSession, CloseSessionResponse>("/api/accounts/CloseSession", reqInit);

                    if (resInit.Success)
                    {
                        SessionToken = null;
                        result = EErrorCodes.Success;
                    }
                    else
                    {
                        result = resInit.Errors[0].Code;
                    }
                }
                else
                {
                    result = EErrorCodes.Success;
                }
            }
            catch (Exception ex)
            {
                _lastError = ex;
                result = EErrorCodes.GeneralError;
            }

            return result;
        }

        public EErrorCodes GetTimeSeriesInfo(string country, string ticker)
        {
            EErrorCodes result = EErrorCodes.GeneralError;

            try
            {
                if (Client != null && SessionToken != null)
                {
                    GetTimeSeriesInfo reqGetTSInfo = new GetTimeSeriesInfo();
                    reqGetTSInfo.SessionToken = SessionToken;
                    reqGetTSInfo.Ticker = ticker;
                    reqGetTSInfo.CountryCode = country;

                    GetTimeSeriesInfoResponse resGetTSInfo = Post<GetTimeSeriesInfo, GetTimeSeriesInfoResponse>("/api/timeseries/GetTimeSeriesInfo", reqGetTSInfo);
                    if (resGetTSInfo.Success)
                    {
                        IndicatorData indData = null;
                        if (!Indicators.TryGetValue(ticker, out indData))
                        {
                            indData = new IndicatorData();
                            indData.Name = resGetTSInfo.Payload.Name;
                            indData.Code = resGetTSInfo.Payload.Ticker;
                            Indicators.Add(ticker, indData);
                        }

                        indData.SetColumns(resGetTSInfo.Payload.Columns);

                        result = EErrorCodes.Success;
                    }
                    else
                    {
                        result = resGetTSInfo.Errors[0].Code;
                    }
                }
                else
                {
                    return EErrorCodes.SessionClosed;
                }


            }
            catch (Exception ex)
            {
                _lastError = ex;
                result = EErrorCodes.GeneralError;
            }

            return result;
        }

        public EErrorCodes GetTimeSeries(string country, string ticker, ETimeFrame timeframe, DateTime periodStart, DateTime periodEnd)
        {
            EErrorCodes result = EErrorCodes.GeneralError;

            try
            {
                if (SessionToken != null)
                {
                    GetTimeSeries reqGetTs = new GetTimeSeries();
                    reqGetTs.Ticker = ticker;
                    reqGetTs.CountryCode = country;
                    reqGetTs.SessionToken = SessionToken;
                    reqGetTs.PeriodStart = periodStart != DateTime.MinValue ? periodStart : DateTime.Parse("01/01/1970");
                    reqGetTs.PeriodEnd = periodEnd != DateTime.MinValue ? periodEnd : DateTime.Now;
                    reqGetTs.TimeFrame = timeframe;

                    GetTimeSeriesResponse resGetTs = Post<GetTimeSeries, GetTimeSeriesResponse>("/api/timeseries/GetTimeSeries", reqGetTs);
                    if (resGetTs.Success)
                    {
                        // creating new records if needed
                        IndicatorData indData = null;
                        if (Indicators.TryGetValue(ticker, out indData))
                        {
                            for (int i = 0; i < resGetTs.Payload.Values.Quotes.Count; ++i)
                            {
                                QuoteRecord rec = resGetTs.Payload.Values.Quotes[i];
                                for (int v = 0; v < rec.Values.Count; ++v)
                                {
                                    String sTsName = indData.SeriesNames[v];
                                    IndicatorSeriesData indTsData = null;
                                    if (indData.Series.TryGetValue(sTsName, out indTsData))
                                    {
                                        indTsData.Values.Add(
                                            rec.Time,
                                            rec.Values[v]
                                        );
                                    }
                                }
                            }

                            result = EErrorCodes.Success;
                        }
                        else
                        {
                            result = EErrorCodes.TickerNotFound;
                        }
                    }
                    else
                    {
                        _lastError = new Exception(resGetTs.Errors[0].Message);
                        result = resGetTs.Errors[0].Code;
                    }
                }
                else
                {                    
                    return EErrorCodes.SessionClosed;
                }

            }
            catch (Exception ex)
            {
                _lastError = ex;
                result = EErrorCodes.GeneralError;
            }

            return result;

        }

        public Exception GetLastError()
        {
            return _lastError;
        }

        public IList<string> GetColumns(string ticker)
        {
            IList<string> columns = null;
            IndicatorData indData = null;
            if (Indicators.TryGetValue(ticker, out indData))
            {
                columns = new List<string>(indData.Series.Keys);
            }

            return columns;
        }

        public IDictionary<DateTime, decimal> GetTimeseriesData(string ticker, string timeseries)
        {
            IDictionary<DateTime, decimal> values = null;
            IndicatorData indData = null;
            IndicatorSeriesData indSeriesData = null;
            if (Indicators.TryGetValue(ticker, out indData) && indData.Series.TryGetValue(timeseries, out indSeriesData))
            {
                values = new Dictionary<DateTime, decimal>(indSeriesData.Values);
            }

            return values;
        }

        public decimal GetDateValue(string ticker, string timeseries, DateTime dt)
        {
            decimal result = Decimal.MinValue;

            IndicatorData indData = null;
            IndicatorSeriesData indSeriesData = null;
            if (Indicators.TryGetValue(ticker, out indData) && indData.Series.TryGetValue(timeseries, out indSeriesData))
            {
                DateTime k = indSeriesData.Values.Keys.LastOrDefault(x => x <= dt);
                if (k != DateTime.MinValue)
                {
                    result = indSeriesData.Values[k];
                }
            }

            return result;
        }

        
        #region Support methods
        TResponse Post<TRequest, TResponse>(string method, TRequest request)
        {
            TResponse response = Client.Post<TResponse>(method, request);

            return response;
        }
        #endregion
        
    }
}
