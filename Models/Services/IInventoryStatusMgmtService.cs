using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IInventoryStatusMgmtService
    {

        ProcessResponse SaveStatusType(InventoryStatusMaster request);
        List<InventoryStatusMaster> GetAllStatus();
        InventoryStatusMaster GetStatusById(int id);
        ProcessResponse UpdateStatusType(InventoryStatusMaster request);


    }
}
