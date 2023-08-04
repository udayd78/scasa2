using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class payRollMgmtService : IpayRollMgmtService
    {
        private readonly IPayRollMgmtRepo pRepo;
        public payRollMgmtService(IPayRollMgmtRepo _pRepo)
        {
            pRepo = _pRepo;
        }
        public ProcessResponse SavePayRoll(PayRollMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.PRMId > 0)
                {
                    //PayRollMaster ctgm = new PayRollMaster();
                    //ctgm = pRepo.GetPayRollById(request.PRMId);
                    //ctgm.BasicSalary = request.BasicSalary;
                    //ctgm.DearnessAllowance = request.DearnessAllowance;
                    //ctgm.Conveyance = request.Conveyance;
                    //ctgm.OtherAllowances = request.OtherAllowances;
                    //ctgm.HRA = request.HRA;
                    //ctgm.FoodAllowance = request.FoodAllowance;
                    //ctgm.MedicalAllowances = request.MedicalAllowances;
                    //ctgm.ProvidentFund = request.ProvidentFund;
                    //ctgm.ProfessionalTax = request.ProfessionalTax;
                    //ctgm.ESIFund = request.ESIFund;
                    //ctgm.TDS = request.TDS;
                    response = pRepo.UpdatePayRoll(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = pRepo.SavePayRoll(request);
                }


            }
            catch (Exception ex)
            {
                pRepo.LogError(ex);
            }

            return response;
        }
        public List<PayRollMaster> GetPayRoll()
        {
            return pRepo.GetAllPayRolls();
        }
        public PayRollMaster GetPayRollByUserId(int id)
        {
            return pRepo.GetPayRollByUserId(id);
        }
    }
}
