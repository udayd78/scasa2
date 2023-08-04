
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class InventoryLocationMgmtRepo : IInventoryLocationMgmtRepo
    {
        private readonly MyDbContext context;

        public InventoryLocationMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveLocationType(InventoryLocationMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.inventoryLocationMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.locationId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }

        public List<InventoryLocationMaster> GetAllLocationType()
        {
            List<InventoryLocationMaster> response = new List<InventoryLocationMaster>();
            try
            {
                response = context.inventoryLocationMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public InventoryLocationMaster GetAllLocationTypeById(int id)
        {
            InventoryLocationMaster response = new InventoryLocationMaster();
            try
            {
                response = context.inventoryLocationMasters.Where(a => a.IsDeleted == false && a.locationId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateLocationType(InventoryLocationMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.locationId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }


        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }

    }
}
 


        
        


        
           



           


    

