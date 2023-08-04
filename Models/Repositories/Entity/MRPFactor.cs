using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class MRPFactor
    {
        [Key]
        public int TRId { get; set; }
        public string FactorName { get; set; }
        public decimal? FactorValue { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
