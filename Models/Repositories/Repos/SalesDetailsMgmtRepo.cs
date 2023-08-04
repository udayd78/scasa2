using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class SalesDetailsMgmtRepo : ISalesDetailsMgmtRepo
    {
        private readonly MyDbContext context;

        public SalesDetailsMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }
        #region CRFQ Details
        public ProcessResponse SaveCRFQDetails(CRFQDetails request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.cRFQDetails.Add(request);
                context.SaveChanges();
                responce.currentId = request.CRFQDetailsId;
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
        public List<CRFQDetails> GetAllCRFQDetails()
        {
            List<CRFQDetails> response = new List<CRFQDetails>();
            try
            {
                response = context.cRFQDetails.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public CRFQDetails GetCRFQDetailsById(int id)
        {
            CRFQDetails response = new CRFQDetails();
            try
            {
                response = context.cRFQDetails.Where(a => a.IsDeleted == false && a.CRFQDetailsId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List<CRFQDetails> GetCRFQDetails0fMaster(int mastId)
        {
            return context.cRFQDetails.Where(a => a.IsDeleted == false && a.CRFQId == mastId).ToList();
        }
        public ProcessResponse UpdateCRFQDetails(CRFQDetails request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                CRFQDetails temp = context.cRFQDetails.Where(a => a.CRFQId == request.CRFQId).FirstOrDefault();
                context.Entry(temp).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.CRFQDetailsId;
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

        #region Quote Details
        public ProcessResponse SaveQuoteDetails(QuoteDetails request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.quoteDetails.Add(request);
                context.SaveChanges();
                responce.currentId = request.QuoteDetailsId;
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
        public List<QuoteDetails> GetAllQuoteDetails()
        {
            List<QuoteDetails> response = new List<QuoteDetails>();
            try
            {
                response = context.quoteDetails.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public QuoteDetails GetQuoteDetailsById(int id)
        {
            QuoteDetails response = new QuoteDetails();
            try
            {
                response = context.quoteDetails.Where(a => a.IsDeleted == false && a.QuoteDetailsId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateQuoteDetails(QuoteDetails request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                QuoteDetails qd = new QuoteDetails();
                qd = context.quoteDetails.Where(a => a.QuoteDetailsId == request.QuoteDetailsId).FirstOrDefault();
                context.Entry(qd).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.QuoteDetailsId;
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

        #region SO Details
        public ProcessResponse SaveSODetails(SalesOrderDetails request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.salesOrderDetails.Add(request);
                context.SaveChanges();
                responce.currentId = request.SODId;
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
        public List<SalesOrderDetails> GetAllSODetails()
        {
            List<SalesOrderDetails> response = new List<SalesOrderDetails>();
            try
            {
                response = context.salesOrderDetails.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List<DeleverySaleOrderModel> GetAllReadyToDispatchSOMasters()
        {
            List<DeleverySaleOrderModel> response = new List<DeleverySaleOrderModel>();
            try
            {
                response = (from m in context.salesOrderMasters
                            join curr in context.customerMasters on m.CustomerId equals curr.Cid
                            join a in context.addressMasters on m.ShippingAddressId equals a.Addid
                            join c in context.cityMasters on a.CityId equals c.Id
                            join s in context.stateMasters on a.StateId equals s.Id
                            join con in context.countryMasters on a.CountryId equals con.Id
                            where m.IsDeleted == false && m.SOStatus == "Invoice Created" &&(m.DeliveryStatus == "Ready to Dispatch" || m.DeliveryStatus == "Partially Delivered")
                            select new DeleverySaleOrderModel
                            {
                                SOMId=m.SOMId,
                                CustomerName=curr.FullName,
                                CurrentStatus=m.CurrentStatus,
                                Address= a.HouseNumber + ", " + a.StreetName + ", " + a.Location + ", <BR/>" + c.CityName + "," + s.StateName + "," + con.CountryName,
                                InvoiceCreatedDate=m.InvoiceCreatedDate,
                                DeliveryStatus=m.DeliveryStatus,
                                //InvoiceCreatedDate=m.InvoiceCreatedDate
                            }).OrderByDescending(b=>b.SOMId).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public SalesOrderDetails GetSalesById(int id)
        {
            SalesOrderDetails response = new SalesOrderDetails();
            try
            {
                response = context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SODId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List<SalesOrderDetails> GetSoDetailsOfMaster(int mId)
        {
            return context.salesOrderDetails.Where(a => a.IsDeleted == false && a.SOMId == mId).ToList();
        }
        public ProcessResponse UpdateSODetails(SalesOrderDetails request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                SalesOrderDetails so = context.salesOrderDetails.Where(a => a.SODId == request.SODId).FirstOrDefault();
                context.Entry(so).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.SODId;
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
    }
}
