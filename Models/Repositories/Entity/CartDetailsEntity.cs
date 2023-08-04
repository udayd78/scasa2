using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class CartDetailsEntity
    {
        [Key]
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
    }
}
