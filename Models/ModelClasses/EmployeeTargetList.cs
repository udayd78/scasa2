using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class EmployeeTargetList
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
    }
}
