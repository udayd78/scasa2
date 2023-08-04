using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class FinanceTransactionsModel
    {
        public int TRID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfTransaction { get; set; }
        public int? FromHeadID { get; set; }
        public string FromHeadID_Name { get; set; }
        public string FromGroupName { get; set; }
        public string Description { get; set; }
        public string VoucherType { get; set; }
        public int? VoucherNumber { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public DateTime? TTime { get; set; }
        public string DoneBy { get; set; }
        public string remarks { get; set; }
        public string ToHeadID_Name { get; set; }
        public string ToGroupName { get; set; }
        public int? ToHeaID { get; set; }
        public int? UserId { get; set; }
        public string UserId_Name { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerId_Name { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDetails { get; set; }
        public DateTime? ChequeDate { get; set; }
        public List<FHeadDrops> fromHeadDrops{get;set;}
        public List<FHeadDrops> toHeadDrops { get; set; }
        public CompanyMaster companyDetails { get; set; }
    }
    public class LedgerScreenModel
    {
        public List<FinanceTransactionsModel> Ledgers { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        public int LedId { get; set; }
        public decimal CurrentBallance { get; set; }
        public CompanyMaster companyDetails { get; set; }
        public string headName { get; set; }
        public string headAddress { get; set; }
    }
}
