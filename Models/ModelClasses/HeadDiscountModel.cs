using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class HeadDiscountModel
    {
        public int TRId { get; set; }
        public decimal? SEDisc { set; get; }
        public decimal? SHDisc { set; get; }
        public string GivenBy {get; set; }
        public decimal GivenPercentage { get; set; }
        public string Remarks { get; set; }
    }
}
