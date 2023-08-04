using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class IncentoryCatWiseModel
    {
        
        public int? CategoryId { get; set; }
        public string CagoryId_Name { get; set; }
       
        public int? ShowroomQty { get; set; }
        public decimal? ShowroomAmount { get; set; }
        public int? WharehouseQty { get; set; }
        public decimal? WareHouseAmount { get; set; }
        public int? INTransitQty { get; set; }
        public decimal? InTransitAmount { get; set; }
    }
    public class InventoryCategoryPrintModel
    {
        public int InventoryId { get; set; }
        public string Title { get; set; }
        public string ModelNumber { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string PrimaryImage { get; set; }
        public string ItemDescription { get; set; }
        public string Brand { get; set; }

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Breadth { get; set; }

        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public decimal? MRPPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public int? Qty { get; set; }
        public int? ShowroomQty { get; set; }
        public int? WharehouseQty { get; set; }
        public int? InventoryStatusId { get; set; }
    }
    public class PrintList
    {
        public int CategoryId { get; set; }
        public List<CategoryDrop> catDrops { get; set; }
        public int SubCategoryId { get; set; }
        public List<SubCategoryDrop> SubDrops { get; set; }
        public List<InventoryCategoryPrintModel> Items { get; set; }
    }
}
