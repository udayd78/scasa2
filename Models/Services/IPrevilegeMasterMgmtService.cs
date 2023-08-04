using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IPrevilegeMasterMgmtService
    {
        ProcessResponse SavePrevilege(PrevilegeMaster request);
        List<PrevilegeMaster> GetAllPrevileges();
        PrevilegeMaster GetPrevilegeById(int id);
    }
}
