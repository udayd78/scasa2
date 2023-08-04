using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public interface INotificationService
    {
        ProcessResponse SendProductDeleteEmail(string moduleName, string productDetails,
            string deletedBy, DateTime deletedon);
        bool PushEmail(string emailtext, string to, string subject, string cc = "", byte[] attachement = null);
        bool SentTextSms(string phoneNumber, string message, string countrycode);
        EmailTemplateEntity GetEmailTemplateByModule(string modulename);
        ProcessResponse SendResetPasswordEmail(string moduleName, string key, string toEamil, string userName,
           int userId);
        ProcessResponse SendRegistrationEmail(string moduleName, string toEamil, string userName, string pword);
        ProcessResponse SendReleivingEmailToAdmin(string moduleName, string toEamil, String adminName, String employeeName, string employeeDesignation, DateTime releivingDate);
        ProcessResponse SendReleivingEmailToEmployee(string moduleName, string toEamil, String name, DateTime releivingDate);
        ProcessResponse SendReviewEmpNotificationToAdmin(string moduleName, string toEmail, string adminName, string employeeName, string designation);
        ProcessResponse SendApproveRequestForCeo(string moduleName, string toEmail, string ceoName, string employeeName, string designation);
        ProcessResponse SendAcceptanceToEmployee(string moduleName, string toEmail, string employeeName, string designation,
            DateTime DOJ, string reportingHead, string key, byte[] attachment = null);
        ProcessResponse SendAcceptEmpNotificationToAdmin(string moduleName, string toEmail, string adminName, string employeeName, string designation, DateTime DOJ);
        ProcessResponse SendJoiningNotificationToReportingHead(string moduleName, string toEmail, string reportingHead, string employeeName, string designation, DateTime dOj);
        ProcessResponse SendPasswordToEmployee(string moduleName, string toEmail, string employeeName, string password);
        ProcessResponse ResendJoiningAcceptanceToEmployee(string moduleName, string toEmail, string empName, string desig, DateTime dOJ, string rHead, string key);
        ProcessResponse ResendCredentialsToEmployee(string moduleName, string toEmail, string employeeName, string userId, string password);
        ProcessResponse SendQuoteMailToCustomer(string moduleName, string toEmail, string CustName, List<CRFQDetails> crfqs);
        ProcessResponse SendOrderMailToCustomer(string moduleName, string toEmail, string CustName, List<SalesOrderDetails> crfqs);
        ProcessResponse SendDailyReport();
        ProcessResponse SendWeaklyReport();
        ProcessResponse SendMonthlyReport();
        void TestJob();
    }
}
