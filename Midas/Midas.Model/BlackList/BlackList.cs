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
            {"CALI", ReasonForBlackList.Chinese },

            //Revenue is too small compared to capitalization
            //{"ADVM", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"CBYL", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"OVAS", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"YECO", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"ITEK", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"NVLS", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"ADHD", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"TRIL", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"NSPR", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"ROSG", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"ECYT", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"XNET", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"MATN", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"CYNA", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
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
            {"IFON", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"BSPM", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"OGXI", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"TKAI", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"TAIT", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"TTPH", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"NURO", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"MIRN", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"MIII", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"ROKA", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"TNXP", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
            {"ADVM", ReasonForBlackList.RevenueIsTooSmallComparedToCapitalization },
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