using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface ISalesDetailsMgmtService
    {
        ProcessResponse SaveCRFQ(CRFQDetails request);
        List<CRFQDetails> GetAllCRFQDetails();
        CRFQDetails GetCRFQDetailsById(int id);
        List<CRFQDetails> GetCRFQDetails0fMaster(int mastId);
        ProcessResponse SaveQuoteDetails(QuoteDetails request);
        List<QuoteDetails> GetAllQuoteDetails();
        QuoteDetails GetQuoteDetailsById(int id);
        ProcessResponse SaveSODetails(SalesOrderDetails request);
        List<SalesOrderDetails> GetAllSODetails();
        List<DeleverySaleOrderModel> GetAllReadyToDispatchSOMasters();
        SalesOrderDetails GetSODetailsById(int id);
        List<SalesOrderDetails> GetSoDetailsOfMaster(int mId);
    }
}
