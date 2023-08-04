using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class FinanceMgmtRepo : IFinanceMgmtRepo
    {
        private readonly MyDbContext context;

        public FinanceMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        #region Finance Group
        public ProcessResponse SaveFinGroup(FinanceGroups request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.financeGroups.Add(request);
                context.SaveChanges();
                responce.currentId = request.GroupId;
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
        public List<FinanceGroups> GetAllFinGroups()
        {
            List<FinanceGroups> response = new List<FinanceGroups>();
            try
            {
                response = context.financeGroups.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public FinanceGroups GetFinGroupById(int id)
        {
            FinanceGroups response = new FinanceGroups();
            try
            {
                response = context.financeGroups.Where(a => a.IsDeleted == false && a.GroupId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public FinanceGroups GetFinanceGroupByName(string name)
        {
            return context.financeGroups.Where(a => a.IsDeleted == false && a.GroupName == name).FirstOrDefault();
        }
        public ProcessResponse UpdateFinGroup(FinanceGroups request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                FinanceGroups fg = new FinanceGroups();
                fg = context.financeGroups.Where(a => a.GroupId == request.GroupId).FirstOrDefault();
                context.Entry(fg).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.GroupId;
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

        #region Finance Head
        public ProcessResponse SaveFinHead(FinanceHeads request)
        {
            ProcessResponse responce = new ProcessResponse();

            try
            {
                request.IsDeleted = false;
                context.financeHeads.Add(request);
                context.SaveChanges();
                responce.currentId = request.HeadId;
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
        public List<FinanceHeads> GetAllFinHeads(int grpId)
        {
            List<FinanceHeads> response = new List<FinanceHeads>();
            try
            {
                response = context.financeHeads.Where(a => a.IsDeleted == false&& a.GroupId==grpId).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public FinanceHeads GetFinHeadById(int id)
        {
            FinanceHeads response = new FinanceHeads();
            try
            {
                response = context.financeHeads.Where(a => a.IsDeleted == false && a.HeadId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public FinanceHeads GetFinHeadByStaffId(int id)
        {
            return context.financeHeads.Where(a => a.IsDeleted == false && a.StaffId == id).FirstOrDefault();
        }
        public ProcessResponse UpdateFinHead(FinanceHeads request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                FinanceHeads fh = new FinanceHeads();
                fh = context.financeHeads.Where(a => a.HeadId == request.HeadId).FirstOrDefault();
                context.Entry(fh).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.HeadId;
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

        public ProcessResponse SaveFinancialTransactions(FinanceTransactions request)
        {
            ProcessResponse pr = new ProcessResponse();

            try
            {
                //request.IsDeleted = false;
                context.financeTransactions.Add(request);
                context.SaveChanges();
                pr.currentId = request.TRID;
                pr.statusCode = 1;
                pr.statusMessage = "Sucess";
            }catch(Exception ex)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Failed";
                LogError(ex);
            }

            return pr;
        }
        public ProcessResponse UpdateFinanceTransaction(FinanceTransactions request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if(request.TRID > 0)
                {
                    var fT = context.financeTransactions.Where(a => a.TRID == request.TRID).FirstOrDefault();
                    if(fT!=null)
                    {
                        context.Entry(fT).CurrentValues.SetValues(fT);
                        context.SaveChanges();
                        response.statusCode = 1;
                        response.statusMessage = "Update success";

                    }
                    else
                    {
                        context.financeTransactions.Add(request);
                        context.SaveChanges();
                        response.statusCode = 1;
                        response.statusMessage = "save success";

                    }
                }
            }
            catch(Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public FinanceTransactions GetFTransactionById(int id)
        {
            return context.financeTransactions.Where(a => a.TRID == id).FirstOrDefault();
        }

        public List<FinanceTransactionsModel> GetFTransactionsByLedgerId(int ledgerId, DateTime fromDate, DateTime  toDate )
        {
            List<FinanceTransactionsModel> response = new List<FinanceTransactionsModel>();
            try
            {
                DateTime fDt = fromDate.AddDays(-1);
                DateTime tDt = toDate.AddDays(1);
                response = (from ft in context.financeTransactions
                            join fhFrom in context.financeHeads on ft.FromHeadID equals fhFrom.HeadId
                            join fjTo in context.financeHeads on ft.ToHeaID equals fjTo.HeadId
                            join um in context.userMasters on ft.UserId equals um.UserId into tempum
                            from u in tempum.DefaultIfEmpty()
                            join cm in context.customerMasters on ft.CustomerId equals cm.Cid into tempcm
                            from c in tempcm.DefaultIfEmpty()
                            where (ft.FromHeadID == ledgerId && ft.ToHeaID == ledgerId) 
                            && ft.DateOfTransaction > fDt && ft.DateOfTransaction < tDt
                            select new FinanceTransactionsModel
                            {
                                DateOfTransaction = ft.DateOfTransaction,
                                ChequeDate = ft.ChequeDate,
                                ChequeDetails = ft.ChequeDetails,
                                ChequeNo = ft.ChequeNo,
                                UserId = ft.UserId,
                                 CustomerId=ft.CustomerId,
                                  CustomerId_Name = c.FullName,
                                   UserId_Name=u.UserName,
                                Credit = ft.Credit, Debit = ft.Debit,
                                Description = ft.Description,
                                DoneBy = ft.DoneBy,
                                FromHeadID = ft.FromHeadID,
                                VoucherNumber = ft.VoucherNumber,
                                TTime = ft.TTime,
                                TRID = ft.TRID,
                                ToHeaID = ft.ToHeaID,
                                ToHeadID_Name = fjTo.HeadName,
                                remarks = ft.remarks,
                                FromHeadID_Name = fhFrom.HeadName,
                                VoucherType = ft.VoucherType

                            }).OrderByDescending(b=>b.DateOfTransaction).
                            ToList();
            }
            catch(Exception ex)
            {
                LogError(ex);
            }
            return response;
        }

        public  FinanceTransactionsModel  GetFTransactionByTrId(int trid)
        {
             FinanceTransactionsModel  response = new FinanceTransactionsModel();
            try
            {
                FinanceTransactions oltft = context.financeTransactions.Where(a => a.TRID == trid).FirstOrDefault();
                if (oltft.TRID > oltft.RelatedTrId)
                {
                    trid = oltft.RelatedTrId;
                }
                response = (from ft in context.financeTransactions
                            join fhFrom in context.financeHeads on ft.FromHeadID equals fhFrom.HeadId
                            join fjTo in context.financeHeads on ft.ToHeaID equals fjTo.HeadId
                            join um in context.userMasters on ft.UserId equals um.UserId into tempum
                            from u in tempum.DefaultIfEmpty()
                            join cm in context.customerMasters on ft.CustomerId equals cm.Cid into tempcm
                            from c in tempcm.DefaultIfEmpty()
                            where (ft.TRID == trid)
                            select new FinanceTransactionsModel
                            {
                                DateOfTransaction = ft.DateOfTransaction,
                                ChequeDate = ft.ChequeDate,
                                ChequeDetails = ft.ChequeDetails,
                                ChequeNo = ft.ChequeNo,
                                UserId = ft.UserId,
                                CustomerId = ft.CustomerId,
                                CustomerId_Name = c.FullName,
                                UserId_Name = u.UserName,
                                Credit = ft.Credit,
                                Debit = ft.Debit,
                                Description = ft.Description,
                                DoneBy = ft.DoneBy,
                                FromHeadID = ft.FromHeadID,
                                VoucherNumber = ft.VoucherNumber,
                                TTime = ft.TTime,
                                TRID = ft.TRID,
                                ToHeaID = ft.ToHeaID,
                                ToHeadID_Name = fjTo.HeadName,
                                remarks = ft.remarks,
                                FromHeadID_Name = fhFrom.HeadName,
                                VoucherType = ft.VoucherType

                            }).FirstOrDefault();
                response.companyDetails = context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public FinanceTransactionsModel GetPayMentModelByTrId(int trid)
        {
            FinanceTransactionsModel response = new FinanceTransactionsModel();
            response.companyDetails = new CompanyMaster();
            try
            {
                response = (from ft in context.payments
                            
                            where (ft.PayId == trid)
                            select new FinanceTransactionsModel
                            {
                                DateOfTransaction = ft.PaymentDate,
                                Credit=ft.Amount,
                                Description=ft.Description

                            }).FirstOrDefault();
                response.companyDetails = context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public FinanceTransactionsModel GetReceiptModelByTrId(int trid)
        {
            FinanceTransactionsModel response = new FinanceTransactionsModel();
            try
            {
                response = (from ft in context.recipts
                            
                            where (ft.PayId == trid)
                            select new FinanceTransactionsModel
                            {
                                DateOfTransaction = ft.PaymentDate,
                                Credit=ft.Amount,
                                Description=ft.Description

                            }).FirstOrDefault();
                response.companyDetails = context.companyMasters.Where(a => a.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public List<FinanceTransactionsModel> GetFTransactionsByGroupId(int groupId, DateTime fromDate, DateTime toDate)
        {
            List<FinanceTransactionsModel> response = new List<FinanceTransactionsModel>();
            try
            {
                DateTime fDt = fromDate.AddDays(-1);
                DateTime tDt = toDate.AddDays(1);
                response = (from ft in context.financeTransactions
                            join fhFrom in context.financeHeads on ft.FromHeadID equals fhFrom.HeadId
                            join fjTo in context.financeHeads on ft.ToHeaID equals fjTo.HeadId
                            join fhG in context.financeGroups on fhFrom.GroupId equals fhG.GroupId
                            join um in context.userMasters on ft.UserId equals um.UserId into tempum
                            from u in tempum.DefaultIfEmpty()
                            join cm in context.customerMasters on ft.CustomerId equals cm.Cid into tempcm
                            from c in tempcm.DefaultIfEmpty()
                            where  fhG.GroupId==groupId 
                            && ft.DateOfTransaction > fDt && ft.DateOfTransaction < tDt
                            select new FinanceTransactionsModel
                            {
                                DateOfTransaction = ft.DateOfTransaction,
                                ChequeDate = ft.ChequeDate,
                                ChequeDetails = ft.ChequeDetails,
                                ChequeNo = ft.ChequeNo,
                                UserId = ft.UserId,
                                CustomerId = ft.CustomerId,
                                CustomerId_Name = c.FullName,
                                UserId_Name = u.UserName,
                                Credit = ft.Credit,
                                Debit = ft.Debit,
                                Description = ft.Description,
                                DoneBy = ft.DoneBy,
                                FromHeadID = ft.FromHeadID,
                                VoucherNumber = ft.VoucherNumber,
                                TTime = ft.TTime,
                                TRID = ft.TRID,
                                ToHeaID = ft.ToHeaID,
                                ToHeadID_Name = fjTo.HeadName,
                                remarks = ft.remarks,
                                FromHeadID_Name = fhFrom.HeadName,
                                VoucherType = ft.VoucherType

                            }).OrderByDescending(b => b.DateOfTransaction).
                            ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public FinanceTransactions GetFinTransatioinById(int id)
        {
            return context.financeTransactions.Where(a => a.TRID == id).FirstOrDefault();
        }
        public List<FinanceTransactionsModel> GetAllFinanceTransactions(int ledgId,DateTime fromDate,DateTime toDate)
        {

            List<FinanceTransactionsModel> response = new List<FinanceTransactionsModel>();
            
                response = (from ft in context.financeTransactions
                            join fh in context.financeHeads on ft.FromHeadID equals fh.HeadId
                            join fto in context.financeHeads on ft.ToHeaID equals fto.HeadId
                            where ft.FromHeadID == ledgId && ft.DateOfTransaction >= fromDate && ft.DateOfTransaction <= toDate
                            select new FinanceTransactionsModel
                            {
                                DateOfTransaction = ft.DateOfTransaction,
                                FromHeadID_Name = fto.HeadName,
                                
                                VoucherType = ft.VoucherType,
                                VoucherNumber = ft.VoucherNumber,
                                Debit = ft.Debit,
                                Credit = ft.Credit,
                                TRID = ft.TRID,
                            }).OrderBy(b => b.DateOfTransaction).ToList();               
            
            return response;                      
        }
        public int GetLastVoucherNumber()
        {
            int res = 0;
            try
            {
                FinanceTransactions f = context.financeTransactions.OrderBy(b => b.VoucherNumber).Last();
                if (f != null)
                {
                    res = (int)f.VoucherNumber;
                }
            }catch(Exception e) { }
                
            return res;
        }
        public List<FinanceTransactionsModel> GetTodayTransactions(DateTime strt,DateTime edt)
        {
            List<FinanceTransactionsModel> response = new List<FinanceTransactionsModel>();

            //DateTime tod = DateTime.Now;
            //DateTime strt = new DateTime(tod.Year, tod.Month, tod.Day);
            //DateTime monthStartDay = new DateTime(tod.Year, tod.Month, 1);
            //DateTime monthEndDay = monthStartDay.AddMonths(1).AddDays(-1);
            //DateTime edt = new DateTime();
            //if (monthEndDay.Day == strt.Day)
            //{
            //    edt = new DateTime(tod.Year, (tod.Month+1), 1);
            //}
            //else
            //{
            //    edt = new DateTime(tod.Year, tod.Month, (tod.Day + 1));
            //}
            response = (from ft in context.financeTransactions
                        join fh in context.financeHeads on ft.FromHeadID equals fh.HeadId
                        join fto in context.financeHeads on ft.ToHeaID equals fto.HeadId
                        join fg in context.financeGroups on fh.GroupId equals fg.GroupId
                        join tg in context.financeGroups on fto.GroupId equals tg.GroupId
                        where ft.DateOfTransaction >= strt && ft.DateOfTransaction <= edt && (ft.VoucherType == "Payment" || ft.VoucherType== "Receipt") && (ft.VoucherNumber%2==0)
                        select new FinanceTransactionsModel
                        {
                            DateOfTransaction = ft.DateOfTransaction,
                            FromHeadID_Name = fh.HeadName,
                            ToHeadID_Name=fto.HeadName,
                            FromGroupName=fg.GroupName,
                            ToGroupName=tg.GroupName,
                            VoucherType = ft.VoucherType,
                            VoucherNumber = ft.VoucherNumber,
                            Debit = ft.Debit,
                            Credit = ft.Credit,
                            TRID = ft.TRID,
                        }).OrderBy(b => b.DateOfTransaction).ToList();

            return response;
        }
        public List<Payments> GetAllPayments(DateTime from, DateTime to)
        {
            List<Payments> res = new List<Payments>();

            try
            {
                res = context.payments.Where(a => a.IsDeleted == false && a.PaymentDate>from && a.PaymentDate<to).ToList();
            }catch(Exception e)
            {

            }

            return res;
        }
        public ProcessResponse SavePayment(Payments req)
        {
            ProcessResponse pr = new ProcessResponse();

            try
            {
                context.payments.Add(req);
                context.SaveChanges();
                pr.currentId = req.PayId;
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Please try again";
            }

            return pr;
        }
        public ProcessResponse DeletePayment(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                Payments rp = context.payments.Where(a => a.IsDeleted == false && a.PayId == id).FirstOrDefault();
                Payments old = rp;
                rp.IsDeleted = true;
                context.Entry(old).CurrentValues.SetValues(rp);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            catch (Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Please Try again later";
            }
            return pr;
        }
        public List<Recipts> GetAllReceipts(DateTime from, DateTime to)
        {
            List<Recipts> res = new List<Recipts>();

            try
            {
                res = context.recipts.Where(a => a.IsDeleted == false && a.PaymentDate > from && a.PaymentDate < to).ToList();
            }
            catch (Exception e)
            {

            }

            return res;
        }
        public ProcessResponse SaveRecipt(Recipts req)
        {
            ProcessResponse pr = new ProcessResponse();

            try
            {
                context.recipts.Add(req);
                context.SaveChanges();
                pr.currentId = req.PayId;
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            catch (Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Please try again";
            }

            return pr;
        }
        public ProcessResponse DeleteReceipt(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                Recipts rp = context.recipts.Where(a => a.IsDeleted == false && a.PayId == id).FirstOrDefault();
                Recipts old = rp;
                rp.IsDeleted = true;
                context.Entry(old).CurrentValues.SetValues(rp);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            catch (Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Please Try again later";
            }
            return pr;
        }
    }
}

