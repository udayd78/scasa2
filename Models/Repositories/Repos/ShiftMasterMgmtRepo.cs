using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class ShiftMasterMgmtRepo : IShiftMasterMgmtRepo
    {

        private readonly MyDbContext context;

        public ShiftMasterMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveShift(ShiftMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.shiftMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.SMId;
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
        public List<ShiftMaster> GetAllShifts()
        {
            List<ShiftMaster> response = new List<ShiftMaster>();
            try
            {
                response = context.shiftMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ShiftMaster GetShiftById(int id)
        {
            ShiftMaster response = new ShiftMaster();
            try
            {
                response = context.shiftMasters.Where(a => a.IsDeleted == false && a.SMId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateShifts(ShiftMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.SMId;
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
