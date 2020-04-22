using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRS_Portal.Entity;
using CRS_Portal.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using CRS_Portal.HelperMethods;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Hosting;

namespace CRS_Portal.Controllers
{
    public class ClosedAccountDetailsController : Controller
    {
        ClosedAccountDetailsDbContext objClosedAccountDetailsDbContext;
        string _message = string.Empty;
        private IHostingEnvironment _hostingEnv;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadClosedAccountDetails()
        {
            try
            {
                using (objClosedAccountDetailsDbContext = new ClosedAccountDetailsDbContext())
                {
                    var _lstmodel = objClosedAccountDetailsDbContext.DbClosedAccountDetails.Select(p => new ClosedAccountDetails()
                    {
                        ID = p.ID,
                        DealNo = p.DealNo,
                        DealDt = p.DealDt,
                        CustNo = p.CustNo,
                        CustName = p.CustName,
                        PrincipalAmt = p.PrincipalAmt,
                        InterestAmtPaid = p.InterestAmtPaid
                    }).ToList();

                    var sa = new JsonSerializerSettings();
                    return Json(_lstmodel, sa);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        [HttpPost]
        public IActionResult DeleteClosedAccountDetails(string id)
        {
            try
            {
                using (objClosedAccountDetailsDbContext = new ClosedAccountDetailsDbContext())
                {
                    var modelFromDb = objClosedAccountDetailsDbContext.DbClosedAccountDetails.Where(r => r.ID == int.Parse(id)).FirstOrDefault();
                    if (modelFromDb != null)
                    {
                        objClosedAccountDetailsDbContext.Remove(modelFromDb);
                        objClosedAccountDetailsDbContext.SaveChanges();
                    }
                    _message = "'" + modelFromDb.CustNo + "' - closed account detail has been deleted";
                    return Json(new { success = true, message = _message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        [HttpPost]
        public IActionResult DeleteAllClosedAccountDetails()
        {
            try
            {
                using (objClosedAccountDetailsDbContext = new ClosedAccountDetailsDbContext())
                {
                    List<ClosedAccountDetails> lstClosedAccountDetails = objClosedAccountDetailsDbContext.DbClosedAccountDetails.Where(o => true).ToList();
                    objClosedAccountDetailsDbContext.RemoveRange(lstClosedAccountDetails);
                    objClosedAccountDetailsDbContext.SaveChanges();
                }
                _message = "All closed account details has been deleted";
                return Json(new { success = true, message = _message });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClosedAccountDetailsFileUpload(IList<IFormFile> files)
        {
            try
            {
                bool isContinue = false;
                if (files.Count == 1)
                {
                    isContinue = true;
                }

                if (isContinue)
                {
                    string filePath = string.Empty;
                    for (int i = 0; i < files.Count; i++)
                    {
                        string filename = ContentDispositionHeaderValue.Parse(files[i].ContentDisposition).FileName.ToString().Trim('"');
                        filename = Helper.EnsureCorrectFilename(filename);

                        using (FileStream output = System.IO.File.Create(Helper.GetPathAndFilename(filename, "ClosedAccountDetails", _hostingEnv)))
                        {
                            await files[i].CopyToAsync(output);
                            filePath = Path.GetDirectoryName(output.Name);
                            output.Dispose();
                        }
                        _message = BulkClosedAccountDetailsUpload(filePath + @"\" + filename, HttpContext.Session.GetString("UserID"));
                        if (_message == "false")
                        {
                            _message = "Uploaded closed account details file has incorrect format! Please upload correct file and try again.";
                            //Log.WriteEventLogwithParam("AccountStatusCode", "AcctStsCodeFileUpload", "Post", "Start", _message, _userID);
                            return Json(new { success = false, message = _message });
                        }
                    }
                    _message = "Closed account details has been imported";
                    //Log.WriteEventLogwithParam("AccountStatusCode", "AcctStsCodeFileUpload", "Post", "End", _message, _userID);
                    return Json(new { success = true, message = _message });
                }
                else
                {
                    _message = "Imported closed account details file has incorrect format! Please upload correct file and try again.";
                    //Log.WriteEventLogwithParam("AccountStatusCode", "AcctStsCodeFileUpload", "Post", "End", _message, _userID);
                    return Json(new { success = false, message = _message });
                }
            }
            catch (Exception ex)
            {
                //Log.WriteEventErrorLogwithParam("AccountStatusCode", "AcctStsCodeFileUpload", "Post", "Error", "Error Occurred", _userID, ex);
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        private string BulkClosedAccountDetailsUpload(string fileNamewithPath, string _userID)
        {
            try
            {
                DataTable dtContent = Helper.GetDataTableFromExcel(fileNamewithPath);
                string query = string.Empty;
                List<ClosedAccountDetails> objlstClosedAccountDetails = new List<ClosedAccountDetails>();
                foreach (DataRow item in dtContent.Rows)
                {
                    if (item.ItemArray.Length == 11)
                    {
                        ClosedAccountDetails objClosedAccountDetails = new ClosedAccountDetails();
                        objClosedAccountDetails.DealNo = item.ItemArray[0].ToString();
                        objClosedAccountDetails.DealDt = item.ItemArray[1].ToString();
                        objClosedAccountDetails.ValueDt = item.ItemArray[2].ToString();
                        objClosedAccountDetails.MaturityDt = item.ItemArray[3].ToString();
                        objClosedAccountDetails.CustNo = item.ItemArray[4].ToString();
                        objClosedAccountDetails.CustName = item.ItemArray[5].ToString();
                        objClosedAccountDetails.PrincipalAmt = item.ItemArray[6].ToString();
                        objClosedAccountDetails.Currency = item.ItemArray[7].ToString();
                        objClosedAccountDetails.InterestAmtPaid = item.ItemArray[8].ToString();
                        objClosedAccountDetails.ProdType = item.ItemArray[9].ToString();
                        objClosedAccountDetails.PaymentCode = item.ItemArray[10].ToString();
                        objlstClosedAccountDetails.Add(objClosedAccountDetails);

                        if (objlstClosedAccountDetails.Count == 1000)
                        {
                            using (objClosedAccountDetailsDbContext = new ClosedAccountDetailsDbContext())
                            {
                                objClosedAccountDetailsDbContext.DbClosedAccountDetails.AddRange(objlstClosedAccountDetails);
                                objClosedAccountDetailsDbContext.SaveChanges();
                            }
                            objlstClosedAccountDetails = new List<ClosedAccountDetails>();
                        }
                    }
                }
                if (objlstClosedAccountDetails.Count > 0)
                {
                    using (objClosedAccountDetailsDbContext = new ClosedAccountDetailsDbContext())
                    {
                        objClosedAccountDetailsDbContext.DbClosedAccountDetails.AddRange(objlstClosedAccountDetails);
                        objClosedAccountDetailsDbContext.SaveChanges();
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }
    }
}