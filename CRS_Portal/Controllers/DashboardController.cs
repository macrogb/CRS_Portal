using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CRS_Portal.Entity;
using CRS_Portal.HelperMethods;
using CRS_Portal.Models;

namespace CRS_Portal.Controllers
{
    [CheckSessionIsAvailable]
    public class DashboardController : Controller
    {
        NotificationDbContext _objNotificationDbContext;
        PolmtpfDbContext _objPolmtpfDbContext;

        string auditSts = string.Empty;
        string _message = string.Empty;
        string _userID = string.Empty;

        public DashboardController()
        {
            using (_objPolmtpfDbContext = new PolmtpfDbContext())
            {
                auditSts = Helper.ReturnPolmtpf("AUDIT", "Request");
                
            }
        }

        public IActionResult Index()
        {
            _userID = HttpContext.Session.GetString("UsrID_BankName");
            if (auditSts == "Y" || auditSts == "P")
            {
                ViewBag.IsSCVAuditRunning = true;
            }
            else
            {
                ViewBag.IsSCVAuditRunning = false;
            }
            Log.WriteEventLogwithParam("Dashboard", "Index", "Get", "Start", "Home Index method called by User", _userID);
            return View();
        }

        public IActionResult LoadDashboard()
        {
            _userID = HttpContext.Session.GetString("UsrID_BankName");
            Log.WriteEventLogwithParam("Dashboard", "LoadDashboard", "Get", "Start", "LoadDashboard method called by User", _userID);
            SCVAuditRun model = new SCVAuditRun();
            try
            {
                SCVAuditLicense apiResponse = Helper.GetAuditLicDataForBank(HttpContext.Session.GetString("BankName").ToLower(),
                    HttpContext.Session.GetString("BankFRN"));
                if (apiResponse != null)
                {
                    model.SCVLicenseActivatedDate = Helper.DateFormater(apiResponse.KeyRegDate);
                    model.SCVLicenseExpiryDate = Helper.DateFormater(apiResponse.KeyExpDate);
                    if (!string.IsNullOrEmpty(apiResponse.KeyExpDate))
                    {
                        model.RemainingDays = Helper.DateDifferenceByDays(apiResponse.KeyExpDate);
                    }
                    string sdate = Helper.ReturnPolmtpf("AUDIT", "STARTTIME");
                    if (!string.IsNullOrEmpty(sdate))
                    {
                        string[] split = sdate.Split(" ");
                        model.SCVLastAuditRunDate = Helper.DateFormater(split[0]) + " " + Helper.TimeFormater(split[1]);
                    }
                  
                    model.CompletedSCVAuditCount = apiResponse.KeyAccessedCount;
                    model.TotalSCVAuditCount = apiResponse.KeyTotalAccessCount;
                    model.RemainingSCVAuditCount = apiResponse.KeyAccessReminingCount;

                    var HighSCV = Helper.ReturnPolmtpf("AUDIT", "SCVHIGH");
                    if (string.IsNullOrEmpty(HighSCV))
                    {
                        HighSCV = "0";
                    }

                    var HighEXC = Helper.ReturnPolmtpf("AUDIT", "EXCHIGH");
                    if (string.IsNullOrEmpty(HighEXC))
                    {
                        HighEXC = "0";
                    }

                    var MediumSCV = Helper.ReturnPolmtpf("AUDIT", "SCVMEDIUM");
                    if (string.IsNullOrEmpty(MediumSCV))
                    {
                        MediumSCV = "0";
                    }

                    var MediumEXC = Helper.ReturnPolmtpf("AUDIT", "EXCMEDIUM");
                    if (string.IsNullOrEmpty(MediumEXC))
                    {
                        MediumEXC = "0";
                    }

                    var LowSCV = Helper.ReturnPolmtpf("AUDIT", "SCVLOW");
                    if (string.IsNullOrEmpty(LowSCV))
                    {
                        LowSCV = "0";
                    }

                    var LowEXC = Helper.ReturnPolmtpf("AUDIT", "EXCLOW");
                    if (string.IsNullOrEmpty(LowEXC))
                    {
                        LowEXC = "0";
                    }
                    model.ExpectionRpt_HighRiskCount = int.Parse(HighSCV) + int.Parse(HighEXC);
                    model.ExpectionRpt_MediumRiskCount = int.Parse(MediumSCV) + int.Parse(MediumEXC);
                    model.ExpectionRpt_LowRiskCount = int.Parse(LowSCV) + int.Parse(LowEXC);
                }
                Log.WriteEventLogwithParam("Dashboard", "LoadDashboard", "Get", "End", "LoadDashboard method called by User", _userID);
                var sa = new JsonSerializerSettings();
                return Json(model, sa);
            }
            catch (Exception ex)
            {
                Log.WriteEventErrorLogwithParam("Dashboard", "LoadDashboard", "Get", "Error", "Error Occurred", _userID, ex);
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        public IActionResult Logout()
        {
            try
            {
                _userID = HttpContext.Session.GetString("UsrID_BankName");
                Log.WriteEventLogwithParam("Dashboard", "Logout", "Get", "Start", "Logout method called by User", _userID);
                CRSMacroSettings.BankName = "";
                HttpContext.Session.SetString("lstOfMenus", "");
                HttpContext.Session.SetString("UserID", "");
                HttpContext.Session.SetString("UserName", "");

                HttpContext.Session.SetString("LastLoginDate", "");
                HttpContext.Session.SetString("LastLoginTime", "");
                HttpContext.Session.SetString("BankName", "");
                HttpContext.Session.SetString("BankFRN", "");

                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                Log.WriteEventErrorLogwithParam("Dashboard", "Logout", "Get", "Error", "Error Occurred", _userID, ex);
                return RedirectToAction("Index", "Login");
            }
        }

        #region Notification

        public IActionResult GetNewNotification()
        {
            _userID = HttpContext.Session.GetString("UsrID_BankName");
            Log.WriteEventLogwithParam("Dashboard", "GetNewNotification", "Get", "Start", "GetNewNotification method called by User", _userID);
            try
            {
                int _count = 0;
                List<Notification> _notification;
                using (_objNotificationDbContext = new NotificationDbContext())
                {
                    _notification = _objNotificationDbContext.DbNotificationModel.
                                                    Where(r => r.DestinationBankname.ToLower() == HttpContext.Session.GetString("BankName").ToLower() &&
                                                    r.DestinationBankFRN == HttpContext.Session.GetString("BankFRN") &&
                                                    r.IsActive == true
                                                    ).OrderByDescending(r => r.ID).ToList();

                    if (_notification.Count > 3)
                    {
                        _notification = _notification.OrderByDescending(r => r.ID).Take(3).ToList();
                    }
                    //Your “Volume Trendline” PDF is ready
                    string messageFormat = "<li> " +
                                           "<p> " +
                                           "##message##<a href='/UserDetails/Notification'> more details... </a> " +
                                           "<span class='timeline-icon'><i class='fas fa-bell' style='color:##colorName##'></i></span> " +
                                           "<span class='timeline-date'>##dateTime##</span> " +
                                           "</p> " +
                                           "</li> ";

                    foreach (var item in _notification)
                    {
                        if (item.UserSeen)
                        {
                            string mess = string.Empty;
                            mess = messageFormat.Replace("##message##", item.Message);
                            mess = mess.Replace("##colorName##", "red");
                            mess = mess.Replace("##dateTime##", Helper.DateFormater(item.CreatedDate) + ", " + Helper.TimeFormater(item.CreatedTime));
                            _message += mess;
                            _count += 1;
                        }
                        else
                        {
                            string mess = string.Empty;
                            mess = messageFormat.Replace("##message##", item.Message);
                            mess = mess.Replace("##colorName##", "green");
                            mess = mess.Replace("##dateTime##", Helper.DateFormater(item.CreatedDate) + ", " + Helper.TimeFormater(item.CreatedTime));
                            _message += mess;
                        }
                    }
                }
                Log.WriteEventLogwithParam("Dashboard", "GetNewNotification", "Get", "End", "GetNewNotification method called by User", _userID);
                return Json(new { success = true, msgCount = _count, message = _message });
            }
            catch (Exception ex)
            {
                Log.WriteEventErrorLogwithParam("Dashboard", "GetNewNotification", "Get", "Error", "Error Occurred", _userID, ex);
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }

        }

        public IActionResult NotificationAsRead()
        {
            _userID = HttpContext.Session.GetString("UsrID_BankName");
            Log.WriteEventLogwithParam("Dashboard", "NotificationAsRead", "Get", "Start", "NotificationAsRead method called by User", _userID);
            int _count = 0;
            try
            {
                string query = "UPDATE SCV_Notification SET UserSeen = 0, ModifiedDate= '" +
                   DateTime.Today.ToString("yyyyMMdd") + "', ModifiedTime ='" +
                   DateTime.Now.ToString("HHmmss") + "' where UserSeen = 1 AND DestinationBankname = '" +
                   HttpContext.Session.GetString("BankName").ToLower() + "' AND DestinationBankFRN = '" +
                   HttpContext.Session.GetString("BankFRN") + "'";

                using (_objNotificationDbContext = new NotificationDbContext())
                {
                    _objNotificationDbContext.Database.ExecuteSqlCommand(query);
                }
                Log.WriteEventLogwithParam("Dashboard", "NotificationAsRead", "Get", "End", "NotificationAsRead method called by User", _userID);
                return Json(new { success = true, msgCount = _count, message = _message });
            }
            catch (Exception ex)
            {
                Log.WriteEventErrorLogwithParam("Dashboard", "NotificationAsRead", "Get", "Error", "Error Occurred", _userID, ex);
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        #endregion

    }
}
