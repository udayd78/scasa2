using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IpayRollMgmtService
    {
        ProcessResponse SavePayRoll(PayRollMaster request);
        List<PayRollMaster> GetPayRoll();
        PayRollMaster GetPayRollByUserId(int id);
    }
}
