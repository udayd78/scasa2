using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class PayRollMaster
    {
        [Key]
        public int PRMId { get; set; }
        public int? StaffId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HRA { get; set; }
        public decimal? DearnessAllowance { get; set; }
        public decimal? FoodAllowance { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? MedicalAllowances { get; set; }
        public decimal? OtherAllowances { get; set; }
        public decimal? ProvidentFund { get; set; }
        public decimal? ProfessionalTax { get; set; }
        public decimal? ESIFund { get; set; }
        public decimal? TDS { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
