using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class SaleOrdersForAccounts
    {
        [Key]
        public int TRId { get; set; }
        public int? SOId { get; set; }
        public int? StaffId { get; set; }
        public int? CustomerId { get; set; }
        public string CurrentStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string TaxApplicable { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DelivaryCharges { get; set; }
        public decimal? DiscountBySe { get; set; }
        public decimal? DiscountBySHead { get; set; }
        public decimal? TotalOrderValue { get; set; }
        public decimal? TotalValue { get; set; }
        public bool? IsDeleted { get; set; }
        public int? LastModifiedBy_Id { get; set; }
        public string LastModifiedBy_Name { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public decimal RoundedValue { get; set; }
    }
}
