using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class GSTMasterMgmtRepo : IGSTMasterMgmtRepo
    {

        private readonly MyDbContext context;

        public GSTMasterMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveGST(GSTMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                request.IsEnabled = true;
                context.gSTMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.GSTMasterId;
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
        public List<GSTListModel> GetGstModelList()
        {
            List<GSTListModel> response = new List<GSTListModel>();
            try
            {
                response = (from g in context.gSTMasters
                            join c in context.categoryMasters on g.CategoryId equals c.CategoryId
                            join s in context.subCategoryMasters on g.SubCategoryId equals s.SubCategoryId
                            where g.IsDeleted == false && c.IsDeleted == false && s.IsDeleted == false
                            select new GSTListModel
                            {
                                GSTMasterId=g.GSTMasterId,
                                TaxName=g.TaxName,
                                TaxAmount=g.TaxAmount,
                                CategoryName=c.CategoryName,
                                SubCategoryName=s.SubCategoryName,
                                IsEnabled=g.IsEnabled
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List<GSTMaster> GetAllGSTs()
        {
             return context.gSTMasters.Where(a => a.IsDeleted == false).ToList();
        }
        public GSTMaster GetGSTById(int id)
        {
            GSTMaster response = new GSTMaster();
            try
            {
                response = context.gSTMasters.Where(a => a.IsDeleted == false && a.GSTMasterId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateGST(GSTMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                GSTMaster temp = context.gSTMasters.Where(a => a.IsDeleted == false && a.GSTMasterId == request.GSTMasterId).FirstOrDefault();
                context.Entry(temp).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.GSTMasterId;
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
        public int GetGStIdNoBySubId(int sId)
        {
            int a = 0;
            try
            {
                a = context.gSTMasters.Where(a => a.IsDeleted == false && a.IsEnabled == true && a.SubCategoryId == sId).Select(b => b.GSTMasterId).FirstOrDefault();
            }catch(Exception e)
            {
                LogError(e);
            }
            return a;
        }
        public GSTMaster getGstBySubCatId(int id)
        {
            GSTMaster g = new GSTMaster();
            try
            {
                g = context.gSTMasters.Where(a => a.IsDeleted == false && a.IsEnabled == true && a.SubCategoryId == id && a.TaxName=="GST").FirstOrDefault();
            }catch(Exception e)
            {

            }
            return g;
        }
        public decimal GetGstPercentBySubCatId(int id)
        {
            decimal re = 0;
            try
            {
                re = (decimal)context.gSTMasters.Where(a => a.IsDeleted == false && a.SubCategoryId == id).Select(b => b.TaxAmount).FirstOrDefault();
            }catch(Exception e)
            {
                LogError(e);
            }
            return re;
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
