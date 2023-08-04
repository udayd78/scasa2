using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCASA.Models.ModelClasses;
using Microsoft.Extensions.Logging;
using SCASA.Models.Services;
using SCASA.Models.Utilities;

namespace SCASA.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly ILogger<AuthenticateController> _logger;
       // private readonly INotificationService _nService;
        private readonly IUserMgmtService _uService;
        private readonly INotificationService _nService;

        public AuthenticateController(ILogger<AuthenticateController> logger,
            IUserMgmtService uService, INotificationService nService)
        {
            _logger = logger; 
            _uService = uService;
            _nService = nService;

        }

        public IActionResult Login(string url = "EmpireHome/Dashboard")
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
               loginCheckResponse.userName = "NA"; 
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.url = url;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            ViewBag.url = request.url;

            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
               loginCheckResponse.userName = "NA"; 
            }
            ViewBag.LoggedUser = loginCheckResponse;

            if (string.IsNullOrEmpty(request.emailid) || string.IsNullOrEmpty(request.pword))
            {
                ViewBag.ErrorMessage = "Fill Mandatory fields";
                return View(request);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var loginCheck = _uService.LoginCheck(request);
                    if (loginCheck.statusCode == 1)
                    {
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "loggedUser", loginCheck);
                        if(loginCheck.userTypeName == "Sales Executive")
                        {
                            return RedirectToAction("Index", "Sales");
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "EmpireHome");
                        }
                        
                    }
                    else
                    {
                        ViewBag.ErrorMessage = loginCheck.statusMessage;
                        //  ViewBag.CaptchaKey = "6LeplcYUAAAAAJlmUhStKiuJ6ucEqdotoWTYomZf";
                        ViewBag.src = request.url;
                        return View(request);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid emailid / mobile number or password";
                    // ViewBag.CaptchaKey = "6LeplcYUAAAAAJlmUhStKiuJ6ucEqdotoWTYomZf";
                    ViewBag.src = request.url;
                    return View(request);
                }
            }
        }

        public IActionResult Logout()
        {
            LoginResponse lr = new LoginResponse();
            lr.userId = 0;
            lr.userName = "NA";
            lr.userTypeName = "";
            lr.emailId = "";
            SessionHelper.SetObjectAsJson(HttpContext.Session, "loggedUser", lr);
            return RedirectToAction("Login", "Authenticate");
         

        }

        public IActionResult RequestOTP()
        {

            return View();
        }

        [HttpPost]
        public IActionResult RequestOTP(LoginRequestForPWChange request)
        {
            if(ModelState.IsValid)
            {
                ProcessResponse ps = _uService.InitiateResetPassword(request.emailId);
                if(ps.statusCode == 1)
                {
                  
                    

                    return RedirectToAction("ResetPassword", new { cc=ps.currentId});
                }
                else
                {
                    ViewBag.ErrorMessage = ps.statusMessage;
                }
            }

            return View(request);
        }

        public IActionResult ResetPassword(int cc)
        {
            PasswordChangeRequest request= new PasswordChangeRequest();
            request.UserId = cc;
            return View(request);
        }

        [HttpPost]
        public IActionResult ResetPassword(PasswordChangeRequest request)
        {
            if(ModelState.IsValid)
            {
                ProcessResponse ps = _uService.CompletePasswordRequest(request.key, request.UserId,request.password);
                if(ps.statusCode == 1)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.ErrorMessage = ps.statusMessage;
                }
            }
            return View(request);
        }
    }
}
