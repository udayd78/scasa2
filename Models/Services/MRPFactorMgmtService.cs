using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class MRPFactorMgmtService : IMRPFactorMgmtService
    {
        private readonly IMRPFactorMgmtRepo mRepo;
        public MRPFactorMgmtService(IMRPFactorMgmtRepo _mRepo)
        {
            mRepo = _mRepo;
        }
        public ProcessResponse SaveMRPFactor(MRPFactor request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.TRId > 0)
                {
                    MRPFactor um = new MRPFactor();
                    um = mRepo.GetFactorById(request.TRId);
                    um.FactorName = request.FactorName;
                    um.FactorValue = request.FactorValue;
                    

                    response = mRepo.UpdateMRPFactors(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveMRPFactor(request);
                }
            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }
            return response;
        }
        public List<MRPFactor> GetAllMRPFactors()
        {
            return mRepo.GetAllMRPFactors();
        }
        public MRPFactor GetFactorById(int id)
        {
            return mRepo.GetFactorById(id);
        }
        public MRPFactor GetFactorByValue(decimal f)
        {
            return mRepo.GetFactorByvalue(f);
        }
    }
}
