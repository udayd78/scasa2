using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class InventoryStatusMgmtService : IInventoryStatusMgmtService
    {

        private readonly IInventoryStatusMgmtRepo sRepo;
        public InventoryStatusMgmtService (IInventoryStatusMgmtRepo _sRepo)
        {
            sRepo = _sRepo;
        }

        public ProcessResponse SaveStatusType(InventoryStatusMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.StatusId > 0)
                {
                    InventoryStatusMaster ctgm = new InventoryStatusMaster();
                    ctgm = sRepo.GetStatusById(request.StatusId);
                    ctgm.StatusName = request.StatusName;
                    response = sRepo.UpdateStatusType(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response =sRepo.SaveStatus(request);
                }


            }
            catch (Exception ex)
            {
                sRepo.LogError(ex);
            }

            return response;
        }
        public List<InventoryStatusMaster> GetAllStatus()
        {
            return sRepo.GetAllStatusTypes();
        }
        public InventoryStatusMaster GetStatusById(int id)
        {
            return sRepo.GetStatusById(id);
        }

        public ProcessResponse UpdateStatusType(InventoryStatusMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = sRepo.UpdateStatusType(request);
            }
            catch (Exception ex)
            {
                sRepo.LogError(ex);
            }
            return response;
        }
    }
}
