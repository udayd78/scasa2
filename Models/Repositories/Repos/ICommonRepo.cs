using SCASA.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface ICommonRepo
    {
        List<GSTDrop> GetAllGST();
        List<CategoryDrop> GetCatsDrop();
        List<SubCategoryDrop> GetSubCatsDrop(int id);

        List<ConditionDrop> GetConditionsDrop();
        List<LocationDrop> GetLoctionDrop();
        List<InventoryStatusDrop> GetInventoryStatusDrop();
        List<MRPFactopDrop> getMRPFactorDrops();
        List<CountryDrop> GetAllCountries();
        List<StateDrop> GetAllStates(int countryId);
        List<CityDrop> GetAllCities(int stateId);
        string GetCityNameOfId(int id);
        string GetStateNameOfId(int id);
        string GetCountryNameOfId(int id);
        List<UserTypeDrop> GetUserTypes();
        List<StaffDrop> GetStaffDropsForReportingHead(string type);
        List<ShiftDrop> GetAllShift();
        List<FHeadDrops> GetFinanceHeadDrops();
        List<FHeadDrops> GetBankAndCashHeads();
        List<FHeadDrops> GetNoBankAndCashHeads();
        List<CustomerDrop> GetCustomerDrops();
    }
}
