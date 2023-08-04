using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class QuoteDetails
    {
        [Key]
        public int QuoteDetailsId { get; set; }
        public int? QuoteMasterId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? DisAmtBySE { get; set; }
        public decimal? DisAmtByHead { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
