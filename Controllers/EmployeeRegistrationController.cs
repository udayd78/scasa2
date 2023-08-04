using SCASA.Models.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCASA.Controllers
{
    public class EmployeeRegistrationController : Controller
    {
        private readonly IUserMgmtService uService;
        private readonly ICommonService cService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly INotificationService nService;
        private readonly IConfiguration _config;

        public EmployeeRegistrationController(IUserMgmtService _uServices,ICommonService 
            _cService,IHostingEnvironment _hostingEnvironment,INotificationService _nService, IConfiguration confg)
        {
            uService = _uServices;
            cService = _cService;
            hostingEnvironment = _hostingEnvironment;
            nService = _nService;
            _config = confg;
        }
        public IActionResult CompleteForm(string key="")
        {
            UserMaster myObj = new UserMaster();

            UserMasterModel finalObject = new UserMasterModel();
            if (!string.IsNullOrEmpty(key))
            {
                myObj = uService.GetUserByKey(key);
                if (myObj.CurrentStatus != "New")
                {
                    return RedirectToAction("UserDetailsSubmitionDone");
                }
                CloneObjects.CopyPropertiesTo(myObj, finalObject);
                finalObject.AadharNumber = finalObject.AadharNumber != null ?  PasswordEncryption.Decrypt(finalObject.AadharNumber): "";
                finalObject.AccountNumber =finalObject.AccountNumber!=null? PasswordEncryption.Decrypt(finalObject.AccountNumber):"";
                finalObject.BankName =finalObject.BankName!=null? PasswordEncryption.Decrypt(finalObject.BankName):"";
                finalObject.BranchAddress = finalObject.BranchAddress!=null? PasswordEncryption.Decrypt(finalObject.BranchAddress):"";
                finalObject.IfscCode =finalObject.IfscCode!=null? PasswordEncryption.Decrypt(finalObject.IfscCode):"";
                finalObject.AccountHolderName =finalObject.AccountHolderName!=null? PasswordEncryption.Decrypt(finalObject.AccountHolderName):"";
                finalObject.UPINumber =finalObject.UPINumber!=null? PasswordEncryption.Decrypt(finalObject.UPINumber):"";
                UserTypeMaster utm= uService.GetUserTypeById((int)myObj.UserTypeId);
                ViewBag.UserTypeName = utm.TypeName;
                if (myObj == null)
                {
                    return RedirectToAction("UserDetailsSubmitionDone");
                }
            }
            finalObject.userTypeDrops = cService.GetUserTypeDrops();
            finalObject.countryDrops = cService.GetAllCountries();
            int countryId = 101;
            if (key =="")
            {
                countryId = finalObject.CountryId == null ? 101 : (int) finalObject.CountryId;
            }
            finalObject.stateDrops = cService.GetAllStates(countryId);
            //int stateId = key != "" ? finalObject.StateId : finalObject.stateDrops[0].StateId;
            int stateId = finalObject.stateDrops[0].StateId;
            
            finalObject.cityDrops = cService.GetAllCities(stateId);
            return View(finalObject);
        }

        [HttpPost]
        public IActionResult CompleteForm(UserMasterModel request)
        {
            if (request.DateOfBirth == null)
            {
                ModelState.AddModelError("DateOfBirth", "Please Select Date Of Birth");
            }
            if(ModelState.IsValid)
            {
                if (request.CurrentStatus == "New")
                {
                    UserMaster um = uService.GetUserById(request.UserId);
                    request.IsActive = um.IsActive;
                    request.IsDeleted = um.IsDeleted;
                    request.CreatedBy = um.CreatedBy;
                    request.CreatedOn = um.CreatedOn;
                    request.UserTypeId = um.UserTypeId;
                    request.FormFillUrl = um.FormFillUrl;
                    request.CurrentStatus = "SubmittedForReview";
                    request.LastModifiedBy = request.UserName;
                    request.LastModifiedOn = DateTime.Now;
                    request.SubMitedOn = DateTime.Now;
                    request.BankName = PasswordEncryption.Encrypt(request.BankName);
                    request.AccountNumber = PasswordEncryption.Encrypt(request.AccountNumber);
                    request.IfscCode = PasswordEncryption.Encrypt(request.IfscCode);
                    request.AccountHolderName = PasswordEncryption.Encrypt(request.AccountHolderName);
                    request.BranchAddress = PasswordEncryption.Encrypt(request.BranchAddress);
                    request.UPINumber = PasswordEncryption.Encrypt(request.UPINumber);
                    request.AadharNumber = PasswordEncryption.Encrypt(request.AadharNumber);
                    bool isProfileImageUploaded = false;
                    string ProfileImageName = "";
                    if (request.ProfileImageUploaded != null)
                    {
                        var fileNameUploaded = Path.GetFileName(request.ProfileImageUploaded.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.ProfileImageUploaded.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.ProfileImageUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                            ProfileImageName = filename;
                            isProfileImageUploaded = true;
                        }
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
                    UserMaster response = new UserMaster();
                    CloneObjects.CopyPropertiesTo(request, response);
                    response.ProfileImage = isProfileImageUploaded ? ProfileImageName : null;
                    response.PanCard = isPanCardUploaded ? PanCardName : null;
                    response.AdharCard = isAdharCardUploaded ? AdharCardName : null;
                    response.AadharBack = isAdharBackUploaded ? AadharBackName : null;
                    response.ResumeDoc = isResumeDocUploaded ? ResumeDocName : null;
                    response.SalarySlip = isSalarySlipUploaded ? SalarySlipName : null;
                    response.PreviousRelievingLetter = isPreviousRelievingLetterUploaded ? PreviousRelievingLetterName : null;
                    ProcessResponse resp =uService.SaveUser(response);
                    string adminemail = _config.GetValue<string>("EmailConfig:adminEmail");
                    string adminPersonName = _config.GetValue<string>("EmailConfig:adminPersonName");
                    UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
                    
                    if (resp.statusCode == 1)
                    {
                        var mTA = nService.SendReviewEmpNotificationToAdmin(AppSettings.EmailTemplates.ReviewEmpNotificationToAdmin, adminemail,
                        adminPersonName, request.UserName, utm.TypeName);
                        return RedirectToAction("UserDetailsSubmitionDone");
                    }
                    else
                    {
                        ViewBag.ermsg = resp.statusMessage;
                    }
                }
            }
            request.userTypeDrops = cService.GetUserTypeDrops();
            request.countryDrops = cService.GetAllCountries();
            int countryId = (int) request.CountryId;
            request.stateDrops = cService.GetAllStates(countryId);
            int stateId = request.StateId;
            request.cityDrops = cService.GetAllCities(stateId);
            ViewBag.ErrorMessaage = "Please Fill All the Required Fields";
            return View(request);
        }
        public IActionResult UserDetailsSubmitionDone()
        {
            return View();
        }
    }
}
