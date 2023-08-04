
using SCASA.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class AttendanceMaster
    {
        [Key]
        public int TRId { get; set; }
        public Nullable<DateTime> DateofAttendance { get; set; }
        public Nullable<DateTime> InTime { get; set; }
        public Nullable<DateTime> OutTime { get; set; }
        public string AttStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public string Remarks { get; set; }
        public int? StaffId { get; set; }

        

    }
}
