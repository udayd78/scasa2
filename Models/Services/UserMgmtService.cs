using SCASA.Models.Utilities;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCASA.Models.Services
{
    public class UserMgmtService : IUserMgmtService
    {
        private readonly IUserMgmtRepo uRepo;
        private readonly INotificationService _nService;
        public UserMgmtService(IUserMgmtRepo userMgmtRepo, INotificationService nService)
        {
            uRepo = userMgmtRepo;
            _nService = nService;
        }

        #region  User Type Repo
        public ProcessResponse SaveUserType(UserTypeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.TypeId > 0)
                {
                    UserTypeMaster um = new UserTypeMaster();
                    um = uRepo.GetAllUserTypeById(request.TypeId);
                    um.TypeName = request.TypeName;
                    um.TypeCode = request.TypeCode;
                    response = uRepo.UpdateUserType(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = uRepo.SaveUserType(request);
                }


            }
            catch (Exception ex)
            {
                uRepo.LogError(ex);
            }

            return response;
        }


        public List<UserTypeMaster> GetAllUserTypes()
        {
            return uRepo.GetAllUserTypes();
        }

        public UserTypeMaster GetUserTypeById(int id)
        {
            return uRepo.GetAllUserTypeById(id);
        }

        public ProcessResponse UpdateUserType(UserTypeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                response = uRepo.UpdateUserType(request);
            }
            catch (Exception ex)
            {
                uRepo.LogError(ex);
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
                    UserMaster myObj = new UserMaster();
                    myObj.UserId = request.UserId;
                    myObj.PWord = request.PWord;
                    myObj.UserName = request.UserName;
                    myObj.EmailId = request.EmailId;
                    myObj.MobileNumber = request.MobileNumber;
                    myObj.UserTypeId = request.UserTypeId;
                    myObj.Address1 = request.Address1;
                    myObj.Address2 = request.Address2;
                    myObj.CityId = request.CityId;
                    myObj.StateId = request.StateId;
                    myObj.CountryId = request.CountryId;
                    myObj.ZipCode = request.ZipCode;
                    myObj.ReferredByMobile = request.ReferredByMobile;
                    myObj.ReferredbyName = request.ReferredbyName;
                    myObj.IsActive = request.IsActive;
                    myObj.IsDeleted = request.IsDeleted;
                    myObj.CreatedBy = request.CreatedBy;
                    myObj.CreatedOn = request.CreatedOn;
                    myObj.LastModifiedBy = request.LastModifiedBy;
                    myObj.LastModifiedOn = request.LastModifiedOn;
                    myObj.EmergencyContactNumber = request.EmergencyContactNumber;
                    myObj.ProfileImage = request.ProfileImage;
                    myObj.MaxDiscoutPercentage = request.MaxDiscoutPercentage;
                    myObj.ReleivedOn = request.ReleivedOn;
                    myObj.ReleivedRemarks = request.ReleivedRemarks;
                    myObj.EmployeeCode = request.EmployeeCode;
                    myObj.CurrentStatus = request.CurrentStatus;
                    myObj.PANNumber = request.PANNumber;
                    myObj.PFNumber = request.PFNumber;
                    myObj.UANNumber = request.UANNumber;
                    myObj.ESICNumber = request.ESICNumber;
                    myObj.BioMetricId = request.BioMetricId;
                    myObj.DateOfBirth = request.DateOfBirth;
                    myObj.Gender = request.Gender;
                    myObj.DateOfJoin = request.DateOfJoin;
                    myObj.AadharNumber = request.AadharNumber;
                    myObj.PreviousOrgAddress = request.PreviousOrgAddress;
                    myObj.PreviousOrgExperience = request.PreviousOrgExperience;
                    myObj.PreviousOrgName = request.PreviousOrgName;
                    myObj.TotalPreviousExpeience = request.TotalPreviousExpeience;
                    myObj.AdharCard = request.AdharCard;
                    myObj.PanCard = request.PanCard;
                    myObj.PreviousRelievingLetter = request.PreviousRelievingLetter;
                    myObj.ResumeDoc = request.ResumeDoc;
                    myObj.SalarySlip = request.SalarySlip;
                    myObj.ReportingManager = request.ReportingManager;
                    myObj.SubMitedOn = request.SubMitedOn;
                    myObj.ApprovedOn = request.ApprovedOn;
                    myObj.ApprovedBy = request.ApprovedBy;
                    myObj.ReviewedOn = request.ReviewedOn;
                    myObj.ReviewedBy = request.ReviewedBy;
                    myObj.AcceptedOn = request.AcceptedOn;
                    myObj.ShiftId = request.ShiftId;
                    myObj.FormFillUrl = request.FormFillUrl;
                    myObj.BankName = request.BankName;
                    myObj.AccountNumber = request.AccountNumber;
                    myObj.IfscCode = request.IfscCode;
                    myObj.AccountHolderName = request.AccountHolderName;
                    myObj.BranchAddress = request.BranchAddress;
                    myObj.UPINumber = request.UPINumber;
                    myObj.AadharBack = request.AadharBack;
                    myObj.DisplayName = request.DisplayName;
                    myObj.WhatsappNumber = request.WhatsappNumber;
                    myObj.OfficialEmail = request.OfficialEmail;
                    response = uRepo.SaveUser(myObj);
                }
                else
                {
                    string url = RandomGenerator.RandomString(40, true) + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString();
                    request.CurrentStatus = "New";
                    request.FormFillUrl = url;
                    response = uRepo.SaveUser(request);
                    if (response.statusCode == 1)
                    {
                        // update employee code;
                        UserMaster um = uRepo.GetUserById(response.currentId);
                        um.EmployeeCode = "EM" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + um.UserId;
                        var r = uRepo.SaveUser(um);

                        //sending email 
                        var email = _nService.SendRegistrationEmail(AppSettings.EmailTemplates.InviteForRegistration, um.EmailId, um.UserName, url);
                    }
                }
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed " + ex.Message;
            }
            return response;
        }
        public bool EmailAvailablityCheck(string emailId)
        {
            bool result = true;

            var findResult = uRepo.EmailAvailablityCheck(emailId);
            return result;
        }
        public bool MobileAvailablityCheck(string mobileNumber)
        {
            bool result = true;
            var findResult = uRepo.MobileAvailablityCheck(mobileNumber);

            return result;
        }
        public ProcessResponse RegisterUser(UserMaster userMaster)
        {
            ProcessResponse result = new ProcessResponse();
            try
            {
                result = uRepo.RegisterUser(userMaster);
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
                result = uRepo.GetUserById(userId);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<SalesExecutiveListModel> GetUsersByType(string type, int logID, string sear = "")
        {
            return uRepo.GetUsersByType(type , logID , sear);
        }
        public UserMasterDisplay GetUserDisplayModelById(int userId)
        {
            UserMasterDisplay result = new UserMasterDisplay();
            try
            {
                result = uRepo.GetUserDisplayById(userId);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public UserProfileModel GetUserShortProfileById(int userId)
        {
            UserProfileModel result = new UserProfileModel();
            try
            {
                result = uRepo.GetUserProfile(userId);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public void UpdatePassword(UserMaster request)
        {
            uRepo.UpdatePassword(request);
        }
        public UserMaster GetUserByEmail(string email, int userid = 0)
        {
            UserMaster result = new UserMaster();
            try
            {
                result = uRepo.GetUserByEmail(email, userid);
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

                response = uRepo.LoginCheck(request);
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
                response = uRepo.GetOTPInformation(userId);
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
                response = uRepo.SaveOtpInformation(request);

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
                response = uRepo.UpdateOtpInformation(request);
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
            return uRepo.GetAllUsers(type);
        }
        public List<NewStaffListDisplayModel> GetNewUsers(string type = "")
        {
            return uRepo.GetNewUser(type);
        }
        public List<EmployeeTargetList> GetStaffForTargetList(int id = 0,string s="")
        {
            //if (id != 0)
            //{
            //    UserMaster um = uRepo.GetUserById(id);
            //    UserTypeMaster utm = uRepo.GetUserTypeById((int)um.UserTypeId);
            //}
            return uRepo.GetSalesEmployeeForTargets(id,s);
        }
        public int GetUser_Count(string type = "", int pageNumber = 1, int pageSize = 10)
        {
            return uRepo.getUser_Count(type, pageNumber, pageSize);
        }

        public ProcessResponse InitiateResetPassword(string emailId)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {

                ps = uRepo.InitiateResetPassword(emailId);
                if (ps.statusCode == 1)
                {
                    // send email
                    var udata = uRepo.GetUserByEmail(emailId);
                    var email = _nService.SendResetPasswordEmail(AppSettings.EmailTemplates.ForgotPassword, ps.statusMessage, udata.EmailId, udata.UserName, udata.UserId);
                }

            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = "failed";
            }
            return ps;

        }
        public ProcessResponse CompletePasswordRequest(string key, int userid, string pword)
        {
            ProcessResponse ps = new ProcessResponse();

            ps = uRepo.CompletePasswordRequest(key, userid, pword);


            return ps;

        }

        public ProcessResponse UpdatePasswordReset(PasswordChangeRequest request)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {

                ps = uRepo.UpdatePasswordReset(request);
            }
            catch (Exception ex)
            {

                ps.statusMessage = "Unable to update";
                ps.statusCode = 0;
            }
            return ps;
        }
        public List<UserMasterDisplay> GetSearchUsers(string uData)
        {
            UserMasterDisplay umd = new UserMasterDisplay();
            List<UserMasterDisplay> myList = new List<UserMasterDisplay>();
            
            UserMaster um = new UserMaster();
            List<UserMaster> ul = new List<UserMaster>();

            um = uRepo.GetUserByEmail(uData);
            
            if (um == null)
            {
                um = uRepo.GetUserByCode(uData);
                if (um == null)
                {
                    ul = uRepo.GetUsersByName(uData);
                    if (ul != null)
                    {
                        foreach(UserMaster u in ul)
                        {
                            CloneObjects.CopyPropertiesTo(u, umd);
                            myList.Add(umd);
                        }
                    }
                }
                else
                {
                    CloneObjects.CopyPropertiesTo(um, umd);
                    myList.Add(umd);
                }
            }
            else
            {
                CloneObjects.CopyPropertiesTo(um, umd);
                myList.Add(umd);
            }
            

            return myList;
        }
        public List<StaffAttDisplayModel> GetStaffForAtt()
        {
            List<StaffAttDisplayModel> myList = new List<StaffAttDisplayModel>();

            myList = uRepo.GetStaffListForAtt();
            return myList;
        }
        public List<StaffAttDisplayModel> GetAttandance(DateTime dt, string employeecode = "")
        {
            List<StaffAttDisplayModel> myList = new List<StaffAttDisplayModel>();

            myList = uRepo.GetStaffListForAtt();
            if (myList.Count > 0)
            {
                var startDate = new DateTime(dt.Year, dt.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                int todayDay = endDate.Day;
                for (int i = 0; i < myList.Count; i++)
                {
                    int currentUserId = myList[i].StaffId;
                    List<DailyAttModel> dailyAtt = new List<DailyAttModel>();
                    for (int j = 1; j <= todayDay; j++)
                    {
                        DateTime cDay = new DateTime(dt.Year, dt.Month, j);
                        string aStatus = uRepo.GetPresentDayAttendance(currentUserId, cDay);
                        dailyAtt.Add(new DailyAttModel
                        {
                            Day = j,
                            Status = aStatus
                        });
                    }
                    myList[i].attList = dailyAtt;
                }
                if (!string.IsNullOrEmpty(employeecode))
                {
                    myList = myList.Where(a => a.EmployeeCode == employeecode).ToList();
                }
            }

            return myList;
        }

        public UserMaster GetUserByKey(string key)
        {
            return uRepo.GetUserByKey(key);
        }
        public List<StaffDrop> GetStaffForSalary()
        {
            return uRepo.GetStaffForSalary();
        }
        public DateTime GetLastLoginDetails(int id)
        {
            return uRepo.GetLastLoginTime(id);
        }
        #region  Target Master
        public ProcessResponse SaveTargetMaster(TargetMaster request)
        {
            ProcessResponse pr = new ProcessResponse();
            TargetMaster tm = uRepo.GetTargetOfUserByMonth((int)request.UserId, (int)request.YearNumber, (int)request.MonthNumber);
            if (tm == null)
            {
                pr= uRepo.SaveTarget(request);
            }
            else
            {
                pr.statusCode = 0;
                pr.statusMessage = "Already Exists";
            }
            return pr;
        }
        public List<TargetMaster> getTargetsOfUser(int id)
        {
            return uRepo.GetAllTargetsOfEmployee(id);
        }
        public List<TargetMaster> GetTargetsOfHead(int id)
        {
            List<TargetMaster> tml = new List<TargetMaster>();

            List<EmployeeTargetList> el = uRepo.GetSalesEmployeeForTargets(id);
            List<MonthAndYear> myl = new List<MonthAndYear>();
            MonthAndYear sample = new MonthAndYear();
            sample.Month = 0;sample.year = 0;
            myl.Add(sample);
            foreach(EmployeeTargetList e in el)
            {
                List<TargetMaster> etl = uRepo.GetAllTargetsOfEmployee(e.UserId);
                foreach(TargetMaster t in etl)
                {
                    MonthAndYear m = new MonthAndYear();
                    bool ad = true;
                    foreach (MonthAndYear my in myl)
                    {                        
                        if (my.Month == t.MonthNumber && my.year == t.YearNumber)
                        {
                            ad = false;
                        }
                    }
                    if (ad)
                    {
                        m.Month = (int)t.MonthNumber;
                        m.year = (int)t.YearNumber;
                        myl.Add(m);
                    }
                }
            }
            foreach(MonthAndYear m in myl)
            {
                if (m.Month != 0 && m.year != 0)
                {
                    TargetMaster tm = new TargetMaster();
                    tm.MonthNumber = m.Month;
                    tm.YearNumber = m.year;
                    tm.TargetGiven = 0;
                    tm.TargetDone = 0;
                    List<TargetMaster> monthTargets = uRepo.GetAllTargetOfheadByMonth(id, m.year, m.Month);
                    foreach (TargetMaster t in monthTargets)
                    {
                        tm.TargetGiven += t.TargetGiven;
                        tm.TargetDone += t.TargetDone;
                    }
                    tml.Add(tm);
                }
            }
            return tml;
        }

        public void UpdateActivityLog(ActivityLog request)
        {
            uRepo.UpdateActivityLog(request);
        }

        public ActivityLog GetActivtyLogById(int id)
        {
            return uRepo.GetActivtyLogById(id);
        }

        public List<ActivityLog> GetMyActivityLogs(int userid)
        {
            return uRepo.GetMyActivityLogs(userid);
        }

        public List<ActivityLog> GetMyActivityLogsUnread(int userid)
        {
            return uRepo.GetMyActivityLogsUnread(userid);
        }

        public int GetMyActivityLogsUnreadCount(int userid)
        {
            return uRepo.GetMyActivityLogsUnreadCount(userid);
        }

        public int GetMyActivityLogsCount(int userid)
        {
            return uRepo.GetMyActivityLogsCount(userid);
        }

        public int GetUserTypeCode(int userId)
        {
            return uRepo.GetUserTypeCode(userId);
        }

        public void UpdateStaffOfferUrl(string url, int sId)
        {
            uRepo.UpdateStaffOfferUrl(url, sId);
        }

        public AppointmentPDFModel GetDataForAppointmentOrder(int sId)
        {
            return uRepo.GetDataForAppointmentOrder(sId);
        }
        public int GetAdminId()
        {
            return uRepo.GetAdminId();
        }

        public void LogError(Exception ex)
        {
            uRepo.LogError(ex);
        }
        #endregion
    }
}
