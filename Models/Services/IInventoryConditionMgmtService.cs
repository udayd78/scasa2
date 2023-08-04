using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;

namespace SCASA.Models.Services
{
    public interface IInventoryConditionMgmtService
    {
        InventoryConditionMaster GetAllConditionTypeById(int conditionId);
        ProcessResponse UpdateConditionType(InventoryConditionMaster request);
        ProcessResponse SaveConditionType(InventoryConditionMaster request);
       
      
        List<InventoryConditionMaster> GetAllCondition();
        
    }
}