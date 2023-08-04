using Microsoft.AspNetCore.Mvc;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Controllers
{
    public class FinanceController : Controller
    {
        private readonly ICommonService cServise;
        private readonly IFinanceMgmtService fServise;
        private readonly ISalesMasterMgmtService smService;
        private readonly ICompanyMgmtService companyService;
        private readonly ICustomerMgmtService cusService;
        public FinanceController(ICommonService _cService,IFinanceMgmtService _fService, ISalesMasterMgmtService _smService,ICompanyMgmtService companySer,
                                 ICustomerMgmtService _cusSer)
        {
            cServise = _cService;
            fServise = _fService;
            smService = _smService;
            companyService = companySer;
            cusService = _cusSer;
        }
        public IActionResult GetCurrentBalance(int hId)
        {
            FinanceHeads fh = fServise.GetFinHeadById(hId);
            CultureInfo indian = new CultureInfo("hi-IN");
            string cBal = string.Format(indian, "{0:N}", fh.CurrentBallance);
            return Json(new { bal = cBal});
        }
        public IActionResult Payments(int id=0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            FinanceTransactionsModel ftm = new FinanceTransactionsModel();

            ftm.fromHeadDrops = cServise.GetBankAndCashHeads();            
            ftm.toHeadDrops = cServise.GetNoBankAndCashHeads();
              
            ViewBag.fCurBal = 0;
            ViewBag.tCurBal = 0;
            
            if (ftm.fromHeadDrops.Count>0)
            {
                ViewBag.fCurBal = ftm.fromHeadDrops[0].CurrentBallance;
            }            
            if (ftm.toHeadDrops.Count>0)
            {
                ViewBag.tCurBal = ftm.toHeadDrops[0].CurrentBallance;
            }
            if (id > 0)
            {
                FinanceTransactions ft = fServise.GetFinTransById(id);
                
                ViewBag.isNew = "No";
                FinanceHeads fh = fServise.GetFinHeadById((int)ft.FromHeadID);
                FinanceGroups fg = fServise.GetFinGroupById((int)fh.GroupId);
                FinanceHeads th = fServise.GetFinHeadById((int)ft.ToHeaID);
                if (fg.GroupName== "Bank Accounts" || fg.GroupName== "Cash-in-hand")
                {                    
                    ViewBag.tCurBal = th.CurrentBallance;
                    ViewBag.fCurBal = fh.CurrentBallance;
                    //CloneObjects.CopyPropertiesTo(ft, ftm);
                    ftm.DateOfTransaction = ft.DateOfTransaction;
                    ftm.FromHeadID = ft.FromHeadID;
                    ftm.ToHeaID = ft.ToHeaID;
                    ftm.Credit = ft.Debit;
                    ftm.Description = ft.Description;
                    ftm.TRID = ft.TRID;
                }
                else
                {
                    //ft= fServise.GetFinTransById(ft.RelatedTrId);
                    ViewBag.tCurBal = fh.CurrentBallance;
                    ViewBag.fCurBal = th.CurrentBallance;
                    ftm.DateOfTransaction = ft.DateOfTransaction;
                    ftm.Description = ft.Description;
                    ftm.FromHeadID = ft.ToHeaID;
                    ftm.ToHeaID = ft.FromHeadID;
                    ftm.Credit = ft.Credit;
                    ftm.TRID = ft.RelatedTrId;
                }                
            }
            else
            {
                ftm.DateOfTransaction = DateTime.Now;
            }
            return View(ftm);
        }
        public IActionResult SavePayment(FinanceTransactionsModel request) 
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            FinanceHeads fh = new FinanceHeads();
            FinanceHeads th = new FinanceHeads();
            ProcessResponse response = new ProcessResponse();
            if (request.FromHeadID == null || request.ToHeaID == null)
            {
                response.statusCode = 5;
                response.statusMessage = "Please select from and to Heads";
                return Json(new { result = response });
            }
            if (request.Credit <= 0)
            {
                response.statusCode = 4;
                response.statusMessage = "Amount Canot be Negative or 0";
                return Json(new { result = response });
            }
            if (request.DateOfTransaction >= DateTime.Now)
            {
                response.statusCode = 3;
                response.statusMessage = "Date of Transaction can't be in future";
                return Json(new { result = response });
            }
            fh = fServise.GetFinHeadById((int)request.FromHeadID);
            th = fServise.GetFinHeadById((int)request.ToHeaID);
            if (request.TRID > 0)
            {
                FinanceTransactions oldft = fServise.GetFinTransById(request.TRID);
                FinanceTransactions oldft2 = fServise.GetFinTransById(oldft.RelatedTrId);
                oldft.DateOfTransaction = request.DateOfTransaction;
                oldft2.DateOfTransaction = request.DateOfTransaction;
                oldft.Description = request.Description;
                oldft2.Description = request.Description;
                if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit == request.Credit)
                {
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit == request.Credit)
                {
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);                    
                    th.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(th);
                    oldft.ToHeaID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.ToHeaID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit == request.Credit)
                {
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    fh.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(fh);
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.ToHeaID = request.FromHeadID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit == request.Credit)
                {
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);
                    th.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    fh.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(fh);
                    oldft.ToHeaID = request.ToHeaID;
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.ToHeaID = request.FromHeadID;
                        oldft2.FromHeadID = request.ToHeaID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit != request.Credit)
                {
                    decimal diff = (decimal)(request.Credit - oldft.Debit);
                    fh.CurrentBallance -= diff;
                    fServise.SaveFinHead(fh);
                    th.CurrentBallance += diff;
                    fServise.SaveFinHead(th);
                    oldft.Debit = request.Credit;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.Credit = request.Credit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit != request.Credit)
                {
                    decimal diff = (decimal)(request.Credit - oldft.Debit);
                    fh.CurrentBallance -= diff;
                    fServise.SaveFinHead(fh);
                    th.CurrentBallance += request.Credit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);
                    oldft.Debit = request.Credit;
                    oldft.ToHeaID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.ToHeaID;
                        oldft2.Credit = request.Credit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit != request.Credit)
                {
                    decimal diff = (decimal)(request.Credit - oldft.Debit);
                    fh.CurrentBallance -= request.Credit;
                    fServise.SaveFinHead(fh);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    th.CurrentBallance += diff;
                    fServise.SaveFinHead(th);
                    oldft.Debit = request.Credit;
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.Credit = request.Credit;
                        oldft2.ToHeaID = request.FromHeadID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit != request.Credit)
                {
                    decimal diff = (decimal)(request.Credit - oldft.Debit);
                    fh.CurrentBallance -= request.Credit;
                    fServise.SaveFinHead(fh);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    th.CurrentBallance += request.Credit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);
                    oldft.Debit = request.Credit;
                    oldft.ToHeaID = request.ToHeaID;
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.Credit = request.Credit;
                        oldft2.FromHeadID = request.ToHeaID;
                        oldft2.ToHeaID = request.FromHeadID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
            }
            else
            {
                
                int v = fServise.GetPreviousVoucherNumber();
                FinanceTransactions ft = new FinanceTransactions();
                CloneObjects.CopyPropertiesTo(request, ft);
                ft.VoucherType = "Payment";
                ft.VoucherNumber = ++v;
                ft.Debit = request.Credit;
                ft.Credit = 0;
                ft.TTime = DateTime.Now;
                ft.DoneBy = loginCheckResponse.userName;
                //ft.IsDeleted = false;
                response = fServise.SaveFinTransactions(ft);
                if (response.statusCode == 1)
                {
                    FinanceTransactions f = new FinanceTransactions();
                    f.VoucherType = "Payment";
                    f.DateOfTransaction = request.DateOfTransaction;
                    f.Description = request.Description;
                    f.VoucherNumber = ++v;
                    f.Debit = 0;
                    f.Credit = request.Credit;
                    f.ToHeaID = request.FromHeadID;
                    f.FromHeadID = request.ToHeaID;
                    f.TTime = DateTime.Now;
                    f.DoneBy = loginCheckResponse.userName;
                    f.RelatedTrId = response.currentId;
                    //ft.IsDeleted = false;
                    var response1 = fServise.SaveFinTransactions(f);
                    ft.RelatedTrId = response1.currentId;
                    var r5 = fServise.SaveFinTransactions(ft);

                    fh.CurrentBallance -= request.Credit;
                    var response2 = fServise.SaveFinHead(fh);
                    if (response2.statusCode == 1)
                    {
                        th.CurrentBallance += request.Credit;
                        response2 = fServise.SaveFinHead(th);
                    }
                }
            }
            return Json(new { result = response });
        }
        public IActionResult Receipt(int id=0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            FinanceTransactionsModel ftm = new FinanceTransactionsModel();

            ftm.fromHeadDrops = cServise.GetNoBankAndCashHeads();
            ftm.toHeadDrops = cServise.GetBankAndCashHeads();
               
            ViewBag.fCurBal = 0;
            ViewBag.tCurBal = 0;
            
            if (ftm.fromHeadDrops.Count > 0)
            {
                ViewBag.fCurBal = ftm.fromHeadDrops[0].CurrentBallance;
            }            
            if (ftm.toHeadDrops.Count > 0)
            {
                ViewBag.tCurBal = ftm.toHeadDrops[0].CurrentBallance;
            }
            if (id > 0)
            {
                FinanceTransactions ft = fServise.GetFinTransById(id);
                //CloneObjects.CopyPropertiesTo(ft, ftm);
                //ftm.Credit = ftm.Debit;
                ViewBag.isNew = "No";
                FinanceHeads fh = fServise.GetFinHeadById((int)ft.FromHeadID);
                FinanceHeads th = fServise.GetFinHeadById((int)ft.ToHeaID);
                FinanceGroups fg = fServise.GetFinGroupById((int)fh.GroupId);
                if (fg.GroupName == "Bank Accounts" || fg.GroupName == "Cash-in-hand")
                {
                    ViewBag.tCurBal = fh.CurrentBallance;
                    ViewBag.fCurBal = th.CurrentBallance;
                    //CloneObjects.CopyPropertiesTo(ft, ftm);
                    ftm.DateOfTransaction = ft.DateOfTransaction;
                    ftm.FromHeadID = ft.ToHeaID;
                    ftm.ToHeaID = ft.FromHeadID;
                    ftm.Credit = ft.Credit;
                    ftm.Description = ft.Description;
                    ftm.TRID = ft.TRID;
                }
                else
                {
                    //ft= fServise.GetFinTransById(ft.RelatedTrId);
                    ViewBag.tCurBal = th.CurrentBallance;
                    ViewBag.fCurBal = fh.CurrentBallance;
                    ftm.DateOfTransaction = ft.DateOfTransaction;
                    ftm.Description = ft.Description;
                    ftm.FromHeadID = ft.FromHeadID;
                    ftm.ToHeaID = ft.ToHeaID;
                    ftm.Credit = ft.Debit;
                    ftm.TRID = ft.RelatedTrId;
                }
            }
            else
            {
                ftm.DateOfTransaction = DateTime.Now;
            }
            return View(ftm);
        }
        public IActionResult SaveRecipt(FinanceTransactionsModel request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ProcessResponse response = new ProcessResponse();
            if(request.FromHeadID== null || request.ToHeaID == null)
            {
                response.statusCode = 5;
                response.statusMessage = "Please select from and to Heads";
                return Json(new { result = response });
            }
            if (request.Credit <= 0)
            {
                response.statusCode = 4;
                response.statusMessage = "Amount Canot be Negative or 0";
                return Json(new { result = response });
            }
            if (request.DateOfTransaction >= DateTime.Now)
            {
                response.statusCode = 3;
                response.statusMessage = "Date of Transaction can't be in future";
                return Json(new { result = response });
            }
            FinanceHeads fh = fServise.GetFinHeadById((int)request.FromHeadID);
            FinanceHeads th = fServise.GetFinHeadById((int)request.ToHeaID);
            if (request.TRID > 0)
            {
                request.Debit = request.Credit;
                FinanceTransactions oldft = fServise.GetFinTransById(request.TRID);
                FinanceTransactions oldft2 = fServise.GetFinTransById(oldft.RelatedTrId);
                oldft.DateOfTransaction = request.DateOfTransaction;
                oldft2.DateOfTransaction = request.DateOfTransaction;
                oldft.Description = request.Description;
                oldft2.Description = request.Description;
                if (oldft.ToHeaID == request.FromHeadID && oldft.FromHeadID == request.ToHeaID && oldft.Credit == request.Credit)
                {
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.FromHeadID && oldft.FromHeadID == request.ToHeaID && oldft.Credit == request.Credit)
                {
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance += oldft.Credit;
                    fServise.SaveFinHead(oldth);
                    fh.CurrentBallance -= oldft.Credit;
                    fServise.SaveFinHead(fh);
                    oldft.ToHeaID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.FromHeadID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.FromHeadID && oldft.FromHeadID != request.ToHeaID && oldft.Credit == request.Credit)
                {
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance -= oldft.Credit;
                    fServise.SaveFinHead(oldfh);
                    th.CurrentBallance += request.Credit;
                    fServise.SaveFinHead(th);
                    oldft.FromHeadID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.ToHeaID = request.ToHeaID;
                    }
                }
                else if (oldft.ToHeaID != request.FromHeadID && oldft.FromHeadID != request.ToHeaID && oldft.Credit == request.Credit)
                {
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance += oldft.Credit;
                    fServise.SaveFinHead(oldth);
                    th.CurrentBallance += oldft.Credit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance -= oldft.Credit;
                    fServise.SaveFinHead(oldfh);
                    fh.CurrentBallance -= oldft.Credit;
                    fServise.SaveFinHead(fh);
                    oldft.ToHeaID = request.FromHeadID;
                    oldft.FromHeadID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.FromHeadID;
                        oldft2.ToHeaID = request.ToHeaID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.FromHeadID && oldft.FromHeadID == request.ToHeaID && oldft.Credit != request.Credit)
                {
                    decimal diff = (decimal)(request.Credit - oldft.Credit);
                    fh.CurrentBallance -= diff;
                    fServise.SaveFinHead(fh);
                    th.CurrentBallance += diff;
                    fServise.SaveFinHead(th);
                    oldft.Credit = request.Credit;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.Debit = request.Credit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.FromHeadID && oldft.FromHeadID == request.ToHeaID && oldft.Credit != request.Credit)
                {
                    decimal diff = (decimal)(request.Credit - oldft.Credit);
                    th.CurrentBallance += diff;
                    fServise.SaveFinHead(th);
                    fh.CurrentBallance -= request.Credit;
                    fServise.SaveFinHead(fh);
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance += oldft.Credit;
                    fServise.SaveFinHead(oldth);
                    oldft.Credit = request.Credit;
                    oldft.ToHeaID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.FromHeadID;
                        oldft2.Debit = request.Credit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.FromHeadID && oldft.FromHeadID != request.ToHeaID && oldft.Credit != request.Credit)
                {
                    decimal diff = (decimal)(request.Credit - oldft.Credit);
                    th.CurrentBallance += request.Credit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance -= oldft.Credit;
                    fServise.SaveFinHead(oldfh);
                    fh.CurrentBallance -= diff;
                    fServise.SaveFinHead(fh);
                    oldft.Credit = request.Credit;
                    oldft.FromHeadID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.ToHeaID = request.ToHeaID;
                        oldft2.Debit = request.Credit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.FromHeadID && oldft.FromHeadID != request.ToHeaID && oldft.Credit != request.Credit)
                {
                    //decimal diff = (decimal)(request.Credit - oldft.Credit);
                    fh.CurrentBallance -= request.Credit;
                    fServise.SaveFinHead(fh);
                    th.CurrentBallance += request.Credit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance -= oldft.Credit;
                    fServise.SaveFinHead(oldfh);                    
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance += oldft.Credit;
                    fServise.SaveFinHead(oldth);
                    oldft.Credit = request.Credit;
                    oldft.ToHeaID = request.FromHeadID;
                    oldft.FromHeadID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.FromHeadID;
                        oldft2.ToHeaID = request.ToHeaID;
                        oldft2.Debit = request.Credit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
            }
            else
            {                
                int v = fServise.GetPreviousVoucherNumber();
                FinanceTransactions ft = new FinanceTransactions();
                CloneObjects.CopyPropertiesTo(request, ft);
                ft.VoucherType = "Receipt";
                ft.VoucherNumber = ++v;
                ft.Debit = 0;
                ft.Credit = request.Credit;
                ft.FromHeadID = request.ToHeaID;
                ft.ToHeaID = request.FromHeadID;
                ft.TTime = DateTime.Now;
                ft.DoneBy = loginCheckResponse.userName;
                //ft.IsDeleted = false;
                response = fServise.SaveFinTransactions(ft);
                if (response.statusCode == 1)
                {
                    FinanceTransactions f = new FinanceTransactions();
                    f.VoucherType = "Receipt";
                    f.DateOfTransaction = request.DateOfTransaction;
                    f.Description = request.Description;
                    f.VoucherNumber = ++v;
                    f.Debit = request.Credit;
                    f.Credit = 0;
                    f.ToHeaID = request.ToHeaID;
                    f.FromHeadID = request.FromHeadID;
                    f.TTime = DateTime.Now;
                    f.DoneBy = loginCheckResponse.userName;
                    f.RelatedTrId = response.currentId;
                    //ft.IsDeleted = false;
                    var response1 = fServise.SaveFinTransactions(f);
                    ft.RelatedTrId = response1.currentId;
                    var r5 = fServise.SaveFinTransactions(ft);

                    fh.CurrentBallance -= request.Credit;
                    var response2 = fServise.SaveFinHead(fh);
                    if (response2.statusCode == 1)
                    {
                        th.CurrentBallance += request.Credit;
                        response2 = fServise.SaveFinHead(th);
                    }
                }
            }
            return Json(new { result = response });
        }
        public IActionResult ContraEntry(int id=0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            FinanceTransactionsModel ftm = new FinanceTransactionsModel();
            
            ftm.fromHeadDrops = cServise.GetBankAndCashHeads();
            ftm.toHeadDrops = cServise.GetBankAndCashHeads();
            ViewBag.fCurBal = 0;
            ViewBag.tCurBal = 0;
            List<FHeadDrops> drps = cServise.GetFHeadDrops();
            //int i = 1;
            //foreach (FHeadDrops h in drps)
            //{
            //    if (h.HeadCode == "Bank" || h.HeadCode == "Cash")
            //    {
            //        if (i != 2)
            //        {
            //            ftm.toHeadDrops.Add(h);
            //        }
            //        if (i != 1)
            //        {
            //            ftm.fromHeadDrops.Add(h);
            //        }
            //        i++;
            //    }

            //}
            if (ftm.fromHeadDrops.Count > 1)
            {
                ftm.fromHeadDrops.RemoveAt(1);
            }
            if (ftm.fromHeadDrops.Count > 0)
            {
                ftm.toHeadDrops.RemoveAt(0);
                ViewBag.fCurBal = ftm.fromHeadDrops[0].CurrentBallance;
            }            
            if (ftm.toHeadDrops.Count > 0)
            {
                ViewBag.tCurBal = ftm.toHeadDrops[0].CurrentBallance;
            }
            if (id > 0)
            {
                FinanceTransactions ft = fServise.GetFinTransById(id);
                if (ft.TRID < ft.RelatedTrId)
                {
                    CloneObjects.CopyPropertiesTo(ft, ftm);
                    ftm.Credit = ftm.Debit;
                    ViewBag.isNew = "No";
                    FinanceHeads fh = fServise.GetFinHeadById((int)ft.FromHeadID);
                    FinanceHeads th = fServise.GetFinHeadById((int)ft.ToHeaID);
                    ViewBag.tCurBal = fh.CurrentBallance;
                    ViewBag.fCurBal = th.CurrentBallance;
                }
                else
                {
                    FinanceHeads fh = fServise.GetFinHeadById((int)ft.FromHeadID);
                    FinanceHeads th = fServise.GetFinHeadById((int)ft.ToHeaID);
                    ViewBag.tCurBal = th.CurrentBallance;
                    ViewBag.fCurBal = fh.CurrentBallance;
                    ftm.Credit = ft.Credit;
                    ftm.FromHeadID = ft.ToHeaID;
                    ftm.ToHeaID = ft.ToHeaID;
                    ftm.DateOfTransaction = ft.DateOfTransaction;
                    ftm.Description = ft.Description;
                    ftm.TRID = ft.RelatedTrId;
                }
                //List<FHeadDrops> newdrps = cServise.GetFHeadDrops();
                
                foreach (FHeadDrops h in drps)
                {
                    if (h.HeadCode == "Bank" || h.HeadCode == "Cash")
                    {
                        if (h.HeadId!=ft.FromHeadID)
                        {
                            ftm.toHeadDrops.Add(h);
                        }
                        if (h.HeadId!=ft.ToHeaID)
                        {
                            ftm.fromHeadDrops.Add(h);
                        }
                       
                    }

                }               
            }
            else
            {
                ftm.DateOfTransaction = DateTime.Now;
            }
            return View(ftm);
        }
        public IActionResult SaveContra(FinanceTransactionsModel request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ProcessResponse response = new ProcessResponse();
            if(request.FromHeadID==null || request.ToHeaID == null)
            {
                response.statusCode = 5;
                response.statusMessage = "Please select from and to Heads";
                return Json(new { result = response });
            }
            if (request.Credit <= 0)
            {
                response.statusCode = 4;
                response.statusMessage = "Amount Canot be Negative or 0";
                return Json(new { result = response });
            }
            if (request.DateOfTransaction >= DateTime.Now)
            {
                response.statusCode = 3;
                response.statusMessage = "Date of Transaction can't be in future";
                return Json(new { result = response });
            }
            FinanceHeads fh = fServise.GetFinHeadById((int)request.FromHeadID);
            FinanceHeads th = fServise.GetFinHeadById((int)request.ToHeaID);
            if (request.TRID > 0)
            {
                request.Debit = request.Credit;
                FinanceTransactions oldft = fServise.GetFinTransById(request.TRID);
                FinanceTransactions oldft2 = fServise.GetFinTransById(oldft.RelatedTrId);
                oldft.DateOfTransaction = request.DateOfTransaction;
                oldft2.DateOfTransaction = request.DateOfTransaction;
                oldft.Description = request.Description;
                oldft2.Description = request.Description;
                if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit == request.Debit)
                {
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit == request.Debit)
                {
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);
                    th.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(th);
                    oldft.ToHeaID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.ToHeaID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit == request.Debit)
                {
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    fh.CurrentBallance -= request.Debit;
                    fServise.SaveFinHead(fh);
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.ToHeaID = request.FromHeadID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit == request.Debit)
                {
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);
                    th.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    fh.CurrentBallance -= request.Debit;
                    fServise.SaveFinHead(fh);
                    oldft.ToHeaID = request.ToHeaID;
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.ToHeaID;
                        oldft2.ToHeaID = request.FromHeadID;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit != request.Debit)
                {
                    decimal diff = (decimal)(request.Debit - oldft.Debit);
                    fh.CurrentBallance -= diff;
                    fServise.SaveFinHead(fh);
                    th.CurrentBallance += diff;
                    fServise.SaveFinHead(th);
                    oldft.Debit = request.Debit;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.Credit = request.Debit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID == request.FromHeadID && oldft.Debit != request.Debit)
                {
                    decimal diff = (decimal)(request.Debit - oldft.Debit);
                    fh.CurrentBallance -= diff;
                    fServise.SaveFinHead(fh);
                    th.CurrentBallance += request.Debit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);
                    oldft.Debit = request.Debit;
                    oldft.ToHeaID = request.ToHeaID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.ToHeaID;
                        oldft2.Credit = request.Debit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID == request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit != request.Debit)
                {
                    decimal diff = (decimal)(request.Debit - oldft.Debit);
                    fh.CurrentBallance -= request.Debit;
                    fServise.SaveFinHead(fh);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    th.CurrentBallance += diff;
                    fServise.SaveFinHead(th);
                    oldft.Debit = request.Debit;
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.ToHeaID = request.FromHeadID;
                        oldft2.Credit = request.Debit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
                else if (oldft.ToHeaID != request.ToHeaID && oldft.FromHeadID != request.FromHeadID && oldft.Debit != request.Debit)
                {
                    decimal diff = (decimal)(request.Debit - oldft.Debit);
                    fh.CurrentBallance -= request.Debit;
                    fServise.SaveFinHead(fh);
                    FinanceHeads oldfh = fServise.GetFinHeadById((int)oldft.FromHeadID);
                    oldfh.CurrentBallance += oldft.Debit;
                    fServise.SaveFinHead(oldfh);
                    th.CurrentBallance += request.Debit;
                    fServise.SaveFinHead(th);
                    FinanceHeads oldth = fServise.GetFinHeadById((int)oldft.ToHeaID);
                    oldth.CurrentBallance -= oldft.Debit;
                    fServise.SaveFinHead(oldth);
                    oldft.Debit = request.Debit;
                    oldft.ToHeaID = request.ToHeaID;
                    oldft.FromHeadID = request.FromHeadID;
                    response = fServise.SaveFinTransactions(oldft);
                    if (response.statusCode == 1)
                    {
                        oldft2.FromHeadID = request.ToHeaID;
                        oldft2.ToHeaID = request.FromHeadID;
                        oldft2.Credit = request.Debit;
                        response = fServise.SaveFinTransactions(oldft2);
                    }
                }
            }
            else
            {
                int v = fServise.GetPreviousVoucherNumber();
                FinanceTransactions ft = new FinanceTransactions();
                CloneObjects.CopyPropertiesTo(request, ft);
                ft.VoucherType = "Contra";
                ft.VoucherNumber = ++v;
                ft.Debit = ft.Credit;
                ft.Credit = 0;
                ft.TTime = DateTime.Now;
                ft.DoneBy = loginCheckResponse.userName;
                //ft.IsDeleted = false;
                response = fServise.SaveFinTransactions(ft);
                if (response.statusCode == 1)
                {
                    FinanceTransactions f = new FinanceTransactions();
                    f.VoucherType = "Contra";
                    f.DateOfTransaction = request.DateOfTransaction;
                    f.Description = request.Description;
                    f.VoucherNumber = ++v;
                    f.Debit = 0;
                    f.Credit = request.Credit;
                    f.ToHeaID = request.FromHeadID;
                    f.FromHeadID = request.ToHeaID;
                    f.TTime = DateTime.Now;
                    f.DoneBy = loginCheckResponse.userName;
                    f.RelatedTrId = response.currentId;
                    //ft.IsDeleted = false;
                    var response1 = fServise.SaveFinTransactions(f);
                    ft.RelatedTrId = response1.currentId;
                    var r5 = fServise.SaveFinTransactions(ft);

                    fh.CurrentBallance -= request.Credit;
                    var response2 = fServise.SaveFinHead(fh);
                    if (response2.statusCode == 1)
                    {
                        th.CurrentBallance += request.Credit;
                        response2 = fServise.SaveFinHead(th);
                    }
                }
            }
            return Json(new { result = response });
        }
        public IActionResult LedgersDisplay(LedgerScreenModel req)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            FinanceHeads fh = fServise.GetFinHeadById(req.LedId);
            
            LedgerScreenModel res = new LedgerScreenModel();
            res.Ledgers = new List<FinanceTransactionsModel>();
            DateTime currentDate = DateTime.Now;
            ViewBag.ermsg = "";
            if (req.FromDate.Year <= 2020)
            {
                req.FromDate= currentDate.AddDays(-7);
            }
            if (req.ToDate.Year <= 2020)
            {
                req.ToDate = DateTime.Now; 
            }
            if (fh != null)
            {
                FinanceGroups fg = fServise.GetFinGroupById((int)fh.GroupId);
                if (fg.GroupName == "Sales Accounts")
                {
                    if (currentDate.Month <= 3)
                    {
                        req.FromDate = new DateTime((currentDate.Year - 1), 4, 1);
                        req.ToDate = new DateTime(currentDate.Year, 3, 31);
                    }
                    else
                    {
                        req.FromDate = new DateTime((currentDate.Year), 4, 1);
                        req.ToDate = new DateTime((currentDate.Year - 1), 3, 31);
                    }
                }
            }
            //if (req.ToDate > DateTime.Now)
            //{
            //    ViewBag.ermsg = "To Date canot exced Current Day";
            //}
            //else
            //{
                res.Ledgers = fServise.GetAllTransactions(req.LedId, req.FromDate, req.ToDate);
            //}
            if (req.LedId > 0)
            {
                res.CurrentBallance = (decimal)fServise.GetFinHeadById(req.LedId).CurrentBallance;
            }
            res.FromDate = req.FromDate;
            res.ToDate = req.ToDate;
            //if (req.LedId == 0)
            //{
            //    int cnt = res.Ledgers.Count;
            //    res.FromDate = (DateTime)res.Ledgers[cnt - 1].DateOfTransaction;
            //}
            
            ViewBag.heads = cServise.GetFHeadDrops();
            res.LedId = req.LedId;
            return View(res);
        }

        public IActionResult OpenOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<SaleOrdersForAccountsModel> myList = new List<SaleOrdersForAccountsModel>();
            myList = smService.GetOrdersSubmittedForAccounts();
            List<FinanceHeads> fHeads = smService.GetFinanceHeads();
            ViewBag.fHeads = fHeads; 
            return View(myList) ;
        }
        [HttpPost]
        public IActionResult SalesReciept(SalesReceipts request)
        {
            var result = smService.SaveSalesReceipt(request);
            return RedirectToAction("OpenOrders");
        }

        public IActionResult SOReceipts(int soid)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<SalesReceiptsModel> myList = new List<SalesReceiptsModel>();
            myList = smService.GetSalesReciepts(soid);
            return View(myList);
        }

        [HttpPost]
        public IActionResult PostToFinance(int trid)
        {
            var res = smService.PostReceiptToFinance(trid);
            return Ok(new { result = res });
        }
        public IActionResult GetSOReceiptPartial(int trid,int soid)
        {
            SalesReceiptsModel myList = new  SalesReceiptsModel();
            myList = smService.GetSalesReciepts(soid).Where(a=>a.TRId == trid).FirstOrDefault();
            myList.companyDetails = smService.GetCompany();
            return PartialView("_SOReceipt", myList);
        }

        public IActionResult GetFinaniceReceiptPartial(int trid)
        {
            FinanceTransactionsModel myList = new FinanceTransactionsModel();
            myList =  fServise.GetFTransactionByTrId(trid);
            return PartialView("_FinanceVoucher", myList);
        }
        public IActionResult GetPaymentPartial(int trid)
        {
            FinanceTransactionsModel myList = fServise.GetPayMentModelByTrId(trid);
            return PartialView("_PaymentReceipt", myList);
        }
        public IActionResult GetReceiptPartial(int trid)
        {
            FinanceTransactionsModel myList = fServise.GetReceiptModelByTrId(trid);
            return PartialView("_ReceiptVoucher", myList);
        }

        public IActionResult TransactionDayBook(LedgerScreenModel req)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            DateTime crdt = DateTime.Now;
            if (req.ToDate > DateTime.Now)
            {
                ViewBag.ermsg = "To Date canot exced Current Day";
                req.ToDate = crdt;
            }
            if (req.FromDate.Year <= 2020 || req.ToDate.Year <= 2020)
            {
                req.FromDate =crdt;
                req.ToDate = crdt;
            }
            
           
            LedgerScreenModel res = new LedgerScreenModel();
            res.Ledgers= fServise.GetTodayTransactions(req.FromDate, req.ToDate);
            res.ToDate = req.ToDate;
            res.FromDate = req.FromDate;
            return View(res);
        }
        public IActionResult PrintCompleteTransaction(DateTime from,DateTime to)
        {
            LedgerScreenModel res = new LedgerScreenModel();
            res.FromDate = from;
            res.ToDate = to;
            res.Ledgers = fServise.GetTodayTransactions(from, to);
            res.companyDetails = companyService.GetFirstCompany();
            return View(res);
        }
        public IActionResult PrintLedgerTransaction(DateTime from, DateTime to, int id = 0)
        {
            LedgerScreenModel res = new LedgerScreenModel();
            res.FromDate = from;
            res.ToDate = to;
            res.Ledgers = fServise.GetAllTransactions(id, from, to);
            res.companyDetails = companyService.GetFirstCompany();
            FinanceHeads fh = fServise.GetFinHeadById(id);
            res.headName = fh.HeadName;
            FinanceGroups fg = fServise.GetFinGroupById((int)fh.GroupId);
            if (fg.GroupName == "Sales Accounts")
            {
                AddressMaster am = cusService.GetFirstAddressOfCustomer((int)fh.CustomerId);
                res.headAddress = am.HouseNumber + ", " + am.StreetName + ", " + am.Location;
                res.headAddress += cServise.GetCityNameOfId((int)am.CityId)+",";
                res.headAddress += cServise.GetStateNameOfId((int)am.StateId)+",";
                res.headAddress += cServise.GetCountryNameOfId((int)am.CountryId)+",";
                res.headAddress += am.PostalCode;
            }else if(fg.GroupName == "Salaries")
            {
                res.headAddress = "Salaries";
            }
            else
            {
                res.headAddress = "";
            }
            return View(res);
        }
        public IActionResult PaymentVoucher()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            Payments p = new Payments();
            return View(p);
        }
        [HttpPost]
        public IActionResult PaymentVoucher(Payments req)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                req.DoneBy = loginCheckResponse.userName;
                req.DoneOn = DateTime.Now;
                req.IsDeleted = false;
                ProcessResponse pr = fServise.SavePayment(req);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("PaymentsList");
                }
                else
                {
                    return View(req);
                }
            }
            return View(req);
        }
        public IActionResult PaymentsList(DateTime from, DateTime to)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            DateTime curre = DateTime.Now;
            ViewBag.ermsg = "";
            List<Payments> res = new List<Payments>();
            if (from.Year < 2020)
            {
                from = new DateTime(curre.Year, curre.Month, curre.Day);
                to = from.AddDays(1);
            }
            if (to.Year < 2020)
            {
                to = curre.AddDays(1);
            }
            else
            {
                to = to.AddDays(1);
            }
            if (to > from)
            {
                res = fServise.GetAllPayments(from, to);
            }
            else
            {
                ViewBag.ermsg = "Please Select correct Dates";
            }
            ViewBag.frDt = from;
            ViewBag.toDt = to;
            return View(res);
        }
        public IActionResult DeletePayment(int id)
        {
            ProcessResponse pr= fServise.DeletePayment(id);
            return Json(new { res = pr });
        }
        public IActionResult ReceiptVoucher()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            Recipts r = new Recipts();
            return View(r);
        }
        [HttpPost]
        public IActionResult ReceiptVoucher(Recipts req)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                req.DoneBy = loginCheckResponse.userName;
                req.DoneOn = DateTime.Now;
                req.IsDeleted = false;
                ProcessResponse pr = fServise.SaveRecipt(req);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("ReciptsList");
                }
                else
                {
                    return View(req);
                }
            }
            return View(req);
        }
        public IActionResult ReciptsList(DateTime from, DateTime to)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            DateTime curre = DateTime.Now;
            ViewBag.ermsg = "";
            List<Recipts> res = new List<Recipts>();
            if (from.Year < 2020)
            {
                from = new DateTime(curre.Year, curre.Month, curre.Day);
                to = from.AddDays(1);
            }
            if (to.Year < 2020)
            {
                to = curre.AddDays(1);
            }
            else
            {
                to = to.AddDays(1);
            }
            if (to > from)
            {
                res = fServise.GetAllReceipts(from, to);
            }
            else
            {
                ViewBag.ermsg = "Please Select correct Dates";
            }
            ViewBag.frDt = from;
            ViewBag.toDt = to;
            return View(res);
        }
        public IActionResult DeleteReceipt(int id)
        {
            ProcessResponse pr = fServise.DeleteReceipt(id);
            return Json(new { res = pr });
        }
    }
}