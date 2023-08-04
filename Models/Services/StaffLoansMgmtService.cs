using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class StaffLoansMgmtService : IStaffLoansMgmtService
    {
        private readonly IStaffLoansMgmtRepo sRepo;
        public StaffLoansMgmtService(IStaffLoansMgmtRepo _sRepo)
        {
            sRepo = _sRepo;
        }

        public ProcessResponse SaveStaffLoans(StaffLoans request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.LoanId > 0)
                {
                    //StaffLoans um = new StaffLoans();
                    //um = sRepo.GetStaffLoanById(request.LoanId);
                    //um.StaffId = request.StaffId;
                    //um.AmountTaken = request.StartTime;
                    //um.EndTime = request.EndTime;

                    //response = sRepo.UpdateShifts(request);
                }
                else
                {
                    //List<StaffLoans> sl = sRepo.GetAllLoansByUserId((int)request.StaffId);
                    //if (sl != null)
                    //{
                    //    request.IsDeleted = false;
                    //    response = sRepo.SaveStaffLoan(request);
                    //}
                    //else
                    //{
                    //    response.statusCode = 0;
                    //    response.statusMessage = "Already Taken";
                    //}
                    request.IsDeleted = false;
                    response = sRepo.SaveStaffLoan(request);
                }
            }
            catch (Exception ex)
            {
                sRepo.LogError(ex);
            }
            return response;
        }
        public List<StaffLoansDisplay> GetAllStaffLoans()
        {
            return sRepo.GetAllStaffLoans();
        }
        public StaffLoans GetStaffLoanById(int id)
        {
            return sRepo.GetStaffLoanById(id);
        }
        public List<StaffLoans> GetLoansOfUser(int id)
        {
            return sRepo.GetAllLoansByUserId(id);
        }
    }
}
