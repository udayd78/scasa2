using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class SalesExecutiveListModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
      
        public string EmergencyContactNumber { get; set; }
        public string ProfileImage { get; set; }
        public decimal MaxDiscoutPercentage { get; set; } 
        public int? TotalQutations { get; set; }
        public int? TotalOrders { get; set; }
        public decimal TotalOrderValue { get; set; }
    }
}
