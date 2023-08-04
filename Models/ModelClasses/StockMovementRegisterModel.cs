using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class StockMovementRegisterModel
    {
        public int TRId { get; set; }
        public int? InventoryId { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
        [Required(ErrorMessage = "Qty required")]
        public int? Qty { get; set; }
        public DateTime? MovedOn { get; set; }
        public string MovedBy { get; set; }
        public string Notes { get; set; }
        public bool? IsDeleted { get; set; }

        public int? WarehouseQty { get; set; }
        public int? ShowroomQty { get; set; }
        public string ModelNumber { get;   set; }
    }

    public class StockMovementMasterModel
    {
        public int SMRId { get; set; }
        public DateTime? MovedOn { get; set; }
        public string MovedFrom { get; set; }
        public int StaffId { get; set; }
        public string MovedTo { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public string StockTransferNumber { get; set; }
        public string MovedBy { get; set; }
        public int? Qty1 { get; set; }
        public int InventoryId1 { get; set; }
        public int?[] InventoryId { get; set; }
        public int?[] Qty { get; set; }

        public List<InventoryModelDrop> modelDrop { get; set; }
        public int customerID { get; set; }
    }


    public class StockMovementForPrint
    {

        public int SMIId { get; set; }
        public string StockTransferNumber { get; set; }
        public string DespatchDocumnetNo { get; set; }
        public string DespatchThrough { get; set; }
        public string EWayBillNo { get; set; }
        public DateTime? STDate { get; set; }
        public string Notes { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ToMobile { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromMobile { get; set; }
        public int? StockMovementMasterId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public int? SeqId { get; set; }
        public decimal? TotalValue { get; set; }
        public int SMRId { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
        public string AmountInWords { get; set; }
        public StockMovementMasterForPrint stockMaster { get; set; }

    }
    public class StockMovementMasterForPrint
    {
        public int SMRId { get; set; }
        public DateTime? MovedOn { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public string StockTransferNumber { get; set; }
        public string MovedBy { get; set; }
        public int? Qty1 { get; set; }
        public int InventoryId1 { get; set; }
        public List<StockMovementRegisterForPrint> items { get; set; }
    }
    public class StockMovementRegisterForPrint
    {
        
        public int TRId { get; set; }
        public int? InventoryId { get; set; }
        public string ProductImage { get; set; }
        public string MovedFrom { get; set; }
        public string MovedTo { get; set; }
        public int? Qty { get; set; }
        public DateTime? MovedOn { get; set; }
        public string MovedBy { get; set; }
        public string Notes { get; set; }
        public bool? IsDeleted { get; set; }
        public int? SMRId { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Description { get;  set; }
        public string ModelNumber { get;   set; }
    }
}