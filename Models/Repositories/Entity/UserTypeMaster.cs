using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class UserTypeMaster
    {
        [Key]
        public int TypeId { get; set; }
        [Required(ErrorMessage ="Type Name required")]
        public string TypeName { get; set; }
        public bool? IsDeleted { get; set; }
        [Required(ErrorMessage = "Type Code required")]
        public int? TypeCode { get; set; }
    }
}
