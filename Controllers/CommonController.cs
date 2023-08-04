using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCASA.Models.ModelClasses;
using SCASA.Models.Services;
namespace SCASA.Controllers
{
    public class CommonController : Controller
    {
        private readonly ICommonService cService;
        private readonly INotificationService nService;
        public CommonController(ICommonService _cService,INotificationService _nService)
        {
            cService = _cService;
            nService = _nService;
        }

        [HttpPost]
        public IActionResult GetAllCats()
        {
            List<CategoryDrop> myList = new List<CategoryDrop>();
            myList = cService.GetCatsDrop();
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetAllSubCats(int catId)
        {
            List<SubCategoryDrop> myList = new List<SubCategoryDrop>();
            myList = cService.GetSubCatsDrop(catId);
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetConditionsDrop()
        {
            List<ConditionDrop> myList = new List<ConditionDrop>();
            myList = cService.GetConditionsDrop();
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetLoctionDrop()
        {
            List<LocationDrop> myList = new List<LocationDrop>();
            myList = cService.GetLoctionDrop();
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetInventoryStatusDrop()
        {
            List<InventoryStatusDrop> myList = new List<InventoryStatusDrop>();
            myList = cService.GetInventoryStatusDrop();
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetAllCountries()
        {
            List<CountryDrop> myList = new List<CountryDrop>();
            myList = cService.GetAllCountries();
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetAllStates(int countryId)
        {
            List<StateDrop> myList = new List<StateDrop>();
            myList = cService.GetAllStates(countryId);
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetAllCities(int stateId)
        {
            List<CityDrop> myList = new List<CityDrop>();
            myList = cService.GetAllCities(stateId);
            return Json(new { result = myList });

        }
        public IActionResult GetReportingHesds(int typeId)
        {
            List<StaffDrop> rHs = cService.GetStaffDropsForReportingHead(typeId);

            return Json(new { result = rHs });
        }
        public IActionResult GetCashAndBankFinHeadDrops()
        {
            List<FHeadDrops> finaldrops = cService.GetBankAndCashHeads();            
            return Json(new { result = finaldrops });
        }
        public IActionResult SendDailyEmail()
        {
            nService.SendDailyReport();
            return Json("ok" );
        }
        public IActionResult SendWeaklyEmail()
        {
            nService.SendWeaklyReport();
            return Json("ok" );
        }
    }
}
