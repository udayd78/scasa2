using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IStaffLoansMgmtService
    {
        ProcessResponse SaveStaffLoans(StaffLoans request);
        List<StaffLoansDisplay> GetAllStaffLoans();
        StaffLoans GetStaffLoanById(int id);
        List<StaffLoans> GetLoansOfUser(int id);

    }
}
