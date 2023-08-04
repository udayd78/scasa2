using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface ISalesMasterMgmtRepo
    {

        List<FinanceHeads> GetFinanceHeads();
        ProcessResponse PostReceiptToFinance(int trid); 
        List<SalesReceiptsModel> GetSalesReciepts(int soid);
        ProcessResponse SaveSalesReceipt(SalesReceipts request);
         ProcessResponse SubmitSOForAccounts(int soid, int currentUserId);

         List<SaleOrdersForAccountsModel> GetOrdersSubmittedForAccounts();
        void SaveActivity(CustomerSalesActivity ca);
        List<LeadInfo> GetLeadInfo(int staffId);
        List<LoansInformation> GetLoansInformation(int staffId);
        List<AttendanceInfo> GetStaffAttendance(int staffId);
       
        List<MPayRollDisplayModel> GetMonthlyPayRoll(int staffId);
        List<ProductsDisplaySales> SearchProducts(int catid = 0, int subCatId = 0, string search = "", int pageNumber = 1, int pageSize = 12);
        int SearchProdsCount(int catid = 0, int subCat=0, string search = "");
        List<ProductsDisplaySales> GetProductsByCats(int catid, int pageNumber = 1, int pageSize = 12);
        int GetCatProdsCount(int catid);
        ProcessResponse SaveCRFQ(CRFQMaster request);
        List<CRFQMaster> GetAllCRFQ();
        CRFQMaster GetCRFQById(int id);
        ProcessResponse UpdateCRFQ(CRFQMaster request);
        ProcessResponse SaveQuote(QuoteMaster request);
        List<QuoteMaster> GetAllQuote();
        QuoteMaster GetQuoteById(int id);
        ProcessResponse UpdateQuote(QuoteMaster request);
        ProcessResponse SaveSalesOrder(SalesOrderMaster request);
        List<SalesOrderMaster> GetAllSales();
        SalesOrderMaster GetSalesById(int id);
        ProcessResponse UpdateSales(SalesOrderMaster request);
        void LogError(Exception ex);
        CustomerSalesActivity GetActivity(int id);
        void UpdateActivty(CustomerSalesActivity request);
        CompanyMaster GetCompany();
        ProcessResponse DeleteSaleOrder(int id);

    }
}
