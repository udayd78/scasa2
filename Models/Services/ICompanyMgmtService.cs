using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface ICompanyMgmtService
    {
        ProcessResponse SaveCompnyType(CompanyMaster request);
        List<CompanyDisplayModel> GetAllCompnies();
        CompanyMaster GetCompanyById(int id);
        CompanyMaster GetFirstCompany();
    }
}
