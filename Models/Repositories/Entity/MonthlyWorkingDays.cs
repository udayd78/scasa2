using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class MonthlyWorkingDays
    {

        [Key]
        public int TRId { get; set; }
        [Required(ErrorMessage = "Month Number Required")]
        public int MonthNumber { get; set; }
        [Required(ErrorMessage = "Year Number Required")]
        public int YearNumber { get; set; }

        public bool? IsDeleted { get; set; }
        public int NumberOfDays { get; set; }
    }
}
