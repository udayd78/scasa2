
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
   public interface IInventoryLocationMgmtRepo
    {
        ProcessResponse SaveLocationType(InventoryLocationMaster request);

        List<InventoryLocationMaster> GetAllLocationType();

        InventoryLocationMaster GetAllLocationTypeById(int id);

        ProcessResponse UpdateLocationType(InventoryLocationMaster request);

        void LogError(Exception ex);
        
    }
}
