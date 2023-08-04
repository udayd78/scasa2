using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class ReportsMgmgtRepo : IReportsMgmgtRepo
    {
        private readonly MyDbContext context;
        public ReportsMgmgtRepo(MyDbContext _context)
        {
            context = _context;
        }
        public List<ReportsDataModel> GetDailyReports()
        {
            List<ReportsDataModel> result = new List<ReportsDataModel>();
            try
            {
                UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Sales Executive").FirstOrDefault();
                List<UserMaster> sEList = context.userMasters.Where(a => a.IsDeleted == false && a.CurrentStatus == "Active" && a.UserTypeId == utm.TypeId).ToList();
                DateTime curentDate = DateTime.Now;
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                DateTime eDate = sDate.AddDays(1);
                foreach (UserMaster u in sEList)
                {

                    ReportsDataModel r = new ReportsDataModel();
                    r.sEName = u.UserName;
                    r.attendCnt = context.customerSalesActivities.Where(a => a.IsDeleted == false && a.AttendedOn >= sDate && a.AttendedOn <= eDate && a.StaffId == u.UserId).Select(b => b.CustomerId).Count();
                    r.closedCount = context.salesOrderMasters.Where(a => a.StaffId == u.UserId && a.IsDeleted == false && a.CreatedOn >= sDate && a.CreatedOn <= eDate).Select(b => b.CustomerId).Count();
                    TargetMaster tm = context.targetMasters.Where(a => a.IsDeleted == false && a.UserId == u.UserId && a.MonthNumber == curentDate.Month && a.YearNumber == curentDate.Year).FirstOrDefault();
                    if (tm != null)
                    {
                        r.AchievedTarget = (decimal)tm.TargetDone;
                        r.Monthtarget = (decimal)tm.TargetGiven;
                    }
                    var soMs = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.StaffId == u.UserId && a.CreatedOn >= sDate && a.CreatedOn <= eDate).ToList();
                    decimal saleTotal = 0;
                    foreach (var m in soMs)
                    {
                        var soDs = context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == m.SOMId).ToList();
                        foreach (var d in soDs)
                        {
                            saleTotal += (decimal)d.TotalPrice;
                        }
                    }
                    r.salesValue = saleTotal;
                    result.Add(r);
                }

            }
            catch (Exception e)
            {
                LogError(e);
            }

            return result;
        }
        public List<ReportsDataModel> GetWeeklyReports()
        {
            List<ReportsDataModel> result = new List<ReportsDataModel>();
            try
            {
                UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Sales Executive").FirstOrDefault();
                List<UserMaster> sEList = context.userMasters.Where(a => a.IsDeleted == false && a.CurrentStatus == "Active" && a.UserTypeId == utm.TypeId).ToList();
                DateTime curentDate = DateTime.Now;
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
                DateTime eDate = sDate.AddMonths(1).AddDays(-1);
                if (curentDate.DayOfWeek == DayOfWeek.Monday)
                {
                    sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    int sd = curentDate.Day - 1;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    int sd = curentDate.Day - 2;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Thursday)
                {
                    int sd = curentDate.Day - 3;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Friday)
                {
                    int sd = curentDate.Day - 4;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    int sd = curentDate.Day - 5;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    int sd = curentDate.Day - 6;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }

                foreach (UserMaster u in sEList)
                {

                    ReportsDataModel r = new ReportsDataModel();
                    r.sEName = u.UserName;
                    r.attendCnt = context.customerSalesActivities.Where(a => a.IsDeleted == false && a.AttendedOn >= sDate && a.AttendedOn <= eDate && a.StaffId == u.UserId).Select(b => b.CustomerId).Count();
                    r.closedCount = context.salesOrderMasters.Where(a => a.StaffId == u.UserId && a.IsDeleted == false && a.CreatedOn >= sDate && a.CreatedOn <= eDate).Select(b => b.CustomerId).Count();
                    TargetMaster tm = context.targetMasters.Where(a => a.IsDeleted == false && a.UserId == u.UserId && a.MonthNumber == curentDate.Month && a.YearNumber == curentDate.Year).FirstOrDefault();
                    if (tm != null)
                    {
                        r.AchievedTarget = (decimal)tm.TargetDone;
                        r.Monthtarget = (decimal)tm.TargetGiven;
                    }
                    var soMs = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.StaffId == u.UserId && a.CreatedOn >= sDate && a.CreatedOn <= eDate).ToList();
                    decimal saleTotal = 0;
                    foreach (var m in soMs)
                    {
                        var soDs = context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == m.SOMId).ToList();
                        foreach (var d in soDs)
                        {
                            saleTotal += (decimal)d.TotalPrice;
                        }
                    }
                    r.salesValue = saleTotal;
                    result.Add(r);
                }
            }
            catch (Exception e)
            {
                LogError(e);
            }

            return result;
        }
        public List<ReportsDataModel> GetMonthlyReports()
        {
            List<ReportsDataModel> result = new List<ReportsDataModel>();
            try
            {
                UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Sales Executive").FirstOrDefault();
                List<UserMaster> sEList = context.userMasters.Where(a => a.IsDeleted == false && a.CurrentStatus == "Active" && a.UserTypeId == utm.TypeId).ToList();
                DateTime curentDate = DateTime.Now;
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
                DateTime eDate = sDate.AddMonths(1).AddDays(-1);
                foreach (UserMaster u in sEList)
                {

                    ReportsDataModel r = new ReportsDataModel();
                    r.sEName = u.UserName;
                    r.attendCnt = context.customerSalesActivities.Where(a => a.IsDeleted == false && a.AttendedOn >= sDate && a.AttendedOn <= eDate && a.StaffId == u.UserId).Select(b => b.CustomerId).Count();
                    r.closedCount = context.salesOrderMasters.Where(a => a.StaffId == u.UserId && a.IsDeleted == false && a.CreatedOn >= sDate && a.CreatedOn <= eDate).Select(b => b.CustomerId).Count();
                    TargetMaster tm = context.targetMasters.Where(a => a.IsDeleted == false && a.UserId == u.UserId && a.MonthNumber == curentDate.Month && a.YearNumber == curentDate.Year).FirstOrDefault();
                    if (tm != null)
                    {
                        r.AchievedTarget = (decimal)tm.TargetDone;
                        r.Monthtarget = (decimal)tm.TargetGiven;
                    }
                    var soMs = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.StaffId == u.UserId && a.CreatedOn >= sDate && a.CreatedOn <= eDate).ToList();
                    decimal saleTotal = 0;
                    foreach (var m in soMs)
                    {
                        var soDs = context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == m.SOMId).ToList();
                        foreach (var d in soDs)
                        {
                            saleTotal += (decimal)d.TotalPrice;
                        }
                    }
                    r.salesValue = saleTotal;
                    result.Add(r);
                }

            }
            catch (Exception e)
            {
                LogError(e);
            }

            return result;
        }
        //public decimal GetMonthlyCollections()
        //{
        //    decimal result = 0;
        //    try
        //    {
        //        DateTime curentDate = DateTime.Now;
        //        DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
        //        DateTime eDate = sDate.AddMonths(1).AddDays(-1);
        //        result = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Receipt" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Debit).Sum();
        //    }catch(Exception e)
        //    {
        //        LogError(e);
        //    }
        //    return result;
        //}
        //public decimal GetMOnthlyDeliverysTotal()
        //{
        //    decimal result = 0;
        //    try
        //    {
        //        DateTime curentDate = DateTime.Now;
        //        DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
        //        DateTime eDate = sDate.AddMonths(1).AddDays(-1);
        //        var sos = context.salesOrderMasters.Where(a => a.CurrentStatus == "Paid Full" && a.CreatedOn >= sDate && a.CreatedOn <= eDate).ToList();
        //        foreach(var s in sos)
        //        {
        //            result += (decimal)context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == s.SOMId).Select(b => b.TotalPrice).Sum();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LogError(e);
        //    }
        //    return result;
        //}
        //public decimal GetMOnthlyExpendeture()
        //{
        //    decimal result = 0;
        //    try
        //    {
        //        DateTime curentDate = DateTime.Now;
        //        DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
        //        DateTime eDate = sDate.AddMonths(1).AddDays(-1);
        //        result = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Payment" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Credit).Sum();
        //    }
        //    catch (Exception e)
        //    {
        //        LogError(e);
        //    }
        //    return result;
        //}
        public DashBoardValuesList GetDynamicValues()
        {
            DashBoardValuesList result = new DashBoardValuesList();
            result.delivered = 0;
            DateTime curentDate = DateTime.Now;
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
                DateTime eDate = sDate.AddMonths(1).AddDays(-1);
                result.collectionTotal = (decimal)context.recipts.Where(a => a.PaymentDate >= sDate && a.PaymentDate <= eDate && a.IsDeleted == false).Select(b => b.Amount).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
                DateTime eDate = sDate.AddMonths(1).AddDays(-1);
                List<DCMaster> dcs = context.dCMasters.Where(a => a.IsDeleted == false && a.CurrentStatus == "Despatched" && a.DespatchedOn > sDate && a.DespatchedOn < eDate).ToList();
                foreach (DCMaster d in dcs)
                {
                    result.delivered += (decimal)context.deliveryDetails.Where(a => a.IsDeleted == false && a.DeliverMasterId == d.DCId).Sum(b => b.TotalPrice);
                }
                //var sos = context.salesOrderMasters.Where(a => a.CurrentStatus == "Paid Full" && a.CreatedOn >= sDate && a.CreatedOn <= eDate).ToList();
                //foreach (var s in sos)
                //{
                //    result.delivered += (decimal)context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == s.SOMId).Select(b => b.TotalPrice).Sum();
                //}
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
                DateTime eDate = sDate.AddMonths(1).AddDays(-1);
                result.expendeture = (decimal)context.payments.Where(a => a.PaymentDate >= sDate && a.PaymentDate <= eDate && a.IsDeleted == false).Select(b => b.Amount).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                DateTime eDate = sDate.AddDays(1);
                result.dailyReceipt = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Receipt" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Credit).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
                DateTime eDate = sDate.AddMonths(1).AddDays(-1);
                if (curentDate.DayOfWeek == DayOfWeek.Monday)
                {
                    sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    int sd = curentDate.Day - 1;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Wednesday)
                {
                    int sd = curentDate.Day - 2;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Thursday)
                {
                    int sd = curentDate.Day - 3;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Friday)
                {
                    int sd = curentDate.Day - 4;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    int sd = curentDate.Day - 5;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                else if (curentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    int sd = curentDate.Day - 6;
                    sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                    eDate = sDate.AddDays(7);
                }
                result.weeklyReceipt = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Receipt" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Credit).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            return result;
        }
        public DashBoardValuesList GetDailyDynamicValues()
        {
            DashBoardValuesList result = new DashBoardValuesList();
            result.delivered = 0;
            DateTime curentDate = DateTime.Now;
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                DateTime eDate = sDate.AddDays(1).AddSeconds(-1);
                result.collectionTotal = (decimal)context.recipts.Where(a => a.PaymentDate >= sDate && a.PaymentDate <= eDate && a.IsDeleted == false).Select(b => b.Amount).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                DateTime eDate = sDate.AddDays(1).AddSeconds(-1);
                List<DCMaster> dcs = context.dCMasters.Where(a => a.IsDeleted == false && a.CurrentStatus == "Despatched" && a.DespatchedOn > sDate && a.DespatchedOn < eDate).ToList();
                foreach (DCMaster d in dcs)
                {
                    result.delivered += (decimal)context.deliveryDetails.Where(a => a.IsDeleted == false && a.DeliverMasterId == d.DCId).Sum(b => b.TotalPrice);
                    //result.delivered += (decimal)context.invoiceItemDetails.Where(a => a.IsDeleted == false && a.InvoiceId == d.InvoiceId).Sum(b => b.TotalPrice);
                }
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                DateTime eDate = sDate.AddDays(1).AddSeconds(-1);
                result.expendeture = (decimal)context.payments.Where(a => a.PaymentDate >= sDate && a.PaymentDate <= eDate && a.IsDeleted == false).Select(b => b.Amount).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                DateTime eDate = sDate.AddDays(1).AddSeconds(-1);
                //result.dailyReceipt = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Receipt" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Credit).Sum();
                var soMs = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.CreatedOn >= sDate && a.CreatedOn <= eDate).ToList();
                result.dailyReceipt = 0;
                foreach (var so in soMs)
                {
                    result.dailyReceipt += (decimal)context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == so.SOMId).Select(b => b.TotalPrice).Sum();
                }
            }
            catch (Exception e)
            {
                LogError(e);
            }
            //try
            //{
            //    DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
            //    DateTime eDate = sDate.AddMonths(1).AddDays(-1);
            //    if (curentDate.DayOfWeek == DayOfWeek.Monday)
            //    {
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Tuesday)
            //    {
            //        int sd = curentDate.Day - 1;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Wednesday)
            //    {
            //        int sd = curentDate.Day - 2;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Thursday)
            //    {
            //        int sd = curentDate.Day - 3;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Friday)
            //    {
            //        int sd = curentDate.Day - 4;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Saturday)
            //    {
            //        int sd = curentDate.Day - 5;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        int sd = curentDate.Day - 6;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    result.weeklyReceipt = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Receipt" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Credit).Sum();
            //}
            //catch (Exception e)
            //{
            //    LogError(e);
            //}
            return result;
        }
        public DashBoardValuesList GetWeaklyDynamicValues()
        {
            DashBoardValuesList result = new DashBoardValuesList();
            result.delivered = 0;
            DateTime curentDate = DateTime.Now;
            DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
            DateTime eDate = sDate.AddDays(1).AddSeconds(-1);
            if (curentDate.DayOfWeek == DayOfWeek.Monday)
            {
                sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
                eDate = sDate.AddDays(7);
            }
            else if (curentDate.DayOfWeek == DayOfWeek.Tuesday)
            {
                int sd = curentDate.Day - 1;
                sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                eDate = sDate.AddDays(7);
            }
            else if (curentDate.DayOfWeek == DayOfWeek.Wednesday)
            {
                int sd = curentDate.Day - 2;
                sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                eDate = sDate.AddDays(7);
            }
            else if (curentDate.DayOfWeek == DayOfWeek.Thursday)
            {
                int sd = curentDate.Day - 3;
                sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                eDate = sDate.AddDays(7);
            }
            else if (curentDate.DayOfWeek == DayOfWeek.Friday)
            {
                int sd = curentDate.Day - 4;
                sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                eDate = sDate.AddDays(7);
            }
            else if (curentDate.DayOfWeek == DayOfWeek.Saturday)
            {
                int sd = curentDate.Day - 5;
                sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                eDate = sDate.AddDays(7);
            }
            else if (curentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                int sd = curentDate.Day - 6;
                sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
                eDate = sDate.AddDays(7);
            }
            try
            {

                result.collectionTotal = (decimal)context.recipts.Where(a => a.PaymentDate >= sDate && a.PaymentDate <= eDate && a.IsDeleted == false).Select(b => b.Amount).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                List<DCMaster> dcs = context.dCMasters.Where(a => a.IsDeleted == false && a.CurrentStatus == "Despatched" && a.DespatchedOn > sDate && a.DespatchedOn < eDate).ToList();
                foreach (DCMaster d in dcs)
                {
                    result.delivered += (decimal)context.deliveryDetails.Where(a => a.IsDeleted == false && a.DeliverMasterId == d.DCId).Sum(b => b.TotalPrice);
                    //result.delivered += (decimal)context.invoiceItemDetails.Where(a => a.IsDeleted == false && a.InvoiceId == d.InvoiceId).Sum(b => b.TotalPrice);
                }
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                result.expendeture = (decimal)context.payments.Where(a => a.PaymentDate >= sDate && a.PaymentDate <= eDate && a.IsDeleted == false).Select(b => b.Amount).Sum();
            }
            catch (Exception e)
            {
                LogError(e);
            }
            try
            {
                //result.dailyReceipt = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Receipt" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Credit).Sum();
                var soMs = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.CreatedOn >= sDate && a.CreatedOn <= eDate).ToList();
                result.dailyReceipt = 0;
                foreach (var so in soMs)
                {
                    result.dailyReceipt += (decimal)context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == so.SOMId).Select(b => b.TotalPrice).Sum();
                }
            }
            catch (Exception e)
            {
                LogError(e);
            }
            //try
            //{
            //    DateTime sDate = new DateTime(curentDate.Year, curentDate.Month, 1);
            //    DateTime eDate = sDate.AddMonths(1).AddDays(-1);
            //    if (curentDate.DayOfWeek == DayOfWeek.Monday)
            //    {
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, curentDate.Day);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Tuesday)
            //    {
            //        int sd = curentDate.Day - 1;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Wednesday)
            //    {
            //        int sd = curentDate.Day - 2;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Thursday)
            //    {
            //        int sd = curentDate.Day - 3;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Friday)
            //    {
            //        int sd = curentDate.Day - 4;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Saturday)
            //    {
            //        int sd = curentDate.Day - 5;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    else if (curentDate.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        int sd = curentDate.Day - 6;
            //        sDate = new DateTime(curentDate.Year, curentDate.Month, sd);
            //        eDate = sDate.AddDays(7);
            //    }
            //    result.weeklyReceipt = (decimal)context.financeTransactions.Where(a => a.VoucherType == "Receipt" && a.DateOfTransaction >= sDate && a.DateOfTransaction <= eDate).Select(b => b.Credit).Sum();
            //}
            //catch (Exception e)
            //{
            //    LogError(e);
            //}
            return result;
        }
        public StockBallanceTable CreateStock()
        {
            StockBallanceTable presbnew = new StockBallanceTable();
            try
            {
                StockBallanceTable newsb = new StockBallanceTable();
                List<InventoryMaster> total = context.inventoryMasters.Where(a => a.IsDeleted == false).ToList();
                DateTime curr = DateTime.Now;
                newsb.Month = curr.Month;
                newsb.Year = curr.Year;
                newsb.IsDeleted = false;
                InventoryStatusMaster ism = context.inventoryStatusMasters.Where(a => a.StatusName == "In Transit" && a.IsDeleted == false).FirstOrDefault();
                InventoryLocationMaster ilms = context.inventoryLocationMasters.Where(a => a.IsDeleted == false && a.LocationName == "Showroom").FirstOrDefault();
                InventoryLocationMaster ilmw = context.inventoryLocationMasters.Where(a => a.IsDeleted == false && a.LocationName == "Warehouse").FirstOrDefault();
                decimal sro = (decimal)total.Where(a => a.InventoryStatusId != ism.StatusId && a.InventoryLocationId == ilms.locationId).Sum(b => b.MRPPrice);
                decimal who = (decimal)total.Where(a => a.InventoryStatusId != ism.StatusId && a.InventoryLocationId == ilmw.locationId).Sum(b => b.MRPPrice);
                decimal ito = (decimal)total.Where(a => a.InventoryStatusId == ism.StatusId && a.InventoryLocationId == ilmw.locationId).Sum(b => b.MRPPrice);
                newsb.SROpeningBal = sro;
                newsb.WHOpeningBal = who;
                newsb.ITOpeningBal = ito;
                newsb.ITClosingBal = 0;
                newsb.SRClosingBAl = 0;
                newsb.WHClosingBal = 0;
                context.Add(newsb);
                context.SaveChanges();
                DateTime predt = curr.AddDays(-1);
                presbnew = context.stockBallanceTables.Where(a => a.IsDeleted == false && a.Month == predt.Month && a.Year == predt.Year).FirstOrDefault();
                StockBallanceTable presbold = presbnew;
                presbnew.SRClosingBAl = sro;
                presbnew.WHClosingBal = who;
                presbnew.ITClosingBal = ito;
                context.Entry(presbold).CurrentValues.SetValues(presbnew);
                context.SaveChanges();
                //pr.currentId = newsb.PrId;
                //pr.statusCode = 1;
                //pr.statusMessage = "Sucess";
            }
            catch(Exception e)
            {
                //pr.statusCode = 0;
                //pr.statusMessage = "Failed";
                LogError(e);
            }
            return presbnew;
        }
        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }
    }
}