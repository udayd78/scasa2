using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace SCASA.Models.Repositories.Entity
{
    public class InventoryImages
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageURL { get; set; }
        public int? InventoryId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
