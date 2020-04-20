using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRS_Portal.Entity;
using CRS_Portal.HelperMethods;
using CRS_Portal.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRS_Portal.Controllers
{
    public class LoginController : Controller
    {
        UserDbContext objUserDbContext;
        Methods obj = new Methods();
        string _message = string.Empty;

        public IActionResult Index()
        {
            Log.WriteEventLogwithParam("Login", "Index", "Get", "Start", "Login page Get method was called", "System");
            HttpContext.Session.SetString("UserID", "");
            HttpContext.Session.SetString("UserName", "");
            HttpContext.Session.SetString("LastLoginDate", "");
            HttpContext.Session.SetString("LastLoginTime", "");
            HttpContext.Session.SetString("BankName", "");
            HttpContext.Session.SetString("BankFRN", "");
            
            return View();
        }

        public bool SendMailUsingPrimary(string sBankName, string strMailMessage, string strMailSubject, bool Origin)
        {
            bool result = false;
            int iRetryCount = 0;
            for (int iCount = 0; iCount < 3; iCount++)
            {
                iRetryCount = iRetryCount + 1;
                try
                {
                    //To Check Email function
                    EmailParam objEmailParam = new EmailParam();
                    SendEmail sendEmail = new SendEmail();
                    string emailTemplatePath = sendEmail.GetEmailTemplateByBankname(sBankName, SCVMacroSettings.FilePath);
                    string msgbdy = System.IO.File.ReadAllText(emailTemplatePath);

                    objEmailParam.PrimaryEnableSsl = bool.Parse(Helper.ReturnPolmtpf("MAILPrimary", "PrimaryEnableSsl"));
                    objEmailParam.PrimaryUseDefaultCredentials = bool.Parse(Helper.ReturnPolmtpf("MAILPrimary", "PrimaryUseDefaultCredentials"));
                    objEmailParam.PrimaryPortNo = int.Parse(Helper.ReturnPolmtpf("MAILPrimary", "PrimaryPortNo"));
                    objEmailParam.PrimaryPassword = Helper.ReturnPolmtpf("MAILPrimary", "PrimaryPassword");
                    objEmailParam.PrimaryFrom = Helper.ReturnPolmtpf("MAILPrimary", "PrimaryFrom");
                    objEmailParam.PrimaryUserName = Helper.ReturnPolmtpf("MAILPrimary", "PrimaryUserName");
                    objEmailParam.PrimarySMTP = Helper.ReturnPolmtpf("MAILPrimary", "PrimarySMTP");

                    objEmailParam.ToEmailAddress = Helper.ReturnPolmtpf("MAILSend", "MailTo");
                    objEmailParam.CCEmailAddress = Helper.ReturnPolmtpf("MAILSend", "MailCC");
                    objEmailParam.BCCEmailAddress = Helper.ReturnPolmtpf("MAILSend", "MailBCC");
                    objEmailParam.ApplicationName = Helper.ReturnPolmtpf("MAILSend", "ExceptionName"); ;

                    objEmailParam.MailSubject = strMailSubject;
                    string _MailContent = strMailMessage;
                    objEmailParam.MailContent = obj.ReplaceText(msgbdy, "Content message", _MailContent);
                    objEmailParam.AloneMailContent = _MailContent;
                    sendEmail.SendEmailUsingPrimary(objEmailParam);

                    // Email successfully sent. kill for loop
                    iRetryCount = 4;
                    iCount = 4;
                    result = true;
                    return result;
                }
                catch (Exception exRetry)
                {
                    if (iRetryCount == 3)
                    {
                        if (Origin)
                        {
                           result= SendMailUsingSecondary(sBankName, strMailMessage, strMailSubject,false);
                           return result;
                        }
                        throw exRetry;
                    }
                    // Sleep for 5 min if any network or internet change
                    System.Threading.Thread.Sleep(300000);
                }
            }
            return result;
        }

        public bool SendMailUsingSecondary(string sBankName, string strMailMessage, string strMailSubject, bool Origin)
        {
            bool result = false;
            int iRetryCount = 0;
            for (int iCount = 0; iCount < 3; iCount++)
            {
                iRetryCount = iRetryCount + 1;
                try
                {
                    //To Check Email function
                    EmailParam objEmailParam = new EmailParam();
                    SendEmail sendEmail = new SendEmail();
                    string emailTemplatePath = sendEmail.GetEmailTemplateByBankname(sBankName, SCVMacroSettings.FilePath);
                    string msgbdy = System.IO.File.ReadAllText(emailTemplatePath);

                    objEmailParam.SecondaryEnableSsl = bool.Parse(Helper.ReturnPolmtpf("MAILSecondary", "SecondaryEnableSsl"));
                    objEmailParam.SecondaryUseDefaultCredentials = bool.Parse(Helper.ReturnPolmtpf("MAILSecondary", "SecondaryUseDefaultCredentials"));
                    objEmailParam.SecondaryPortNo = int.Parse(Helper.ReturnPolmtpf("MAILSecondary", "SecondaryPortNo"));
                    objEmailParam.SecondaryPassword = Helper.ReturnPolmtpf("MAILSecondary", "SecondaryPassword");
                    objEmailParam.SecondaryFrom = Helper.ReturnPolmtpf("MAILSecondary", "SecondaryFrom");
                    objEmailParam.SecondaryUserName = Helper.ReturnPolmtpf("MAILSecondary", "SecondaryUserName");
                    objEmailParam.SecondarySMTP = Helper.ReturnPolmtpf("MAILSecondary", "SecondarySMTP");

                    objEmailParam.ToEmailAddress = Helper.ReturnPolmtpf("MAILSend", "MailTo");
                    objEmailParam.CCEmailAddress = Helper.ReturnPolmtpf("MAILSend", "MailCC");
                    objEmailParam.BCCEmailAddress = Helper.ReturnPolmtpf("MAILSend", "MailBCC");
                    objEmailParam.ApplicationName = Helper.ReturnPolmtpf("MAILSend", "ExceptionName"); ;

                    objEmailParam.MailSubject = strMailSubject;
                    string _MailContent = strMailMessage;
                    objEmailParam.MailContent = obj.ReplaceText(msgbdy, "Content message", _MailContent);
                    objEmailParam.AloneMailContent = _MailContent;
                    sendEmail.SendEmailUsingSecondary(objEmailParam);

                    // Email successfully sent. kill for loop
                    iRetryCount = 4;
                    iCount = 4;
                    result = true;
                    return result;
                }
                catch (Exception exRetry)
                {
                    if (iRetryCount == 3)
                    {
                        if (Origin)
                        {
                            result = SendMailUsingPrimary(sBankName, strMailMessage, strMailSubject, false);
                            return result;
                        }
                        throw exRetry;
                    }
                    // Sleep for 5 min if any network or internet change
                    System.Threading.Thread.Sleep(300000);
                }
            }
            return result;
        }

        [HttpPost]  
        public IActionResult Index(Login model)
        {
            


            _message = string.Format("User has entered User name : {0}, Password : {1}", model.Username, model.Password);
            Log.WriteEventLogwithParam("Login", "Index", "Post", "Start", _message, "System");
            if (ModelState.IsValid)
            {
                try
                {
                    using (objUserDbContext = new UserDbContext())
                    {
                        var usr = objUserDbContext.DbUserModel.Where(r => r.UserName == model.Username
                                        && r.Password == obj.EncryPass(model.Password)).FirstOrDefault();

                        if (usr != null && usr.IsActive)
                        {
                            _message = string.Format("Active user. User  ID : {0} retrieved from Database", model.UserID);
                            Log.WriteEventLogwithParam("Login", "Index", "Post", "middle", _message, model.UserID.ToString());
                            HttpContext.Session.SetString("UserID", usr.UserID.ToString());
                            HttpContext.Session.SetString("UserName", (usr.FirstName + " " + usr.LastName).Trim());
                            HttpContext.Session.SetString("UserType", usr.UserTypeDesc);
                            if (!string.IsNullOrEmpty(usr.LastLoginDate))
                            {
                                HttpContext.Session.SetString("LastLoginDate", Helper.DateFormater(usr.LastLoginDate));
                                HttpContext.Session.SetString("LastLoginTime", Helper.TimeFormater(usr.LastLoginTime));
                            }

                            HttpContext.Session.SetString("UsrID_BankName", usr.UserID.ToString() +" - ("+usr.BankName.ToString()+")");
                            HttpContext.Session.SetString("BankName", usr.BankName.ToString());
                            HttpContext.Session.SetString("BankFRN", usr.BankFRNNo.ToString());

                            SCVMacroSettings.BankName = usr.BankName.ToString();

                            usr.LastLoginDate = DateTime.Today.ToString("yyyyMMdd");
                            usr.LastLoginTime = DateTime.Now.ToString("HHmmss");

                            List<string> lstMenu = Helper.LoadMenuByUserID((long)usr.UserID);
                            string joinedMenu = string.Join(",", lstMenu);
                            HttpContext.Session.SetString("lstOfMenus", joinedMenu);

                            objUserDbContext.SaveChanges();
                            _message = string.Format("For User  ID : {0} Last Login Date and Time updated to database", model.UserID);
                            Log.WriteEventLogwithParam("Login", "Index", "Post", "End", _message, model.UserID.ToString());
                            return Json(new { success = true, message = "User have been successfully logged in." });
                        }
                        else
                        {
                            _message = string.Format("User has entered User name : {0}, Password : {1} is not matched with Database", model.Username, model.Password);
                            Log.WriteEventLogwithParam("Login", "Index", "Post", "End", _message, model.UserID.ToString());
                            return Json(new { success = false, message = "Invalid user name or password" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _message = string.Format("For User  name : {0} got an Error : {1}", model.Username, ex.ToString());
                    Log.WriteEventErrorLogwithParam("Login", "Index", "Post", "End", _message, model.UserID.ToString(), ex);
                    return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
                }
                
            }
            else
            {
                List<string> fieldOrder = new List<string>(new string[] {
                                 "UserName","Password"  })
                    .Select(f => f.ToLower()).ToList();

                var _message1 = ModelState
                    .Select(m => new { Order = fieldOrder.IndexOf(m.Key.ToLower()), Error = m.Value })
                    .OrderBy(m => m.Order)
                    .SelectMany(m => m.Error.Errors.Select(e => e.ErrorMessage)).ToList();

                _message = string.Join("<br/>", _message1);

               // _message = string.Format("User has not entered sufficient information because Model state validation got failed. ErrorMessage : {0} ", _message);
      

                return Json(new { success = false, message = _message });
            }
        }
    }
}
