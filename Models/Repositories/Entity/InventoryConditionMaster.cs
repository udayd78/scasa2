using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class InventoryConditionMaster
    {
        [Key]
        public int ConditionId { get; set; }
        [Required(ErrorMessage ="Condition Name Required")]
        public string ConditionName { get; set; }
        public bool? IsDeleted { get; set; }
       

    }
}
