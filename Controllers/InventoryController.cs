using SCASA.Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService iService;
        private readonly ICommonService cService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IMRPFactorMgmtService mrpService;
        private readonly IRecordExceptionService exceptionService;
        private readonly IGSTMasterMgmtService gstService;
        private readonly INotificationService nService;
        public InventoryController(IInventoryService _iservice,
            ICommonService _cService, IHostingEnvironment environement, INotificationService _nService, IRecordExceptionService _exService,
            IMRPFactorMgmtService _mrpService , IGSTMasterMgmtService _gstService)
        {
            iService = _iservice;
            cService = _cService;
            hostingEnvironment = environement;
            nService = _nService;
            mrpService = _mrpService;
            exceptionService = _exService;
            gstService = _gstService;
        }
        //public IActionResult Warehouse(int pageNumber = 1, int pageSize = 10, string type = "", string search = "",
        //    int CategoryId = 0, int SubCategoryId = 0)
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


        //    List<InventoryDisplayModel> myList = new List<InventoryDisplayModel>();
        //    myList = iService.GetInventoryAll("Warehouse", pageNumber, pageSize, CategoryId, SubCategoryId, search);
        //    int totalRecords = iService.GetInventoryAll_Count("Warehouse", pageNumber,
        //        pageSize, CategoryId, SubCategoryId, search);
        //    ViewBag.TotalRecords = totalRecords;
        //    ViewBag.TotalCount = totalRecords;
        //    ViewBag.pageNumber = pageNumber;
        //    ViewBag.pageSize = pageSize;
        //    ViewBag.search = search;
        //    InventoryDisplayModelBase myDataBase = new InventoryDisplayModelBase();
        //    myDataBase.myList = myList;
        //    myDataBase.categoryDrops = cService.GetCatsDrop();
        //    int cCatid = (CategoryId > 0) ? CategoryId : myDataBase.categoryDrops[0].CategoryId;
        //    myDataBase.subCategoryDrops = cService.GetSubCatsDrop(cCatid);
        //    myDataBase.CategoryId = CategoryId;
        //    myDataBase.SubCategoryId = SubCategoryId;
        //    myDataBase.search = search;
        //    return View(myDataBase);
        //}
        public IActionResult All(int pageNumber = 1, int pageSize = 10, string type = "", string search = "",
            int CategoryId = 0, int SubCategoryId = 0)
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


            List<InventoryAllDisplayModel> myList = new List<InventoryAllDisplayModel>();
            myList = iService.GetInventoryAll("All", pageNumber, pageSize, CategoryId, SubCategoryId, search);
            int totalRecords = iService.GetInventoryAll_Count("All", pageNumber, pageSize, CategoryId, SubCategoryId, search);
            ViewBag.TotalRecords = totalRecords;
            if (search == null)
            {
                search = "";
            }
            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.search = search;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            InventoryDisplayModelBase myDataBase = new InventoryDisplayModelBase();
            myDataBase.myList = myList;
            myDataBase.categoryDrops = cService.GetCatsDrop();
            int cCatid = (CategoryId > 0) ? CategoryId : myDataBase.categoryDrops[0].CategoryId;
            myDataBase.subCategoryDrops = cService.GetSubCatsDrop(cCatid);
            myDataBase.CategoryId = CategoryId;
            myDataBase.SubCategoryId = SubCategoryId;
            myDataBase.search = search;
            return View(myDataBase);
        }
        //public IActionResult Showroom(int pageNumber = 1, int pageSize = 10, string type = "", string search = "",
        //    int CategoryId = 0, int SubCategoryId = 0)
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

        //    List<InventoryDisplayModel> myList = new List<InventoryDisplayModel>();
        //    myList = iService.GetInventoryAll("Showroom", pageNumber, pageSize, CategoryId, SubCategoryId, search);
        //    int totalRecords = iService.GetInventoryAll_Count("Showroom", pageNumber, pageSize, CategoryId, SubCategoryId, search);
        //    ViewBag.TotalRecords = totalRecords;
        //    ViewBag.TotalCount = totalRecords;
        //    ViewBag.pageNumber = pageNumber;
        //    ViewBag.pageSize = pageSize;
        //    ViewBag.search = search;
        //    InventoryDisplayModelBase myDataBase = new InventoryDisplayModelBase();
        //    myDataBase.myList = myList;
        //    myDataBase.categoryDrops = cService.GetCatsDrop();
        //    int cCatid = (CategoryId > 0) ? CategoryId : myDataBase.categoryDrops[0].CategoryId;
        //    myDataBase.subCategoryDrops = cService.GetSubCatsDrop(cCatid);
        //    myDataBase.CategoryId = CategoryId;
        //    myDataBase.SubCategoryId = SubCategoryId;
        //    myDataBase.search = search;
        //    return View(myDataBase);
        //}
        public IActionResult InventoryData(int id = 0, int CatId = 0, int SubCatId = 0, int pageNumber = 1, int pageSize = 10, string search = "")
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
            InventoryMaster myObj = new InventoryMaster();
            myObj = iService.GetInentoryById(id);

            InventoryMasterModel finalObject = new InventoryMasterModel();
            if (id > 0)
            {
                if (myObj.MRPFactorId == null)
                {
                    List<MRPFactor> m = mrpService.GetAllMRPFactors();
                    myObj.MRPFactorId = m[0].TRId;
                }
                //MRPFactor mrp = mrpService.GetFactorById((int)myObj.MRPFactorId);
                //myObj.MRPFactorId = 0;
                CloneObjects.CopyPropertiesTo(myObj, finalObject);
                //finalObject.mrpfac = (decimal)mrp.FactorValue;
                string[] mainImg = finalObject.PrimaryImage.Split(",");

                finalObject.MainImagesList = new List<string>();
                int count = mainImg.Count();
                if (count >= 1)
                {
                    for (int i = 0; i < count; i++)
                    {
                        finalObject.MainImagesList.Add(mainImg[i]);
                    }
                } 
              
            }
            finalObject.categoryDrops = cService.GetCatsDrop();
            finalObject.conditionDrops = cService.GetConditionsDrop();
            finalObject.inventoryStatusDrops = cService.GetInventoryStatusDrop();
            finalObject.locationDrops = cService.GetLoctionDrop();
            finalObject.gstDrop = cService.GetAllGST();
            int cCatid = finalObject.CategoryId;
            finalObject.CategoryId = (id > 0) ? finalObject.CategoryId : (CatId > 0 ? CatId : finalObject.CategoryId);
            finalObject.subCategoryDrops = cService.GetSubCatsDrop(CatId > 0 ? CatId : cCatid);
            finalObject.mRPfactorDrops = cService.GetMrpFactorDrop();
            finalObject.OtherImages = iService.InventoryOtherImgsUploaded(id);
            
            foreach (MRPFactopDrop m in finalObject.mRPfactorDrops)
            {
                m.FactorName += "  ( " + m.FactorValue + " )";
            }
            if (search == null)
            {
                search = "";
            }
            ViewBag.pn = pageNumber;
            ViewBag.ps = pageSize;
            ViewBag.ser = search;
            ViewBag.catId = CatId;
            ViewBag.suCId = SubCatId;
            //GSTMaster gm = gstService.getGstBySubCatId(id > 0 ? 0 : finalObject.subCategoryDrops[0].SubCategoryId);
            //ViewBag.gstValueName = gm.TaxName + gm.TaxAmount;
            return View(finalObject);
        }

        [HttpPost]
        public IActionResult InventoryData(InventoryMasterModel request, int pageNumber = 1, int pageSize = 10, string search = "",
            int CategoryId = 0, int SubCategoryId = 0)
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
            if (loginCheckResponse.userTypeCode == 8)
            {
                ModelState.Remove("ActualPrice");
                ModelState.Remove("MRPPrice");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // save InventoryMaster main image
                    bool isMainFileUploaded = false;
                    string mainfilename = "";
                    if (request.ProductMainImageUploaded != null)
                    {
                        int imgcnt = 1;
                        foreach (FormFile file in request.ProductMainImageUploaded)
                        {

                            var fileNameUploaded = Path.GetFileName(file.FileName);
                            if (fileNameUploaded != null)
                            {
                                var conentType = file.ContentType;
                                string filename = DateTime.UtcNow.ToString();
                                filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                                filename = Regex.Replace(filename, "[A-Za-z ]", "");
                                filename = filename + RandomGenerator.RandomString(4, false);
                                string extension = Path.GetExtension(fileNameUploaded);
                                filename += extension;
                                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                                var filePath = Path.Combine(uploads, filename);
                                file.CopyTo(new FileStream(filePath, FileMode.Create));
                                if (imgcnt == 1)
                                {
                                    mainfilename = filename;
                                }
                                else
                                {
                                    mainfilename += "," + filename;
                                }
                                imgcnt++;

                                isMainFileUploaded = true;

                            }

                        }
                    }

                    // save InventoryMaster main image
                    bool isColorImageuploaded = false;
                    string colorfilename = "";
                    if (request.ColorImageUploaded != null)
                    {
                        int imgcnt = 1;
                        foreach (FormFile file in request.ColorImageUploaded)
                        {
                            var fileNameUploaded = Path.GetFileName(file.FileName);
                            if (fileNameUploaded != null)
                            {
                                var conentType = file.ContentType;
                                string filename = DateTime.UtcNow.ToString();
                                filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                                filename = Regex.Replace(filename, "[A-Za-z ]", "");
                                filename = filename + RandomGenerator.RandomString(4, false);
                                string extension = Path.GetExtension(fileNameUploaded);
                                filename += extension;
                                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                                var filePath = Path.Combine(uploads, filename);
                                file.CopyTo(new FileStream(filePath, FileMode.Create));
                                if (imgcnt == 1)
                                {
                                    colorfilename = filename;
                                }
                                else
                                {
                                    colorfilename += "," + filename;
                                }
                                imgcnt++;


                                isColorImageuploaded = true;
                            }
                        }

                    }

                    InventoryMaster im = new InventoryMaster();
                    if (request.InventoryId > 0)
                    {

                        im = iService.GetInentoryById(request.InventoryId);
                        //MRPFactor mrp = mrpService.GetFactorByValue((decimal)request.mrpfac);
                        if (request.MRPFactorId == null)
                        {
                            im.MRPFactorId = im.MRPFactorId;
                        }
                        else
                        {
                            im.MRPFactorId = request.MRPFactorId;
                        }
                        im.ActualPrice = request.ActualPrice;
                        im.Brand = request.Brand;
                        im.Breadth = request.Breadth;
                        im.CategoryId = request.CategoryId;
                        im.ColorImage = isColorImageuploaded ? colorfilename : im.ColorImage;
                        im.ColorName = request.ColorName;
                        im.Height = request.Height;
                        im.InventoryConditonId = request.InventoryConditonId;
                        im.InventoryLocationId = request.InventoryLocationId;
                        im.InventoryStatusId = request.InventoryStatusId;
                        im.InvoiceNumber = request.InvoiceNumber;
                        im.ItemDescription = request.ItemDescription;
                        im.LastModifiedBy = loginCheckResponse.userName;
                        im.LastModifiedOn = DateTime.Now;
                        im.ModelNumber = request.ModelNumber;
                        //im.MRPFactorId = request.MRPFactorId;
                        im.MRPPrice = request.MRPPrice;
                        im.PODate = request.PODate;
                        im.PODetails = request.PODetails;
                        im.PrimaryImage = isMainFileUploaded ? (mainfilename + "," + im.PrimaryImage) : im.PrimaryImage;
                        int tempQty = (int)im.Qty;
                        im.Qty = request.Qty;
                        if (tempQty != im.Qty)
                        {
                            tempQty = request.Qty - tempQty;
                            im.WharehouseQty += tempQty;
                        }
                        im.RackName = request.RackName;
                        im.RecievedDate = request.RecievedDate;
                        im.SubCategoryId = request.SubCategoryId;
                        im.Width = request.Width;
                        im.GSTMasterId = request.GSTMasterId;
                        im.ThresholdValue = request.ThresholdValue;
                        im.LeadTime = request.LeadTime;
                        im.Title = request.Title;
                        im.HSNCode = request.HSNCode;
                        var res = iService.UpdateInventory(im);

                        if (request.ProductOtherImagesUploaded != null)
                        {
                            foreach (FormFile file in request.ProductOtherImagesUploaded)
                            {
                                var fileNameUploaded = Path.GetFileName(file.FileName);
                                if (fileNameUploaded != null)
                                {
                                    var conentType = file.ContentType;
                                    string filename = DateTime.UtcNow.ToString();
                                    filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                                    filename = Regex.Replace(filename, "[A-Za-z ]", "");
                                    filename = filename + RandomGenerator.RandomString(4, false);
                                    string extension = Path.GetExtension(fileNameUploaded);
                                    filename += extension;
                                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                                    var filePath = Path.Combine(uploads, filename);
                                    file.CopyTo(new FileStream(filePath, FileMode.Create));
                                    InventoryImages detimages = new InventoryImages();
                                    detimages.ImageURL = filename;
                                    detimages.IsDeleted = false;
                                    detimages.InventoryId = request.InventoryId;
                                    iService.UpdateInventoryImages(detimages);
                                }
                            }
                        }
                        if (request.DocumentsUploaded != null)
                        {
                            foreach (FormFile file in request.DocumentsUploaded)
                            {
                                var fileNameUploaded = Path.GetFileName(file.FileName);
                                if (fileNameUploaded != null)
                                {
                                    var conentType = file.ContentType;
                                    string filename = DateTime.UtcNow.ToString();
                                    filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                                    filename = Regex.Replace(filename, "[A-Za-z ]", "");
                                    filename = filename + RandomGenerator.RandomString(4, false);
                                    string extension = Path.GetExtension(fileNameUploaded);
                                    filename += extension;
                                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                                    var filePath = Path.Combine(uploads, filename);
                                    file.CopyTo(new FileStream(filePath, FileMode.Create));

                                    InventoryDocuments detimages = new InventoryDocuments();
                                    detimages.DocumentName = fileNameUploaded;
                                    detimages.UploadedBy = loginCheckResponse.userName;
                                    detimages.UploadedOn = DateTime.Now;
                                    detimages.DocumentURL = filename;
                                    detimages.IsDeleted = false;
                                    detimages.InventoryId = request.InventoryId;
                                    iService.UploadInventoryDocument(detimages);
                                }
                            }
                        }


                    }
                    else
                    {
                        CloneObjects.CopyPropertiesTo(request, im);
                        //MRPFactor mrp = mrpService.GetFactorByValue((decimal)request.mrpfac);
                        if (request.MRPFactorId == null)
                        {
                            im.MRPFactorId = im.MRPFactorId;
                        }
                        else
                        {
                            im.MRPFactorId = request.MRPFactorId;
                        }
                        im.ColorImage = colorfilename;
                        im.CreatedBy = loginCheckResponse.userName;
                        im.CreatedOn = DateTime.Now;
                        im.IsDeleted = false;
                        im.LastModifiedBy = loginCheckResponse.userName;
                        im.LastModifiedOn = DateTime.Now;
                        im.PrimaryImage = mainfilename;
                        im.WharehouseQty = request.Qty;
                        im.ShowroomQty = 0;
                        var resp = iService.UpdateInventory(im);
                        if (resp.currentId > 0)
                        {
                            if (request.ProductOtherImagesUploaded != null)
                            {
                                foreach (FormFile file in request.ProductOtherImagesUploaded)
                                {
                                    var fileNameUploaded = Path.GetFileName(file.FileName);
                                    if (fileNameUploaded != null)
                                    {
                                        var conentType = file.ContentType;
                                        string filename = DateTime.UtcNow.ToString();
                                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                                        filename = filename + RandomGenerator.RandomString(4, false);
                                        string extension = Path.GetExtension(fileNameUploaded);
                                        filename += extension;
                                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                                        var filePath = Path.Combine(uploads, filename);
                                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                                        InventoryImages detimages = new InventoryImages();
                                        detimages.ImageURL = filename;
                                        detimages.IsDeleted = false;
                                        detimages.InventoryId = resp.currentId;
                                        iService.UpdateInventoryImages(detimages);
                                    }
                                }
                            }
                            if (request.DocumentsUploaded != null)
                            {
                                foreach (FormFile file in request.DocumentsUploaded)
                                {
                                    var fileNameUploaded = Path.GetFileName(file.FileName);
                                    if (fileNameUploaded != null)
                                    {
                                        var conentType = file.ContentType;
                                        string filename = DateTime.UtcNow.ToString();
                                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                                        filename = filename + RandomGenerator.RandomString(4, false);
                                        string extension = Path.GetExtension(fileNameUploaded);
                                        filename += extension;
                                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                                        var filePath = Path.Combine(uploads, filename);
                                        file.CopyTo(new FileStream(filePath, FileMode.Create));

                                        InventoryDocuments detimages = new InventoryDocuments();
                                        detimages.DocumentName = fileNameUploaded;
                                        detimages.DocumentURL = filename;
                                        detimages.IsDeleted = false;
                                        detimages.InventoryId = resp.currentId;
                                        iService.UploadInventoryDocument(detimages);
                                    }
                                }
                            }
                        }
                    }

                    return RedirectToAction("All"/*, new { CategoryId = request.CategoryId, SubCategoryId = request.SubCategoryId }*/);
                }
                catch (Exception ex)
                {
                    ex.Source = "Inventory Add Edit";
                    exceptionService.LogError(ex);
                }
                
            }

            request.categoryDrops = cService.GetCatsDrop();
            request.conditionDrops = cService.GetConditionsDrop();
            request.inventoryStatusDrops = cService.GetInventoryStatusDrop();
            request.locationDrops = cService.GetLoctionDrop();
            request.gstDrop = cService.GetAllGST();
            request.subCategoryDrops = cService.GetSubCatsDrop((int)request.CategoryId);
            request.mRPfactorDrops = cService.GetMrpFactorDrop();
            foreach (MRPFactopDrop m in request.mRPfactorDrops)
            {
                m.FactorName += " (" + m.FactorValue + ")";
            }
            if (request.InventoryId > 0)
            {
                request.OtherImages = iService.InventoryOtherImgsUploaded(request.InventoryId);
                InventoryMaster myObj = iService.GetInentoryById(request.InventoryId);                
                string[] mainImg = myObj.PrimaryImage.Split(",");

                request.MainImagesList = new List<string>();
                int count = mainImg.Count();
                if (count >= 1)
                {
                    for (int i = 0; i < count; i++)
                    {
                        request.MainImagesList.Add(mainImg[i]);
                    }
                }
            }
            if (search == null)
            {
                search = "";
            }
            ViewBag.pn = pageNumber;
            ViewBag.ps = pageSize;
            ViewBag.ser = search;
            ViewBag.catId = CategoryId;
            ViewBag.suCId = SubCategoryId;
            return View(request);
        }

        [HttpPost]
        public IActionResult DeleteInventory(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ProcessResponse pr = new ProcessResponse();
            InventoryMaster sm = new InventoryMaster();
            sm = iService.GetInentoryById(id);
            sm.IsDeleted = true;
            pr = iService.UpdateInventory(sm);
            // send email
            string productDetails = "Model Number : " + sm.ModelNumber + ", Qty " + sm.Qty;
            var s = nService.SendProductDeleteEmail(AppSettings.EmailTemplates.ProductDeleted,
                productDetails, loginCheckResponse.userName, DateTime.Now);
            return Json(new { result = pr });

        }

        [HttpPost]
        public IActionResult DeleteInventoryImage(string image, int invId)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ProcessResponse pr = new ProcessResponse();
            //iService.DeleteProductImage(image, invId);
            pr.statusCode = 1;
            pr.statusMessage = "Success";
            return Json(new { result = pr });

        }
        public IActionResult ProductDetail(int id = 0, int pageNumber = 1, int pageSize = 10, string search = "",
            int CategoryId = 0, int SubCategoryId = 0)
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

            InventoryMaster myObj = new InventoryMaster();
            myObj = iService.GetInentoryById(id);


            InventoryMasterModel finalObject = new InventoryMasterModel();
            if (id > 0)
            {
                CloneObjects.CopyPropertiesTo(myObj, finalObject);
            }
            finalObject.categoryDrops = cService.GetCatsDrop();
            finalObject.conditionDrops = cService.GetConditionsDrop();
            finalObject.inventoryStatusDrops = cService.GetInventoryStatusDrop();
            finalObject.locationDrops = cService.GetLoctionDrop();
            finalObject.gstDrop = cService.GetAllGST();
            int cCatid = finalObject.CategoryId;
            finalObject.subCategoryDrops = cService.GetSubCatsDrop(cCatid);
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
            if (search == null)
            {
                search = "";
            }
            ViewBag.pn = pageNumber;
            ViewBag.ps = pageSize;
            ViewBag.ser = search;
            ViewBag.catId = CategoryId;
            ViewBag.suCId = SubCategoryId;
            return View(finalObject);

        }

        public IActionResult StockMovement(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string type = "", string search = "",
            int CategoryId = 0, int SubCategoryId = 0)
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

            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddDays(-7);
            }
            if (toDate == null)
            {
                toDate = DateTime.Now;
            }
            List<StockMovmentInvoice> myList = new List<StockMovmentInvoice>();
            myList = iService.GetStockMovInvoices(fromDate, toDate, pageNumber, pageSize);
            int totalRecords = iService.GetStockMovInvoices_Count(fromDate, toDate);
            ViewBag.TotalRecords = totalRecords;

            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.search = search;
            ViewBag.FromDate = Convert.ToDateTime(fromDate).ToShortDateString();
            ViewBag.ToDate = Convert.ToDateTime(toDate).ToShortDateString();

            return View(myList);
        }
        public IActionResult CurrentStandBy(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string type = "", string search = "",
            int CategoryId = 0, int SubCategoryId = 0)
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

            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddDays(-7);
            }
            if (toDate == null)
            {
                toDate = DateTime.Now;
            }
            List<StandByInvoice> myList = new List<StandByInvoice>();
            myList = iService.GetStandByInvoices(fromDate, toDate, pageNumber, pageSize);
            int totalRecords = iService.GetCurrentStandByInvoices_Count(fromDate, toDate);
            ViewBag.TotalRecords = totalRecords;

            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.search = search;
            ViewBag.FromDate = Convert.ToDateTime(fromDate).ToShortDateString();
            ViewBag.ToDate = Convert.ToDateTime(toDate).ToShortDateString();

            return View(myList);
        }
        public IActionResult PreviousStandBy(int pageNumber = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string type = "", string search = "",
            int CategoryId = 0, int SubCategoryId = 0)
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

            if (fromDate == null)
            {
                fromDate = DateTime.Now.AddDays(-7);
            }
            if (toDate == null)
            {
                toDate = DateTime.Now;
            }
            List<StandByInvoice> myList = new List<StandByInvoice>();
            myList = iService.GetPreviousStandByInvoices(fromDate, toDate, pageNumber, pageSize);
            int totalRecords = iService.GetPreviousStandByInvoices_Count(fromDate, toDate);
            ViewBag.TotalRecords = totalRecords;

            ViewBag.TotalCount = totalRecords;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.search = search;
            ViewBag.FromDate = Convert.ToDateTime(fromDate).ToShortDateString();
            ViewBag.ToDate = Convert.ToDateTime(toDate).ToShortDateString();

            return View(myList);
        }
        //public IActionResult MoveStock(int inventoryId = 0)
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
        //    StockMovementRegisterModel sm = new StockMovementRegisterModel();
        //    var v = iService.GetInentoryById(inventoryId);
        //    sm.InventoryId = inventoryId;
        //    sm.ShowroomQty = v.ShowroomQty;
        //    sm.WarehouseQty = v.WharehouseQty;
        //    sm.ModelNumber = v.ModelNumber;
        //    return View(sm);


        //}
        //[HttpPost]
        //public IActionResult MoveStock(StockMovementRegisterModel request)
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
        //    ProcessResponse pr = new ProcessResponse();
        //    if (ModelState.IsValid)
        //    {
        //        bool isAllok = true;
        //        if(request.MovedFrom == request.MovedTo)
        //        {
        //            pr.statusCode = 0;
        //            pr.statusMessage = "Select different source and destination";
        //            isAllok = false;
        //        }
        //        var im = iService.GetInentoryById((int)request.InventoryId);
        //        if(request.MovedFrom == "Warehouse")
        //        {
        //            if(request.Qty > im.WharehouseQty)
        //            {
        //                pr.statusCode = 0;
        //                pr.statusMessage = "Moving quantity exceeding the available qty.";
        //                isAllok = false;
        //            }
        //        }
        //        if(request.MovedFrom == "Showroom")
        //        {
        //            if (request.Qty > im.ShowroomQty)
        //            {
        //                pr.statusCode = 0;
        //                pr.statusMessage = "Moving quantity exceeding the available qty.";
        //                isAllok = false;
        //            }
        //        }
        //        if(isAllok)
        //        {
        //            request.IsDeleted = false;
        //            request.MovedBy = loginCheckResponse.userName;
        //            request.MovedOn = DateTime.Now;
        //            StockMovementRegister sm = new StockMovementRegister();
        //            CloneObjects.CopyPropertiesTo(request, sm);
        //            pr = iService.MoveStock(sm);
        //            return RedirectToAction("All");
        //        }
        //    }

        //    ViewBag.ErrorMessage = pr.statusMessage;
        //    return View(request);

        //}

        public IActionResult MoveStock()
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
            StockMovementMasterModel sm = new StockMovementMasterModel();
            sm.modelDrop = iService.GetInventoryModelDrop();
            InventoryModelDrop d = new InventoryModelDrop
            {
                ModelNumber = " Select ",
                InventoryId = 0
            };
            ViewBag.customers = cService.GetCustomerDrops();
            sm.modelDrop.Insert(0, d);

            return View(sm);


        }
        
        [HttpPost]
        public IActionResult MoveStock(StockMovementMasterModel request)
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
            ProcessResponse pr = new ProcessResponse();
            if (ModelState.IsValid)
            {
                bool isAllok = true;
                if (request.MovedFrom == request.MovedTo)
                {
                    pr.statusCode = 0;
                    pr.statusMessage = "Select different source and destination";
                    isAllok = false;
                }

                if (isAllok)
                {
                    if (request.MovedTo != "StandBY")
                    {
                        request.IsDeleted = false;
                        request.MovedBy = loginCheckResponse.userName;
                        request.MovedOn = DateTime.Now;
                        
                        pr = iService.MoveStock(request);
                        if (pr.statusCode == 1)
                        {
                            return RedirectToAction("StockMovement");
                        }
                    }
                    else
                    {
                        request.IsDeleted = false;
                        request.MovedBy = loginCheckResponse.userName;
                        request.MovedOn = DateTime.Now;
                        request.StaffId = loginCheckResponse.userId;
                        pr = iService.MoveStandByStock(request);
                        if (pr.statusCode == 1)
                        {
                            return RedirectToAction("CurrentStandBy");
                        }
                    }
                }
            }
            ViewBag.ErrorMessage = pr.statusMessage;
            StockMovementMasterModel sm = new StockMovementMasterModel();
            sm.modelDrop = iService.GetInventoryModelDrop();
            InventoryModelDrop d = new InventoryModelDrop
            {
                ModelNumber = " Select ",
                InventoryId = 0
            };
            sm.modelDrop.Insert(0, d);
            request.modelDrop = sm.modelDrop;
            ViewBag.customers = cService.GetCustomerDrops();
            return View(request);
        }

        [HttpPost]
        public IActionResult GetInvModelDrop(int categoryId, int subcategoryId)
        {
            List<InventoryModelDrop> myList = new List<InventoryModelDrop>();
            myList = iService.GetInventoryModelDrop(categoryId, subcategoryId);
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetInvBasicInfo(int id)
        {
            return Json(new { result = iService.GetStockMmtBasicInfo(id) });
        }


        public IActionResult StockMovementInvoice(int id)
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
            StockMovementForPrint myObj = new StockMovementForPrint();
            myObj = iService.GetStockStockMovementByInvoice(id);
            return View(myObj);
        }
        public IActionResult StandByInvoice(int id)
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
            StockMovementForPrint myObj = new StockMovementForPrint();
            myObj = iService.GetStandByInvoice(id);
            return View(myObj);
        }
        public IActionResult DeleteOtherImage(int id)
        {
            InventoryImages ii = iService.GetInventoryImageById(id);
            ii.IsDeleted = true;
            ProcessResponse pr = iService.UpdateInventoryImages(ii);

            return Json(new { result = pr });
        }
        public IActionResult DeleteMainImage(string name,int id)
        {
            ProcessResponse pr = new ProcessResponse();
            InventoryMaster myObj = iService.GetInentoryById(id);
            string[] mainImg = myObj.PrimaryImage.Split(",");
            myObj.PrimaryImage = "";
            int imgId=0;            
            foreach(string mi in mainImg)
            {
                if (/*!Equals(name, mi)*/name!=mi){
                    if (imgId == 0)
                    {
                        myObj.PrimaryImage = mi;
                    }
                    else
                    {
                        myObj.PrimaryImage += "," + mi;
                    }
                    imgId++;
                }                
            }
            if (!string.IsNullOrEmpty(myObj.PrimaryImage))
            {
                pr = iService.UpdateInventory(myObj);
            }
            else
            {
                pr.statusCode = 5;
            }
            return Json(new { result = pr });
        }
        public IActionResult DeleteInvDoc(int docId)
        {
            InventoryDocuments id = iService.GetInvDocById(docId);
            id.IsDeleted = true;
            ProcessResponse pr = iService.UploadInventoryDocument(id);

            return Json(new { result = pr });
        }
        public IActionResult UpdateEWayBill(string ewaybill,int dcmid )
        {
            iService.UpdateStockMovementInvoiceEBill(dcmid, ewaybill);
            return Json(new { res = "OK" });
        }
        public IActionResult Updatedispatch(string dispchd,int dcmid)
        {
            iService.UpdateStockMovementInvoiceDispatch(dcmid, dispchd);
            return Json(new { res = "OK" });
        }
        public IActionResult UpdateStandByEWayBill(string ewaybill, int dcmid)
        {
            iService.UpdateStandByInvoiceEBill(dcmid, ewaybill);
            return Json(new { res = "OK" });
        }
        public IActionResult UpdateStandBydispatch(string dispchd, int dcmid)
        {
            iService.UpdateStandByInvoiceDispatch(dcmid, dispchd);
            return Json(new { res = "OK" });
        }
        public IActionResult GetBackOurProduct(int id)
        {
            iService.GetBackFromStandBack(id);
            return RedirectToAction("PreviousStandBy");
        }
        public IActionResult InventoryQuantityReport()
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

            var res = iService.GetCategoryWiswReports();
            return View(res);
        }
        public IActionResult GetCatWiseCount()
        {
            var a = iService.GetCategoryWiswReports();
            return RedirectToAction("Dashboard","EmpireHome");
        }
        public IActionResult StolckReport(int CategoryId = 0,int SubCategoryId = 0,int lId = 0)
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

            PrintList res = new PrintList();
            res.catDrops= cService.GetCatsDrop();
            int cCatid = (CategoryId > 0) ? CategoryId : res.catDrops[0].CategoryId;
            res.SubDrops = cService.GetSubCatsDrop(cCatid);
            res.Items = iService.GetProdsCatWiseToPrint(CategoryId, SubCategoryId, lId);

            return View(res);
        }
    }
}
