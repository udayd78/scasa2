using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class SalesOrderMaster
    {
        [Key]
        public int SOMId { get; set; }
        public int? QuoteId { get; set; }
        public int? CustomerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int StaffId { get;   set; }
        public int ShippingAddressId { get;   set; }
        public string TaxApplicable { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DelivaryCharges { get; set; }
        public string SOStatus { get; set; }
        public string OrderType { get; set; }
        public decimal RoundedValue { get; set; }
        public DateTime? InvoiceCreatedDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string Notes { get; set; }
        public string DespatchDocumnetNo { get; set; }
        public string DespatchThrough { get; set; }
    }
}
