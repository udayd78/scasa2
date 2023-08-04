using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IShiftMasterMgmtRepo
    {

        ProcessResponse SaveShift(ShiftMaster request);
        List<ShiftMaster> GetAllShifts();
        ShiftMaster GetShiftById(int id);
        ProcessResponse UpdateShifts(ShiftMaster request);
        void LogError(Exception ex);

    }
}
