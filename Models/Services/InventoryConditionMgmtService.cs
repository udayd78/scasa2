using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class InventoryConditionMgmtService : IInventoryConditionMgmtService
    {
        private readonly IInventoryConditionMgmtRepo uRepo;


        public InventoryConditionMgmtService(IInventoryConditionMgmtRepo InventoryConditionMgmtRepo)
        {
            uRepo = InventoryConditionMgmtRepo;
        }

        public ProcessResponse SaveConditionType(InventoryConditionMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.ConditionId > 0)
                {
                    InventoryConditionMaster Ic = new InventoryConditionMaster();
                    Ic = uRepo.GetAllConditionTypeById(request.ConditionId);
                    Ic.ConditionName = request.ConditionName;
                    response = uRepo.UpdateConditionType(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = uRepo.SaveConditionType(request);
                }


            }
            catch (Exception ex)
            {
                uRepo.LogError(ex);
            }

            return response;
        }
        public List<InventoryConditionMaster> GetAllCondition()
        {
            return uRepo.GetAllCondition();
        }
        public InventoryConditionMaster GetAllConditionTypeById(int id)
        {
            return uRepo.GetAllConditionTypeById(id);
        }
        public ProcessResponse UpdateConditionType(InventoryConditionMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = uRepo.UpdateConditionType(request);
            }
            catch (Exception ex)
            {
                uRepo.LogError(ex);
            }
            return response;
        }

  
    }
}














