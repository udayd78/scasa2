
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class InventoryLocationMgmtService : IInventoryLocationMgmtService
    {
        private readonly IInventoryLocationMgmtRepo lRepo;


        public InventoryLocationMgmtService(IInventoryLocationMgmtRepo locationMgmtRepo)
        {
            lRepo = locationMgmtRepo;


        }
          
        public ProcessResponse SaveLocationType(InventoryLocationMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {


                if (request.locationId > 0)
                {
                    InventoryLocationMaster um = new InventoryLocationMaster();
                    um = lRepo.GetAllLocationTypeById(request.locationId);
                    um.LocationName = request.LocationName;

                    response = lRepo.UpdateLocationType(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = lRepo.SaveLocationType(request);
                }
            }
            catch(Exception ex)
            {
                lRepo.LogError(ex);
            }
            return response;
        }

        public List<InventoryLocationMaster> GetAllLocationType()
        {
            return lRepo.GetAllLocationType();
        }
        public InventoryLocationMaster GetAllLocationTypeById(int id)
        {
            return lRepo.GetAllLocationTypeById(id);
        }
        public ProcessResponse UpdateLocationType(InventoryLocationMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response=lRepo.UpdateLocationType(request);
            }
            catch(Exception ex)
            {
                lRepo.LogError(ex);
            }
            return response;
        }
            
    }
}
