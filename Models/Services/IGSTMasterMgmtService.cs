using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IGSTMasterMgmtService
    {

        ProcessResponse SaveGST(GSTMaster request);
        List<GSTListModel> GetGstModelList();
        List<GSTMaster> GetAllGST();
        GSTMaster GetGSTById(int id);
        int GetGStIdNoBySubId(int sId);
        GSTMaster getGstBySubCatId(int id);
        decimal GetGstPercentBySubCatId(int id);
    }
}
