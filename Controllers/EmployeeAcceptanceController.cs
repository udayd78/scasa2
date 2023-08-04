using SCASA.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Controllers
{
    public class EmployeeAcceptanceController : Controller
    {
        private readonly IUserMgmtService uService;
        private readonly IConfiguration _config;
        private readonly INotificationService nService;
        private readonly IFinanceMgmtService fService;
        public EmployeeAcceptanceController (IUserMgmtService _uService, INotificationService _nService, IConfiguration conf,IFinanceMgmtService _fService)
        {
            uService = _uService;
            nService = _nService;
            _config = conf;
            fService = _fService;
        }
        public IActionResult ThankyouFOrAcceptance(string key = "")
        {
            UserMaster um = uService.GetUserByKey(key);
            if (string.IsNullOrEmpty(um.PWord))
            {
                um.CurrentStatus = "Active";
                um.AcceptedOn = DateTime.Now;
                string password = RandomGenerator.RandomString(8, false);
                um.PWord = PasswordEncryption.Encrypt(password);
                ProcessResponse pr = uService.SaveUser(um);
                string adminemail = _config.GetValue<string>("EmailConfig:adminEmail");
                string adminPersonName = _config.GetValue<string>("EmailConfig:adminPersonName");
                if (pr.statusCode == 1)
                {
                    FinanceHeads fh = new FinanceHeads();
                    fh.HeadName = um.UserName;
                    fh.StartingBallance = 0;
                    fh.CurrentBallance = 0;
                    fh.IsDeleted = false;
                    fh.HeadCode = "Staff";
                    fh.StaffId = um.UserId;
                    FinanceGroups fg = fService.GetFinanceGroupByName("Salaries");
                    if (fg == null)
                    {
                        fg = new FinanceGroups();
                        fg.GroupName = "Salaries";
                        fg.GroupCode = "cash";
                        fg.IsDeleted = false;
                        fService.SaveFinGroup(fg);
                        fg= fService.GetFinanceGroupByName("Salaries");
                    }
                    fh.GroupId = fg.GroupId;
                    ProcessResponse fp = fService.SaveFinHead(fh);
                    var eTE = nService.SendPasswordToEmployee(AppSettings.EmailTemplates.PasswordToEmployee, um.EmailId, um.UserName, password);

                    UserTypeMaster utm = uService.GetUserTypeById((int)um.UserTypeId);
                    var eTA = nService.SendAcceptEmpNotificationToAdmin(AppSettings.EmailTemplates.AcceptEmpNotificationToAdmin, adminemail, adminPersonName, um.UserName, utm.TypeName, (DateTime)um.DateOfJoin);

                    UserMaster rh = uService.GetUserById((int)um.ReportingManager);
                    var eTRH = nService.SendJoiningNotificationToReportingHead(AppSettings.EmailTemplates.JoiningNotificationToReportingHead, rh.EmailId, rh.UserName, um.UserName, utm.TypeName, (DateTime)um.DateOfJoin);
                }
            }


            return View();
        }
    }
}
