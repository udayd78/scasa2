using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class ReservedQtyMaster
    {
        [Key]
        public int TRId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int DCmId { get; set; }
        public string SalesExename { get; set; }
        public int? SOMId { get; set; }
        public int? StandById { get; set; }
        public int? WQty { get; set; }
        public int? SQty { get; set; }
    }
}
