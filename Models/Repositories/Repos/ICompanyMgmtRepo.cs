using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface ICompanyMgmtRepo
    {
        ProcessResponse SaveCompany(CompanyMaster request);
        List<CompanyDisplayModel> GetAllCompanys();
        CompanyMaster GetConpanyById(int id);
        CompanyMaster GetFirstCompany();
        ProcessResponse UpdateCompany(CompanyMaster request);
        void LogError(Exception ex);

    }
}
