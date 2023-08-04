using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class FinanceTransactions
    {
        [Key]
        public int TRID { get; set; }
        public DateTime? DateOfTransaction { get; set; }
        public int? FromHeadID { get; set; }
        public string Description { get; set; }
        public string VoucherType { get; set; }
        public int? VoucherNumber { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public DateTime? TTime { get; set; }
        public string DoneBy { get; set; }
        public string remarks { get; set; }
        public int? ToHeaID { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDetails { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }
        public int RelatedTrId { get; set; }
    }
}
