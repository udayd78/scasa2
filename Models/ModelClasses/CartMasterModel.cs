using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class CartMasterModel
    {
        public int CartId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedByName { get; set; }
        public int? CreatedById { get; set; }
        public int? CustomerId { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public List<CartDetailsModel> cartDetails { get; set; }
    }

    public class CartDetailsModel
    {
        public int CartDetailId { get; set; }
        public int? CartId { get; set; }
        public int? InventoryId { get; set; }
        public string InventoryImage { get; set; }
        public string InventoryTitle { get; set; }
        public int? Qty { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Breadth { get; set; }
        public decimal? OrderLineTotal { get; set; }
    }
    public class CRFQMasterModel
    {
        public int CRFQId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string ClosedBy { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? QuoteReffId { get; set; }
        public int StaffId { get; set; }
        public string SalesExecutiveName { get; set; }
        public List<CRFQDetailsModel> crfqDetails { get; set; }
        public string CustomerName { get;   set; }
        public string CurrentStatus { get; set; }
        
    }

    public class CRFQDetailsModel
    {
        public int CRFQDetailsId { get; set; }
        public int? CRFQId { get; set; }
        public int? InventoryId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemPrise { get; set; }
        public decimal? DisAmtBySE { get; set; }
        public decimal? DisAmtByHead { get; set; }
         
        public string InventoryImage { get; set; }
        public string InventoryTitle { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? TotalPrice { get; set; }
        public string CurrentStatus { get; set; } 
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        public decimal? Height { get; set; }
     
        public decimal? Width { get; set; }
     
        public decimal? Breadth { get; set; }
        public decimal? OrderLineTotal { get; set; }
        public decimal? AdminDiscount { get; set; }
        public int? CurrentWarehouseQty { get; set; }
        public int? CurrentShowroomQty { get; set; }
    }
}
