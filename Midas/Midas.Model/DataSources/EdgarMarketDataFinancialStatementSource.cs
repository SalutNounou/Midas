using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Midas.Model.Documents;
using Newtonsoft.Json.Linq;
using log4net;

namespace Midas.Model.DataSources
{
    public class EdgarMarketDataFinancialStatementSource : IFinancialStatementSource
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EdgarMarketDataFinancialStatementSource));

        private readonly string _apiKey;

        public EdgarMarketDataFinancialStatementSource(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<ObservableCollection<FinancialStatement>> GetQuarterlyFinancialStatementsAsync(string ticker)
        {
            try
            {
                string jsonData;

                using (WebClient web = new WebClient())
                {
                    jsonData = await web.DownloadStringTaskAsync(string.Format("http://edgaronline.api.mashery.com/v2/corefinancials/qtr.json?primarysymbols={0}&appkey={1}", ticker, _apiKey));
                }
                var results = ParseFinancialStatements(jsonData);
                return results;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0} - {1}", exc.Message, exc.StackTrace));
                throw;
            }
        }


        public ObservableCollection<FinancialStatement> GetQuarterlyFinancialStatements(string ticker)
        {
            try
            {
                string jsonData;

                using (WebClient web = new WebClient())
                {
                    jsonData =  web.DownloadString(string.Format("http://edgaronline.api.mashery.com/v2/corefinancials/qtr.json?primarysymbols={0}&appkey={1}", ticker, _apiKey));
                }
                var results = ParseFinancialStatements(jsonData);
                return results;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0} - {1}", exc.Message, exc.StackTrace));
                throw;
            }
        }


        public ObservableCollection<FinancialStatement> GetQuarterlyFinancialStatements(IEnumerable<string> tickers)
        {
            try
            {
                string jsonData;

                using (WebClient web = new WebClient())
                {
                    jsonData = web.DownloadString(string.Format("http://edgaronline.api.mashery.com/v2/corefinancials/qtr.json?primarysymbols={0}&limit=1000&appkey={1}", string.Join(",",tickers), _apiKey));
                }
                var results = ParseFinancialStatements(jsonData);
                return results;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0} - {1}", exc.Message, exc.StackTrace));
                throw;
            }
        }




        public async Task<ObservableCollection<FinancialStatement>> GetAnnualFinancialStatementsAsync(string ticker)
        {
            try
            {
                string jsonData;

                using (WebClient web = new WebClient())
                {
                    jsonData = await web.DownloadStringTaskAsync(string.Format("http://edgaronline.api.mashery.com/v2/corefinancials/ann.json?primarysymbols={0}&appkey={1}", ticker, _apiKey));
                }
                var results = ParseFinancialStatements(jsonData);
                return results;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0} - {1}", exc.Message, exc.StackTrace));
                throw;
            }
        }


        public ObservableCollection<FinancialStatement> GetAnnualFinancialStatements(string ticker)
        {
            try
            {
                string jsonData;

                using (WebClient web = new WebClient())
                {
                    jsonData = web.DownloadString(string.Format("http://edgaronline.api.mashery.com/v2/corefinancials/ann.json?primarysymbols={0}&appkey={1}", ticker, _apiKey));
                }
                var results = ParseFinancialStatements(jsonData);
                return results;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0} - {1}", exc.Message, exc.StackTrace));
                throw;
            }
        }

        public ObservableCollection<FinancialStatement> GetAnnualFinancialStatements(IEnumerable<string> tickers)
        {
            try
            {
                string jsonData;

                using (WebClient web = new WebClient())
                {
                    jsonData = web.DownloadString(string.Format("http://edgaronline.api.mashery.com/v2/corefinancials/ann.json?primarysymbols={0}&limit=1000&appkey={1}", string.Join(",", tickers), _apiKey));
                }
                var results = ParseFinancialStatements(jsonData);
                return results;
            }
            catch (Exception exc)
            {
                Log.Error(string.Format("{0} - {1}", exc.Message, exc.StackTrace));
                throw;
            }
        }


        private ObservableCollection<FinancialStatement> ParseFinancialStatements(string jsonData)
        {
            dynamic statements = JObject.Parse(jsonData);
            var results = new ObservableCollection<FinancialStatement>();
            if (statements != null && statements.result != null && statements.result.rows != null)
            {
                foreach (var row in statements.result.rows.Children())
                {
                    FinancialStatement statement = new FinancialStatement();
                    foreach (var entry in row.values.Children())
                    {
                        string field = entry.field;
                        string value = entry.value;
                        if (Setters.ContainsKey(field) && value != null && value != "null")
                        {
                            Setters[field](statement, value);
                        }
                    }
                    results.Add(statement);
                }
            }
            return results;
        }

        private static readonly Dictionary<string, Action<FinancialStatement, string>> Setters =
            new Dictionary<string, Action<FinancialStatement, string>>
            {
                #region Metadata
                {"cik", (f,s)=>f.Cik=s },
                {"companyname", (f,s)=>f.CompanyName=s },
                {"entityid", (f,s)=>f.EntityId=s },
                {"primaryexchange", (f,s)=>f.PrimaryExchange=s },
                {"primarysymbol", (f,s)=>f.PrimarySymbol=s },
                {"siccode", (f,s)=>f.SicCode=s },
                {"sicdescription", (f,s)=>f.SicDescription=s },
                //{"usdconversionrate", (f,s)=>f.UsdConversionRate=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"restated", (f,s)=>f.Restated=Convert.ToBoolean(s) },
                {"receiveddate", (f,s)=>f.ReceivedDate=Convert.ToDateTime(s, CultureInfo.InvariantCulture) },
                {"preliminary", (f,s)=>f.Preliminary=Convert.ToBoolean(s) },
                {"periodlengthcode", (f,s)=>f.PeriodLengthCode=s },
                {"periodlength", (f,s)=>f.PeriodLength=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"periodenddate", (f,s)=>f.PeriodEnd=Convert.ToDateTime(s,CultureInfo.InvariantCulture) },
                {"original", (f,s)=>f.Original=Convert.ToBoolean(s) },
                {"formtype", (f,s)=>f.FormType=s },
                {"fiscalyear", (f,s)=>f.FiscalYear=Convert.ToInt32(s) },
                {"fiscalquarter", (f,s)=>f.FiscalQuarter=Convert.ToInt32(s) },
                {"dcn", (f,s)=>f.Dcn=s },
                {"currencycode", (f,s)=>f.CurrencyCode=s },
                {"crosscalculated", (f,s)=>f.CrossCalculated=Convert.ToBoolean(s) },
                {"audited", (f,s)=>f.Audited=Convert.ToBoolean(s) },
                {"amended", (f,s)=>f.Amended=Convert.ToBoolean(s) },
                #endregion Metadata
                {"changeincurrentassets", (f,s)=>f.CashFlowStatement.ChangeInCurrentAsset=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"changeincurrentliabilities", (f,s)=>f.CashFlowStatement.ChangeInCurrentLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"changeininventories", (f,s)=>f.CashFlowStatement.ChangeInInventories=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"dividendspaid", (f,s)=>f.CashFlowStatement.DividendsPaid=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"effectofexchangerateoncash", (f,s)=>f.CashFlowStatement.EffectOfExchangeRateOnCash=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"capitalexpenditures", (f,s)=>f.CashFlowStatement.CapitalExpanditure=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },


                {"cashfromfinancingactivities", (f,s)=>f.CashFlowStatement.CashFromFinancingActivities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cashfrominvestingactivities", (f,s)=>f.CashFlowStatement.CashFromInvestingActivities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cashfromoperatingactivities", (f,s)=>f.CashFlowStatement.CashFromOperatingActivities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cfdepreciationamortization", (f,s)=>f.CashFlowStatement.CfDepreciationAmortization=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"changeinaccountsreceivable", (f,s)=>f.CashFlowStatement.ChangeInAccountReceivable=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"investmentchangesnet", (f,s)=>f.CashFlowStatement.InvestmentChangesNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"netchangeincash", (f,s)=>f.CashFlowStatement.NetChangeInCash=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totaladjustments", (f,s)=>f.CashFlowStatement.TotalAdjustments=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"ebit", (f,s)=>f.IncomeStatement.Ebit=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"costofrevenue", (f,s)=>f.IncomeStatement.CostOfRevenue=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"discontinuedoperations", (f,s)=>f.IncomeStatement.DiscontinuedOperation=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"equityearnings", (f,s)=>f.IncomeStatement.EquityEarnings=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"accountingchange", (f,s)=>f.CashFlowStatement.AccountingChange=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"extraordinaryitems", (f,s)=>f.IncomeStatement.ExtraordinaryItems=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"grossprofit", (f,s)=>f.IncomeStatement.GrossProfit=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"incomebeforetaxes", (f,s)=>f.IncomeStatement.IncomeBeforeTaxes=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"interestexpense", (f,s)=>f.IncomeStatement.InterestExpense=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"netincome", (f,s)=>f.IncomeStatement.NetIncome=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"netincomeapplicabletocommon", (f,s)=>f.IncomeStatement.NetIncomeApplicableToCommon=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"researchdevelopmentexpense", (f,s)=>f.IncomeStatement.ResearchDevelopementExpense=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalrevenue", (f,s)=>f.IncomeStatement.TotalRevenue=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"sellinggeneraladministrativeexpenses", (f,s)=>f.IncomeStatement.SellingGeneralAdministrativeExpense=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"commonstock", (f,s)=>f.BalanceSheet.CommonStock=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"deferredcharges", (f,s)=>f.BalanceSheet.DeferredCHarges=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },


                {"cashandcashequivalents", (f,s)=>f.BalanceSheet.CashAndCashEquivalent=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"cashcashequivalentsandshortterminvestments", (f,s)=>f.BalanceSheet.CashCashEquivalentAndShortTermInvestments=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"goodwill", (f,s)=>f.BalanceSheet.Goodwill=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"intangibleassets", (f,s)=>f.BalanceSheet.IntangibleAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"inventoriesnet", (f,s)=>f.BalanceSheet.InventoriesNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"minorityinterest", (f,s)=>f.BalanceSheet.MinorityInterest=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"otherassets", (f,s)=>f.BalanceSheet.OtherAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"othercurrentassets", (f,s)=>f.BalanceSheet.OtherCurrentAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"othercurrentliabilities", (f,s)=>f.BalanceSheet.OtherCurrentLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"otherequity", (f,s)=>f.BalanceSheet.OtherEquity=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"otherliabilities", (f,s)=>f.BalanceSheet.OtherLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"preferredstock", (f,s)=>f.BalanceSheet.PreferredStock=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"propertyplantequipmentnet", (f,s)=>f.BalanceSheet.PropertyPlantEquipmentNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"retainedearnings", (f,s)=>f.BalanceSheet.RetainedEarnings=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalassets", (f,s)=>f.BalanceSheet.TotalAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalcurrentassets", (f,s)=>f.BalanceSheet.TotalCurrentAssets=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalcurrentliabilities", (f,s)=>f.BalanceSheet.TotalCurrentLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalliabilities", (f,s)=>f.BalanceSheet.TotalLiabilities=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },

                {"totallongtermdebt", (f,s)=>f.BalanceSheet.TotalLongTermDebt=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalreceivablesnet", (f,s)=>f.BalanceSheet.TotalReceivableNet=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalshorttermdebt", (f,s)=>f.BalanceSheet.TotalShortTermDebt=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"totalstockholdersequity", (f,s)=>f.BalanceSheet.TotalStockHolderEquity=Convert.ToDecimal(s, CultureInfo.InvariantCulture) },
                {"treasurystock", (f,s)=>f.BalanceSheet.TreasuryStock=Convert.ToDecimal(s, CultureInfo.InvariantCulture) }
            };
    }
}