#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
using DMFX.DarkMindConnect;
using DMFX.Interfaces;
using DMFX.QuotesInterfaces;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class DMFXCOTFinFut : Indicator
	{
		public enum ECOTFinCodes
		{
			CANADIAN_DOLLAR__CHICAGO_MERCANTILE_EXCHANGE	=0,
			SWISS_FRANC__CHICAGO_MERCANTILE_EXCHANGE	=1,
			BRITISH_POUND_STERLING__CHICAGO_MERCANTILE_EXCHANGE	=2,
			JAPANESE_YEN__CHICAGO_MERCANTILE_EXCHANGE	=3,
			EURO_FX__CHICAGO_MERCANTILE_EXCHANGE	=4,
			AUSTRALIAN_DOLLAR__CHICAGO_MERCANTILE_EXCHANGE	=5,
			EURO_FXvBRITISH_POUND_XRATE__CHICAGO_MERCANTILE_EXCHANGE	=6,
			EURO_FXvJAPANESE_YEN_XRATE__CHICAGO_MERCANTILE_EXCHANGE	=7,
			RUSSIAN_RUBLE__CHICAGO_MERCANTILE_EXCHANGE	=8,
			MEXICAN_PESO__CHICAGO_MERCANTILE_EXCHANGE	=9,
			BRAZILIAN_REAL__CHICAGO_MERCANTILE_EXCHANGE	=10,
			NEW_ZEALAND_DOLLAR__CHICAGO_MERCANTILE_EXCHANGE	=11,
			SOUTH_AFRICAN_RAND__CHICAGO_MERCANTILE_EXCHANGE	=12,
			DJIA_Consolidated__CHICAGO_BOARD_OF_TRADE	=13,
			DOW_JONES_INDUSTRIAL_AVG_x_USD5__CHICAGO_BOARD_OF_TRADE	=14,
			DOW_JONES_US_REAL_ESTATE_IDX__CHICAGO_BOARD_OF_TRADE	=15,
			SnP_500_Consolidated__CHICAGO_MERCANTILE_EXCHANGE	=16,
			SnP_500_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=17,
			EMINI_SnP_CONSU_STAPLES_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=18,
			EMINI_SnP_ENERGY_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=19,
			EMINI_SnP_500_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=20,
			EMINI_SnP_FINANCIAL_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=21,
			EMINI_SnP_HEALTH_CARE_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=22,
			EMINI_SnP_INDUSTRIAL_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=23,
			EMINI_SnP_MATERIALS_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=24,
			EMINI_SnP_TECHNOLOGY_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=25,
			EMINI_SnP_UTILITIES_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=26,
			SnP_500_TOTAL_RETURN_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=27,
			NASDAQ100_Consolidated__CHICAGO_MERCANTILE_EXCHANGE	=28,
			NASDAQ100_STOCK_INDEX_MINI__CHICAGO_MERCANTILE_EXCHANGE	=29,
			EMINI_RUSSELL_2000_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=30,
			NIKKEI_STOCK_AVERAGE__CHICAGO_MERCANTILE_EXCHANGE	=31,
			NIKKEI_STOCK_AVERAGE_YEN_DENOM__CHICAGO_MERCANTILE_EXCHANGE	=32,
			MSCI_EAFE_MINI_INDEX__ICE_FUTURES_US	=33,
			MSCI_EMERGING_MKTS_MINI_INDEX__ICE_FUTURES_US	=34,
			EMINI_SnP_400_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=35,
			SnP_500_ANNUAL_DIVIDEND_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=36,
			US_TREASURY_BONDS__CHICAGO_BOARD_OF_TRADE	=37,
			ULTRA_US_TREASURY_BONDS__CHICAGO_BOARD_OF_TRADE	=38,
			_2YEAR_US_TREASURY_NOTES__CHICAGO_BOARD_OF_TRADE	=39,
			_10YEAR_US_TREASURY_NOTES__CHICAGO_BOARD_OF_TRADE	=40,
			ULTRA_10YEAR_US_TNOTES__CHICAGO_BOARD_OF_TRADE	=41,
			_5YEAR_US_TREASURY_NOTES__CHICAGO_BOARD_OF_TRADE	=42,
			_30DAY_FEDERAL_FUNDS__CHICAGO_BOARD_OF_TRADE	=43,
			_3MONTH_EURODOLLARS__CHICAGO_MERCANTILE_EXCHANGE	=44,
			_3MONTH_SOFR__CHICAGO_MERCANTILE_EXCHANGE	=45,
			_1MONTH_SOFR__CHICAGO_MERCANTILE_EXCHANGE	=46,
			_10_YEAR_DELIVERABLE_IR__CHICAGO_BOARD_OF_TRADE	=47,
			_5_YEAR_DELIVERABLE_IR__CHICAGO_BOARD_OF_TRADE	=48,
			_10_YEAR_ERIS_SWAP___CHICAGO_BOARD_OF_TRADE	=49,
			_5_YEAR_ERIS_SWAP__CHICAGO_BOARD_OF_TRADE	=50,
			BITCOINUSD__CBOE_FUTURES_EXCHANGE	=51,
			BITCOIN__CHICAGO_MERCANTILE_EXCHANGE	=52,
			US_DOLLAR_INDEX__ICE_FUTURES_US	=53,
			VIX_FUTURES__CBOE_FUTURES_EXCHANGE	=54,
			BLOOMBERG_COMMODITY_INDEX__CHICAGO_BOARD_OF_TRADE	=55,
			EMINI_SnP_CONSUMER_DISC_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=56,
			EMINI_RUSSELL_1000_VALUE_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=57,
			RUSSELL_2000_MINI_INDEX_FUTURE__ICE_FUTURES_US	=58,
			_5Year_Eris_Standard_Initial__ERIS_FUTURES_EXCHANGE	=59,
			RUSSELL_1000_VALUE_INDEX_MINI__ICE_FUTURES_US	=60,
			SnP_GSCI_COMMODITY_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=61,
			EURO_FXvJAPANESE_YEN__SMALL__ICE_FUTURES_US	=62,
			EURO_FXvBRITISH_POUND__SMALL__ICE_FUTURES_US	=63,
			DOW_JONES_INDUSTRIAL_AVERAGE__CHICAGO_BOARD_OF_TRADE	=64,
			NASDAQ100_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=65,
			Russell_2000_Stock_Index_Future__Chicago_Mercantile_Exchange	=66,
			RUSSEL_1000_MINI_INDEX_FUTURE__ICE_FUTURES_US	=67,
			Russell_2000_Stock_Index__ICE_Futures_US	=68,
			EMINI_MSCI_EAFE__CHICAGO_MERCANTILE_EXCHANGE	=69,
			EMINI_MSCI_EMERGING_MARKETS__CHICAGO_MERCANTILE_EXCHANGE	=70,
			SnP_400_MIDCAP_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE	=71,
			DTCC_RepoUS_Treasury_less30_YR__NYSE_LIFFE__NYPC	=72,
			ONEMONTH_EURODOLLAR__CHICAGO_MERCANTILE_EXCHANGE	=73,
			INTEREST_RATE_SWAPS_10YR__CHICAGO_BOARD_OF_TRADE	=74,
			INTEREST_RATE_SWAPS_5YR__CHICAGO_BOARD_OF_TRADE	=75,
			_3MO_EUROYEN_TIBOR__CHICAGO_MERCANTILE_EXCHANGE	=76
		}
		
		static public string GetTicker(ECOTFinCodes cotId)
		{
			string[,] values = new string[,] {
				{	"COT.FinFut.090741",		"CANADIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.092741",		"SWISS FRANC - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.096742",		"BRITISH POUND STERLING - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.097741",		"JAPANESE YEN - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.099741",		"EURO FX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.232741",		"AUSTRALIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.299741",		"EURO FX/BRITISH POUND XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.399741",		"EURO FX/JAPANESE YEN XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.089741",		"RUSSIAN RUBLE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.095741",		"MEXICAN PESO - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.102741",		"BRAZILIAN REAL - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.112741",		"NEW ZEALAND DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.122741",		"SOUTH AFRICAN RAND - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.12460+",		"DJIA Consolidated - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.124603",		"DOW JONES INDUSTRIAL AVG- x $5 - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.124606",		"DOW JONES U.S. REAL ESTATE IDX - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.13874+",		"S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.138741",		"S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.138748",		"E-MINI S&P CONSU STAPLES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.138749",		"E-MINI S&P ENERGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874A",		"E-MINI S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874C",		"E-MINI S&P FINANCIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874E",		"E-MINI S&P HEALTH CARE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874F",		"E-MINI S&P INDUSTRIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874H",		"E-MINI S&P MATERIALS INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874I",		"E-MINI S&P TECHNOLOGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874J",		"E-MINI S&P UTILITIES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.13874N",		"S&P 500 TOTAL RETURN INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.20974+",		"NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.209742",		"NASDAQ-100 STOCK INDEX (MINI) - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.239742",		"E-MINI RUSSELL 2000 INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.240741",		"NIKKEI STOCK AVERAGE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.240743",		"NIKKEI STOCK AVERAGE YEN DENOM - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.244041",		"MSCI EAFE MINI INDEX - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.244042",		"MSCI EMERGING MKTS MINI INDEX - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.33874A",		"E-MINI S&P 400 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.43874A",		"S&P 500 ANNUAL DIVIDEND INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.020601",		"U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.020604",		"ULTRA U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.042601",		"2-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.043602",		"10-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.043607",		"ULTRA 10-YEAR U.S. T-NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.044601",		"5-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.045601",		"30-DAY FEDERAL FUNDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.132741",		"3-MONTH EURODOLLARS - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.134741",		"3-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.134742",		"1-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.246605",		"10 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.246606",		"5 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.343601",		"10 YEAR ERIS SWAP  - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.344601",		"5 YEAR ERIS SWAP - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.1330E1",		"BITCOIN-USD - CBOE FUTURES EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.133741",		"BITCOIN - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.098662",		"U.S. DOLLAR INDEX - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.1170E1",		"VIX FUTURES - CBOE FUTURES EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.221602",		"BLOOMBERG COMMODITY INDEX - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.138747",		"E-MINI S&P CONSUMER DISC INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.239744",		"EMINI RUSSELL 1000 VALUE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.23977A",		"RUSSELL 2000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.ZB9105",		"5-Year Eris Standard- Initial - ERIS FUTURES EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.23977C",		"RUSSELL 1000 VALUE INDEX MINI - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.256741",		"S&P GSCI COMMODITY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.299661",		"EURO FX/JAPANESE YEN - SMALL - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.599661",		"EURO FX/BRITISH POUND - SMALL - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.124601",		"DOW JONES INDUSTRIAL AVERAGE - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.209741",		"NASDAQ-100 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.239741",		"Russell 2000 Stock Index Future - Chicago Mercantile Exchange (Net Positions)"	},
				{	"COT.FinFut.239772",		"RUSSEL 1000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFut.239777",		"Russell 2000 Stock Index - ICE Futures U.S. (Net Positions)"	},
				{	"COT.FinFut.244741",		"E-MINI MSCI EAFE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.244742",		"E-MINI MSCI EMERGING MARKETS - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.338741",		"S&P 400 MIDCAP STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.045L2T",		"DTCC Repo-US Treasury <30 YR - NYSE LIFFE - NYPC (Net Positions)"	},
				{	"COT.FinFut.032741",		"ONE-MONTH EURODOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFut.246602",		"INTEREST RATE SWAPS 10YR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.247602",		"INTEREST RATE SWAPS 5YR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFut.597741",		"3-MO. EUROYEN TIBOR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	}
			};
			
			return values[(int)cotId,0];
		}
		
		DMFXClient _dmfxClient = null; // DMFX connection client
		string _country = "US"; // US by default
		string _ticker = null; // ticker to display
		/*
		private System.Drawing.KnownColor[] Colors = {KnownColor.Red, KnownColor.Green, KnownColor.Blue, KnownColor.Orange, KnownColor.Purple};
		*/
		
		
		protected override void OnStateChange()
		{

				if (State == State.SetDefaults)
				{
					Description									= @"DarkMindFX - COT indicator for Financials - Futures Only Net Positions";
					Name										= "DMFX - COT: Financials - Futures Only";
					Calculate									= Calculate.OnBarClose;
					IsOverlay									= false;
					DisplayInDataBox							= true;
					DrawOnPricePanel							= false;
					DrawHorizontalGridLines						= true;
					DrawVerticalGridLines						= true;
					PaintPriceMarkers							= true;
					ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
					//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
					//See Help Guide for additional information.
					IsSuspendedWhileInactive					= true;

                    Host = "http://darkmindfx.com/";
                    AccountKey = "[Your Account Key]";
                    COTCode = ECOTFinCodes.EURO_FX__CHICAGO_MERCANTILE_EXCHANGE;
					WeeksBack = 500;
					
					AddPlot(Brushes.Red, "Open_Interest_All");
					AddPlot(Brushes.Red, "Dealer_Positions_Long_All");
					AddPlot(Brushes.Orange, "Dealer_Positions_Short_All");
					AddPlot(Brushes.Blue, "Asset_Mgr_Positions_Long_All");
					AddPlot(Brushes.Green, "Asset_Mgr_Positions_Short_All");
					AddPlot(Brushes.Red, "Lev_Money_Positions_Long_All");
					AddPlot(Brushes.Orange, "Lev_Money_Positions_Short_All");
					AddPlot(Brushes.Blue, "Other_Rept_Positions_Long_All");
					AddPlot(Brushes.Green, "Other_Rept_Positions_Short_All");
				}
				else if (State == State.Configure)
				{
					
					if(_dmfxClient == null)
					{
						_dmfxClient = new DMFXClient();
					}
					
					_ticker = GetTicker(COTCode);
					EErrorCodes result = _dmfxClient.InitSession(Host, AccountKey);	
					if(result == EErrorCodes.Success)
					{
						Print("Session opened for " + AccountKey);
						
						result = _dmfxClient.GetTimeSeriesInfo(_country, _ticker);
						if(result == EErrorCodes.Success)
						{
							DateTime periodStart = DateTime.UtcNow - TimeSpan.FromDays(Math.Max(WeeksBack, 52) * 7);
							DateTime periodEnd = DateTime.UtcNow;
							result = _dmfxClient.GetTimeSeries(_country, _ticker, ETimeFrame.Weekly, periodStart, periodEnd);
							if(result != EErrorCodes.Success)
							{
								Print("GetTimeSeries failed: " + result + " - " + _dmfxClient.GetLastError());
							}
							else
							{
								Print("Data loaded for " + _ticker);
							}
						}
						else
						{
							Print("GetTimeSeriesInfo failed: " + result + " - " + _dmfxClient.GetLastError());
						}
					}
					else
					{
						Print("InitSession failed: " + result + " - " + _dmfxClient.GetLastError());
					}
				}
				else if(State == State.Terminated)
				{
					if(_dmfxClient != null)
					{
						EErrorCodes result = _dmfxClient.CloseSession();
						Print("Session closed for " + AccountKey);
					}
				}
			
		}

		protected override void OnBarUpdate()
		{
			
			if(_dmfxClient != null)
			{
				IList<string> columns = _dmfxClient.GetColumns(_ticker);
				if(columns != null)
				{
					DateTime dtNow = Time[0];

					for(int i = 0; i < columns.Count; ++i)
					{
						decimal value = _dmfxClient.GetDateValue(_ticker, columns[i], dtNow);
						Values[i][0] = (double)value;
					}
				}
			}
			
		}
		
		
		
		[NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "AccountKey", GroupName = "NinjaScriptParameters", Order = 0)]
		public string AccountKey
		{ get; set; }
		
		[NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "COTCode", GroupName = "NinjaScriptParameters", Order = 1)]
		public NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFut.ECOTFinCodes COTCode
		{ get; set; }
		
		[NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "WeeksBack", GroupName = "NinjaScriptParameters", Order = 2)]
		public int WeeksBack
		{ get; set; }
		
		[NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Host", GroupName = "NinjaScriptParameters", Order = 3)]
		public string Host
		{ get; set; }
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private DMFXCOTFinFut[] cacheDMFXCOTFinFut;
		public DMFXCOTFinFut DMFXCOTFinFut(string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFut.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return DMFXCOTFinFut(Input, accountKey, cOTCode, weeksBack, host);
		}

		public DMFXCOTFinFut DMFXCOTFinFut(ISeries<double> input, string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFut.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			if (cacheDMFXCOTFinFut != null)
				for (int idx = 0; idx < cacheDMFXCOTFinFut.Length; idx++)
					if (cacheDMFXCOTFinFut[idx] != null && cacheDMFXCOTFinFut[idx].AccountKey == accountKey && cacheDMFXCOTFinFut[idx].COTCode == cOTCode && cacheDMFXCOTFinFut[idx].WeeksBack == weeksBack && cacheDMFXCOTFinFut[idx].Host == host && cacheDMFXCOTFinFut[idx].EqualsInput(input))
						return cacheDMFXCOTFinFut[idx];
			return CacheIndicator<DMFXCOTFinFut>(new DMFXCOTFinFut(){ AccountKey = accountKey, COTCode = cOTCode, WeeksBack = weeksBack, Host = host }, input, ref cacheDMFXCOTFinFut);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.DMFXCOTFinFut DMFXCOTFinFut(string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFut.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFut(Input, accountKey, cOTCode, weeksBack, host);
		}

		public Indicators.DMFXCOTFinFut DMFXCOTFinFut(ISeries<double> input , string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFut.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFut(input, accountKey, cOTCode, weeksBack, host);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.DMFXCOTFinFut DMFXCOTFinFut(string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFut.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFut(Input, accountKey, cOTCode, weeksBack, host);
		}

		public Indicators.DMFXCOTFinFut DMFXCOTFinFut(ISeries<double> input , string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFut.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFut(input, accountKey, cOTCode, weeksBack, host);
		}
	}
}

#endregion
