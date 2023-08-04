using SCASA.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class StockMovementDisplayModel
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
        public int TRId { get; set; }
        public string MovedBy { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
        public string Notes { get; set; }
        public DateTime? MovedOn { get; set; }
    }

    public class StockMovementDisplayModelBase
    {
        public string search { get; set; }

        public List<StockMovementDisplayModel> myList { get; set; }
        public List<CategoryDrop> categoryDrops { get; set; }
        public List<SubCategoryDrop> subCategoryDrops { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
