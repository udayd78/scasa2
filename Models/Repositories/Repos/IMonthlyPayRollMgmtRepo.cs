using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IMonthlyPayRollMgmtRepo
    {
        ProcessResponse SaveMonthPay(MonthlyPayRoll request);
        List<MPayRollDisplayModel> GetAllMonthPayRolls();
        MonthlyPayRoll GetMPayRollBypayRollId(int id);
        MPayRollDisplayModel GetMPayRollByStaffIdWithDate(int sid, int month, int year);
        List<MPayRollDisplayModel> GetMPayRollByStaffId(int sid);
        List<MPayRollDisplayModel> GetPayRollByMonth(int month, int year);
        List<MPayRollDisplayModel> GetPayRollsByStaffOfYear(int sid, int year);
        List<MPayRollDisplayModel> GetPayRollsOfYear(int year);
        void LogError(Exception ex);
    }
}
