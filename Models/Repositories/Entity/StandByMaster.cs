using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StandByMaster
    {
        [Key]
        public int TrId { get; set; }
        public int? CustomerId { get; set; }
        public int? AddressId { get; set; }
        public string MovedFrom { get; set; }
        public DateTime? MovedOn { get; set; }
        public string MovedBy { get; set; }
        public int? MovedStaffId { get; set; }
        public string StockTransferNumber { get; set; }
        public string currentStatus { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
