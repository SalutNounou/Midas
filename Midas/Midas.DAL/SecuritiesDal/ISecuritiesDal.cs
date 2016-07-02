using System.Collections.Generic;
using Midas.Model;

namespace Midas.DAL.SecuritiesDal
{
    public interface ISecuritiesDal
    {
        bool ImportSecurities(IEnumerable<Security> securities);
    }


    public interface ISecurityDalFactory
    {
        ISecuritiesDal GetSecurityDal();
    }


    public class SecurityDalFactory : ISecurityDalFactory
    {
        public ISecuritiesDal GetSecurityDal()
        {
            return new SecurityDalEf();
        }
    }
}