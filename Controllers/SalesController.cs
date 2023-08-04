using SCASA.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace SCASA.Controllers
{
    public class SalesController : Controller
    {
        private readonly ICustomerMgmtService cService;
        private readonly ICommonService comService;
        private readonly ISalesMasterMgmtService sMService;
        private readonly ISalesDetailsMgmtService sDService;
        private readonly IFinanceMgmtService fService;
        private readonly IUserMgmtService uService;
        private readonly INotificationService nService;
        private readonly IInventoryService iService;
        private readonly IGSTMasterMgmtService gstService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly ICompanyMgmtService companyService;

        public SalesController(ICustomerMgmtService _cService, ISalesMasterMgmtService _sMService, IFinanceMgmtService _fService, ISalesDetailsMgmtService _sDService,
            IUserMgmtService _uService, ICommonService commmService, IGSTMasterMgmtService _gstService, IInventoryService _iService, INotificationService _nService,
            IHostingEnvironment _hostingEnvironment, IConfiguration config, ICompanyMgmtService _companyService)
        {
            cService = _cService;
            sMService = _sMService;
            sDService = _sDService;
            fService = _fService;
            comService = commmService;
           iService = _iService;
            gstService = _gstService;
            uService = _uService;
            nService = _nService;
            hostingEnvironment = _hostingEnvironment;
            _config = config;
            companyService = _companyService;
        }

        public IActionResult Index(int s = 0)
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
            
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; 
            ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);

            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);

            ProductDisplaySalesModel response = new ProductDisplaySalesModel();
            response = sMService.GetCatsForSales();
            response.payrollInfo = sMService.GetPayrollForStaff(loginCheckResponse.userId);
            response.leadInfo = sMService.GetLeadInfo(loginCheckResponse.userId);
            response.attendanceInfo = sMService.GetStaffAttendance(loginCheckResponse.userId);
            response.loanInfo = sMService.GetLoansInformation(loginCheckResponse.userId);
            response.targets = uService.getTargetsOfUser(loginCheckResponse.userId);

            return View(response);
        }
        public IActionResult Inventory(int catid =0, int pageNumber =1 , int pageSize = 12)
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

            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer;
            ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ct= comService.GetCatsDrop();
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);
            if (catid == 0)
            {
                catid = ct[0].CategoryId;
            }
            ViewBag.cid = catid;
            ViewBag.pn = pageNumber;
            ViewBag.ps = pageSize;
            List<ProductsDisplaySales> response = new List<ProductsDisplaySales>();
            response = sMService.GetCatProds(catid,pageNumber,pageSize);
            ViewBag.totProds = sMService.GetNoOfCartProds(catid);
            return View(response);
        }
        public IActionResult LoadMoreInventory(int catid = 0, int pageNumber = 1, int pageSize = 12)
        {
            List<ProductsDisplaySales> response = new List<ProductsDisplaySales>();
            response = sMService.GetCatProds(catid, pageNumber, pageSize);
            foreach (ProductsDisplaySales p in response)
            {
                string[] m = p.MainImages.Split(",");
                p.MainImages = m[0];
            }
            return Json(new { re = response });
        }
        public IActionResult Customer(int pageNumber=1,int pageSize=10,string search = "")
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

            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);
            if (search == null)
            {
                search = "";
            }
            List<CustomerListModel> cL = cService.GetAllCustomers(pageNumber,pageSize, search);
            
            int totalCustomers = cService.GetNumberOfCustomers(search);
            ViewBag.TotalRecords = totalCustomers;
            ViewBag.TotalCount = cL.Count;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.search = search;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalCustomers / (decimal)pageSize));
            return View(cL);
        }
        public IActionResult SelectCustomer(int id=0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            ViewBag.cats = comService.GetCategorysWithSubCat();
           
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");

            if (id > 0) {
                CustomerMaster cm = cService.GetCustomerById(id);
                if (sCustomer.statusCode == 0) { 
                    
                    if (cm != null)
                    {
                        SelectedCustomer sC = new SelectedCustomer();
                        sC.Cid = id;
                        sC.FullName = cm.FullName;
                        sC.CoustemerCode = cm.CoustemerCode;
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "SelectedCustomer", sC);

                        //update activity log
                        CustomerSalesActivity csa = new CustomerSalesActivity();
                        csa.AttendedOn = DateTime.Now;
                        csa.CustomerId = sC.Cid;
                        csa.StaffId = loginCheckResponse.userId;
                        csa.Remarks = "Attending the customer";
                        csa.IsDeleted = false;
                        sMService.SaveActivity(csa);
                    }
                }
                else
                {
                    if(sCustomer.Cid == id)
                    {
                        // do nothing, same customer
                    }
                    else
                    {
                        CustomerSalesActivity csaOld = sMService.GetActivity(sCustomer.Cid);
                        csaOld.EndTime = DateTime.Now;
                        sMService.UpdateActivty(csaOld);
                        SelectedCustomer sC = new SelectedCustomer();
                        sC.Cid = id;
                        sC.FullName = cm.FullName;
                        sC.CoustemerCode = cm.CoustemerCode;
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "SelectedCustomer", sC);

                        //update activity log
                        CustomerSalesActivity csa = new CustomerSalesActivity();
                        csa.AttendedOn = DateTime.Now;
                        csa.CustomerId = sC.Cid;
                        csa.StaffId = loginCheckResponse.userId;
                        csa.Remarks = "Attending the customer";
                        csa.IsDeleted = false;
                        sMService.SaveActivity(csa); 
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult RegisterCustomer(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);

          
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            CustomerDisplayModel finalObject = new CustomerDisplayModel();
            CustomerMaster cDM = new CustomerMaster();
            CustomerAddressModel aM = new CustomerAddressModel();
            if (id > 0)
            {
                
                cDM = cService.GetCustomerById(id);
                aM = cService.GetAddModelByUserId(id);               
                CloneObjects.CopyPropertiesTo(cDM, finalObject);
                //finalObject.Addid = aM.Addid;
                //finalObject.HouseNumber = aM.HouseNumber;
                //finalObject.StreetName = aM.StreetName;
                //finalObject.Location = aM.Location;
                //finalObject.CityId = aM.CityId;
                //finalObject.StateId = (int)aM.StateId;
                //finalObject.CountryId = (int)aM.CountryId;
                //finalObject.AddressType = aM.AddressType;
                finalObject.cAddress = aM;
            }
            else
            {
                finalObject.cAddress = new CustomerAddressModel();
            }
            finalObject.countryDrops = new List<CountryDrop>();
            finalObject.countryDrops= comService.GetAllCountries();
            int countryId = id > 0 ? (int)finalObject.cAddress.CountryId : 101;
            finalObject.stateDrops = comService.GetAllStates(countryId);
            int stateId = id > 0 ? (int)finalObject.cAddress.StateId : finalObject.stateDrops[0].StateId;
            finalObject.cityDrops = comService.GetAllCities(stateId);

            return View(finalObject);
        }
        [HttpPost]
        public IActionResult RegisterCustomer(CustomerDisplayModel request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);

           
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
                if (request.Cid > 0)
                {
                    CustomerMaster cm = cService.GetCustomerById(request.Cid);
                    AddressMaster am = cService.GetAddressById(request.cAddress.Addid);
                    cm.FullName = request.FullName;
                    cm.MobileNumber = request.MobileNumber;
                    cm.EmailId = request.EmailId;
                    cm.WhatsAppNumber = request.WhatsAppNumber;
                    cm.RefferedBy = request.RefferedBy;
                    cm.OrganigationName = request.OrganigationName;
                    cm.OrganizationEmailId = request.OrganizationEmailId;
                    cm.OrganizationNumber = request.OrganizationNumber;


                    CustomerAddressModel cam = new CustomerAddressModel();
                    CloneObjects.CopyPropertiesTo(am, cam);
                    cam.HouseNumber = request.cAddress.HouseNumber;
                    cam.StreetName = request.cAddress.StreetName;
                    cam.Location = request.cAddress.Location;
                    cam.CountryId = request.cAddress.CountryId;
                    cam.StateId = request.cAddress.StateId;
                    cam.CityId = request.cAddress.CityId;
                    cam.AddressType = request.cAddress.AddressType;
                    cam.PostalCode = request.cAddress.PostalCode;

                    CustomerDisplayModel cdm = new CustomerDisplayModel();
                    CloneObjects.CopyPropertiesTo(cm, cdm);

                    ProcessResponse pr = cService.SaveCustomer(cdm);
                    if (pr.statusCode == 1)
                    {
                        request.Cid = pr.currentId;
                        pr = cService.SaveAddress(cam);
                        if (pr.statusCode == 1)
                        {
                            return RedirectToAction("Customer");
                        }
                    }
                }
                else
                {
                    request.RegisteredBy = loginCheckResponse.userName;
                    request.RegisteredOn = DateTime.Now;
                    request.IsDeleted = false;
                    request.IsCustomer = false;
                    request.cAddress.IsShipping = true;
                    request.CoustemerCode = "EMC" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + RandomGenerator.RandomString(6, false);
                    request.CurrentStatus = "Active";
                    request.IsCustomer = true;
                    ProcessResponse pr = cService.SaveCustomer(request);

                    if (pr.statusCode == 1)
                    {
                        request.Cid = pr.currentId;
                        CustomerAddressModel cm = new CustomerAddressModel();
                        CloneObjects.CopyPropertiesTo(request.cAddress, cm);
                        cm.Cid = request.Cid;
                        pr = cService.SaveAddress(cm);
                        if (pr.statusCode == 1)
                        {
                            return RedirectToAction("Customer");
                        }
                    }
                }
            }
            request.countryDrops = new List<CountryDrop>();
            request.countryDrops = comService.GetAllCountries();
            int? countryId = request.cAddress.CountryId;
            request.stateDrops = comService.GetAllStates((int)countryId);
            int stateId =(int) request.cAddress.StateId;
            request.cityDrops = comService.GetAllCities(stateId);
            request.CityId = request.cAddress.CityId;
            return View(request);
        }
        public IActionResult CRFQDetails()
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
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);


            return View();
        }
        public IActionResult MyCRFQ()
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
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            List<CRFQMasterModel> mylist = new List<CRFQMasterModel>();
            mylist = cService.GetMyCRFQs(sCustomer.Cid, loginCheckResponse.userId);
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);

            return View(mylist);
        }
        //public IActionResult QuoteDetails()
        //{
        //    LoginResponse loginCheckResponse = new LoginResponse();
        //    loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
        //    if (loginCheckResponse == null)
        //    {
        //        loginCheckResponse = new LoginResponse();
        //        loginCheckResponse.userId = 0;
        //        loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
        //    }
        //    ViewBag.LoggedUser = loginCheckResponse;
        //    SelectedCustomer sCustomer = new SelectedCustomer();
        //    sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
        //    if (sCustomer == null)
        //    {
        //        sCustomer = new SelectedCustomer();
        //        sCustomer.statusCode = 0;
        //        sCustomer.statusMessage = "NA";
        //    }
        //    ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
        //    ViewBag.cats = comService.GetCategorysWithSubCat();
        //    return View();
        //}
        public IActionResult MyQuote(string  ecode="",int pageNumber=1, int pageSize=10)
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
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.ecde = ecode;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);
            List<CRFQMasterModel> mylist = new List<CRFQMasterModel>();
            mylist = cService.GetMyCRFQs(sCustomer.Cid, loginCheckResponse.userId,pageNumber,pageSize);
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            
            //List<GSTMaster> taxes = gstService.GetAllGST();
            //ViewBag.Taxes = taxes;
            int totalRecords= cService.GatCrfqsOfSECount(loginCheckResponse.userId);
            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            //ViewBag.search = search;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            ViewBag.extraDscount = uService.GetUserById(loginCheckResponse.userId).MaxDiscoutPercentage;
            return View(mylist);           
        }
        public IActionResult QuoteDetails(int qmId)
        {
            LoginResponse loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            CRFQMaster cd = sMService.GetCRFQById(qmId);
            CustomerMaster cm = cService.GetCustomerById((int)cd.CustomerId);
            List<CRFQDetails> finallist = sDService.GetCRFQDetails0fMaster(qmId);
            QuoteModelForMail QmForMail = new QuoteModelForMail();

            QmForMail.crfqsList = finallist;
            QmForMail.custMaster = cm;
            QmForMail.sExecutive = uService.GetUserById(loginCheckResponse.userId);
            QmForMail.companyDetails = companyService.GetFirstCompany();
            QmForMail.imgURL = _config.GetValue<string>("OtherConfig:ProductImatesURL");
            return View(QmForMail);
        }
        public async Task<IActionResult> SendQuoteAsync(int qmId)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            CRFQMaster cd = sMService.GetCRFQById(qmId);
            CustomerMaster cm = cService.GetCustomerById((int)cd.CustomerId);
            List<CRFQDetails> finallist = sDService.GetCRFQDetails0fMaster(qmId);
            CompanyMaster comMaster = new CompanyMaster();
            QuoteModelForMail QmForMail = new QuoteModelForMail();

            QmForMail.crfqsList = finallist;
            QmForMail.custMaster = cm;
            QmForMail.sExecutive =uService.GetUserById(loginCheckResponse.userId);
            QmForMail.companyDetails = companyService.GetFirstCompany();
            QmForMail.imgURL= _config.GetValue<string>("OtherConfig:ProductImatesURL");
            //var pr = nService.SendQuoteMailToCustomer(AppSettings.EmailTemplates.SendQuoteToCostomer,cm.EmailId, cm.FullName,finallist);
            string filepath = string.Empty;
            try
            {
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "Pdfs");
                string fileName = "Quotation_" + cm.FullName;
                char[] charsToTrim = { ' ', '.' };
                fileName = fileName.Trim(charsToTrim);
                fileName = fileName.Replace(" ", "");
                var pdfname = String.Format("{0}.pdf", fileName + cm.Cid.ToString());
                var path = Path.Combine(uploads, pdfname);
                path = Path.GetFullPath(path);
                filepath = path;
                var pdf = new ViewAsPdf("QuoteModel", QmForMail)
                {
                    PageMargins = { Left = 20, Bottom = 5, Right = 20, Top = 5 },
                    SaveOnServerPath = path

                };
                byte[] pdfTosend = await pdf.BuildFile(ControllerContext);

                try
                {
                    EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                    emailTemplate = nService.GetEmailTemplateByModule(AppSettings.EmailTemplates.SendQuoteToCostomer);
                    if (emailTemplate != null)
                    {
                        string emailCC = _config.GetValue<string>("OtherConfig:QuoteEmailCC");
                        
                        string emailText = emailTemplate.EmailContent;
                        
                        emailText = emailText.Replace("##UserName##", cm.FullName);

                      

                        string emailimagesurl = _config.GetValue<string>("EmailConfig:EMAILIMAGEURL");
                        string smtpserver = _config.GetValue<string>("EmailConfig:smtpServer");
                        string smtpUsername = _config.GetValue<string>("EmailConfig:smtpEmail");
                        string smtpPassword = _config.GetValue<string>("EmailConfig:smtppassword");
                        int smtpPort = _config.GetValue<int>("EmailConfig:portNumber");

                        emailText = emailText.Replace("##EMAILIMAGES##", emailimagesurl);
                        MailMessage msg = new MailMessage(smtpUsername, cm.EmailId, emailTemplate.Subject, emailText);

                        MailMessage mail = new MailMessage();
                        mail.To.Add(cm.EmailId);
                        mail.CC.Add(emailCC);
                        if (QmForMail.sExecutive.OfficialEmail != null)
                        {
                            mail.CC.Add(QmForMail.sExecutive.OfficialEmail);
                        }
                        mail.From = new MailAddress(smtpUsername);
                        mail.Subject = emailTemplate.Subject;
                        mail.Body = emailText;
                        mail.IsBodyHtml = true;
                        if (pdfTosend != null)
                        {
                            var att = new Attachment(new MemoryStream(pdfTosend), "Quotation.pdf");
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
                            uService.LogError(ex);
                        }
                    }
                }catch(Exception e)
                {

                }
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("MyQuote");
        }
        public IActionResult SaleOrders(string emg="", int pageNumber = 1, int pageSize = 10)
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
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }

            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);
            ViewBag.emgsg = emg;
            List<SaleOrderMasterModel> mylist = new List<SaleOrderMasterModel>();
            mylist = cService.GetMySaleOrders(loginCheckResponse.userId,pageNumber,pageSize);

            int totalRecords = cService.GatCountOfOpenSO(loginCheckResponse.userId);
            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            //ViewBag.search = search;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            return View(mylist);
        }
        public IActionResult ClosedOrders(int pageNumber = 1, int pageSize = 10)
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
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }

            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid, loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);

            List<SaleOrderMasterModel> mylist = new List<SaleOrderMasterModel>();
            mylist = cService.GetMyClosedOrders(loginCheckResponse.userId,pageNumber,pageSize);

            int totalRecords = cService.GetCountOfOpenClosedOrders(loginCheckResponse.userId);
            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            //ViewBag.search = search;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            return View(mylist);
        }

        public async Task<IActionResult> SendOrderAsync(int oId)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            SalesOrderMaster cd = sMService.GetSalesById(oId);
            CustomerMaster cm = cService.GetCustomerById((int)cd.CustomerId);
            List<SalesOrderDetails> finallist = sDService.GetSoDetailsOfMaster(oId);
            OrderModerForMail QmForMail = new OrderModerForMail();

            QmForMail.Orders = finallist;
            QmForMail.custMaster = cm;
            QmForMail.sExecutive = uService.GetUserById(loginCheckResponse.userId);
            QmForMail.companyDetails = companyService.GetFirstCompany();
            QmForMail.imgURL = _config.GetValue<string>("OtherConfig:ProductImatesURL");
            QmForMail.roundedValue = cd.RoundedValue;
            QmForMail.delivaryCharges = cd.DelivaryCharges == null ? 0 : (decimal)cd.DelivaryCharges;
            string filepath = string.Empty;
            try
            {
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "Pdfs");
                string fileName = "Order_" + cm.FullName;
                char[] charsToTrim = { ' ', '.' };
                fileName = fileName.Trim(charsToTrim);
                fileName = fileName.Replace(" ", "");
                var pdfname = String.Format("{0}.pdf", fileName + cm.Cid.ToString());
                var path = Path.Combine(uploads, pdfname);
                path = Path.GetFullPath(path);
                filepath = path;
                var pdf = new ViewAsPdf("OrderModel", QmForMail)
                {
                    PageMargins = { Left = 20, Bottom = 5, Right = 20, Top = 5 },
                    SaveOnServerPath = path

                };
                byte[] pdfTosend = await pdf.BuildFile(ControllerContext);

                try
                {
                    EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                    emailTemplate = nService.GetEmailTemplateByModule(AppSettings.EmailTemplates.SendOrderToCostomer);
                    if (emailTemplate != null)
                    {
                        string emailCC = _config.GetValue<string>("OtherConfig:QuoteEmailCC");
                        string emailText = emailTemplate.EmailContent;

                        emailText = emailText.Replace("##UserName##", cm.FullName);

                        string emailimagesurl = _config.GetValue<string>("EmailConfig:EMAILIMAGEURL");
                        string smtpserver = _config.GetValue<string>("EmailConfig:smtpServer");
                        string smtpUsername = _config.GetValue<string>("EmailConfig:smtpEmail");
                        string smtpPassword = _config.GetValue<string>("EmailConfig:smtppassword");
                        int smtpPort = _config.GetValue<int>("EmailConfig:portNumber");

                        emailText = emailText.Replace("##EMAILIMAGES##", emailimagesurl);
                        MailMessage msg = new MailMessage(smtpUsername, cm.EmailId, emailTemplate.Subject, emailText);

                        MailMessage mail = new MailMessage();
                        mail.To.Add(cm.EmailId);
                        mail.CC.Add(emailCC);
                        if (QmForMail.sExecutive.OfficialEmail != null)
                        {
                            mail.CC.Add(QmForMail.sExecutive.OfficialEmail);
                        }
                        mail.From = new MailAddress(smtpUsername);
                        mail.Subject = emailTemplate.Subject;
                        mail.Body = emailText;
                        mail.IsBodyHtml = true;
                        if (pdfTosend != null)
                        {
                            var att = new Attachment(new MemoryStream(pdfTosend), "Order.pdf");
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
                            uService.LogError(ex);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("SaleOrders");
        }
        public IActionResult Custom()
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
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }

            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);

            List<SaleOrderMasterModel> mylist = new List<SaleOrderMasterModel>();
            mylist = cService.GetMySaleOrdersCustom(loginCheckResponse.userId);

            return View(mylist);
        }
        public IActionResult SaleOrderDetail(int oId)
        {
            LoginResponse loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            SalesOrderMaster cd = sMService.GetSalesById(oId);
            CustomerMaster cm = cService.GetCustomerById((int)cd.CustomerId);
            List<SalesOrderDetails> finallist = sDService.GetSoDetailsOfMaster(oId);
            OrderModerForMail QmForMail = new OrderModerForMail();

            QmForMail.Orders = finallist;
            QmForMail.custMaster = cm;
            QmForMail.sExecutive = uService.GetUserById(loginCheckResponse.userId);
            QmForMail.companyDetails = companyService.GetFirstCompany();
            QmForMail.imgURL = _config.GetValue<string>("OtherConfig:ProductImatesURL");
            QmForMail.roundedValue = cd.RoundedValue;
            QmForMail.delivaryCharges = cd.DelivaryCharges == null ? 0 : (decimal)cd.DelivaryCharges;
            return View(QmForMail);
        }
        public IActionResult MyCart(string emsg="")
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
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);
            ViewBag.errorMsg = emsg;
            return View(cService.GetMyCart(sCustomer.Cid, loginCheckResponse.userId));
        }

        public IActionResult ProductDetails(int id=0)
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
            var ctdrs = comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            //InventoryMaster myObj = new InventoryMaster();
            //myObj = iService.GetInentoryById(id);
            SalesProductDetailModel finalObject = iService.GetProductForSales(id);
            //ViewBag.ps = pageSize;
            //InventoryMasterModel finalObject = new InventoryMasterModel();
            //if (id > 0)
            //{
            //    CloneObjects.CopyPropertiesTo(myObj, finalObject);
            //}
            //finalObject.categoryDrops = comService.GetCatsDrop();
            //finalObject.conditionDrops = comService.GetConditionsDrop();
            //finalObject.inventoryStatusDrops = comService.GetInventoryStatusDrop();
            //finalObject.locationDrops = comService.GetLoctionDrop();
            //finalObject.gstDrop = comService.GetAllGST();
            //int cCatid = finalObject.CategoryId;
            //finalObject.subCategoryDrops = comService.GetSubCatsDrop(cCatid);
            finalObject.DocsUploaded = iService.InventoryDocsUploaded(id);
            finalObject.OtherImages = iService.InventoryOtherImgsUploaded(id);
            string[] mainImg = finalObject.PrimaryImage.Split(",");
            finalObject.MainImage1 = mainImg[0];
            finalObject.MainImagesList = new List<string>();
            int count = mainImg.Count();
            if (count > 1)
            {
                for (int i = 1; i < count; i++)
                {
                    finalObject.MainImagesList.Add(mainImg[i]);
                }
            }
            return View(finalObject); 
        }
        public IActionResult Search(int catId = 0, int subCatId = 0, string search="", int pageNumber = 1, int pageSize = 12)
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
            var ctdrs= comService.GetCategorysWithSubCat();
            ViewBag.cats = ctdrs;
            ViewBag.subCts = comService.GetSubCatsDrop(ctdrs[0].CategoryId);
            if(catId>0 )
            {
                ViewBag.subCts = comService.GetSubCatsDrop(catId);
            }
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }
            ViewBag.sC = sCustomer; ViewBag.CartCount = cService.GetUserCartCount(sCustomer.Cid,loginCheckResponse.userId);
            //string[] ctd = catDet.Split(",");
            //long cId = long.Parse(ctd[0]);
            //int catId = Convert.ToInt32(cId);
            //long sId = long.Parse(ctd[1]);
            //int subcatId = Convert.ToInt32(sId);
            List<ProductsDisplaySales> result = new List<ProductsDisplaySales>();
            result = sMService.SearchProducts(catId,subCatId, search,pageNumber,pageSize);
            ViewBag.cid = catId;
            ViewBag.sCid = subCatId;
            ViewBag.pn = pageNumber;
            ViewBag.ps = pageSize;
            if (search == null)
            {
                search = "";
            }
            ViewBag.ser = search;
            ViewBag.totProds = sMService.SearchProdsCount(catId,subCatId, search);
            return View(result);
        }
        public IActionResult LoadMoreSearchProducts(int catid = 0,int subId=0,string search="", int pageNumber = 1, int pageSize = 12)
        {
            List<ProductsDisplaySales> response = new List<ProductsDisplaySales>();
            response = sMService.SearchProducts(catid,subId,search, pageNumber, pageSize);
            foreach (ProductsDisplaySales p in response)
            {
                string[] m = p.MainImages.Split(",");
                p.MainImages = m[0];
            }
            return Json(new { re = response });
        }
        public IActionResult Logout()
        {
            try
            {
                SelectedCustomer sCustomer = new SelectedCustomer();
                sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
                CustomerSalesActivity csa = sMService.GetActivity(sCustomer.Cid);
                csa.EndTime = DateTime.Now;
                sMService.UpdateActivty(csa);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "SelectedCustomer", null);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
               
                return RedirectToAction("Index");
            }
            
        }

        public PartialViewResult RendorCart()
        {
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
                sCustomer.statusMessage = "NA";
            }

            return PartialView(cService.GetMyCart(sCustomer.Cid, 0));
        }


        public ActionResult AddToCart(int inventoryid,int qty)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                LoginResponse loginCheckResponse = new LoginResponse();
                loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
                if (loginCheckResponse == null)
                {
                    loginCheckResponse = new LoginResponse();
                    loginCheckResponse.userId = 0;
                    loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
                }
               


                SelectedCustomer sCustomer = new SelectedCustomer();
                sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
                if (sCustomer == null)
                {
                    sCustomer = new SelectedCustomer();
                    sCustomer.statusCode = 0;
                    sCustomer.statusMessage = "NA";
                }

                InventoryMaster invmaster = iService.GetInentoryById(inventoryid);
                if (inventoryid > 0 && qty > 0 && sCustomer.Cid > 0 && qty<= invmaster.Qty)
                {
                    CartMasterEntity cm = new CartMasterEntity();
                    cm.OrderType = "Regular";
                    //var invmaster = iService.GetInentoryById(inventoryid);
                    int totalAvailablQty = (int) invmaster.ShowroomQty + (int) invmaster.WharehouseQty;
                    if(qty > totalAvailablQty)
                    {
                        cm.OrderType = "Custom";
                    }
                   
                    cm.CreatedByName = loginCheckResponse.userName;
                    cm.CreatedOn = DateTime.Now;
                    cm.CreatedById = loginCheckResponse.userId;
                    cm.CurrentStatus = "Open";
                    cm.CustomerId = sCustomer.Cid;
                    cm.IsDeleted = false;

                    var r = cService.SaveCartMaster(cm);
                    if(r.currentId > 0)
                    {
                        if (iService.IsCart(r.currentId, inventoryid))
                        {
                            CartDetailsEntity cdm = new CartDetailsEntity();
                            cdm.CartId = r.currentId;
                            cdm.InventoryId = inventoryid;
                            cdm.InventoryImage = invmaster.PrimaryImage;
                            cdm.InventoryTitle = invmaster.Title;
                            cdm.IsDeleted = false;
                            cdm.ItemPrice = invmaster.MRPPrice;
                            cdm.Qty = qty;
                            cdm.TotalPrice = cdm.Qty * cdm.ItemPrice;
                            cdm.Height = invmaster.Height;
                            cdm.Width = invmaster.Width;
                            cdm.Breadth = invmaster.Breadth;
                            cdm.ColorImage = invmaster.ColorImage;
                            cdm.ColorName = invmaster.ColorName;

                            var rd = cService.SaveCartDetails(cdm);
                            response.statusCode = 1;
                        }
                        else
                        {
                            response.statusCode = 0;
                            response.statusMessage = "Already in Cart";
                        }
                        //var inventory = iService.GetInentoryById(inventoryid);
                    }
                }
                else
                {
                    if(qty > invmaster.Qty)
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Exceding available Quantity";
                    }
                    else
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Select Item";
                    }
                }
            }
            catch(Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Select Item";
            }
            return Json(new { result = response });
        }
        public ActionResult UpdateCart(int detailId, int qty)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                LoginResponse loginCheckResponse = new LoginResponse();
                loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
                if (loginCheckResponse == null)
                {
                    loginCheckResponse = new LoginResponse();
                    loginCheckResponse.userId = 0;
                    loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
                }



                SelectedCustomer sCustomer = new SelectedCustomer();
                sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
                if (sCustomer == null)
                {
                    sCustomer = new SelectedCustomer();
                    sCustomer.statusCode = 0;
                    sCustomer.statusMessage = "NA";
                }

                if (detailId > 0 && qty > 0)
                {
                    CartDetailsEntity cd = cService.GetCartDetailById(detailId);
                    InventoryMaster im = iService.GetInentoryById((int)cd.InventoryId);
                    if (im.Qty >= qty)
                    {
                        if (qty != cd.Qty)
                        {
                            cd.Qty = qty;
                            cd.TotalPrice = cd.ItemPrice * qty;
                            cService.UpdateCartDetails(cd);
                            response.statusCode = 1;
                            response.statusMessage = "Successfully Updated";
                        }
                        else
                        {
                            response.statusCode = 0;
                            response.statusMessage = "No changes made";
                        }
                    }
                    else
                    {
                        response.statusCode = 0;
                        response.statusMessage = "limit exceeds available qty";
                    }
                   
                }
                else
                {
                    response.statusCode = 0;
                    response.statusMessage = "Select Item";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Select Item";
            }
            return Json(new { result = response });
        }
        public ActionResult DeleteCart(int id)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                LoginResponse loginCheckResponse = new LoginResponse();
                loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
                if (loginCheckResponse == null)
                {
                    loginCheckResponse = new LoginResponse();
                    loginCheckResponse.userId = 0;
                    loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
                }



                SelectedCustomer sCustomer = new SelectedCustomer();
                sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
                if (sCustomer == null)
                {
                    sCustomer = new SelectedCustomer();
                    sCustomer.statusCode = 0;
                    sCustomer.statusMessage = "NA";
                }

                if (id > 0)
                {
                    CartDetailsEntity cd = cService.GetCartDetailById(id);
                    if (cd != null)
                    {
                        cd.IsDeleted = true;
                        cService.UpdateCartDetails(cd);
                        response.statusCode = 1;
                        response.statusMessage = "Successfully Deleted";
                    }
                    else
                    {
                        response.statusCode = 0;
                        response.statusMessage = "No changes made";
                    }

                }
                else
                {
                    response.statusCode = 0;
                    response.statusMessage = "Select Item";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Select Item";
            }
            return RedirectToAction("MyCart");
        }

        public ActionResult CreateCRFQ(decimal seDisPercen=0)
        {
            if (seDisPercen < 0)
            {
                string emsg = "Discount Percentage can not be negative";
                return RedirectToAction("MyCart", new { emsg });
            }
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser"); 
            SelectedCustomer sCustomer = new SelectedCustomer();
            sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (uService.GetUserById(loginCheckResponse.userId).MaxDiscoutPercentage >= seDisPercen) {
                try
                {
                    if (sCustomer.Cid > 0)
                    {
                        var cartDetails = cService.GetMyCart(sCustomer.Cid, loginCheckResponse.userId);
                        if (cartDetails != null)
                        {
                            //save crfq first 
                            CRFQMaster crfq = new CRFQMaster();
                            crfq.ClosedBy = null;
                            crfq.ClosedOn = null;
                            crfq.CreatedBy = loginCheckResponse.userName;
                            crfq.CreatedOn = DateTime.Now;
                            crfq.CurrentStatus = "Open";
                            crfq.CustomerId = sCustomer.Cid;
                            crfq.IsDeleted = false;
                            crfq.StaffId = loginCheckResponse.userId;
                            var crfqSave = cService.SaveCRFQ(crfq);
                            if (crfqSave.currentId > 0)
                            {
                                foreach (var d in cartDetails.cartDetails)
                                {
                                    CRFQDetails cd = new CRFQDetails();
                                    cd.CRFQId = crfqSave.currentId;
                                    cd.DisAmtByHead = 0;
                                    cd.AdminDiscount = 0;
                                    cd.DisAmtBySE = seDisPercen;
                                    cd.InventoryId = d.InventoryId;
                                    cd.InventoryImage = d.InventoryImage;
                                    cd.InventoryTitle = d.InventoryTitle;
                                    cd.IsDeleted = false;
                                    cd.ItemPrise = d.ItemPrice;
                                    cd.Quantity = d.Qty;
                                    cd.OrderLineTotal = d.Qty * d.ItemPrice;
                                    cd.TotalPrice = d.TotalPrice - ((d.TotalPrice * (cd.DisAmtBySE + cd.DisAmtByHead)) / 100);
                                    cd.ColorName = d.ColorName;
                                    cd.Breadth = d.Breadth;
                                    cd.ColorImage = d.ColorImage;
                                    cd.Height = d.Height;
                                    cd.Width = d.Width;
                                    var cdRes = cService.SaveCRFQDetails(cd);
                                }
                                // update cartmaster
                                var ps = cService.CloseCartMaster(cartDetails.CartId);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("MyQuote");
            }
            else
            {
                string emsg = "Discount Percentage Exceded the limit";
                return RedirectToAction("MyCart", new { emsg });
            }
        }

        [HttpPost]
        public ActionResult SendQuoteForDiscount(int crfqId, int currentUserId)
        {
            QuotesSubmittedForApproval qa = new QuotesSubmittedForApproval();
            CRFQMasterModel cDetails = cService.GetSingleCRFQ(crfqId);
            UserMaster um = uService.GetUserById(currentUserId);
            if (cDetails != null)
            {
                qa.CurrentStatus = "Submitted for Discount";
                qa.DiscountGiven = 0;
                qa.AdminDiscount = 0;
                qa.GivenBy = null;
                qa.GivenOn = null;
                qa.IsDeleted = false;
                qa.OrderValue = cDetails.crfqDetails.Sum(a => a.TotalPrice);
                qa.SalesExecDiscount = um.MaxDiscoutPercentage;
                qa.SubmittedBy = um.UserName;
                qa.SubmittedById = um.UserId;
                qa.SubmittedOn = DateTime.Now;
                qa.QuoteMasterId = cDetails.CRFQId;
                cService.SubmitQuoteForDiscount(qa);

                // activity log

                ActivityLog al = new ActivityLog();
                al.ActivityBy = um.UserId;
                al.ActivityByName = um.UserName;
                al.ActivityOn = DateTime.Now;
                al.ActivityStatus = "Open";
                al.ActivityText = um.UserName + " have submitted Quotation for Discount";
                al.IsDeleted = false;
                al.IsRead = false;
                al.ModuleName = "Quotes";
                al.ReferenceId = qa.QuoteMasterId;
                al.TargerUserId = um.ReportingManager;
                 uService.UpdateActivityLog(al);

                //update quote status;

                CRFQMaster cm = cService.GetCRFQMaster((int)qa.QuoteMasterId);
                cm.CurrentStatus = "Submitted For Discount";
                var r = cService.SaveCRFQ(cm);
            }

            return Json("Ok");
        }
        public ActionResult SendQuoteForAdminDiscount(int crfqId, int currentUserId)
        {
            QuotesSubmittedForApproval qa = new QuotesSubmittedForApproval();
            CRFQMasterModel cDetails = cService.GetSingleCRFQ(crfqId);
            UserMaster um = uService.GetUserById(currentUserId);
            if (cDetails != null)
            {
                qa.CurrentStatus = "Submitted for Extra Discount";
                qa.DiscountGiven = 0;
                qa.GivenBy = null;
                qa.GivenOn = null;
                qa.IsDeleted = false;
                qa.OrderValue = cDetails.crfqDetails.Sum(a => a.TotalPrice);
                qa.SalesExecDiscount = um.MaxDiscoutPercentage;
                qa.DiscountGiven = cService.GetQuoteExtraDisc(crfqId);
                qa.SubmittedBy = um.UserName;
                qa.SubmittedById = um.UserId;
                qa.SubmittedOn = DateTime.Now;
                qa.QuoteMasterId = cDetails.CRFQId;
                cService.SubmitQuoteForDiscount(qa);

                // activity log

                ActivityLog al = new ActivityLog();
                al.ActivityBy = um.UserId;
                al.ActivityByName = um.UserName;
                al.ActivityOn = DateTime.Now;
                al.ActivityStatus = "Open";
                al.ActivityText = um.UserName + " have submitted Quotation for More Discount";
                al.IsDeleted = false;
                al.IsRead = false;
                al.ModuleName = "Quotes";
                al.ReferenceId = qa.QuoteMasterId;
                al.TargerUserId = uService.GetAdminId();
                uService.UpdateActivityLog(al);

                //update quote status;

                CRFQMaster cm = cService.GetCRFQMaster((int)qa.QuoteMasterId);
                cm.CurrentStatus = "Submitted For Extra Discount";
                var r = cService.SaveCRFQ(cm);
            }

            return Json("Ok");
        }
        public IActionResult UpdateQuoteDisc(decimal extraDisc,int qId)
        {
            LoginResponse loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            List<CRFQDetails> myList = cService.GetCRFQDetailsByCRFQId(qId);
            ProcessResponse pr = new ProcessResponse();
            foreach(CRFQDetails c in myList)
            {
                decimal a = (decimal)c.DisAmtBySE + extraDisc;
                if(a<= uService.GetUserById(loginCheckResponse.userId).MaxDiscoutPercentage)
                {
                    c.DisAmtBySE = a;
                    c.TotalPrice = (c.OrderLineTotal -((c.DisAmtBySE*c.OrderLineTotal)/100));
                    pr=cService.SaveCRFQDetails(c);
                }
                else
                {
                    pr.statusCode = 0;
                    pr.statusMessage = "Discount Limit Exceded";
                }
            }
            return RedirectToAction("MyQuote", new {ecode=pr.statusMessage });
        }

        [HttpPost]
        public ActionResult CreateSaleOrder(CreateSaleOrderModel request)
        {
            CRFQMasterModel myList = new CRFQMasterModel();
            myList = cService.GetSingleCRFQ(request.CRFQId);
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            int x = 0;
            bool sh = false;
            bool wh = false;
            
            if (myList!= null)
            {
                //List<InventoryMaster> invLocal = new List<InventoryMaster>();
                //foreach (var v in myList.crfqDetails)
                //{
                //    InventoryMaster im = iService.GetInentoryById((int)v.InventoryId);
                //    invLocal.Add(im);
                //}
                foreach (var v in myList.crfqDetails)
                {
                    InventoryMaster im = iService.GetInentoryById((int)v.InventoryId);
                    if (request.SelShowRoomQty[x] > 0 && im.ShowroomQty >= request.SelShowRoomQty[x])
                    {
                        //invLocal[x].ShowroomQty -= request.SelShowRoomQty[x];
                        sh = true;
                    }
                    if (request.SelWareHouseQty[x] > 0 && im.WharehouseQty >= request.SelWareHouseQty[x])
                    {
                        //invLocal[x].WharehouseQty -= request.SelWareHouseQty[x];
                        wh = true;
                    }
                    if ((request.SelWareHouseQty[x] + request.SelShowRoomQty[x]) != v.Quantity)
                    {
                        sh = false;
                        wh = false;
                    }
                    if (sh == false && wh == false)
                    {
                        string emg = "Please Select correct quantity for all products";
                        return RedirectToAction("MyQuote", new { emg = emg });
                    }
                    x++;
                    sh = false;
                    wh = false;
                }
                //GSTMaster gm = new GSTMaster();
                //gm = gstService.GetGSTById(request.TaxId);
                AddressMaster add = new AddressMaster();
                add = cService.GetUserAddresses((int)myList.CustomerId).FirstOrDefault();
                SalesOrderMaster so = new SalesOrderMaster();
                so.CreatedBy = loginCheckResponse.userName;
                so.CreatedOn = DateTime.Now;
                so.CurrentStatus = "Open";
                so.SOStatus = "Open";
                so.CustomerId = myList.CustomerId;
                so.IsDeleted = false;
                so.QuoteId = myList.CRFQId;
                so.StaffId = loginCheckResponse.userId;
                so.ShippingAddressId = add.Addid;
                so.TaxAmount = 0;
                so.TaxApplicable = "";
                so.TaxPercentage = 0;
                so.DelivaryCharges = request.DelivaryCharges == null ? 0 : request.DelivaryCharges;
                so.RoundedValue = request.RoundOff;
                so.OrderType = "Regular";
                var sR = sMService.SaveSOMaster(so);
                x = 0;
                if (sR.currentId > 0)
                {
                    foreach (var v in myList.crfqDetails)
                    {
                        SalesOrderDetails sd = new SalesOrderDetails();
                        CloneObjects.CopyPropertiesTo(v, sd);
                        InventoryMaster im = iService.GetInentoryById((int)v.InventoryId);
                        sd.ItemPrice = v.ItemPrise;
                        sd.AdminDiscount = v.AdminDiscount;
                        sd.IsDeleted = false;
                        sd.LastMOdifiedBy = loginCheckResponse.userName;
                        sd.LastModifiedOn = DateTime.Now;
                        sd.SOMId = sR.currentId;
                        sd.DeliveryDate = null;
                        sd.ShippingAddressId = add.Addid;
                        sd.TaxPercentage= gstService.GetGstPercentBySubCatId((int)im.SubCategoryId);
                        sd.TaxDeducted = ((sd.TotalPrice * sd.TaxPercentage) / (100 + sd.TaxPercentage));
                        sd.TaxAmount = sd.TotalPrice - sd.TaxDeducted;
                        var sdR = sDService.SaveSODetails(sd);

                        //Update Inventory and create reserved qty

                        ReservedQtyMaster rqm = new ReservedQtyMaster();
                        rqm.ProductId = v.InventoryId;
                        rqm.Quantity = (int)v.Quantity;
                        rqm.IsDeleted = false;
                        rqm.CurrentStatus = "SO Created";
                        rqm.DCmId = 0;
                        rqm.StandById = 0;
                        rqm.WQty = request.SelWareHouseQty[x];
                        rqm.SQty = request.SelShowRoomQty[x];
                        rqm.SOMId = sR.currentId;
                        rqm.SalesExename = loginCheckResponse.userName;
                        cService.SaveReservedQtyMaster(rqm);

                        im.Qty -= (int)v.Quantity;

                        if (request.SelShowRoomQty[x] > 0)
                        {
                            im.ShowroomQty -= request.SelShowRoomQty[x];
                        }
                        if (request.SelWareHouseQty[x] > 0)
                        {
                            im.WharehouseQty -= request.SelWareHouseQty[x];
                        }
                        iService.UpdateInventory(im);
                        x++;
                    }
                }
                
                               
                //update crfq quote

                CRFQMaster cm = cService.GetCRFQMaster(myList.CRFQId);
                cm.CurrentStatus = "SO Created";
                var r = cService.SaveCRFQ(cm);
            }
            return RedirectToAction("SaleOrders");
        }       
        //public ActionResult SendSOEmail(int soid)
        //{

        //    return RedirectToAction("SaleOrders");
        //}

        public ActionResult SubmitForPayment(int soid)
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
            var res = sMService.SubmitSOForAccounts(soid, loginCheckResponse.userId);
            ViewBag.ErrorMessage = "Successfully submitted";
            return RedirectToAction("SaleOrders");
        }
        public IActionResult CloneCart(int crId)
        {
            LoginResponse loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            CRFQMaster master = cService.GetCRFQMaster(crId);
            List<CRFQDetails> detList = cService.GetCRFQDetailsByCRFQId(crId);

            SelectedCustomer sCustomer = SessionHelper.GetObjectFromJson<SelectedCustomer>(HttpContext.Session, "SelectedCustomer");
            if (sCustomer == null)
            {
                sCustomer = new SelectedCustomer();
                sCustomer.statusCode = 0;
            }
            if (sCustomer.Cid != master.CustomerId)
            {
                return RedirectToAction("MyQuote");
            }
            else
            {
                CartMasterModel crt = cService.GetMyCart(sCustomer.Cid,loginCheckResponse.userId);
                if (crt == null)
                {
                    CartMasterEntity cm = new CartMasterEntity();
                    cm.OrderType = "Regular";                    
                    cm.CreatedByName = loginCheckResponse.userName;
                    cm.CreatedOn = DateTime.Now;
                    cm.CreatedById = loginCheckResponse.userId;
                    cm.CurrentStatus = "Open";
                    cm.CustomerId = sCustomer.Cid;
                    cm.IsDeleted = false;
                    var r = cService.SaveCartMaster(cm);
                    crt = cService.GetMyCart(sCustomer.Cid,loginCheckResponse.userId);
                }
                foreach(CRFQDetails d in detList)
                {
                    CartDetailsEntity cdm = new CartDetailsEntity();
                    cdm.CartId = crt.CartId;
                    cdm.InventoryId = d.InventoryId;
                    cdm.InventoryImage = d.InventoryImage;
                    cdm.InventoryTitle = d.InventoryTitle;
                    cdm.IsDeleted = false;
                    cdm.ItemPrice = d.ItemPrise;
                    cdm.Qty = (int)d.Quantity;
                    cdm.TotalPrice = cdm.Qty * cdm.ItemPrice;
                    cdm.Height = d.Height;
                    cdm.Width = d.Width;
                    cdm.Breadth = d.Breadth;
                    cdm.ColorImage = d.ColorImage;
                    cdm.ColorName = d.ColorName;

                    var rd = cService.SaveCartDetails(cdm);
                }
                return RedirectToAction("MyCart");
            }
            
        }
        public IActionResult UpdateRoundUpValue(decimal rounded,int sOId)
        {
            SalesOrderMaster som = sMService.GetSalesById(sOId);
            som.RoundedValue = rounded;
            sMService.SaveSOMaster(som);
            return RedirectToAction("SaleOrders");
        }

    }
}
