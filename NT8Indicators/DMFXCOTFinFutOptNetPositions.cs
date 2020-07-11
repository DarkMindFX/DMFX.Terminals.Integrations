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
	
	
	public class DMFXCOTFinFutOptNetPositions : Indicator
	{
		
		public enum ECOTFinCodes
		{
			CANADIAN_DOLLAR__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=0,
			SWISS_FRANC__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=1,
			BRITISH_POUND_STERLING__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=2,
			JAPANESE_YEN__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=3,
			EURO_FX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=4,
			AUSTRALIAN_DOLLAR__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=5,
			EURO_FXvBRITISH_POUND_XRATE__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=6,
			EURO_FXvJAPANESE_YEN_XRATE__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=7,
			RUSSIAN_RUBLE__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=8,
			MEXICAN_PESO__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=9,
			BRAZILIAN_REAL__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=10,
			NEW_ZEALAND_DOLLAR__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=11,
			SOUTH_AFRICAN_RAND__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=12,
			DJIA_Consolidated__CHICAGO_BOARD_OF_TRADE_Net_Positions	=13,
			DOW_JONES_INDUSTRIAL_AVG_x_USD5__CHICAGO_BOARD_OF_TRADE_Net_Positions	=14,
			DOW_JONES_US_REAL_ESTATE_IDX__CHICAGO_BOARD_OF_TRADE_Net_Positions	=15,
			SnP_500_Consolidated__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=16,
			SnP_500_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=17,
			EMINI_SnP_CONSU_STAPLES_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=18,
			EMINI_SnP_ENERGY_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=19,
			EMINI_SnP_500_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=20,
			EMINI_SnP_FINANCIAL_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=21,
			EMINI_SnP_HEALTH_CARE_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=22,
			EMINI_SnP_INDUSTRIAL_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=23,
			EMINI_SnP_MATERIALS_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=24,
			EMINI_SnP_TECHNOLOGY_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=25,
			EMINI_SnP_UTILITIES_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=26,
			SnP_500_TOTAL_RETURN_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=27,
			NASDAQ100_Consolidated__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=28,
			NASDAQ100_STOCK_INDEX_MINI__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=29,
			EMINI_RUSSELL_2000_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=30,
			NIKKEI_STOCK_AVERAGE__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=31,
			NIKKEI_STOCK_AVERAGE_YEN_DENOM__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=32,
			MSCI_EAFE_MINI_INDEX__ICE_FUTURES_US_Net_Positions	=33,
			MSCI_EMERGING_MKTS_MINI_INDEX__ICE_FUTURES_US_Net_Positions	=34,
			EMINI_SnP_400_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=35,
			SnP_500_ANNUAL_DIVIDEND_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=36,
			US_TREASURY_BONDS__CHICAGO_BOARD_OF_TRADE_Net_Positions	=37,
			ULTRA_US_TREASURY_BONDS__CHICAGO_BOARD_OF_TRADE_Net_Positions	=38,
			_2YEAR_US_TREASURY_NOTES__CHICAGO_BOARD_OF_TRADE_Net_Positions	=39,
			_10YEAR_US_TREASURY_NOTES__CHICAGO_BOARD_OF_TRADE_Net_Positions	=40,
			ULTRA_10YEAR_US_TNOTES__CHICAGO_BOARD_OF_TRADE_Net_Positions	=41,
			_5YEAR_US_TREASURY_NOTES__CHICAGO_BOARD_OF_TRADE_Net_Positions	=42,
			_30DAY_FEDERAL_FUNDS__CHICAGO_BOARD_OF_TRADE_Net_Positions	=43,
			_3MONTH_EURODOLLARS__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=44,
			_3MONTH_SOFR__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=45,
			_1MONTH_SOFR__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=46,
			_10_YEAR_DELIVERABLE_IR__CHICAGO_BOARD_OF_TRADE_Net_Positions	=47,
			_5_YEAR_DELIVERABLE_IR__CHICAGO_BOARD_OF_TRADE_Net_Positions	=48,
			_10_YEAR_ERIS_SWAP___CHICAGO_BOARD_OF_TRADE_Net_Positions	=49,
			_5_YEAR_ERIS_SWAP__CHICAGO_BOARD_OF_TRADE_Net_Positions	=50,
			BITCOINUSD__CBOE_FUTURES_EXCHANGE_Net_Positions	=51,
			BITCOIN__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=52,
			US_DOLLAR_INDEX__ICE_FUTURES_US_Net_Positions	=53,
			VIX_FUTURES__CBOE_FUTURES_EXCHANGE_Net_Positions	=54,
			BLOOMBERG_COMMODITY_INDEX__CHICAGO_BOARD_OF_TRADE_Net_Positions	=55,
			EMINI_SnP_CONSUMER_DISC_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=56,
			EMINI_RUSSELL_1000_VALUE_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=57,
			RUSSELL_2000_MINI_INDEX_FUTURE__ICE_FUTURES_US_Net_Positions	=58,
			_5Year_Eris_Standard_Initial__ERIS_FUTURES_EXCHANGE_Net_Positions	=59,
			RUSSELL_1000_VALUE_INDEX_MINI__ICE_FUTURES_US_Net_Positions	=60,
			SnP_GSCI_COMMODITY_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=61,
			EURO_FXvJAPANESE_YEN__SMALL__ICE_FUTURES_US_Net_Positions	=62,
			EURO_FXvBRITISH_POUND__SMALL__ICE_FUTURES_US_Net_Positions	=63,
			DOW_JONES_INDUSTRIAL_AVERAGE__CHICAGO_BOARD_OF_TRADE_Net_Positions	=64,
			NASDAQ100_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=65,
			Russell_2000_Stock_Index_Future__Chicago_Mercantile_Exchange_Net_Positions	=66,
			RUSSEL_1000_MINI_INDEX_FUTURE__ICE_FUTURES_US_Net_Positions	=67,
			Russell_2000_Stock_Index__ICE_Futures_US_Net_Positions	=68,
			EMINI_MSCI_EAFE__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=69,
			EMINI_MSCI_EMERGING_MARKETS__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=70,
			SnP_400_MIDCAP_STOCK_INDEX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=71,
			DTCC_RepoUS_Treasury_less30_YR__NYSE_LIFFE__NYPC_Net_Positions	=72,
			ONEMONTH_EURODOLLAR__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=73,
			INTEREST_RATE_SWAPS_10YR__CHICAGO_BOARD_OF_TRADE_Net_Positions	=74,
			INTEREST_RATE_SWAPS_5YR__CHICAGO_BOARD_OF_TRADE_Net_Positions	=75,
			_3MO_EUROYEN_TIBOR__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions	=76
		}
		
		static public string GetTicker(ECOTFinCodes cotId)
		{
			string[,] values = new string[,] {
				{	"COT.FinFutOpt.090741.Net",		"CANADIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.092741.Net",		"SWISS FRANC - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.096742.Net",		"BRITISH POUND STERLING - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.097741.Net",		"JAPANESE YEN - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.099741.Net",		"EURO FX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.232741.Net",		"AUSTRALIAN DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.299741.Net",		"EURO FX/BRITISH POUND XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.399741.Net",		"EURO FX/JAPANESE YEN XRATE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.089741.Net",		"RUSSIAN RUBLE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.095741.Net",		"MEXICAN PESO - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.102741.Net",		"BRAZILIAN REAL - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.112741.Net",		"NEW ZEALAND DOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.122741.Net",		"SOUTH AFRICAN RAND - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.12460+.Net",		"DJIA Consolidated - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.124603.Net",		"DOW JONES INDUSTRIAL AVG- x $5 - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.124606.Net",		"DOW JONES U.S. REAL ESTATE IDX - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.13874+.Net",		"S&P 500 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.138741.Net",		"S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.138748.Net",		"E-MINI S&P CONSU STAPLES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.138749.Net",		"E-MINI S&P ENERGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874A.Net",		"E-MINI S&P 500 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874C.Net",		"E-MINI S&P FINANCIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874E.Net",		"E-MINI S&P HEALTH CARE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874F.Net",		"E-MINI S&P INDUSTRIAL INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874H.Net",		"E-MINI S&P MATERIALS INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874I.Net",		"E-MINI S&P TECHNOLOGY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874J.Net",		"E-MINI S&P UTILITIES INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.13874N.Net",		"S&P 500 TOTAL RETURN INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.20974+.Net",		"NASDAQ-100 Consolidated - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.209742.Net",		"NASDAQ-100 STOCK INDEX (MINI) - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.239742.Net",		"E-MINI RUSSELL 2000 INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.240741.Net",		"NIKKEI STOCK AVERAGE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.240743.Net",		"NIKKEI STOCK AVERAGE YEN DENOM - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.244041.Net",		"MSCI EAFE MINI INDEX - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.244042.Net",		"MSCI EMERGING MKTS MINI INDEX - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.33874A.Net",		"E-MINI S&P 400 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.43874A.Net",		"S&P 500 ANNUAL DIVIDEND INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.020601.Net",		"U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.020604.Net",		"ULTRA U.S. TREASURY BONDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.042601.Net",		"2-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.043602.Net",		"10-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.043607.Net",		"ULTRA 10-YEAR U.S. T-NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.044601.Net",		"5-YEAR U.S. TREASURY NOTES - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.045601.Net",		"30-DAY FEDERAL FUNDS - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.132741.Net",		"3-MONTH EURODOLLARS - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.134741.Net",		"3-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.134742.Net",		"1-MONTH SOFR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.246605.Net",		"10 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.246606.Net",		"5 YEAR DELIVERABLE IR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.343601.Net",		"10 YEAR ERIS SWAP  - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.344601.Net",		"5 YEAR ERIS SWAP - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.1330E1.Net",		"BITCOIN-USD - CBOE FUTURES EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.133741.Net",		"BITCOIN - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.098662.Net",		"U.S. DOLLAR INDEX - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.1170E1.Net",		"VIX FUTURES - CBOE FUTURES EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.221602.Net",		"BLOOMBERG COMMODITY INDEX - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.138747.Net",		"E-MINI S&P CONSUMER DISC INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.239744.Net",		"EMINI RUSSELL 1000 VALUE INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.23977A.Net",		"RUSSELL 2000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.ZB9105.Net",		"5-Year Eris Standard- Initial - ERIS FUTURES EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.23977C.Net",		"RUSSELL 1000 VALUE INDEX MINI - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.256741.Net",		"S&P GSCI COMMODITY INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.299661.Net",		"EURO FX/JAPANESE YEN - SMALL - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.599661.Net",		"EURO FX/BRITISH POUND - SMALL - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.124601.Net",		"DOW JONES INDUSTRIAL AVERAGE - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.209741.Net",		"NASDAQ-100 STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.239741.Net",		"Russell 2000 Stock Index Future - Chicago Mercantile Exchange (Net Positions)"	},
				{	"COT.FinFutOpt.239772.Net",		"RUSSEL 1000 MINI INDEX FUTURE - ICE FUTURES U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.239777.Net",		"Russell 2000 Stock Index - ICE Futures U.S. (Net Positions)"	},
				{	"COT.FinFutOpt.244741.Net",		"E-MINI MSCI EAFE - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.244742.Net",		"E-MINI MSCI EMERGING MARKETS - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.338741.Net",		"S&P 400 MIDCAP STOCK INDEX - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.045L2T.Net",		"DTCC Repo-US Treasury <30 YR - NYSE LIFFE - NYPC (Net Positions)"	},
				{	"COT.FinFutOpt.032741.Net",		"ONE-MONTH EURODOLLAR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	},
				{	"COT.FinFutOpt.246602.Net",		"INTEREST RATE SWAPS 10YR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.247602.Net",		"INTEREST RATE SWAPS 5YR - CHICAGO BOARD OF TRADE (Net Positions)"	},
				{	"COT.FinFutOpt.597741.Net",		"3-MO. EUROYEN TIBOR - CHICAGO MERCANTILE EXCHANGE (Net Positions)"	}
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
					Name										= "DMFX - COT: Financials - Futures & Options (Net Positions)";
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
                    COTCode = NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes.EURO_FX__CHICAGO_MERCANTILE_EXCHANGE_Net_Positions;
					WeeksBack = 500;
					
					AddPlot(Brushes.Red, "Dealer_Positions_Net");
					AddPlot(Brushes.Orange, "Asset_Mgr_Positions_Net");
					AddPlot(Brushes.Blue, "Lev_Money_Positions_Net");
					AddPlot(Brushes.Green, "Other_Rept_Positions_Net");
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
		public NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes COTCode
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
		private DMFXCOTFinFutOptNetPositions[] cacheDMFXCOTFinFutOptNetPositions;
		public DMFXCOTFinFutOptNetPositions DMFXCOTFinFutOptNetPositions(string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return DMFXCOTFinFutOptNetPositions(Input, accountKey, cOTCode, weeksBack, host);
		}

		public DMFXCOTFinFutOptNetPositions DMFXCOTFinFutOptNetPositions(ISeries<double> input, string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			if (cacheDMFXCOTFinFutOptNetPositions != null)
				for (int idx = 0; idx < cacheDMFXCOTFinFutOptNetPositions.Length; idx++)
					if (cacheDMFXCOTFinFutOptNetPositions[idx] != null && cacheDMFXCOTFinFutOptNetPositions[idx].AccountKey == accountKey && cacheDMFXCOTFinFutOptNetPositions[idx].COTCode == cOTCode && cacheDMFXCOTFinFutOptNetPositions[idx].WeeksBack == weeksBack && cacheDMFXCOTFinFutOptNetPositions[idx].Host == host && cacheDMFXCOTFinFutOptNetPositions[idx].EqualsInput(input))
						return cacheDMFXCOTFinFutOptNetPositions[idx];
			return CacheIndicator<DMFXCOTFinFutOptNetPositions>(new DMFXCOTFinFutOptNetPositions(){ AccountKey = accountKey, COTCode = cOTCode, WeeksBack = weeksBack, Host = host }, input, ref cacheDMFXCOTFinFutOptNetPositions);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.DMFXCOTFinFutOptNetPositions DMFXCOTFinFutOptNetPositions(string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFutOptNetPositions(Input, accountKey, cOTCode, weeksBack, host);
		}

		public Indicators.DMFXCOTFinFutOptNetPositions DMFXCOTFinFutOptNetPositions(ISeries<double> input , string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFutOptNetPositions(input, accountKey, cOTCode, weeksBack, host);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.DMFXCOTFinFutOptNetPositions DMFXCOTFinFutOptNetPositions(string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFutOptNetPositions(Input, accountKey, cOTCode, weeksBack, host);
		}

		public Indicators.DMFXCOTFinFutOptNetPositions DMFXCOTFinFutOptNetPositions(ISeries<double> input , string accountKey, NinjaTrader.NinjaScript.Indicators.DMFXCOTFinFutOptNetPositions.ECOTFinCodes cOTCode, int weeksBack, string host)
		{
			return indicator.DMFXCOTFinFutOptNetPositions(input, accountKey, cOTCode, weeksBack, host);
		}
	}
}

#endregion
