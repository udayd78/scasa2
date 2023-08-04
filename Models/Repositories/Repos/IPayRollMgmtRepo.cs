using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IPayRollMgmtRepo
    {
        ProcessResponse SavePayRoll(PayRollMaster request);
        public List<PayRollMaster> GetAllPayRolls();
        PayRollMaster GetPayRollById(int id);
        public PayRollMaster GetPayRollByUserId(int id);
        public ProcessResponse UpdatePayRoll(PayRollMaster request);
        void LogError(Exception ex);
    }
}
