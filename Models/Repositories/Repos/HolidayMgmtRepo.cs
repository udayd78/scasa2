using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class HolidayMgmtRepo : IHolidayMgmtRepo
    {

        private readonly MyDbContext context;

        public HolidayMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveHoliday(HolidayMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.holidayMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.HMId;
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
        public List<HolidayMaster> GetAllHolidays()
        {
            List<HolidayMaster> response = new List<HolidayMaster>();
            try
            {
                response = context.holidayMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public HolidayMaster GetHolidayById(int id)
        {
            HolidayMaster response = new HolidayMaster();
            try
            {
                response = context.holidayMasters.Where(a => a.IsDeleted == false && a.HMId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateHolidays(HolidayMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.HMId;
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
