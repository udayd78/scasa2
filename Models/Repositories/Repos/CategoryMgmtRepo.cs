using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class CategoryMgmtRepo : ICategoryMgmtRepo
    {
        private readonly MyDbContext context;

        public CategoryMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }

        #region Category Master

        public ProcessResponse SaveMainCategory(CategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.categoryMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.CategoryId;
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

        public List<CategoryMaster> GetAllCategoryTypes()
        {
            List<CategoryMaster> response = new List<CategoryMaster>();
            try
            {
                response = context.categoryMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }


        public CategoryMaster GetCategoryById(int id)
        {
            CategoryMaster response = new CategoryMaster();
            try
            {
                response = context.categoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateCategoryType(CategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.CategoryId;
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

        public ProcessResponse SaveSubCategory(SubCategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.subCategoryMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.SubCategoryId;
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

        public List<SubCategoryMaster> GetAllSubCategories(int catId)
        {
            List<SubCategoryMaster> response = new List<SubCategoryMaster>();
            try
            {
                response = context.subCategoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == catId).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }


        public SubCategoryMaster GetSubCategoryById(int id)
        {
            SubCategoryMaster response = new SubCategoryMaster();
            try
            {
                response = context.subCategoryMasters.Where(a => a.IsDeleted == false && a.SubCategoryId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateSubCategory(SubCategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.SubCategoryId;
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

        #endregion
    }
}

