using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class CRFQDetails
    {
        [Key]
        public int CRFQDetailsId { get; set; }
        public int? CRFQId { get; set; }
        public int? InventoryId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemPrise { get; set; }
        public decimal? DisAmtBySE { get; set; }
        public decimal? DisAmtByHead { get; set; }
        public bool? IsDeleted { get; set; }
        public string InventoryImage { get;   set; }
        public string InventoryTitle { get;   set; }
        public decimal? TotalPrice { get;   set; }
        public decimal? Breadth { get;  set; }
        public string ColorImage { get;  set; }
        public string ColorName { get;  set; }
        public decimal? Height { get;  set; }
        public decimal? Width { get;  set; }
        public decimal? OrderLineTotal { get; set; }
        public decimal? AdminDiscount { get; set; }
    }
}
