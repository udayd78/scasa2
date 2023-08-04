using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class CategoryDrop
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; internal set; }
        //public List<ProductsDisplaySales> products { get; set; }
    }
    public class CatsWithSubs
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; internal set; }
        //public List<SubCategoryDrop> Subs { get; set; }
    }
    public class SubCategoryDrop
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }
    public class UserTypeDrop
    {
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
    }
    public class CityDrop
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
    public class CountryDrop
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class StateDrop
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }

    public class ConditionDrop
    {
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
    }
    public class LocationDrop
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class InventoryStatusDrop
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
    public class MRPFactopDrop
    {
        public int TRId { get; set; }
        public string FactorName { get; set; }
        public decimal? FactorValue { get; set; }
    }

    public class StaffDrop
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string EmployeeCode { get; set; }
    }
    public class GSTDrop
    { 
        public int GSTMasterId { get; set; }
        public string TaxName { get; set; }
    }
    public class InventoryModelDrop
    {
        public string ModelNumber { get; set; }
        public int InventoryId { get; set; }
    }

    public class StockMovmentBasicInfo
    {
        public string ProductImage { get; set; }
        public int? WarehouseQty { get; set; }
        public int? ShowroomQty { get; set; }
    }
    public class ShiftDrop
    {
        public int ShifId { get; set; }
        public string ShiftName { get; set; }
    }

    public class ProductDisplaySalesModel
    {
        public List<CategoryDrop> cats { get; set; }
        public List<TargetMaster> targets { get; set; }
        public List<LeadInfo> leadInfo { get; set; }
        public List<AttendanceInfo> attendanceInfo { get; set; }
        public List<MPayRollDisplayModel> payrollInfo { get; set; }
        public List<LoansInformation> loanInfo { get; set; }
    }
   
    public class ProductsDisplaySales
    {
        public int InventroyId { get; set; }
        public string ModelNumber { get; set; }
        public string ItemDescription { get; set; }
        public  string  colorImages { get; set; }
        public string MainImages { get; set; }
        public decimal? MRP { get; set; }
        public string ColorName { get; set; }
        public string Title { get; set; }
        public int? AvailableQtyS { get;   set; }
        public int? AvaliableQtyW { get;   set; }
        public int? AvaliableQtyR { get;   set; }
    }
    public class FHeadDrops
    {
        public int HeadId { get; set; }
        public string HeadName { get; set; }
        public string HeadCode { get; set; }
        public decimal CurrentBallance { get; set; }

    }
    public class CustomerDrop
    {
        public int cusId { get; set; }
        public string Name { get; set; }
    }
    
}
