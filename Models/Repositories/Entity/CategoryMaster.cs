using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class CategoryMaster
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Category Name Required")]
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
