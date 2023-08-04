using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class CompanyMgmtService : ICompanyMgmtService
    {
        private readonly ICompanyMgmtRepo cRepo;
        public CompanyMgmtService (ICompanyMgmtRepo _cRepo)
        {
            cRepo = _cRepo;
        }
        public ProcessResponse SaveCompnyType(CompanyMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.CompanyId > 0)
                {
                    CompanyMaster ctgm = cRepo.GetConpanyById(request.CompanyId);
                    ctgm.CompanyName = request.CompanyName;
                    ctgm.CompanyAddress = request.CompanyAddress;
                    ctgm.WarehouseAddress = request.WarehouseAddress;
                    ctgm.ShowroomAddress = request.ShowroomAddress;
                    ctgm.CEOName = request.CEOName;
                    ctgm.CEOEmail = request.CEOEmail;
                    ctgm.CEOContactNumber = request.CEOContactNumber;
                    ctgm.WarehousePhoneNumbers = request.WarehousePhoneNumbers;
                    ctgm.WarehouseEmail = request.WarehouseEmail;
                    ctgm.ShowroomEmail = request.ShowroomEmail;
                    ctgm.ShowroomPhoneNumbers = request.ShowroomPhoneNumbers;
                    ctgm.GSTIN = request.GSTIN;
                    ctgm.Code = request.Code;
                    ctgm.CompanyDisplayName = request.CompanyDisplayName;
                    ctgm.CompanyWebsite = request.CompanyWebsite;
                    response = cRepo.UpdateCompany(ctgm);
                }
                else
                {
                    request.IsDeleted = false;
                    response = cRepo.SaveCompany(request);
                }


            }
            catch (Exception ex)
            {
                cRepo.LogError(ex);
            }

            return response;
        }
        public List<CompanyDisplayModel> GetAllCompnies()
        {
            return cRepo.GetAllCompanys();
        }
        public CompanyMaster GetCompanyById(int id)
        {
            return cRepo.GetConpanyById(id);
        }
        public CompanyMaster GetFirstCompany()
        {
            return cRepo.GetFirstCompany();
        }
    }
}
