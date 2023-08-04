using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class CustomerListModel
    {
        public int Cid { get; set; }
        public string FullName { get; set; }
        public string CoustemerCode { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string WhatsAppNumber { get; set; }
       
        public string CityName { get; set; }
        public string Street { get; set; }
    }
    public class customertableModel
    {
        public int Cid { get; set; }
        public string FullName { get; set; }
        public string CoustemerCode { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string WhatsAppNumber { get; set; }
    }
    public class AddressModelForCustomer
    {
        public string CityName { get; set; }
        public string Street { get; set; }
    }
}
