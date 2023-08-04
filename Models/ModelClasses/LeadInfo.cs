using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class LeadInfo
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime date { get; set; }

        public int crfqCount { get; set; }
        public int qouteCount { get; set; }
        public int salesCount { get; set; }
        public int attendedCount { get; set; }

        public int closedCount { get; set; }
        public decimal timeSpent { get; set; }
        public int hotCount { get; set; }
        public TimeSpan? attendedTime { get; set; }
    }
    public class GroupLead
    {
        public List<LeadInfo> leads { get; set; }
    }
    public class ReportsDataModel
    {
        public string sEName { get; set; }
        public decimal salesValue { get; set; }
        public int attendCnt { get; set; }
        public int closedCount { get; set; }
        public decimal ATS { get; set; }
        public decimal Monthtarget { get; set; }
        public decimal AchievedTarget { get;set; }
    }
    public class DashBoardClass
    {
        public List<ReportsDataModel> dailyReport { get; set; }
        public List<ReportsDataModel> weeklyReport { get; set; }
        public List<ReportsDataModel> MonthlyReport { get; set; }
        public DashBoardValuesList FinalValues { get; set; }
    }
    public class DashBoardValuesList
    {
        public decimal SalesTotal { get; set; }
        public decimal collectionTotal { get; set; }
        public decimal delivered { get; set; }
        public decimal expendeture { get; set; }
        public decimal dailyReceipt { get; set; }
        public decimal weeklyReceipt { get; set; }
    }
}
