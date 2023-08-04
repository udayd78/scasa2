using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class CustomerMaster
    {
        [Key]
        public int Cid { get; set; }
        public string FullName { get; set; }
        public string CoustemerCode { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public int? AddressId { get; set; }
        public string WhatsAppNumber { get; set; }
        public string RegisteredBy { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public string RefferedBy { get; set; }
        public string OrganigationName { get; set; }
        public string OrginizationAddress { get; set; }
        public string OrganizationNumber { get; set; }
        public string OrganizationEmailId { get; set; }
        public bool? IsCustomer { get; set; }
        public int? FinanceHeadId { get;   set; }
        public string GSTNumber { get; set; }
    }
}
