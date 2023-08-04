using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IMRPFactorMgmtService
    {
        ProcessResponse SaveMRPFactor(MRPFactor request);
        List<MRPFactor> GetAllMRPFactors();
        MRPFactor GetFactorById(int id);
        MRPFactor GetFactorByValue(decimal f);

    }
}
