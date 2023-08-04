using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class TargetMaster
    {
        [Key]
        public int TargetId { get; set; }
        public int? UserId { get; set; }
        public int? MonthNumber { get; set; }
        public int? YearNumber { get; set; }
        public decimal? TargetGiven { get; set; }
        public decimal? TargetDone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastMOdifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
