using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class ReturnPolicy
    {
        [Key]
        public int ReturnId { get; set; }
        public string PolicyName { get; set; }
        public int? NoOfDays { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
