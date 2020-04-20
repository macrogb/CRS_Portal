using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Text;
using CRS_Portal.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Globalization;
using System.Data;
using CRS_Portal.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace CRS_Portal.HelperMethods
{
    public static class Helper
    {
        //string Baseurl = "https://api.macroiglobal.co.uk:441";
        //string Baseurl = "https://localhost:44319";
        
        public static string LoadDatabaseByBankName()
        {
            string sBankName = SCVMacroSettings.BankName;

            if (sBankName == "Canara")
            {
                return SCVMacroSettings.Canara_Audit_CS;
            }
            if (sBankName == "BOI")
            {
                return SCVMacroSettings.BOI_Audit_CS;
            }
            if (sBankName == "ICICI")
            {
                return SCVMacroSettings.ICICI_Audit_CS;
            }
            else
            {
                return SCVMacroSettings.Initial_CS;
            }
        }

        public static string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        public static string GetPathAndFilename(string filename, string processName, IHostingEnvironment env)
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd");
            string bankName = SCVMacroSettings.BankName;
            string path = ReturnPolmtpf("FILE", "SAASUPLOAD") + bankName + @"\" + processName + @"\" + dt + @"\";
            //string path = env.WebRootPath + "\\uploads\\" + bankName + "\\" + processName + "\\" + dt + "\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + filename;
        }

        public static string GetAuditBatchFilePath(string bankName, IHostingEnvironment env)
        {
            string path = string.Empty;
            if (bankName.ToLower() == "canara")
            {
                path = ReturnPolmtpf("FILE", "SAASBATCH") + bankName + "_Audit.bat";
                //path = env.WebRootPath + "\\BatchFile\\Audit\\" + bankName + "_Audit.bat";
            }
            return path;
        }

        public static SCVAuditLicense GetAuditLicDataForBank(string BankName, string FRNnum)
        {
            string Baseurl = SCVMacroSettings.SCVWebAPIURL;
            SCVAuditLicense _model = new SCVAuditLicense();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    SCVInput _m = new SCVInput();
                    _m.BankFRNNo = FRNnum;
                    _m.BankName = BankName;
                    //var stringContent = JsonConvert.SerializeObject(_m);

                    //string ms = string.Format("SCVApi/GetAuditLicenseDetailsForBankData?Param={0}", stringContent);             
                    HttpResponseMessage Res = client.PostAsJsonAsync("SCVApi/GetAuditLicenseDetailsForBankData", _m).Result;

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        _model = JsonConvert.DeserializeObject<SCVAuditLicense>(Res.Content.ReadAsStringAsync().Result);
                    }
                }
                catch (Exception)
                {
                    //throw;
                }             
            }
            return _model;         
        }

        public static void UpdateAuditSettingsToAuditHistory(string BankName, string FRNnum, string fileInfo)
        {
            string Baseurl = SCVMacroSettings.SCVWebAPIURL;
            SCVAuditLicense _model = new SCVAuditLicense();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    SCVInput _m = new SCVInput();
                    _m.BankFRNNo = FRNnum;
                    _m.BankName = BankName;
                    _m.Data = fileInfo;
                    //var stringContent = JsonConvert.SerializeObject(_m);

                    //string ms = string.Format("SCVApi/GetAuditLicenseDetailsForBankData?Param={0}", stringContent);             
                    HttpResponseMessage Res = client.PostAsJsonAsync("SCVApi/UpdateAuditUploadedFilePath", _m).Result;

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        _model = JsonConvert.DeserializeObject<SCVAuditLicense>(Res.Content.ReadAsStringAsync().Result);
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }

        public static List<SCVAuditHistory> GetAuditHistotyDataForBank(string BankName, string FRNnum)
        {
            string Baseurl = SCVMacroSettings.SCVWebAPIURL;
            //string Baseurl = "https://localhost:44319";
            List<SCVAuditHistory> _model = new List<SCVAuditHistory>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    SCVInput _m = new SCVInput();
                    _m.BankFRNNo = FRNnum;
                    _m.BankName = BankName;

                    string ms = string.Format("SCVApi/GetAuditHistoryDetailsForBank");
                    HttpResponseMessage Res = client.PostAsJsonAsync(ms, _m).Result;
                    //int v = (int)Res.StatusCode;
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        if (Response != null)
                        {
                            _model = JsonConvert.DeserializeObject<List<SCVAuditHistory>>(Response);
                        }
                    }
                }
                catch (Exception)
                {
                    //throw;
                }                
            }
            return _model;
        }

        public static EMContentPF GetEmailContent(string BankName, string FRNnum, string ReferenceNo)
        {
            string Baseurl = SCVMacroSettings.SCVWebAPIURL;
            //string Baseurl = "http://localhost:62164";
            EMContentPF EmailContent = new EMContentPF();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    SCVInput _m = new SCVInput();
                    _m.BankFRNNo = FRNnum;
                    _m.BankName = BankName;
                    _m.ReferenceNumber = ReferenceNo;

                    string ms = string.Format("SCVApi/GetEmailContentofAudit");
                    HttpResponseMessage Res = client.PostAsJsonAsync(ms, _m).Result;
                    //int v = (int)Res.StatusCode;
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        if (Response != null)
                        {
                            EmailContent = JsonConvert.DeserializeObject<EMContentPF>(Response);
                        }
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
            return EmailContent;
        }

        public static bool ResendMailAlert(string BankName, string FRNnum, string ReferenceNo)
        {
            //string Baseurl = SCVMacroSettings.SCVWebAPIURL;
            string Baseurl = "http://localhost:62164";
            bool status = false;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    SCVInput _m = new SCVInput();
                    _m.BankFRNNo = FRNnum;
                    _m.BankName = BankName;
                    _m.ReferenceNumber = ReferenceNo;

                    string ms = string.Format("SCVApi/ResendMailAlert");
                    HttpResponseMessage Res = client.PostAsJsonAsync(ms, _m).Result;
                    //int v = (int)Res.StatusCode;
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        if (Response != null)
                        {
                            bool dictResponse = JsonConvert.DeserializeObject<bool>(Response);
                            status = dictResponse;
                        }
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
            return status;
        }

        public static SCVAuditLicense UpdateAuditLicDataForBank(string BankName, string FRNnum, string toEmail, string ccEmail, string bccEmail)
        {
            string Baseurl = SCVMacroSettings.SCVWebAPIURL;
            SCVAuditLicense _model = new SCVAuditLicense();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    SCVInput _m = new SCVInput();
                    _m.BankFRNNo = FRNnum;
                    _m.BankName = BankName;
                    _m.ToEmailAddress = toEmail;
                    _m.BCCEmailAddress = bccEmail;
                    _m.CCEmailAddress = ccEmail;
                    //var stringContent = JsonConvert.SerializeObject(_m);

                    //string ms = string.Format("SCVApi/GetAuditLicenseDetailsForBankData?Param={0}", stringContent);             
                    HttpResponseMessage Res = client.PostAsJsonAsync("SCVApi/UpdateEmailDetilsTOSCVAuditLicense", _m).Result;

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        _model = JsonConvert.DeserializeObject<SCVAuditLicense>(Res.Content.ReadAsStringAsync().Result);
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
            return _model;
        }

        public static string DateFormater(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                string text = DateTime.ParseExact(date, "yyyyMMdd",
                            CultureInfo.InvariantCulture).ToString("yyyy-MMM-dd");

                return String.Join("-", text.Split('-').Reverse().ToArray());
            }
            return date;
        }

        public static string TimeFormater(string time)
        {
            if (!string.IsNullOrEmpty(time))
            {
                return time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4, 2);
            }
            return time;
        }

        public static string DateDifferenceByDays(string SourceDate)
        {
            DateTime sDt = DateTime.Now;
            DateTime eDt = DateTime.ParseExact(SourceDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            return (eDt.Date - sDt.Date).Days + " Days";
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            DataTable tbl = new DataTable();
            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = File.OpenRead(path) )
                    {
                        pck.Load(stream);
                    }
                    var ws = pck.Workbook.Worksheets.First();

                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column{0}", firstRowCell.Start.Column));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    return tbl;
                }
            }
            catch (Exception ex)
            {
                return tbl;              
            }
            
        }

        public static bool CheckAnyChildAvailable(long userID, int parentID )
        {
            ModuleDbContext _objModuleDbContext;
            List<ModuleChild> modelC = new List<ModuleChild>();

            using (_objModuleDbContext = new ModuleDbContext())
            {
                var linq = (from a in _objModuleDbContext.DbModuleChildModel.Where(r => r.ModuleParentId == parentID)
                                    join b in _objModuleDbContext.DbUserAccessLevelModel.Where(r => r.UserID == userID && r.IsAccess == true)
                                    on a.ID equals b.MenuID
                                    select new { b.ID }
                                    ).ToList(); 
                if (linq.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public static List<ModuleParent> LoadMenulevels(long usrID)
        {
            ModuleDbContext _objModuleDbContext;
            List<ModuleParent> modelP = new List<ModuleParent>();

            using (_objModuleDbContext = new ModuleDbContext())
            {
                modelP = _objModuleDbContext.DbModuleParentModel.ToList();

                foreach (var item in modelP)
                {
                    List<ModuleChild> modelC = new List<ModuleChild>();
                    modelC = _objModuleDbContext.DbModuleChildModel.Where(r => r.ModuleParentId == item.ID).ToList();
                    foreach (var _item in modelC)
                    {
                        var accLevel = _objModuleDbContext.DbUserAccessLevelModel.Where(r => r.UserID == usrID && r.MenuID == _item.ID).FirstOrDefault();
                        if (accLevel != null && accLevel.IsAccess)
                        {
                            _item.IsChecked_C = true;
                        }
                        else
                        {
                            _item.IsChecked_C = false;
                        }
                    }
                    item.ModuleChild = modelC;
                }
            }
            return modelP;
        }

        public static List<string> LoadMenuByUserID(long usrId)
        {
            List<string> lstPageID = new List<string>();
            List<ModuleParent> lstmdlPar = LoadMenulevels(usrId);
            if (lstmdlPar.Count > 0)
            {
                lstPageID = lstmdlPar.SelectMany(r => r.ModuleChild.Where(a => a.IsChecked_C == false)).Select(s => s.PageID).ToList();
                
                if (!CheckAnyChildAvailable(usrId,1))
                {
                    lstPageID.Add("liDashboard");
                }
                if (!CheckAnyChildAvailable(usrId, 2))
                {
                    lstPageID.Add("liSCVAudit");
                }
                if (!CheckAnyChildAvailable(usrId, 3))
                {
                    lstPageID.Add("liBankSetting");
                }
                if (!CheckAnyChildAvailable(usrId, 4))
                {
                    lstPageID.Add("liUsrSetting");
                }
                if (!CheckAnyChildAvailable(usrId, 5))
                {
                    lstPageID.Add("liadminSetting");
                }
                if (!CheckAnyChildAvailable(usrId, 6))
                {
                    lstPageID.Add("liSCVAutomation");
                }
            }
            return lstPageID;
        }

        public static IEnumerable<SelectListItem> convertListToIEnumerable(List<string> data, bool addSelect)
        {
            var selectList = new List<SelectListItem>();

            if (addSelect)
            {
                selectList.Add(new SelectListItem
                {
                    Value = "-1",
                    Text = "-- Select --"
                });
            }
            foreach (var element in data)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element.Replace("[", "").Replace("]", "")
                });
            }
            
            return selectList;
        }

        public static IEnumerable<SelectListItem> GetSelectListItems(string usrID)
        {
            List<UserDetails> objlstUsr = new List<UserDetails>();
            using (UserDbContext objUserDbContext = new UserDbContext())
            {
                objlstUsr = objUserDbContext.DbUserModel.Where(r => r.IsActive == true).ToList();
            }

            var selectList = new List<SelectListItem>();
            foreach (var element in objlstUsr)
            {
                selectList.Add(new SelectListItem
                {      
                    Value = element.UserID.ToString(),
                    Text = element.UserName +" (" + element.BankName + " - " + element.BankFRNNo + ") "
                });
            }
            foreach (var item in selectList)
            {
                if (item.Value == usrID)
                {
                    item.Selected = true;
                }
            }
            return selectList;
        }

        public static void UpdatePolmtpf(string pModule, string pType, string pDet)
        {
            using (PolmtpfDbContext _objPolmtpfDbContext = new PolmtpfDbContext())
            {
                var _polm = _objPolmtpfDbContext.DbPOLMTPFModel.Where(r => r.PMODULE == pModule && r.PTYPE == pType).FirstOrDefault();
                if (_polm != null)
                {
                    _polm.POLDET = pDet;
                    _objPolmtpfDbContext.SaveChanges();
                }
            }
        }

        public static string ReturnPolmtpf(string pModule, string pType)
        {
            using (PolmtpfDbContext _objPolmtpfDbContext = new PolmtpfDbContext())
            {
                var _polm = _objPolmtpfDbContext.DbPOLMTPFModel.Where(r => r.PMODULE == pModule && r.PTYPE == pType).FirstOrDefault();
                if (_polm != null)
                {
                    return _polm.POLDET;
                }
            }
            return "";
        }

        public static void ExecuteCommand(string query)
        {
            using (PolmtpfDbContext _objPolmtpfDbContext = new PolmtpfDbContext())
            {               
                if (!string.IsNullOrEmpty(query))
                {
                    _objPolmtpfDbContext.Database.ExecuteSqlCommand(query);
                }
            }
        }

        public static DataTable ExecuteQueryGetDataTable(string query)
        {
            using (PolmtpfDbContext context = new PolmtpfDbContext())
            {
                var dt = new DataTable();
                var conn = context.Database.GetDbConnection();
                var connectionState = conn.State;
                try
                {
                    if (connectionState != ConnectionState.Open) conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        //cmd.Parameters.Add(new SqlParameter("jobCardId", 100525));
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // error handling
                    throw;
                }
                finally
                {
                    if (connectionState != ConnectionState.Closed) conn.Close();
                }
                return dt;
            }
        }

        public static bool CheckEmaillstIsValid(string emailAddress)
        {
            bool result = false;
            if (string.IsNullOrEmpty(emailAddress))
            {
                result = true;
            }
            else
            {
                if (emailAddress.Contains(";"))
                {
                    var data = emailAddress.Split(";").ToList();
                    foreach (var item in data)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var mail = new MailAddress(item);
                                result = true;
                            }
                        }
                        catch
                        {
                            result = false;
                        }
                    }
                }
                else
                {
                    try
                    {
                        var mail = new MailAddress(emailAddress);
                        result = true;
                    }
                    catch (Exception)
                    {

                        result = false;
                    }
                }
            }
            return result;
        }

        public static void SaveUploadedFileInfo2Polmtpf(string fileName, int pos, string sProcFileType)
        {
            string fileType = ReturnPolmtpf("FILE", "TYPE");
            if (fileType == "1" && sProcFileType == "1")
            {
                if (pos == 0)
                    UpdatePolmtpf("FILE", "CABCD", fileName);
                else if (pos == 1)
                    UpdatePolmtpf("FILE", "EABCD", fileName);
            }
            else if (fileType == "1" && sProcFileType == "2")
            {
                UpdatePolmtpf("FILE", "CABCD", fileName);
            }
            else if (fileType == "1" && sProcFileType == "3")
            {
                UpdatePolmtpf("FILE", "EABCD", fileName);
            }
            else if (fileType == "2" && sProcFileType == "1")
            {
                if (pos == 0)
                    UpdatePolmtpf("FILE", "CABD", fileName);
                else if (pos == 1)
                    UpdatePolmtpf("FILE", "CC", fileName);
                else if (pos == 2)
                    UpdatePolmtpf("FILE", "EABD", fileName);
                else if (pos == 3)
                    UpdatePolmtpf("FILE", "EC", fileName);
            }
            else if (fileType == "2" && sProcFileType == "2")
            {
                UpdatePolmtpf("FILE", "CABD", fileName);
                UpdatePolmtpf("FILE", "CC", fileName);
            }
            else if (fileType == "2" && sProcFileType == "3")
            {
                UpdatePolmtpf("FILE", "EABD", fileName);
                UpdatePolmtpf("FILE", "EC", fileName);
            }
            else if (fileType == "4" && sProcFileType == "1")
            {
                if (pos == 0)
                    UpdatePolmtpf("FILE", "CA", fileName);
                else if (pos == 1)
                    UpdatePolmtpf("FILE", "CB", fileName);
                else if (pos == 2)
                    UpdatePolmtpf("FILE", "CC", fileName);
                else if (pos == 3)
                    UpdatePolmtpf("FILE", "CD", fileName);
                else if (pos == 4)
                    UpdatePolmtpf("FILE", "EA", fileName);
                else if (pos == 5)
                    UpdatePolmtpf("FILE", "EB", fileName);
                else if (pos == 6)
                    UpdatePolmtpf("FILE", "EC", fileName);
                else if (pos == 7)
                    UpdatePolmtpf("FILE", "ED", fileName);
            }
            else if (fileType == "4" && sProcFileType == "2")
            {
                UpdatePolmtpf("FILE", "CA", fileName);
                UpdatePolmtpf("FILE", "CB", fileName);
                UpdatePolmtpf("FILE", "CC", fileName);
                UpdatePolmtpf("FILE", "CD", fileName);
            }
            else if (fileType == "4" && sProcFileType == "3")
            {
                UpdatePolmtpf("FILE", "EA", fileName);
                UpdatePolmtpf("FILE", "EB", fileName);
                UpdatePolmtpf("FILE", "EC", fileName);
                UpdatePolmtpf("FILE", "ED", fileName);
            }
        }

        public static bool CheckSCVFileFormat_One(string sProcFileType)
        {
            string fPath = ReturnPolmtpf("FILE", "INPUT");
            string[] sABCDColMap = { "SCVRN", "CUSTTIT", "CUST1FNAME", "CUST2FNAME", "CUST3FNAME", "CUSTSURNAME", "CUSTPREVNAME", "CUSTNINO", "CUSTPASSPORT",
                "CUSTOTHNIDTYPE", "CUSTOTHNIDNO", "COMPREGNO", "CUSTDOB", "B_SCVRN", "CUSTADDRLINE1", "CUSTADDRLINE2", "CUSTADDRLINE3", "CUSTADDRLINE4",
                "CUSTADDRLINE5", "CUSTADDRLINE6", "CUSTPOSTALCODE", "CUSTCOUNTRY", "CUSTEMAILID", "CUSTMAINPHONE", "CUSTEVENPHONE", "CUSTMOBNO", "C_SCVRN",
                "ACCTIT", "ACCNUM", "BIC", "IBAN", "SORTCODE", "ACCPRODTYPE", "PRODNAME", "ACCHOLDERIND", "ACCSTSCODE", "EXCLTYPE", "RECENTTRAN", "ACCBRNCHJUR",
                "BRRD", "STRUCTDEPACC", "ACCBAL", "AUTHNEGBAL", "CCY", "ORGBALWINT", "EXCGRATE", "ORGBALWOINT", "TRNELGDEP", "D_SCVRN", "CUSTAGGBAL",
                "CUSTCOMPOFFAMNT" };

            var list = new List<Tuple<string, string, string[], string>>();
            if (sProcFileType == "1")
            {
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CABCD"), sABCDColMap, "SCV_ABCD_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EABCD"), sABCDColMap, "SCV_ABCD_DETAILS_EXC"));
            }
            else if (sProcFileType == "2")
            {
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CABCD"), sABCDColMap, "SCV_ABCD_DETAILS_SCV"));
            }
            else if (sProcFileType == "3")
            {
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EABCD"), sABCDColMap, "SCV_ABCD_DETAILS_EXC"));
            }


            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Item2))
                {
                    if (!retriveFileData(fPath, item.Item1, item.Item2, item.Item3, item.Item4))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckSCVFileFormat_Two(string sProcFileType)
        {
            string fPath = ReturnPolmtpf("FILE", "INPUT");
            string[] sABDColMap = new string[] { "SCVRN", "CUSTTIT", "CUST1FNAME", "CUST2FNAME", "CUST3FNAME", "CUSTSURNAME", "CUSTPREVNAME", "CUSTNINO",
                "CUSTPASSPORT", "CUSTOTHNIDTYPE", "CUSTOTHNIDNO", "COMPREGNO", "CUSTDOB", "B_SCVRN", "CUSTADDRLINE1", "CUSTADDRLINE2", "CUSTADDRLINE3",
                "CUSTADDRLINE4", "CUSTADDRLINE5", "CUSTADDRLINE6", "CUSTPOSTALCODE", "CUSTCOUNTRY", "CUSTEMAILID", "CUSTMAINPHONE", "CUSTEVENPHONE",
                "CUSTMOBNO", "D_SCVRN", "CUSTAGGBAL", "CUSTCOMPOFFAMNT" };
            string[] sCColMap = { "SCVRN", "CUSTTIT", "ACCNUM", "BIC", "IBAN", "SORTCODE", "ACCPRODTYPE", "PRODNAME", "ACCHOLDERIND", "ACCSTSCODE", "EXCLTYPE",
                "RECENTTRAN", "ACCBRNCHJUR", "BRRD", "STRUCTDEPACC", "ACCBAL", "AUTHNEGBAL", "CCY", "ORGBALWINT", "EXCGRATE", "ORGBALWOINT", "TRNELGDEP" };

            var list = new List<Tuple<string, string, string[], string>>();
            if (sProcFileType == "1")
            {
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CABD"), sABDColMap, "SCV_ABD_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EABD"), sABDColMap, "SCV_ABD_DETAILS_EXC"));
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CC"), sCColMap, "SCV_C_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EC"), sCColMap, "SCV_C_DETAILS_EXC"));
            }
            else if (sProcFileType == "2")
            {
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CABD"), sABDColMap, "SCV_ABD_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EABD"), sABDColMap, "SCV_C_DETAILS_SCV"));
            }
            else if (sProcFileType == "3")
            {
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CC"), sCColMap, "SCV_ABD_DETAILS_EXC"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EC"), sCColMap, "SCV_C_DETAILS_EXC"));
            }


            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Item2))
                {
                    if (!retriveFileData(fPath, item.Item1, item.Item2, item.Item3, item.Item4))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckSCVFileFormat_Four(string sProcFileType)
        {


            string fPath = ReturnPolmtpf("FILE", "INPUT");
            string[] sAColMap = { "SCVRN", "CUSTTIT", "CUST1FNAME", "CUST2FNAME", "CUST3FNAME", "CUSTSURNAME", "CUSTPREVNAME", "CUSTNINO", "CUSTPASSPORT",
                                                "CUSTOTHNIDTYPE", "CUSTOTHNIDNO", "COMPREGNO", "CUSTDOB" };
            string[] sBColMap = { "SCVRN", "CUSTADDRLINE1", "CUSTADDRLINE2", "CUSTADDRLINE3", "CUSTADDRLINE4", "CUSTADDRLINE5", "CUSTADDRLINE6",
                                                "CUSTPOSTALCODE", "CUSTCOUNTRY", "CUSTEMAILID", "CUSTMAINPHONE", "CUSTEVENPHONE", "CUSTMOBNO" };
            string[] sCColMap = { "SCVRN", "CUSTTIT", "ACCNUM", "BIC", "IBAN", "SORTCODE", "ACCPRODTYPE", "PRODNAME", "ACCHOLDERIND", "ACCSTSCODE", "EXCLTYPE",
                                                "RECENTTRAN", "ACCBRNCHJUR", "BRRD", "STRUCTDEPACC", "ACCBAL", "AUTHNEGBAL", "CCY", "ORGBALWINT", "EXCGRATE", "ORGBALWOINT",
                                                "TRNELGDEP" };
            string[] sDColMap = { "SCVRN", "CUSTAGGBAL", "CUSTCOMPOFFAMNT" };

            var list = new List<Tuple<string, string, string[], string>>();
            if (sProcFileType == "1")
            {
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CA"), sAColMap, "SCV_A_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EA"), sAColMap, "SCV_A_DETAILS_EXC"));
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CB"), sBColMap, "SCV_B_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EB"), sBColMap, "SCV_B_DETAILS_EXC"));
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CC"), sCColMap, "SCV_C_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EC"), sCColMap, "SCV_C_DETAILS_EXC"));
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CD"), sDColMap, "SCV_D_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "ED"), sDColMap, "SCV_D_DETAILS_EXC"));
            }
            else if (sProcFileType == "2")
            {
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CA"), sAColMap, "SCV_A_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CB"), sBColMap, "SCV_B_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CC"), sCColMap, "SCV_C_DETAILS_SCV"));
                list.Add(new Tuple<string, string, string[], string>("", ReturnPolmtpf("FILE", "CD"), sDColMap, "SCV_D_DETAILS_SCV"));
            }
            else if (sProcFileType == "3")
            {
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EA"), sAColMap, "SCV_A_DETAILS_EXC"));                
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EB"), sBColMap, "SCV_B_DETAILS_EXC"));                
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "EC"), sCColMap, "SCV_C_DETAILS_EXC"));
                list.Add(new Tuple<string, string, string[], string>("E", ReturnPolmtpf("FILE", "ED"), sDColMap, "SCV_D_DETAILS_EXC"));
            }


            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Item2))
                {
                    if (!retriveFileData(fPath, item.Item1, item.Item2, item.Item3, item.Item4))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool retriveFileData(string fPath, string actstatus, string fname, string[] sColMapping, string tabName)
        {
            try
            {
                if (ReturnPolmtpf("FILE", "HEADER") == "N")
                {
                     return retriveFileDataWithOutHeader(fPath, actstatus, fname, sColMapping, tabName);           
                }
                string[] header = new string[4];
                string sFileLoc = fPath + fname;

                StreamReader fstream = new StreamReader(sFileLoc);
                ArrayList data = new ArrayList();
                DataTable dt = new DataTable();

                while (!fstream.EndOfStream)
                {
                    string rec = fstream.ReadLine();
                    rec = rec.Replace("~", "");
                    data.Add(rec);
                    //data.Add(fstream.ReadLine());
                }

                if (data.Count >= 1)
                {
                    header[0] = data[0].ToString();
                }

                int i = 0;
                char delimiter = char.Parse(ReturnPolmtpf("COLUMN", "DELIMITER"));

                for (int j = data.Count - 1; j > 1; j--)
                {
                    if (!data[j].ToString().Contains(delimiter.ToString()))
                    {
                        //string query = "INSERT INTO SCV_Footer_Details VALUES('" + tabName + "', '" + fname + "', '" + data[j].ToString() + "')";
                        //obj.ExecuteCommand(query);
                    }
                    else
                    {
                        break;
                    }
                }

                for (i = 1; i < data.Count; i++)
                {
                    if (data[i].ToString().Trim().Contains(delimiter.ToString()))
                    {
                        string[] actdet = data[i].ToString().Split(delimiter);

                        if (i == 1)
                        {
                            for (int j = 0; j < sColMapping.Length; j++)
                                dt.Columns.Add(sColMapping[j]);
                        }
                        dt.Rows.Add(actdet);
                    }
                }

                if (dt.Rows.Count >= 1)
                {
                    SqlBulkCopy sbc = new SqlBulkCopy(LoadDatabaseByBankName());
                    sbc.BulkCopyTimeout = 0;
                    for (int j = 0; j < sColMapping.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(sColMapping[j]))
                        {
                            sbc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(sColMapping[j], sColMapping[j]));
                        }
                    }
                    sbc.DestinationTableName = tabName;
                    sbc.WriteToServer(dt);
                    sbc.Close();
                }
                fstream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool retriveFileDataWithOutHeader(string fPath, string actstatus, string fname, string[] sColMapping, string tabName)
        {
            try
            {
                string[] header = new string[4];
                string sFileLoc = fPath + fname;

                StreamReader fstream = new StreamReader(sFileLoc);

                ArrayList data = new ArrayList();
                DataTable dt = new DataTable();

                while (!fstream.EndOfStream)
                {
                    string rec = fstream.ReadLine();
                    rec = rec.Replace("~", "");
                    data.Add(rec);
                }

                int i = 0;
                char delimiter = char.Parse(ReturnPolmtpf("COLUMN", "DELIMITER"));

                for (int j = data.Count - 1; j > 1; j--)
                {
                    if (!data[j].ToString().Contains(delimiter.ToString()))
                    {
                        //string query = "INSERT INTO SCV_Footer_Details VALUES('" + tabName + "', '" + fname + "', '" + data[j].ToString() + "')";
                        //obj.ExecuteCommand(query);
                    }
                    else
                    {
                        break;
                    }
                }

                for (i = 0; i < data.Count; i++)
                {
                    if (data[i].ToString().Trim().Contains(delimiter.ToString()))
                    {
                        string[] actdet = data[i].ToString().Split(delimiter);

                        if (i == 0)
                        {
                            for (int j = 0; j < sColMapping.Length; j++)
                                dt.Columns.Add(sColMapping[j]);
                        }

                        dt.Rows.Add(actdet);
                    }
                }

                if (dt.Rows.Count >= 1)
                {
                    SqlBulkCopy sbc = new SqlBulkCopy(LoadDatabaseByBankName());
                    sbc.BulkCopyTimeout = 0;
                    for (int j = 0; j < sColMapping.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(sColMapping[j]))
                        {
                            sbc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(sColMapping[j], sColMapping[j]));
                        }
                    }
                    sbc.DestinationTableName = tabName;

                    sbc.WriteToServer(dt);

                    sbc.Close();
                }

                fstream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void ClearFileName()
        {
            UpdatePolmtpf("FILE", "CABCD", "");
            UpdatePolmtpf("FILE", "EABCD", "");

            UpdatePolmtpf("FILE", "CC", "");
            UpdatePolmtpf("FILE", "EC", "");
            UpdatePolmtpf("FILE", "EABD", "");
            UpdatePolmtpf("FILE", "CABD", "");

            UpdatePolmtpf("FILE", "CA", "");
            UpdatePolmtpf("FILE", "EA", "");
            UpdatePolmtpf("FILE", "CB", "");
            UpdatePolmtpf("FILE", "EB", "");
            UpdatePolmtpf("FILE", "CC", "");
            UpdatePolmtpf("FILE", "EC", "");
            UpdatePolmtpf("FILE", "CD", "");
            UpdatePolmtpf("FILE", "ED", "");
        }

        

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }

        public static string GetCellEndName(int wscolCount)
        {
            string[] arr1 = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            string value = string.Empty; 
             
            if (wscolCount > 26)
            {
                wscolCount = 26;
            }


            //Find Total Column Width
            for (int m = 0; m <= (wscolCount - 1); m++)
            {
                value = arr1[m] + "1";
            }
            return value;
        }

        public static bool DatasetToExcelExport(DataSet ds, string fPath, string fName, int Count, string orgin)
        {
            bool result = false;
            try
            {
                if (!Directory.Exists(fPath))
                {
                    Directory.CreateDirectory(fPath);
                }
                using (ExcelPackage pck = new ExcelPackage())
                {
                    foreach (DataTable dataTable in ds.Tables)
                    {
                        ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dataTable.TableName);
                        workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                        workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                        
                        if (orgin == "AccountStautsCode")
                        {
                            workSheet.Column(2).Width = 100;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(4).Width = 24;
                        }
                        else if (orgin == "BIC")
                        {
                            workSheet.Column(1).Width = 20;
                            workSheet.Column(1).Style.WrapText = true;
                            workSheet.Column(2).Width = 20;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(3).Width = 20;
                            workSheet.Column(3).Style.WrapText = true;
                            workSheet.Column(4).Width = 20;
                            workSheet.Column(4).Style.WrapText = true;
                        }
                        else if (orgin == "CorpAccount")
                        {
                            workSheet.Column(1).Width = 22;
                            workSheet.Column(1).Style.WrapText = true;
                        }
                        else if (orgin == "POAAccount")
                        {
                            workSheet.Column(1).Width = 22;
                            workSheet.Column(1).Style.WrapText = true;
                        }
                        else if (orgin == "Product")
                        {
                            workSheet.Column(2).Width = 27;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(2).Width = 18;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(3).Width = 30;
                            workSheet.Column(3).Style.WrapText = true;
                        }
                        else if (orgin == "SortCode")
                        {
                            workSheet.Column(1).Width = 15;
                            workSheet.Column(1).Style.WrapText = true;
                        }
                        else if (orgin == "ExcRptExcludeSts")
                        {                            
                            workSheet.Column(1).Width = 50;
                            workSheet.Column(1).Style.WrapText = true;
                            workSheet.Column(2).Width = 150;
                            workSheet.Column(2).Style.WrapText = true;
                        }
                        else if (orgin == "ExceptionReport")
                        {
                            workSheet.Column(1).Width = 50;
                            workSheet.Column(1).Style.WrapText = true;
                            workSheet.Column(2).Width = 150;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(3).Width = 11;
                            workSheet.Column(3).Style.WrapText = true;
                        }
                        else if (orgin == "ExcpSummary")
                        {
                            workSheet.Column(3).Width = 12;
                            workSheet.Column(3).Style.WrapText = true;
                            workSheet.Column(4).Width = 41;
                            workSheet.Column(4).Style.WrapText = true;
                            workSheet.Column(5).Width = 41;
                            workSheet.Column(5).Style.WrapText = true;

                            ExcelAddress _formatRangeAddress1 = new ExcelAddress("D2:D147");
                            string _statement = "IF(OFFSET(D2,0,1)-D2<0,1,0)";
                            var _cond1 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress1);
                            _cond1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond1.Style.Fill.BackgroundColor.Color = Color.FromArgb(153, 255, 206);
                            _cond1.Formula = _statement;

                            ExcelAddress _formatRangeAddress2 = new ExcelAddress("E2:E147");
                            //_statement = "IF(OFFSET(E2,0,-1)-E2<0,1,0)";
                            var _cond2 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress2);
                            _cond2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond2.Style.Fill.BackgroundColor.Color = Color.FromArgb(229, 255, 242);
                            _cond2.Formula = _statement;

                            ExcelAddress _formatRangeAddress3 = new ExcelAddress("D2:D147");
                            _statement = "IF(OFFSET(E2,0,-1)-E2<0,1,0)";
                            var _cond3 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress3);
                            _cond3.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond3.Style.Fill.BackgroundColor.Color = Color.FromArgb(255, 229, 229);
                            _cond3.Formula = _statement;

                            ExcelAddress _formatRangeAddress4 = new ExcelAddress("E2:E147");
                            var _cond4 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress4);
                            _cond4.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond4.Style.Fill.BackgroundColor.Color = Color.FromArgb(255, 102, 102);
                            _cond4.Formula = _statement;


                        }


                        string val = GetCellEndName(Count);
                        string cellValue = "A1:" + val; 
                        using (ExcelRange Rng = workSheet.Cells[cellValue])
                        {
                            Rng.Style.Font.Size = 13;
                            Rng.Style.Font.Bold = true;
                            Rng.Style.Font.Color.SetColor(Color.White);
                            //Rng.Style.Font.UnderLine = true;
                            Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            Rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(76, 181, 245)); 
                            //Rng.Style.WrapText = true;
                            //Rng.Style.ShrinkToFit = true;
                        }
                        
                    }

                    pck.SaveAs(new FileInfo(fPath+""+ fName));
                }
                result = true;
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        public static byte[] DataSetToExcelContent(DataSet ds, string fName, int Count, string orgin,ref  bool result)
        {
            result = false;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    foreach (DataTable dataTable in ds.Tables)
                    {
                        ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dataTable.TableName);
                        workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                        workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                        if (orgin == "AccountStautsCode")
                        {
                            workSheet.Column(2).Width = 100;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(4).Width = 24;
                        }
                        else if (orgin == "BIC")
                        {
                            workSheet.Column(1).Width = 20;
                            workSheet.Column(1).Style.WrapText = true;
                            workSheet.Column(2).Width = 20;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(3).Width = 20;
                            workSheet.Column(3).Style.WrapText = true;
                            workSheet.Column(4).Width = 20;
                            workSheet.Column(4).Style.WrapText = true;
                        }
                        else if (orgin == "CorpAccount")
                        {
                            workSheet.Column(1).Width = 22;
                            workSheet.Column(1).Style.WrapText = true;
                        }
                        else if (orgin == "POAAccount")
                        {
                            workSheet.Column(1).Width = 22;
                            workSheet.Column(1).Style.WrapText = true;
                        }
                        else if (orgin == "Product")
                        {
                            workSheet.Column(2).Width = 27;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(2).Width = 18;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(3).Width = 30;
                            workSheet.Column(3).Style.WrapText = true;
                        }
                        else if (orgin == "SortCode")
                        {
                            workSheet.Column(1).Width = 15;
                            workSheet.Column(1).Style.WrapText = true;
                        }
                        else if (orgin == "ExcRptExcludeSts")
                        {
                            workSheet.Column(1).Width = 50;
                            workSheet.Column(1).Style.WrapText = true;
                            workSheet.Column(2).Width = 150;
                            workSheet.Column(2).Style.WrapText = true;
                        }
                        else if (orgin == "ExceptionReport")
                        {
                            workSheet.Column(1).Width = 50;
                            workSheet.Column(1).Style.WrapText = true;
                            workSheet.Column(2).Width = 150;
                            workSheet.Column(2).Style.WrapText = true;
                            workSheet.Column(3).Width = 11;
                            workSheet.Column(3).Style.WrapText = true;
                        }
                        else if (orgin == "ExcpSummary")
                        {
                            workSheet.Column(3).Width = 12;
                            workSheet.Column(3).Style.WrapText = true;
                            workSheet.Column(4).Width = 41;
                            workSheet.Column(4).Style.WrapText = true;
                            workSheet.Column(5).Width = 41;
                            workSheet.Column(5).Style.WrapText = true;

                            ExcelAddress _formatRangeAddress1 = new ExcelAddress("D2:D147");
                            string _statement = "IF(OFFSET(D2,0,1)-D2<0,1,0)";
                            var _cond1 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress1);
                            _cond1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond1.Style.Fill.BackgroundColor.Color = Color.FromArgb(153, 255, 206);
                            _cond1.Formula = _statement;

                            ExcelAddress _formatRangeAddress2 = new ExcelAddress("E2:E147");
                            //_statement = "IF(OFFSET(E2,0,-1)-E2<0,1,0)";
                            var _cond2 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress2);
                            _cond2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond2.Style.Fill.BackgroundColor.Color = Color.FromArgb(229, 255, 242);
                            _cond2.Formula = _statement;

                            ExcelAddress _formatRangeAddress3 = new ExcelAddress("D2:D147");
                            _statement = "IF(OFFSET(E2,0,-1)-E2<0,1,0)";
                            var _cond3 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress3);
                            _cond3.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond3.Style.Fill.BackgroundColor.Color = Color.FromArgb(255, 229, 229);
                            _cond3.Formula = _statement;

                            ExcelAddress _formatRangeAddress4 = new ExcelAddress("E2:E147");
                            var _cond4 = workSheet.ConditionalFormatting.AddExpression(_formatRangeAddress4);
                            _cond4.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            _cond4.Style.Fill.BackgroundColor.Color = Color.FromArgb(255, 102, 102);
                            _cond4.Formula = _statement;


                        }


                        string val = GetCellEndName(Count);
                        string cellValue = "A1:" + val;
                        using (ExcelRange Rng = workSheet.Cells[cellValue])
                        {
                            Rng.Style.Font.Size = 13;
                            Rng.Style.Font.Bold = true;
                            Rng.Style.Font.Color.SetColor(Color.White);
                            //Rng.Style.Font.UnderLine = true;
                            Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            Rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(76, 181, 245));
                            //Rng.Style.WrapText = true;
                            //Rng.Style.ShrinkToFit = true;
                        }

                    }
                    //pck.SaveAs(new FileInfo(fPath + "" + fName));
                    byte[] ExcelContent = pck.GetAsByteArray();
                    result = true;
                    return ExcelContent;
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public static string CreateDirectoryIfNotExists(string fPath)
        {
            if (!Directory.Exists(fPath))
            {
                Directory.CreateDirectory(fPath);
            }
            return fPath;
        }


        public static bool DeleteSCVAuditReportFromHistory(SCVInput model)
        {
            bool result = false;
            string Baseurl = SCVMacroSettings.SCVWebAPIURL;
            SCVAuditLicense _model = new SCVAuditLicense();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(Baseurl);

                    HttpResponseMessage Res = client.PostAsJsonAsync("SCVApi/DeleteSCVAuditReport", model).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        _model = JsonConvert.DeserializeObject<SCVAuditLicense>(Res.Content.ReadAsStringAsync().Result);
                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        #region EMail

        public static void SendEmail(string ToMailAddress, string CCMailAddress, string BankName, string Subject, string UserName)
        {
            string _EmailTo = ToMailAddress;
            string bankName = BankName;
            string strMailSubject = bankName + " Subject";
            string name = bankName;

            string mailcontent = "Welcome " + name + ",<br/><br/>" +
                     "We were asked to reset your Developer Portal account password. <br/><br/>" +
                     "Follow the instructions below if this request comes from you. <br/><br/>" +
                     "Ignore this E-Mail if the request to reset your password does not come from you and don't worry, your account is safe. <br/><br/>" +
                     "Click the following link to set a new password.<br/><br/>" +
                     "<br/><br/> If clicking the link doesn't work then you can copy the link into your browser window or type it there directly.  <br/><br/>" +
                     "Best regards, <br/>" +
                     "The API Portal Team";

            string path = GetEmailTemplateByBankname();
            path = path.Replace("~\\", "");

            string msgbdy = System.IO.File.ReadAllText(path);

            NewMailNotification(_EmailTo, strMailSubject, msgbdy);
        }

        public static string GetEmailTemplateByBankname()
        {
            string path = string.Empty;
            string sbankName = ReturnPolmtpf("APCNFG", "BANKNAME");
            if (sbankName == "UBI")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\UBI_design.html");
            }
            else if (sbankName == "UBL")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\UBL_design.html");
            }
            else if (sbankName == "SBI")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\SBI_design.html");
            }
            else if (sbankName == "ICICI")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\ICICI_design.html");
            }
            else if (sbankName == "BFC Bank")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\BFC_design.html");
            }
            else if (sbankName == "Bank of India UK")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\BOI_design.html");
            }
            else if (sbankName == "BOB")
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\BOB_design.html");
            }
            else
            {
                path = System.IO.Path.GetFullPath(@"~\EmailTemplate\design.html");
            }
            return path;
        }

        public static void NewMailNotification(string _EmailTo, string strMailSubject, string msgbdy)
        {
            DataLayer objDataLayer = new DataLayer();
            string _ApplnNm = string.Empty;
            string mailFrom = string.Empty;
            string _UserId = string.Empty;
            string _Password = string.Empty;

            if (strMailSubject.Contains("License Exp")) // this is used for License module
            {
                _ApplnNm = ReturnPolmtpf("LICMAIL", "LicenseApplicationName");
                mailFrom = ReturnPolmtpf("LICMAIL", "LicenseFROM");
                _UserId = ReturnPolmtpf("LICMAIL", "LicenseDefaultEmUserId");
                _Password = ReturnPolmtpf("LICMAIL", "LicenseDefaultEmPassword");
            }
            else
            {
                _ApplnNm = ReturnPolmtpf("APCNFG", "ApplicationName");
                mailFrom = ReturnPolmtpf("MAIL", "FROM");
                _UserId = ReturnPolmtpf("APCNFG", "DefaultEmUserId");
                _Password = ReturnPolmtpf("APCNFG", "DefaultEmPassword");
            }

            string headBgColor = ReturnPolmtpf("APCNFG", "HeadBGColor");
            string headFontColor = ReturnPolmtpf("APCNFG", "HeaderFontColor");

            string mBody = GetMessageBody(_ApplnNm, msgbdy, headBgColor, headFontColor);
            string _Provider = ReturnPolmtpf("APCNFG", "Provider");
            string _constr = "";


            string _SMTP = ReturnPolmtpf("APCNFG", "DefaultSMTP");
            string _HdrBgClr = ReturnPolmtpf("APCNFG", "HeadBGColor");
            string _HdrFntClr = ReturnPolmtpf("APCNFG", "HeaderFontColor");

            bool enableSSL = bool.Parse(ReturnPolmtpf("MAIL", "ENABLESSL"));
            int port = int.Parse(ReturnPolmtpf("NET", "SMTPPORT"));
            string fpath = "";
            string sMsgTo = _EmailTo.IndexOf(";") > 0 ? _EmailTo.Substring(0, _EmailTo.IndexOf(";")) : _EmailTo;

            MailMessage msg = new MailMessage(_ApplnNm + " <" + mailFrom + ">", sMsgTo, strMailSubject, mBody);

            try
            {
                AlertEmail objEmail = new AlertEmail();
                objEmail.SendEmail_2020(_Provider, _constr, _EmailTo, _SMTP, _UserId, _Password, fpath, msg, enableSSL, port);
            }
            catch (Exception ex)
            {
                objDataLayer.WriteErrorLog("Mathods.NewMailNotification", _EmailTo, ex.ToString(), "");
            }
        }

        /* This function is used to develop the Email Message Body */
        private static string GetMessageBody(string pApplnName, string pMessage, string _headerBgColor, string _headerFontColor)
        {
            string MsgBody = pMessage;

            //MsgBody = " <html> " +
            //           " <head><style type='text/css'>.bodytext{font-family: Trebuchet MS, Segoe UI, Verdana,  Arial, sans-serif,  Helvetica;font-size: 14px;color: #000000; padding: 1px 1px 1px 2px;} .bodytextbold{font-family: Trebuchet MS, Segoe UI, Verdana,  Arial, sans-serif,  Helvetica;font-size: 12px;font-weight: bold;color: #000000;	padding: 1px 1px 1px 2px;}</style> " +
            //           " </head> " +
            //           " <body> " +
            //           "    <table width='760' cellspacing='5' cellpadding='5' border='2' style='border-collapse:collapse;' bordercolor='" + _headerBgColor + "' " +
            //           "        bgcolor='#F9F8F0' align='center'> " +
            //           "        <tr> " +
            //           "            <td bgcolor='" + _headerBgColor + "' height='80' align='left' style='font-size:1.5em;color:" + _headerFontColor + ";Padding-left: 20px;' class='bodytext'> " +
            //                            pApplnName +
            //           "            </td> " +
            //           "        </tr> " +
            //           "        <tr> " +
            //           "            <td align='left' style='font-family: Arial; font-size: 12px; color: #000000;'> " +
            //           "                <table align='center' width='700'> ";
            //MsgBody += //"                    <tr>" +
            //           //"                         <td height='30' class='bodytext'> This Email message is sent from " +
            //           //"                            <span style='font-weight:bold;'>" + pApplnName + "</span> application." +
            //           //"                         </td> " +
            //           //"                    </tr> " +
            //           //"                    <tr> " +
            //           //"                         <td height='30' class='bodytext'> " +
            //           //"                            <span >Dated on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString() + "</span> " +
            //           //"                         </td> " +
            //           //"                    </tr> " +
            //           "                    <tr> " +
            //           "                         <td height='120' class='bodytext'> " +
            //           "                            <p align='justify' style='font-weight:bold;'>  " + pMessage +
            //           "                            </p> " +
            //           "                         </td> " +
            //           "                    </tr> ";

            //MsgBody += "                    <tr> " +
            //            "                        <td height='30' class='bodytext'> " +
            //            "                            (This is an auto generated email. Please do not reply to this.) " +
            //            "                        </td> " +
            //            "                    </tr> " +
            //            "                </table> " +
            //            "            </td> " +
            //            "        </tr> " +
            //            "    </table> " +
            //            " </body> " +
            //            " </html>";
            return MsgBody;
        }

        #endregion EMail


    }
}
