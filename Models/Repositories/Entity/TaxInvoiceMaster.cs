using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class TaxInvoiceMaster
    {
        [Key]
        public int InvoiceId { get; set; }
        public int? SOMId { get; set; }
        public string InvoiceNumber { get; set; }
        public string EWayBillNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string OtherRemarks { get; set; }
        public string DespatchDocumnetNo { get; set; }
        public string DespatchThrough { get; set; }
        public string EWayBillNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Notes { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ToMobile { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromMobile { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public int? SeqId { get; set; }
        public int? SMRId { get; set; }
        public decimal? TotalValue { get; set; }
        public string CreatedBy { get;   set; }
        public DateTime CreatedOn { get;   set; }
        public string CustomerName { get; set; }
        public string CustGstNumber { get; set; }
        public decimal? RoundedValue { get; set; }
    }
}
