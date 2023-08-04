using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using SCASA.Models.ModelClasses;
using System.Threading.Tasks;
using SCASA.Models.Repositories.Entity;

namespace SCASA.Models.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepo cRepo;
        private readonly IUserMgmtRepo uRepo;
        public CommonService(ICommonRepo _cRepo , IUserMgmtRepo _uRepo)
        {
            cRepo = _cRepo;
            uRepo = _uRepo;
        }

        public List<CategoryDrop> GetCatsDrop()
        {
            return cRepo.GetCatsDrop();
        }
        public List<CatsWithSubs> GetCategorysWithSubCat()
        {
            List<CatsWithSubs> result = new List<CatsWithSubs>();
            List<CategoryDrop> cats= cRepo.GetCatsDrop();
            foreach(CategoryDrop c in cats)
            {
                CatsWithSubs a = new CatsWithSubs();
                a.CategoryId = c.CategoryId;
                a.CategoryImage = c.CategoryImage;
                a.CategoryName = c.CategoryName;
                //a.Subs = cRepo.GetSubCatsDrop(c.CategoryId);
                result.Add(a);
            }
            return result;
        }

        public List<SubCategoryDrop> GetSubCatsDrop(int id)
        {
            return cRepo.GetSubCatsDrop(id);
        }

        public List<ConditionDrop> GetConditionsDrop()
        {
            return cRepo.GetConditionsDrop();
        }
        public List<LocationDrop> GetLoctionDrop()
        {
            return cRepo.GetLoctionDrop();
        }
        public List<InventoryStatusDrop> GetInventoryStatusDrop()
        {
            return cRepo.GetInventoryStatusDrop();
        }
        public List<MRPFactopDrop> GetMrpFactorDrop()
        {
            return cRepo.getMRPFactorDrops();
        }
        public List<UserTypeDrop> GetUserTypeDrops()
        {
            return cRepo.GetUserTypes();
        }

        public List<CountryDrop> GetAllCountries()
        {
            return cRepo.GetAllCountries();
        }
        public List<StateDrop> GetAllStates(int countryId)
        {
            return cRepo.GetAllStates(countryId);
        }

        public List<CityDrop> GetAllCities(int stateId)
        {
            return cRepo.GetAllCities(stateId);
        }
        public string GetCountryNameOfId(int id)
        {
            return cRepo.GetCountryNameOfId(id);
        }
        public string GetCityNameOfId(int id)
        {
            return cRepo.GetCityNameOfId(id);
        }
        public string GetStateNameOfId(int id)
        {
            return cRepo.GetStateNameOfId(id);
        }
        public List<GSTDrop> GetAllGST()
        {
            return cRepo.GetAllGST();
        }
        public List<StaffDrop> GetStaffDropsForReportingHead(int userTypeId)
        {
            UserTypeMaster utm = uRepo.GetUserTypeById(userTypeId);
            List<StaffDrop> sds = new List<StaffDrop>();
            if(utm.TypeName=="Sales Executive"|| utm.TypeName =="Wharehouse Incharge")
            {
                sds = cRepo.GetStaffDropsForReportingHead("Sales Head");
            }
            else
            {
                sds = cRepo.GetStaffDropsForReportingHead("CEO");
            }
            return sds;
        }
        public List<ShiftDrop> GetShiftDrops()
        {
            return cRepo.GetAllShift();
        }
        public List<FHeadDrops> GetFHeadDrops()
        {
            return cRepo.GetFinanceHeadDrops();
        }
        public List<FHeadDrops> GetBankAndCashHeads()
        {
            return cRepo.GetBankAndCashHeads();
        }
        public List<FHeadDrops> GetNoBankAndCashHeads()
        {
            return cRepo.GetNoBankAndCashHeads();
        }
        public List<CustomerDrop> GetCustomerDrops()
        {
            return cRepo.GetCustomerDrops();
        }
    }
}
