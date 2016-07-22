using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;
using Midas.Engines.Strategy;
using Midas.Model;

namespace Midas.Engines.Engines
{
    public class StatementEngine : ISourceEngine
    {
        private readonly IStatementRetrieverStrategy _statementStrategy;

        public StatementEngine(IStatementRetrieverStrategy statementStrategy)
        {
            _statementStrategy = statementStrategy;

            ShouldWork = true;
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(StatementEngine));

        private const int BufferSecuritySize = 40;

        public bool ShouldWork { get; private set; }

        public void DoCycle()
        {
            Log.Info("Starting Statement Engine Cycle");
            var securitiesToHandle =
                SecurityDalFactory.GetInstance()
                    .GetSecurityDal()
                    .GetAllSecurities()
                    //.Where(x => x.HasNotANullPrice())
                    //.Where(x => x.HasNotNullMarketCap())
                    .Where(x => x.IsNotADuplicate())
                    .Where(x => !x.StatementsAreUpToDate())
                    .Where(x => x.NbOfFailedAttemptsToGetStatements < 3)
                    .Take(BufferSecuritySize).ToList();


            var toHandle = (IList<Security>)securitiesToHandle;

            if (!toHandle.Any())
            {
                Log.Info("Securities up to date. Stopping the Engine.");
                ShouldWork = false;
                Log.Info("Statement Engine Cycle Ended.");
                return;
            }
            try
            {
                var securitiesAndStatements = _statementStrategy.RetrieveStatementsForSecurities(securitiesToHandle).ToList();
                RefreshSecurities(securitiesAndStatements);
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message);
            }

            Log.Info("Statement Engine Cycle Ended.");
        }

        private static void RefreshSecurities(IEnumerable<SecurityAndStatements> securitiesAndStatements)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new MidasContext()))
                {
                    foreach (var securityAndStatement in securitiesAndStatements)
                    {
                        var security1 = securityAndStatement.Security;
                        var securityDb = unitOfWork.Securities.Find(s => s.Ticker == security1.Ticker).FirstOrDefault();
                        if (securityDb != null)
                        {
                            if (DateTime.Today.ToOADate() - securityDb.DateOfLatestAttemptToGetStatements.ToOADate() > 1 && securityDb.DateOfLatestAttemptToGetStatements > new DateTime(1900, 1, 1))
                                securityDb.NbOfFailedAttemptsToGetStatements = 0;
                            if (!security1.Has10K && !security1.Has10Q)
                                securityDb.NbOfFailedAttemptsToGetStatements++;

                            securityDb.Has10K = security1.Has10K;
                            securityDb.Has10Q = security1.Has10Q;
                            securityDb.Has20F = security1.Has20F;
                            securityDb.Has40F = security1.Has40F;
                            securityDb.DateOfLatest10QFinancialStatement = security1.DateOfLatest10QFinancialStatement;
                            securityDb.DateOfLatest10KFinancialStatement = security1.DateOfLatest10KFinancialStatement;
                            securityDb.DateOfLatest20FFinancialStatement = security1.DateOfLatest20FFinancialStatement;
                            securityDb.DateOfLatest40FFinancialStatement = security1.DateOfLatest40FFinancialStatement;
                            securityDb.DateOfLatestAttemptToGetStatements = DateTime.Today;
                        }
                        var toRemove = unitOfWork.FinancialStatements.Find(f => f.PrimarySymbol == security1.Ticker);
                        unitOfWork.FinancialStatements.RemoveRange(toRemove);
                        unitOfWork.FinancialStatements.AddRange(securityAndStatement.FinancialStatements);
                        var qCount = securityAndStatement.FinancialStatements.Count(f => f.FormType == "10-Q");
                        var kCount = securityAndStatement.FinancialStatements.Count(f => f.FormType == "10-K");
                        var f20Count = securityAndStatement.FinancialStatements.Count(f => f.FormType == "20-F");
                        var f40Count = securityAndStatement.FinancialStatements.Count(f => f.FormType == "40-F");
                        Log.Info(String.Format("Saving {0} 10-Q, {1} 10-K,{2} 20-F and {3} 40-F for security {4}", qCount, kCount, f20Count, f40Count, security1.Ticker));
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(string.Format("{0} - {1}", exception.Message, exception.StackTrace));
            }
        }





        public void StopEngine()
        {
            ShouldWork = false;
        }

    }
}