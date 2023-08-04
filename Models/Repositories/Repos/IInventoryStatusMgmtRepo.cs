using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IInventoryStatusMgmtRepo
    {
        ProcessResponse SaveStatus(InventoryStatusMaster request);
        List<InventoryStatusMaster> GetAllStatusTypes();
        InventoryStatusMaster GetStatusById(int id);
        ProcessResponse UpdateStatusType(InventoryStatusMaster request);
        void LogError(Exception ex);

    }
}
