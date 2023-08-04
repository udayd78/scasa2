using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class QuoteModelForMail
    {
        public List<CRFQDetails> crfqsList { get; set; }
        public CompanyMaster companyDetails { get; set; }
        public CustomerMaster custMaster { get; set; }
        public UserMaster sExecutive { set; get; }
        public string imgURL { get; set; }
    }
    public class OrderModerForMail
    {
        public List<SalesOrderDetails> Orders { get; set; }
        public CompanyMaster companyDetails { get; set; }
        public CustomerMaster custMaster { get; set; }
        public UserMaster sExecutive { set; get; }
        public string imgURL { get; set; }
        public decimal roundedValue { get; set; }
        public decimal delivaryCharges { get; set; }
    }
}
