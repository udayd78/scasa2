using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class HolidayMaster
    {
        [Key]
        public int HMId { get; set; }
        public DateTime? HolidayDate { get; set; }

        [Required(ErrorMessage = "Holiday Name Required")]
        public string HolidayName { get; set; }
        public string HolidayDesc { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
