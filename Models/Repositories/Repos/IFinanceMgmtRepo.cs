using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IFinanceMgmtRepo
    {
        FinanceTransactionsModel GetFTransactionByTrId(int trid);
        ProcessResponse SaveFinGroup(FinanceGroups request);
        List<FinanceGroups> GetAllFinGroups();
        FinanceGroups GetFinGroupById(int id);
        FinanceGroups GetFinanceGroupByName(string name);
        ProcessResponse UpdateFinGroup(FinanceGroups request);
        ProcessResponse SaveFinHead(FinanceHeads request);
        List<FinanceHeads> GetAllFinHeads(int grpId);
        FinanceHeads GetFinHeadById(int id);
        FinanceHeads GetFinHeadByStaffId(int id);
        ProcessResponse UpdateFinHead(FinanceHeads request);
        void LogError(Exception ex);
        ProcessResponse SaveFinancialTransactions(FinanceTransactions request);
        ProcessResponse UpdateFinanceTransaction(FinanceTransactions request);
        List<FinanceTransactionsModel> GetFTransactionsByLedgerId(int ledgerId, DateTime fromDate, DateTime toDate);
        List<FinanceTransactionsModel> GetFTransactionsByGroupId(int groupId, DateTime fromDate, DateTime toDate);
        List<FinanceTransactionsModel> GetAllFinanceTransactions(int ledgId, DateTime fromDate, DateTime toDate);
        FinanceTransactions GetFinTransatioinById(int id);        
        int GetLastVoucherNumber();
        List<FinanceTransactionsModel> GetTodayTransactions(DateTime strt, DateTime edt);
        List<Payments> GetAllPayments(DateTime from, DateTime to);
        ProcessResponse SavePayment(Payments req);
        ProcessResponse DeletePayment(int id);
        List<Recipts> GetAllReceipts(DateTime from, DateTime to);
        ProcessResponse SaveRecipt(Recipts req);
        ProcessResponse DeleteReceipt(int id);
        FinanceTransactionsModel GetPayMentModelByTrId(int trid);
        FinanceTransactionsModel GetReceiptModelByTrId(int trid);
    }
}
