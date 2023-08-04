using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class GSTMasterModel
    {
        public int GSTMasterId { get; set; }
        [Required(ErrorMessage = "Tax Name Required")]
        public string TaxName { get; set; }
        [Required(ErrorMessage = "Tax Name Required")]
        public decimal? TaxAmount { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public List<CategoryDrop> CatDros { get; set; }
        public List<SubCategoryDrop> SubCatDros { get; set; }
    }
    public class GSTListModel
    {
        public int GSTMasterId { get; set; }
        public string TaxName { get; set; }
        public bool? IsEnabled { get; set; }
        public decimal? TaxAmount { get; set; }        
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        
    }
}
