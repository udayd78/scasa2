using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface ISalesDetailsMgmtRepo
    {
        ProcessResponse SaveCRFQDetails(CRFQDetails request);
        List<CRFQDetails> GetAllCRFQDetails();
        CRFQDetails GetCRFQDetailsById(int id);
        List<CRFQDetails> GetCRFQDetails0fMaster(int mastId);
        ProcessResponse UpdateCRFQDetails(CRFQDetails request);
        ProcessResponse SaveQuoteDetails(QuoteDetails request);
        List<QuoteDetails> GetAllQuoteDetails();
        QuoteDetails GetQuoteDetailsById(int id);
        ProcessResponse UpdateQuoteDetails(QuoteDetails request);
        ProcessResponse SaveSODetails(SalesOrderDetails request);
        List<SalesOrderDetails> GetAllSODetails();
        List<DeleverySaleOrderModel> GetAllReadyToDispatchSOMasters();
        SalesOrderDetails GetSalesById(int id);
        List<SalesOrderDetails> GetSoDetailsOfMaster(int mId);
        ProcessResponse UpdateSODetails(SalesOrderDetails request);
        void LogError(Exception ex);
    }
}
