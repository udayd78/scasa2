using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class ReleiveStaffModel
    {
        public int StaffId { get; set; }
        [Required(ErrorMessage = "Remarks required")]
        public string Remarks { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Date is required")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RelievingDate { get; set; }
    }
}
