using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class GSTMaster
    {
        [Key]
        public int GSTMasterId { get; set; }
        [Required(ErrorMessage = "Tax Name Required")]
        public string TaxName { get; set; }
        [Required(ErrorMessage = "Tax Name Required")]
        public decimal? TaxAmount { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
