using SCASA.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{ 

    
   public class InventoryLocationMaster
    {
        [Key]
        public int locationId { get; set; }
        [Required(ErrorMessage = "Location Name Required")]
        public string LocationName { get; set; }
       
        public bool? IsDeleted { get; set; }

    }
}
