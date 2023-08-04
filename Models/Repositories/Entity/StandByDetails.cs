using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StandByDetails
    {
        [Key]
        public int DetailId { get; set; }
        public int? MasterId { get; set; }
        public int? InventoryId { get; set; }
        public int? Quantity { get; set; }
        public string Notes { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
