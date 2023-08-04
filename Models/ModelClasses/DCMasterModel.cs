using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class DCMasterModel
    {
        public int DCId { get; set; }
        public int? InvoiceId { get; set; }
        public DateTime? DCDate { get; set; }
        public string ToAddress { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DespatchedOn { get; set; }
        public int? DespatchedBy { get; set; }
        public string EwayBillDetails { get; set; }
        public DateTime? EwayBillDate { get; set; }
        public List<InvoiceItemDetails> ItemDetails { get; set; }
        public TaxInvoiceMaster taxInvoice { get; set; }
        public string CustomerName { get; internal set; }
        public string CurrentStatus { get; set; }
        public string DespatchedByName { get;   set; }
        public int? CustomerId { get; set; }
        public string CustGstNumber { get; set; }
    }
    public class SODModelForDC
    {
        public int SODId { get; set; }
        public bool YesOrNo { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? InventoryId { get; set; }
        public string InventoryTitle { get; set; }
        public string InventoryImage { get; set; }
    }
    public class ModelForRDToDel
    {
        public int DCId { get; set; }
        public bool[] YesOrNo { get; set; }
        public List<SODModelForDC> Items { get; set; }
        public DateTime? DespatchedOn { get; set; }
        public int? DespatchedBy { get; set; }
        public int LogedUser { get; set; }
        public string EwayBillDetails { get; set; }
        public string CustGstNumber { get; set; }
    }

    public class TaxInvoiceMasterModel
    {
        public int InvoiceId { get; set; }
        public int? SOMId { get; set; }
        public string InvoiceNumber { get; set; }
        public string EWayBillNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string OtherRemarks { get; set; }
        public string DespatchDocumnetNo { get; set; }
        public string DespatchThrough { get; set; }
        public string EWayBillNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Notes { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string ToMobile { get; set; }
        public string ToEmail { get; set; }
        public string CustomerName { get; set; }
        public string FromEmail { get; set; }
        public string FromMobile { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public int? SeqId { get; set; }
        public int? SMRId { get; set; }
        public decimal? TotalValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<DeliveryDetails> ItemDetails { get; set; }
        public string ItemDescription { get; set; }
        public string totalValueInWords { get; set; }
        public string CustGstNumber { get; set; }
        public decimal? RoundedValue { get; set; }
        public decimal? DelivaryCharges { get; set; }
    }
    public class DCDeliverModel
    {
        public List<SalesOrderDetails> sodetails { get; set; }
        public int DCMasterId { get; set; }
        public DateTime? DespatchedOn { get; set; }
        
    }
}
