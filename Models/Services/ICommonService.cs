using SCASA.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface ICommonService
    {
        List<GSTDrop> GetAllGST();
        List<CategoryDrop> GetCatsDrop();
        List<CatsWithSubs> GetCategorysWithSubCat();
        List<SubCategoryDrop> GetSubCatsDrop(int id);
        List<ConditionDrop> GetConditionsDrop();
        List<LocationDrop> GetLoctionDrop();
        List<InventoryStatusDrop> GetInventoryStatusDrop();
        List<MRPFactopDrop> GetMrpFactorDrop();
        List<CountryDrop> GetAllCountries();
        List<StateDrop> GetAllStates(int countryId);
        List<CityDrop> GetAllCities(int stateId);
        string GetCityNameOfId(int id);
        string GetStateNameOfId(int id);
        string GetCountryNameOfId(int id);
        List<UserTypeDrop> GetUserTypeDrops();
        List<StaffDrop> GetStaffDropsForReportingHead(int userTypeId);
        List<ShiftDrop> GetShiftDrops();
        List<FHeadDrops> GetFHeadDrops();
        List<FHeadDrops> GetBankAndCashHeads();
        List<FHeadDrops> GetNoBankAndCashHeads();
        List<CustomerDrop> GetCustomerDrops();
    }
}
