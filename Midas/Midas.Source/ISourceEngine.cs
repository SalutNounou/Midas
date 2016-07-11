using System;
using System.Collections.Generic;
using System.Linq;
using Midas.DAL;
using Midas.DAL.SecuritiesDal;
using Midas.Model;

namespace Midas.Source
{
    public interface ISourceEngine
    {
        bool ShouldWork { get; }
        void DoCycle();
        void StopEngine();
    }
}