using Microsoft.Extensions.Configuration;
using RestSharp;
using SCASA.Models.ModelClasses;
using SCASA.Models.Repositories.Entity;
using SCASA.Models.Repositories.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SCASA.Models.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MyDbContext context;
        private readonly IConfiguration _config;
        private readonly IReportsMgmgtRepo rRepo;

        public ProcessResponse SendProductDeleteEmail(string moduleName, string productDetails, string deletedBy, DateTime deletedon)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string adminname1 = _config.GetValue<string>("EmailConfig:adminPersonName");
                    string adminemail = _config.GetValue<string>("EmailConfig:adminEmail");
                    string emailText = emailTemplate.EmailContent;
                    string rd = deletedon.ToString("D");
                    emailText = emailText.Replace("##ADMINNAME##", adminname1);
                    emailText = emailText.Replace("##STAFFNAME##", deletedBy);
                    emailText = emailText.Replace("##PRODUCTDETAILS##", productDetails);
                    emailText = emailText.Replace("##DDATE##", rd);
                    bool res = PushEmail(emailText, adminemail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusMessage = ex.Message;
                ps.statusCode = 0;
            }
            return ps;
        }
        public NotificationService(IConfiguration config, MyDbContext context, IReportsMgmgtRepo _rService)
        {
            _config = config;
            this.context = context;
            rRepo = _rService;
        }

        public bool PushEmail(string emailtext, string to, string subject, string cc = "", byte[] attachement = null)
        {
            bool res = false;
            try
            {

                string emailimagesurl = _config.GetValue<string>("EmailConfig:EMAILIMAGEURL");
                string smtpserver = _config.GetValue<string>("EmailConfig:smtpServer");
                string smtpUsername = _config.GetValue<string>("EmailConfig:smtpEmail");
                string smtpPassword = _config.GetValue<string>("EmailConfig:smtppassword");
                int smtpPort = _config.GetValue<int>("EmailConfig:portNumber");

                emailtext = emailtext.Replace("##EMAILIMAGES##", emailimagesurl);
                MailMessage msg = new MailMessage(smtpUsername, to, subject, emailtext);

                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                if (!string.IsNullOrEmpty(cc))
                {
                    List<string> ccMails = cc.Split(",").ToList();
                    foreach(string e in ccMails)
                    {
                        mail.CC.Add(e);
                    }
                }
                
                mail.From = new MailAddress(smtpUsername);
                mail.Subject = subject;
                mail.Body = emailtext;
                mail.IsBodyHtml = true;
                if(attachement != null)
                {
                    using var attachment = new Attachment(new MemoryStream(attachement), "OfferLetter.pdf");
                    mail.Attachments.Add(attachment);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpserver;
                smtp.Port = smtpPort;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);
                    
                    res = true;
                }
                catch (Exception ex)
                {
                    res = false;
                }



            }
            catch (Exception ex)
            {
                // LogException.Record(ex);
                res = false;

            }
            return res;
        }

        public bool SentTextSms(string phoneNumber, string message, string countrycode)
        {
            bool res = false;
            try
            {
                string baseurl = _config.GetValue<string>("SMSConfig:smsBaseUrl");
                string authkey = _config.GetValue<string>("SMSConfig:smsAuthKey");
                string senderId = _config.GetValue<string>("SMSConfig:smsSenderId");

                string url = string.Empty;
                if (countrycode != "91")
                {
                    url = baseurl + "auth=" + authkey + "&msisdn=" + phoneNumber + "&countrycode=" + countrycode + "&senderid=" + senderId + "&message=" + message;
                }
                else
                {
                    url = baseurl + "auth=" + authkey + "&msisdn=" + phoneNumber + "&senderid=" + senderId + "&message=" + message;
                }

                //WebClient client = new WebClient();
                //Stream data = client.OpenRead(baseurl);
                //StreamReader reader = new StreamReader(data);
                //string s = reader.ReadToEnd();
                //data.Close();
                //reader.Close();

                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                res = true;
            }
            catch (Exception ex)
            {
                // LogException.Record(ex);
                res = false;
            }

            return res;
        }

        public EmailTemplateEntity GetEmailTemplateByModule(string modulename)
        {
            EmailTemplateEntity response = new EmailTemplateEntity();
            response = context.emailTemplateEntities.Where(a => a.ModuleName == modulename).FirstOrDefault();
            return response;
        }


        public ProcessResponse SendResetPasswordEmail(string moduleName, string key, string toEamil, string userName,
           int userId)
        {
            ProcessResponse ps = new ProcessResponse();
            EmailTemplateEntity template = new EmailTemplateEntity();
            template = GetEmailTemplateByModule(moduleName);
            if (template != null)
            {
                string HostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");

                string emailText = template.EmailContent;
                emailText = emailText.Replace("##STAFFNAME##", userName);
                emailText = emailText.Replace("##OTP##", " :  " + key);
                bool res = PushEmail(emailText, toEamil, template.Subject, emailCC);
                if (res == false)
                {
                    ps.statusMessage = "failed to send email";
                    ps.statusCode = 0;
                }
                else
                {
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }

            }
            return ps;

        }

        public ProcessResponse SendRegistrationEmail(string moduleName, string toEamil, string userName, string key)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string WebHostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                    key = WebHostURL + "EmployeeRegistration/CompleteForm?key=" + key;
                    string emailText = emailTemplate.EmailContent;
                    emailText = emailText.Replace("##STAFFNAME##", userName);
                    emailText = emailText.Replace("##LINK##", key);
                    bool res = PushEmail(emailText, toEamil, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusMessage = ex.Message;
                ps.statusCode = 0;
            }
            return ps;
        }
       
        public ProcessResponse SendReleivingEmailToAdmin(string moduleName,string toEamil,String adminName,String employeeName,string employeeDesignation,DateTime releivingDate)
        {
            ProcessResponse ps = new ProcessResponse();
            try{ 
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string adminname1 = _config.GetValue<string>("EmailConfig:adminPersonName");
                    string emailText = emailTemplate.EmailContent;
                    string rd = releivingDate.ToString("D");
                    emailText = emailText.Replace("##ADMINNAME##", adminname1);
                    emailText = emailText.Replace("##STAFFNAME##", employeeName);
                    emailText = emailText.Replace("destignation", employeeDesignation);
                    emailText = emailText.Replace("##RDATE##", rd);

                    //emailText = emailText.Replace("##STAFFNAME##", adminName);
                    //emailText = emailText.Replace("##PASSWORD##", employeeName);
                    //emailText = emailText.Replace("##STAFFNAME##", employeeDesignation);
                    //emailText = emailText.Replace("##STAFFNAME##", rd);
                    bool res = PushEmail(emailText, toEamil, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {

                ps.statusMessage = ex.Message;
                ps.statusCode = 0;
            }

            return ps;
        }
        public ProcessResponse SendReleivingEmailToEmployee(string moduleName, string toEamil, String name, DateTime releivingDate)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    string rd = releivingDate.ToString("D");
                    emailText = emailText.Replace("##STAFFNAME##", name);
                    emailText = emailText.Replace("##RDATE##", rd);
                    bool res = PushEmail(emailText, toEamil, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusMessage = ex.Message;
                ps.statusCode = 0;
            }
            return ps;
        }
        public ProcessResponse SendReviewEmpNotificationToAdmin(string moduleName,string toEmail,string adminName,string employeeName,string designation)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    emailText = emailText.Replace("##ADMIN##", adminName);
                    emailText = emailText.Replace("##UserName##", employeeName);
                    emailText = emailText.Replace("##Designation##", designation);
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
                }catch(Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendApproveRequestForCeo(string moduleName, string toEmail, string ceoName, string employeeName,
            string designation)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    emailText = emailText.Replace("##CEO##", ceoName);
                    emailText = emailText.Replace("##UserName##", employeeName);
                    emailText = emailText.Replace("##Designation##", designation);
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendAcceptanceToEmployee(string moduleName, string toEmail, string employeeName,string designation, 
            DateTime DOJ,string reportingHead, string key, byte[] attachment = null )
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string WebHostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                    key = WebHostURL + "EmployeeAcceptance/ThankyouFOrAcceptance?key=" + key;
                    string emailText = emailTemplate.EmailContent;
                    string rd = DOJ.ToString("D");
                    emailText = emailText.Replace("##UserName##", employeeName);
                    emailText = emailText.Replace("##Designation##", designation);
                    emailText = emailText.Replace("##DATE##", rd);
                    emailText = emailText.Replace("##ReportName##", reportingHead);
                    emailText = emailText.Replace("##LINK##", key);
                    
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC,attachment);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendAcceptEmpNotificationToAdmin(string moduleName, string toEmail,string adminName, string employeeName,string designation, DateTime DOJ)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    string rd = DOJ.ToString("D");
                    emailText = emailText.Replace("##ADMIN##", adminName);
                    emailText = emailText.Replace("##UserName##", employeeName);
                    emailText = emailText.Replace("##Designation##", designation);
                    emailText = emailText.Replace("##DATE##", rd);
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendJoiningNotificationToReportingHead(string moduleName, string toEmail,string reportingHead,string employeeName, string designation,DateTime dOj)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    string rd = dOj.ToString("D");
                    emailText = emailText.Replace("##ReportingHead##", reportingHead);
                    emailText = emailText.Replace("##userName##", employeeName);
                    emailText = emailText.Replace("##Designation##", designation);
                    emailText = emailText.Replace("##DATE##", rd);
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendPasswordToEmployee(string moduleName, string toEmail, string employeeName,string password)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    //string rd = dOj.ToString("D");
                    //emailText = emailText.Replace("##ADMIN##", reportingHead);
                    emailText = emailText.Replace("##STAFFNAME##", employeeName);
                    emailText = emailText.Replace("##PASSWORD##", password);
                    //emailText = emailText.Replace("##Designation##", designation);
                    //emailText = emailText.Replace("##DATE##", rd);
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse ResendJoiningAcceptanceToEmployee(string moduleName, string toEmail,string empName,string desig,DateTime dOJ,string rHead,string key)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string WebHostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                    key = WebHostURL + "EmployeeAcceptance/ThankyouFOrAcceptance?key=" + key;
                    string emailText = emailTemplate.EmailContent;
                    string rd = dOJ.ToString("D");
                    emailText = emailText.Replace("##UserName##", empName);
                    emailText = emailText.Replace("##Designation##", desig);
                    emailText = emailText.Replace("##DATE##", rd);
                    emailText = emailText.Replace("##ReportName##", rHead);
                    emailText = emailText.Replace("##LINK##", key);
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse ResendCredentialsToEmployee(string moduleName, string toEmail, string employeeName,string userId, string password)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;                    
                    emailText = emailText.Replace("##UserName##", employeeName);
                    emailText = emailText.Replace("##Password##", password);
                    emailText = emailText.Replace("##UserId##", userId);                    
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendQuoteMailToCustomer(string moduleName,string toEmail,string CustName,List<CRFQDetails> crfqs)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:QuoteEmailCC");
                    string producturl = _config.GetValue<string>("OtherConfig:ProductImatesURL");
                    string emailText = emailTemplate.EmailContent;
                    string quote = "";
                    int i = 1;
                    decimal tot = 0;
                    System.Globalization.CultureInfo Indian = new System.Globalization.CultureInfo("hi-IN");
                    foreach (CRFQDetails m in crfqs)
                    {
                        int dis = (int)(m.DisAmtByHead + m.DisAmtBySE);
                        //string img=""
                        quote += "<tr><td>"+i+"</td><td><img src ='"+ producturl +  m.InventoryImage+ "' height ='70' width ='100'></td><td>"+m.InventoryTitle+"</td><td>"+ String.Format(Indian, "{0:N}", m.ItemPrise) + "</td><td>"+m.Quantity+"</td><td>"+ String.Format(Indian, "{0:N}", m.OrderLineTotal) + "</td><td>"+dis+"</td><td>"+ String.Format(Indian, "{0:N}", m.TotalPrice) + "</td></tr>";
                        tot += (decimal)m.TotalPrice;
                        i++;
                    }
                    string t = tot.ToString();

                    emailText = emailText.Replace("##UserName##", CustName);
                    emailText = emailText.Replace("##Quote##", quote);
                    emailText = emailText.Replace("##TotalAmount##", t);
                    
                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendOrderMailToCustomer(string moduleName, string toEmail, string CustName, List<SalesOrderDetails> crfqs)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    string quote = "";
                    int i = 1;
                    decimal tot = 0;
                    System.Globalization.CultureInfo Indian = new System.Globalization.CultureInfo("hi-IN");
                    foreach (SalesOrderDetails m in crfqs)
                    {
                        int dis = (int)(m.DisAmtByHead + m.DisAmtBySE);
                        //string img=""
                        quote += "<tr><td>" + i + "</td><td><img src ='" + m.InventoryImage + "' height ='70' width ='100'></td><td>" + m.InventoryTitle + "</td><td>" + String.Format(Indian, "{0:N}", m.ItemPrice) + "</td><td>" + m.Quantity + "</td><td>" + String.Format(Indian, "{0:N}", m.OrderLineTotal) + "</td><td>" + dis + "</td><td>" + String.Format(Indian, "{0:N}", m.TotalPrice) + "</td></tr>";
                        tot += (decimal)m.TotalPrice;
                        i++;
                    }
                    string t = String.Format(Indian, "{0:N}", tot);

                    emailText = emailText.Replace("##UserName##", CustName);
                    emailText = emailText.Replace("##Quote##", quote);
                    emailText = emailText.Replace("##TotalAmount##", t);

                    bool res = PushEmail(emailText, toEmail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendDailyReport()
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule("SendDailyRports");
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    String ToMail1 = _config.GetValue<string>("OtherConfig:MDMail");
                    String ToMail2 = _config.GetValue<string>("OtherConfig:CEOEmail");
                    string emailText = emailTemplate.EmailContent;
                    string DRep = "";
                    int i = 1;
                    decimal tot = 0;
                    System.Globalization.CultureInfo Indian = new System.Globalization.CultureInfo("hi-IN");
                    List<ReportsDataModel> daily = rRepo.GetDailyReports();
                    DashBoardValuesList listvalues = rRepo.GetDailyDynamicValues();
                    decimal daiSaleTot = 0;
                    int daiAtt = 0;
                    int daiClo = 0;
                    decimal tarTotal = 0;
                    decimal achveTotal = 0;
                    decimal achivePer = 0;
                    foreach (ReportsDataModel m in daily)
                    {
                        //int dis = (int)(m.DisAmtByHead + m.DisAmtBySE);
                        ////string img=""
                        //quote += "<tr><td>" + i + "</td><td><img src ='" + m.InventoryImage + "' height ='70' width ='100'></td><td>" + m.InventoryTitle + "</td><td>" + String.Format(Indian, "{0:N}", m.ItemPrice) + "</td><td>" + m.Quantity + "</td><td>" + String.Format(Indian, "{0:N}", m.OrderLineTotal) + "</td><td>" + dis + "</td><td>" + String.Format(Indian, "{0:N}", m.TotalPrice) + "</td></tr>";
                        //tot += (decimal)m.TotalPrice;
                        decimal cldper = 0;
                        if (m.attendCnt != 0)
                        {
                            cldper = Math.Round((((decimal)m.closedCount / (decimal)m.attendCnt) * 100), 2);
                        }
                        decimal targPer = 0;
                        if (m.Monthtarget != 0)
                        {
                            targPer = Math.Round((((decimal)m.AchievedTarget / (decimal)m.Monthtarget) * 100), 2);
                        }
                        daiSaleTot += m.salesValue;
                        daiAtt += m.attendCnt;
                        daiClo += m.closedCount;
                        tarTotal += m.Monthtarget;
                        achveTotal += m.AchievedTarget;
                        if (i % 2 == 1)
                        {
                            DRep += "<tr bgcolor='#ddd' align='right'><td style=' padding: 5px; text-align: left;'>" + m.sEName + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.salesValue) + "</td><td style='padding: 5px;'>" + m.attendCnt + "</td><td style=' padding: 5px;'>" + m.closedCount + "</td><td style=' padding: 5px;'>" + cldper + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.Monthtarget) + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.AchievedTarget) + "</td><td style=' padding: 5px;'>" + targPer + "%</td></tr>";
                        }
                        else
                        {
                            DRep += "<tr align='right'><td style=' padding: 5px; text-align: left;'>" + m.sEName + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.salesValue) + "</td><td style='padding: 5px;'>" + m.attendCnt + "</td><td style=' padding: 5px;'>" + m.closedCount + "</td><td style=' padding: 5px;'>" + cldper + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.Monthtarget) + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.AchievedTarget) + "</td><td style=' padding: 5px;'>" + targPer + "%</td></tr>";
                        }

                        i++;
                    }
                    if (tarTotal != 0)
                    {
                        achivePer = Math.Round((((decimal)achveTotal / (decimal)tarTotal) * 100), 2);
                    }
                    decimal daiCloPercent = 0;
                    if (daiAtt != 0)
                    {
                        daiCloPercent = Math.Round((((decimal)daiClo / (decimal)daiAtt) * 100), 2);
                    }

                    string dayDate = "";
                    DateTime curDate = DateTime.Now;
                    dayDate += curDate.DayOfWeek;
                    string d = curDate.ToString("dd/MMMM/yyyy");
                    dayDate += "  " + d;
                    emailText = emailText.Replace("##SaleOrderToday##", String.Format(Indian, "{0:N}", listvalues.dailyReceipt));
                    emailText = emailText.Replace("##CollectionToday##", String.Format(Indian, "{0:N}", listvalues.collectionTotal));
                    emailText = emailText.Replace("##DeliveredToday##", String.Format(Indian, "{0:N}", listvalues.delivered));
                    emailText = emailText.Replace("##SpendedToday##", String.Format(Indian, "{0:N}", listvalues.expendeture));
                    emailText = emailText.Replace("##DayWithDate##", dayDate);
                    emailText = emailText.Replace("##MainTableData##", DRep);
                    emailText = emailText.Replace("##TableTotalSales##", String.Format(Indian, "{0:N}", daiSaleTot));
                    emailText = emailText.Replace("##TableTotalAttended##", String.Format(Indian, "{0:N}", daiAtt));
                    emailText = emailText.Replace("##TableTotalClosed##", String.Format(Indian, "{0:N}", daiClo));
                    emailText = emailText.Replace("##TableTotalClosedPercent##", String.Format(Indian, "{0:N}", daiCloPercent));
                    emailText = emailText.Replace("##TableTotalMonthlyTarget##", String.Format(Indian, "{0:N}", tarTotal));
                    emailText = emailText.Replace("##TableTotalMonthlyAchieved##", String.Format(Indian, "{0:N}", achveTotal));
                    emailText = emailText.Replace("##TableTotalMonthlyAchievedPercent##", String.Format(Indian, "{0:N}", achivePer));
                    emailText = emailText.Replace("##DonwTableTotalReceipts##", String.Format(Indian, "{0:N}", listvalues.dailyReceipt));
                    emailText = emailText.Replace("##DonwTableTotalWalkins##", String.Format(Indian, "{0:N}", daiAtt));
                    emailText = emailText.Replace("##DonwTableTotalConversions##", String.Format(Indian, "{0:N}", (daiClo)));
                    emailCC += "," + ToMail2;
                    bool res = PushEmail(emailText, ToMail1, emailTemplate.Subject, emailCC);
                    //bool res1 = PushEmail(emailText, ToMail2, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendWeaklyReport()
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule("SendWeeklyReports");
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    String ToMail1 = _config.GetValue<string>("OtherConfig:MDMail");
                    String ToMail2 = _config.GetValue<string>("OtherConfig:CEOEmail");
                    string emailText = emailTemplate.EmailContent;
                    string DRep = "";
                    int i = 1;
                    decimal tot = 0;
                    System.Globalization.CultureInfo Indian = new System.Globalization.CultureInfo("hi-IN");
                    List<ReportsDataModel> daily = rRepo.GetWeeklyReports();
                    DashBoardValuesList listvalues = rRepo.GetWeaklyDynamicValues();
                    decimal daiSaleTot = 0;
                    int daiAtt = 0;
                    int daiClo = 0;
                    decimal tarTotal = 0;
                    decimal achveTotal = 0;
                    decimal achivePer = 0;
                    foreach (ReportsDataModel m in daily)
                    {
                        //int dis = (int)(m.DisAmtByHead + m.DisAmtBySE);
                        ////string img=""
                        //quote += "<tr><td>" + i + "</td><td><img src ='" + m.InventoryImage + "' height ='70' width ='100'></td><td>" + m.InventoryTitle + "</td><td>" + String.Format(Indian, "{0:N}", m.ItemPrice) + "</td><td>" + m.Quantity + "</td><td>" + String.Format(Indian, "{0:N}", m.OrderLineTotal) + "</td><td>" + dis + "</td><td>" + String.Format(Indian, "{0:N}", m.TotalPrice) + "</td></tr>";
                        //tot += (decimal)m.TotalPrice;
                        decimal cldper = 0;
                        if (m.attendCnt != 0)
                        {
                            cldper = Math.Round((((decimal)m.closedCount / (decimal)m.attendCnt) * 100), 2);
                        }
                        decimal targPer = 0;
                        if (m.Monthtarget != 0)
                        {
                            targPer = Math.Round((((decimal)m.AchievedTarget / (decimal)m.Monthtarget) * 100), 2);
                        }
                        daiSaleTot += m.salesValue;
                        daiAtt += m.attendCnt;
                        daiClo += m.closedCount;
                        tarTotal += m.Monthtarget;
                        achveTotal += m.AchievedTarget;
                        if (i % 2 == 1)
                        {
                            DRep += "<tr bgcolor='#ddd' align='right'><td style=' padding: 5px; text-align: left;'>" + m.sEName + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.salesValue) + "</td><td style='padding: 5px;'>" + m.attendCnt + "</td><td style=' padding: 5px;'>" + m.closedCount + "</td><td style=' padding: 5px;'>" + cldper + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.Monthtarget) + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.AchievedTarget) + "</td><td style=' padding: 5px;'>" + targPer + "%</td></tr>";
                        }
                        else
                        {
                            DRep += "<tr align='right'><td style=' padding: 5px; text-align: left;'>" + m.sEName + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.salesValue) + "</td><td style='padding: 5px;'>" + m.attendCnt + "</td><td style=' padding: 5px;'>" + m.closedCount + "</td><td style=' padding: 5px;'>" + cldper + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.Monthtarget) + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.AchievedTarget) + "</td><td style=' padding: 5px;'>" + targPer + "%</td></tr>";
                        }

                        i++;
                    }
                    if (tarTotal != 0)
                    {
                        achivePer = Math.Round((((decimal)achveTotal / (decimal)tarTotal) * 100), 2);
                    }
                    decimal daiCloPercent = 0;
                    if (daiAtt != 0)
                    {
                        daiCloPercent = Math.Round((((decimal)daiClo / (decimal)daiAtt) * 100), 2);
                    }

                    string dayDate = "";
                    DateTime curDate = DateTime.Now;
                    //DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                    //DateTime sunday = Convert.ToDateTime(monday).AddDays(7);
                    DateTime sDate = new DateTime(curDate.Year, curDate.Month, 1);
                    DateTime eDate = sDate.AddDays(1).AddSeconds(-1);
                    if (curDate.DayOfWeek == DayOfWeek.Monday)
                    {
                        sDate = new DateTime(curDate.Year, curDate.Month, curDate.Day);
                        eDate = sDate.AddDays(7);
                    }
                    else if (curDate.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        int sd = curDate.Day - 1;
                        sDate = new DateTime(curDate.Year, curDate.Month, sd);
                        eDate = sDate.AddDays(7);
                    }
                    else if (curDate.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        int sd = curDate.Day - 2;
                        sDate = new DateTime(curDate.Year, curDate.Month, sd);
                        eDate = sDate.AddDays(7);
                    }
                    else if (curDate.DayOfWeek == DayOfWeek.Thursday)
                    {
                        int sd = curDate.Day - 3;
                        sDate = new DateTime(curDate.Year, curDate.Month, sd);
                        eDate = sDate.AddDays(7);
                    }
                    else if (curDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        int sd = curDate.Day - 4;
                        sDate = new DateTime(curDate.Year, curDate.Month, sd);
                        eDate = sDate.AddDays(7);
                    }
                    else if (curDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        int sd = curDate.Day - 5;
                        sDate = new DateTime(curDate.Year, curDate.Month, sd);
                        eDate = sDate.AddDays(7);
                    }
                    else if (curDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        int sd = curDate.Day - 6;
                        sDate = new DateTime(curDate.Year, curDate.Month, sd);
                        eDate = sDate.AddDays(7);
                    }
                    dayDate += sDate.ToString("dd/MMMM/yyyy") + " , " + eDate.ToString("dd/MMMM/yyyy");
                    //string d = curDate.ToString("dd/MMMM/yyyy");
                    //dayDate += "  " + d;
                    emailText = emailText.Replace("##SaleOrderToday##", String.Format(Indian, "{0:N}", listvalues.dailyReceipt));
                    emailText = emailText.Replace("##CollectionToday##", String.Format(Indian, "{0:N}", listvalues.collectionTotal));
                    emailText = emailText.Replace("##DeliveredToday##", String.Format(Indian, "{0:N}", listvalues.delivered));
                    emailText = emailText.Replace("##SpendedToday##", String.Format(Indian, "{0:N}", listvalues.expendeture));
                    emailText = emailText.Replace("##DayWithDate##", dayDate);
                    emailText = emailText.Replace("##MainTableData##", DRep);
                    emailText = emailText.Replace("##TableTotalSales##", String.Format(Indian, "{0:N}", daiSaleTot));
                    emailText = emailText.Replace("##TableTotalAttended##", String.Format(Indian, "{0:N}", daiAtt));
                    emailText = emailText.Replace("##TableTotalClosed##", String.Format(Indian, "{0:N}", daiClo));
                    emailText = emailText.Replace("##TableTotalClosedPercent##", String.Format(Indian, "{0:N}", daiCloPercent));
                    emailText = emailText.Replace("##TableTotalMonthlyTarget##", String.Format(Indian, "{0:N}", tarTotal));
                    emailText = emailText.Replace("##TableTotalMonthlyAchieved##", String.Format(Indian, "{0:N}", achveTotal));
                    emailText = emailText.Replace("##TableTotalMonthlyAchievedPercent##", String.Format(Indian, "{0:N}", achivePer));
                    emailText = emailText.Replace("##DonwTableTotalReceipts##", String.Format(Indian, "{0:N}", listvalues.dailyReceipt));
                    emailText = emailText.Replace("##DonwTableTotalWalkins##", String.Format(Indian, "{0:N}", daiAtt));
                    emailText = emailText.Replace("##DonwTableTotalConversions##", String.Format(Indian, "{0:N}", (daiClo)));
                    emailCC += "," + ToMail2;
                    bool res = PushEmail(emailText, ToMail1, emailTemplate.Subject, emailCC);
                    //bool res1 = PushEmail(emailText, ToMail2, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public ProcessResponse SendMonthlyReport()
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplateEntity emailTemplate = new EmailTemplateEntity();
                emailTemplate = GetEmailTemplateByModule("SendDailyRports");
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    String ToMail1 = _config.GetValue<string>("OtherConfig:EmailCC");
                    String ToMail2 = _config.GetValue<string>("OtherConfig:EmailCC");
                    string emailText = emailTemplate.EmailContent;
                    string DRep = "";
                    int i = 1;
                    decimal tot = 0;
                    System.Globalization.CultureInfo Indian = new System.Globalization.CultureInfo("hi-IN");
                    List<ReportsDataModel> daily = rRepo.GetDailyReports();
                    DashBoardValuesList listvalues = rRepo.GetDailyDynamicValues();
                    decimal daiSaleTot = 0;
                    int daiAtt = 0;
                    int daiClo = 0;
                    decimal tarTotal = 0;
                    decimal achveTotal = 0;
                    decimal achivePer = 0;
                    foreach (ReportsDataModel m in daily)
                    {
                        //int dis = (int)(m.DisAmtByHead + m.DisAmtBySE);
                        ////string img=""
                        //quote += "<tr><td>" + i + "</td><td><img src ='" + m.InventoryImage + "' height ='70' width ='100'></td><td>" + m.InventoryTitle + "</td><td>" + String.Format(Indian, "{0:N}", m.ItemPrice) + "</td><td>" + m.Quantity + "</td><td>" + String.Format(Indian, "{0:N}", m.OrderLineTotal) + "</td><td>" + dis + "</td><td>" + String.Format(Indian, "{0:N}", m.TotalPrice) + "</td></tr>";
                        //tot += (decimal)m.TotalPrice;
                        decimal cldper = 0;
                        if (m.attendCnt != 0)
                        {
                            cldper = Math.Round((((decimal)m.closedCount / (decimal)m.attendCnt) * 100), 2);
                        }
                        decimal targPer = 0;
                        if (m.Monthtarget != 0)
                        {
                            targPer = Math.Round((((decimal)m.AchievedTarget / (decimal)m.Monthtarget) * 100), 2);
                        }
                        daiSaleTot += m.salesValue;
                        daiAtt += m.attendCnt;
                        daiClo += m.closedCount;
                        tarTotal += m.Monthtarget;
                        achveTotal += m.AchievedTarget;
                        if (i % 2 == 1)
                        {
                            DRep += "<tr bgcolor='#ddd' align='right'><td style=' padding: 5px; text-align: left;'>" + m.sEName + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.salesValue) + "</td><td style='padding: 5px;'>" + m.attendCnt + "</td><td style=' padding: 5px;'>" + m.closedCount + "</td><td style=' padding: 5px;'>" + cldper + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.Monthtarget) + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.AchievedTarget) + "</td><td style=' padding: 5px;'>" + targPer + "%</td></tr>";
                        }
                        else
                        {
                            DRep += "<tr align='right'><td style=' padding: 5px; text-align: left;'>" + m.sEName + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.salesValue) + "</td><td style='padding: 5px;'>" + m.attendCnt + "</td><td style=' padding: 5px;'>" + m.closedCount + "</td><td style=' padding: 5px;'>" + cldper + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.Monthtarget) + "</td><td style=' padding: 5px;'>" + String.Format(Indian, "{0:N}", m.AchievedTarget) + "</td><td style=' padding: 5px;'>" + targPer + "%</td></tr>";
                        }

                        i++;
                    }
                    if (tarTotal != 0)
                    {
                        achivePer = Math.Round((((decimal)achveTotal / (decimal)tarTotal) * 100), 2);
                    }
                    decimal daiCloPercent = 0;
                    if (daiAtt != 0)
                    {
                        daiCloPercent = Math.Round((((decimal)daiClo / (decimal)daiAtt) * 100), 2);
                    }

                    string dayDate = "";
                    DateTime curDate = DateTime.Now;
                    dayDate += curDate.Month + " , " + curDate.Year;
                    emailText = emailText.Replace("##SaleOrderToday##", String.Format(Indian, "{0:N}", listvalues.SalesTotal));
                    emailText = emailText.Replace("##CollectionToday##", String.Format(Indian, "{0:N}", listvalues.collectionTotal));
                    emailText = emailText.Replace("##DeliveredToday##", String.Format(Indian, "{0:N}", listvalues.delivered));
                    emailText = emailText.Replace("##SpendedToday##", String.Format(Indian, "{0:N}", listvalues.expendeture));
                    emailText = emailText.Replace("##DayWithDate##", dayDate);
                    emailText = emailText.Replace("##MainTableData##", DRep);
                    emailText = emailText.Replace("##TableTotalSales##", String.Format(Indian, "{0:N}", daiSaleTot));
                    emailText = emailText.Replace("##TableTotalAttended##", String.Format(Indian, "{0:N}", daiAtt));
                    emailText = emailText.Replace("##TableTotalClosed##", String.Format(Indian, "{0:N}", daiClo));
                    emailText = emailText.Replace("##TableTotalClosedPercent##", String.Format(Indian, "{0:N}", daiCloPercent));
                    emailText = emailText.Replace("##TableTotalMonthlyTarget##", String.Format(Indian, "{0:N}", tarTotal));
                    emailText = emailText.Replace("##TableTotalMonthlyAchieved##", String.Format(Indian, "{0:N}", achveTotal));
                    emailText = emailText.Replace("##TableTotalMonthlyAchievedPercent##", String.Format(Indian, "{0:N}", achivePer));
                    emailText = emailText.Replace("##DonwTableTotalReceipts##", String.Format(Indian, "{0:N}", listvalues.dailyReceipt));
                    emailText = emailText.Replace("##DonwTableTotalWalkins##", String.Format(Indian, "{0:N}", daiAtt));
                    emailText = emailText.Replace("##DonwTableTotalConversions##", String.Format(Indian, "{0:N}", (daiAtt - daiClo)));

                    bool res = PushEmail(emailText, ToMail1, emailTemplate.Subject, emailCC);
                    bool res1 = PushEmail(emailText, ToMail2, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = ex.Message;
            }
            return ps;
        }
        public void TestJob()
        {
            int x = 0;
        }
    }
}