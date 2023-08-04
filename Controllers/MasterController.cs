using SCASA.Models.ModelClasses;
using Microsoft.AspNetCore.Mvc;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCASA.Models.Utilities;

namespace SCASA.Controllers
{
    public class MasterController : Controller
    {
        private readonly IUserMgmtService uService;
        private readonly ICategoryMgmtService ctgService;
        private readonly IInventoryLocationMgmtService locService;
        private readonly IInventoryConditionMgmtService conService;
        private readonly IInventoryStatusMgmtService stService;
        private readonly IMWorkingDaysMgmtService mService;
        private readonly IModuleMgmtService mdService;
        private readonly IPrevilegeMasterMgmtService pService;
        private readonly IHolidayMgmtService hService;
        private readonly IShiftMasterMgmtService shService;
        private readonly ICompanyMgmtService comService;
        private readonly IGSTMasterMgmtService gService;
        private readonly IFinanceMgmtService fService;
        private readonly IMRPFactorMgmtService mfService;
        private readonly ICommonService commonService;

        public MasterController(IUserMgmtService _uService, ICategoryMgmtService _ctgServices, IInventoryLocationMgmtService _locService,
            IInventoryConditionMgmtService _conService, IInventoryStatusMgmtService _stServices, IMWorkingDaysMgmtService _mService, IModuleMgmtService _mdService,
            IPrevilegeMasterMgmtService _pService, IHolidayMgmtService _hService, IShiftMasterMgmtService _shService, ICompanyMgmtService _comServoce,
            IGSTMasterMgmtService _gService, IFinanceMgmtService _fService, IMRPFactorMgmtService _mfService,ICommonService _commonService)
        {
            uService = _uService;
            ctgService = _ctgServices;
            locService = _locService;
            conService = _conService;
            stService = _stServices;
            mService = _mService;
            mdService = _mdService;
            pService = _pService;
            hService = _hService;
            shService = _shService;
            comService = _comServoce;
            gService = _gService;
            fService = _fService;
            mfService = _mfService;
            commonService = _commonService;
        }
        #region UserType
        public IActionResult Usertypes()
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

            List<UserTypeMaster> myList = new List<UserTypeMaster>();
            myList = uService.GetAllUserTypes();
            return View(myList);
        }
        public IActionResult UserTypeData(int id = 0)
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

