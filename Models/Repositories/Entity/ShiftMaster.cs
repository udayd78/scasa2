using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class ShiftMaster
    {
        [Key]
        public int SMId { get; set; }

        [Required(ErrorMessage = "Shift Name Required")]
        public string ShiftName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
