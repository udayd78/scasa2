using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class SalesReceiptsModel
    {
        public int TRId { get; set; }
        public int? SOId { get; set; }
        public string StaffDetails { get; set; }
        public int? ReceivedBy { get; set; }
        public string RecievedBy_Name { get; set; }
        public DateTime? RecievedOn { get; set; }
        public decimal? AmountReceived { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPostedToFinance { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerDetails { get; set; }
        public int? FinanceHeadId { get; set; }
        public string FinanceHeadName { get; set; }
        public string InstrumentNumber { get; set; }
        public string InstrumentDetails { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public string PaymentMode { get; set; }
        public CompanyMaster companyDetails { get; set; }
    }

    
}
