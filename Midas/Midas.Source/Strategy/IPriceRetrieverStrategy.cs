using System.Collections.Generic;
using Midas.Model;

namespace Midas.Source.Strategy
{
    public interface IPriceRetrieverStrategy
    {
        IEnumerable<Security> RetrievePriceForSecurities(IEnumerable<Security> securities);
    }
}