using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
   public interface IShiftMasterMgmtService
   {
        ProcessResponse SaveShift(ShiftMaster request);
        List<ShiftMaster> GetAllShifts();
        ShiftMaster GetShiftById(int id);

   }
}
