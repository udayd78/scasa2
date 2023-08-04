using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace SCASA.Models.Repositories.Entity
{
    public class CartMasterEntity
    {
        [Key]
        public int CartId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedByName { get; set; }
        public int? CreatedById { get; set; }
        public int? CustomerId { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; } 
        public string OrderType { get; set; }
    }
}
