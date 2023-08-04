using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
   public interface IHolidayMgmtService
    {

        ProcessResponse SaveHolidays(Repositories.Entity.HolidayMaster request);
        List<HolidayMaster> GetAllHolidays();
        HolidayMaster GetHolidayById(int id);
        ProcessResponse UpdateHolidays(HolidayMaster request);
    }
}
