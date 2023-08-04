using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class AppointmentPDFModel
    {
        public string EmployeeName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PIN { get; set; }
        public string Designation { get; set; }
        public DateTime DOJ { get; set; }
        public string ReportingManager { get; set; }
        public decimal Salary { get; set; }

    }
}
