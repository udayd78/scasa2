using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class CreateNewStaffModel
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "User Name Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "User Email Id Required")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "User Mobile Number Required")]
        public string MobileNumber { get; set; }
        public int? UserTypeId { get; set; }
    }
}
