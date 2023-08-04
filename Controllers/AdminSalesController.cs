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
    public class AdminSalesController : Controller
    {
        private readonly IUserMgmtService uService;
        private readonly ICustomerMgmtService customerService;
        private readonly ICommonService comService;
        private readonly ISalesMasterMgmtService sMService;
        private readonly IInventoryService iService;
        private readonly ISalesDetailsMgmtService sDService;
        private readonly IGSTMasterMgmtService gstService;
        public AdminSalesController(IUserMgmtService _uService, ICustomerMgmtService _customerService, ICommonService _comService,IInventoryService _iServ,
                                    ISalesMasterMgmtService _sMService, ISalesDetailsMgmtService _sDService, IGSTMasterMgmtService _gstService)
        {
            uService = _uService;
            customerService = _customerService;
            comService = _comService;
            iService = _iServ;
            sMService = _sMService;
            sDService = _sDService;
            gstService = _gstService;
        }
        public IActionResult SalesExecutives(string search="")
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

            List<SalesExecutiveListModel> uml = uService.GetUsersByType("Sales Executive" , loginCheckResponse.userId , search);
            return View(uml);
        }
        public IActionResult Crfqs(int id, string emg = "", int pageNumber = 1, int pageSize = 10)
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
            ViewBag.UserName = um.UserName;
            ViewBag.ergmsg = emg;
            List<CRFQMasterModel> mylist = customerService.GetMyCRFQs(0, id,pageNumber,pageSize);
            //List<CRFQMasterModel> mylist =customerService.GetCRFQsOfStaff(id);
            int totalRecords = customerService.GatCrfqsOfSECount(id);
            ViewBag.SEId = id;
            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            //ViewBag.search = search;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalRecords / (decimal)pageSize));
            return View(mylist);
        }
        public IActionResult DeleteCrfq(int id)
        {
            CRFQMaster cm = sMService.GetCRFQById(id);
            cm.IsDeleted = true;
            ProcessResponse pr = sMService.SaveCRFQ(cm);
            return Json(new { re = pr });
        }
        public ActionResult CreateSaleOrder(CreateSaleOrderModel request)
        {
            CRFQMasterModel myList = new CRFQMasterModel();
            myList = customerService.GetSingleCRFQ(request.CRFQId);
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            int x = 0;
            bool sh = false;
            bool wh = false;

            if (myList != null)
            {
                //List<InventoryMaster> invLocal = new List<InventoryMaster>();
                //foreach(var v in myList.crfqDetails)
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
                        string emg = "Please Select correct quantity for all products ";
                        return RedirectToAction("Crfqs", new { id = myList.StaffId, emg = emg });
                    }
                    x++;
                    sh = false;
                    wh = false;
                }
                //GSTMaster gm = new GSTMaster();
                //gm = gstService.GetGSTById(request.TaxId);
                AddressMaster add = new AddressMaster();
                add = customerService.GetUserAddresses((int)myList.CustomerId).FirstOrDefault();
                UserMaster um = uService.GetUserById(myList.StaffId);
                SalesOrderMaster so = new SalesOrderMaster();
                so.CreatedBy = loginCheckResponse.userName;
                so.CreatedOn = DateTime.Now;
                so.CurrentStatus = "Open";
                so.SOStatus = "Open";
                so.CustomerId = myList.CustomerId;
                so.IsDeleted = false;
                so.QuoteId = myList.CRFQId;
                so.StaffId = myList.StaffId;
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
                        sd.TaxPercentage = gstService.GetGstPercentBySubCatId((int)im.SubCategoryId);
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
                        rqm.SOMId = sR.currentId;
                        rqm.SalesExename = um.UserName;
                        customerService.SaveReservedQtyMaster(rqm);

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

                CRFQMaster cm = customerService.GetCRFQMaster(myList.CRFQId);
                cm.CurrentStatus = "SO Created";
                var r = customerService.SaveCRFQ(cm);
            }
            return RedirectToAction("Crfqs",new {id=myList.StaffId });
        }
        public IActionResult Discounts()
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
            List<QuotesSubmittedForApprovalModel> myList = new List<QuotesSubmittedForApprovalModel>();
            myList = customerService.GetQuotesSubmittedForApproval(loginCheckResponse.userId);
            return View(myList) ;
           
        }
        public IActionResult OpenOrders(string notEmg="", int pageNumber = 1, int pageSize = 10)
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
            List<SaleOrderMasterModel> mylist = new List<SaleOrderMasterModel>();
            int totalRecords = 0;
            if (loginCheckResponse.userTypeCode < 5 || loginCheckResponse.userTypeCode  == 6)
            {
                mylist = customerService.GetOpenSaleOrder(0,pageNumber,pageSize);
                totalRecords = customerService.GetOpenSaleOrderCount(0);
            }
            else
            {
                mylist = customerService.GetOpenSaleOrder(loginCheckResponse.userId,pageNumber,pageSize);
                totalRecords = customerService.GetOpenSaleOrderCount(loginCheckResponse.userId);
            }
            ViewBag.emgNft = notEmg;
                        
            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalRecords / (decimal)pageSize));
            return View(mylist);
        }
        public IActionResult DeleteSaleOrder(int id)
        {
            
            ProcessResponse pr = sMService.DeleteSaleOrder(id);
            return Json(new { re = pr });
        }
        public IActionResult HeadDiscount(int trid, string emg="")
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
            QuotesSubmittedForApprovalModel  myList = new  QuotesSubmittedForApprovalModel();
            myList = customerService.GetQuotesSubmittedForApprovalSingle(trid);
            ViewBag.excedError = emg;
            return View(myList);
        }
        [HttpPost]
        public IActionResult CreateDC(CreateDCModel reqquest)
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
            var res = customerService.GenerateDC(reqquest, loginCheckResponse.userId);
            if(res.statusCode == 1)
            {
                return RedirectToAction("DeliveryChallans");
            }

            return RedirectToAction("OpenOrders", new { notEmg=res.statusMessage });
        }
        public IActionResult DeliveryChallans()
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
            List<DeleverySaleOrderModel> myList = sDService.GetAllReadyToDispatchSOMasters();
            return View(myList);
        }

        [HttpPost]
        public IActionResult GiveHeadDiscount(HeadDiscountModel rquest)
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
            rquest.GivenBy = loginCheckResponse.userName;

            ProcessResponse p = customerService.HeadDiscountSubmit(rquest, loginCheckResponse.userId);
            if (p.statusCode == 1)
            {
                return RedirectToAction("Discounts");
            }
            else
            {
                string emsg = p.statusMessage;
                return RedirectToAction("HeadDiscount",new { trid =rquest.TRId ,emg=emsg});
            }
        }
        public IActionResult GiveAdminDiscount(HeadDiscountModel rquest)
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
            rquest.GivenBy = loginCheckResponse.userName;

            ProcessResponse p = customerService.AdminDiscountSubmit(rquest, loginCheckResponse.userId);
            if (p.statusCode == 1)
            {
                return RedirectToAction("Discounts");
            }
            else
            {
                string emsg = p.statusMessage;
                return RedirectToAction("HeadDiscount", new { trid = rquest.TRId, emg = emsg });
            }
        }
        public IActionResult PrintTaxInvoice(int invoiceid)
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
            var taxInvoice = customerService.GetTaxInvoice(invoiceid);
            //var Taxes = comService.GetAllGST();
            taxInvoice.DelivaryCharges = taxInvoice.DelivaryCharges == null ? 0 : taxInvoice.DelivaryCharges;
            taxInvoice.totalValueInWords = AppSettings.NumberToWords.ConvertAmount((double)(taxInvoice.TotalValue+taxInvoice.DelivaryCharges));
            return View(taxInvoice);
        }
        public IActionResult PrintDC(int dcid)
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
            //var dc = customerService.GetDCById(dcid);
            //var taxInvoice = customerService.GetTaxInvoice((int)dc.InvoiceId);
            var taxInvoice = customerService.GetTaxInvoice((int)dcid);
            taxInvoice.totalValueInWords = AppSettings.NumberToWords.ConvertAmount((double)taxInvoice.TotalValue);
            return View(taxInvoice);
        }
        [HttpPost]
        public IActionResult UpdateEWayBill(string ewaybill,int dcmid)
        {
            customerService.UpdateEWayBill(ewaybill,dcmid);
            return Json(new { res = "OK" });
        }
        public IActionResult UpdateCustGst(string gNo,int dcmid)
        {
            customerService.UpdateCustGstNumber(gNo, dcmid);
            return Json(new { res = "OK" });
        }
        public IActionResult UpdateDispatchedDetails(DateTime dDate, int dBy, int dcmid)
        {
            customerService.UpdateDispatched(dDate,dBy, dcmid);
            return Json(new { res = "OK" });
        }

        public IActionResult UpdateDespatchInfo(int dcmid)
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
            ModelForRDToDel res = new ModelForRDToDel();
            //res = customerService.GetAllDC("All").Where(a => a.DCId == dcmid).FirstOrDefault();
            //DCDeliverModel fin = new DCDeliverModel();
            //fin.DCMasterId = dcmid;
            res.Items = customerService.GetItemsOfINvoiceMaster(dcmid);
            //ViewBag.items = ItemDetails;
            res.DCId= dcmid;
            return View(res);
        }
        [HttpPost]
        public IActionResult UpdateDespatchInfo(ModelForRDToDel request)
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

            if(request.DespatchedOn == null)
            {
                ModelState.AddModelError("DespatchedOn", "Date is required");
            }
            bool a = false;
            foreach(bool b in request.YesOrNo)
            {
                if (b)
                {
                    a = true;
                }
            }
            if (!a)
            {
                ModelState.AddModelError("YesOrNo", "please select items");
            }
            if(ModelState.IsValid)
            {
                request.LogedUser = loginCheckResponse.userId;

                //customerService.UpdateDispatched((DateTime) request.DespatchedOn, (int)request.DespatchedBy, request.DCId);
                customerService.CreatePartialDC(request);
                return RedirectToAction("DeliveryChallans");
            }
            request.Items = customerService.GetItemsOfINvoiceMaster(request.DCId);
            return View(request);/*RedirectToAction("UpdateDespatchInfo",new { dcmid = request.DCId });*/
        }

        public IActionResult ReadytoDespatch()
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
            List<DCMasterModel> myList = new List<DCMasterModel>();
            myList = customerService.GetAllDC("All");
            return View(myList);
        }
        public IActionResult Despatched()
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
            List<DCMasterModel> myList = new List<DCMasterModel>();
            myList = customerService.GetAllDC("Despatched");
            return View(myList);
        }       
    }
}

