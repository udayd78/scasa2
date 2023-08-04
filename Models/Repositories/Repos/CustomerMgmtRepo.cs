using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class CustomerMgmtRepo : ICustomerMgmtRepo
    {
        private readonly MyDbContext context;

        public CustomerMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        #region Customer
        public ProcessResponse SaveCustomer(CustomerMaster request)
        {
            ProcessResponse responce = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.customerMasters.Add(request);
                context.SaveChanges();
                responce.currentId = request.Cid;
                responce.statusCode = 1;
                responce.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce;
        }
        public List<CustomerListModel> GetAllCustomers(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            List<CustomerListModel> response = new List<CustomerListModel>();
            List<customertableModel> firstData = new List<customertableModel>();
            try
            {
                SqlParameter[] sParams =
               {
                new SqlParameter("pageNumber",pageNumber),
                new SqlParameter("pageSize",pageSize),
                new SqlParameter("search", search ?? ""),
                };
                string sp = StoredProcedures.GetAllCustomers + " @pageNumber, @pageSize, @search";
                firstData = context.Set<customertableModel>().FromSqlRaw(sp, sParams).ToList();
                foreach(customertableModel c in firstData)
                {
                    CustomerListModel a = new CustomerListModel();
                    CloneObjects.CopyPropertiesTo(c, a);
                   
                        AddressModelForCustomer amc = (from ad in context.addressMasters
                                                       join ct in context.cityMasters on ad.CityId equals ct.Id
                                                       where ad.IsDeleted == false && ad.CustomerId == c.Cid
                                                       select new AddressModelForCustomer
                                                       {
                                                           CityName = ct.CityName,
                                                           Street = ad.StreetName
                                                       }).FirstOrDefault();
                    if (amc != null)
                    {
                        a.CityName = amc.CityName;
                        a.Street = amc.Street;
                    }
                    else
                    {
                        a.CityName = "";
                        a.Street = "";
                    }
                    response.Add(a);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public int GetCustomers_Count(string search = "")
        {
            //RecordsCountFromSql result = new RecordsCountFromSql();
            int total = 0;
            //try
            //{
            //    SqlParameter[] sParams =
            //    {
            //    new SqlParameter("search", search ?? ""),
            //    };
            //    string sp = StoredProcedures.GetAllCustomers_Count + "@search";
            //    total = context.Set<RecordsCountFromSql>().FromSqlRaw(sp, sParams).Select(r=>r.cnt).FirstOrDefault();
            //}
            //catch (Exception ex)
            //{

            //}
            total = context.customerMasters.Where(a => a.IsDeleted == false && (a.FullName.Contains(search) || a.EmailId.Contains(search) || a.MobileNumber.Contains(search)
                                                                                || a.WhatsAppNumber.Contains(search) ||a.OrganigationName.Contains(search) ||
                                                                                a.OrganizationEmailId.Contains(search) || a.OrganizationNumber.Contains(search))).Count();
            return total;
        }
        public CustomerMaster GetCustomerById(int id)
        {
            CustomerMaster response = new CustomerMaster();
            try
            {
                response = context.customerMasters.Where(a => a.IsDeleted == false && a.Cid == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse CreateCustomerFH(int id)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                var customer = context.customerMasters.Where(a => a.Cid == id).FirstOrDefault();
                var fGroup = context.financeGroups.Where(a => a.GroupName == "Sales Accounts" && a.IsDeleted == false).FirstOrDefault();
                FinanceHeads fh = new FinanceHeads();
                fh.CurrentBallance = 0;
                fh.CustomerId = id;
                fh.GroupId = fGroup.GroupId;
                fh.HeadCode = "Customer";
                fh.HeadName = customer.FullName;
                fh.StartingBallance = 0;
                fh.IsDeleted = false;
                context.financeHeads.Add(fh);
                context.SaveChanges();

                customer.FinanceHeadId = fh.HeadId;
                context.Entry(customer).CurrentValues.SetValues(customer);
                context.SaveChanges();
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.statusCode = 0;
                response.statusMessage = "Failed";
            }

            return response;
        }

        public List<CustomerDisplayModel> CustomerList()
        {
            List<CustomerDisplayModel> cl = new List<CustomerDisplayModel>();
            cl = (from c in context.customerMasters
                  where c.IsDeleted == false
                  select new CustomerDisplayModel
                  {
                      Cid = c.Cid,
                      FullName = c.FullName,
                      CoustemerCode = c.CoustemerCode,
                      MobileNumber = c.MobileNumber,
                      EmailId = c.EmailId,
                      WhatsAppNumber = c.WhatsAppNumber,
                      OrganigationName = c.OrganigationName,
                      OrginizationAddress = c.OrginizationAddress,
                      OrganizationEmailId = c.OrganizationEmailId,
                      OrganizationNumber = c.OrganizationNumber,
                      TotalOrders = context.salesOrderMasters.Where(a=>a.CustomerId == c.Cid).Count(),
                      TotalQuotations = context.cRFQMasters.Where(a=>a.CustomerId == c.Cid).Count(),
                      ReadyToDispatch = context.dCMasters.Where(a=>a.CustomerId == c.Cid && a.CurrentStatus == "Ready to Despatch").Count(),
                      DespatchedOrders = context.dCMasters.Where(a => a.CustomerId == c.Cid && a.CurrentStatus == "Despatched").Count(),
                      FinanceHeadId = c.FinanceHeadId

                  }).ToList();

            return cl;
        }
        public ProcessResponse UpdateCustomer(CustomerMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                CustomerMaster saveObj = context.customerMasters.Where(a => a.Cid == request.Cid).FirstOrDefault();
                context.Entry(saveObj).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.Cid;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Address
        public ProcessResponse SaveAddress(AddressMaster request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.addressMasters.Add(request);
                context.SaveChanges();
                responce.currentId = request.Addid;
                responce.statusCode = 1;
                responce.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                responce.statusCode = 0;
                responce.statusMessage = "Failed";
                LogError(ex);
            }
            return responce;

        }
        public List<CustomerAddressModel> GetAddress(int id)
        {
            List<CustomerAddressModel> response = new List<CustomerAddressModel>();
            try
            {
                response = (from a in context.addressMasters
                                //join cu in context.customerMasters on a.CustomerId equals cu.Cid
                            join c in context.countryMasters on a.CountryId equals c.Id
                            join s in context.stateMasters on a.StateId equals s.Id
                            join ct in context.cityMasters on a.CityId equals ct.Id
                            where a.IsDeleted == false && a.CustomerId == id
                            select new CustomerAddressModel
                            {
                                Addid = a.Addid,
                                HouseNumber = a.HouseNumber,
                                StreetName = a.StreetName,
                                Location = a.Location,
                                CountryName = c.CountryName,
                                StateName = s.StateName,
                                CityName = ct.CityName,
                                AddressType = a.AddressType,
                                Cid = (int)a.CustomerId,
                                PostalCode = a.PostalCode
                            }).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public AddressMaster GetAddressById(int id)
        {
            AddressMaster response = new AddressMaster();
            try
            {
                response = context.addressMasters.Where(a => a.IsDeleted == false && a.Addid == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List<AddressMaster> GetAddressByUserId(int id)
        {
            List<AddressMaster> response = new List<AddressMaster>();
            try
            {
                response = context.addressMasters.Where(a => a.IsDeleted == false && a.CustomerId == id).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public CustomerAddressModel GetAddModelByUserId(int id)
        {
            CustomerAddressModel response = new CustomerAddressModel();
            try
            {
                response = context.addressMasters.Where(a => a.IsDeleted == false && a.CustomerId == id)
                    .Select(a => new CustomerAddressModel
                    {
                        Addid = a.Addid,
                        HouseNumber = a.HouseNumber,
                        StreetName = a.StreetName,
                        Location = a.Location,
                        CityId = a.CityId,
                        StateId = a.StateId,
                        CountryId = a.CountryId,
                        AddressType = a.AddressType,
                        PostalCode=a.PostalCode

                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateAddress(AddressMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                AddressMaster saveObj = context.addressMasters.Where(a => a.Addid == request.Addid).FirstOrDefault();
                context.Entry(saveObj).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.Addid;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion
        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }

        public CartMasterModel GetMyCart(int customerId, int salesExecutiveId)
        {
            CartMasterModel result = new CartMasterModel();
            try
            {
                result = (from c in context.cartMasterEntities
                          where c.CustomerId == customerId && c.CreatedById == salesExecutiveId && c.CurrentStatus == "Open" && c.IsDeleted == false
                          select new CartMasterModel
                          {
                              CurrentStatus = c.CurrentStatus,
                              CustomerId = c.CustomerId,
                              CartId = c.CartId,
                              CreatedById = c.CreatedById,
                              CreatedByName = c.CreatedByName
                          }).FirstOrDefault();
                if (result != null)
                {
                    int cartid = result.CartId;
                    List<CartDetailsModel> cm = (from cd in context.cartDetailsEntities
                                                 where cd.IsDeleted == false && cd.CartId == cartid
                                                 select new CartDetailsModel
                                                 {
                                                     CartDetailId = cd.CartDetailId,
                                                     CartId = cd.CartId,
                                                     InventoryId = cd.InventoryId,
                                                     InventoryImage = cd.InventoryImage,
                                                     InventoryTitle = cd.InventoryTitle,
                                                     IsDeleted = cd.IsDeleted,

                                                     ItemPrice = cd.ItemPrice,
                                                     Qty = cd.Qty,
                                                     TotalPrice = cd.TotalPrice,
                                                     Width = cd.Width,
                                                     Height = cd.Height,
                                                     ColorImage = cd.ColorImage,
                                                     Breadth = cd.Breadth,
                                                     ColorName = cd.ColorName,
                                                     CurrentStatus = cd.CurrentStatus

                                                 }).ToList();
                    result.cartDetails = cm;
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public ProcessResponse SaveCartMaster(CartMasterEntity cm)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                var check = context.cartMasterEntities.Where(a => a.CustomerId == cm.CustomerId  && a.CreatedById == cm.CreatedById
                && a.CurrentStatus.Trim().Equals("Open")).FirstOrDefault();
                if (check == null)
                {
                    cm.IsDeleted = false;
                    cm.CurrentStatus = "Open";
                    context.cartMasterEntities.Add(cm);
                    context.SaveChanges();
                    response.currentId = cm.CartId;
                    response.statusCode = 1;
                    response.statusMessage = "Saved";
                }
                else
                {
                    response.currentId = check.CartId;
                    response.statusCode = 1;
                    response.statusMessage = "Saved";
                }

            }
            catch (Exception ex)
            {
                response.currentId = cm.CartId;
                response.statusCode = 0;
                response.statusMessage = "failed";
            }
            return response;
        }

        public CartMasterEntity GetCartMaster(int id)
        {
            return context.cartMasterEntities.Where(a => a.CartId == id).FirstOrDefault();
        }
        public ProcessResponse CloseCartMaster(int cartId)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                var check = context.cartMasterEntities.Where(a => a.CartId == cartId).FirstOrDefault();
                if (check != null)
                {
                    check.IsDeleted = true;
                    check.CurrentStatus = "Closed";
                    context.Entry(check).CurrentValues.SetValues(check);
                    context.SaveChanges();
                    response.statusCode = 1;
                    response.statusMessage = "closed";
                }
                else
                {
                    response.currentId = check.CartId;
                    response.statusCode = 1;
                    response.statusMessage = "Saved";
                }

            }
            catch (Exception ex)
            {
                response.currentId = cartId;
                response.statusCode = 0;
                response.statusMessage = "failed";
            }
            return response;
        }
        public ProcessResponse SaveCartDetails(CartDetailsEntity cm)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (cm.CartDetailId > 0)
                {
                    var check = context.cartDetailsEntities.Where(a => a.IsDeleted == false && a.CartDetailId == cm.CartDetailId).FirstOrDefault();
                    context.Entry(check).CurrentValues.SetValues(cm);
                    context.SaveChanges();
                }
                else
                {
                    cm.IsDeleted = false;
                    context.cartDetailsEntities.Add(cm);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                response.currentId = cm.CartDetailId;
                response.statusCode = 0;
                response.statusMessage = "failed";
            }
            return response;
        }


        public CartDetailsEntity CartDetailMaster(int id)
        {
            return context.cartDetailsEntities.Where(a => a.CartDetailId == id).FirstOrDefault();
        }

        public int GetUserCartCount(int userId, int salesExecutiveId)
        {
            int totalItems = 0;
            var cart = context.cartMasterEntities.Where(a => a.CustomerId == userId && a.IsDeleted == false && a.CreatedById == salesExecutiveId &&
            a.CurrentStatus == "Open").FirstOrDefault();
            if (cart == null)
            {
                totalItems = 0;
            }
            else
            {
                int currentId = cart.CartId;
                int itemCount = context.cartDetailsEntities.Where(a => a.CartId == currentId && a.IsDeleted == false).Count();
                totalItems = itemCount;
            }

            return totalItems;
        }

        public CartDetailsEntity GetCartDetailById(int id)
        {
            return context.cartDetailsEntities.Where(a => a.CartDetailId == id).FirstOrDefault();
        }
        public void UpdateCartDetails(CartDetailsEntity cartDetailsEntity)
        {
            var cd = context.cartDetailsEntities.Where(a => a.CartDetailId == cartDetailsEntity.CartDetailId).FirstOrDefault();
            context.Entry(cd).CurrentValues.SetValues(cartDetailsEntity);
            context.SaveChanges();
        }

        public ProcessResponse SaveCRFQ(CRFQMaster cm)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (cm.CRFQId > 0)
                {
                    var check = context.cRFQMasters.Where(a => a.IsDeleted == false && a.CRFQId == cm.CRFQId).FirstOrDefault();
                    context.Entry(check).CurrentValues.SetValues(cm);
                    context.SaveChanges();
                    response.currentId = cm.CRFQId;
                    response.statusCode = 1;
                    response.statusMessage = "success";
                }
                else
                {
                    cm.IsDeleted = false;
                    context.cRFQMasters.Add(cm);
                    context.SaveChanges();
                    response.currentId = cm.CRFQId;
                    response.statusCode = 1;
                    response.statusMessage = "success";
                }

            }
            catch (Exception ex)
            {
                response.currentId = 0;
                response.statusCode = 0;
                response.statusMessage = "failed";
            }
            return response;
        }

        public ProcessResponse SaveCRFQDetails(CRFQDetails cm)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (cm.CRFQDetailsId > 0)
                {
                    var check = context.cRFQDetails.Where(a => a.IsDeleted == false && a.CRFQDetailsId == cm.CRFQDetailsId).FirstOrDefault();
                    context.Entry(check).CurrentValues.SetValues(cm);
                    context.SaveChanges();
                }
                else
                {
                    cm.IsDeleted = false;
                    context.cRFQDetails.Add(cm);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                response.currentId = 0;
                response.statusCode = 0;
                response.statusMessage = "failed";
            }
            return response;
        }

        public CRFQMaster GetCRFQMaster(int id)
        {
            return context.cRFQMasters.Where(a => a.CRFQId == id).FirstOrDefault();
        }
        public CRFQDetails GetCRFQDetails(int id)
        {
            return context.cRFQDetails.Where(a => a.CRFQDetailsId == id).FirstOrDefault();
        }
        public List<CRFQDetails> GetCRFQDetailsByCRFQId(int id)
        {
            return context.cRFQDetails.Where(a => a.CRFQId == id &&a.IsDeleted==false).ToList();
        }

        public List<CRFQMasterModel> GetMyCRFQs(int customerId, int staffId, int pageNumber=1,int pageSize=10)
        {
            List<CRFQMasterModel> result = new List<CRFQMasterModel>();
            try
            {
                int skipNumber = 0;
                if (pageNumber > 1)
                {
                    skipNumber = (pageNumber - 1) * pageSize;
                }
                result = (from c in context.cRFQMasters
                          join u in context.userMasters on c.StaffId equals u.UserId
                          join uc in context.customerMasters on c.CustomerId equals uc.Cid
                          where c.StaffId == staffId && c.IsDeleted == false && c.CurrentStatus!= "SO Created"
                          select new CRFQMasterModel
                          {
                              ClosedBy = c.ClosedBy,
                              crfqDetails = null,
                              CRFQId = c.CRFQId,
                              StaffId = c.StaffId,
                              ClosedOn = c.ClosedOn,
                              CreatedBy = c.CreatedBy,
                              CreatedOn = c.CreatedOn,
                              CustomerId = c.CustomerId,
                              IsDeleted = c.IsDeleted,
                              QuoteReffId = c.QuoteReffId,
                              SalesExecutiveName = u.UserName,
                              CustomerName = uc.FullName,
                              CurrentStatus = c.CurrentStatus
                          }).OrderByDescending(b => b.CRFQId).Skip(skipNumber).Take(pageSize).ToList();
                if (result != null)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        int cartid = result[i].CRFQId;
                        List<CRFQDetailsModel> cm = (from cd in context.cRFQDetails
                                                     join im in context.inventoryMasters on cd.InventoryId equals im.InventoryId
                                                     where cd.IsDeleted == false && cd.CRFQId == cartid
                                                     select new CRFQDetailsModel
                                                     {
                                                         CRFQDetailsId = cd.CRFQDetailsId,
                                                         CRFQId = cd.CRFQId,
                                                         DisAmtByHead = cd.DisAmtByHead,
                                                         DisAmtBySE = cd.DisAmtBySE,
                                                         InventoryId = cd.InventoryId,
                                                         InventoryImage = cd.InventoryImage,
                                                         InventoryTitle = cd.InventoryTitle,
                                                         IsDeleted = cd.IsDeleted,
                                                         ItemPrise = cd.ItemPrise,
                                                         Quantity = cd.Quantity,
                                                         TotalPrice = cd.TotalPrice,
                                                         Breadth = cd.Breadth,
                                                         ColorImage = cd.ColorImage,
                                                         ColorName = cd.ColorName,
                                                         CurrentStatus = null,
                                                         Height = cd.Height,
                                                         Width = cd.Width,
                                                         OrderLineTotal = cd.OrderLineTotal,
                                                         AdminDiscount=cd.AdminDiscount,
                                                         CurrentWarehouseQty = im.WharehouseQty,
                                                         CurrentShowroomQty = im.ShowroomQty
                                                     }).ToList();
                        result[i].crfqDetails = cm;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public int GatCrfqsOfSECount(int staffId)
        {
            int r = 0;
            try
            {
                r = context.cRFQMasters.Where(a => a.StaffId == staffId && a.IsDeleted == false && a.CurrentStatus != "SO Created").Select(b => b.CRFQId).Count();
            }catch(Exception e)
            {

            }
            return r;

        }
        public CRFQMasterModel GetSingleCRFQ(int crfqId)
        {
            CRFQMasterModel result = new CRFQMasterModel();
            try
            {
                result = (from c in context.cRFQMasters
                          join u in context.userMasters on c.StaffId equals u.UserId
                          join uc in context.customerMasters on c.CustomerId equals uc.Cid
                          where c.CRFQId == crfqId && c.IsDeleted == false
                          select new CRFQMasterModel
                          {
                              ClosedBy = c.ClosedBy,
                              crfqDetails = null,
                              CRFQId = c.CRFQId,
                              StaffId = c.StaffId,
                              ClosedOn = c.ClosedOn,
                              CreatedBy = c.CreatedBy,
                              CreatedOn = c.CreatedOn,
                              CustomerId = c.CustomerId,
                              IsDeleted = c.IsDeleted,
                              QuoteReffId = c.QuoteReffId,
                              SalesExecutiveName = u.UserName,
                              CustomerName = uc.FullName,
                              CurrentStatus = c.CurrentStatus
                          }).FirstOrDefault();
                if (result != null)
                {

                    int cartid = crfqId;
                    List<CRFQDetailsModel> cm = (from cd in context.cRFQDetails
                                                 join i in context.inventoryMasters on cd.InventoryId equals i.InventoryId
                                                 where cd.IsDeleted == false && cd.CRFQId == cartid
                                                 select new CRFQDetailsModel
                                                 {
                                                     CRFQDetailsId = cd.CRFQDetailsId,
                                                     CRFQId = cd.CRFQId,
                                                     DisAmtByHead = cd.DisAmtByHead,
                                                     DisAmtBySE = cd.DisAmtBySE,
                                                     InventoryId = cd.InventoryId,
                                                     InventoryImage = cd.InventoryImage,
                                                     InventoryTitle = cd.InventoryTitle,
                                                     IsDeleted = cd.IsDeleted,
                                                     ItemPrise = cd.ItemPrise,
                                                     Quantity = cd.Quantity,
                                                     TotalPrice = cd.TotalPrice,
                                                     Breadth = cd.Breadth,
                                                     ColorImage = cd.ColorImage,
                                                     ColorName = cd.ColorName,
                                                     CurrentStatus = null,
                                                     Height = cd.Height,
                                                     Width = cd.Width,
                                                     OrderLineTotal = cd.OrderLineTotal,
                                                     AdminDiscount=cd.AdminDiscount,
                                                     
                                                 }).ToList();
                    result.crfqDetails = cm;


                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<CRFQMasterModel> GetCRFQsOfStaff(int staffId)
        {
            List<CRFQMasterModel> result = new List<CRFQMasterModel>();
            try
            {
                result = (from c in context.cRFQMasters
                          join u in context.userMasters on c.StaffId equals u.UserId
                          join uc in context.customerMasters on c.CustomerId equals uc.Cid
                          where c.StaffId == staffId && c.IsDeleted == false && c.CurrentStatus!= "SO Created"
                          select new CRFQMasterModel
                          {
                              ClosedBy = c.ClosedBy,
                              crfqDetails = null,
                              CRFQId = c.CRFQId,
                              StaffId = c.StaffId,
                              ClosedOn = c.ClosedOn,
                              CreatedBy = c.CreatedBy,
                              CreatedOn = c.CreatedOn,
                              CustomerId = c.CustomerId,
                              IsDeleted = c.IsDeleted,
                              QuoteReffId = c.QuoteReffId,
                              SalesExecutiveName = u.UserName,
                              CustomerName = uc.FullName
                          }).ToList();
                if (result != null)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        int cartid = result[i].CRFQId;
                        List<CRFQDetailsModel> cm = (from cd in context.cRFQDetails
                                                     join p in context.inventoryMasters on cd.InventoryId equals p.InventoryId
                                                     where cd.IsDeleted == false && cd.CRFQId == cartid
                                                     select new CRFQDetailsModel
                                                     {
                                                         CRFQDetailsId = cd.CRFQDetailsId,
                                                         CRFQId = cd.CRFQId,
                                                         DisAmtByHead = cd.DisAmtByHead,
                                                         DisAmtBySE = cd.DisAmtBySE,
                                                         InventoryId = cd.InventoryId,
                                                         InventoryImage = cd.InventoryImage,
                                                         InventoryTitle = cd.InventoryTitle,
                                                         IsDeleted = cd.IsDeleted,
                                                         ItemPrise = cd.ItemPrise,
                                                         Quantity = cd.Quantity,
                                                         TotalPrice = cd.TotalPrice,
                                                         CurrentShowroomQty=p.ShowroomQty,
                                                         CurrentWarehouseQty=p.WharehouseQty
                                                     }).ToList();
                        result[i].crfqDetails = cm;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public void SubmitQuoteForDiscount(QuotesSubmittedForApproval request)
        {
            if (request.TRId > 0)
            {
                var q = context.quotesSubmittedForApprovals.Where(a => a.TRId == request.TRId).FirstOrDefault();
                context.Entry(q).CurrentValues.SetValues(q);
                context.SaveChanges();
            }
            else
            {
                context.quotesSubmittedForApprovals.Add(request);
                context.SaveChanges();

            }
        }
        public QuotesSubmittedForApproval GetQuoteSubmittedById(int trid)
        {
            return context.quotesSubmittedForApprovals.Where(a => a.TRId == trid).FirstOrDefault();
        }
        public List<QuotesSubmittedForApprovalModel> GetQuotesSubmittedForApproval(int id)
        {
            UserMaster um = context.userMasters.Where(a => a.IsDeleted == false && a.UserId == id).FirstOrDefault();
            List<QuotesSubmittedForApprovalModel> myList = new List<QuotesSubmittedForApprovalModel>();
            UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeId == um.UserTypeId).FirstOrDefault();
            if (utm.TypeName == "Sales Head")
            {
                myList = (from q in context.quotesSubmittedForApprovals
                          join quote in context.cRFQMasters on q.QuoteMasterId equals quote.CRFQId
                          join cust in context.customerMasters on quote.CustomerId equals cust.Cid
                          join ca in context.addressMasters on cust.Cid equals ca.CustomerId
                          join city in context.cityMasters on ca.CityId equals city.Id
                          join state in context.stateMasters on ca.StateId equals state.Id
                          join country in context.countryMasters on ca.CountryId equals country.Id
                          join umast in context.userMasters on quote.StaffId equals umast.UserId
                          where q.IsDeleted == false && umast.ReportingManager == id
                          select new QuotesSubmittedForApprovalModel
                          {
                              CurrentStatus = q.CurrentStatus,
                              IsDeleted = q.IsDeleted,
                              DiscountGiven = q.DiscountGiven,
                              GivenBy = q.GivenBy,
                              GivenOn = q.GivenOn,
                              OrderValue = q.OrderValue,
                              QuoteMasterId = q.QuoteMasterId,
                              Remarks = q.Remarks,
                              SalesExecDiscount = q.SalesExecDiscount,
                              SubmittedBy = q.SubmittedBy,
                              SubmittedById = q.SubmittedById,
                              SubmittedOn = q.SubmittedOn,
                              CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                              CustomerName = cust.FullName,
                              TRId = q.TRId,
                              AdminDiscount=q.AdminDiscount
                          }).OrderByDescending(b=>b.TRId).ToList();
                if (myList.Count > 0)
                {
                    for (int i = 0; i < myList.Count; i++)
                    {
                        int currentId = (int)myList[i].QuoteMasterId;
                        CRFQMasterModel c = GetSingleCRFQ(currentId);
                        myList[i].quoteDetails = c;
                    }
                }                
            }
            else
            {
                myList = (from q in context.quotesSubmittedForApprovals
                          join quote in context.cRFQMasters on q.QuoteMasterId equals quote.CRFQId
                          join cust in context.customerMasters on quote.CustomerId equals cust.Cid
                          join ca in context.addressMasters on cust.Cid equals ca.CustomerId
                          join city in context.cityMasters on ca.CityId equals city.Id
                          join state in context.stateMasters on ca.StateId equals state.Id
                          join country in context.countryMasters on ca.CountryId equals country.Id
                          where q.IsDeleted == false
                          select new QuotesSubmittedForApprovalModel
                          {
                              CurrentStatus = q.CurrentStatus,
                              IsDeleted = q.IsDeleted,
                              DiscountGiven = q.DiscountGiven,
                              GivenBy = q.GivenBy,
                              GivenOn = q.GivenOn,
                              OrderValue = q.OrderValue,
                              QuoteMasterId = q.QuoteMasterId,
                              Remarks = q.Remarks,
                              SalesExecDiscount = q.SalesExecDiscount,
                              SubmittedBy = q.SubmittedBy,
                              SubmittedById = q.SubmittedById,
                              SubmittedOn = q.SubmittedOn,
                              CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                              CustomerName = cust.FullName,
                              TRId = q.TRId,
                              AdminDiscount=q.AdminDiscount
                          }).ToList();
                if (myList.Count > 0)
                {
                    for (int i = 0; i < myList.Count; i++)
                    {
                        int currentId = (int)myList[i].QuoteMasterId;
                        CRFQMasterModel c = GetSingleCRFQ(currentId);
                        myList[i].quoteDetails = c;
                    }
                }
            }
            return myList;
        }
        public QuotesSubmittedForApprovalModel GetQuotesSubmittedForApprovalSingle(int trid)
        {
            QuotesSubmittedForApprovalModel myList = new QuotesSubmittedForApprovalModel();
            myList = (from q in context.quotesSubmittedForApprovals
                      join quote in context.cRFQMasters on q.QuoteMasterId equals quote.CRFQId
                      join cust in context.customerMasters on quote.CustomerId equals cust.Cid
                      join ca in context.addressMasters on cust.Cid equals ca.CustomerId
                      join city in context.cityMasters on ca.CityId equals city.Id
                      join state in context.stateMasters on ca.StateId equals state.Id
                      join country in context.countryMasters on ca.CountryId equals country.Id
                      where q.IsDeleted == false && q.TRId == trid
                      select new QuotesSubmittedForApprovalModel
                      {
                          CurrentStatus = q.CurrentStatus,
                          IsDeleted = q.IsDeleted,
                          DiscountGiven = q.DiscountGiven,
                          GivenBy = q.GivenBy,
                          GivenOn = q.GivenOn,
                          OrderValue = q.OrderValue,
                          QuoteMasterId = q.QuoteMasterId,
                          Remarks = q.Remarks,
                          SalesExecDiscount = q.SalesExecDiscount,
                          SubmittedBy = q.SubmittedBy,
                          SubmittedById = q.SubmittedById,
                          SubmittedOn = q.SubmittedOn,
                          CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                          CustomerName = cust.FullName,
                          TRId = q.TRId,
                          AdminDiscount=q.AdminDiscount
                      }).FirstOrDefault();
            if (myList != null)
            {
                int currentId = (int)myList.QuoteMasterId;
                CRFQMasterModel c = GetSingleCRFQ(currentId);
                myList.quoteDetails = c;

            }
            return myList;
        }

        public ProcessResponse HeadDiscountSubmit(HeadDiscountModel request,int seId)
        {
            ProcessResponse pr = new ProcessResponse();
            UserMaster um = context.userMasters.Where(a => a.IsDeleted == false && a.UserId == seId).FirstOrDefault();
            UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeId == um.UserTypeId).FirstOrDefault();
            decimal shdis = (decimal)(request.SHDisc == null ? 0 : request.SHDisc);
            //decimal givenper = request.GivenPercentage == null ? 0 : request.GivenPercentage;
            decimal toDis = (decimal)(request.SEDisc +shdis+ request.GivenPercentage);

            Boolean giveDiscount = false;
            if (utm.TypeName == "Sales Head")
            {
                if(toDis <= um.MaxDiscoutPercentage)
                {
                    giveDiscount = true;
                }
            }
            else
            {
                if (toDis < 95)
                {
                    giveDiscount = true;
                }
            }
            if (giveDiscount)
            {
                // update discount table first
                QuotesSubmittedForApproval qsf = new QuotesSubmittedForApproval();
                qsf = context.quotesSubmittedForApprovals.Where(a => a.TRId == request.TRId).FirstOrDefault();
                qsf.Remarks = request.Remarks;
                qsf.GivenBy = request.GivenBy;
                qsf.GivenOn = DateTime.Now;
                qsf.DiscountGiven = request.GivenPercentage;
                qsf.CurrentStatus = "Discount Given by " + request.GivenBy;
                context.Entry(qsf).CurrentValues.SetValues(qsf);
                context.SaveChanges();

                // update quote details
                int currentCrfQId = (int)qsf.QuoteMasterId;
                List<CRFQDetails> cRFQDetails = new List<CRFQDetails>();
                cRFQDetails = context.cRFQDetails.Where(A => A.IsDeleted == false && A.CRFQId == currentCrfQId).ToList();
                if (cRFQDetails.Count > 0)
                {
                    foreach (var v in cRFQDetails)
                    {
                        var tempC = context.cRFQDetails.Where(A => A.CRFQDetailsId == v.CRFQDetailsId).FirstOrDefault();
                        tempC.DisAmtByHead = request.GivenPercentage;
                        tempC.TotalPrice = tempC.OrderLineTotal - (tempC.OrderLineTotal * ((tempC.DisAmtBySE + tempC.DisAmtByHead) / 100));
                        context.Entry(tempC).CurrentValues.SetValues(tempC);
                        context.SaveChanges();
                    }
                }

                // update quotemaster

                CRFQMaster cr = context.cRFQMasters.Where(A => A.CRFQId == currentCrfQId).FirstOrDefault();
                cr.CurrentStatus = "Discount Approved";
                context.Entry(cr).CurrentValues.SetValues(cr);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            else
            {
                pr.statusCode = 0;
                pr.statusMessage = "Limit Exceded";
            }
            return pr;
        }
        public ProcessResponse AdminDiscountSubmit(HeadDiscountModel request, int seId)
        {
            ProcessResponse pr = new ProcessResponse();
            UserMaster um = context.userMasters.Where(a => a.IsDeleted == false && a.UserId == seId).FirstOrDefault();
            UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeId == um.UserTypeId).FirstOrDefault();
            QuotesSubmittedForApproval qsf = new QuotesSubmittedForApproval();
            qsf = context.quotesSubmittedForApprovals.Where(a => a.TRId == request.TRId).FirstOrDefault();
            decimal SHDisc = (decimal)context.cRFQDetails.Where(a => a.IsDeleted == false && a.CRFQId == qsf.QuoteMasterId).Select(b => b.DisAmtByHead).FirstOrDefault();
            decimal toDis = (decimal)(request.SEDisc + SHDisc + request.GivenPercentage);
            Boolean giveDiscount = false;
           
                if (toDis < 95)
                {
                    giveDiscount = true;
                }
           
            if (giveDiscount)
            {
                // update discount table first
                
                qsf.Remarks = request.Remarks;
                qsf.GivenBy = request.GivenBy;
                qsf.GivenOn = DateTime.Now;
                qsf.AdminDiscount = request.GivenPercentage;
                qsf.CurrentStatus = "Admin Discount Given by " + request.GivenBy;
                context.Entry(qsf).CurrentValues.SetValues(qsf);
                context.SaveChanges();

                // update quote details
                int currentCrfQId = (int)qsf.QuoteMasterId;
                List<CRFQDetails> cRFQDetails = new List<CRFQDetails>();
                cRFQDetails = context.cRFQDetails.Where(A => A.IsDeleted == false && A.CRFQId == currentCrfQId).ToList();
                if (cRFQDetails.Count > 0)
                {
                    foreach (var v in cRFQDetails)
                    {
                        var tempC = context.cRFQDetails.Where(A => A.CRFQDetailsId == v.CRFQDetailsId).FirstOrDefault();
                        tempC.AdminDiscount = request.GivenPercentage;
                        tempC.TotalPrice = tempC.OrderLineTotal - (tempC.OrderLineTotal * ((tempC.DisAmtBySE + tempC.DisAmtByHead+tempC.AdminDiscount) / 100));
                        context.Entry(tempC).CurrentValues.SetValues(tempC);
                        context.SaveChanges();
                    }
                }

                // update quotemaster

                CRFQMaster cr = context.cRFQMasters.Where(A => A.CRFQId == currentCrfQId).FirstOrDefault();
                cr.CurrentStatus = "Discount Approved BY Admin";
                context.Entry(cr).CurrentValues.SetValues(cr);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            else
            {
                pr.statusCode = 0;
                pr.statusMessage = "Limit Exceded";
            }
            return pr;
        }

        public List<SaleOrderMasterModel> GetMySaleOrders(int staffId, int pageNumber = 1, int pageSize = 10)
        {
            List<SaleOrderMasterModel> result = new List<SaleOrderMasterModel>();
            try
            {
                int skipNumber = 0;
                if (pageNumber > 1)
                {
                    skipNumber = (pageNumber - 1) * pageSize;
                }
                result = (from c in context.salesOrderMasters
                          join u in context.userMasters on c.StaffId equals u.UserId
                          join uc in context.customerMasters on c.CustomerId equals uc.Cid
                          join ca in context.addressMasters on c.ShippingAddressId equals ca.Addid
                          join city in context.cityMasters on ca.CityId equals city.Id
                          join state in context.stateMasters on ca.StateId equals state.Id
                          join country in context.countryMasters on ca.CountryId equals country.Id
                          where c.StaffId == staffId && c.IsDeleted == false && c.OrderType == "Regular" &&c.CurrentStatus!= "Paid Full"
                          select new SaleOrderMasterModel
                          {
                              CurrentStatus = c.CurrentStatus,
                              CreatedBy = c.CreatedBy,
                              CreatedOn = c.CreatedOn,
                              CustomerId = c.CustomerId,
                              CustomerName = uc.FullName,
                              IsDeleted = c.IsDeleted,
                              QuoteId = c.QuoteId,
                              saleOrderDetails = null,
                              SalesExecutiveName = u.UserName,
                              SOMId = c.SOMId,
                              StaffId = c.StaffId,
                              ShippingAddressId = c.ShippingAddressId,
                              CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                              TaxAmount = c.TaxAmount,
                              TaxPercentage = c.TaxPercentage,
                              TaxApplicable = c.TaxApplicable,
                              DelivaryCharges = c.DelivaryCharges==null?0:c.DelivaryCharges,
                              SOStatus = c.SOStatus,
                              RoundedValue=c.RoundedValue==null?0:c.RoundedValue
                          }).OrderByDescending(b => b.SOMId).Skip(skipNumber).Take(pageSize).ToList();
                if (result != null)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        int cartid = result[i].SOMId;
                        List<SaleOderDetailModel> cm = (from cd in context.salesOrderDetails

                                                        where cd.IsDeleted == false && cd.SOMId == cartid
                                                        select new SaleOderDetailModel
                                                        {
                                                            SOMId = cd.SOMId,
                                                            Breadth = cd.Breadth,
                                                            ColorImage = cd.ColorImage,
                                                            ColorName = cd.ColorName,
                                                            DeliveryDate = cd.DeliveryDate,
                                                            DisAmtByHead = cd.DisAmtByHead,
                                                            DisAmtBySE = cd.DisAmtBySE,
                                                            Height = cd.Height,
                                                            IsDeleted = cd.IsDeleted,
                                                            ItemPrice = cd.ItemPrice,
                                                            LastMOdifiedBy = cd.LastMOdifiedBy,
                                                            LastModifiedOn = cd.LastModifiedOn,
                                                            OrderLineTotal = cd.OrderLineTotal,
                                                            Quantity = cd.Quantity,
                                                            ShippingAddressId = cd.ShippingAddressId,
                                                            SODId = cd.SODId,
                                                            TotalPrice = cd.TotalPrice,
                                                            Width = cd.Width,
                                                            InventoryId = cd.InventoryId,
                                                            InventoryImage = cd.InventoryImage,
                                                            InventoryTitle = cd.InventoryTitle,
                                                            AdminDiscount=cd.AdminDiscount

                                                        }).ToList();
                        result[i].saleOrderDetails = cm;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public int GatCountOfOpenSO(int staffId)
        {
            int r = 0;
            try
            {
                r = context.salesOrderMasters.Where(c=>c.StaffId == staffId && c.IsDeleted == false && c.OrderType == "Regular" && c.CurrentStatus != "Paid Full").Select(b => b.SOMId).Count();
            }
            catch (Exception e)
            {

            }
            return r;

        }
        public List<SaleOrderMasterModel> GetMyClosedOrders(int staffId, int pageNumber = 1, int pageSize = 10)
        {
            List<SaleOrderMasterModel> result = new List<SaleOrderMasterModel>();
            try
            {
                int skipNumber = 0;
                if (pageNumber > 1)
                {
                    skipNumber = (pageNumber - 1) * pageSize;
                }
                result = (from c in context.salesOrderMasters
                          join u in context.userMasters on c.StaffId equals u.UserId
                          join uc in context.customerMasters on c.CustomerId equals uc.Cid
                          join ca in context.addressMasters on c.ShippingAddressId equals ca.Addid
                          join city in context.cityMasters on ca.CityId equals city.Id
                          join state in context.stateMasters on ca.StateId equals state.Id
                          join country in context.countryMasters on ca.CountryId equals country.Id
                          where c.StaffId == staffId && c.IsDeleted == false && c.OrderType == "Regular" && c.CurrentStatus == "Paid Full"
                          select new SaleOrderMasterModel
                          {
                              CurrentStatus = c.CurrentStatus,
                              CreatedBy = c.CreatedBy,
                              CreatedOn = c.CreatedOn,
                              CustomerId = c.CustomerId,
                              CustomerName = uc.FullName,
                              IsDeleted = c.IsDeleted,
                              QuoteId = c.QuoteId,
                              saleOrderDetails = null,
                              SalesExecutiveName = u.UserName,
                              SOMId = c.SOMId,
                              StaffId = c.StaffId,
                              ShippingAddressId = c.ShippingAddressId,
                              CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                              TaxAmount = c.TaxAmount,
                              TaxPercentage = c.TaxPercentage,
                              TaxApplicable = c.TaxApplicable,
                              DelivaryCharges = c.DelivaryCharges,
                              SOStatus = c.SOStatus,
                              RoundedValue = c.RoundedValue

                          }).OrderByDescending(b => b.SOMId).Skip(skipNumber).Take(pageSize).ToList();
                if (result != null)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        int cartid = result[i].SOMId;
                        List<SaleOderDetailModel> cm = (from cd in context.salesOrderDetails

                                                        where cd.IsDeleted == false && cd.SOMId == cartid
                                                        select new SaleOderDetailModel
                                                        {
                                                            SOMId = cd.SOMId,
                                                            Breadth = cd.Breadth,
                                                            ColorImage = cd.ColorImage,
                                                            ColorName = cd.ColorName,
                                                            DeliveryDate = cd.DeliveryDate,
                                                            DisAmtByHead = cd.DisAmtByHead,
                                                            DisAmtBySE = cd.DisAmtBySE,
                                                            Height = cd.Height,
                                                            IsDeleted = cd.IsDeleted,
                                                            ItemPrice = cd.ItemPrice,
                                                            LastMOdifiedBy = cd.LastMOdifiedBy,
                                                            LastModifiedOn = cd.LastModifiedOn,
                                                            OrderLineTotal = cd.OrderLineTotal,
                                                            Quantity = cd.Quantity,
                                                            ShippingAddressId = cd.ShippingAddressId,
                                                            SODId = cd.SODId,
                                                            TotalPrice = cd.TotalPrice,
                                                            Width = cd.Width,
                                                            InventoryId = cd.InventoryId,
                                                            InventoryImage = cd.InventoryImage,
                                                            InventoryTitle = cd.InventoryTitle,
                                                            AdminDiscount = cd.AdminDiscount

                                                        }).ToList();
                        result[i].saleOrderDetails = cm;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public int GetCountOfOpenClosedOrders(int staffId)
        {
            int r = 0;
            try
            {
                r = context.salesOrderMasters.Where(c => c.StaffId == staffId && c.IsDeleted == false && c.OrderType == "Regular" && c.CurrentStatus == "Paid Full").Select(b => b.SOMId).Count();
            }
            catch (Exception e)
            {

            }
            return r;

        }
        public List<SaleOrderMasterModel> GetMySaleOrdersCustom(int staffId)
        {
            List<SaleOrderMasterModel> result = new List<SaleOrderMasterModel>();
            try
            {
                result = (from c in context.salesOrderMasters
                          join u in context.userMasters on c.StaffId equals u.UserId
                          join uc in context.customerMasters on c.CustomerId equals uc.Cid
                          join ca in context.addressMasters on c.ShippingAddressId equals ca.Addid
                          join city in context.cityMasters on ca.CityId equals city.Id
                          join state in context.stateMasters on ca.StateId equals state.Id
                          join country in context.countryMasters on ca.CountryId equals country.Id
                          where c.StaffId == staffId && c.IsDeleted == false && c.OrderType == "Custom"
                          select new SaleOrderMasterModel
                          {
                              CurrentStatus = c.CurrentStatus,
                              CreatedBy = c.CreatedBy,
                              CreatedOn = c.CreatedOn,
                              CustomerId = c.CustomerId,
                              CustomerName = uc.FullName,
                              IsDeleted = c.IsDeleted,
                              QuoteId = c.QuoteId,
                              saleOrderDetails = null,
                              SalesExecutiveName = u.UserName,
                              SOMId = c.SOMId,
                              StaffId = c.StaffId,
                              ShippingAddressId = c.ShippingAddressId,
                              CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                              TaxAmount = c.TaxAmount,
                              TaxPercentage = c.TaxPercentage,
                              TaxApplicable = c.TaxApplicable,
                              DelivaryCharges = c.DelivaryCharges,
                              SOStatus = c.SOStatus

                          }).ToList();
                if (result != null)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        int cartid = result[i].SOMId;
                        List<SaleOderDetailModel> cm = (from cd in context.salesOrderDetails

                                                        where cd.IsDeleted == false && cd.SOMId == cartid
                                                        select new SaleOderDetailModel
                                                        {
                                                            SOMId = cd.SOMId,
                                                            Breadth = cd.Breadth,
                                                            ColorImage = cd.ColorImage,
                                                            ColorName = cd.ColorName,
                                                            DeliveryDate = cd.DeliveryDate,
                                                            DisAmtByHead = cd.DisAmtByHead,
                                                            DisAmtBySE = cd.DisAmtBySE,
                                                            Height = cd.Height,
                                                            IsDeleted = cd.IsDeleted,
                                                            ItemPrice = cd.ItemPrice,
                                                            LastMOdifiedBy = cd.LastMOdifiedBy,
                                                            LastModifiedOn = cd.LastModifiedOn,
                                                            OrderLineTotal = cd.OrderLineTotal,
                                                            Quantity = cd.Quantity,
                                                            ShippingAddressId = cd.ShippingAddressId,
                                                            SODId = cd.SODId,
                                                            TotalPrice = cd.TotalPrice,
                                                            Width = cd.Width,
                                                            InventoryId = cd.InventoryId,
                                                            InventoryImage = cd.InventoryImage,
                                                            InventoryTitle = cd.InventoryTitle


                                                        }).ToList();
                        result[i].saleOrderDetails = cm;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<SaleOrderMasterModel> GetOpenSaleOrder(int staffId = 0, int pageNumber = 1, int pageSize = 10)
        {
            List<SaleOrderMasterModel> result = new List<SaleOrderMasterModel>();
            try
            {
                int skip = 0;
                if (pageNumber > 1)
                {
                    skip = (pageNumber - 1) * pageSize;
                }
                if (staffId == 0)
                {
                    result = (from c in context.salesOrderMasters
                              join u in context.userMasters on c.StaffId equals u.UserId
                              join uc in context.customerMasters on c.CustomerId equals uc.Cid
                              join ca in context.addressMasters on c.ShippingAddressId equals ca.Addid
                              join city in context.cityMasters on ca.CityId equals city.Id
                              join state in context.stateMasters on ca.StateId equals state.Id
                              join country in context.countryMasters on ca.CountryId equals country.Id
                              where c.IsDeleted == false && c.DeliveryStatus != "Delivered"
                              select new SaleOrderMasterModel
                              {
                                  CurrentStatus = c.CurrentStatus,
                                  CreatedBy = c.CreatedBy,
                                  CreatedOn = c.CreatedOn,
                                  CustomerId = c.CustomerId,
                                  CustomerName = uc.FullName,
                                  IsDeleted = c.IsDeleted,
                                  QuoteId = c.QuoteId,
                                  saleOrderDetails = null,
                                  SalesExecutiveName = u.UserName,
                                  SOMId = c.SOMId,
                                  StaffId = c.StaffId,
                                  ShippingAddressId = c.ShippingAddressId,
                                  CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                                  TaxAmount = c.TaxAmount,
                                  TaxPercentage = c.TaxPercentage,
                                  TaxApplicable = c.TaxApplicable,
                                  DelivaryCharges = c.DelivaryCharges==null?0:c.DelivaryCharges,
                                  SOStatus = c.SOStatus,
                                  RoundedValue=c.RoundedValue
                              }).Skip(skip).Take(pageSize).ToList();
                }
                else
                {
                    var staffIds = context.userMasters.Where(a => a.IsDeleted == false && a.ReportingManager == staffId).Select(b => b.UserId).ToList();
                    result = (from c in context.salesOrderMasters
                              join u in context.userMasters on c.StaffId equals u.UserId
                              join uc in context.customerMasters on c.CustomerId equals uc.Cid
                              join ca in context.addressMasters on c.ShippingAddressId equals ca.Addid
                              join city in context.cityMasters on ca.CityId equals city.Id
                              join state in context.stateMasters on ca.StateId equals state.Id
                              join country in context.countryMasters on ca.CountryId equals country.Id
                              where c.IsDeleted == false && staffIds.Contains(c.StaffId) && c.DeliveryStatus != "Delivered"
                              select new SaleOrderMasterModel
                              {
                                  CurrentStatus = c.CurrentStatus,
                                  CreatedBy = c.CreatedBy,
                                  CreatedOn = c.CreatedOn,
                                  CustomerId = c.CustomerId,
                                  CustomerName = uc.FullName,
                                  IsDeleted = c.IsDeleted,
                                  QuoteId = c.QuoteId,
                                  saleOrderDetails = null,
                                  SalesExecutiveName = u.UserName,
                                  SOMId = c.SOMId,
                                  StaffId = c.StaffId,
                                  ShippingAddressId = c.ShippingAddressId,
                                  CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                                  TaxAmount = c.TaxAmount,
                                  TaxPercentage = c.TaxPercentage,
                                  TaxApplicable = c.TaxApplicable,
                                  DelivaryCharges = c.DelivaryCharges,
                                  SOStatus = c.SOStatus,
                                  RoundedValue=c.RoundedValue

                              }).Skip(skip).Take(pageSize).ToList();
                }

                if (result != null)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        int cartid = result[i].SOMId;
                        List<SaleOderDetailModel> cm = (from cd in context.salesOrderDetails

                                                        where cd.IsDeleted == false && cd.SOMId == cartid
                                                        select new SaleOderDetailModel
                                                        {
                                                            SOMId = cd.SOMId,
                                                            Breadth = cd.Breadth,
                                                            ColorImage = cd.ColorImage,
                                                            ColorName = cd.ColorName,
                                                            DeliveryDate = cd.DeliveryDate,
                                                            DisAmtByHead = cd.DisAmtByHead+cd.AdminDiscount,
                                                            DisAmtBySE = cd.DisAmtBySE,
                                                            Height = cd.Height,
                                                            IsDeleted = cd.IsDeleted,
                                                            ItemPrice = cd.ItemPrice,
                                                            LastMOdifiedBy = cd.LastMOdifiedBy,
                                                            LastModifiedOn = cd.LastModifiedOn,
                                                            OrderLineTotal = cd.OrderLineTotal,
                                                            Quantity = cd.Quantity,
                                                            ShippingAddressId = cd.ShippingAddressId,
                                                            SODId = cd.SODId,
                                                            TotalPrice = cd.TotalPrice,
                                                            Width = cd.Width,
                                                            InventoryId = cd.InventoryId,
                                                            InventoryImage = cd.InventoryImage,
                                                            InventoryTitle = cd.InventoryTitle,
                                                            CurrentShowroomQty = context.inventoryMasters.Where(a => a.InventoryId == cd.InventoryId).Select(a => a.ShowroomQty).FirstOrDefault(),
                                                            CurrentWarehouseQty = context.inventoryMasters.Where(a => a.InventoryId == cd.InventoryId).Select(a => a.WharehouseQty).FirstOrDefault(),
                                                            TaxAmount=cd.TaxAmount,
                                                            TaxPercentage=cd.TaxPercentage,
                                                            TaxDeducted=cd.TaxDeducted
                                                        }).ToList();
                        result[i].saleOrderDetails = cm;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<SaleOrderMasterModel> GetOpenSaleOrderForDc(int staffId = 0)
        {
            List<SaleOrderMasterModel> result = new List<SaleOrderMasterModel>();
            try
            {                
                if (staffId == 0)
                {
                    result = (from c in context.salesOrderMasters
                              join u in context.userMasters on c.StaffId equals u.UserId
                              join uc in context.customerMasters on c.CustomerId equals uc.Cid
                              join ca in context.addressMasters on c.ShippingAddressId equals ca.Addid
                              join city in context.cityMasters on ca.CityId equals city.Id
                              join state in context.stateMasters on ca.StateId equals state.Id
                              join country in context.countryMasters on ca.CountryId equals country.Id
                              where c.IsDeleted == false
                              select new SaleOrderMasterModel
                              {
                                  CurrentStatus = c.CurrentStatus,
                                  CreatedBy = c.CreatedBy,
                                  CreatedOn = c.CreatedOn,
                                  CustomerId = c.CustomerId,
                                  CustomerName = uc.FullName,
                                  IsDeleted = c.IsDeleted,
                                  QuoteId = c.QuoteId,
                                  saleOrderDetails = null,
                                  SalesExecutiveName = u.UserName,
                                  SOMId = c.SOMId,
                                  StaffId = c.StaffId,
                                  ShippingAddressId = c.ShippingAddressId,
                                  CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                                  TaxAmount = c.TaxAmount,
                                  TaxPercentage = c.TaxPercentage,
                                  TaxApplicable = c.TaxApplicable,
                                  DelivaryCharges = c.DelivaryCharges == null ? 0 : c.DelivaryCharges,
                                  SOStatus = c.SOStatus,
                                  RoundedValue = c.RoundedValue
                              }).ToList();
                }
                else
                {
                    var staffIds = context.userMasters.Where(a => a.IsDeleted == false && a.ReportingManager == staffId).Select(b => b.UserId).ToList();
                    result = (from c in context.salesOrderMasters
                              join u in context.userMasters on c.StaffId equals u.UserId
                              join uc in context.customerMasters on c.CustomerId equals uc.Cid
                              join ca in context.addressMasters on c.ShippingAddressId equals ca.Addid
                              join city in context.cityMasters on ca.CityId equals city.Id
                              join state in context.stateMasters on ca.StateId equals state.Id
                              join country in context.countryMasters on ca.CountryId equals country.Id
                              where c.IsDeleted == false && staffIds.Contains(c.StaffId)
                              select new SaleOrderMasterModel
                              {
                                  CurrentStatus = c.CurrentStatus,
                                  CreatedBy = c.CreatedBy,
                                  CreatedOn = c.CreatedOn,
                                  CustomerId = c.CustomerId,
                                  CustomerName = uc.FullName,
                                  IsDeleted = c.IsDeleted,
                                  QuoteId = c.QuoteId,
                                  saleOrderDetails = null,
                                  SalesExecutiveName = u.UserName,
                                  SOMId = c.SOMId,
                                  StaffId = c.StaffId,
                                  ShippingAddressId = c.ShippingAddressId,
                                  CustomerAddress = ca.Location + " " + ca.HouseNumber + " " + ca.StreetName + " " + city.CityName + " " + state.StateName + " " + country.CountryName + " " + ca.PostalCode,
                                  TaxAmount = c.TaxAmount,
                                  TaxPercentage = c.TaxPercentage,
                                  TaxApplicable = c.TaxApplicable,
                                  DelivaryCharges = c.DelivaryCharges,
                                  SOStatus = c.SOStatus,
                                  RoundedValue = c.RoundedValue

                              }).ToList();
                }

                if (result != null)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        int cartid = result[i].SOMId;
                        List<SaleOderDetailModel> cm = (from cd in context.salesOrderDetails

                                                        where cd.IsDeleted == false && cd.SOMId == cartid
                                                        select new SaleOderDetailModel
                                                        {
                                                            SOMId = cd.SOMId,
                                                            Breadth = cd.Breadth,
                                                            ColorImage = cd.ColorImage,
                                                            ColorName = cd.ColorName,
                                                            DeliveryDate = cd.DeliveryDate,
                                                            DisAmtByHead = cd.DisAmtByHead,
                                                            DisAmtBySE = cd.DisAmtBySE,
                                                            Height = cd.Height,
                                                            IsDeleted = cd.IsDeleted,
                                                            ItemPrice = cd.ItemPrice,
                                                            LastMOdifiedBy = cd.LastMOdifiedBy,
                                                            LastModifiedOn = cd.LastModifiedOn,
                                                            OrderLineTotal = cd.OrderLineTotal,
                                                            Quantity = cd.Quantity,
                                                            ShippingAddressId = cd.ShippingAddressId,
                                                            SODId = cd.SODId,
                                                            TotalPrice = cd.TotalPrice,
                                                            Width = cd.Width,
                                                            InventoryId = cd.InventoryId,
                                                            InventoryImage = cd.InventoryImage,
                                                            InventoryTitle = cd.InventoryTitle,
                                                            CurrentShowroomQty = context.inventoryMasters.Where(a => a.InventoryId == cd.InventoryId).Select(a => a.ShowroomQty).FirstOrDefault(),
                                                            CurrentWarehouseQty = context.inventoryMasters.Where(a => a.InventoryId == cd.InventoryId).Select(a => a.WharehouseQty).FirstOrDefault(),
                                                            TaxAmount = cd.TaxAmount,
                                                            TaxPercentage = cd.TaxPercentage,
                                                            TaxDeducted = cd.TaxDeducted
                                                        }).ToList();
                        result[i].saleOrderDetails = cm;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public int GetOpenSaleOrderCount(int staffId = 0)
        {
            int result = 0;
            try
            {
                if (staffId == 0)
                {
                    result = (from c in context.salesOrderMasters                              
                              where c.IsDeleted == false && c.DeliveryStatus != "Delivered"
                              select new SaleOrderMasterModel
                              {
                                  SOMId = c.SOMId,
                              }).Count();
                }
                else
                {
                    var staffIds = context.userMasters.Where(a => a.IsDeleted == false && a.ReportingManager == staffId).Select(b => b.UserId).ToList();
                    result = (from c in context.salesOrderMasters                              
                              where c.IsDeleted == false && staffIds.Contains(c.StaffId) && c.DeliveryStatus != "Delivered"
                              select new SaleOrderMasterModel
                              {
                                  SOMId = c.SOMId,
                              }).Count();
                }
            }catch(Exception e)
            {

            }
            return result;
        }

        public ProcessResponse GenerateDC(CreateDCModel request, int currentUserId)
        {
            ProcessResponse res = new ProcessResponse();
            try
            {
                SaleOrderMasterModel saleOrder = new SaleOrderMasterModel();
                saleOrder = GetOpenSaleOrderForDc(0).Where(a => a.SOMId == request.SOMId).FirstOrDefault();
                if (saleOrder != null)
                {
                   // bool isSingleInvoice = true;
                    //bool fromShowroom = false;
                    //bool fromWarehouse = false;
                    //List<DoubleBoolClass> abc = new List<DoubleBoolClass>();

                    //int x = 0;
                    //foreach(var v in saleOrder.saleOrderDetails)
                    //{
                    //    InventoryMaster im = context.inventoryMasters.Where(a => a.IsDeleted == false && a.InventoryId == v.InventoryId).FirstOrDefault();
                    //    bool sh = false;
                    //    bool wh = false;
                    //    if (request.SelShowRoomQty[x] > 0 && im.ShowroomQty>=request.SelShowRoomQty[x])
                    //    {
                    //        sh = true;
                    //    }
                    //    if(request.SelWareHouseQty[x]> 0 && im.WharehouseQty >= request.SelWareHouseQty[x])
                    //    {
                    //        wh = true;
                    //    }
                    //    if((request.SelWareHouseQty[x]+ request.SelShowRoomQty[x]) != v.Quantity)
                    //    {
                    //        sh = false;
                    //        wh = false;
                    //    }
                    //    if(sh==false && wh == false)
                    //    {
                    //        res.statusCode = 0;
                    //        res.statusMessage = "Please Select Correct Quantity for all Products";
                    //        return res;
                    //    }
                    //    x++;
                    //}
                    //foreach (var v in request.SelShowRoomQty)
                    //{
                    //    DoubleBoolClass xyz = new DoubleBoolClass();
                    //    xyz.shQtySelected = false;
                    //    xyz.whQtySelected = false;
                    //    if (v > 0)
                    //    {
                    //        fromShowroom = true;
                    //        xyz.shQtySelected = true;
                    //    }
                    //    abc.Add(xyz);
                    //}

                    //foreach (var v in request.SelWareHouseQty)
                    //{
                    //    if (v > 0)
                    //    {
                    //        fromWarehouse = true;
                    //        abc[x].whQtySelected = true;
                    //    }
                    //    if(abc[x].shQtySelected==false && abc[x].whQtySelected == false)
                    //    {
                    //        res.statusCode = 0;
                    //        res.statusMessage = "Please Select Quantityfor all Products";
                    //        return res;
                    //    }
                    //}
                    
                    //if (fromShowroom && fromWarehouse)
                    //{
                    //    isSingleInvoice = false;
                    //}
                   
                        //TaxInvoiceMaster tim = new TaxInvoiceMaster();
                        //tim = context.taxInvoiceMasters.Where(a => a.SOMId == request.SOMId).FirstOrDefault();
                        //if (tim == null)
                        //{
                        //    tim = new TaxInvoiceMaster();
                        //    tim.SOMId = request.SOMId;
                        //    tim.CurrentStatus = "Open";
                        //    tim.TotalValue = saleOrder.saleOrderDetails.Sum(a => a.TotalPrice);
                        //    tim.CreatedBy = context.userMasters.Where(a => a.UserId == currentUserId).Select(b => b.UserName).FirstOrDefault();
                        //    tim.CreatedOn = DateTime.Now;
                        //    tim.DespatchDocumnetNo = request.SOMId + "/" + DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
                        //    tim.DespatchThrough = "NA";
                        //    tim.EWayBillNo = "NA";
                        //    tim.EWayBillNumber = "NA";

                        //    CompanyMaster cmfix = context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
                        //    if (cmfix != null)
                        //    {
                        //        tim.FromAddress = cmfix.CompanyAddress;
                        //        tim.FromEmail = cmfix.ShowroomEmail;
                        //        tim.FromMobile = cmfix.ShowroomPhoneNumbers;
                        //    }                           
                        //    tim.InvoiceDate = DateTime.Now;
                        //    tim.InvoiceNumber = (context.taxInvoiceMasters.Where(a => a.IsDeleted == false).Count() + 1).ToString() + "/" + DateTime.Now.Year.ToString() + (DateTime.Now.Year + 1).ToString();
                        //    tim.IsDeleted = false;
                        //    tim.Notes = request.DCRemarks;
                        //    tim.OtherRemarks = "";
                        //    tim.PaymentStatus = saleOrder.CurrentStatus;
                        //    var customerDetails = context.customerMasters.Where(a => a.Cid == saleOrder.CustomerId).FirstOrDefault();
                        //    tim.CustomerName = customerDetails.FullName;
                        //    var address = context.addressMasters.Where(a => a.CustomerId == saleOrder.CustomerId ).FirstOrDefault();
                        //    if(address == null)
                        //    {
                        //        res.statusCode = 0;
                        //        res.statusMessage = "Customer Billing Address not provided";
                        //        return res;
                        //    }
                        //    string BillingAddress = address.HouseNumber + ", " + address.StreetName + ", " + address.Location + ", <BR/>";
                        //    BillingAddress += context.cityMasters.Where(a => a.Id == address.CityId).Select(a => a.CityName).FirstOrDefault();
                        //    BillingAddress += "," + context.stateMasters.Where(a => a.Id == address.StateId).Select(a => a.StateName).FirstOrDefault();
                        //    BillingAddress += "," + context.countryMasters.Where(a => a.Id == address.CountryId).Select(a => a.CountryName).FirstOrDefault();
                        //    BillingAddress += " - " + address.PostalCode;
                        //    tim.ToAddress = BillingAddress;
                        //    tim.ToEmail = customerDetails.EmailId;
                        //    tim.ToMobile = customerDetails.MobileNumber;
                        //    tim.RoundedValue = saleOrder.RoundedValue;
                        //    context.taxInvoiceMasters.Add(tim);
                        //    context.SaveChanges();

                            // update sale order
                            
                    SalesOrderMaster som = context.salesOrderMasters.Where(a => a.SOMId == request.SOMId).FirstOrDefault();
                    som.SOStatus = "Invoice Created";
                    som.InvoiceCreatedDate = DateTime.Now;
                    som.DeliveryStatus = "Ready to Dispatch";
                    som.Notes = request.DCRemarks;
                    som.DespatchDocumnetNo = request.SOMId + "/" + DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString();
                    som.DespatchThrough = "NA";
                    context.Entry(som).CurrentValues.SetValues(som);
                    context.SaveChanges();
                    List<SalesOrderDetails> sods = context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == som.SOMId).ToList();
                    foreach(SalesOrderDetails s in sods)
                    {
                        s.DeliveryStatus = "Ready to Dispatch";
                        context.Entry(s).CurrentValues.SetValues(s);
                        context.SaveChanges();
                    }
                        //}

                        //foreach (var v in saleOrder.saleOrderDetails)
                        //{
                        //    InvoiceItemDetails itd = new InvoiceItemDetails();
                        //    itd.Breadth = v.Breadth;
                        //    itd.ColorImage = v.ColorImage;
                        //    itd.ColorName = v.ColorName;
                        //    itd.CurrentStatus = "NA";
                        //    itd.DeliveryDate = DateTime.Now;
                        //    itd.DisAmtByHead = v.DisAmtByHead;
                        //    itd.DisAmtBySE = v.DisAmtBySE;
                        //    itd.Height = v.Height;
                        //    itd.InventoryId = v.InventoryId;
                        //    itd.InventoryImage = v.InventoryImage;
                        //    itd.InventoryTitle = v.InventoryTitle;
                        //    itd.InvoiceId = tim.InvoiceId;
                        //    itd.IsDeleted = false;
                        //    itd.ItemPrice = v.ItemPrice;
                        //    itd.LastMOdifiedBy = context.userMasters.Where(a => a.UserId == currentUserId).Select(b => b.UserName).FirstOrDefault();
                        //    itd.LastModifiedOn = DateTime.Now;
                        //    itd.OrderLineTotal = v.OrderLineTotal;
                        //    itd.Quantity = v.Quantity;
                        //    itd.ShippingAddressId = 0;
                        //    itd.ShippingFrom = "Company" ;
                        //    itd.TotalPrice = v.TotalPrice;
                        //    itd.Width = v.Width;
                        //    itd.TaxDeducted = v.TaxDeducted;
                        //    itd.TaxPercentage = v.TaxPercentage;
                        //    itd.TaxAmount = v.TaxAmount;
                        //    context.invoiceItemDetails.Add(itd);
                        //    context.SaveChanges();                            
                        //}


                        //// update inventory 
                        ////for (int i = 0; i < request.SODId.Count(); i++)
                        ////{
                        ////    int cInvId = (int)saleOrder.saleOrderDetails.Where(a => a.SODId == request.SODId[0]).Select(b => b.InventoryId).FirstOrDefault();
                        ////    InventoryMaster iM = context.inventoryMasters.Where(a => a.InventoryId == cInvId).FirstOrDefault();
                        ////    if (request.SelWareHouseQty[i] > 0)
                        ////    {
                        ////        iM.WharehouseQty -= request.SelWareHouseQty[i];
                        ////        iM.Qty -= request.SelWareHouseQty[i];
                        ////    }
                        ////    if (request.SelShowRoomQty[i] > 0)
                        ////    {
                        ////        iM.ShowroomQty -= request.SelShowRoomQty[i];
                        ////        iM.Qty -= request.SelShowRoomQty[i];
                        ////    }
                        ////    context.Entry(iM).CurrentValues.SetValues(iM);
                        ////    context.SaveChanges();
                        ////}
                        //// create dc 
                        //var cd = context.customerMasters.Where(a => a.Cid == saleOrder.CustomerId).FirstOrDefault();
                        //DCMaster dcM = new DCMaster();
                        //dcM.CreatedBy = currentUserId;
                        //dcM.CreatedOn = DateTime.Now;
                        //dcM.DCDate = DateTime.Now;
                        //dcM.DespatchedBy = null;
                        //dcM.DespatchedOn = null;
                        //dcM.EwayBillDate = null;
                        //dcM.EwayBillDetails = null;
                        //dcM.CurrentStatus = "Ready to Dispatch";
                        //dcM.InvoiceId = tim.InvoiceId;
                        //dcM.IsDeleted = false;
                        //dcM.ToAddress = tim.ToAddress;
                        //dcM.CustomerId = cd.Cid;
                        //dcM.CustomerName = cd.FullName;
                        //dcM.CustGstNumber = cd.GSTNumber;
                        //context.dCMasters.Add(dcM);
                        //context.SaveChanges();
                        //res.statusCode = 1;
                        //res.statusMessage = "Success";
                        //res.currentId = dcM.DCId;

                    List<ReservedQtyMaster> rqs = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.SOMId == request.SOMId).ToList();
                    foreach(ReservedQtyMaster r in rqs)
                    {
                        //r.DCmId = dcM.DCId;
                        r.CurrentStatus = "Ready to Dispatch";
                        ReservedQtyMaster rq = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.TRId == r.TRId).FirstOrDefault();
                        context.Entry(rq).CurrentValues.SetValues(r);
                        context.SaveChanges();
                    }
                        //int i = 0;
                        //foreach (var v in saleOrder.saleOrderDetails)
                        //{
                        //    ReservedQtyMaster rqm = new ReservedQtyMaster();
                        //    rqm.ProductId = v.InventoryId;
                        //    rqm.Quantity = (int)v.Quantity;
                        //    rqm.IsDeleted = false;
                        //    rqm.CurrentStatus = "Ready to Dispatch";
                        //    rqm.DCmId = dcM.DCId;
                        //    rqm.SalesExename = saleOrder.CreatedBy;
                        //    context.reservedQtyMasters.Add(rqm);
                        //    context.SaveChanges();
                        //    InventoryMaster im = context.inventoryMasters.Where(a => a.IsDeleted == false && a.InventoryId == v.InventoryId).FirstOrDefault();
                        //    im.Qty -= (int)v.Quantity;
                            
                        //    if (request.SelShowRoomQty[i] > 0)
                        //    {
                        //        im.ShowroomQty -= request.SelShowRoomQty[i];
                        //    }
                        //    if (request.SelWareHouseQty[i] > 0)
                        //    {
                        //        im.WharehouseQty -= request.SelWareHouseQty[i];
                        //    }
                        //    InventoryMaster oldim = context.inventoryMasters.Where(a => a.IsDeleted == false && a.InventoryId == v.InventoryId).FirstOrDefault();

                        //    context.Entry(oldim).CurrentValues.SetValues(im);
                        //    context.SaveChanges();
                        //    i++;
                        //}
                }
            }
            catch (Exception ex)
            {
                res.statusCode = 0;
                res.statusMessage = "failed";
            }
            return res;
        }
        public ProcessResponse SaveReservedQtyMaster(ReservedQtyMaster request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                if (request.TRId > 0)
                {
                    ReservedQtyMaster rqm = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.TRId == request.TRId).FirstOrDefault();
                    context.Entry(rqm).CurrentValues.SetValues(request);
                    context.SaveChanges();
                    pr.statusCode = 1;
                    pr.currentId = request.TRId;
                    pr.statusMessage = "Updated";                    
                }
                else
                {
                    context.reservedQtyMasters.Add(request);
                    context.SaveChanges();
                    pr.statusCode = 1;
                    pr.currentId = request.TRId;
                    pr.statusMessage = "Added";
                }
            }catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Failed";
            }
            return pr;
        }

        public List<DCMasterModel> GetAllDC(string src)
        {
            List<DCMasterModel> myList = new List<DCMasterModel>();
            try
            {
                //if (src == "All")
                //{
                //    myList = (from dc in context.dCMasters
                //              join u in context.userMasters on dc.DespatchedBy equals u.UserId into tempDC
                //              from ud in tempDC.DefaultIfEmpty()
                //              where dc.IsDeleted == false && dc.CurrentStatus == "Ready to Dispatch"
                //              select new DCMasterModel
                //              {
                //                  CreatedBy = dc.CreatedBy,
                //                  CreatedOn = dc.CreatedOn,
                //                  DCDate = dc.DCDate,
                //                  DCId = dc.DCId,
                //                  DespatchedBy = dc.DespatchedBy,
                //                  DespatchedOn = dc.DespatchedOn,
                //                  EwayBillDate = dc.EwayBillDate,
                //                  EwayBillDetails = dc.EwayBillDetails,
                //                  InvoiceId = dc.InvoiceId,
                //                  CustomerName = dc.CustomerName,
                //                  ToAddress = dc.ToAddress,
                //                  DespatchedByName = ud.UserName,
                //                  CurrentStatus = dc.CurrentStatus,
                //                  CustGstNumber=dc.CustGstNumber
                //              }).ToList();
                //}
                //if (src == "Despatched")
                //{
                    myList = (from dc in context.dCMasters
                              join u in context.userMasters on dc.DespatchedBy equals u.UserId into tempDC
                              from ud in tempDC.DefaultIfEmpty()
                              where dc.IsDeleted == false && dc.CurrentStatus == "Despatched"
                              select new DCMasterModel
                              {
                                  CreatedBy = dc.CreatedBy,
                                  CreatedOn = dc.CreatedOn,
                                  DCDate = dc.DCDate,
                                  DCId = dc.DCId,
                                  DespatchedBy = dc.DespatchedBy,
                                  DespatchedOn = dc.DespatchedOn,
                                  EwayBillDate = dc.EwayBillDate,
                                  EwayBillDetails = dc.EwayBillDetails,
                                  InvoiceId = dc.InvoiceId,
                                  CustomerName = dc.CustomerName,
                                  ToAddress = dc.ToAddress,
                                  DespatchedByName = ud.UserName,
                                  CurrentStatus = dc.CurrentStatus

                              }).OrderByDescending(b=>b.DCId).ToList();
                //}
            }
            catch(Exception ex)
            {

            }
            return myList;
        }
        public List<SODModelForDC> GetItemsOfINvoiceMaster(int id)
        {
            List<SODModelForDC> ItemDetails = new List<SODModelForDC>();
            try
            {
                //SalesOrderMaster som = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.SOMId == id).FirstOrDefault();
                //List<SalesOrderDetails>
                //DCMaster dcm = context.dCMasters.Where(a => a.IsDeleted == false && a.InvoiceId == id).FirstOrDefault();
                ItemDetails = (from d in context.salesOrderDetails
                               where d.SOMId == id && d.DeliveryStatus != "Delivered"
                               select new SODModelForDC
                               {
                                   SODId = d.SODId,
                                   InventoryId = d.InventoryId,
                                   InventoryTitle = d.InventoryTitle,
                                   InventoryImage = d.InventoryImage,
                                   Quantity = d.Quantity
                               }).OrderByDescending(b => b.SODId).ToList();
            }
            catch(Exception e)
            {

            }
            return ItemDetails;
        }
        public TaxInvoiceMasterModel GetTaxInvoice(int invoiceid)
        {
            TaxInvoiceMasterModel result = new TaxInvoiceMasterModel();
            try
            {
                CompanyMaster comany = context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
                result = (from t in context.dCMasters
                          join s in context.salesOrderMasters on t.SOMId equals s.SOMId
                          join cus in context.customerMasters on t.CustomerId equals cus.Cid
                          where t.IsDeleted == false && t.DCId == invoiceid
                          select new TaxInvoiceMasterModel
                          {
                              FromEmail = comany.ShowroomEmail,
                              //InvoiceId = t.InvoiceId,
                              InvoiceDate = t.CreatedOn,
                              //CreatedBy = context.userMasters.Where(a=>a.IsDeleted==false && a.UserId==t.CreatedBy).Select(b=>b.UserName).FirstOrDefault(),
                              CreatedOn = (DateTime)t.CreatedOn,
                              CurrentStatus = t.CurrentStatus,
                              DespatchDocumnetNo = s.DespatchDocumnetNo,
                              DespatchThrough = s.DespatchThrough,
                              EWayBillNo = t.EwayBillDetails,
                              EWayBillNumber = t.EwayBillDetails,
                              FromAddress = comany.CompanyAddress,
                              FromMobile = comany.ShowroomPhoneNumbers,
                              InvoiceNumber = t.DCId.ToString(),
                              IsDeleted = t.IsDeleted,
                              Notes = s.Notes,
                              //OtherRemarks = t.OtherRemarks,
                              ToAddress = t.ToAddress,
                              //PaymentStatus = t.PaymentStatus,
                              //SeqId = t.SeqId,
                              //SMRId = t.SMRId,
                              SOMId = t.SOMId,
                              ToEmail = cus.EmailId,
                              ToMobile = cus.MobileNumber,
                              TotalValue = context.deliveryDetails.Where(a=>a.IsDeleted==false && a.DeliverMasterId==t.DCId).Sum(b=>b.TotalPrice),
                              CustomerName = t.CustomerName,
                              ItemDetails = context.deliveryDetails.Where(a=>a.DeliverMasterId == t.DCId).ToList(),
                              CustGstNumber=t.CustGstNumber,
                              RoundedValue=s.RoundedValue,
                              DelivaryCharges=s.DelivaryCharges
                          }).FirstOrDefault(); 
            }
            catch(Exception ex)
            {

            }

            return result;
        }

        public void UpdateEWayBill(string ewaybill, int dcmid)
        {
            try
            {
                DCMaster dm = new DCMaster();
                dm = context.dCMasters.Where(a => a.DCId == dcmid).FirstOrDefault();
                dm.EwayBillDetails = ewaybill;
                context.Entry(dm).CurrentValues.SetValues(dm);
                context.SaveChanges();
                TaxInvoiceMaster ti = new TaxInvoiceMaster();
                ti = context.taxInvoiceMasters.Where(a => a.InvoiceId == dm.InvoiceId).FirstOrDefault();
                ti.EWayBillNo = ewaybill;
                context.Entry(ti).CurrentValues.SetValues(ti);
                context.SaveChanges();
            }catch(Exception ex)
            {

            }
        }
        public void UpdateCustGstNumber(string gNo, int dcmid)
        {
            try
            {
                DCMaster dm = new DCMaster();
                dm = context.dCMasters.Where(a => a.DCId == dcmid).FirstOrDefault();
                dm.CustGstNumber = gNo;
                context.Entry(dm).CurrentValues.SetValues(dm);
                context.SaveChanges();
                TaxInvoiceMaster ti = new TaxInvoiceMaster();
                ti = context.taxInvoiceMasters.Where(a => a.InvoiceId == dm.InvoiceId).FirstOrDefault();
                ti.CustGstNumber = gNo;
                context.Entry(ti).CurrentValues.SetValues(ti);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateDispatched(DateTime dDate, int dBy,int dcmid)
        {
            try
            {
                DCMaster dm = new DCMaster();
                dm = context.dCMasters.Where(a => a.DCId == dcmid).FirstOrDefault();
                dm.DespatchedBy = dBy;
                dm.DespatchedOn = dDate;
                dm.CurrentStatus = "Despatched";
                context.Entry(dm).CurrentValues.SetValues(dm);
                context.SaveChanges();

                List<ReservedQtyMaster> rqm = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.DCmId == dcmid).ToList();
                foreach(ReservedQtyMaster r in rqm)
                {
                    r.CurrentStatus = "Despatched";
                    ReservedQtyMaster o = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.TRId == r.TRId).FirstOrDefault();
                    context.Entry(o).CurrentValues.SetValues(r);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public DCMaster GetDCById(int id)
        {
            return context.dCMasters.Where(a => a.DCId == id).FirstOrDefault();
        }
        public decimal GetQuoteExtraDisc(int crfrId)
        {
            decimal res = 0;

            try
            {
                res = (decimal)context.cRFQDetails.Where(a => a.IsDeleted == false && a.CRFQId == crfrId).Select(b => b.DisAmtByHead).FirstOrDefault();
            }
            catch { }

            return res;
        }
        public AddressMaster GetFirstAddressOfCustomer(int id)
        {
            AddressMaster res = new AddressMaster();
            try
            {
                res = context.addressMasters.Where(a => a.IsDeleted == false && a.CustomerId == id).FirstOrDefault();
            }catch(Exception e)
            {

            }
            return res;
        }
        public ProcessResponse CreatePartialDC(ModelForRDToDel res)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                // create dc 
                SalesOrderMaster som = context.salesOrderMasters.Where(a => a.IsDeleted == false && a.SOMId == res.DCId).FirstOrDefault();
                var cust = context.customerMasters.Where(a => a.Cid == som.CustomerId).FirstOrDefault();
                AddressMaster adr = context.addressMasters.Where(a => a.IsDeleted == false && a.CustomerId == cust.Cid).FirstOrDefault();
                CityMaster c = context.cityMasters.Where(a => a.IsDeleted == false && a.Id == adr.CityId).FirstOrDefault();
                StateMaster st = context.stateMasters.Where(a => a.IsDeleted == false && a.Id == adr.StateId).FirstOrDefault();
                CountryMaster con = context.countryMasters.Where(a => a.IsDeleted == false && a.Id == adr.CountryId).FirstOrDefault();
                DCMaster dcM = new DCMaster();
                dcM.CreatedBy = res.LogedUser;
                dcM.CreatedOn = DateTime.Now;
                dcM.DCDate = res.DespatchedOn;
                dcM.DespatchedBy = res.LogedUser;
                dcM.DespatchedOn = res.DespatchedOn;
                dcM.EwayBillDate = null;
                dcM.EwayBillDetails = res.EwayBillDetails;
                dcM.CurrentStatus = "Despatched";
                dcM.InvoiceId = 0;
                dcM.IsDeleted = false;
                dcM.ToAddress = adr.HouseNumber + ", " + adr.StreetName + ", " + adr.Location + ", <BR/>" + c.CityName + "," + st.StateName + "," + con.CountryName;
                dcM.CustomerId = cust.Cid;
                dcM.CustomerName = cust.FullName;
                dcM.CustGstNumber =res.CustGstNumber;
                dcM.SOMId = som.SOMId;
                context.dCMasters.Add(dcM);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
                pr.currentId = dcM.DCId;
                //List<SalesOrderDetails> sods = context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == res.DCId && a.DeliveryStatus != "Delivered").OrderByDescending(b=>b.SODId).ToList();
                int x = 0;
                int y = 0;
                //SaleOrderMasterModel saleOrder = new SaleOrderMasterModel();
                //saleOrder = GetOpenSaleOrderForDc(0).Where(a => a.SOMId == res.DCId).FirstOrDefault();
                List<SaleOderDetailModel> v = (from cd in context.salesOrderDetails
                                                where cd.IsDeleted == false && cd.SOMId == res.DCId && cd.DeliveryStatus != "Delivered"
                                                select new SaleOderDetailModel
                                                {
                                                    SOMId = cd.SOMId,                                                    
                                                    Breadth = cd.Breadth,
                                                    ColorImage = cd.ColorImage,
                                                    ColorName = cd.ColorName,
                                                    DeliveryDate = cd.DeliveryDate,
                                                    DisAmtByHead = cd.DisAmtByHead,
                                                    DisAmtBySE = cd.DisAmtBySE,
                                                    Height = cd.Height,
                                                    IsDeleted = cd.IsDeleted,
                                                    ItemPrice = cd.ItemPrice,
                                                    LastMOdifiedBy = cd.LastMOdifiedBy,
                                                    LastModifiedOn = cd.LastModifiedOn,
                                                    OrderLineTotal = cd.OrderLineTotal,
                                                    Quantity = cd.Quantity,
                                                    ShippingAddressId = cd.ShippingAddressId,
                                                    SODId = cd.SODId,
                                                    TotalPrice = cd.TotalPrice,
                                                    Width = cd.Width,
                                                    InventoryId = cd.InventoryId,
                                                    InventoryImage = cd.InventoryImage,
                                                    InventoryTitle = cd.InventoryTitle,
                                                    CurrentShowroomQty = context.inventoryMasters.Where(a => a.InventoryId == cd.InventoryId).Select(a => a.ShowroomQty).FirstOrDefault(),
                                                    CurrentWarehouseQty = context.inventoryMasters.Where(a => a.InventoryId == cd.InventoryId).Select(a => a.WharehouseQty).FirstOrDefault(),
                                                    TaxAmount = cd.TaxAmount,
                                                    TaxPercentage = cd.TaxPercentage,
                                                    TaxDeducted = cd.TaxDeducted
                                                }).OrderByDescending(b=>b.SODId).ToList();
                //var v = saleOrder.saleOrderDetails;
                int[] prodcts = new int[v.Count()];
                foreach (var d in v)
                {
                    if (res.YesOrNo[x])
                    {
                        DeliveryDetails dd = new DeliveryDetails();
                        dd.Breadth = d.Breadth;
                        dd.ColorImage = d.ColorImage;
                        dd.ColorName = d.ColorName;
                        dd.CurrentStatus = "NA";
                        dd.DeliveryDate = DateTime.Now;
                        dd.DisAmtByHead = d.DisAmtByHead;
                        dd.DisAmtBySE = d.DisAmtBySE;
                        dd.Height = d.Height;
                        dd.InventoryId = d.InventoryId;
                        dd.InventoryImage = d.InventoryImage;
                        dd.InventoryTitle = d.InventoryTitle;
                        dd.DeliverMasterId = dcM.DCId;
                        dd.IsDeleted = false;
                        dd.ItemPrice = d.ItemPrice;
                        dd.LastMOdifiedBy = context.userMasters.Where(a => a.UserId == res.LogedUser).Select(b => b.UserName).FirstOrDefault();
                        dd.LastModifiedOn = DateTime.Now;
                        dd.OrderLineTotal = d.OrderLineTotal;
                        dd.Quantity = d.Quantity;
                        dd.ShippingAddressId = 0;
                        dd.ShippingFrom = "Company";
                        dd.TotalPrice = d.TotalPrice;
                        dd.Width = d.Width;
                        dd.TaxDeducted = d.TaxDeducted;
                        dd.TaxPercentage = d.TaxPercentage;
                        dd.TaxAmount = d.TaxAmount;
                        context.deliveryDetails.Add(dd);
                        context.SaveChanges();

                        SalesOrderDetails nd = context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SODId == d.SODId).FirstOrDefault();
                        SalesOrderDetails sd = nd;
                        sd.DeliveryStatus = "Delivered";
                        context.Entry(nd).CurrentValues.SetValues(sd);
                        context.SaveChanges();
                        prodcts[y] = (int)d.InventoryId;
                        y++;
                    }
                    x++;
                }
                for(int i = 0; i < y; i++)
                {
                    ReservedQtyMaster rq = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.SOMId == res.DCId && a.ProductId == prodcts[i]).First();
                    ReservedQtyMaster r = rq;
                    r.CurrentStatus = "Despatched";
                    context.Entry(rq).CurrentValues.SetValues(r);
                    context.SaveChanges();
                }
                //List<ReservedQtyMaster> rqs = context.reservedQtyMasters.Where(a => a.IsDeleted == false && a.SOMId == res.DCId).ToList();
                //foreach(ReservedQtyMaster r in rqs)
                //{
                //    var a = r;
                //    r.CurrentStatus = "Despatched";
                //    context.Entry(a).CurrentValues.SetValues(r);
                //    context.SaveChanges();
                //}
                SalesOrderMaster nwm = som;
                if (y == x)
                {
                    som.DeliveryStatus = "Delivered";
                }
                else
                {
                    som.DeliveryStatus = "Partially Delivered";
                }
                context.Entry(nwm).CurrentValues.SetValues(som);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "please try again later";
            }
            return pr;
        }
    }
}
