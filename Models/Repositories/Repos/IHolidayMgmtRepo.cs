using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IHolidayMgmtRepo
    {
        ProcessResponse SaveHoliday(HolidayMaster request);
        List<HolidayMaster> GetAllHolidays();
        HolidayMaster GetHolidayById(int id);
        ProcessResponse UpdateHolidays(HolidayMaster request);
        void LogError(Exception ex);
    }
}
