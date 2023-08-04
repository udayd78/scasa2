using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StockMovementMaster
    {
        [Key]
        public int SMRId { get; set; }
        public DateTime? MovedOn { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public string StockTransferNumber { get; set; }
        public string MovedBy { get; set; }
    }
}
