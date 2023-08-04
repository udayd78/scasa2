using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface IUserMgmtService
    {
        void LogError(Exception ex);
        void UpdateStaffOfferUrl(string url, int sId);
        AppointmentPDFModel GetDataForAppointmentOrder(int sId);
        void UpdateActivityLog(ActivityLog request);
        ActivityLog GetActivtyLogById(int id);

        List<ActivityLog> GetMyActivityLogs(int userid);
        List<ActivityLog> GetMyActivityLogsUnread(int userid);
        int GetMyActivityLogsUnreadCount(int userid);
        int GetMyActivityLogsCount(int userid);
        UserMaster GetUserByKey(string key);
        List<StaffAttDisplayModel> GetStaffForAtt();
        List<StaffAttDisplayModel> GetAttandance(DateTime dt, string employeecode = "");
        ProcessResponse SaveUserType(UserTypeMaster request);
        List<UserTypeMaster> GetAllUserTypes();

        UserTypeMaster GetUserTypeById(int id);

        ProcessResponse UpdateUserType(UserTypeMaster request);

        ProcessResponse SaveUser(UserMaster request);
        bool EmailAvailablityCheck(string emailId);
        bool MobileAvailablityCheck(string mobileNumber);

        ProcessResponse RegisterUser(UserMaster userMaster);
        int GetUserTypeCode(int userId);
        UserMaster GetUserById(int userId);
        List<SalesExecutiveListModel> GetUsersByType(string type, int logID, string sear = "");
        UserMasterDisplay GetUserDisplayModelById(int userId);
        public UserProfileModel GetUserShortProfileById(int userId);
        void UpdatePassword(UserMaster request);
        UserMaster GetUserByEmail(string email, int userid = 0);
        LoginResponse LoginCheck(LoginRequest request);
        List<UserMasterDisplay> GetAllUsers(string type = "");
        List<NewStaffListDisplayModel> GetNewUsers(string type = "");
        List<EmployeeTargetList> GetStaffForTargetList(int id = 0, string s = "");
        int GetUser_Count(string type = "", int pageNumber = 1, int pageSize = 10);

        ProcessResponse InitiateResetPassword(string emailId);
        ProcessResponse CompletePasswordRequest(string key, int userid, string pword);

        ProcessResponse UpdatePasswordReset(PasswordChangeRequest request);
        List<UserMasterDisplay> GetSearchUsers(string uData);
        List<StaffDrop> GetStaffForSalary();
        public DateTime GetLastLoginDetails(int id);

        ProcessResponse SaveTargetMaster(TargetMaster request);
        List<TargetMaster> getTargetsOfUser(int id);
        List<TargetMaster> GetTargetsOfHead(int id);
        int GetAdminId();
    }
}