            UserTypeMaster myObject = new UserTypeMaster();
            if (id > 0)
            {
                myObject = uService.GetUserTypeById(id);
            }
            return View(myObject);
        }

        [HttpPost]
        public IActionResult UserTypeData(UserTypeMaster request)
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
                // save or update call
                ProcessResponse pr = uService.SaveUserType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Usertypes");
                }
            }
            return View(request);
        }

        [HttpPost]
        public IActionResult DeleteUserType(int id)
        {

            ProcessResponse pr = new ProcessResponse();
            UserTypeMaster um = new UserTypeMaster();
            um = uService.GetUserTypeById(id);
            um.IsDeleted = true;
            pr = uService.UpdateUserType(um);
            return Json(new { result = pr });
        }
        #endregion
        #region Category
        public IActionResult Categories()
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
            List<CategoryMaster> myList = new List<CategoryMaster>();
            myList = ctgService.GetAllCategories();
            return View(myList);
        }
        public IActionResult CategoryType(int id = 0)
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
            CategoryMaster myObject = new CategoryMaster();
            if (id > 0)
            {
                myObject = ctgService.GetCategoryById(id);
            }
            return View(myObject);
        }

        [HttpPost]
        public IActionResult CategoryType(CategoryMaster request)
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
                // save or update call
                ProcessResponse pr = ctgService.SaveCategoryType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Categories");
                }
            }
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteCatyegoryType(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            CategoryMaster cm = new CategoryMaster();
            cm = ctgService.GetCategoryById(id);
            cm.IsDeleted = true;
            pr = ctgService.UpdateCategoryType(cm);
            return Json(new { result = pr });
        }
        #endregion
        #region Subcategory
        public IActionResult SubCategories(int catId = 0)
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
            List<SubCategoryMaster> myList = new List<SubCategoryMaster>();
            myList = ctgService.GetAllSubCategories(catId);
            List<CategoryMaster> cats = new List<CategoryMaster>();
            cats = ctgService.GetAllCategories();
            ViewBag.Catgories = cats;

            return View(myList);
        }

        public IActionResult SubCategoryData(int id = 0)
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
            SubCategoryMaster myObj = new SubCategoryMaster();
            if (id > 0)
            {
                myObj = ctgService.GetSubCategoryById(id);
            }

            List<CategoryMaster> cats = new List<CategoryMaster>();
            cats = ctgService.GetAllCategories();
            ViewBag.Catgories = cats;

            return View(myObj);
        }

        [HttpPost]
        public IActionResult SubCategoryData(SubCategoryMaster request)
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
                request.IsDeleted = false;
                var res = ctgService.SaveSubCategory(request);
                return RedirectToAction("Subcategories", new { catId = request.CategoryId });
            }
            List<CategoryMaster> cats = new List<CategoryMaster>();
            cats = ctgService.GetAllCategories();
            ViewBag.Catgories = cats;

            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteSubCategory(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            SubCategoryMaster cond = new SubCategoryMaster();
            cond = ctgService.GetSubCategoryById(id);
            cond.IsDeleted = true;
            pr = ctgService.SaveSubCategory(cond);
            return Json(new { result = pr });
        }
        #endregion
        #region Condition
        public IActionResult Condition()
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
            List<InventoryConditionMaster> myList = new List<InventoryConditionMaster>();
            myList = conService.GetAllCondition();
            return View(myList);
        }
        public IActionResult ConditionTypeData(int id = 0)
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
            InventoryConditionMaster myObject = new InventoryConditionMaster();
            if (id > 0)
            {
                myObject = conService.GetAllConditionTypeById(id);
            }
            return View(myObject);
        }
        [HttpPost]
        public IActionResult ConditionTypeData(InventoryConditionMaster request)
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
                // save or update call
                ProcessResponse pr = conService.SaveConditionType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Condition");
                }
            }
            return View(request);
        }

        [HttpPost]
        public IActionResult DeleteConditionType(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            InventoryConditionMaster cond = new InventoryConditionMaster();
            cond = conService.GetAllConditionTypeById(id);
            cond.IsDeleted = true;
            pr = conService.UpdateConditionType(cond);
            return Json(new { result = pr });
        }
        #endregion
        #region Location
        public IActionResult Loctation()
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
            List<InventoryLocationMaster> mylist = new List<InventoryLocationMaster>();
            mylist = locService.GetAllLocationType();

            return View(mylist);
        }

        public IActionResult LocationTypeData(int id = 0)
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
            InventoryLocationMaster myObject = new InventoryLocationMaster();
            if (id > 0)
            {
                myObject = locService.GetAllLocationTypeById(id);

            }
            return View(myObject);
        }

        [HttpPost]
        public IActionResult LocationTypeData(InventoryLocationMaster request)
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
                // save or update call
                ProcessResponse pr = locService.SaveLocationType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Loctation");
                }
            }
            return View(request);

        }
        [HttpPost]
        public IActionResult DeleteLocationType(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            InventoryLocationMaster um = new InventoryLocationMaster();
            um = locService.GetAllLocationTypeById(id);
            um.IsDeleted = true;
            pr = locService.UpdateLocationType(um);
            return Json(new { result = pr });
        }
        #endregion
        #region Inventory
        public IActionResult InventoryStatus()
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
            List<InventoryStatusMaster> myList = new List<InventoryStatusMaster>();
            myList = stService.GetAllStatus();
            return View(myList);
        }
        public IActionResult InventoryStatusType(int id = 0)
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
            InventoryStatusMaster myObject = new InventoryStatusMaster();
            if (id > 0)
            {
                myObject = stService.GetStatusById(id);
            }
            return View(myObject);
        }

        [HttpPost]
        public IActionResult InventoryStatusType(InventoryStatusMaster request)
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
                // save or update call
                ProcessResponse pr = stService.SaveStatusType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("InventoryStatus");
                }
            }
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteStatusType(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            InventoryStatusMaster sm = new InventoryStatusMaster();
            sm = stService.GetStatusById(id);
            sm.IsDeleted = true;
            pr = stService.UpdateStatusType(sm);
            return Json(new { result = pr });
        }
        #endregion
        #region MonthlyDays
        public IActionResult MonthlyWorkingDays(int MonthNumber, int YearNumber)
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

            List<MWorkingDaysDisplay> myObj = mService.GetAllDays(MonthNumber, YearNumber);

            return View(myObj);
        }

        public IActionResult MonthlyWorkingDaysData(int id = 0)
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
            MonthlyWorkingDays myObj = new MonthlyWorkingDays();


            return View(myObj);

        }

        [HttpPost]
        public IActionResult MonthlyWorkingDaysData(MonthlyWorkingDays request)
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
                ProcessResponse pr = mService.SaveDays(request);

                if (pr.statusCode == 1)
                {
                    return RedirectToAction("MonthlyWorkingDays");
                }
                else
                {
                    ViewBag.ErrorMessage = pr.statusMessage; 
                    return View(request);
                }
            }

            return View(request);
        }
        #endregion
        #region Modules
        public IActionResult Modules()
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
            List<ModulesMaster> mylist = new List<ModulesMaster>();
            mylist = mdService.GetAllModules();

            return View(mylist);
        }
        public IActionResult ModuleTypedata(int id = 0)
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
            ModulesMaster myObject = new ModulesMaster();
            if (id > 0)
            {
                myObject = mdService.GetModuleTypeById(id);

            }
            return View(myObject);
        }
        [HttpPost]
        public IActionResult ModuleTypedata(ModulesMaster request)
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
                // save or update call
                ProcessResponse pr = mdService.SaveModules(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Modules");
                }
            }
            return View(request);
        }

        [HttpPost]
        public IActionResult DeleteModules(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            ModulesMaster um = new ModulesMaster();
            um = mdService.GetModuleTypeById(id);
            um.IsDeleted = true;
            pr = mdService.UpdateModules(um);

            return Json(new { result = pr });
        }
        #endregion
        #region Previlege
        public IActionResult Previleges()
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
            List<PrevilegeMaster> mylist = new List<PrevilegeMaster>();
            mylist = pService.GetAllPrevileges();

            return View(mylist);
        }

        public IActionResult PrevilegeTypedata(int id = 0)
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
            PrevilegeMaster myObject = new PrevilegeMaster();
            if (id > 0)
            {
                myObject = pService.GetPrevilegeById(id);

            }
            return View(myObject);
        }

        [HttpPost]
        public IActionResult PrevilegeTypedata(PrevilegeMaster request)
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
                PrevilegeMaster p = new PrevilegeMaster();
                if (request.PrMId > 0)
                {
                    p = pService.GetPrevilegeById(request.PrMId);
                    p.PrevilegeName = request.PrevilegeName;
                }
                else
                {
                    p = request;
                }
                ProcessResponse pr = pService.SavePrevilege(p);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Previleges");
                }
            }
            return View(request);
        }

        [HttpPost]
        public IActionResult DeletePrevilege(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            PrevilegeMaster um = new PrevilegeMaster();
            um = pService.GetPrevilegeById(id);
            um.IsDeleted = true;
            pr = pService.SavePrevilege(um);

            return Json(new { result = pr });
        }
        #endregion
        #region Holidays
        public IActionResult Holidays()
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
            List<HolidayMaster> mylist = new List<HolidayMaster>();
            mylist = hService.GetAllHolidays();

            return View(mylist);
        }

        public IActionResult HolidayTypedata(int id = 0)
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
            HolidayMaster myObject = new HolidayMaster();
            if (id > 0)
            {
                myObject = hService.GetHolidayById(id);

            }
            return View(myObject);
        }
        [HttpPost]
        public IActionResult HolidayTypedata(HolidayMaster request)
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
                // save or update call
                ProcessResponse pr = hService.SaveHolidays(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Holidays");
                }
            }
            return View(request);
        }
        
        [HttpPost]
        public IActionResult DeleteHolidays(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            HolidayMaster um = new HolidayMaster();
            um = hService.GetHolidayById(id);
            um.IsDeleted = true;
            pr = hService.SaveHolidays(um);

            return Json(new { result = pr });
        }
        #endregion
        #region Shift
        public IActionResult Shifts()
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
            List<ShiftMaster> mylist = new List<ShiftMaster>();
            mylist = shService.GetAllShifts();

            return View(mylist);
        }
        public IActionResult ShiftTypedata(int id = 0)
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
            ShiftMaster myObject = new ShiftMaster();
            if (id > 0)
            {
                myObject = shService.GetShiftById(id);

            }
            return View(myObject);
        }
        [HttpPost]
        public IActionResult ShiftTypedata(ShiftMaster request)
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
                // save or update call
                ProcessResponse pr = shService.SaveShift(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Shifts");
                }
            }
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteShifts(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            ShiftMaster um = new ShiftMaster();
            um = shService.GetShiftById(id);
            um.IsDeleted = true;
            pr = shService.SaveShift(um);

            return Json(new { result = pr });
        }
        #endregion
        #region Company
        public IActionResult Company()
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

            List<CompanyDisplayModel> myObj = comService.GetAllCompnies();
            return View(myObj);
        }
        public IActionResult CompanyData(int id = 0)
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

            CompanyMaster obj = new CompanyMaster();
            if (id > 0)
            {
                obj = comService.GetCompanyById(id);
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult CompanyData(CompanyMaster request)
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
                // save or update call
                ProcessResponse pr = comService.SaveCompnyType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Company");
                }
            }
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteCompany(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            CompanyMaster cm = new CompanyMaster();
            cm = comService.GetCompanyById(id);
            cm.IsDeleted = true;
            pr = comService.SaveCompnyType(cm);
            return Json(new { result = pr });
        }
        #endregion
        #region GST
        public IActionResult GST()
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

            List<GSTListModel> myObj = gService.GetGstModelList();
            return View(myObj);

        }
        public IActionResult GSTData(int id = 0)
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

            GSTMasterModel obj = new GSTMasterModel();
            obj.CatDros = commonService.GetCatsDrop();
            obj.SubCatDros = commonService.GetSubCatsDrop(obj.CatDros[0].CategoryId);
            if (id > 0)
            {
                GSTMaster tem = gService.GetGSTById(id);
                CloneObjects.CopyPropertiesTo(tem, obj);
                obj.CatDros = commonService.GetCatsDrop();
                obj.SubCatDros = commonService.GetSubCatsDrop(tem.CategoryId);
            }
            
            return View(obj);
        }
        [HttpPost]
        public IActionResult GSTData(GSTMasterModel request)
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
                // save or update call
                GSTMaster saveobj = new GSTMaster();
                ProcessResponse pr = new ProcessResponse();
                if (request.GSTMasterId > 0)
                {
                    saveobj = gService.GetGSTById(request.GSTMasterId);
                    saveobj.CategoryId = request.CategoryId;
                    saveobj.SubCategoryId = request.SubCategoryId;
                    saveobj.TaxName = request.TaxName;
                    saveobj.TaxAmount = request.TaxAmount;
                    pr = gService.SaveGST(saveobj);
                }
                else
                {
                    CloneObjects.CopyPropertiesTo(request,saveobj);
                    pr = gService.SaveGST(saveobj);
                }
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("GST");
                }
            }
            request.CatDros = commonService.GetCatsDrop();
            request.SubCatDros = commonService.GetSubCatsDrop(request.CategoryId);
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteGST(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            GSTMaster cm = new GSTMaster();
            cm = gService.GetGSTById(id);
            cm.IsDeleted = true;
            cm.IsEnabled = false;
            pr = gService.SaveGST(cm);
            return Json(new { result = pr });
        }

        public IActionResult GSTStatus(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            GSTMaster cm = new GSTMaster();
            cm = gService.GetGSTById(id);
            if (cm.IsEnabled == true)
            {
                cm.IsEnabled = false;
            }
            else
            {
                cm.IsEnabled = true;
            }
            pr = gService.SaveGST(cm);
            return Json(new { result = pr });
        }
        public IActionResult GetGstBySubCat(int subId)
        {
            GSTMaster gm = gService.getGstBySubCatId(subId);
            return Json(new { re=gm });
        }
        #endregion
        #region FinanceGroup
        public IActionResult FinanceGroup()
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

            List<FinanceGroups> myObj = fService.GetAllFinGroups();
            return View(myObj);
        }
        public IActionResult FinanceGroupData(int id = 0)
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

            FinanceGroups obj = new FinanceGroups();
            if (id > 0)
            {
                obj = fService.GetFinGroupById(id);
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult FinanceGroupData(FinanceGroups request)
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
                // save or update call
                ProcessResponse pr = fService.SaveFinGroup(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("FinanceGroup");
                }
            }
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteFinGroup(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            FinanceGroups cm = new FinanceGroups();
            cm = fService.GetFinGroupById(id);
            cm.IsDeleted = true;
            pr = fService.SaveFinGroup(cm);
            return Json(new { result = pr });
        }
        #endregion
        #region FinanceHead
        public IActionResult FinanceHead(int grpId=0)
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

            List<FinanceHeads> myObj = fService.GetAllFinHeads(grpId);

            List<FinanceGroups> groups = new List<FinanceGroups>();
            groups = fService.GetAllFinGroups();
            ViewBag.Groups = groups;
            return View(myObj);
        }
        public IActionResult FinanceHeadData(int id = 0)
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

            FinanceHeads obj = new FinanceHeads();
            if (id > 0)
            {
                obj = fService.GetFinHeadById(id);
            }
            List<FinanceGroups> Groups = new List<FinanceGroups>();
            Groups = fService.GetAllFinGroups();
            ViewBag.Groups = Groups;
           // ViewBag.URL = url;
            return View(obj);
        }
        [HttpPost]
        public IActionResult FinanceHeadData(FinanceHeads request)
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
                FinanceHeads fh = new FinanceHeads();
                if(request.HeadId==0)
                {
                    CloneObjects.CopyPropertiesTo(request, fh);
                    fh.CurrentBallance = request.StartingBallance;
                }
                else
                {
                    fh = fService.GetFinHeadById(request.HeadId);
                    fh.HeadName = request.HeadName;
                    fh.HeadCode = request.HeadCode;
                    fh.HeadId = request.HeadId;
                    fh.GroupId = request.GroupId;
                }
                // save or update call
                ProcessResponse pr = fService.SaveFinHead(fh);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("FinanceHead", new { grpId = request.GroupId });
                }
            }
            List<FinanceGroups> Groups = new List<FinanceGroups>();
            Groups = fService.GetAllFinGroups();
            ViewBag.Groups = Groups;

            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteFinHead(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            FinanceHeads cm = new FinanceHeads();
            cm = fService.GetFinHeadById(id);
            cm.IsDeleted = true;
            pr = fService.SaveFinHead(cm);
            return Json(new { result = pr });
        }
        #endregion
        #region MRPFactor
        public IActionResult MRPFactors()
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
            List<MRPFactor> mylist = new List<MRPFactor>();
            mylist = mfService.GetAllMRPFactors();

            return View(mylist);
        }
        public IActionResult MRPFactorData(int id = 0)
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
            MRPFactor myObject = new MRPFactor();
            if (id > 0)
            {
                myObject = mfService.GetFactorById(id);

            }
            return View(myObject);
        }
        [HttpPost]
        public IActionResult MRPFactorData(MRPFactor request)
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
                // save or update call
                ProcessResponse pr = mfService.SaveMRPFactor(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("MRPFactors");
                }
            }
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteMRPFactor(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            MRPFactor um = new MRPFactor();
            um = mfService.GetFactorById(id);
            um.IsDeleted = true;
            pr = mfService.SaveMRPFactor(um);

            return Json(new { result = pr });
        }
        public IActionResult GetMrpMrpFac(int id)
        {
            MRPFactor myObject = mfService.GetFactorById(id);
            return Json(new { re = myObject });
        }
        #endregion
    }


}
