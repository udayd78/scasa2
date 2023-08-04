using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface ICategoryMgmtRepo
    {

        ProcessResponse SaveMainCategory(CategoryMaster request);

        List<CategoryMaster> GetAllCategoryTypes();

        CategoryMaster GetCategoryById(int id);

        ProcessResponse UpdateCategoryType(CategoryMaster request);

        void LogError(Exception ex);

          ProcessResponse SaveSubCategory(SubCategoryMaster request);

          List<SubCategoryMaster> GetAllSubCategories(int catId);


          SubCategoryMaster GetSubCategoryById(int id);

         ProcessResponse UpdateSubCategory(SubCategoryMaster request);

    }
}
