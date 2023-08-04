using SCASA.Models.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Rotativa.AspNetCore;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SCASA.Controllers
{
	public class StaffController : Controller
	{
        private readonly IUserMgmtService uService;
        private readonly ICommonService cService;
        private readonly IpayRollMgmtService pSercice;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly INotificationService nService;
        private readonly IAttendanceMgmtService stfService;
        private readonly IMonthlyPayrollService mprService;
        private readonly IConfiguration _config;
        private readonly IStaffLoansMgmtService sService;
        private readonly IFinanceMgmtService fService;

        public StaffController(IUserMgmtService _uServices, IpayRollMgmtService _pService,
            ICommonService _cService, IHostingEnvironment environement, INotificationService _nService, IAttendanceMgmtService _stfService,
            IMonthlyPayrollService _mprService,IConfiguration confg, IStaffLoansMgmtService _s , IFinanceMgmtService _f)
        {
            uService = _uServices;
            cService = _cService;
            pSercice = _pService;
            hostingEnvironment = environement;
            nService = _nService;
            stfService = _stfService;
            mprService = _mprService;
            _config = confg;
            sService = _s;
            fService = _f;
        }
        public IActionResult StaffList(string uData="")
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

            List<UserMasterDisplay> myList = new List<UserMasterDisplay>();
            myList = uService.GetAllUsers(uData);
             
            return View(myList);
        }
        public IActionResult ResendCredentials(int id)
        {
            UserMaster um = uService.GetUserById(id);
            string pwd = PasswordEncryption.Decrypt(um.PWord);
            var pr = nService.ResendCredentialsToEmployee(AppSettings.EmailTemplates.ResendLogInDetails, um.EmailId, um.UserName, um.EmailId, pwd);
            return Json(new { result = pr });
        }
        public IActionResult NewStaffList(string uData = "")
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

            List<NewStaffListDisplayModel> myList = new List<NewStaffListDisplayModel>();
            myList = uService.GetNewUsers(uData);
            if (string.IsNullOrEmpty( uData))
            {
                ViewBag.emsg = "No Data Exists";
            }
            else
            {
                ViewBag.emsg = "No Data Found";
            }
            ViewBag.ShiftDrops = cService.GetShiftDrops();
            List<StaffDrop> Rh = cService.GetStaffDropsForReportingHead(5);
            foreach(StaffDrop u in Rh)
            {
                u.StaffName += "(" + u.EmployeeCode + ")";
            }
            ViewBag.RepHeadDrps = Rh;
            return View(myList);
        }
        public IActionResult MakeActive(int id,int shifId=0,int repToId=0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            UserMaster um = uService.GetUserById(id);
            string oldStatus = um.CurrentStatus;
            
            um.CurrentStatus = "Active";
            string password = RandomGenerator.RandomString(8, false);
            um.PWord = PasswordEncryption.Encrypt(password);
            um.LastModifiedOn = DateTime.Now;
            um.LastModifiedBy = loginCheckResponse.userName;
            
            if (shifId != 0)
            {
                um.DateOfJoin = DateTime.Now;
                um.ShiftId = shifId;
                um.ProfileImage = "dummy.png";
                um.MaxDiscoutPercentage = 0;
            }
            if (repToId == 0)
            {
                if (um.ReportingManager > 0)
                {
                    um.ReportingManager = um.ReportingManager;
                }
                else
                {
                    List<StaffDrop> Rh = cService.GetStaffDropsForReportingHead(3);
                    um.ReportingManager = Rh[0].StaffId;
                }                
            }
            else
            {
                um.ReportingManager = repToId;
            }
            var PR = uService.SaveUser(um);
            if (PR.statusCode == 1 && (oldStatus == "New" ||oldStatus== "SubmittedForReview"))
            {
                PayRollMaster npm = new PayRollMaster();
                npm= pSercice.GetPayRollByUserId(id);
                if (npm == null)
                {
                    npm.BasicSalary = 0;
                    npm.HRA = 0;
                    npm.DearnessAllowance = 0;
                    npm.FoodAllowance = 0;
                    npm.Conveyance = 0;
                    npm.MedicalAllowances = 0;
                    npm.OtherAllowances = 0;
                    npm.ProvidentFund = 0;
                    npm.ProfessionalTax = 0;
                    npm.ESIFund = 0;
                    npm.TDS = 0;
                    npm.StaffId = id;
                    npm.CreatedOn = DateTime.Now;
                    npm.CreatedBy = loginCheckResponse.userId;
                    pSercice.SavePayRoll(npm);
                }
            }
            um = uService.GetUserById(id);
            string adminemail = _config.GetValue<string>("EmailConfig:adminEmail");
            string adminPersonName = _config.GetValue<string>("EmailConfig:adminPersonName");
            UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
            var eTA = nService.SendAcceptEmpNotificationToAdmin(AppSettings.EmailTemplates.AcceptEmpNotificationToAdmin, adminemail, adminPersonName, um.UserName, utm.TypeName, (DateTime)um.DateOfJoin);

            UserMaster rh = uService.GetUserById((int)um.ReportingManager);
            var eTRH = nService.SendJoiningNotificationToReportingHead(AppSettings.EmailTemplates.JoiningNotificationToReportingHead, rh.EmailId, rh.UserName, um.UserName, utm.TypeName, (DateTime)um.DateOfJoin);
            string pwd = PasswordEncryption.Decrypt(um.PWord);
            //var rscr = nService.ResendCredentialsToEmployee(AppSettings.EmailTemplates.ResendLogInDetails, um.EmailId, um.UserName, um.EmailId, pwd);
            var eTE = nService.SendPasswordToEmployee(AppSettings.EmailTemplates.PasswordToEmployee, um.EmailId, um.UserName, password);
            return Json(new {result=PR });
        }
        public IActionResult AddNewStaff()
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

            ViewBag.userTypeDrops = cService.GetUserTypeDrops();
            CreateNewStaffModel response = new CreateNewStaffModel();
            return View(response);
        }
        [HttpPost]
        public IActionResult AddNewStaff(CreateNewStaffModel request)
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

            var emialcheck = uService.GetUserByEmail(request.EmailId);
            if (emialcheck != null)
            {
                ModelState.AddModelError("EmailId", "Email id not available");
            }
            var emialcheck1 = uService.GetUserByEmail(request.MobileNumber);
            if (emialcheck1 != null)
            {
                ModelState.AddModelError("MobileNumber", "Mobile Number not available");
            }
            try
            {
                long mbno = long.Parse(request.MobileNumber);
            }
            catch
            {
                ModelState.AddModelError("MobileNumber", "Mobile Number Should Contain only numbers");
            }
            
            if (ModelState.IsValid)
            {
                UserMaster um = new UserMaster();
                CloneObjects.CopyPropertiesTo(request, um);
                ProcessResponse response = new ProcessResponse();
                
                um.CurrentStatus = "New";
                um.CreatedBy = loginCheckResponse.userName;
                um.CreatedOn = DateTime.Now;
                um.LastModifiedBy = loginCheckResponse.userName;
                um.LastModifiedOn = DateTime.Now;
                
                um.ReleivedOn = null;
                response = uService.SaveUser(um);

                if (response.statusCode == 1)
                {
                    return RedirectToAction("NewStaffList");
                }
                else
                {
                    ViewBag.ErrorMessage = response.statusMessage;
                }
            }
            ViewBag.userTypeDrops = cService.GetUserTypeDrops();

            return View(request);
        }
        public IActionResult UpdateUser(int id = 0)
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

            UserMaster myObj = new UserMaster();
            PayRollMaster pObj = new PayRollMaster();

            UserMasterModel finalObject = new UserMasterModel();
            
            if (id > 0)
            {
                myObj = uService.GetUserById(id);
                finalObject.DateOfJoin = myObj.DateOfJoin;
                finalObject.DateOfBirth = myObj.DateOfBirth;
                
                CloneObjects.CopyPropertiesTo(myObj, finalObject);
                finalObject.AadharNumber = myObj.AadharNumber != null ?  PasswordEncryption.Decrypt(myObj.AadharNumber) : "";
                finalObject.BankName = myObj.BankName   != null ?  PasswordEncryption.Decrypt(myObj.BankName) : "";
                finalObject.AccountNumber = myObj.AccountNumber != null ?  PasswordEncryption.Decrypt(myObj.AccountNumber) : "";
                finalObject.AccountHolderName = myObj.AccountHolderName != null ?  PasswordEncryption.Decrypt(myObj.AccountHolderName) : "";
                finalObject.IfscCode =  PasswordEncryption.Decrypt(myObj.IfscCode);
                finalObject.BranchAddress = PasswordEncryption.Decrypt(myObj.BranchAddress);
                finalObject.UPINumber = PasswordEncryption.Decrypt(myObj.UPINumber);
                //finalObject.AadharCardUploaded = myObj.AdharCard as IFormFile;


                pObj = pSercice.GetPayRollByUserId(id);
                if (pObj != null)
                {
                    finalObject.BasicSalary = pObj.BasicSalary;
                    finalObject.HRA = pObj.HRA;
                    finalObject.DearnessAllowance = pObj.DearnessAllowance;
                    finalObject.FoodAllowance = pObj.FoodAllowance;
                    finalObject.Conveyance = pObj.Conveyance;
                    finalObject.MedicalAllowances = pObj.MedicalAllowances;
                    finalObject.OtherAllowances = pObj.OtherAllowances;
                    finalObject.ProvidentFund = pObj.ProvidentFund;
                    finalObject.ProfessionalTax = pObj.ProfessionalTax;
                    finalObject.ESIFund = pObj.ESIFund;
                    finalObject.TDS = pObj.TDS;
                    finalObject.PRMId = pObj.PRMId;
                }
                //else
                //{
                //    pObj.BasicSalary = 0;
                //    pObj.Conveyance = 0;
                //    pObj.CreatedBy = loginCheckResponse.userId;
                //    pObj.CreatedOn = DateTime.Now;
                //    pObj.DearnessAllowance = 0;
                //    pObj.ESIFund = 0;
                //    pObj.FoodAllowance = 0;
                //    pObj.HRA = 0;
                //    pObj.IsDeleted = false;
                //    pObj.MedicalAllowances = 0;
                //    pObj.OtherAllowances = 0;
                //    pObj.ProfessionalTax = 0;
                //    pObj.ProvidentFund = 0;
                //    pObj.StaffId = id;
                //    pObj.TDS = 0;
                //    pSercice.SavePayRoll(pObj);
                //    pObj = pSercice.GetPayRollByUserId(id);
                //    finalObject.BasicSalary = pObj.BasicSalary;
                //    finalObject.HRA = pObj.HRA;
                //    finalObject.DearnessAllowance = pObj.DearnessAllowance;
                //    finalObject.FoodAllowance = pObj.FoodAllowance;
                //    finalObject.Conveyance = pObj.Conveyance;
                //    finalObject.MedicalAllowances = pObj.MedicalAllowances;
                //    finalObject.OtherAllowances = pObj.OtherAllowances;
                //    finalObject.ProvidentFund = pObj.ProvidentFund;
                //    finalObject.ProfessionalTax = pObj.ProfessionalTax;
                //    finalObject.ESIFund = pObj.ESIFund;
                //    finalObject.TDS = pObj.TDS;
                //    finalObject.PRMId = pObj.PRMId;
                //}
                if (finalObject.MaxDiscoutPercentage == null)
                {
                    finalObject.MaxDiscoutPercentage = 0;
                }
            }
            finalObject.userTypeDrops = cService.GetUserTypeDrops();
            finalObject.countryDrops = cService.GetAllCountries();
            int countryId = id > 0 ?  (int) finalObject.CountryId : 101;
            finalObject.stateDrops = cService.GetAllStates(countryId);
            int stateId = id > 0 ? finalObject.StateId : finalObject.stateDrops[0].StateId;
            finalObject.cityDrops = cService.GetAllCities(stateId);
            finalObject.ShiftDrops = cService.GetShiftDrops();
            finalObject.StaffDrops = cService.GetStaffDropsForReportingHead((int)finalObject.UserTypeId);
                       
            return View(finalObject);
        }
        [HttpPost]
        public IActionResult UpdateUser(UserMasterModel request)
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
            if(request.UserId > 0)
            {
                var emialcheck = uService.GetUserByEmail(request.EmailId,request.UserId);
                if (emialcheck != null)
                {
                    ModelState.AddModelError("EmailId", "Email id not available");
                }
                var emialcheck1 = uService.GetUserByEmail(request.MobileNumber, request.UserId);
                if (emialcheck1 != null)
                {
                    ModelState.AddModelError("MobileNumber", "Mobile Number not available");
                }
                var u = uService.GetUserById(request.UserId);                
                if (request.DateOfJoin == null)
                {
                    ModelState.AddModelError("DateOfJoin", "PleaseSelect Date Of Join");
                }else if (request.DateOfJoin < u.CreatedOn)
                {
                    ModelState.AddModelError("DateOfJoin", "Can't Select Previous Date");
                }
            }
            else
            {
                var emialcheck = uService.GetUserByEmail(request.EmailId);
                if (emialcheck != null)
                {
                    ModelState.AddModelError("EmailId", "Email id not available");
                }
                var emialcheck1 = uService.GetUserByEmail(request.MobileNumber);
                if (emialcheck1 != null)
                {
                    ModelState.AddModelError("MobileNumber", "Mobile Number not available");
                }
            }
            try
            {
                long mbno = long.Parse(request.MobileNumber);
            }
            catch
            {
                ModelState.AddModelError("MobileNumber", "Mobile Number Should Contain only numbers");
            }

            if (ModelState.IsValid)
            {
                //save Profile Image
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

                UserMaster um = new UserMaster();
                PayRollMaster pm = new PayRollMaster();
                
                ProcessResponse response = new ProcessResponse();
                if (request.UserId > 0)
                {
                    um = uService.GetUserById(request.UserId);
                    if (um.CurrentStatus == "SubmittedForReview")
                    {
                        um.ReviewedBy = loginCheckResponse.userName;
                        um.ReviewedOn = DateTime.Now;
                        PayRollMaster npm = new PayRollMaster();
                        npm.BasicSalary = request.BasicSalary;
                        npm.HRA = request.HRA;
                        npm.DearnessAllowance = request.DearnessAllowance;
                        npm.FoodAllowance = request.FoodAllowance;
                        npm.Conveyance = request.Conveyance;
                        npm.MedicalAllowances = request.MedicalAllowances;
                        npm.OtherAllowances = request.OtherAllowances;
                        npm.ProvidentFund = request.ProvidentFund;
                        npm.ProfessionalTax = request.ProfessionalTax;
                        npm.ESIFund = request.ESIFund;
                        npm.TDS = request.TDS;
                        //CloneObjects.CopyPropertiesTo(request, npm);
                        npm.StaffId = request.UserId;
                        npm.CreatedOn = DateTime.Now;
                        npm.CreatedBy = loginCheckResponse.userId;
                        pSercice.SavePayRoll(npm);
                        um.CurrentStatus = "SentForApproval";
                        string adminemail = _config.GetValue<string>("EmailConfig:ceoEmail");
                        string ceoName = _config.GetValue<string>("EmailConfig:ceoName");

                        UserTypeMaster utm = uService.GetUserTypeById((int)request.UserTypeId);
                        //email to ceo for approvall
                        var mTC = nService.SendApproveRequestForCeo(AppSettings.EmailTemplates.ApproveRequestForCeo,adminemail, ceoName, request.UserName,utm.TypeName);

                    }
                    else
                    {
                        um.CurrentStatus = um.CurrentStatus;
                    }

                    //pm = pSercice.GetPayRollByUserId(request.UserId);


                    um.UserName = request.UserName;
                    um.EmailId = request.EmailId;
                    um.MobileNumber = request.MobileNumber;
                    um.UserTypeId = request.UserTypeId;
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
                    um.AadharNumber = request.AadharNumber;
                    um.PANNumber = request.PANNumber;
                    um.PreviousOrgName = request.PreviousOrgName;
                    um.PreviousOrgAddress = request.PreviousOrgAddress;
                    um.PreviousOrgExperience = request.PreviousOrgExperience;
                    um.TotalPreviousExpeience = request.TotalPreviousExpeience;
                    um.BankName = request.BankName;
                    um.AccountNumber = request.AccountNumber;
                    um.AccountHolderName = request.AccountHolderName;
                    um.IfscCode = request.IfscCode;
                    um.BranchAddress = request.BranchAddress;
                    um.UPINumber = request.UPINumber;
                    um.DateOfJoin = request.DateOfJoin;
                    um.ReportingManager = request.ReportingManager;
                    um.ShiftId = request.ShiftId;
                    um.MaxDiscoutPercentage = request.MaxDiscoutPercentage;
                    um.PFNumber = request.PFNumber;
                    um.UANNumber = request.UANNumber;
                    um.ESICNumber = request.ESICNumber;
                    um.BioMetricId = request.BioMetricId;
 
                    
                    um.LastModifiedBy = loginCheckResponse.userName;
                    um.LastModifiedOn = DateTime.Now;
 
                    um.ProfileImage = isProfileImageUploaded ? profileImageName : um.ProfileImage;

                    um.PanCard = isPanCardUploaded ? PanCardName : um.PanCard;
                    um.AdharCard = isAdharCardUploaded ? AdharCardName : um.AdharCard;
                    um.AadharBack = isAdharBackUploaded ? AadharBackName : um.AadharBack;
                    um.ResumeDoc = isResumeDocUploaded ? ResumeDocName : um.ResumeDoc;
                    um.SalarySlip = isSalarySlipUploaded ? SalarySlipName : um.SalarySlip;
                    um.PreviousRelievingLetter = isPreviousRelievingLetterUploaded ? PreviousRelievingLetterName : um.PreviousRelievingLetter;
                    um.ProfileImage = isProfileImageUploaded ? profileImageName : um.ProfileImage;
                    um.BankName = PasswordEncryption.Encrypt(request.BankName);
                    um.AccountNumber = PasswordEncryption.Encrypt(request.AccountNumber);
                    um.IfscCode = PasswordEncryption.Encrypt(request.IfscCode);
                    um.AccountHolderName = PasswordEncryption.Encrypt(request.AccountHolderName);
                    um.BranchAddress = PasswordEncryption.Encrypt(request.BranchAddress);
                    um.UPINumber = PasswordEncryption.Encrypt(request.UPINumber);
                    um.AadharNumber = PasswordEncryption.Encrypt(request.AadharNumber);
                    if (um.MaxDiscoutPercentage == null)
                    {
                        um.MaxDiscoutPercentage = 0;
                    }
                    um.DisplayName = request.DisplayName == null ? request.UserName : request.DisplayName;
                    um.OfficialEmail = request.OfficialEmail == null ? request.EmailId : request.OfficialEmail;
                    um.WhatsappNumber = request.WhatsappNumber;
                    response = uService.SaveUser(um);
                    if(response.currentId > 0)
                    {
                        pm = pSercice.GetPayRollByUserId(request.UserId);
                        //pm.PRMId = request.PRMId;
                        pm.BasicSalary = request.BasicSalary;
                        pm.DearnessAllowance = request.DearnessAllowance;
                        pm.Conveyance = request.Conveyance;
                        pm.OtherAllowances = request.OtherAllowances;
                        pm.HRA = request.HRA;
                        pm.FoodAllowance = request.FoodAllowance;
                        pm.MedicalAllowances = request.MedicalAllowances;
                        pm.ProvidentFund = request.ProvidentFund;
                        pm.ProfessionalTax = request.ProfessionalTax;
                        pm.ESIFund = request.ESIFund;

                        pm.TDS = request.TDS;
                        pSercice.SavePayRoll(pm);
                    }
                }
                
                if(response.statusCode == 1)
                {
                    if(um.CurrentStatus== "SubmittedForReview"|| um.CurrentStatus == "SentForApproval"|| um.CurrentStatus == "Approved")
                    {
                        return RedirectToAction("NewStaffList");
                    }
                    else if(um.CurrentStatus== "Active"|| um.CurrentStatus== "Relieved"||um.CurrentStatus== "DisActive")
                    {
                        return RedirectToAction("StaffList");
                    }
                    
                }
                else
                {
                    ViewBag.ErrorMessage = response.statusMessage;
                }
                
            }
            request.userTypeDrops = cService.GetUserTypeDrops();
            request.countryDrops = cService.GetAllCountries();
            request.stateDrops = cService.GetAllStates((int)request.CountryId);
            request.cityDrops = cService.GetAllCities((int)request.StateId);
            request.StaffDrops = cService.GetStaffDropsForReportingHead((int)request.UserTypeId);
            request.ShiftDrops = cService.GetShiftDrops();

            return View(request);

        }
        public async Task<IActionResult> ResendJoiningLetterAsync(int id)
        {
            //UserMaster um = uService.GetUserById(id);

            //UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
            //UserMaster rHead = uService.GetUserById((int)um.ReportingManager);


            //var ete = nService.ResendJoiningAcceptanceToEmployee(AppSettings.EmailTemplates.ResendJoiningRequest, um.EmailId, um.UserName, utm.TypeName, (DateTime)um.DateOfJoin, rHead.UserName, um.FormFillUrl);

            //return Json(new { result = ete });
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA";
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            string filepath = string.Empty;

            ProcessResponse pr = new ProcessResponse();
            try
            {
                UserMaster um = new UserMaster();
                um = uService.GetUserById(id);
                //um.CurrentStatus = "Approved";
                //um.ApprovedBy = loginCheckResponse.userName;
                //um.ApprovedOn = DateTime.Now;

                //pr = uService.SaveUser(um);

                //email to employee for accept to join
                UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
                UserMaster rUser = uService.GetUserById((int)um.ReportingManager);
                //create offer letter 
                AppointmentPDFModel m = new AppointmentPDFModel();
                m = uService.GetDataForAppointmentOrder(id);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "Pdfs");
                string fileName = "OFFER_" + m.EmployeeName;
                char[] charsToTrim = { ' ', '.' };
                fileName = fileName.Trim(charsToTrim);
                fileName = fileName.Replace(" ", "");
                var pdfname = String.Format("{0}.pdf", fileName + id.ToString());
                var path = Path.Combine(uploads, pdfname);
                path = Path.GetFullPath(path);
                filepath = path;
                var pdf = new ViewAsPdf("AppointmentOrder", m)
                {
                    PageMargins = { Left = 20, Bottom = 5, Right = 20, Top = 5 },
                    SaveOnServerPath = path

                };
                byte[] pdfTosend = await pdf.BuildFile(ControllerContext);
                uService.UpdateStaffOfferUrl(pdfname, id);
                // sending email  
                try
                {
                    EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                    emailTemplate = nService.GetEmailTemplateByModule(AppSettings.EmailTemplates.AcceptanceToEmployee);
                    if (emailTemplate != null)
                    {
                        string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                        string WebHostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                        string key = WebHostURL + "EmployeeAcceptance/ThankyouFOrAcceptance?key=" + um.FormFillUrl;
                        string emailText = emailTemplate.EmailContent;
                        string rd = Convert.ToDateTime(um.DateOfJoin).ToString("D");
                        emailText = emailText.Replace("##UserName##", um.UserName);
                        emailText = emailText.Replace("##Designation##", utm.TypeName);
                        emailText = emailText.Replace("##DATE##", rd);
                        emailText = emailText.Replace("##ReportName##", rUser.UserName);
                        emailText = emailText.Replace("##LINK##", key);

                        string emailimagesurl = _config.GetValue<string>("EmailConfig:EMAILIMAGEURL");
                        string smtpserver = _config.GetValue<string>("EmailConfig:smtpServer");
                        string smtpUsername = _config.GetValue<string>("EmailConfig:smtpEmail");
                        string smtpPassword = _config.GetValue<string>("EmailConfig:smtppassword");
                        int smtpPort = _config.GetValue<int>("EmailConfig:portNumber");

                        emailText = emailText.Replace("##EMAILIMAGES##", emailimagesurl);
                        MailMessage msg = new MailMessage(smtpUsername, um.EmailId, emailTemplate.Subject, emailText);

                        MailMessage mail = new MailMessage();
                        mail.To.Add(um.EmailId);
                        mail.From = new MailAddress(smtpUsername);
                        mail.Subject = emailTemplate.Subject;
                        mail.Body = emailText;
                        mail.IsBodyHtml = true;
                        if (pdfTosend != null)
                        {
                            var att = new Attachment(new MemoryStream(pdfTosend), "Appointment.pdf");
                            mail.Attachments.Add(att);
                        }
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpserver;
                        smtp.Port = smtpPort;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                        smtp.EnableSsl = true;

                        try
                        {
                            smtp.Send(mail);


                        }
                        catch (Exception ex)
                        {
                            ex.Source = "approve staff";
                            uService.LogError(ex);
                        }


                    }
                }
                catch (Exception ex)
                {
                    ex.Source = "approve staff";
                    uService.LogError(ex);
                }
            }
            catch (Exception ex)
            {
                ex.Source = "approve staff " + filepath;
                uService.LogError(ex);
            }
            //var email = nService.SendAcceptanceToEmployee(AppSettings.EmailTemplates.AcceptanceToEmployee, um.EmailId,
            //    um.UserName, utm.TypeName, (DateTime)um.DateOfJoin, rUser.UserName, um.FormFillUrl, pdfFile);


            return RedirectToAction("NewStaffList");
        }

        public IActionResult DeleteStaff(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            UserMaster um = new UserMaster();
            um = uService.GetUserById(id);
            um.IsDeleted = true;
            um.IsActive = false;
            um.CurrentStatus = "Deleted";
            //um.LastModifiedBy=
            pr = uService.SaveUser(um);
            return Json(new { result = pr });
        }
        public async Task<IActionResult> ApproveStaffAsync(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA";
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            string filepath = string.Empty;

            ProcessResponse pr = new ProcessResponse();
            try
            {
                UserMaster um = new UserMaster();
                um = uService.GetUserById(id);
                um.CurrentStatus = "Approved";
                um.ApprovedBy = loginCheckResponse.userName;
                um.ApprovedOn = DateTime.Now;

                pr = uService.SaveUser(um);

                //email to employee for accept to join
                UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
                UserMaster rUser = uService.GetUserById((int)um.ReportingManager);
                //create offer letter 
                AppointmentPDFModel m = new AppointmentPDFModel();
                m = uService.GetDataForAppointmentOrder(id);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "Pdfs");
                string fileName = "OFFER_" + m.EmployeeName;
                char[] charsToTrim = { ' ', '.' };
                fileName = fileName.Trim(charsToTrim);
                fileName = fileName.Replace(" ", "");
                var pdfname = String.Format("{0}.pdf", fileName + id.ToString());
                var path = Path.Combine(uploads, pdfname);
                path = Path.GetFullPath(path);
                filepath = path;
                var pdf = new ViewAsPdf("AppointmentOrder", m)
                {
                    PageMargins = { Left = 20, Bottom = 5, Right = 20, Top = 5 },
                    SaveOnServerPath = path

                };
                byte[] pdfTosend = await pdf.BuildFile(ControllerContext);
                uService.UpdateStaffOfferUrl(pdfname, id);
                // sending email  
                try
                {
                    EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                    emailTemplate = nService.GetEmailTemplateByModule(AppSettings.EmailTemplates.AcceptanceToEmployee);
                    if (emailTemplate != null)
                    {
                        string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                        string WebHostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                        string key = WebHostURL + "EmployeeAcceptance/ThankyouFOrAcceptance?key=" + um.FormFillUrl;
                        string emailText = emailTemplate.EmailContent;
                        string rd = Convert.ToDateTime(um.DateOfJoin).ToString("D");
                        emailText = emailText.Replace("##UserName##", um.UserName);
                        emailText = emailText.Replace("##Designation##", utm.TypeName);
                        emailText = emailText.Replace("##DATE##", rd);
                        emailText = emailText.Replace("##ReportName##", rUser.UserName);
                        emailText = emailText.Replace("##LINK##", key);

                        string emailimagesurl = _config.GetValue<string>("EmailConfig:EMAILIMAGEURL");
                        string smtpserver = _config.GetValue<string>("EmailConfig:smtpServer");
                        string smtpUsername = _config.GetValue<string>("EmailConfig:smtpEmail");
                        string smtpPassword = _config.GetValue<string>("EmailConfig:smtppassword");
                        int smtpPort = _config.GetValue<int>("EmailConfig:portNumber");

                        emailText = emailText.Replace("##EMAILIMAGES##", emailimagesurl);
                        MailMessage msg = new MailMessage(smtpUsername, um.EmailId, emailTemplate.Subject, emailText);

                        MailMessage mail = new MailMessage();
                        mail.To.Add(um.EmailId);
                        mail.From = new MailAddress(smtpUsername);
                        mail.Subject = emailTemplate.Subject;
                        mail.Body = emailText;
                        mail.IsBodyHtml = true;
                        if (pdfTosend != null)
                        {
                            var att = new Attachment(new MemoryStream(pdfTosend), "Appointment.pdf");
                            mail.Attachments.Add(att);
                        }
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpserver;
                        smtp.Port = smtpPort;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                        smtp.EnableSsl = true;

                        try
                        {
                            smtp.Send(mail);
                          

                        }
                        catch (Exception ex)
                        {
                            ex.Source = "approve staff";
                            uService.LogError(ex);
                        }


                    }
                }
                catch (Exception ex)
                {
                    ex.Source = "approve staff";
                    uService.LogError(ex);
                }
            }
            catch (Exception ex)            
            {
                ex.Source = "approve staff " + filepath;
                uService.LogError(ex);
            }
            //var email = nService.SendAcceptanceToEmployee(AppSettings.EmailTemplates.AcceptanceToEmployee, um.EmailId,
            //    um.UserName, utm.TypeName, (DateTime)um.DateOfJoin, rUser.UserName, um.FormFillUrl, pdfFile);


            return RedirectToAction("NewStaffList");
        }
        [HttpPost]
        public IActionResult ReleiveStaff_Old(int id, string ReleivedRemarks)
        {
            ProcessResponse pr = new ProcessResponse();
            UserMaster um = new UserMaster();
            um = uService.GetUserById(id);
            um.ReleivedOn = DateTime.Now;
            um.CurrentStatus = "Relieved";
            um.IsActive = false;
            um.ReleivedRemarks = ReleivedRemarks;
            pr = uService.SaveUser(um);
            // send email to admin and user
            //var eMail = nService.SendReleivingEmailToEmployee(AppSettings.EmailTemplates.RelievingLetterToEmployee, um.EmailId, um.UserName, um.ReleivedOn);
            //var aMail = nService.SendReleivingEmailToAdmin(AppSettings.EmailTemplates.RelievingLetterToAdmin, um.EmailId,"", um.UserName,"", um.ReleivedOn);
            return Json(new { result = pr });
        }
        public IActionResult Profile(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA";
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            UserMasterDisplay umd = uService.GetUserDisplayModelById(id);
            return View(umd);
        }
        public IActionResult StaffAttendance(int Month=0, int Year=0, string empCode = "")
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
            
            List<StaffAttDisplayModel> attList = new List<StaffAttDisplayModel>();
            DateTime dt = new DateTime();
            if(Month==0 || Year==00)
            {
                dt = DateTime.Now;
            }
            else
            {
                dt = new DateTime(Year, Month, 1);
            }
            
            attList = uService.GetAttandance((DateTime)dt, empCode);

            // this is to display staff list for attenance
            List<StaffAttDisplayModel> staf = uService.GetStaffForAtt();
            ViewBag.staffDrop = staf;
            ViewBag.ShowingDate =dt;
            if (!string.IsNullOrEmpty(empCode))
            {
                ViewBag.ecode=empCode;
            }
            return View(attList);
        }
        [HttpPost]
        public IActionResult StaffAttendanceData(AttendanceModel request)
        {
            DateTime d = (DateTime)request.DateofAttendance;
            DayOfWeek day = d.DayOfWeek;
            if ((day == DayOfWeek.Friday || day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)&&request.AttStatus=="W")
            {
                ProcessResponse resu = new ProcessResponse();
                resu.statusCode = 2;
                resu.statusMessage = "On Friday/Saturday/Sunday Cannot be accepted as WeekOff \nIt will be considered as Absent \n Is it OK?";
                return Json(new { result=resu});
            }
            UserMaster um = uService.GetUserById(request.StaffId);
            DateTime joinDate = (DateTime)um.DateOfJoin;
            if ((d.Year < joinDate.Year) ||(d.Year==joinDate.Year&&d.Month<joinDate.Month)||(d.Year==joinDate.Year&&d.Month==joinDate.Month&&d.Day<joinDate.Day))
            {
                ProcessResponse resu = new ProcessResponse();
                resu.statusCode = 3;
                resu.statusMessage = "Attendance before join date is not possible";
                return Json(new { result = resu });
            }
            AttendanceMaster am = new AttendanceMaster();
            request.IsDeleted = false;
            CloneObjects.CopyPropertiesTo(request, am);
            var res = stfService.SaveAttendanceType(am);
            return Json(new { result = res });
        }
        public IActionResult ReleiveStaff(int id=0)
        {
            UserMaster um = new UserMaster();
            try
            {   
                um = uService.GetUserById(id);
                ViewBag.um = um;
            }
            catch(Exception ex)
            {

            }
            ReleiveStaffModel sm = new ReleiveStaffModel();
            sm.StaffId = id;
            return View(sm);
        }
        [HttpPost]
        public IActionResult ReleiveStaff(ReleiveStaffModel request)
        {

            try
            {
                ProcessResponse pr = new ProcessResponse();
                if(ModelState.IsValid)
                {
                    if(request.StaffId > 0)
                    {
                        UserMaster umT = new UserMaster();
                        umT = uService.GetUserById(request.StaffId);
                        umT.ReleivedOn = request.RelievingDate;
                        umT.CurrentStatus = "Relieved";
                        umT.IsActive = false;
                        umT.ReleivedRemarks = request.Remarks;
                        pr = uService.SaveUser(umT);
                        // send emails
                        var AD = nService.SendReleivingEmailToAdmin(AppSettings.EmailTemplates.RelievingLetterToAdmin,
                            _config.GetValue<string>("EmailConfig:adminEmail"), umT.UserName, umT.UserName, " ", Convert.ToDateTime(request.RelievingDate));
                        var ed = nService.SendReleivingEmailToEmployee(AppSettings.EmailTemplates.RelievingLetterToEmployee, umT.EmailId,
                            umT.UserName, Convert.ToDateTime(request.RelievingDate));
                        return RedirectToAction("StaffList");
                    }
                    else
                    {
                        pr.statusCode = 0;
                        pr.statusMessage = "Failed";
                    }
                    
                  
                }
            }
            catch (Exception ex)
            {

            }
            UserMaster um = new UserMaster();
            um = uService.GetUserById(request.StaffId);
            ViewBag.um = um;
            return View(request);
        }
        public IActionResult MonthlyPayRoll(int sId = 0,int month = 0,int year = 0)
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

            List<MPayRollDisplayModel> response = mprService.GetMPayRollDisplays(sId,month,year);
            List<StaffAttDisplayModel> staf= uService.GetStaffForAtt();
            
            foreach (StaffAttDisplayModel s in staf)
            {
                s.StaffId = s.StaffId;
                s.StaffName += "  (" + s.EmployeeCode + ")";
            }
            ViewBag.staff = staf;
            List<FHeadDrops> drps = cService.GetFHeadDrops();
            List<FHeadDrops> finalDrops = cService.GetBankAndCashHeads();
            
            ViewBag.FromHeads = finalDrops;
            return View(response);
        }
        [HttpPost]
        public IActionResult Generate(int sId,int month,int year)
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
            if (month == 0 || year == 0)
            {
                ProcessResponse pz = new ProcessResponse();
                pz.statusCode = 3;
                pz.statusMessage = "Please Select Month and Year";
                return Json(new { result = pz });
            }
            else
            {
                ProcessResponse pr = mprService.SaveMonthlyPayRoll(sId, month, year, loginCheckResponse.userId);
                return Json(new { result = pr });
            }
        }
        public IActionResult PostSalaryToFinance(int payid,DateTime doTrans,int fromId)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            MonthlyPayRoll mpr = mprService.GetMonthlyPayRollById(payid);
            string month = "";
            if (mpr.MonthNumber == 1){month = "Jan";}
            else if (mpr.MonthNumber == 2) { month = "Feb"; }
            else if (mpr.MonthNumber == 3) { month = "Mar"; }
            else if (mpr.MonthNumber == 4) { month = "Apr"; }
            else if (mpr.MonthNumber == 5) { month = "May"; }
            else if (mpr.MonthNumber == 6) { month = "June"; }
            else if (mpr.MonthNumber == 7) { month = "July"; }
            else if (mpr.MonthNumber == 8) { month = "Agu"; }
            else if (mpr.MonthNumber == 9) { month = "Sep"; }
            else if (mpr.MonthNumber == 10) { month = "Oct"; }
            else if (mpr.MonthNumber == 11) { month = "Nov"; }
            else if (mpr.MonthNumber == 12) { month = "Dec"; }
            UserMaster um = uService.GetUserById((int)mpr.StaffId);
            FinanceHeads fh = fService.GetFinHeadByStaffId((int)mpr.StaffId);
            if (fh == null)
            {                
                fh = new FinanceHeads();
                fh.StaffId = mpr.StaffId;
                FinanceGroups fg = fService.GetFinanceGroupByName("Salaries");
                if (fg == null)
                {
                    fg = new FinanceGroups();
                    fg.GroupName = "Salaries";
                    fg.GroupCode = "cash";
                    fg.IsDeleted = false;
                    fService.SaveFinGroup(fg);
                    fg = fService.GetFinanceGroupByName("Salaries");
                }
                fh.GroupId = fg.GroupId;
                fh.HeadName = um.UserName;
                fh.StartingBallance = 0;
                fh.CurrentBallance = 0;
                fh.IsDeleted = false;
                fh.HeadCode = "Staff";
                ProcessResponse fp = fService.SaveFinHead(fh);
                fh= fService.GetFinHeadByStaffId((int)mpr.StaffId);
            }
            FinanceTransactions ft = new FinanceTransactions();
            ft.FromHeadID = fromId;
            ft.Description = "Salary Of " + um.UserName + " on " + month + " , " + mpr.YearNumber;
            ft.VoucherType = "Payment";
            int vcr = fService.GetPreviousVoucherNumber();
            ft.VoucherNumber = ++vcr;
            ft.Debit = mpr.NetSalary;
            ft.Credit = 0;
            ft.TTime = DateTime.Now;
            ft.DateOfTransaction = doTrans;
            ft.DoneBy = loginCheckResponse.userName;
            ft.ToHeaID = fh.HeadId;
            ft.ChequeDate = doTrans;
            ft.UserId = mpr.StaffId;
            ProcessResponse pr = fService.SaveFinTransactions(ft);
            if (pr.statusCode == 1)
            {

                FinanceTransactions ft2 = new FinanceTransactions();
                ft2.FromHeadID = fh.HeadId;
                ft2.Description = "Salary Of " + um.UserName + " on " + month + " , " + mpr.YearNumber;
                ft2.VoucherType = "Payment";
                ft2.VoucherNumber = ++vcr;
                ft2.Debit = 0;
                ft2.Credit = mpr.NetSalary;
                ft2.TTime = DateTime.Now;
                ft2.DateOfTransaction = doTrans;
                ft2.DoneBy = loginCheckResponse.userName;
                ft2.ToHeaID = fromId;
                ft2.ChequeDate = doTrans;
                ft2.UserId = mpr.StaffId;
                ft2.RelatedTrId = pr.currentId;
                pr = fService.SaveFinTransactions(ft2);
                ft.RelatedTrId = pr.currentId;
                pr = fService.SaveFinTransactions(ft);
                if (pr.statusCode == 1)
                {
                    fh.CurrentBallance += mpr.NetSalary;
                    ProcessResponse pr2 = fService.SaveFinHead(fh);
                    FinanceHeads fh1 = fService.GetFinHeadById(fromId);
                    fh1.CurrentBallance -= mpr.NetSalary;
                    pr2 = fService.SaveFinHead(fh1);
                    mpr.IsPostedToFinance = true;
                    pr2 = mprService.UpdateMonthlyPayroll(mpr);
                }                
            }
            return Json(new { result = pr });
        }
        public IActionResult ReportingManagerList(string sear="")
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

            int id = 0;
            List<EmployeeTargetList> rhL= uService.GetStaffForTargetList(id,sear);
            return View(rhL);
        }
        public IActionResult SubOrdinatesList(int id,string sear = "")
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

            UserMaster um = uService.GetUserById(id);
            List<EmployeeTargetList> sL = uService.GetStaffForTargetList(id, sear);
            ViewBag.uName = um.UserName;
            ViewBag.id = id;
            return View(sL);
        }
        public IActionResult Targets (int id)
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

            UserMaster um = uService.GetUserById(id);
            UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
            if (utm.TypeName == "Sales Executive")
            {
                List<TargetMaster> tML = uService.getTargetsOfUser(id);
                ViewBag.uName = um.UserName;
                ViewBag.uId = id;
                ViewBag.TypeName = utm.TypeName;
                return View(tML);
        }
            else
            {
                List<TargetMaster> htl = uService.GetTargetsOfHead(id);
                ViewBag.uName = um.UserName;
                ViewBag.uId = id;
                ViewBag.TypeName = utm.TypeName;
                return View(htl);
            }
        }
        public IActionResult TargetBack(int id)
        {
            UserMaster um = uService.GetUserById(id);
            UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
            if(utm.TypeName== "Sales Executive")
            {
                return RedirectToAction("SubOrdinatesList", new {id=um.ReportingManager });
            }
            else
            {
                return RedirectToAction("ReportingManagerList");
            }
        }
        public IActionResult AddTarget(int id)
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

            ViewBag.uId = id;
            TargetMaster tm = new TargetMaster();
            //if (id > 0)
            //{
            //    tm = uService.getTargetsOfUser(id);
            //}
            tm.UserId = id;
            return View(tm);
        }
        [HttpPost]
        public IActionResult AddTarget(TargetMaster request)
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
                request.CreatedBy = loginCheckResponse.userName;
                request.CreatedOn = DateTime.Now;
                request.LastModifiedBy = loginCheckResponse.userName;
                request.LastMOdifiedOn = DateTime.Now;
                request.TargetDone = 0;
                ProcessResponse pr = uService.SaveTargetMaster(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Targets", new { id = request.UserId });
                }
                else
                {
                    ViewBag.errmsg = pr.statusMessage;
                }
            }
            return View(request);
        }

        public IActionResult StaffLoans()
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

            List<StaffLoansDisplay> myList = new List<StaffLoansDisplay>();
            myList = sService.GetAllStaffLoans();

            List<UserMasterDisplay> cats = new List<UserMasterDisplay>();
            cats = uService.GetAllUsers();
            ViewBag.Loans = cats;

            return View(myList);
        }
        public IActionResult StaffLoansData(int id = 0)
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
            StaffLoans myObject = new StaffLoans();
            if (id > 0)
            {
                myObject = sService.GetStaffLoanById(id);

            }
            List<UserMasterDisplay> cats = new List<UserMasterDisplay>();
            cats = uService.GetAllUsers();
            ViewBag.Loans = cats;
            myObject.NoofMonths = 12;
            return View(myObject);
        }

        [HttpPost]
        public IActionResult StaffLoansData(StaffLoans request)
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

            return View(request);
        }
        public IActionResult CreateStaffLoan(StaffLoans request)
        {
            ProcessResponse pr = new ProcessResponse();
            pr.statusCode = 0;
            if (ModelState.IsValid)
            {
                if (request.RepaymentMode == "EMI")
                {
                    if (request.NoofMonths == null)
                    {
                        request.NoofMonths = 12;
                    }
                    request.MonthlyEMI = (request.AmountTaken / request.NoofMonths);
                }
                else
                {
                    request.NoofMonths = 1;
                    request.MonthlyEMI = request.AmountTaken;
                }
                request.TakenOn = DateTime.Now;
                request.LoanStatus = "Open";
                // save or update call
                pr = sService.SaveStaffLoans(request);
                //if (pr.statusCode == 1)
                //{
                //    return RedirectToAction("StaffLoans");
                //}
                //else
                //{
                //    ViewBag.loan = pr.statusMessage;
                //}
            }
            //List<UserMasterDisplay> cats = new List<UserMasterDisplay>();
            //cats = uService.GetAllUsers();

            //ViewBag.Loans = cats;
            return Json(new { result = pr });
        }
        public IActionResult CheckAvailbilityForLoan(int sId)
        {
            UserMaster um = uService.GetUserById(sId);
            List<StaffLoans> sll = sService.GetLoansOfUser(sId);
            decimal a=0;
            decimal e = 0;
            bool isSingleEmi = false;
            int singlLoan = 0;
            if (sll != null)
            {
                foreach(StaffLoans l in sll)
                {
                    a += (decimal)l.AmountTaken;
                    if (l.RepaymentMode == "EMI")
                    {
                        e += (decimal)l.MonthlyEMI;
                    }
                    if (l.RepaymentMode == "Single")
                    {
                        isSingleEmi = true;
                        singlLoan+= (int)l.AmountTaken;
                    }
                }
            }
            PayRollMaster prm = pSercice.GetPayRollByUserId(sId);
            
            if (prm.BasicSalary == null)        {prm.BasicSalary = 0; }
            if (prm.HRA == null)                { prm.HRA = 0; }            
            if (prm.DearnessAllowance == null)  {prm.DearnessAllowance = 0; }            
            if (prm.FoodAllowance == null)      {prm.FoodAllowance = 0; }            
            if (prm.Conveyance == null)         {prm.Conveyance = 0; }            
            if (prm.MedicalAllowances == null)  {prm.MedicalAllowances = 0; }            
            if (prm.OtherAllowances == null)    {prm.OtherAllowances = 0; }
            if (prm.ProvidentFund == null)      {prm.ProvidentFund = 0; }            
            if (prm.ProfessionalTax == null)    {prm.ProfessionalTax = 0; }            
            if (prm.ESIFund == null)            {prm.ESIFund = 0; }            
            if (prm.TDS == null)                {prm.TDS = 0; }
            decimal sal = (decimal)(prm.BasicSalary + prm.HRA + prm.DearnessAllowance + prm.FoodAllowance + prm.Conveyance + prm.MedicalAllowances + prm.OtherAllowances) - (decimal)(prm.ProvidentFund + prm.ProfessionalTax + prm.ESIFund + prm.TDS);
            decimal lonableSalary = ((sal / 100) * 60);
            return Json(new { amount = a , name=um.UserName , emi=e , s=lonableSalary , singleEMIExists=isSingleEmi , singleAmt=singlLoan});
        }

        public async Task<byte[]> AppointmentOrderAsync(int sId)
        {
            AppointmentPDFModel m = new AppointmentPDFModel();
            m = uService.GetDataForAppointmentOrder(sId);
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "Pdfs");
            string fileName = "OFFER_" + m.EmployeeName; 
            char[] charsToTrim = { ' ', '.' }; 
            fileName = fileName.Trim(charsToTrim);
            fileName = fileName.Replace(" ", "");
            var pdfname = String.Format("{0}.pdf", fileName + sId.ToString());
            var path = Path.Combine(uploads, pdfname);
            path = Path.GetFullPath(path);
            var pdf = new ViewAsPdf("AppointmentOrder", m)
            {
                PageMargins = { Left = 20, Bottom = 5, Right = 20, Top = 5 },
                SaveOnServerPath = path
               

            };
            uService.UpdateStaffOfferUrl(path, sId);
            var pdfFile = await pdf.BuildFile(ControllerContext);
            return pdfFile;
        }

        public void CreateOfferLetter(int sId)
        {
            AppointmentPDFModel m = new AppointmentPDFModel();
            m = uService.GetDataForAppointmentOrder(sId);
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "Pdfs");
            string fileName = "OFFER_" + m.EmployeeName;
            char[] charsToTrim = { ' ', '.' };
            fileName = fileName.Trim(charsToTrim);
            fileName = fileName.Replace(" ", "");
            var pdfname = String.Format("{0}.pdf", fileName + sId.ToString());
            var path = Path.Combine(uploads, pdfname);
            path = Path.GetFullPath(path);
            var pdf = new ViewAsPdf("AppointmentOrder", m)
            {
                PageMargins = { Left = 20, Bottom = 5, Right = 20, Top = 5 },
                SaveOnServerPath = path


            };
            uService.UpdateStaffOfferUrl(path, sId);
            
        }
    }
}



