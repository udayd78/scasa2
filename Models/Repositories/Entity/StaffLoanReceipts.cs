using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StaffLoanReceipts
    {
        [Key]
        public int RecieptId { get; set; }
        public int? LoanId { get; set; }
        public decimal? AmountDeducted { get; set; }
        public DateTime? DeductedOn { get; set; }
        public int? MonthNumber { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
