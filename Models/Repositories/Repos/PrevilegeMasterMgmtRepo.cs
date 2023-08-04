using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class PrevilegeMasterMgmtRepo : IPrevilegeMasterMgmtRepo
    {
        private readonly MyDbContext context;
        public PrevilegeMasterMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }
        public ProcessResponse SavePrevilage(PrevilegeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.PrMId > 0)
                {
                    context.Entry(request).CurrentValues.SetValues(request);
                    context.SaveChanges();
                    response.currentId = request.PrMId;
                    response.statusCode = 1;
                    response.statusMessage = "Success";
                }
                else
                {
                    request.IsDeleted = false;
                    context.previlegeMasters.Add(request);
                    context.SaveChanges();
                    response.currentId = request.PrMId;
                    response.statusCode = 1;
                    response.statusMessage = "success";

                }
            }catch(Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }
            return response;
        }
        public List<PrevilegeMaster> GetAllPrevilegeTypes()
        {
            List<PrevilegeMaster> response = new List<PrevilegeMaster>();
            try
            {
                response = context.previlegeMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }


        public PrevilegeMaster GetPrevilegeById(int id)
        {
            PrevilegeMaster response = new PrevilegeMaster();
            try
            {
                response = context.previlegeMasters.Where(a => a.IsDeleted == false && a.PrMId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
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
