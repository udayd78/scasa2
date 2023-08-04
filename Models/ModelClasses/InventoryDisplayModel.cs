using Microsoft.AspNetCore.Http;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class InventoryDisplayModel
    {
        public int InventoryId { get; set; }
        public int? CategoryId { get; set; }
        public string CagoryId_Name { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryId_Name { get; set; }
        public string ModelNumber { get; set; }
        public string ItemDescription { get; set; }
        public string Brand { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Breadth { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public int? InventoryStatusId { get; set; }
        public string InventoryStatusId_Name { get; set; }
        public int? InventoryConditonId { get; set; }
        public string InventoryConditonId_Name { get; set; }
        
        public decimal? Velocity { get; set; }
        public DateTime? RecievedDate { get; set; }
        public decimal? MRPPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? SellablePrice { get; set; }
        public string RackName { get; set; }
        public int? Qty { get; set; }
        public string PrimaryImage { get; set; }

        public string HSNCode { get; set; }

        public int ShowroomQty { get; set; }
        public int WharehouseQty { get; set; }        
        public string DeleteReason { get; set; }
        public DateTime? DeletedOn { get; set; }
        public decimal? ThresholdValue { get; set; }
        public int? LeadTime { get; set; }
        public string Title { get; set; }
        public int? MRPFactorId { get; set; }
    }
    public class InventoryAllDisplayModel
    {
        public int InventoryId { get; set; }
        public int? CategoryId { get; set; }
        public string CagoryId_Name { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryId_Name { get; set; }
        public string ModelNumber { get; set; }
        public string ItemDescription { get; set; }
        public string Brand { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Breadth { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public int? InventoryStatusId { get; set; }
        public string InventoryStatusId_Name { get; set; }
        public int? InventoryConditonId { get; set; }
        public string InventoryConditonId_Name { get; set; }

        public decimal? Velocity { get; set; }
        public DateTime? RecievedDate { get; set; }
        public decimal? MRPPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? SellablePrice { get; set; }
        public string RackName { get; set; }
        public int? Qty { get; set; }
        public string PrimaryImage { get; set; }

        public string HSNCode { get; set; }

        public int ShowroomQty { get; set; }
        public int WharehouseQty { get; set; }
        public int ReservedQty { get; set; }
        public string DeleteReason { get; set; }
        public DateTime? DeletedOn { get; set; }
        public decimal? ThresholdValue { get; set; }
        public int? LeadTime { get; set; }
        public string Title { get; set; }
        public int? MRPFactorId { get; set; }
        public List<ReservedNameClass> Reserved { get; set; }
    }
    public class RecordsCountFromSql
    {
        public int cnt { get; set; }
    }
    public class ReservedNameClass
    {
        public int qty;
        public string name;
    }

    public class InventoryDisplayModelBase
    {
        public string search { get; set; }

        public List<InventoryAllDisplayModel> myList { get; set; }
        public List<CategoryDrop> categoryDrops { get; set; }
        public List<SubCategoryDrop> subCategoryDrops { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public List<InventoryDocuments> UploadedDocuments { get; set; }
    }
    public class SalesProductDetailModel
    {
        public int InventoryId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Title { get; set; }
        public string PrimaryImage { get; set; }
        public string MainImage1 { get; set; }
        public List<string> MainImagesList { get; set; }
        public List<InventoryImages> OtherImages { get; set; }
        public List<InventoryDocuments> DocsUploaded { get; set; }
        public string ModelNumber { get; set; }
        public string Brand { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Breadth { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public decimal? MRPPrice { get; set; }
        public int ShowroomQty { get; set; }
        public int WharehouseQty { get; set; }
        public int? ReservedQty { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public string ItemDescription { get; set; }
    }
}
