using System.Collections.Generic;
using Midas.Model;

namespace Midas.DAL.SecuritiesDal
{
    public interface ISecuritiesDal
    {
        bool ImportSecurities(IEnumerable<Security> securities);
        IEnumerable<Security> GetAllSecurities();
    }


    public interface ISecurityDalFactory
    {
        ISecuritiesDal GetSecurityDal();
    }


    public class SecurityDalFactory : ISecurityDalFactory
    {

        private static SecurityDalFactory _instance;

        private SecurityDalFactory()
        {
            
        }

        private static readonly object LockObj = new object();
        public static SecurityDalFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (LockObj)
                {
                    _instance = new SecurityDalFactory();
                }
            }
            return _instance;
        }

        public ISecuritiesDal GetSecurityDal()
        {
            return new SecurityDalEf();
        }
    }
}