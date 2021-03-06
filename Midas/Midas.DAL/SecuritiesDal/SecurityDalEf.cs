﻿
using System.Collections.Generic;
using System.Linq;
using Midas.Model;

namespace Midas.DAL.SecuritiesDal
{
    public class SecurityDalEf : ISecuritiesDal
    {
        public bool ImportSecurities(IEnumerable<Security> securities)
        {
            using (var unitOfWork = new UnitOfWork(new MidasContext()))
            {

                var allSecurities = unitOfWork.Securities.GetAll();
                var allSec = allSecurities as IList<Security> ?? allSecurities.ToList();
                foreach (var security in securities)
                {
                    if (allSec.All(s => s.Ticker != security.Ticker))
                    {
                        unitOfWork.Securities.Add(security);
                    }
                   
                }
                unitOfWork.Complete();
            }
            return true;
        }

        public IEnumerable<Security> GetAllSecurities()
        {
            using (var unitOfWork = new UnitOfWork(new MidasContext()))
            {
                return unitOfWork.Securities.GetAll();
            }
        }
    }
}