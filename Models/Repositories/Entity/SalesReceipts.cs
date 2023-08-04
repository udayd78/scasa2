using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class SalesReceipts
    {
        [Key]
        public int TRId { get; set; }
        public int? SOId { get; set; }
        public int? ReceivedBy { get; set; }
        public DateTime? RecievedOn { get; set; }
        public decimal? AmountReceived { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPostedToFinance { get; set; }
        public int? CustomerId { get; set; }
        public int? FinanceHeadId { get; set; }
        public string InstrumentNumber { get; set; }
        public string InstrumentDetails { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public string PaymentMode { get; set; }
    }
}
