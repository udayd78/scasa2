using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class CustomerSalesActivity
    {
        [Key]
        public int ActivityId { get; set; }
        public int? StaffId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? AttendedOn { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
