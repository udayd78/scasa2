using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StaffLoans
    {
        [Key]
        public int LoanId { get; set; }
        public int? StaffId { get; set; }
        public decimal? AmountTaken { get; set; }
        public DateTime? TakenOn { get; set; }
        public string RepaymentMode { get; set; }
        public int? NoofMonths { get; set; }
        public decimal? MonthlyEMI { get; set; }
        public int? GivenBy { get; set; }
        public string LoanStatus { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
