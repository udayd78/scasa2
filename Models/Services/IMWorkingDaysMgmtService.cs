using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IMWorkingDaysMgmtService
    {

        ProcessResponse SaveDays(MonthlyWorkingDays request);

        List<MWorkingDaysDisplay> GetAllDays(int MonthNumber, int YearNumber);

        MonthlyWorkingDays GetDaysById(int id);
    }
}
