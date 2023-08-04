using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class PrevilegeMaster
    {
        [Key]
        public int PrMId { get; set; }
        [Required(ErrorMessage = "Previlege Name Required")]
        public string PrevilegeName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
