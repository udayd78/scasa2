using SCASA.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IReportsMgmtService
    {
        List<ReportsDataModel> GetDailyReports();
        List<ReportsDataModel> GetWeeklyReports();
        List<ReportsDataModel> GetMonthlyReports();
        //decimal GetMonthlyCollections();
        //decimal GetMOnthlyDeliverysTotal();
        //decimal GetMOnthlyExpendeture();
        DashBoardValuesList GetDynamicValues();
    }
}
