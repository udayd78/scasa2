using Microsoft.EntityFrameworkCore;
using SCASA.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Entity
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserMaster>().ToTable("UserMaster");
            modelBuilder.Entity<ApplicationErrorLog>().ToTable("ApplicationErrorLog");
            modelBuilder.Entity<CategoryMaster>().ToTable("CategoryMaster");
            modelBuilder.Entity<SubCategoryMaster>().ToTable("SubCategoryMaster");
            modelBuilder.Entity<UserTypeMaster>().ToTable("UserTypeMaster");
            modelBuilder.Entity<CityMaster>().ToTable("CityMaster");
            modelBuilder.Entity<CountryMaster>().ToTable("CountryMaster");
            modelBuilder.Entity<InventoryConditionMaster>().ToTable("InventoryConditionMaster");
            modelBuilder.Entity<InventoryLocationMaster>().ToTable("InventoryLocationMaster");
            modelBuilder.Entity<InventoryStatusMaster>().ToTable("InventoryStatusMaster");
            modelBuilder.Entity<StateMaster>().ToTable("StateMaster");
            modelBuilder.Entity<InventoryMaster>().ToTable("InventoryMaster");
            modelBuilder.Entity<InventoryImages>().ToTable("InventoryImages");
            modelBuilder.Entity<OtpTransactions>().ToTable("OtpTransactions");
            modelBuilder.Entity<GSTMaster>().ToTable("GSTMaster");
            modelBuilder.Entity<PayRollMaster>().ToTable("PayRollMaster");
            modelBuilder.Entity<EmailTemplateEntity>().ToTable("EmailTemplate");
            modelBuilder.Entity<AttendanceMaster>().ToTable("AttendanceMaster");
            modelBuilder.Entity<MonthlyWorkingDays>().ToTable("MonthlyWorkingDays");
            modelBuilder.Entity<MonthlyPayRoll>().ToTable("MonthlyPayRoll");
            modelBuilder.Entity<ModulesMaster>().ToTable("ModulesMaster");
            modelBuilder.Entity<PrevilegeMaster>().ToTable("PrevilegeMaster");
            modelBuilder.Entity<HolidayMaster>().ToTable("HolidayMaster");
            modelBuilder.Entity<ShiftMaster>().ToTable("ShiftMaster");
            modelBuilder.Entity<InventoryDisplayModel>().HasNoKey();
            modelBuilder.Entity<RecordsCountFromSql>().HasNoKey();
            modelBuilder.Entity<CustomerListModel>().HasNoKey();
            modelBuilder.Entity<customertableModel>().HasNoKey();
            modelBuilder.Entity<InventoryDocuments>().ToTable("InventoryDocuments");
            modelBuilder.Entity<StockMovementRegister>().ToTable("StockMovementRegister");
            modelBuilder.Entity<StockMovementDisplayModel>().HasNoKey();
            modelBuilder.Entity<CompanyMaster>().ToTable("CompanyMaster");
            modelBuilder.Entity<StockMovementMaster>().ToTable("StockMovementMaster");
            modelBuilder.Entity<StockMovmentInvoice>().ToTable("StockMovmentInvoice");
            modelBuilder.Entity<LoginTracking>().ToTable("LoginTracking");
            modelBuilder.Entity<CustomerMaster>().ToTable("CustomerMaster");
            modelBuilder.Entity<AddressMaster>().ToTable("AddressMaster");
            modelBuilder.Entity<CRFQMaster>().ToTable("CRFQMaster");
            modelBuilder.Entity<QuoteMaster>().ToTable("QuoteMaster");
            modelBuilder.Entity<SalesOrderMaster>().ToTable("SalesOrderMaster");
            modelBuilder.Entity<CRFQDetails>().ToTable("CRFQDetails");
            modelBuilder.Entity<QuoteDetails>().ToTable("QuoteDetails");
            modelBuilder.Entity<SalesOrderDetails>().ToTable("SalesOrderDetails");
            modelBuilder.Entity<FinanceGroups>().ToTable("FinanceGroups");
            modelBuilder.Entity<FinanceHeads>().ToTable("FinanceHeads");
            modelBuilder.Entity<CartMasterEntity>().ToTable("CartMaster");
            modelBuilder.Entity<CartDetailsEntity>().ToTable("CartDetails");
            modelBuilder.Entity<TargetMaster>().ToTable("TargetMaster");
            modelBuilder.Entity<StaffLoans>().ToTable("StaffLoans");
            modelBuilder.Entity<CustomerSalesActivity>().ToTable("CustomerSalesActivity");
            modelBuilder.Entity<MRPFactor>().ToTable("MRPFactor");
            modelBuilder.Entity<FinanceTransactions>().ToTable("FinanceTransactions");
            modelBuilder.Entity<StaffLoanReceipts>().ToTable("StaffLoanReceipts");
            modelBuilder.Entity<ActivityLog>().ToTable("ActivityLog");
            modelBuilder.Entity<QuotesSubmittedForApproval>().ToTable("QuotesSubmittedForApproval");
            modelBuilder.Entity<SaleOrdersForAccounts>().ToTable("SaleOrdersForAccounts");
            modelBuilder.Entity<SalesReceipts>().ToTable("SalesReceipts");
            modelBuilder.Entity<InvoiceItemDetails>().ToTable("InvoiceItemDetails");
            modelBuilder.Entity<TaxInvoiceMaster>().ToTable("TaxInvoiceMaster");
            modelBuilder.Entity<DCMaster>().ToTable("DCMaster");
            modelBuilder.Entity<ReservedQtyMaster>().ToTable("ReservedQtyMaster");
            modelBuilder.Entity<StandByMaster>().ToTable("StandByMaster");
            modelBuilder.Entity<StandByDetails>().ToTable("StandByDetails");
            modelBuilder.Entity<StandByInvoice>().ToTable("StandByInvoice");
            modelBuilder.Entity<Payments>().ToTable("Payments");
            modelBuilder.Entity<Recipts>().ToTable("Recipts");
            modelBuilder.Entity<StockBallanceTable>().ToTable("StockBallanceTable");
            modelBuilder.Entity<DeliveryDetails>().ToTable("DeliveryDetails");
            modelBuilder.Entity<ProductsDisplaySales>().HasNoKey();
            modelBuilder.Entity<InventoryCategoryPrintModel>().HasNoKey();
        }
        public DbSet<InvoiceItemDetails> invoiceItemDetails { get; set; }
        public DbSet<TaxInvoiceMaster> taxInvoiceMasters { get; set; }
        public DbSet<DCMaster> dCMasters { get; set; }
        public DbSet<SalesReceipts> salesReceipts { get; set; }
        public DbSet<SaleOrdersForAccounts> saleOrdersForAccounts { get; set; }
        public DbSet<QuotesSubmittedForApproval> quotesSubmittedForApprovals { get; set; }
        public DbSet<ActivityLog> activityLogs { get; set; }
        public DbSet<FinanceTransactions> financeTransactions { get; set; }
        public DbSet<CustomerSalesActivity> customerSalesActivities { get; set; }
        public DbSet<CartDetailsEntity> cartDetailsEntities { get; set; }
        public DbSet<CartMasterEntity> cartMasterEntities { get; set; }
        public DbSet<StockMovmentInvoice> stockMovmentInvoices { get; set; }
        public DbSet<StockMovementMaster> stockMovementMasters { get; set; }
        public DbSet<StockMovementRegister> stockMovementRegisters { get; set; }
        public DbSet<InventoryDocuments> inventoryDocuments { get; set; }
        public DbSet<EmailTemplateEntity> emailTemplateEntities { get; set; }
        public DbSet<GSTMaster> gSTMasters { get; set; }
        public DbSet<OtpTransactions> OtpTransactions { get; set; }
        public DbSet<InventoryMaster> inventoryMasters { get; set; }
        public DbSet<InventoryImages> InventoryImages { get; set; }
        public DbSet<CityMaster> cityMasters { get; set; }
        public DbSet<CountryMaster> countryMasters { get; set; }
        public DbSet<InventoryConditionMaster>inventoryConditionMasters { get; set; }
        public DbSet<InventoryLocationMaster>inventoryLocationMasters { get; set; }
        public DbSet<InventoryStatusMaster> inventoryStatusMasters { get; set; }
        public DbSet<StateMaster> stateMasters { set; get; }
        public DbSet<UserMaster> userMasters { set; get; }
        public  DbSet<ApplicationErrorLog>  applicationErrorLogs { get; set; }
        public DbSet<CategoryMaster> categoryMasters { get; set; }
        public DbSet<SubCategoryMaster> subCategoryMasters { get; set; }
        public DbSet<UserTypeMaster> userTypeMasters  { get; set; }
        public DbSet<PayRollMaster> payRollMasters { get; set; }
        public DbSet<AttendanceMaster> attendanceMasters { get; set; }
        public DbSet<MonthlyWorkingDays> mworkingDaysMasters { get; set; }
        public DbSet<MonthlyPayRoll> monthlyPayRolls { get; set; }
        public DbSet<ModulesMaster> modulesMasters { get; set; }
        public DbSet<PrevilegeMaster> previlegeMasters { get; set; }
        public DbSet<HolidayMaster> holidayMasters { get; set; }
        public DbSet<ShiftMaster> shiftMasters { get; set; }
        public DbSet<CompanyMaster> companyMasters { get; set; }
        public DbSet<LoginTracking> loginTrackings { get; set; }
        public DbSet<CustomerMaster> customerMasters { get; set; }
        public DbSet<AddressMaster>  addressMasters { get; set; }
        public DbSet<CRFQMaster>  cRFQMasters { get; set; }
        public DbSet<QuoteMaster>  quoteMasters { get; set; }
        public DbSet<SalesOrderMaster>  salesOrderMasters { get; set; }
        public DbSet<CRFQDetails>  cRFQDetails { get; set; }
        public DbSet<QuoteDetails>  quoteDetails { get; set; }
        public DbSet<SalesOrderDetails>  salesOrderDetails { get; set; }
        public DbSet<FinanceGroups> financeGroups { get; set; }
        public DbSet<FinanceHeads> financeHeads { get; set; }
        public DbSet<TargetMaster> targetMasters { get; set; }
        public DbSet<StaffLoans> staffLoans { get; set; }
        public DbSet<MRPFactor> mRPFactors { get; set; }
        public DbSet<StaffLoanReceipts> staffLoanReceipts { get; set; }
        public DbSet<ReservedQtyMaster> reservedQtyMasters { get; set; }
        public DbSet<StandByMaster> standByMasters { get; set; }
        public DbSet<StandByDetails> standByDetails { get; set; }
        public DbSet<StandByInvoice> standByInvoices { get; set; }
        public DbSet<Payments> payments { get; set; }
        public DbSet<Recipts> recipts { get; set; }
        public DbSet<StockBallanceTable> stockBallanceTables { get; set; }
        public DbSet<DeliveryDetails> deliveryDetails { get; set; }
    }
}
