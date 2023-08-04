using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IReportsMgmgtRepo
    {
        List<ReportsDataModel> GetDailyReports();
        List<ReportsDataModel> GetWeeklyReports();
        List<ReportsDataModel> GetMonthlyReports();
        //decimal GetMonthlyCollections();
        //decimal GetMOnthlyDeliverysTotal();
        //decimal GetMOnthlyExpendeture();
        DashBoardValuesList GetDynamicValues();
        DashBoardValuesList GetDailyDynamicValues();
        DashBoardValuesList GetWeaklyDynamicValues();
        StockBallanceTable CreateStock();
        void LogError(Exception ex);
    }
}
