
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IInventoryLocationMgmtService
    {
        ProcessResponse SaveLocationType(InventoryLocationMaster request);

       List<InventoryLocationMaster> GetAllLocationType();

        InventoryLocationMaster GetAllLocationTypeById(int id);

        ProcessResponse UpdateLocationType(InventoryLocationMaster request);
       
    }
}
