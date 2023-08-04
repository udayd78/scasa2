using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class ModulesMaster
    {
        [Key]
        public int MoMId { get; set; }
        [Required(ErrorMessage = "Module Name Required")]
        public string ModuleName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
