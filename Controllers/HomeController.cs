using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using SCASA.Models;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCASA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserMgmtService _uService;
        private readonly ICommonService cService;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IUserMgmtService uService, ICommonService _cSer, IHostingEnvironment environement)
        {
            _logger = logger;
            _uService = uService;
            cService = _cSer;
            hostingEnvironment = environement;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetMyNotifications(int uid)
        {
            List<ActivityLog> pm = new List<ActivityLog>();

            pm = _uService.GetMyActivityLogs(uid);
            return PartialView("_NotificationList", pm);
        }
        public IActionResult GetMenuItems(int uid)
        {
            int uType = _uService.GetUserTypeCode(uid);
            if (uType > 0 && uType < 5)
            {
                // admin, super admin, MD, CEO
                return PartialView("_AdminMenu");
            }
            else if (uType == 5)
            {
                // sales head
                return PartialView("_SalesHeadMenu");
            }
            else if (uType == 6)
            {
                //Accountant
                return PartialView("_AccountsMenu");
            }
            else if (uType == 7)
            {
                //Accountant
                return PartialView("_NoMenu");
            }
            else if (uType == 8)
            {
                //Wharehouse
                return PartialView("_WarehouseMenu");
            }
            else if (uType == 9)
            {
                //Wharehouse
                return PartialView("_HRMenu");
            }
            else
            {
                return PartialView("_NoMenu");
            }

        }
        public IActionResult profile()
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
            UserProfileModel finalObject = _uService.GetUserShortProfileById(loginCheckResponse.userId);

            finalObject.Logintime = _uService.GetLastLoginDetails(loginCheckResponse.userId);
            finalObject.countryDrops = cService.GetAllCountries();
            finalObject.stateDrops = cService.GetAllStates((int)finalObject.CountryId);
            finalObject.cityDrops = cService.GetAllCities((int)finalObject.StateId);
            return View(finalObject);

        }
        [HttpPost]
        public IActionResult profile(UserProfileModel request)
        {
            UserMaster testmaster = new UserMaster();
            testmaster = _uService.GetUserByEmail(request.EmailId);
            if (testmaster != null)
            {
                if (testmaster.UserId != request.UserId)
                {
                    ModelState.AddModelError("EmailId", "Email Already Used");
                }
            }
            testmaster = _uService.GetUserByEmail(request.MobileNumber);
            if (testmaster != null)
            {
                if (testmaster.UserId != request.UserId)
                {
                    ModelState.AddModelError("MobileNumber", "Number Already Used");
                }
            }
            if (ModelState.IsValid)
            {
                UserMaster um = _uService.GetUserById(request.UserId);
                um.UserName = request.UserName;
                um.EmailId = request.EmailId;
                um.MobileNumber = request.MobileNumber;
                um.Address1 = request.Address1;
                um.Address2 = request.Address2;
                um.CountryId = request.CountryId;
                um.StateId = request.StateId;
                um.CityId = request.CityId;
                um.ZipCode = request.ZipCode;
                um.LastModifiedBy = um.UserName;
                um.LastModifiedOn = DateTime.Now;
                ProcessResponse pr = _uService.SaveUser(um);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Dashboard", "EmpireHome");
                }
            }
            request.countryDrops = cService.GetAllCountries();
            request.stateDrops = cService.GetAllStates((int)request.CountryId);
            request.cityDrops = cService.GetAllCities((int)request.StateId);
            return View(request);
        }
        public IActionResult Submit()
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
            UserMaster um = _uService.GetUserById(loginCheckResponse.userId);
            UserMasterModel finalObject = new UserMasterModel();
            CloneObjects.CopyPropertiesTo(um, finalObject);
            finalObject.countryDrops = cService.GetAllCountries(); ;
            finalObject.stateDrops = cService.GetAllStates(finalObject.countryDrops[0].CountryId);
            finalObject.cityDrops = cService.GetAllCities(finalObject.stateDrops[0].StateId);
            return View(finalObject);
        }
        [HttpPost]
        public IActionResult Submit(UserMasterModel request)
        {
            if (request.UserId > 0)
            {
                var emialcheck = _uService.GetUserByEmail(request.EmailId, request.UserId);
                if (emialcheck != null)
                {
                    ModelState.AddModelError("EmailId", "Email id not available");
                }
                var emialcheck1 = _uService.GetUserByEmail(request.MobileNumber, request.UserId);
                if (emialcheck1 != null)
                {
                    ModelState.AddModelError("MobileNumber", "Mobile Number not available");
                }
                if (ModelState.IsValid)
                {
                    bool isProfileImageUploaded = false;
                    string profileImageName = "";
                    if (request.ProfileImageUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.ProfileImageUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var contentType = request.ProfileImageUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.ProfileImageUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            profileImageName = filename;
                            isProfileImageUploaded = true;

                        }
                        else
                        {
                            profileImageName = "dummy.png";
                        }
                    }
                    else
                    {
                        profileImageName = "dummy.png";
                    }
                    bool isPanCardUploaded = false;
                    string PanCardName = "";
                    if (request.PanCardUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.PanCardUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.PanCardUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.PanCardUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            PanCardName = filename;
                            isPanCardUploaded = true;
                        }
                    }
                    bool isAdharCardUploaded = false;
                    string AdharCardName = "";
                    if (request.AadharCardUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.AadharCardUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.AadharCardUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.AadharCardUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            AdharCardName = filename;
                            isAdharCardUploaded = true;
                        }
                    }
                    bool isAdharBackUploaded = false;
                    string AadharBackName = "";
                    if (request.AadharBackUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.AadharBackUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.AadharBackUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.AadharBackUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            AadharBackName = filename;
                            isAdharBackUploaded = true;
                        }
                    }
                    bool isResumeDocUploaded = false;
                    string ResumeDocName = "";
                    if (request.ResumeUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.ResumeUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.ResumeUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.ResumeUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            ResumeDocName = filename;
                            isResumeDocUploaded = true;

                        }
                    }
                    bool isSalarySlipUploaded = false;
                    string SalarySlipName = "";
                    if (request.SalarySlipUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.SalarySlipUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.SalarySlipUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.SalarySlipUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            SalarySlipName = filename;
                            isSalarySlipUploaded = true;

                        }
                    }
                    bool isPreviousRelievingLetterUploaded = false;
                    string PreviousRelievingLetterName = "";
                    if (request.PreviousRelievingLetterUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.PreviousRelievingLetterUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.PreviousRelievingLetterUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.PreviousRelievingLetterUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            PreviousRelievingLetterName = filename;
                            isPreviousRelievingLetterUploaded = true;

                        }
                    }

                    UserMaster um = _uService.GetUserById(request.UserId);
                    if (um != null)
                    {
                        um.UserName = request.UserName;
                        um.EmailId = request.EmailId;
                        um.MobileNumber = request.MobileNumber;
                        um.DateOfBirth = request.DateOfBirth;
                        um.Gender = request.Gender;
                        um.Address1 = request.Address1;
                        um.Address2 = request.Address2;
                        um.CountryId = request.CountryId;
                        um.StateId = request.StateId;
                        um.CityId = request.CityId;
                        um.ZipCode = request.ZipCode;
                        um.ReferredbyName = request.ReferredbyName;
                        um.ReferredByMobile = request.ReferredByMobile;
                        um.EmergencyContactNumber = request.EmergencyContactNumber;
                        um.AadharNumber = PasswordEncryption.Encrypt(request.AadharNumber);
                        um.PANNumber = request.PANNumber;
                        um.PreviousOrgName = request.PreviousOrgName;
                        um.PreviousOrgAddress = request.PreviousOrgAddress;
                        um.PreviousOrgExperience = request.PreviousOrgExperience;
                        um.TotalPreviousExpeience = request.TotalPreviousExpeience;
                        um.ProfileImage = isProfileImageUploaded ? profileImageName : null;
                        um.PanCard = isPanCardUploaded ? PanCardName : null;
                        um.AdharCard = isAdharCardUploaded ? AdharCardName : null;
                        um.AadharBack = isAdharBackUploaded ? AadharBackName : null;
                        um.ResumeDoc = isResumeDocUploaded ? ResumeDocName : null;
                        um.SalarySlip = isSalarySlipUploaded ? SalarySlipName : null;
                        um.PreviousRelievingLetter = isPreviousRelievingLetterUploaded ? PreviousRelievingLetterName : null;
                        um.BankName = PasswordEncryption.Encrypt(request.BankName);
                        um.AccountNumber = PasswordEncryption.Encrypt(request.AccountNumber);
                        um.AccountHolderName = PasswordEncryption.Encrypt(request.AccountHolderName);
                        um.IfscCode = PasswordEncryption.Encrypt(request.IfscCode);
                        um.BranchAddress = PasswordEncryption.Encrypt(request.BranchAddress);
                        um.UPINumber = PasswordEncryption.Encrypt(request.UPINumber);
                        return RedirectToAction("Dashboard", "EmpireHome");
                    }
                    else
                    {
                        request.countryDrops = cService.GetAllCountries(); ;
                        request.stateDrops = cService.GetAllStates(request.countryDrops[0].CountryId);
                        request.cityDrops = cService.GetAllCities(request.stateDrops[0].StateId);
                        return View(request);
                    }
                }
                else
                {
                    request.countryDrops = cService.GetAllCountries(); ;
                    request.stateDrops = cService.GetAllStates(request.countryDrops[0].CountryId);
                    request.cityDrops = cService.GetAllCities(request.stateDrops[0].StateId);
                    return View(request);
                }
            }
            else
            {
                request.countryDrops = cService.GetAllCountries(); ;
                request.stateDrops = cService.GetAllStates(request.countryDrops[0].CountryId);
                request.cityDrops = cService.GetAllCities(request.stateDrops[0].StateId);
                return View(request);
            }
        }
        public IActionResult CheckForProfile()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            UserMaster um = _uService.GetUserById(loginCheckResponse.userId);
            if (um.Address1 == null)
            {
                return RedirectToAction("Submit");
            }
            else
            {
                return RedirectToAction("Profile");
            }
        }
    }
    public class TestJob1 : IJob
    {
        private readonly INotificationService _nService;
        public TestJob1(INotificationService nService)
        {
            _nService = nService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _nService.SendDailyReport();
            return Task.CompletedTask;
        }
    }
    public class TestJob2 : IJob
    {
        private readonly INotificationService _nService;
        public TestJob2(INotificationService nService)
        {
            _nService = nService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _nService.SendWeaklyReport();
            return Task.CompletedTask;
        }
    }
}
