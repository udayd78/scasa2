using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class PrevilegeMasterMgmtService : IPrevilegeMasterMgmtService
    {
        private readonly IPrevilegeMasterMgmtRepo pRepo;
        public PrevilegeMasterMgmtService (IPrevilegeMasterMgmtRepo _pRepo)
        {
            pRepo = _pRepo;
        }
        public ProcessResponse SavePrevilege(PrevilegeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = pRepo.SavePrevilage(request);
            }catch(Exception ex)
            {
                pRepo.LogError(ex);
            }
            return response;
        }
        public List<PrevilegeMaster> GetAllPrevileges()
        {
            return pRepo.GetAllPrevilegeTypes();
        }
        public PrevilegeMaster GetPrevilegeById(int id)
        {
            return pRepo.GetPrevilegeById(id);
        }
    }
}
