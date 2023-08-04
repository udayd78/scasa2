using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class SMSRequest
    {
        public string mobileNumber { get; set; }
        public string smsText { get;set; }
        public string countryCode { get; set; }
    }
}
