using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IStaffLoansMgmtRepo
    {
        ProcessResponse SaveStaffLoan(StaffLoans request);
        List<StaffLoansDisplay> GetAllStaffLoans();
        StaffLoans GetStaffLoanById(int id);
        List<StaffLoans> GetAllLoansByUserId(int id);
        ProcessResponse UpdateStaffLoan(StaffLoans request);
        ProcessResponse SaveLoanReceipts(StaffLoanReceipts request);
        List<StaffLoanReceipts> GetAllLoanReceipts();
        StaffLoanReceipts GetLoanReceiptById(int id);
        List<StaffLoanReceipts> GetReciptsByLoanId(int loanId);
        ProcessResponse UpdateStaffLoanReceipt(StaffLoanReceipts request);
        void LogError(Exception ex);
    }
}
