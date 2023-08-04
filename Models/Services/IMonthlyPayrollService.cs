using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IMonthlyPayrollService
    {
        ProcessResponse SaveMonthlyPayRoll(int sId, int month, int year, int createdById);
        ProcessResponse UpdateMonthlyPayroll(MonthlyPayRoll request);
        List<MPayRollDisplayModel> GetMPayRollDisplays(int StaffId, int month, int year);
        MonthlyPayRoll GetMonthlyPayRollById(int mId);
    }
}
