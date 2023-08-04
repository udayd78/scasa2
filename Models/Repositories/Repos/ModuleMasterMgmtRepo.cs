using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class ModuleMasterMgmtRepo : IModuleMasterMgmtRepo
    {

        private readonly MyDbContext context;

        public ModuleMasterMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveModule(ModulesMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.modulesMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.MoMId;
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

        public List<ModulesMaster> GetAllModules()
        {
            List<ModulesMaster> response = new List<ModulesMaster>();
            try
            {
                response = context.modulesMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ModulesMaster GetModuleTypeById(int id)
        {
            ModulesMaster response = new ModulesMaster();
            try
            {
                response = context.modulesMasters.Where(a => a.IsDeleted == false && a.MoMId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateModules(ModulesMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.MoMId;
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
