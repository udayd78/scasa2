using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Repositories.Repos
{
    public interface IUserMgmtRepo
    {
        void UpdateStaffOfferUrl(string url, int sId);
        AppointmentPDFModel GetDataForAppointmentOrder(int sId);
        int GetUserTypeCode(int userId);
        void UpdateActivityLog(ActivityLog request);
        ActivityLog GetActivtyLogById(int id);

        List<ActivityLog> GetMyActivityLogs(int userid);
        List<ActivityLog> GetMyActivityLogsUnread(int userid);
        int GetMyActivityLogsUnreadCount(int userid);
        int GetMyActivityLogsCount(int userid);
        UserMaster GetUserByKey(string key);
        UserMasterDisplay GetUserDisplayById(int id);
        public UserProfileModel GetUserProfile(int id);
        string GetPresentDayAttendance(int userid, DateTime date);
        List<StaffAttDisplayModel> GetStaffListForAtt();
        ProcessResponse SaveUserType(UserTypeMaster request);
        List<UserTypeMaster> GetAllUserTypes();
        UserTypeMaster GetAllUserTypeById(int id);
        ProcessResponse UpdateUserType(UserTypeMaster request);
        bool EmailAvailablityCheck(string emailId);
        bool MobileAvailablityCheck(string mobileNumber);
        UserTypeMaster GetUserTypeById(int id);
        ProcessResponse RegisterUser(UserMaster userMaster);
        ProcessResponse SaveUser(UserMaster request);
        UserMaster GetUserById(int userId);
        List<SalesExecutiveListModel> GetUsersByType(string type, int logID, string sear = "");
        void UpdatePassword(UserMaster request);
        UserMaster GetUserByEmail(string email, int userId = 0);
        List<UserMaster> GetUsersByName(string name);
        UserMaster GetUserByCode(string code);
        LoginResponse LoginCheck(LoginRequest request);
        OtpTransactions GetOTPInformation(int userId);
        ProcessResponse SaveOtpInformation(OtpTransactions request);
        ProcessResponse UpdateOtpInformation(OtpTransactions request);
        List<UserMasterDisplay> GetAllUsers(string type = "");
        List<NewStaffListDisplayModel> GetNewUser(string type = "");
        List<EmployeeTargetList> GetSalesEmployeeForTargets(int id = 0, string se = "");
        int getUser_Count(String type = "", int pageNumber = 1, int pageSize = 10);
        ProcessResponse InitiateResetPassword(string emailId);
        ProcessResponse CompletePasswordRequest(string emailOtp, int userid, string pword);
        ProcessResponse UpdatePasswordReset(PasswordChangeRequest request);
        List<StaffDrop> GetStaffForSalary();
        public DateTime GetLastLoginTime(int id);
        ProcessResponse SaveTarget(TargetMaster request);
        List<TargetMaster> GetAllTargetsOfEmployee(int id);
        TargetMaster GetTargetOfUserByMonth(int uId, int y, int m);
        List<TargetMaster> GetAllTargetOfheadByMonth(int uId, int y, int m);
        void LogError(Exception ex);
        int GetAdminId();
    }
}
