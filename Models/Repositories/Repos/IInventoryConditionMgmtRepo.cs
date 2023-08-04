using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
   public interface IInventoryConditionMgmtRepo
    {
        ProcessResponse SaveConditionType(InventoryConditionMaster request);
        List<InventoryConditionMaster> GetAllCondition();
        InventoryConditionMaster GetAllConditionTypeById(int id);
        ProcessResponse UpdateConditionType(InventoryConditionMaster request);
        void LogError(Exception ex);

    }
}
