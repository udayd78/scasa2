using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class CategoryMgmtService : ICategoryMgmtService
    {

        private readonly ICategoryMgmtRepo cRepo;

        public CategoryMgmtService(ICategoryMgmtRepo _cRepo)
        {
            cRepo = _cRepo;
        }

        public ProcessResponse SaveCategoryType(CategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.CategoryId > 0)
                {
                    CategoryMaster ctgm = new CategoryMaster();
                    ctgm = cRepo.GetCategoryById(request.CategoryId);
                    ctgm.CategoryName = request.CategoryName;
                    ctgm.CategoryImage = request.CategoryImage;
                    response = cRepo.UpdateCategoryType(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = cRepo.SaveMainCategory(request);
                }


            }
            catch (Exception ex)
            {
                cRepo.LogError(ex);
            }

            return response;
        }
        public List<CategoryMaster> GetAllCategories()
        {
            return cRepo.GetAllCategoryTypes();
        }
        public CategoryMaster GetCategoryById(int id)
        {
            return cRepo.GetCategoryById(id);
        }

        public ProcessResponse UpdateCategoryType(CategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = cRepo.UpdateCategoryType(request);
            }
            catch (Exception ex)
            {
                cRepo.LogError(ex);
            }
            return response;
        }


        public ProcessResponse SaveSubCategory(SubCategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.SubCategoryId > 0)
                {
                    SubCategoryMaster ctgm = new SubCategoryMaster();
                    ctgm = cRepo.GetSubCategoryById(request.SubCategoryId);
                    ctgm.SubCategoryName = request.SubCategoryName;
                    ctgm.SubCategoryImage = request.SubCategoryImage;
                    ctgm.CategoryId = request.CategoryId;
                    ctgm.IsDeleted = request.IsDeleted;
                    response = cRepo.UpdateSubCategory(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = cRepo.SaveSubCategory(request);
                }


            }
            catch (Exception ex)
            {
                cRepo.LogError(ex);
            }

            return response;
        }
        public List<SubCategoryMaster> GetAllSubCategories(int catId)
        {
            return cRepo.GetAllSubCategories(catId);
        }
        public SubCategoryMaster GetSubCategoryById(int id)
        {
            return cRepo.GetSubCategoryById(id);
        }
    }
}
