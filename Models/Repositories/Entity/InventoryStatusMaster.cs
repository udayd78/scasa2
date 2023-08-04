using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class InventoryStatusMaster
    {
        [Key]
        public int StatusId { get; set; }
        [Required(ErrorMessage = "Status is Required")]
        public string StatusName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
