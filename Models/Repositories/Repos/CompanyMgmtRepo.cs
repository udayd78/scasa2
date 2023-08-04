using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class CompanyMgmtRepo : ICompanyMgmtRepo
    {
        private readonly MyDbContext context;
        public CompanyMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }
        public ProcessResponse SaveCompany(CompanyMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.companyMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.CompanyId;
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
        public List<CompanyDisplayModel> GetAllCompanys()
        {
            List<CompanyDisplayModel> response = new List<CompanyDisplayModel>();
            try
            {
                response = (from c in context.companyMasters
                           where(c.IsDeleted == false)
                           select new CompanyDisplayModel {
                           CompanyId = c.CompanyId,
                           CompanyName = c.CompanyName,
                           CompanyAddress = c.CompanyAddress,
                           WarehouseAddress = c.WarehouseAddress,
                           ShowroomAddress = c.ShowroomAddress,
                           CEOName = c.CEOName,
                           CEOEmail = c.CEOEmail,
                           CEOContactNumber = c.CEOContactNumber,
                           WarehousePhoneNumbers = c.WarehousePhoneNumbers,
                           WarehouseEmail= c.WarehouseEmail,
                           ShowroomPhoneNumbers = c.ShowroomPhoneNumbers,
                           ShowroomEmail = c.ShowroomEmail,
                           GSTIN = c.GSTIN,
                           Code = c.Code
                           }).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }


        public CompanyMaster GetConpanyById(int id)
        {
            CompanyMaster response = new CompanyMaster();
            try
            {
                response = context.companyMasters.Where(a => a.IsDeleted == false && a.CompanyId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public CompanyMaster GetFirstCompany()
        {
            return context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
        }
        public ProcessResponse UpdateCompany(CompanyMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.CompanyId;
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
