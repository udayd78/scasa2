using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SCASA.Models.Repositories.Entity
{
    public class InventoryMaster
    {

        [Key]
        public int InventoryId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public string Title { get; set; }
        public string ModelNumber { get; set; }
        
        public string ItemDescription { get; set; }
        public string Brand { get; set; }
        
        public decimal? Height { get; set; }
        
        public decimal? Width { get; set; }
       
        public decimal? Breadth { get; set; }
        
        public string ColorName { get; set; }
        public string ColorImage { get; set; }
      
        public int? InventoryStatusId { get; set; }
        
        public int? InventoryConditonId { get; set; }
       
        public int? InventoryLocationId { get; set; }
        public decimal? Velocity { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? RecievedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
       
        public decimal? MRPPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? SellablePrice { get; set; }
        public string RackName { get; set; }
        public string PODetails { get; set; }
        public DateTime? PODate { get; set; }
        public string InvoiceNumber { get; set; }
        public int? Qty { get; set; }
        public string PrimaryImage { get; set; }
        public string HSNCode { get; set; }
        public int? ShowroomQty { get; set; }
        public int? WharehouseQty { get; set; }
        public int? GSTMasterId { get; set; }
        public string DeleteReason { get; set; }
        public DateTime? DeletedOn { get; set; }
        public decimal? ThresholdValue { get; set; }
        public int? LeadTime { get; set; }
        public int? MRPFactorId { get; set; }
    }
}
