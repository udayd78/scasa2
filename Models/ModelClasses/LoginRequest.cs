using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email Id  / Mobile number required")]
        public string emailid { get; set; }

        [Required(ErrorMessage ="Password Required")]
        public string pword { get; set; }
        public string url { get; set; }
        public string mobileNumber { get; set; }
    }

    public class LoginRequestReset
    {

        public string emailid { get; set; }
        public string pword { get; set; }
        public string url { get; set; }
        public string mobileNumber { get; set; }
    }

    public class PasswordResetKeys
    {

        public string passkey { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string pword { get; set; }

        [Required(ErrorMessage = "Conf Password Required")]
        [Compare("pword",ErrorMessage ="Password mismatch")]
        public string confpword { get; set; }
        public string url { get; set; }
        public string mobileOtp { get; set; }
        public string emailOtp { get; set; }

    }
}
