using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class InventoryConditionMgmtRepo: IInventoryConditionMgmtRepo
    {
        private readonly MyDbContext context;

        public InventoryConditionMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }
        public ProcessResponse SaveConditionType(InventoryConditionMaster request)
        {
            ProcessResponse response = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.inventoryConditionMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.ConditionId;
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
        public List<InventoryConditionMaster> GetAllCondition()
        {
            List<InventoryConditionMaster> response = new List<InventoryConditionMaster>();
            try
            {
                response = context.inventoryConditionMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }


        public InventoryConditionMaster GetAllConditionTypeById(int id)
        {
            InventoryConditionMaster response = new InventoryConditionMaster();
            try
            {
                response = context.inventoryConditionMasters.Where(a => a.IsDeleted == false && a.ConditionId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateConditionType(InventoryConditionMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.ConditionId;
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






