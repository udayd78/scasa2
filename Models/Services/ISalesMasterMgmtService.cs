using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface ISalesMasterMgmtService
    {
        List<FinanceHeads> GetFinanceHeads();
        ProcessResponse PostReceiptToFinance(int trid);
        List<SalesReceiptsModel> GetSalesReciepts(int soid);
        ProcessResponse SaveSalesReceipt(SalesReceipts request);
        ProcessResponse SubmitSOForAccounts(int soid, int currentUserId);

        List<SaleOrdersForAccountsModel> GetOrdersSubmittedForAccounts();
        CustomerSalesActivity GetActivity(int id);
        void UpdateActivty(CustomerSalesActivity request);
        void SaveActivity(CustomerSalesActivity ca);
        List<LeadInfo> GetLeadInfo(int staffId);
        List<LoansInformation> GetLoansInformation(int staffId);
        List<AttendanceInfo> GetStaffAttendance(int staffId);

        List<MPayRollDisplayModel> GetMonthlyPayRoll(int staffId);
        List<MPayRollDisplayModel> GetPayrollForStaff(int staffId);
        List<ProductsDisplaySales> SearchProducts(int catid = 0, int subCatId = 0, string search = "", int pageNumber = 1, int pageSize = 12);
        int SearchProdsCount(int catid = 0, int subCatId = 0, string search = "");
        ProductDisplaySalesModel GetCatsForSales(int pageNumber = 1, int pageSize = 10);
        List<ProductsDisplaySales> GetCatProds(int cd, int pagenumber = 1, int pageSize = 12);
        int GetNoOfCartProds(int cid);
        ProcessResponse SaveCRFQ(CRFQMaster request);
        List<CRFQMaster> GetAllCRFQ();
        CRFQMaster GetCRFQById(int id);
        ProcessResponse SaveQuote(QuoteMaster request);
        List<QuoteMaster> GetAllQuote();
        QuoteMaster GetQuoteById(int id);
        ProcessResponse SaveSOMaster(SalesOrderMaster request);
        List<SalesOrderMaster> GetAllSales();
        SalesOrderMaster GetSalesById(int id);
        CompanyMaster GetCompany();
        ProcessResponse DeleteSaleOrder(int id);
    }
}
