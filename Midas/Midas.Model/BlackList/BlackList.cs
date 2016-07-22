using System.Collections.Generic;

namespace Midas.Model.BlackList
{
    public class BlackList
    {
        public static readonly Dictionary<string, ReasonForBlackList> Entries = new Dictionary<string, ReasonForBlackList>
        {
            //Chinese Companies
            { "CCCL", ReasonForBlackList.Chinese },
            {"XNY", ReasonForBlackList.Chinese },
            {"AMCO", ReasonForBlackList.Chinese },
            {"OSN", ReasonForBlackList.Chinese },
            {"KBSF", ReasonForBlackList.Chinese },
            {"CLNT", ReasonForBlackList.Chinese },
            {"CGA", ReasonForBlackList.Chinese },
            {"SORL", ReasonForBlackList.Chinese },
            {"CADC", ReasonForBlackList.Chinese },
            {"NVFY", ReasonForBlackList.Chinese },
            {"DHRM", ReasonForBlackList.Chinese },
            {"GURE", ReasonForBlackList.Chinese },
            {"ZX", ReasonForBlackList.Chinese },
            {"ALN", ReasonForBlackList.Chinese },
            {"BORN", ReasonForBlackList.Chinese },
            {"ABAC", ReasonForBlackList.Chinese },
            {"EVK", ReasonForBlackList.Chinese },
            {"CAAS", ReasonForBlackList.Chinese },
            {"SINO", ReasonForBlackList.Chinese },
            {"MOBI", ReasonForBlackList.Chinese },
            {"STV", ReasonForBlackList.Chinese },

            //Revenue is too small compared to capitalization
            {"ADVM", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"CBYL", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"DNAI", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"VSTM", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"CBMX", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"BNTZ", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"AHPI", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"MDGS", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"CHMA", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"NVCN", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"CBIO", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"BIOD", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"MRNS", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"QLTI", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"LPTN", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"PRAN", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"ZFGN", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"BNTC", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"SGNL", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"CMRX", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },

            //Statements Are Weird
            {"EAGL", ReasonForBlackList.StatementsAreWeird },
            {"SRAQ", ReasonForBlackList.StatementsAreWeird },
            {"MPET", ReasonForBlackList.StatementsAreWeird },
            {"ROYT", ReasonForBlackList.StatementsAreWeird },
        };

        public static bool IsBlackListed(string ticker)
        {
            return Entries.ContainsKey(ticker);
        }



    }
}