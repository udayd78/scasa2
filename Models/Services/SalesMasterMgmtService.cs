using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class SalesMasterMgmtService : ISalesMasterMgmtService
    {
        private readonly ISalesMasterMgmtRepo sRepo;
        private readonly ICommonRepo cRepo;
        public SalesMasterMgmtService(ISalesMasterMgmtRepo _sRepo, ICommonRepo commonRepo)
        {
            sRepo = _sRepo;
            cRepo = commonRepo;
        }
        #region  CRFQ Master
        public ProcessResponse SaveCRFQ(CRFQMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.CRFQId > 0)
                {
                    response = sRepo.UpdateCRFQ(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = sRepo.SaveCRFQ(request);
                }
            }
            catch(Exception ex)
            {
                sRepo.LogError(ex);
            }

            return response;
        }
        public List<CRFQMaster> GetAllCRFQ()
        {
            return sRepo.GetAllCRFQ();
        }
        public CRFQMaster GetCRFQById(int id)
        {
            return sRepo.GetCRFQById(id);
        }
        #endregion

        #region Quote Master
        public ProcessResponse SaveQuote(QuoteMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.QuoteId > 0)
                {
                    response = sRepo.UpdateQuote(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = sRepo.SaveQuote(request);
                }
            }
            catch(Exception ex)
            {
                sRepo.LogError(ex);
            }

            return response;
        }
        public List<QuoteMaster> GetAllQuote()
        {
            return sRepo.GetAllQuote();
        }
        public QuoteMaster GetQuoteById(int id)
        {
            return sRepo.GetQuoteById(id);
        }
        #endregion

        #region SO Master
        public ProcessResponse SaveSOMaster(SalesOrderMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.SOMId > 0)
                {
                    response = sRepo.UpdateSales(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = sRepo.SaveSalesOrder(request);
                }
            }
            catch(Exception ex)
            {
                sRepo.LogError(ex);
            }

            return response;
        }
        public ProcessResponse DeleteSaleOrder(int id)
        {
            return sRepo.DeleteSaleOrder(id);
        }
        public List<SalesOrderMaster> GetAllSales()
        {
            return sRepo.GetAllSales();
        }
        public SalesOrderMaster GetSalesById(int id)
        {
            return sRepo.GetSalesById(id);
        }
        #endregion

        public ProductDisplaySalesModel GetCatsForSales(int pageNumber =1 , int pageSize = 10)
        {
            ProductDisplaySalesModel response = new ProductDisplaySalesModel();
            try
            {
                response.cats = cRepo.GetCatsDrop();
                //if(response.cats.Count > 0)
                //{
                //    for(int i =0; i < response.cats.Count; i ++)
                //    {
                //        response.cats[i].products = sRepo.GetProductsByCats(response.cats[i].CategoryId,pageNumber, pageSize);

                //    }

                //}
            }catch(Exception ex)
            {

            }


            return response;
        }
        public List<ProductsDisplaySales> GetCatProds(int cd,int pagenumber=1,int pageSize = 12)
        {
            return sRepo.GetProductsByCats(cd, pagenumber, pageSize);
        }
        public int GetNoOfCartProds(int cid)
        {
            return sRepo.GetCatProdsCount(cid);
        }

        public List<ProductsDisplaySales> SearchProducts(int catid = 0,int subCatId=0, string search = "", int pageNumber = 1, int pageSize = 12)
        {
            return sRepo.SearchProducts(catid,subCatId, search,pageNumber,pageSize);
        }
        public int SearchProdsCount(int catid = 0, int subCatId = 0, string search = "")
        {
            return sRepo.SearchProdsCount(catid,subCatId,search);
        }

        public List<MPayRollDisplayModel> GetPayrollForStaff(int staffId)
        {
            List<MPayRollDisplayModel> response = new List<MPayRollDisplayModel>();
            try
            {
                response = sRepo.GetMonthlyPayRoll(staffId);

            }catch(Exception ex)
            {

            }

            return response;

        }

        public List<LeadInfo> GetLeadInfo(int staffId)
        {
            List<LeadInfo> response = new List<LeadInfo>();
            try
            {
                response = sRepo.GetLeadInfo(staffId);

            }
            catch (Exception ex)
            {

            }

            return response;

        }

        public List<LoansInformation> GetLoansInformation(int staffId)
        {
            return sRepo.GetLoansInformation(staffId);
        }

        public List<AttendanceInfo> GetStaffAttendance(int staffId)
        {
            return sRepo.GetStaffAttendance(staffId);
        }

        public List<MPayRollDisplayModel> GetMonthlyPayRoll(int staffId)
        {
            return sRepo.GetMonthlyPayRoll(staffId);
        }

        public void SaveActivity(CustomerSalesActivity ca)
        {
            sRepo.SaveActivity(ca);
        }

        public CustomerSalesActivity GetActivity(int id)
        {
            return sRepo.GetActivity(id);
        }

        public void UpdateActivty(CustomerSalesActivity request)
        {
            sRepo.UpdateActivty(request);
        }

        public ProcessResponse SubmitSOForAccounts(int soid, int currentUserId)
        {
            return sRepo.SubmitSOForAccounts(soid, currentUserId);
        }

        public List<SaleOrdersForAccountsModel> GetOrdersSubmittedForAccounts()
        {
            return sRepo.GetOrdersSubmittedForAccounts();
        }

        public ProcessResponse SaveSalesReceipt(SalesReceipts request)
        {
            return sRepo.SaveSalesReceipt(request);
        }

        public List<SalesReceiptsModel> GetSalesReciepts(int soid)
        {
            return sRepo.GetSalesReciepts(soid);
        }

        public List<FinanceHeads> GetFinanceHeads()
        {
            return sRepo.GetFinanceHeads();
        }

        public ProcessResponse PostReceiptToFinance(int trid)
        {
            return sRepo.PostReceiptToFinance(trid);
        }
        public CompanyMaster GetCompany()
        {
            return sRepo.GetCompany();
        }
    }

}
