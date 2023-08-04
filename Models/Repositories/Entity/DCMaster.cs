using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class DCMaster
    {
        [Key]
        public int DCId { get; set; }
        public int? InvoiceId { get; set; }
        public DateTime? DCDate { get; set; }
        public string ToAddress { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DespatchedOn { get; set; }
        public int? DespatchedBy { get; set; }
        public string EwayBillDetails { get; set; }
        public DateTime? EwayBillDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public string CustomerName { get; internal set; }
        public int? CustomerId { get; set; }
        public string CustGstNumber { get; set; }
        public int SOMId { get; set; }
    }
}
