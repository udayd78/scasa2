using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class SalesDetailsMgmtService : ISalesDetailsMgmtService
    {
        private readonly ISalesDetailsMgmtRepo sRepo;
        public SalesDetailsMgmtService(ISalesDetailsMgmtRepo _sRepo)
        {
            sRepo = _sRepo;
        }
        #region  CRFQ Details
        public ProcessResponse SaveCRFQ(CRFQDetails request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.CRFQDetailsId > 0)
                {                                     
                    response = sRepo.UpdateCRFQDetails(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = sRepo.SaveCRFQDetails(request);
                }
            }
            catch (Exception ex)
            {
                sRepo.LogError(ex);
            }
       
            return response;
        }
        public List<CRFQDetails> GetAllCRFQDetails()
        {
            return sRepo.GetAllCRFQDetails();
        }
        public CRFQDetails GetCRFQDetailsById(int id)
        {
            return sRepo.GetCRFQDetailsById(id);
        }
        public List<CRFQDetails> GetCRFQDetails0fMaster(int mastId)
        {
            return sRepo.GetCRFQDetails0fMaster(mastId);
        }
        #endregion

        #region Quote Details
        public ProcessResponse SaveQuoteDetails(QuoteDetails request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.QuoteDetailsId > 0)
                {
                    response = sRepo.UpdateQuoteDetails(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = sRepo.SaveQuoteDetails(request);
                }
            }
            catch(Exception ex)
            {
                sRepo.LogError(ex);
                
            }
            return response;
        }
        public List<QuoteDetails> GetAllQuoteDetails()
        {
            return sRepo.GetAllQuoteDetails();
        }
        public QuoteDetails GetQuoteDetailsById(int id)
        {
            return sRepo.GetQuoteDetailsById(id);
        }
        #endregion

        #region SO Details
        public ProcessResponse SaveSODetails(SalesOrderDetails request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.SODId > 0)
                {
                    response = sRepo.UpdateSODetails(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = sRepo.SaveSODetails(request);

                }
            }
            catch(Exception ex)
            {
                sRepo.LogError(ex);
            }

            return response;
        }
        public List<SalesOrderDetails> GetAllSODetails()
        {
            return sRepo.GetAllSODetails();
        }
        public List<DeleverySaleOrderModel> GetAllReadyToDispatchSOMasters()
        {
            return sRepo.GetAllReadyToDispatchSOMasters();
        }
        public SalesOrderDetails GetSODetailsById(int id)
        {
            return sRepo.GetSalesById(id);
        }
        public List<SalesOrderDetails> GetSoDetailsOfMaster(int mId)
        {
            return sRepo.GetSoDetailsOfMaster(mId);
        }
        #endregion
    }
}
