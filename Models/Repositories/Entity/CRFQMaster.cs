using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class CRFQMaster
    {
        [Key]
        public int CRFQId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string ClosedBy { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? QuoteReffId { get; set; }
        public int StaffId { get;   set; }
        public string CurrentStatus { get;   set; }
    }
}
