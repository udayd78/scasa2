using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class MWorkingDaysMgmtRepo : IMWorkingDaysMgmtRepo
    {

        private readonly MyDbContext context;

        public MWorkingDaysMgmtRepo(MyDbContext _context)
        {
            this.context = _context;


        }
           public ProcessResponse SaveWorkingDays(MonthlyWorkingDays request)
           {
             ProcessResponse response = new ProcessResponse();
              try
              {
                request.IsDeleted = false;
                context.mworkingDaysMasters.Add(request);
                context.SaveChanges();
                response.statusCode =1;
                response.statusMessage = "Success";                 
              }
              catch(Exception ex)
              {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
              }
             return response;

           }

        public List<MWorkingDaysDisplay> GetAllWorkingDays()
        {
            List<MWorkingDaysDisplay> response = new List<MWorkingDaysDisplay>();
            try
            {
                response = (from m in context.mworkingDaysMasters
                            where m.IsDeleted == false
                            select new MWorkingDaysDisplay
                            {
                                TRId = m.TRId,
                                MonthNumber = m.MonthNumber,
                                YearNumber = m.YearNumber,
                                NumberOfDays = m.NumberOfDays,
                                MonthName = null
                            }).ToList();
            }
            catch(Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public MWorkingDaysDisplay GetDaysByMonYer(int MonthNumber,int YearNumber)
        {
            MWorkingDaysDisplay response = new MWorkingDaysDisplay();
            try
            {
                response = (from m in context.mworkingDaysMasters
                            where m.IsDeleted == false && MonthNumber==m.MonthNumber && YearNumber==m.YearNumber
                            select new MWorkingDaysDisplay
                            {
                                TRId = m.TRId,
                                MonthNumber = m.MonthNumber,
                                YearNumber = m.YearNumber,
                                NumberOfDays = m.NumberOfDays,
                                MonthName = null
                            }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                LogError(ex);
            }
            return response;

        }

        public MonthlyWorkingDays GetDaysById(int id)
        {
            MonthlyWorkingDays response = new MonthlyWorkingDays();
            response = context.mworkingDaysMasters.Where(m => m.IsDeleted == false && m.TRId == id).FirstOrDefault();

            return response;
        }

        public ProcessResponse UpdateDays(MonthlyWorkingDays request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                request.IsDeleted = false;
                response.currentId = request.TRId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch(Exception ex)
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
