using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class MPayRollDisplayModel
    {
        [Key]
        public int PayRollId { get; set; }
        public int? StaffId { get; set; }
        public string ECode { get; set; }
        public string EName { get; set; }
        public string EmployeeType { get; set; }
        public int? MonthNumber { get; set; }
        public int? YearNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? NumberOfWorkingDays { get; set; }
        public int NumberOfAbsentDays { get; set; }
        public int? NumberPresentDays { get; set; }
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
        public decimal? Bonus { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? NetSalary { get; set; }
        public DateTime? PaidDate { get; set; }
        public int? PaidBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string Remarks { get; set; }
        public bool? IsPostedToFinance { get; set; }
    }
}
