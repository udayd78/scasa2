using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class QuotesSubmittedForApprovalModel
    {
        public int TRId { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public int? QuoteMasterId { get; set; }
        public string CurrentStatus { get; set; }
        public decimal? OrderValue { get; set; }
        public decimal? SalesExecDiscount { get; set; }
        public decimal? SalesHeadDiscount { get; set; }
        public decimal? DiscountGiven { get; set; }
        public DateTime? GivenOn { get; set; }
        public string GivenBy { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public int SubmittedById { get; internal set; }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public CRFQMasterModel quoteDetails { get; set; }
        public decimal? AdminDiscount { get; set; }
    }
}
