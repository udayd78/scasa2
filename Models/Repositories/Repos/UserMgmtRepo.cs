
using SCASA.Models.Repositories.Entity;
using SCASA.Models.ModelClasses;
using SCASA.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public class UserMgmtRepo : IUserMgmtRepo
    {
        private readonly MyDbContext context;

        public UserMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        #region User Type Master
        public void UpdateStaffOfferUrl(string url, int sId)
        {
            var u = context.userMasters.Where(a => a.UserId == sId).FirstOrDefault();
            u.OfferLetterLink = url;
            context.Entry(u).CurrentValues.SetValues(u);
            context.SaveChanges();
        }
        public AppointmentPDFModel GetDataForAppointmentOrder(int sId)
        {
            AppointmentPDFModel result = new AppointmentPDFModel();
            try
            {
                result = (from u in context.userMasters
                          join c in context.cityMasters on u.CityId equals c.Id
                          join s in context.stateMasters on u.StateId equals s.Id
                          join ct in context.countryMasters on u.CountryId equals ct.Id
                          join ru in context.userMasters on u.ReportingManager equals ru.UserId
                          join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                          where u.UserId == sId
                          select new AppointmentPDFModel
                          {
                              ReportingManager = ru.UserName,
                              address1 = u.Address1,
                              address2 = u.Address2,
                              City = c.CityName,
                              Country = ct.CountryName,
                              Designation = ut.TypeName,
                              DOJ = (DateTime)u.DateOfJoin,
                              EmployeeName = u.UserName,
                              PIN = u.ZipCode,
                              Salary = 0,
                              State = s.StateName,

                          }).FirstOrDefault();
                if (result != null)
                {
                    var payroll = context.payRollMasters.Where(a => a.StaffId == sId && a.IsDeleted == false).FirstOrDefault();
                    if (payroll != null)
                    {
                        result.Salary = (decimal)payroll.BasicSalary * 12;
                    }
                }
            }
            catch (Exception ex)
            {

            }
           
            return result;
        }
        public ProcessResponse SaveUserType(UserTypeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.userTypeMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.TypeId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }

            return response;
        }

        public List<UserTypeMaster> GetAllUserTypes()
        {
            List<UserTypeMaster> response = new List<UserTypeMaster>();
            try
            {
                response = context.userTypeMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public UserTypeMaster GetAllUserTypeById(int id)
        {
            UserTypeMaster response = new UserTypeMaster();
            try
            {
                response = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public ProcessResponse UpdateUserType(UserTypeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.TypeId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }

            return response;
        }


        #endregion
        public ProcessResponse SaveUser(UserMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.UserId > 0)
                {
                    UserMaster temp = context.userMasters.Where(a => a.UserId == request.UserId).FirstOrDefault();
                    context.Entry(temp).CurrentValues.SetValues(request);
                    context.SaveChanges();
                    response.currentId = request.UserId;
                    response.statusCode = 1;
                    response.statusMessage = "Success";
                }
                else
                {
                    request.IsDeleted = false;
                    request.IsActive = true;
                    context.userMasters.Add(request);
                    context.SaveChanges();
                    response.currentId = request.UserId;
                    response.statusCode = 1;
                    response.statusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }
            return response;
        }

        public bool EmailAvailablityCheck(string emailId)
        {
            bool result = true;

            var findResult = context.userMasters.Where(a => a.EmailId == emailId).FirstOrDefault();
            if (findResult != null)
            {
                result = false;
            }
            return result;
        }
        public bool MobileAvailablityCheck(string mobileNumber)
        {
            bool result = true;
            var findResult = context.userMasters.Where(a => a.MobileNumber == mobileNumber).FirstOrDefault();
            if (findResult != null)
            {
                result = false;
            }
            return result;
        }
        public UserTypeMaster GetUserTypeById(int id)
        {
            UserTypeMaster result = new UserTypeMaster();
            try
            {
                result = context.userTypeMasters.Where(a => a.TypeId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public ProcessResponse RegisterUser(UserMaster userMaster)
        {
            ProcessResponse result = new ProcessResponse();
            try
            {
                userMaster.PWord = PasswordEncryption.Encrypt(userMaster.PWord);

                context.userMasters.Add(userMaster);
                context.SaveChanges();

                result.currentId = userMaster.UserId;
                result.statusCode = 1;
                result.statusMessage = "Registration is Success";
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public UserMaster GetUserById(int userId)
        {
            UserMaster result = new UserMaster();
            try
            {
                result = context.userMasters.Where(a => a.UserId == userId && a.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return result;
        }
        public int GetUserTypeCode(int userId)
        {
            int result = 0;
            try
            {
                var r = (from u in context.userMasters
                          join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                          where u.UserId == userId
                          select new  
                          {
                               ut.TypeCode
                          }).FirstOrDefault();
                result = (int) r.TypeCode;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return result;
        }
        public UserMaster GetUserByKey(string key)
        {
            UserMaster result = new UserMaster();
            try
            {
                result = context.userMasters.Where(a => a.FormFillUrl.Equals(key) && a.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return result;
        }
        public List<SalesExecutiveListModel> GetUsersByType(string type , int logID , string sear="")
        {
            List<SalesExecutiveListModel> myList = new List<SalesExecutiveListModel>();
            List<UserMaster> uml = new List<UserMaster>();
            UserMaster logedUser = context.userMasters.Where(a => a.IsDeleted == false && a.UserId == logID).FirstOrDefault();
            UserTypeMaster logedType = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeId == logedUser.UserTypeId).FirstOrDefault();
            UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == type).FirstOrDefault();
            if (logedType.TypeName == "Sales Head")
            {
                if (string.IsNullOrEmpty(sear))
                {
                    uml = context.userMasters.Where(a => a.IsDeleted == false && a.UserTypeId == utm.TypeId && a.ReportingManager==logID).OrderBy(a => a.UserName).ToList();
                }
                else
                {
                    uml = context.userMasters.Where(a => a.IsDeleted == false && a.UserTypeId == utm.TypeId && a.UserName.StartsWith(sear) && a.ReportingManager == logID).OrderBy(a => a.UserName).ToList();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(sear))
                {
                    uml = context.userMasters.Where(a => a.IsDeleted == false && a.UserTypeId == utm.TypeId).OrderBy(a => a.UserName).ToList();
                }
                else
                {
                    uml = context.userMasters.Where(a => a.IsDeleted == false && a.UserTypeId == utm.TypeId && a.UserName.StartsWith(sear)).OrderBy(a => a.UserName).ToList();
                }
            }
            if(uml.Count > 0)
            {
                foreach(var v in uml)
                {
                    int totalOrders = context.salesOrderMasters.Where(A => A.StaffId == v.UserId && A.IsDeleted == false).Count();
                    int totalQutes = context.cRFQMasters.Where(a => a.StaffId == v.UserId && a.IsDeleted == false && a.CurrentStatus!= "SO Created").Count();
                    decimal? totalValue = (from so in context.salesOrderDetails
                                          join smo in context.salesOrderMasters on so.SOMId equals smo.SOMId
                                          where smo.StaffId == v.UserId && smo.IsDeleted == false
                                          select new
                                          {
                                              so.TotalPrice
                                          }).Sum(b=>b.TotalPrice);
                    SalesExecutiveListModel sm = new SalesExecutiveListModel();
                    sm.EmailId = v.EmailId;
                    sm.EmergencyContactNumber = v.EmergencyContactNumber;
                    sm.MaxDiscoutPercentage = v.MaxDiscoutPercentage == null ?  0 : (decimal) v.MaxDiscoutPercentage;
                    sm.MobileNumber = v.MobileNumber;
                    sm.ProfileImage = v.ProfileImage;
                    sm.TotalOrders = totalOrders;
                    sm.TotalOrderValue = (decimal) totalValue;
                    sm.TotalQutations = totalQutes;
                    sm.UserId = v.UserId;
                    sm.UserName = v.UserName;
                    myList.Add(sm);
                }
            }



            return myList;
        }
        public UserMasterDisplay GetUserDisplayById(int id)
        {
            UserMasterDisplay umd = new UserMasterDisplay();
            try
            {
                umd = (from u in context.userMasters
                       join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                       join ci in context.cityMasters on u.CityId equals ci.Id
                       join s in context.stateMasters on u.StateId equals s.Id
                       join co in context.countryMasters on u.CountryId equals co.Id
                       join prm in context.payRollMasters on u.UserId equals prm.StaffId
                       where (u.IsDeleted == false && u.UserId == id)
                       select new UserMasterDisplay()
                       {
                           UserName = u.UserName,
                           EmailId = u.EmailId,
                           IsDeleted = u.IsDeleted,
                           MobileNumber = u.MobileNumber,
                           UserId = u.UserId,
                           UserTypeId = u.UserTypeId,
                           UserTypeName = ut.TypeName,
                           CityId = u.CityId,
                           CityId_Name = ci.CityName,
                           StateId = u.StateId,
                           StateId_Name = s.StateName,
                           CountryId = u.CountryId,
                           ProfileImage = u.ProfileImage,
                           CountryId_Name = co.CountryName,
                           IsActive = u.IsActive,
                           EmployeeCode = u.EmployeeCode,
                           CreatedOn = u.CreatedOn,
                           DateOfJoin = u.DateOfJoin,
                           CurrentStatus = u.CurrentStatus,
                           Gender = u.Gender,
                           Address1 = u.Address1,
                           Address2 = u.Address2,
                           ZipCode = u.ZipCode,
                           ReferredbyName = u.ReferredbyName,
                           ReferredByMobile = u.ReferredByMobile,
                           EmergencyContactNumber = u.EmergencyContactNumber,
                           MaxDiscoutPercentage = (decimal)u.MaxDiscoutPercentage,
                           PANNumber = u.PANNumber,
                           PFNumber = u.PFNumber,
                           UANNumber = u.UANNumber,
                           ESICNumber = u.ESICNumber,
                           BioMetricId = u.BioMetricId,
                           BasicSalary = prm.BasicSalary,
                           HRA = prm.HRA,
                           DearnessAllowance = prm.DearnessAllowance,
                           FoodAllowance = prm.FoodAllowance,
                           Conveyance = prm.Conveyance,
                           MedicalAllowances = prm.MedicalAllowances,
                           OtherAllowances = prm.OtherAllowances,
                           ProvidentFund = prm.ProvidentFund,
                           ProfessionalTax = prm.ProfessionalTax,
                           ESIFund = prm.ESIFund,
                           PanCardUploaded = u.PanCard,
                           AadharFront = u.AdharCard,
                           AadharBack = u.AadharBack,
                           ResumeDoc = u.ResumeDoc,
                           PrevExpLetter =u.PreviousRelievingLetter,
                           SalarySlip = u.SalarySlip,

                           TDS = prm.TDS 
                       }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                LogError(ex);
            }
            return umd;
        }
        public UserProfileModel GetUserProfile(int id)
        {
            UserProfileModel upm = new UserProfileModel();
            try
            {
                upm = (from u in context.userMasters
                       join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                       join ci in context.cityMasters on u.CityId equals ci.Id
                       join s in context.stateMasters on u.StateId equals s.Id
                       join co in context.countryMasters on u.CountryId equals co.Id
                       where (u.IsDeleted == false && u.UserId == id)
                       select new UserProfileModel()
                       {
                           UserId = u.UserId,
                           UserName = u.UserName,
                           EmailId = u.EmailId,
                           MobileNumber=u.MobileNumber,
                           UserTypeName=ut.TypeName,
                           Address1=u.Address1,
                           Address2=u.Address2,
                           CityId=u.CityId,
                           CityId_Name=ci.CityName,
                           StateId=u.StateId,
                           StateId_Name=s.StateName,
                           CountryId=u.CountryId,
                           CountryId_Name=co.CountryName,
                           ZipCode=u.ZipCode,
                           ProfileImage=u.ProfileImage
                       }).FirstOrDefault();
            }catch(Exception ex)
            {
                LogError(ex);
            }

            return upm;
        }
        public void UpdatePassword(UserMaster request)
        {
            UserMaster um = context.userMasters.Where(a => a.UserId == request.UserId).FirstOrDefault();
            if (um != null)
            {
                um.PWord = request.PWord;
                context.Entry(um).CurrentValues.SetValues(um);
                context.SaveChanges();
            }
        }
        public UserMaster GetUserByEmail(string email, int userId = 0)
        {
            UserMaster result = new UserMaster();
            try
            {
                if(userId == 0)
                {
                    result = context.userMasters.Where(a => (a.EmailId == email ||
                a.MobileNumber == email) && a.IsDeleted == false).FirstOrDefault();
                }
                else
                {
                    result = context.userMasters.Where(a => (a.EmailId == email ||
                a.MobileNumber == email) && a.IsDeleted == false && a.UserId != userId).FirstOrDefault();
                }
                
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<UserMaster> GetUsersByName(string name)
        {
            List<UserMaster> result = new List<UserMaster>();
            try
            {
                result = context.userMasters.Where(a => a.UserName == name && a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public UserMaster GetUserByCode(string code)
        {
            UserMaster result = new UserMaster();
            try
            {
                result = context.userMasters.Where(a => a.EmployeeCode == code && a.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public LoginResponse LoginCheck(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            try
            {

                DateTime today = DateTime.Now;
                var obj = (from um in context.userMasters
                           join ut in context.userTypeMasters on um.UserTypeId equals ut.TypeId into utTemp
                           from utype in utTemp.DefaultIfEmpty()
                           where (um.EmailId == request.emailid || um.MobileNumber == request.emailid) &&
                           um.IsDeleted == false && um.CurrentStatus == "Active" && um.IsActive == true
                           select new
                           {
                               um.EmailId,
                               um.UserId,
                               um.UserName,
                               utype.TypeName,
                               um.PWord,
                               utype.TypeCode,
                               um.DateOfJoin
                           }).FirstOrDefault();

                if (obj == null)
                {
                    response.statusCode = 0;
                    response.statusMessage = "Emailid / Mobile number not registered";

                }else if (today < obj.DateOfJoin)
                {
                    response.statusCode = 0;
                    response.statusMessage = "Please login from Joining date";
                }
                else
                {
                    string pw = PasswordEncryption.Encrypt(request.pword);
                    string oldPw = PasswordEncryption.Decrypt(obj.PWord);
                    if (!obj.PWord.Equals(pw))
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Password mismatch";

                    }
                    else
                    {
                        response.statusCode = 1;
                        response.statusMessage = "Login success";
                        response.userId = obj.UserId;
                        response.userName = obj.UserName;
                        response.userTypeName = obj.TypeName;
                        response.emailId = obj.EmailId;
                        response.userTypeCode = (int)obj.TypeCode; 
                        // place an entry in LoginTracking 
                        LoginTracking lt = new LoginTracking(); 

                        lt.UserId = obj.UserId;
                        lt.UserName = obj.UserName;
                        lt.Logintime = DateTime.Now;
                        lt.IsDeleted = false;
                        context.loginTrackings.Add(lt);
                        context.SaveChanges();
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed to login";
                return response;
            }
        }

        public OtpTransactions GetOTPInformation(int userId)
        {
            OtpTransactions response = new OtpTransactions();
            try
            {
                response = context.OtpTransactions.Where(a =>  a.UserId == userId
                && a.CurrentStatus == "Draft").FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return response;

        }
        public ProcessResponse SaveOtpInformation(OtpTransactions request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.OtpTransactions.Add(request);
                context.SaveChanges();
                response.currentId = request.TrId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public ProcessResponse UpdateOtpInformation(OtpTransactions request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                OtpTransactions otp = new OtpTransactions();
                otp = context.OtpTransactions.Where(a => a.TrId == request.TrId).FirstOrDefault();
                context.Entry(otp).CurrentValues.SetValues(request);
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed to save";
            }
            return response;
        }

        public List<UserMasterDisplay> GetAllUsers(string type = "")
        {
            List<UserMasterDisplay> response = new List<UserMasterDisplay>();

            if (string.IsNullOrEmpty(type))
            {
                response = (from u in context.userMasters
                            join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                            //join ci in context.cityMasters on u.CityId equals ci.Id
                            //join s in context.stateMasters on u.StateId equals s.Id
                            //join co in context.countryMasters on u.CountryId equals co.Id
                            
                            where u.IsDeleted == false && (u.CurrentStatus == "Active" || u.CurrentStatus == "Relieved" || u.CurrentStatus == "DisActive")
                            select new UserMasterDisplay
                            {
                                UserName = u.UserName,
                                EmailId = u.EmailId,
                                IsDeleted = u.IsDeleted,
                                MobileNumber = u.MobileNumber,
                                UserId = u.UserId,
                                UserTypeId = u.UserTypeId,
                                UserTypeName = ut.TypeName,
                                CityId = u.CityId,
                                //CityId_Name = ci.CityName,
                                StateId = u.StateId,
                                //StateId_Name = s.StateName,
                                CountryId = u.CountryId,
                                ProfileImage = u.ProfileImage,
                                //CountryId_Name = co.CountryName,
                                IsActive = u.IsActive,
                                EmployeeCode = u.EmployeeCode,
                                CreatedOn = u.CreatedOn,
                                DateOfJoin = u.DateOfJoin,
                                CurrentStatus = u.CurrentStatus,
                                Address1=u.Address1
                            }).OrderBy(b => b.UserName).ToList();
            }
            else
            {
                response = (from u in context.userMasters
                            join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                            join ci in context.cityMasters on u.CityId equals ci.Id
                            join s in context.stateMasters on u.StateId equals s.Id
                            join co in context.countryMasters on u.CountryId equals co.Id
                            where u.IsDeleted == false && (u.UserName.StartsWith(type) || ut.TypeName.StartsWith(type)|| u.EmployeeCode.StartsWith(type)||u.EmailId.StartsWith(type))
                            && (u.CurrentStatus == "Active" || u.CurrentStatus == "Relieved" || u.CurrentStatus == "DisActive")
                            select new UserMasterDisplay
                            {
                                UserName = u.UserName,
                                EmailId = u.EmailId,
                                IsDeleted = u.IsDeleted,
                                MobileNumber = u.MobileNumber,
                                UserId = u.UserId,
                                UserTypeId = u.UserTypeId,
                                UserTypeName = ut.TypeName,
                                CityId = u.CityId,
                                CityId_Name = ci.CityName,
                                StateId = u.StateId,
                                StateId_Name = s.StateName,
                                ProfileImage = u.ProfileImage,
                                CountryId = u.CountryId,
                                CountryId_Name = co.CountryName,
                                IsActive = u.IsActive,
                                EmployeeCode = u.EmployeeCode,
                                CreatedOn = u.CreatedOn,
                                DateOfJoin = u.DateOfJoin,
                                CurrentStatus = u.CurrentStatus,
                                Address1 = u.Address1
                            }).OrderBy(b => b.UserName).ToList();
            }
            return response;
        }
        public List<NewStaffListDisplayModel> GetNewUser(string type = "")
        {
            List<NewStaffListDisplayModel> response = new List<NewStaffListDisplayModel>();
            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    response = (from u in context.userMasters
                                join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                                where u.IsDeleted == false &&(u.CurrentStatus =="New" || u.CurrentStatus == "SubmittedForReview" || u.CurrentStatus == "SentForApproval" || u.CurrentStatus =="Approved")
                                select new NewStaffListDisplayModel
                                {
                                    UserId = u.UserId,
                                    UserName = u.UserName,
                                    EmailId = u.EmailId,
                                    MobileNumber = u.MobileNumber,
                                    UserTypeId = u.UserTypeId,
                                    UserType_Name = ut.TypeName,
                                    CreatedOn = (DateTime)u.CreatedOn,
                                    CreatedBy = u.CreatedBy,
                                    CurrentStatus = u.CurrentStatus,
                                }).OrderBy(a => a.UserName).ToList();
                }
                else
                {
                    response = (from u in context.userMasters
                                join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                                where u.IsDeleted == false && (u.UserName.StartsWith(type) || u.EmployeeCode.StartsWith(type) || u.EmailId.StartsWith(type))
                                && (u.CurrentStatus == "New" || u.CurrentStatus == "SubmittedForReview" || u.CurrentStatus == "SentForApproval"|| u.CurrentStatus == "Approved")
                                select new NewStaffListDisplayModel
                                {
                                    UserId = u.UserId,
                                    UserName = u.UserName,
                                    EmailId = u.EmailId,
                                    MobileNumber = u.MobileNumber,
                                    UserTypeId = u.UserTypeId,
                                    UserType_Name = ut.TypeName,
                                    CreatedOn = (DateTime)u.CreatedOn,
                                    CreatedBy = u.CreatedBy,
                                    CurrentStatus = u.CurrentStatus,
                                }).OrderBy(a => a.UserName).ToList();
                }
            }
            catch(Exception ex)
            {
                LogError(ex);
            }
            return response;
        }
        public List<EmployeeTargetList> GetSalesEmployeeForTargets(int id=0, string se="")
        {
            List<EmployeeTargetList> res = new List<EmployeeTargetList>();
            if (id == 0)
            {
                if (!string.IsNullOrEmpty(se))
                {
                    UserTypeMaster utm = context.userTypeMasters.Where(a =>a.IsDeleted==false && a.TypeName == "Sales Head").FirstOrDefault();
                    res = (from um in context.userMasters

                           where um.IsDeleted == false && um.UserTypeId == utm.TypeId && (um.UserName.StartsWith(se)||um.EmployeeCode.StartsWith(se)) && um.CurrentStatus=="Active"
                           select new EmployeeTargetList
                           {
                               UserId=um.UserId,
                               UserName = um.UserName,
                               EmployeeCode = um.EmployeeCode,
                               EmailId = um.EmailId,
                               MobileNumber = um.MobileNumber
                           }).OrderBy(a => a.UserName).ToList();
                }
                else
                {
                    UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Sales Head").FirstOrDefault();
                    res = (from um in context.userMasters

                           where um.IsDeleted == false && um.UserTypeId == utm.TypeId && um.CurrentStatus == "Active"
                           select new EmployeeTargetList
                           {
                               UserId = um.UserId,
                               UserName = um.UserName,
                               EmployeeCode = um.EmployeeCode,
                               EmailId = um.EmailId,
                               MobileNumber = um.MobileNumber
                           }).OrderBy(a => a.UserName).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(se))
                {
                    res = (from um in context.userMasters

                           where um.IsDeleted == false && um.ReportingManager == id && (um.UserName.StartsWith(se) || um.EmployeeCode.StartsWith(se))
                           select new EmployeeTargetList
                           {
                               UserId = um.UserId,
                               UserName = um.UserName,
                               EmployeeCode=um.EmployeeCode,
                               EmailId = um.EmailId,
                               MobileNumber = um.MobileNumber
                           }).OrderBy(a => a.UserName).ToList();
                }
                else
                {
                    res = (from um in context.userMasters

                           where um.IsDeleted == false && um.ReportingManager == id
                           select new EmployeeTargetList
                           {
                               UserId = um.UserId,
                               UserName = um.UserName,
                               EmployeeCode = um.EmployeeCode,
                               EmailId = um.EmailId,
                               MobileNumber = um.MobileNumber
                           }).OrderBy(a => a.UserName).ToList();
                }
            }

            return res;
        }
        
        public int getUser_Count(String type = "", int pageNumber = 1, int pageSize = 10)
        {
            int total = 0;
            if (!string.IsNullOrEmpty(type))
            {
                total = (from u in context.userMasters
                         join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                         join ci in context.cityMasters on u.CityId equals ci.Id
                         join s in context.stateMasters on u.StateId equals s.Id
                         join co in context.countryMasters on u.CountryId equals co.Id
                         where u.IsDeleted == false && ut.TypeName == type
                         select new UserMasterDisplay
                         {
                             EmployeeCode = u.EmployeeCode
                         }).Count();
            }
            else
            {
                total = (from u in context.userMasters
                         join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                         join ci in context.cityMasters on u.CityId equals ci.Id
                         join s in context.stateMasters on u.StateId equals s.Id
                         join co in context.countryMasters on u.CountryId equals co.Id
                         where u.IsDeleted == false
                         select new UserMasterDisplay
                         {
                             EmployeeCode = u.EmployeeCode
                         }).Count();
            }
            return total;
        }

        //public ProcessResponse SendRegistrationEmail(string moduleName, string toEamil, string userName,
        //    int userId)
        //{
        //    ProcessResponse ps = new ProcessResponse();
        //    try
        //    {
        //        EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
        //        emailTemplate = _nService.GetEmailTemplateByModule(moduleName);
        //        if (emailTemplate != null)
        //        {

        //            string HostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
        //            UserVerificationEntity userVerification = new UserVerificationEntity();
        //            string keyRegistration = string.Empty;
        //            keyRegistration = DateTime.UtcNow.ToString();
        //            keyRegistration = Regex.Replace(keyRegistration, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
        //            keyRegistration += userId.ToString();

        //            userVerification.ActivationKey = keyRegistration;
        //            userVerification.ActivationURL = HostURL + "/Authenticate/Activate?key=" + keyRegistration;
        //            userVerification.DOR = DateTime.UtcNow;
        //            userVerification.Status = "Draft";
        //            userVerification.UserID = userId;
        //            SaveUserVerification(userVerification);
        //            string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
        //            string emailText = emailTemplate.EmailTemplate;
        //            emailText = emailText.Replace("##USERNAME##", userName);
        //            emailText = emailText.Replace("##URL##", userVerification.ActivationURL);
        //            bool res = _nService.PushEmail(emailText, toEamil, emailTemplate.Subject, emailCC);
        //            ps.statusMessage = "email sent";
        //            ps.statusCode = 1;


        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ps.statusMessage = ex.Message;
        //        ps.statusCode = 0;
        //    }

        //    return ps;

        //}
        //public ProcessResponse SaveUserVerification(UserVerificationEntity request)
        //{
        //    ProcessResponse response = new ProcessResponse();
        //    try
        //    {
        //        context.userVerificationEntities.Add(request);
        //        context.SaveChanges();
        //        response.statusCode = 1;
        //        response.statusMessage = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.statusCode = 0;
        //        response.statusMessage = "Failed";
        //    }
        //    return response;

        //}


        //public ProcessResponse InitiateResetPassword(string emailId)
        //{
        //    ProcessResponse ps = new ProcessResponse();
        //    try
        //    {

        //        var um = context.userMasters.Where(a => a.EmailId == emailId && a.IsDeleted == false && a.CurrentStatus == "Active").FirstOrDefault();
        //        if (um != null)
        //        {
        //            string key = RandomGenerator.RandomString(8, false);
        //            key = key + um.UserId.ToString();


        //            ResetPasswordEntity rp = new ResetPasswordEntity();
        //            rp.CurrentStatus = "Draft";
        //            rp.IsDeleted = false;
        //            rp.RaisedOn = DateTime.UtcNow;
        //            rp.ResetKey = key;
        //            rp.UserId = um.UserId;

        //            context.resetPasswordEntities.Add(rp);
        //            context.SaveChanges();
        //            ps.statusMessage = key;
        //            ps.statusCode = 1;
        //            ps.currentId = um.UserId;
        //        }
        //        else
        //        {
        //            ps.statusCode = 0;
        //            ps.statusMessage = "Email ID not registered or your account is de-activated.";
        //            ps.currentId = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ps.statusCode = 0;
        //        ps.statusMessage = "failed";
        //    }
        //    return ps;

        //}
        //public ProcessResponse CompletePasswordRequest(string key)
        //{
        //    ProcessResponse ps = new ProcessResponse();
        //    ResetPasswordEntity rs = new ResetPasswordEntity();

        //    rs = context.resetPasswordEntities.Where(a => a.ResetKey.Equals(key) && a.CurrentStatus.Equals("Draft")).FirstOrDefault();
        //    if (rs != null)
        //    {
        //        rs.ResetOn = DateTime.UtcNow;
        //        rs.CurrentStatus = "Used";
        //        context.Entry(rs).CurrentValues.SetValues(rs);
        //        context.SaveChanges();
        //        ps.statusCode = 1;
        //        ps.statusMessage = "Success";
        //        ps.currentId = Convert.ToInt32(rs.UserId);
        //    }
        //    else
        //    {
        //        ps.statusCode = 0;
        //        ps.statusMessage = "Failed";
        //    }


        //    return ps;

        //}

        public ProcessResponse UpdatePasswordReset(PasswordChangeRequest request)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {

                UserMaster um = new UserMaster();

                um = context.userMasters.Where(a => a.UserId == request.UserId).FirstOrDefault();

                um.PWord = PasswordEncryption.Encrypt(request.password);
                context.Entry(um).CurrentValues.SetValues(um);
                ps.statusCode = 1;
                ps.statusMessage = "Success";
            }
            catch (Exception ex)
            {  
                ps.statusMessage = "Unable to update";
                ps.statusCode = 0;
            }
            return ps;
        }

        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }


        public ProcessResponse InitiateResetPassword(string emailId)
        {
            ProcessResponse ps = new ProcessResponse();
            var um = context.userMasters.Where(a => a.EmailId == emailId && a.IsDeleted == false && a.CurrentStatus == "Active").FirstOrDefault();
            if (um != null)
            {
                string key = RandomGenerator.RandomString(8, false);
                key = key + um.UserId.ToString();


                OtpTransactions rp = new OtpTransactions();
                rp.CurrentStatus = "Draft"; 
                rp.CreatedOn = DateTime.Now;
                rp.EmailOTP = RandomGenerator.RandomNumber(1000, 9999).ToString();
                rp.MobileOTP = RandomGenerator.RandomNumber(1000, 9999).ToString();
                rp.UserId = um.UserId;
                context.OtpTransactions.Add(rp);
                context.SaveChanges(); ;
                ps.statusCode = 1;
                ps.currentId = um.UserId;
                ps.statusMessage = rp.EmailOTP;

            }
            else
            {
                ps.statusCode = 0;
                ps.statusMessage = "Email ID not registered or your account is de-activated.";
                ps.currentId = 0;
            }
            return ps;

        }
        public ProcessResponse CompletePasswordRequest(string emailOtp, int userid, string pword)
        {
            ProcessResponse ps = new ProcessResponse();
            OtpTransactions rs = new OtpTransactions();

            rs = context.OtpTransactions.Where(a => a.UserId == userid && a.EmailOTP == emailOtp &&  a.CurrentStatus.Equals("Draft")).FirstOrDefault();
            if (rs != null)
            {
                rs.UsedOn = DateTime.Now;
                rs.CurrentStatus = "Used";
                context.Entry(rs).CurrentValues.SetValues(rs);
                context.SaveChanges();
                ps.statusCode = 1;
                ps.statusMessage = "Success";
                ps.currentId = Convert.ToInt32(rs.UserId);

                // update password
                UserMaster um = new UserMaster();
                um = context.userMasters.Where(a => a.UserId == userid).FirstOrDefault();
                um.PWord = PasswordEncryption.Encrypt(pword);
                context.Entry(um).CurrentValues.SetValues(um);
                context.SaveChanges();
            }
            else
            {
                ps.statusCode = 0;
                ps.statusMessage = "Invalid OTP or OTP Expired";
            }
            return ps;
        }
        public List<StaffAttDisplayModel> GetStaffListForAtt()
        {
            List<StaffAttDisplayModel> myList = new List<StaffAttDisplayModel>();
            myList = (from u in context.userMasters
                      join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                      where u.IsDeleted == false && u.IsActive == true && u.CurrentStatus == "Active"
                      && (ut.TypeName != "Admin" || ut.TypeName != "SuperAdmin" || ut.TypeName != "CEO")
                      select new StaffAttDisplayModel
                      {
                           EmployeeCode = u.EmployeeCode,
                            StaffId = u.UserId,
                             attList =null,
                              StaffName=u.UserName
                      }).ToList();
            return myList;
        }

        public string GetPresentDayAttendance(int userid, DateTime date)
        {
            string aStatus = "NA";
            AttendanceMaster am = context.attendanceMasters.Where(a => a.StaffId == userid && a.DateofAttendance == date).FirstOrDefault();
            if(am != null)
            {
                aStatus = am.AttStatus;
            }
            return aStatus;
        }
        public List<StaffDrop> GetStaffForSalary()
        {
            UserTypeMaster admin = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Admin").FirstOrDefault();
            UserTypeMaster sAdmin = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "SuperAdmin").FirstOrDefault();
            UserTypeMaster cEO = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "CEO").FirstOrDefault();
            return (from u in context.userMasters
                    where (u.IsDeleted == false && (u.UserTypeId == admin.TypeId || u.UserTypeId == sAdmin.TypeId || u.UserTypeId == cEO.TypeId))
                    select new StaffDrop
                    {
                        StaffId = u.UserId,
                        StaffName = u.UserName,
                        EmployeeCode = u.EmployeeCode
                    }
                   ).OrderBy(b => b.StaffName).ToList();
        }
        public DateTime GetLastLoginTime(int id)
        {
            DateTime lt = new DateTime();
            try
            {
                List<LoginTracking> obj = context.loginTrackings.Where(a => a.IsDeleted == false && a.UserId == id).OrderBy(b=>b.Logintime).ToList();
                int cont = obj.Count;
                if (cont < 2)
                {
                    lt = (DateTime)obj[0].Logintime;
                }
                lt = (DateTime)obj[cont-2].Logintime;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return lt;
        }
        #region  Targets
        public ProcessResponse SaveTarget(TargetMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.TargetId > 0)
                {
                    UserMaster temp = context.userMasters.Where(a => a.UserId == request.UserId).FirstOrDefault();
                    context.Entry(temp).CurrentValues.SetValues(request);
                    context.SaveChanges();
                    response.currentId = request.TargetId;
                    response.statusCode = 1;
                    response.statusMessage = "Success";
                }
                else
                {
                    request.IsDeleted = false;
                    context.targetMasters.Add(request);
                    context.SaveChanges();
                    response.currentId = request.TargetId;
                    response.statusCode = 1;
                    response.statusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }
            return response;
        }
        public List<TargetMaster> GetAllTargetsOfEmployee(int id)
        {
            List<TargetMaster> lTM = new List<TargetMaster>();
            lTM = context.targetMasters.Where(a => a.UserId == id && a.IsDeleted == false).OrderBy(a=>a.YearNumber).ToList();
            return lTM;
        }
        public TargetMaster GetTargetOfUserByMonth(int uId, int y, int m)
        {
            TargetMaster tm = context.targetMasters.Where(a => a.IsDeleted == false && a.UserId == uId && a.MonthNumber == m && a.YearNumber == y).FirstOrDefault();

            return tm;
        }
        public List<TargetMaster> GetAllTargetOfheadByMonth(int uId, int y, int m)
        {
            List<TargetMaster> tml = new List<TargetMaster>();
            List<UserMaster> subEmployees = context.userMasters.Where(a => a.IsDeleted == false && a.ReportingManager == uId).ToList();
            foreach (UserMaster u in subEmployees)
            {
                TargetMaster tm = context.targetMasters.Where(a => a.IsDeleted == false && a.UserId == u.UserId && a.MonthNumber == m && a.YearNumber == y).FirstOrDefault();
                if (tm != null)
                {
                    tml.Add(tm);
                }
            }
            return tml;
        }
        #endregion

        // activity logs

        public void UpdateActivityLog(ActivityLog request)
        {
            try
            {
                if (request.ActivityId > 0)
                {
                    var a = context.activityLogs.Where(a => a.ActivityId == request.ActivityId).FirstOrDefault();
                    context.Entry(a).CurrentValues.SetValues(request);
                    context.SaveChanges();
                }
                else
                {
                    context.activityLogs.Add(request);
                    context.SaveChanges();
                }

            }catch(Exception ex)
            {

            }
        }
        public ActivityLog GetActivtyLogById(int id)
        {
            return context.activityLogs.Where(a => a.ActivityId == id).FirstOrDefault();
        }

        public List<ActivityLog> GetMyActivityLogs(int userid)
        {
            List<ActivityLog> list = new List<ActivityLog>();
            int? ustypeId = GetUserById(userid).UserTypeId;
            var utName = GetAllUserTypeById((int)ustypeId);
            
            if (utName.TypeName == "CEO" || utName.TypeName =="Admin" || utName.TypeName == "Super Admin")
            {
               list= context.activityLogs.Where(a => a.IsDeleted == false).OrderByDescending(b => b.ActivityOn)
               .ToList();
            }
            else
            {
              list=  context.activityLogs.Where(a => a.IsDeleted == false && a.TargerUserId == userid).OrderByDescending(b => b.ActivityOn)
               .ToList();
            }

             
            return list;
        }
        public List<ActivityLog> GetMyActivityLogsUnread(int userid)
        {
            return context.activityLogs.Where(a => a.IsDeleted == false && a.TargerUserId == userid && a.IsRead == false).OrderByDescending(b => b.ActivityOn)
                .ToList();
        }
        public int GetMyActivityLogsUnreadCount(int userid)
        {
            return context.activityLogs.Where(a => a.IsDeleted == false && a.TargerUserId == userid && a.IsRead == false).Count();
        }
        public int GetMyActivityLogsCount(int userid)
        {
            return context.activityLogs.Where(a => a.IsDeleted == false && a.TargerUserId == userid).Count();
        }
        public int GetAdminId()
        {
            int result = 0;

            try
            {
                int utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Admin").Select(b=>b.TypeId).FirstOrDefault();
                result = context.userMasters.Where(a => a.IsDeleted == false && a.UserTypeId == utm).Select(b => b.UserId).FirstOrDefault();
            }
            catch { }

            return result;
        }
    }
}

