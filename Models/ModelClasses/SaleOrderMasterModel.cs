using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.ModelClasses
{
    public class SaleOrderMasterModel
    {
        public int SOMId { get; set; }
        public int? QuoteId { get; set; }
        public int? CustomerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int StaffId { get; set; }
        public string SalesExecutiveName { get; set; }
        public List<SaleOderDetailModel> saleOrderDetails { get; set; }
        public string CustomerName { get; set; }
        public int ShippingAddressId { get;   set; }
        public string CustomerAddress { get;   set; }
        public string TaxApplicable { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DelivaryCharges { get; set; }
        public string SOStatus { get; set; }
        public string OrderType { get; set; }
        public decimal RoundedValue { get; set; }
    }
    public class SaleOderDetailModel
    {
        public int? CurrentWarehouseQty { get; set; }
        public int? CurrentShowroomQty { get; set; }
        public int SODId { get; set; }
        public int? InventoryId { get; set; }
        public string InventoryImage { get; set; }
        public string InventoryTitle { get; set; }
        public int? SOMId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? DisAmtBySE { get; set; }
        public decimal? DisAmtByHead { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? ShippingAddressId { get; set; }
        public string LastMOdifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? Breadth { get; set; }
        public string ColorImage { get; set; }
        public string ColorName { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? OrderLineTotal { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? AdminDiscount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TaxDeducted { get; set; }
    }
    public class DoubleBoolClass
    {
        public bool shQtySelected { get; set; }
        public bool whQtySelected { get; set; }
    }
    public class DeleverySaleOrderModel
    {
        public int SOMId { get; set; }
        public string CustomerName { get; set; }
        public string CurrentStatus { get; set; }
        public int ShippingAddressId { get; set; }
        public string Address { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime? InvoiceCreatedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
} 