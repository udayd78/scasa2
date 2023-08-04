using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class LoginResponse
    {
        public string userName { get; set; }
        public string emailId { get; set; }
        public string userTypeName { get; set; }
        public int userId { get; set; }
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public int userTypeCode { get; set; }

    }
}
