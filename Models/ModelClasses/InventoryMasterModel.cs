using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using SCASA.Models.Repositories.Entity;

namespace SCASA.Models.ModelClasses
{
    public class InventoryMasterModel
    {
        public int InventoryId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "This item is required")]
        public string ModelNumber { get; set; }
        [Required(ErrorMessage = "This item is required")]
        public string ItemDescription { get; set; }
        public string Brand { get; set; }

        [Required(ErrorMessage = "This item is required")]
        
        [Range(0, 9999999999999999.99,ErrorMessage ="Enter only Numbers")]
        public decimal? Height { get; set; }
        [Required(ErrorMessage = "This item is required")]
        
        [Range(0, 9999999999999999.99, ErrorMessage = "Enter only Numbers")]
        public decimal? Width { get; set; }
        [Required(ErrorMessage = "This item is required")]
         
        [Range(0, 9999999999999999.99, ErrorMessage = "Enter only Numbers")]
        public decimal? Breadth { get; set; }
        
        [Required(ErrorMessage = "This item is required")]
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
        [Required(ErrorMessage = "This item is required")]
        public int InventoryStatusId { get; set; }
        [Required(ErrorMessage = "This item is required")]
        public int InventoryConditonId { get; set; }
        [Required(ErrorMessage = "This item is required")]
        public int InventoryLocationId { get; set; }

        
        [Range(0, 9999999999999999.99,ErrorMessage ="Enter only Numbers")]
        public decimal? Velocity { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RecievedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        
        [Range(0, 9999999999999999.99, ErrorMessage = "Enter only digits")]
        [Required(ErrorMessage = "This item is required")]
        public decimal? MRPPrice { get; set; }

        [Required(ErrorMessage = "This item is required")]
        
        [Range(0, 9999999999999999.99, ErrorMessage = "Enter only digits")]
        public decimal? ActualPrice { get; set; }
        [Required(ErrorMessage = "This item is required")]
        public string Title { get; set; }
        public string RackName { get; set; }
        public string PODetails { get; set; }
        public int? MRPFactorId { get; set; }
        public decimal mrpfac { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PODate { get; set; }
        public string InvoiceNumber { get; set; }
       
        [Required(ErrorMessage = "This item is required")]
        [Range(1, 9999999999999999,ErrorMessage ="Minimum 1 qty required")]
        public int Qty { get; set; }
        public string PrimaryImage { get; set; }

        // other fields
        public List<IFormFile> ProductMainImageUploaded { get; set; }
        public List<IFormFile> ColorImageUploaded { get; set; }
        public List<IFormFile> ProductOtherImagesUploaded { get; set; }
        public List<IFormFile> DocumentsUploaded { get; set; }
        public int GSTMasterId { get; set; }
        public string DeleteReason { get; set; }
        public DateTime? DeletedOn { get; set; }
        public decimal? ThresholdValue { get; set; }
        public int? LeadTime { get; set; }

        //drop downs

        public List<InventoryStatusDrop>  inventoryStatusDrops{ get; set; }
        public List<ConditionDrop> conditionDrops { get; set; }
        public List<CategoryDrop>   categoryDrops{ get; set; }
        public List<SubCategoryDrop>  subCategoryDrops { get; set; }
        public List<LocationDrop> locationDrops { get; set; }
        public List<GSTDrop> gstDrop { get; set; }
        public List<MRPFactopDrop> mRPfactorDrops { get; set; }

        public string HSNCode { get; set; }
        public int ShowroomQty { get; set; }
        public int WharehouseQty { get; set; }

        public List<InventoryImages> OtherImages { get; set; }
        public string MainImage1 { get; set; }
        public List<string> MainImagesList { get; set; }
        public List<InventoryDocuments> DocsUploaded { get; set; }
    }
}
