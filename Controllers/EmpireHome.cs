using Microsoft.AspNetCore.Mvc;
using Quartz;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Controllers
{
    public class EmpireHome : Controller
    {
        private readonly IReportsMgmtService rService;
        private readonly INotificationService _nService;
        public EmpireHome(IReportsMgmtService _rService, INotificationService nService)
        {
            rService = _rService;
            _nService = nService;
        }
        public IActionResult Dashboard()
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
            DashBoardClass final = new DashBoardClass();
            final.FinalValues = new DashBoardValuesList();
            final.dailyReport = rService.GetDailyReports();
            final.weeklyReport = rService.GetWeeklyReports();
            var monrep= rService.GetMonthlyReports();
            final.MonthlyReport = monrep;
            
            DashBoardValuesList listvalues = rService.GetDynamicValues();
            final.FinalValues.SalesTotal = monrep.Sum(a => a.salesValue);
            final.FinalValues.collectionTotal = listvalues.collectionTotal; ;
            final.FinalValues.delivered = listvalues.delivered;
            final.FinalValues.expendeture = listvalues.expendeture;
            final.FinalValues.dailyReceipt = listvalues.dailyReceipt;
            final.FinalValues.weeklyReceipt = listvalues.weeklyReceipt;            
            return View(final);
        }

        public IActionResult SalesReport()
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
            return View();
        }
        public IActionResult FinanceReport()
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
            return View();
        }

        public IActionResult SaleExecutiveReport()
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
            return View();
        }
        public IActionResult MonthlySalesProductWise()
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
            return View();
        }

        public IActionResult DailySalesReport()
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
            return View();
        }

        public IActionResult SalesExecutiveCharts()
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

            return View();
        }
        public IActionResult CustomerReports()
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
            return View();
        }

        public IActionResult HeadWise()
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
            return View();
        }
        public IActionResult DailyReports()
        {
            _nService.SendDailyReport();
            return RedirectToAction("Dashboard");
        }
    }
}