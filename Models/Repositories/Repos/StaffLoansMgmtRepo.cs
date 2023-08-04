using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class StaffLoansMgmtRepo : IStaffLoansMgmtRepo
    {
        private readonly MyDbContext context;
        public StaffLoansMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }

        public ProcessResponse SaveStaffLoan(StaffLoans request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.staffLoans.Add(request);
                context.SaveChanges();
                response.currentId = request.LoanId;
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }

            return response;
        }

        public List<StaffLoans> GetAllLoansByUserId(int id)
        {
            List<StaffLoans> response = new List<StaffLoans>();
            try
            {
                response = context.staffLoans.Where(a => a.IsDeleted == false && a.StaffId == id && a.LoanStatus=="Open").ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public List<StaffLoansDisplay> GetAllStaffLoans()
        {
            List<StaffLoansDisplay> response = new List<StaffLoansDisplay>();
            try
            {
                response = (from s in context.staffLoans
                            join u in context.userMasters on s.StaffId equals u.UserId
                            where s.IsDeleted == false
                            select new StaffLoansDisplay
                            {
                              LoanId=s.LoanId,
                              StaffId=s.StaffId,
                              AmountTaken=s.AmountTaken,
                              TakenOn=s.TakenOn,
                              RepaymentMode=s.RepaymentMode,
                              NoofMonths=s.NoofMonths,
                              MonthlyEMI=s.MonthlyEMI,
                              GivenBy=s.GivenBy,
                              LoanStatus=s.LoanStatus,
                              Remarks=s.Remarks,
                              StaffName=u.UserName,
                              EmpCode=u.EmployeeCode

                            }).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public StaffLoans GetStaffLoanById(int id)
        {
            StaffLoans response = new StaffLoans();
            try
            {
                response = context.staffLoans.Where(a => a.IsDeleted == false && a.LoanId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateStaffLoan(StaffLoans request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.LoanId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }

            return response;
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

        public ProcessResponse SaveLoanReceipts(StaffLoanReceipts request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.staffLoanReceipts.Add(request);
                context.SaveChanges();
                response.currentId = request.RecieptId;
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }

            return response;
        }

        public List<StaffLoanReceipts> GetAllLoanReceipts()
        {
            List<StaffLoanReceipts> response = new List<StaffLoanReceipts>();
            response = context.staffLoanReceipts.Where(m => m.IsDeleted == false).ToList();

            return response;
        }
        public StaffLoanReceipts GetLoanReceiptById(int id)
        {
            StaffLoanReceipts response = new StaffLoanReceipts();
            response = context.staffLoanReceipts.Where(m => m.IsDeleted == false && m.RecieptId == id).FirstOrDefault();

            return response;
        }
        public List<StaffLoanReceipts> GetReciptsByLoanId(int loanId)
        {
            List<StaffLoanReceipts> slrs = new List<StaffLoanReceipts>();

            slrs = context.staffLoanReceipts.Where(l => l.IsDeleted == false && l.LoanId == loanId).ToList();

            return slrs;
        }
        public ProcessResponse UpdateStaffLoanReceipt(StaffLoanReceipts request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.RecieptId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }

            return response;
        }
    }
}
