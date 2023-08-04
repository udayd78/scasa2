using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class InventoryStatusMgmtRepo : IInventoryStatusMgmtRepo
    {

        private readonly MyDbContext context;
        public InventoryStatusMgmtRepo(MyDbContext _context)
        {
            context = _context;

        }

        public ProcessResponse SaveStatus(InventoryStatusMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.inventoryStatusMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.StatusId;
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }

            return response;

        }

        public List<InventoryStatusMaster> GetAllStatusTypes()
        {
            List<InventoryStatusMaster> response = new List<InventoryStatusMaster>();
            try
            {
                response = context.inventoryStatusMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }


        public InventoryStatusMaster GetStatusById(int id)
        {
            InventoryStatusMaster response = new InventoryStatusMaster();
            try
            {
                response = context.inventoryStatusMasters.Where(a => a.IsDeleted == false && a.StatusId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateStatusType(InventoryStatusMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.StatusId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
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
