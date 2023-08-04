using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class Recipts
    {
        [Key]
        public int PayId { get; set; }
        //[DataType(DataType.Date)]
        //[Required(ErrorMessage = "This item is required")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PaymentDate { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Enter only Numbers")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "Description is required")]
        
        public string Description { get; set; }
        public string DoneBy { get; set; }
        public DateTime? DoneOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
