using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class StockBallanceTable
    {
        [Key]
        public int PrId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? WHOpeningBal { get; set; }
        public decimal? WHClosingBal { get; set; }
        public decimal? SROpeningBal { get; set; }
        public decimal? SRClosingBAl { get; set; }
        public decimal? ITOpeningBal { get; set; }
        public decimal? ITClosingBal { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
