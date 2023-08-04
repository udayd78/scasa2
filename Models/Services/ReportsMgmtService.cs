using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class ReportsMgmtService : IReportsMgmtService
    {
        private readonly IReportsMgmgtRepo rRepo;
        public ReportsMgmtService(IReportsMgmgtRepo _rRepo)
        {
            rRepo = _rRepo;
        }
        public List<ReportsDataModel> GetDailyReports()
        {
            return rRepo.GetDailyReports();
        }
        public List<ReportsDataModel> GetWeeklyReports()
        {
            return rRepo.GetWeeklyReports();
        }
        public List<ReportsDataModel> GetMonthlyReports()
        {
            return rRepo.GetMonthlyReports();
        }
        //public decimal GetMonthlyCollections()
        //{
        //    return rRepo.GetMonthlyCollections();
        //}
        //public decimal GetMOnthlyDeliverysTotal()
        //{
        //    return rRepo.GetMOnthlyDeliverysTotal();
        //}
        //public decimal GetMOnthlyExpendeture()
        //{
        //    return rRepo.GetMOnthlyExpendeture();
        //}
        public DashBoardValuesList GetDynamicValues()
        {
            return rRepo.GetDynamicValues();
        }
    }
}
