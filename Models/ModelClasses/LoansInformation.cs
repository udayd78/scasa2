using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class LoansInformation
    {
       public decimal? AmountTaken { get; set; }
        public DateTime? TakenOn { get; set; }
        public string RepaymentMode { get; set; }
        public int? NoofMonths { get; set; }
        public decimal? MonthlyEMI { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }
}
