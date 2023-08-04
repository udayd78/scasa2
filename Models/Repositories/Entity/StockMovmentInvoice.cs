using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StockMovmentInvoice
    {
        [Key]
        public int SMIId { get; set; }
        public string StockTransferNumber { get; set; }
        public string DespatchDocumnetNo { get; set; }
        public string DespatchThrough { get; set; }
        public string EWayBillNo { get; set; }
        public DateTime? STDate { get; set; }
        public string Notes { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ToMobile { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromMobile { get; set; }
        public int? StockMovementMasterId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public int? SeqId { get; set; }
        public decimal? TotalValue { get; set; }
        public int SMRId { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
    }
}
