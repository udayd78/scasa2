using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IPrevilegeMasterMgmtRepo
    {
        ProcessResponse SavePrevilage(PrevilegeMaster request);
        List<PrevilegeMaster> GetAllPrevilegeTypes();
        PrevilegeMaster GetPrevilegeById(int id);
        void LogError(Exception ex);
    }
}
