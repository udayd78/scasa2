using SCASA.Models.Utilities;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class CustomerMgmtService : ICustomerMgmtService
    {
        private readonly ICustomerMgmtRepo cRepo;
        public CustomerMgmtService(ICustomerMgmtRepo _cRepo)
        {
            cRepo = _cRepo;
        }
        #region Customer
        public ProcessResponse SaveCustomer(CustomerDisplayModel request)
        {
            ProcessResponse response = new ProcessResponse();
            if (request.Cid > 0)
            {
                CustomerMaster cm = cRepo.GetCustomerById(request.Cid);
                
                cm.FullName = request.FullName;
                cm.MobileNumber = request.MobileNumber;
                cm.EmailId = request.EmailId;
                cm.WhatsAppNumber = request.WhatsAppNumber;
                cm.CurrentStatus = request.CurrentStatus;
        
                cm.OrganigationName = request.OrganigationName;
                cm.OrginizationAddress = request.OrginizationAddress;
                cm.OrganizationNumber = request.OrganizationNumber;
                cm.OrganizationEmailId = request.OrganizationEmailId;
                cm.GSTNumber = request.GSTNumber;
                response = cRepo.UpdateCustomer(cm);
            }
            else
            {
                CustomerMaster cm = new CustomerMaster();

                CloneObjects.CopyPropertiesTo(request, cm);
                cm.IsDeleted = false;
                response = cRepo.SaveCustomer(cm);
            }
            return response;
        }
        public List<CustomerListModel> GetAllCustomers(int number=1,int size=10, string search="")
        {
            return cRepo.GetAllCustomers(number,size,search);
        }
        public int GetNumberOfCustomers(string search)
        {
            return cRepo.GetCustomers_Count(search);
        }
        public CustomerMaster GetCustomerById(int id)
        {
            return cRepo.GetCustomerById(id);
        }
        public List<CustomerDisplayModel> CustomerList()
        {
            return cRepo.CustomerList();
        }
        #endregion

        #region Address
        public ProcessResponse SaveAddress(CustomerAddressModel request)
        {
            ProcessResponse response = new ProcessResponse();
            if (request.Addid > 0)
            {
                AddressMaster am = cRepo.GetAddressById(request.Addid);

                am.HouseNumber = request.HouseNumber;
                am.StreetName = request.StreetName;
                am.Location = request.Location;
                am.CityId = request.CityId;
                am.StateId = request.StateId;
                am.CountryId = request.CountryId;
                am.AddressType = request.AddressType;
                am.IsShipping = request.IsShipping;
                am.IsDeleted = request.IsDeleted;
                am.PostalCode = request.PostalCode;
                response = cRepo.UpdateAddress(am);
            }
            else
            {
                AddressMaster am = new AddressMaster();
                CloneObjects.CopyPropertiesTo(request,am);
                am.IsDeleted = false;
                am.CustomerId = request.Cid;
                response = cRepo.SaveAddress(am);
                
            }
            return response;
        }
        public List<CustomerAddressModel> GetAddress(int id)
        {
            return cRepo.GetAddress(id);
        }
        public AddressMaster GetAddressById(int id)
        {
            return cRepo.GetAddressById(id);
        }
        public List<AddressMaster> getAddListByUserId(int id)
        {
            return cRepo.GetAddressByUserId(id);
        }
        public CustomerAddressModel GetAddModelByUserId(int id)
        {
            return cRepo.GetAddModelByUserId(id);
        }
        public List<AddressMaster> GetUserAddresses(int id)
        {
            return cRepo.GetAddressByUserId(id);
        }

        public CartMasterModel GetMyCart(int customerId, int salesExecutiveId)
        {
            return cRepo.GetMyCart(customerId,   salesExecutiveId);
        }

        public ProcessResponse SaveCartMaster(CartMasterEntity cm)
        {
            return cRepo.SaveCartMaster(cm);
        }

        public ProcessResponse SaveCartDetails(CartDetailsEntity cm)
        {
            return cRepo.SaveCartDetails(cm);
        }

        public CartMasterEntity GetCartMaster(int id)
        {
            return cRepo.GetCartMaster(id);
        }

        public CartDetailsEntity CartDetailMaster(int id)
        {
            return cRepo.CartDetailMaster(id);
        }

        public int GetUserCartCount(int userId, int salesExecutiveId)
        {
            return cRepo.GetUserCartCount(userId,salesExecutiveId);
        }

        public CartDetailsEntity GetCartDetailById(int id)
        {
            return cRepo.GetCartDetailById(id);
        }

        public void UpdateCartDetails(CartDetailsEntity cartDetailsEntity)
        {
            cRepo.UpdateCartDetails(cartDetailsEntity);
        }

        public ProcessResponse SaveCRFQ(CRFQMaster cm)
        {
            return cRepo.SaveCRFQ(cm);
        }

        public ProcessResponse SaveCRFQDetails(CRFQDetails cm)
        {
            return cRepo.SaveCRFQDetails(cm);
        }

        public CRFQMaster GetCRFQMaster(int id)
        {
            return cRepo.GetCRFQMaster(id);
        }

        public CRFQDetails GetCRFQDetails(int id)
        {
            return cRepo.GetCRFQDetails(id);
        }

        public List<CRFQDetails> GetCRFQDetailsByCRFQId(int id)
        {
            return cRepo.GetCRFQDetailsByCRFQId(id);
        }

        public List<CRFQMasterModel> GetMyCRFQs(int customerId, int staffId, int pageNumber = 1, int pageSize = 10)
        {
            return cRepo.GetMyCRFQs(customerId, staffId,pageNumber,pageSize);
        }
        public int GatCrfqsOfSECount(int staffId)
        {
            return cRepo.GatCrfqsOfSECount(staffId);
        }
        public List<CRFQMasterModel> GetCRFQsOfStaff(int staffId)
        {
            return cRepo.GetCRFQsOfStaff(staffId);
        }

        public ProcessResponse CloseCartMaster(int cartId)
        {
            return cRepo.CloseCartMaster(cartId);
        }

        public CRFQMasterModel GetSingleCRFQ(int crfqId)
        {
            return cRepo.GetSingleCRFQ(crfqId);
        }

        public void SubmitQuoteForDiscount(QuotesSubmittedForApproval request)
        {
              cRepo.SubmitQuoteForDiscount(request);
        }

        public QuotesSubmittedForApproval GetQuoteSubmittedById(int trid)
        {
            return cRepo.GetQuoteSubmittedById(trid);
        }

        public List<QuotesSubmittedForApprovalModel> GetQuotesSubmittedForApproval(int id)
        {
            return cRepo.GetQuotesSubmittedForApproval(id);
        }

        public QuotesSubmittedForApprovalModel GetQuotesSubmittedForApprovalSingle(int trid)
        {
            return cRepo.GetQuotesSubmittedForApprovalSingle(trid);
        }

        public ProcessResponse HeadDiscountSubmit(HeadDiscountModel request, int logId)
        {
              return  cRepo.HeadDiscountSubmit(request,logId);
        }
        public ProcessResponse AdminDiscountSubmit(HeadDiscountModel request, int seId)
        {
            return cRepo.AdminDiscountSubmit(request, seId);
        }

        public List<SaleOrderMasterModel> GetMySaleOrders(int staffId, int pageNumber = 1, int pageSize = 10)
        {
            return cRepo.GetMySaleOrders(staffId,pageNumber,pageSize);
        }
        public int GatCountOfOpenSO(int staffId)
        {
            return cRepo.GatCountOfOpenSO(staffId);
        }
        public List<SaleOrderMasterModel> GetMyClosedOrders(int staffId, int pageNumber = 1, int pageSize = 10)
        {
            return cRepo.GetMyClosedOrders(staffId,pageNumber,pageSize);
        }
        public int GetCountOfOpenClosedOrders(int staffId)
        {
            return cRepo.GetCountOfOpenClosedOrders(staffId);
        }
        public List<SaleOrderMasterModel> GetOpenSaleOrder(int staffId = 0, int pageNumber = 1, int pageSize = 10)
        {
            return cRepo.GetOpenSaleOrder(staffId,pageNumber,pageSize);
        }
        public int GetOpenSaleOrderCount(int staffId = 0)
        {
            return cRepo.GetOpenSaleOrderCount(staffId);
        }

        public ProcessResponse GenerateDC(CreateDCModel request, int currentUserId)
        {
            return cRepo.GenerateDC(request, currentUserId);
        }

        public ProcessResponse SaveReservedQtyMaster(ReservedQtyMaster request)
        {
            return cRepo.SaveReservedQtyMaster(request);
        }
        public List<DCMasterModel> GetAllDC(string src)
        {
            return cRepo.GetAllDC(src);
        }
        public List<SODModelForDC> GetItemsOfINvoiceMaster(int id)
        {
            return cRepo.GetItemsOfINvoiceMaster(id);
        }
        public TaxInvoiceMasterModel GetTaxInvoice(int invoiceid)
        {
            return cRepo.GetTaxInvoice(invoiceid);
        }

        public void UpdateEWayBill(string ewaybill, int dcmid)
        {
              cRepo.UpdateEWayBill(ewaybill, dcmid);
        }
        public void UpdateCustGstNumber(string gNo, int dcmid)
        {
            cRepo.UpdateCustGstNumber(gNo, dcmid);
        }

        public void UpdateDispatched(DateTime dDate, int dBy, int dcmid)
        {
            cRepo.UpdateDispatched(dDate, dBy, dcmid);
        }

        public DCMaster GetDCById(int id)
        {
            return cRepo.GetDCById(id);
        }

        public List<SaleOrderMasterModel> GetMySaleOrdersCustom(int staffId)
        {
            return cRepo.GetMySaleOrdersCustom(staffId);
        }
        public decimal GetQuoteExtraDisc(int crfrId)
        {
            return cRepo.GetQuoteExtraDisc(crfrId);
        }

        public ProcessResponse CreateCustomerFH(int id)
        {
            return cRepo.CreateCustomerFH(id);
        }
        public AddressMaster GetFirstAddressOfCustomer(int id)
        {
            return cRepo.GetFirstAddressOfCustomer(id);
        }
        #endregion
        public ProcessResponse CreatePartialDC(ModelForRDToDel res)
        {
            return cRepo.CreatePartialDC(res);
        }
    }
}
