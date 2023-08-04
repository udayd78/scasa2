using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class FinanceMgmtService : IFinanceMgmtService
    {
        private readonly IFinanceMgmtRepo fRepo;
        public FinanceMgmtService(IFinanceMgmtRepo _fRepo)
        {
            fRepo = _fRepo;
        }
        #region  Finance Group
        public ProcessResponse SaveFinGroup(FinanceGroups request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.GroupId > 0)
                {
                    FinanceGroups fg =new FinanceGroups();
                    fg = fRepo.GetFinGroupById(request.GroupId);
                    fg.GroupName = request.GroupName;
                    fg.GroupCode = request.GroupCode;
                    request.IsDeleted = fg.IsDeleted;
                    response = fRepo.UpdateFinGroup(request);

                }
                else
                {
                    request.IsDeleted = false;
                    response = fRepo.SaveFinGroup(request);
                }
            }
            catch (Exception ex)
            {
                fRepo.LogError(ex);
            }

            return response;
        }
        public List<FinanceGroups> GetAllFinGroups()
        {
            return fRepo.GetAllFinGroups();
        }
        public FinanceGroups GetFinGroupById(int id)
        {
            return fRepo.GetFinGroupById(id);
        }
        public FinanceGroups GetFinanceGroupByName(string name)
        {
            return fRepo.GetFinanceGroupByName(name);
        }
        #endregion

        #region  Finance Head
        public ProcessResponse SaveFinHead(FinanceHeads request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.HeadId > 0)
                {
                    FinanceHeads fh = fRepo.GetFinHeadById(request.HeadId);
                    fh.HeadName = request.HeadName;
                    fh.HeadCode = request.HeadCode;
                    request.IsDeleted = fh.IsDeleted;
                    fh.StartingBallance = request.StartingBallance;
                    fh.CurrentBallance = request.CurrentBallance;
                    response = fRepo.UpdateFinHead(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = fRepo.SaveFinHead(request);
                }
            }
            catch (Exception ex)
            {
                fRepo.LogError(ex);
            }

            return response;
        }
        public List<FinanceHeads> GetAllFinHeads(int grpId)
        {
            return fRepo.GetAllFinHeads(grpId);
        }
        public FinanceHeads GetFinHeadById(int id)
        {
            return fRepo.GetFinHeadById(id);
        }
        public FinanceHeads GetFinHeadByStaffId(int id)
        {
            return fRepo.GetFinHeadByStaffId(id);
        }
        #endregion
        public ProcessResponse SaveFinTransactions(FinanceTransactions request)
        {
            ProcessResponse pr = new ProcessResponse();
            if (request.TRID > 0)
            {
                pr = fRepo.UpdateFinanceTransaction(request);
            }
            else
            {
                pr= fRepo.SaveFinancialTransactions(request);
            }
            return pr;
        }
        public List<FinanceTransactionsModel> GetFTransactionsByLedgerId(int ledgerId, DateTime fromDate, DateTime toDate)
        {
            return fRepo.GetFTransactionsByLedgerId(ledgerId,fromDate,toDate);
        }
        public List<FinanceTransactionsModel> GetFTransactionsByGroupId(int groupId, DateTime fromDate, DateTime toDate)
        {
            return fRepo.GetFTransactionsByGroupId(groupId, fromDate, toDate);
        }
        public FinanceTransactions GetFinTransById(int id)
        {
            return fRepo.GetFinTransatioinById(id);
        }
        public List<FinanceTransactionsModel> GetAllTransactions(int ledgId, DateTime fromDate, DateTime toDate)
        {
            return fRepo.GetAllFinanceTransactions(ledgId,fromDate,toDate);
        }
        public int GetPreviousVoucherNumber()
        {
            return fRepo.GetLastVoucherNumber();
        }
        public List<FinanceTransactionsModel> GetTodayTransactions(DateTime strt, DateTime edt)
        {
            return fRepo.GetTodayTransactions(strt,edt);
        }

        public FinanceTransactionsModel GetFTransactionByTrId(int trid)
        {
            return fRepo.GetFTransactionByTrId(trid);
        }

        public List<Payments> GetAllPayments(DateTime from, DateTime to)
        {
            return fRepo.GetAllPayments(from,to);
        }

        public ProcessResponse SavePayment(Payments req)
        {
            return fRepo.SavePayment(req);
        }
        public ProcessResponse DeletePayment(int id)
        {
            return fRepo.DeletePayment(id);
        }

        public List<Recipts> GetAllReceipts(DateTime from, DateTime to)
        {
            return fRepo.GetAllReceipts(from,to);
        }
        
        public ProcessResponse SaveRecipt(Recipts req)
        {
            return fRepo.SaveRecipt(req);
        }
        public ProcessResponse DeleteReceipt(int id)
        {
            return fRepo.DeleteReceipt(id);
        }
        public FinanceTransactionsModel GetPayMentModelByTrId(int trid)
        {
            return fRepo.GetPayMentModelByTrId(trid);
        }
        public FinanceTransactionsModel GetReceiptModelByTrId(int trid)
        {
            return fRepo.GetReceiptModelByTrId(trid);
        }
    }
}
