using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class ActivityLog
    {
        [Key]
        public int ActivityId { get; set; }
        public int? ActivityBy { get; set; }
        public string ActivityText { get; set; }
        public string ModuleName { get; set; }
        public string ActivityStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime? ActivityOn { get; set; }
        public bool? IsRead { get; set; }
        public int? TargerUserId { get; set; }
        public string ActivityByName { get; set; }
    }
}
