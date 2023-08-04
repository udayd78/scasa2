using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System.Collections.Generic;

namespace SCASA.Models.Services
{
    public interface ICategoryMgmtService
    {
        ProcessResponse SaveCategoryType(CategoryMaster request);

        List<CategoryMaster> GetAllCategories();

        CategoryMaster GetCategoryById(int id);

        ProcessResponse UpdateCategoryType(CategoryMaster request);
        ProcessResponse SaveSubCategory(SubCategoryMaster request);
        List<SubCategoryMaster> GetAllSubCategories(int catId);
        SubCategoryMaster GetSubCategoryById(int id);

    }
}