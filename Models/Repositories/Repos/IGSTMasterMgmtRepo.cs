using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IGSTMasterMgmtRepo
    {
        ProcessResponse SaveGST(GSTMaster request);
        List<GSTListModel> GetGstModelList();
        List<GSTMaster> GetAllGSTs();
        GSTMaster GetGSTById(int id);
        ProcessResponse UpdateGST(GSTMaster request);
        int GetGStIdNoBySubId(int sId);
        decimal GetGstPercentBySubCatId(int id);
        GSTMaster getGstBySubCatId(int id);
        void LogError(Exception ex);
    }
}
