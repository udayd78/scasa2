using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class GSTMasterMgmtService : IGSTMasterMgmtService
    {

        private readonly IGSTMasterMgmtRepo gRepo;


        public GSTMasterMgmtService(IGSTMasterMgmtRepo _gRepo)
        {
            gRepo = _gRepo;

        }

        public ProcessResponse SaveGST(GSTMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.GSTMasterId > 0)
                {
                    response = gRepo.UpdateGST(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = gRepo.SaveGST(request);
                }
            }
            catch (Exception ex)
            {
                gRepo.LogError(ex);
            }
            return response;
        }
        public List<GSTMaster> GetAllGST()
        {
            return gRepo.GetAllGSTs();
        }
        public List<GSTListModel> GetGstModelList()
        {
            return gRepo.GetGstModelList();
        }
        public GSTMaster GetGSTById(int id)
        {
            return gRepo.GetGSTById(id);
        }
        public int GetGStIdNoBySubId(int sId)
        {
            return gRepo.GetGStIdNoBySubId(sId);
        }
        public GSTMaster getGstBySubCatId(int id)
        {
            return gRepo.getGstBySubCatId(id);
        }
        public decimal GetGstPercentBySubCatId(int id)
        {
            return gRepo.GetGstPercentBySubCatId(id);
        }
    }
}
