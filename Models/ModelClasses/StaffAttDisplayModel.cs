using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class StaffAttDisplayModel
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string EmployeeCode { set; get; }
        public List<DailyAttModel> attList { set; get; }

        public DateTime DisplayDate { get; set; }
    }
    public class DailyAttModel
    {
        public int Day { get; set; }
        public string Status { get; set; }
    }
        
}
