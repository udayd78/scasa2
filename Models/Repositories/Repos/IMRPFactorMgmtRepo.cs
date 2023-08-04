using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IMRPFactorMgmtRepo
    {
        ProcessResponse SaveMRPFactor(MRPFactor request);
        List<MRPFactor> GetAllMRPFactors();
        MRPFactor GetFactorById(int id);
        MRPFactor GetFactorByvalue(decimal fac);
        ProcessResponse UpdateMRPFactors(MRPFactor request);
        void LogError(Exception ex);
    }
}
