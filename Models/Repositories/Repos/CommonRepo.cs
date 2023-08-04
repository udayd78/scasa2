using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories
{
    public class CommonRepo : ICommonRepo
    {
        private readonly MyDbContext context;
        public CommonRepo(MyDbContext _context)
        {
            context = _context;
        }
        public List<CategoryDrop> GetCatsDrop()
        {
            return context.categoryMasters.Where(a => a.IsDeleted == false)
                .Select(b => new CategoryDrop
                {
                    CategoryId = b.CategoryId,
                    CategoryName = b.CategoryName,
                    CategoryImage = b.CategoryImage
                })
                .OrderBy(b => b.CategoryName).ToList();
        }
        public List<SubCategoryDrop> GetSubCatsDrop(int id)
        {
            return context.subCategoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == id)
                .Select(b => new SubCategoryDrop
                {
                    SubCategoryId = b.SubCategoryId,
                    SubCategoryName = b.SubCategoryName
                })
                .OrderBy(b => b.SubCategoryName).ToList();
        }

        public List<ConditionDrop> GetConditionsDrop()
        {
            return context.inventoryConditionMasters.Where(a => a.IsDeleted == false)
                .Select(b => new ConditionDrop
                {
                    ConditionId = b.ConditionId,
                    ConditionName = b.ConditionName
                }).ToList();
        }
        public List<LocationDrop> GetLoctionDrop()
        {
            return context.inventoryLocationMasters.Where(a => a.IsDeleted == false)
                .Select(b => new LocationDrop
                {
                    LocationId = b.locationId,
                     LocationName=b.LocationName
                })
                .OrderBy(b => b.LocationName).ToList();
        }
        public List<InventoryStatusDrop> GetInventoryStatusDrop()
        {
            return context.inventoryStatusMasters.Where(a => a.IsDeleted == false)
                .Select(b => new InventoryStatusDrop
                {
                     StatusId= b.StatusId,
                      StatusName = b.StatusName
                })
                .OrderBy(b => b.StatusName).ToList();
        }
        public List<MRPFactopDrop> getMRPFactorDrops()
        {
            return context.mRPFactors.Where(a => a.IsDeleted == false)
                .Select(b => new MRPFactopDrop
                {
                    TRId=b.TRId,
                    FactorName=b.FactorName,
                    FactorValue=b.FactorValue
                }).OrderBy(b => b.FactorName).ToList();
        }
        public List<UserTypeDrop> GetUserTypes()
        {
            return context.userTypeMasters.Where(a => a.IsDeleted == false)
                .Select(b => new UserTypeDrop
                {
                    UserTypeId = b.TypeId,
                    UserTypeName = b.TypeName
                }).OrderBy(b => b.UserTypeName).ToList();
        }

        public List<CountryDrop> GetAllCountries()
        {
            return context.countryMasters.Where(a => a.IsDeleted == false)
                .Select(b => new CountryDrop
                {
                    CountryId = b.Id,
                    CountryName = b.CountryName
                }).OrderBy(b => b.CountryName).ToList();
        }
        public List<StateDrop> GetAllStates(int countryId)
        {
            return context.stateMasters.Where(a => a.IsDeleted == false && a.CountryId == countryId)
                .Select(b => new StateDrop
                {
                     StateId = b.Id,
                     StateName = b.StateName,

                }).OrderBy(b => b.StateName).ToList();
        }

        public List<CityDrop> GetAllCities(int stateId)
        {
            return context.cityMasters.Where(a => a.IsDeleted == false && a.StateId == stateId)
                .Select(b => new CityDrop
                {
                    CityId = b.Id,
                    CityName = b.CityName

                }).OrderBy(b => b.CityName).ToList();
        }
        public string GetCityNameOfId(int id)
        {
            string re = "";
            try
            {
                re = context.cityMasters.Where(a => a.IsDeleted == false && a.Id == id).Select(b => b.CityName).FirstOrDefault();
            }catch(Exception e)
            {

            }
            return re;
        }
        public string GetStateNameOfId(int id)
        {
            string re = "";
            try
            {
                re = context.stateMasters.Where(a => a.IsDeleted == false && a.Id == id).Select(b => b.StateName).FirstOrDefault();
            }
            catch (Exception e)
            {

            }
            return re;
        }
        public string GetCountryNameOfId(int id)
        {
            string re = "";
            try
            {
                re = context.countryMasters.Where(a => a.IsDeleted == false && a.Id == id).Select(b => b.CountryName).FirstOrDefault();
            }
            catch (Exception e)
            {

            }
            return re;
        }
        public List<StaffDrop> GetStaffDropsForReportingHead(string type)
        {
            UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == type).FirstOrDefault();

            return (from u in context.userMasters
                   where(u.IsDeleted ==false && u.UserTypeId == utm.TypeId && u.IsActive == true && u.CurrentStatus== "Active")
                select new StaffDrop
                {
                    StaffId = u.UserId,
                    StaffName = u.UserName+" ( "+u.EmployeeCode+" )",
                    EmployeeCode = u.EmployeeCode
                }).OrderBy(b => b.StaffName).ToList();
        }
        public List<GSTDrop> GetAllGST()
        {
            return context.gSTMasters.Where(a => a.IsDeleted == false && a.IsEnabled == true)
                .Select(b => new GSTDrop
                {
                     GSTMasterId = b.GSTMasterId,
                      TaxName=b.TaxName
                }).OrderBy(b => b.TaxName).ToList();
        }
        public List<ShiftDrop> GetAllShift()
        {
            return context.shiftMasters.Where(a => a.IsDeleted == false)
                .Select(b => new ShiftDrop
                {
                    ShifId = b.SMId,
                    ShiftName = b.ShiftName,
                }).OrderBy(b => b.ShiftName).ToList();
        }
        public List<FHeadDrops> GetFinanceHeadDrops()
        {
            List<FHeadDrops> result = new List<FHeadDrops>();
            try
            {
                result = context.financeHeads.Where(a => a.IsDeleted == false)
                    .Select(b => new FHeadDrops
                    {
                        HeadId = b.HeadId,
                        HeadName = b.HeadName,
                        HeadCode = b.HeadCode,
                        CurrentBallance = (int)b.CurrentBallance,
                    }).ToList();
            }catch(Exception e) { }
            return result;
        }
        public List<FHeadDrops> GetBankAndCashHeads()
        {
            FinanceGroups fgb = context.financeGroups.Where(a => a.IsDeleted == false && (a.GroupName == "Bank Accounts")).FirstOrDefault();
            FinanceGroups fgc = context.financeGroups.Where(a => a.IsDeleted == false && (a.GroupName == "Cash-in-hand")).FirstOrDefault();
            List<FHeadDrops> result = new List<FHeadDrops>();
            try
            {
                result = context.financeHeads.Where(a => a.IsDeleted == false && (a.GroupId == fgb.GroupId || a.GroupId == fgc.GroupId))
                    .Select(b => new FHeadDrops
                    {
                        HeadId = b.HeadId,
                        HeadName = b.HeadName,
                        HeadCode = b.HeadCode,
                        CurrentBallance = (int)b.CurrentBallance,
                    }).ToList();
            }catch(Exception e) { }
            return result;
        }
        public List<FHeadDrops> GetNoBankAndCashHeads()
        {
            FinanceGroups fgb = context.financeGroups.Where(a => a.IsDeleted == false && (a.GroupName == "Bank Accounts")).FirstOrDefault();
            FinanceGroups fgc = context.financeGroups.Where(a => a.IsDeleted == false && (a.GroupName == "Cash-in-hand")).FirstOrDefault();
            List<FHeadDrops> result = new List<FHeadDrops>();
            try
            {
                result = context.financeHeads.Where(a => a.IsDeleted == false && (a.GroupId != fgb.GroupId && a.GroupId != fgc.GroupId))
                    .Select(b => new FHeadDrops
                    {
                        HeadId = b.HeadId,
                        HeadName = b.HeadName,
                        HeadCode = b.HeadCode,
                        CurrentBallance = (int)b.CurrentBallance,
                    }).ToList();
            }
            catch (Exception e) { }
            return result;
        }
        public List<CustomerDrop> GetCustomerDrops()
        {
            List<CustomerDrop> result = new List<CustomerDrop>();

            try
            {
                result = (from c in context.customerMasters
                          where c.IsDeleted == false
                          select new CustomerDrop
                          {
                              cusId=c.Cid,
                              Name=c.FullName
                          }).ToList();
            }catch(Exception e)
            {

            }

            return result;
        }
    }
}
