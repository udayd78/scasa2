using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class SubCategoryMaster
    {
        [Key]
        public int SubCategoryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SubCategoryName { get; set; }
        public string SubCategoryImage { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CategoryId { get; set; }
    }
}
