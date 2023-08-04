using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StockMovementRegister
    {
        [Key]
        public int TRId { get; set; }
        public int? InventoryId { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
        public int? Qty { get; set; }
        public DateTime? MovedOn { get; set; }
        public string MovedBy { get; set; }
        public string Notes { get; set; }
        public bool? IsDeleted { get; set; }
        public int? SMRId { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
