using Microsoft.AspNetCore.Mvc;
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
    public class UserController : Controller
    {
        private readonly ICustomerMgmtService cService;
        private readonly ICommonService comService;

        public UserController(ICustomerMgmtService _cService, ICommonService _comService)
        {
            cService = _cService;
            comService = _comService;
        }
        public IActionResult CustomerList(int pageNumber=1,int pageSize=10,string search="")
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
            List<CustomerDisplayModel> cm = new List<CustomerDisplayModel>();
            cm = cService.CustomerList();

            return View(cm);
        }
        public IActionResult CustomerData(int id=0)
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
            CustomerDisplayModel cm = new CustomerDisplayModel();
            CustomerMaster c=new CustomerMaster();
            
            if(id>0)
            {
                c = cService.GetCustomerById(id);
                CloneObjects.CopyPropertiesTo(c, cm);
            }
           
            return View(cm);
        }
        [HttpPost]
        public IActionResult CustomerData(CustomerDisplayModel request)
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
            CustomerMaster c = new CustomerMaster();
            //CustomerDisplayModel cm = new CustomerDisplayModel();
            ProcessResponse pr = new ProcessResponse();
            if (ModelState.IsValid)
            {
                if(request.Cid>0)
                {
                    
                    //CloneObjects.CopyPropertiesTo(c, cm);
                    //cm.FullName = request.FullName;
                    //cm.EmailId = request.EmailId;
                    //cm.MobileNumber = request.MobileNumber;
                    //cm.OrganigationName = request.OrganigationName;
                    //cm.OrganizationEmailId = request.OrganizationEmailId;
                    //cm.OrganizationNumber = request.OrganizationNumber;
                    //cm.OrginizationAddress = request.OrginizationAddress;
                    //cm.WhatsAppNumber = request.WhatsAppNumber;
                    //cm.CoustemerCode = request.CoustemerCode;
                    //cm.GSTNumber = request.GSTNumber;
                    pr = cService.SaveCustomer(request);
                }
                else
                {
                    //CloneObjects.CopyPropertiesTo(request, cm);
                    //request.RefferedBy = loginCheckResponse.userName;
                    request.RegisteredOn = DateTime.Now;
                    pr = cService.SaveCustomer(request);
                }
                //ProcessResponse pr = cService.SaveCustomer(request);
                if(pr.statusCode==1)
                {
                    return RedirectToAction("CustomerList");
                }
            }
            return View(request);
        }

        public IActionResult DeleteCustomer(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            CustomerMaster cm = new CustomerMaster();
            CustomerDisplayModel cd = new CustomerDisplayModel();
            cm = cService.GetCustomerById(id);
            cm.IsDeleted = true;
            CloneObjects.CopyPropertiesTo(cm, cd);
            pr = cService.SaveCustomer(cd);

            return Json(new { result = pr });
        }

        public IActionResult GetAddress(int id)
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
            List<CustomerAddressModel> ad = new List<CustomerAddressModel>();
            ad = cService.GetAddress(id);

            ViewBag.cusId = id;
            return View(ad);
        }
        public IActionResult AddressData(int id,int cid=0)
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
            CustomerAddressModel ca = new CustomerAddressModel();
            AddressMaster am = new AddressMaster();
            if(id>0)
            {
                am = cService.GetAddressById(id);
                CloneObjects.CopyPropertiesTo(am, ca);
            }
            ca.Cid = cid;
            ca.countryDrops = comService.GetAllCountries();
            int countryId = id > 0 ? (int)ca.CountryId : 101;
            ca.stateDrops = comService.GetAllStates(countryId);
            int stateId = id > 0 ? (int)ca.StateId : ca.stateDrops[0].StateId;
            ca.cityDrops = comService.GetAllCities(stateId);
            return View(ca);
        }
        [HttpPost]
        public IActionResult AddressData(CustomerAddressModel request)
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
            AddressMaster am = new AddressMaster();
            CustomerAddressModel cm = new CustomerAddressModel();
            ProcessResponse pr = new ProcessResponse();
            if (ModelState.IsValid)
            {
                if(request.Addid>0)
                {
                    am = cService.GetAddressById(request.Addid);
                    CloneObjects.CopyPropertiesTo(am, cm);
                    cm.HouseNumber = request.HouseNumber;
                    cm.StreetName = request.StreetName;
                    cm.Location = request.Location;
                    cm.AddressType = request.AddressType;
                    cm.CityId = request.CityId;
                    cm.StateId = request.StateId;
                    cm.CountryId = request.CountryId;
                    cm.PostalCode = request.PostalCode;

                    pr = cService.SaveAddress(cm);
                }
                else
                {
                    CloneObjects.CopyPropertiesTo(request, cm);
                    pr = cService.SaveAddress(cm);
                }
              
                if(pr.statusCode==1)
                {
                    return RedirectToAction("GetAddress",new { id = am.CustomerId } );
                }
            }
            request.countryDrops = comService.GetAllCountries();
            request.stateDrops = comService.GetAllStates((int)request.CountryId);
            request.cityDrops = comService.GetAllCities((int)request.StateId);

            return View(request);
        }

        [HttpPost]
        public IActionResult AddLedger(int cid)
        {
            ProcessResponse pr = new ProcessResponse(); 
            pr = cService.CreateCustomerFH(cid);  
            return Json(new { result = pr });
        }
    }
}
