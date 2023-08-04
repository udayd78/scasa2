using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IMWorkingDaysMgmtRepo
    {

        ProcessResponse SaveWorkingDays(MonthlyWorkingDays request);

        List<MWorkingDaysDisplay> GetAllWorkingDays();

        MWorkingDaysDisplay GetDaysByMonYer(int MonthNumber, int YearNumber);

        ProcessResponse UpdateDays(MonthlyWorkingDays request);
        MonthlyWorkingDays GetDaysById(int id);
        void LogError(Exception ex);

    }
}
