using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class FinanceHeads
    {
        [Key]
        public int HeadId { get; set; }
        public int? GroupId { get; set; }
        [Required(ErrorMessage = "Head Name Required")]
        public string HeadName { get; set; }
        public decimal? StartingBallance { get; set; }
        public decimal? CurrentBallance { get; set; }
        public bool? IsDeleted { get; set; }
        public string HeadCode { get; set; }
        public int? CustomerId { get; set; }
        public int? StaffId { get; set; }
    }
}
