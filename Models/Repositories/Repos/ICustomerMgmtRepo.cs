using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface ICustomerMgmtRepo
    {
        DCMaster GetDCById(int id);
        ProcessResponse CreateCustomerFH(int id);
        List<SaleOrderMasterModel> GetMySaleOrdersCustom(int staffId);
        void UpdateDispatched(DateTime dDate, int dBy, int dcmid);
        List<DCMasterModel> GetAllDC(string src);
        ProcessResponse SaveReservedQtyMaster(ReservedQtyMaster request);
        TaxInvoiceMasterModel GetTaxInvoice(int invoiceid);
        List<SODModelForDC> GetItemsOfINvoiceMaster(int id);
        void UpdateEWayBill(string ewaybill, int dcmid);
        void UpdateCustGstNumber(string gNo, int dcmid);
        ProcessResponse GenerateDC(CreateDCModel request, int currentUserId);
        List<SaleOrderMasterModel> GetOpenSaleOrder(int staffId = 0, int pageNumber = 1, int pageSize = 10);
        int GetOpenSaleOrderCount(int staffId = 0);
        List<SaleOrderMasterModel> GetMySaleOrders(int staffId, int pageNumber = 1, int pageSize = 10);
        int GatCountOfOpenSO(int staffId);
        List<SaleOrderMasterModel> GetMyClosedOrders(int staffId, int pageNumber = 1, int pageSize = 10);
        int GetCountOfOpenClosedOrders(int staffId);
        ProcessResponse HeadDiscountSubmit(HeadDiscountModel request, int seId);
        ProcessResponse AdminDiscountSubmit(HeadDiscountModel request, int seId);
        QuotesSubmittedForApprovalModel GetQuotesSubmittedForApprovalSingle(int trid);
         void SubmitQuoteForDiscount(QuotesSubmittedForApproval request);
        QuotesSubmittedForApproval GetQuoteSubmittedById(int trid);
        List<QuotesSubmittedForApprovalModel> GetQuotesSubmittedForApproval(int id);
        CRFQMasterModel GetSingleCRFQ(int crfqId);
        ProcessResponse CloseCartMaster(int cartId);
        ProcessResponse SaveCRFQ(CRFQMaster cm);

          ProcessResponse SaveCRFQDetails(CRFQDetails cm);

          CRFQMaster GetCRFQMaster(int id);
          CRFQDetails GetCRFQDetails(int id);
          List<CRFQDetails> GetCRFQDetailsByCRFQId(int id);

        List<CRFQMasterModel> GetMyCRFQs(int customerId, int staffId, int pageNumber = 1, int pageSize = 10);
        int GatCrfqsOfSECount(int staffId);
        CartDetailsEntity GetCartDetailById(int id);
          void UpdateCartDetails(CartDetailsEntity cartDetailsEntity);
        int GetUserCartCount(int userId, int salesExecutiveId);
        ProcessResponse SaveCustomer(CustomerMaster request);
        List<CustomerListModel> GetAllCustomers(int pageNumber = 1, int pageSize = 10, string search = "");
        int GetCustomers_Count(string search = "");
        CustomerMaster GetCustomerById(int id);
        List<CustomerDisplayModel> CustomerList();
        ProcessResponse UpdateCustomer(CustomerMaster request);
        CustomerAddressModel GetAddModelByUserId(int id);
        ProcessResponse SaveAddress(AddressMaster request);
        List<CustomerAddressModel> GetAddress(int id);
        AddressMaster GetAddressById(int id);
        List<AddressMaster> GetAddressByUserId(int id);
        ProcessResponse UpdateAddress(AddressMaster request);
        void LogError(Exception ex);
        CartMasterModel GetMyCart(int customerId, int salesExecutiveId);

        ProcessResponse SaveCartMaster(CartMasterEntity cm);
        ProcessResponse SaveCartDetails(CartDetailsEntity cm);

        CartMasterEntity GetCartMaster(int id);
        CartDetailsEntity CartDetailMaster(int id);
        List<CRFQMasterModel> GetCRFQsOfStaff(int staffId);
        decimal GetQuoteExtraDisc(int crfrId);
        AddressMaster GetFirstAddressOfCustomer(int id);
        ProcessResponse CreatePartialDC(ModelForRDToDel res);
    }
}
