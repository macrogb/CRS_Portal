using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CRS_Portal.Entity;
using CRS_Portal.HelperMethods;
using CRS_Portal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRS_Portal.Controllers
{
    public class TransactionDetailsController : Controller
    {
        TransactionDetailsDbContext objTransactionDetailsDbContext;
        CurrencyDBContext objCurrencyDBContext;

        string _message = string.Empty;
        private IHostingEnvironment _hostingEnv;
        public TransactionDetailsController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }
        public IActionResult Index()
        {
            TransactionDetailsModel objModel = new TransactionDetailsModel();
            objModel.TranDetails = new TransactionDetails();
            using (objCurrencyDBContext = new CurrencyDBContext())
            {
                objModel.lstCurrency = objCurrencyDBContext.DbCurrency.AsEnumerable().ToList();

            }
            return View(objModel);
        }

        public IActionResult LoadTransactionDetails()
        {

            try
            {
                using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                {
                    var _lstmodel = objTransactionDetailsDbContext.DbTransactionDetails.Select(p => new TransactionDetailsSummary()
                    {
                        ID = p.ID,
                        ActBranch = p.ActBranch,
                        ActNo = p.ActNo,
                        SortCode = p.SortCode,
                        IBAN = p.IBAN,
                        ActBal = p.ActBal

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
        public IActionResult DeleteAllTranDetails()
        {
            try
            {
                using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                {
                    List<TransactionDetails> lsttransactionDetails = objTransactionDetailsDbContext.DbTransactionDetails.Where(o => true).ToList();
                    objTransactionDetailsDbContext.RemoveRange(lsttransactionDetails);
                    objTransactionDetailsDbContext.SaveChanges();

                }
                _message = "All Status codes has been deleted";
                return Json(new { success = true, message = _message });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        //TranDetailsFileUpload
        [HttpPost]
        public async Task<IActionResult> TranDetailsFileUpload(IList<IFormFile> files)
        {
            try
            {
                bool isCountinue = false;
                if (files.Count == 1)
                {
                    isCountinue = true;
                }

                if (isCountinue)
                {
                    string filePath = string.Empty;
                    for (int i = 0; i < files.Count; i++)
                    {
                        string filename = ContentDispositionHeaderValue.Parse(files[i].ContentDisposition).FileName.ToString().Trim('"');
                        filename = Helper.EnsureCorrectFilename(filename);

                        using (FileStream output = System.IO.File.Create(Helper.GetPathAndFilename(filename, "TranDetails", _hostingEnv)))
                        {
                            await files[i].CopyToAsync(output);
                            filePath = Path.GetDirectoryName(output.Name);
                            output.Dispose();
                        }
                        _message = BulkTranDetailsUpload(filePath + @"\" + filename, HttpContext.Session.GetString("UserID"));
                        if (_message == "false")
                        {
                            _message = "Uploaded Account status code file has incorrect format! Please upload correct file and try again.";
                            //Log.WriteEventLogwithParam("AccountStatusCode", "AcctStsCodeFileUpload", "Post", "Start", _message, _userID);
                            return Json(new { success = false, message = _message });
                        }
                    }
                    _message = "Transaction details has been imported";
                    //Log.WriteEventLogwithParam("AccountStatusCode", "AcctStsCodeFileUpload", "Post", "End", _message, _userID);
                    return Json(new { success = true, message = _message });
                }
                else
                {
                    _message = "Imported Status code file has incorrect format! Please upload correct file and try again.";
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

        [HttpPost]
        public IActionResult DeleteTranDetails(string id)
        {
            //_userID = HttpContext.Session.GetString("UsrID_BankName");
            //Log.WriteEventLogwithParam("AccountStatusCode", "DeleteAcctStsCodeDetails", "Post", "Start", "DeleteAcctStsCodeDetails method called by User", _userID);
            try
            {
                using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                {
                    var modelFromDb = objTransactionDetailsDbContext.DbTransactionDetails.Where(r => r.ID == int.Parse(id)).FirstOrDefault();
                    if (modelFromDb != null)
                    {
                        objTransactionDetailsDbContext.Remove(modelFromDb);
                        objTransactionDetailsDbContext.SaveChanges();
                    }
                    _message = "'" + modelFromDb.ActNo + "' - Transaction detail has been deleted";
                    //Log.WriteEventLogwithParam("AccountStatusCode", "DeleteAcctStsCodeDetails", "Post", "End", _message, _userID);
                    return Json(new { success = true, message = _message });
                }
            }
            catch (Exception ex)
            {
                //Log.WriteEventErrorLogwithParam("AccountStatusCode", "DeleteAcctStsCodeDetails", "Post", "Error", "Error Occurred", _userID, ex);
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        private string BulkTranDetailsUpload(string fileNamewithPath, string _userID)
        {
            try
            {
                DataTable dtContent = Helper.GetDataTableFromExcel(fileNamewithPath);
                string query = string.Empty;
                List<TransactionDetails> objlstTransactionDetails = new List<TransactionDetails>();
                foreach (DataRow item in dtContent.Rows)
                {
                    if (item.ItemArray.Length == 24)
                    {
                        TransactionDetails objTranDetails = new TransactionDetails();
                        objTranDetails.ActBranch = item.ItemArray[0].ToString();
                        objTranDetails.ActNo = item.ItemArray[1].ToString();
                        objTranDetails.SortCode = item.ItemArray[2].ToString();
                        objTranDetails.IBAN = item.ItemArray[3].ToString();
                        objTranDetails.CurrCode = item.ItemArray[4].ToString();
                        objTranDetails.ActBal = item.ItemArray[5].ToString();
                        objTranDetails.IntAmt = item.ItemArray[6].ToString();
                        objTranDetails.PaymentCode = item.ItemArray[7].ToString();
                        objTranDetails.ActClsInd = item.ItemArray[8].ToString();
                        objTranDetails.ActClsDt = item.ItemArray[9].ToString();
                        objTranDetails.ActOpenDt = item.ItemArray[10].ToString();
                        objTranDetails.UndocumentedActInd = item.ItemArray[11].ToString();
                        objTranDetails.DorActInd = item.ItemArray[12].ToString();
                        objTranDetails.CustType = item.ItemArray[13].ToString();
                        objTranDetails.ActType = item.ItemArray[14].ToString();
                        objTranDetails.PCUSTID = item.ItemArray[15].ToString();
                        objTranDetails.J1CUSTID = item.ItemArray[16].ToString();
                        objTranDetails.J2CUSTID = item.ItemArray[17].ToString();
                        objTranDetails.J3CUSTID = item.ItemArray[18].ToString();
                        objTranDetails.J4CUSTID = item.ItemArray[19].ToString();
                        objTranDetails.J5CUSTID = item.ItemArray[20].ToString();
                        objTranDetails.J6CUSTID = item.ItemArray[21].ToString();
                        objTranDetails.J7CUSTID = item.ItemArray[22].ToString();
                        objTranDetails.J8CUSTID = item.ItemArray[23].ToString();
                        objlstTransactionDetails.Add(objTranDetails);

                        if(objlstTransactionDetails.Count  == 1000)
                        {
                            using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                            {
                                objTransactionDetailsDbContext.DbTransactionDetails.AddRange(objlstTransactionDetails);
                                objTransactionDetailsDbContext.SaveChanges();
                            }
                            objlstTransactionDetails = new List<TransactionDetails>();
                        }
                    }
                }
                if(objlstTransactionDetails.Count > 0)
                {
                    using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                    {
                        objTransactionDetailsDbContext.DbTransactionDetails.AddRange(objlstTransactionDetails);
                        objTransactionDetailsDbContext.SaveChanges();
                    }
                }
                return "true";
            }
            catch(Exception ex)
            {
                return "false";
            }
        }

        [HttpGet]
        public ActionResult LoadTranDetailsByID(string id)
        {
            try
            {
                TransactionDetailsModel objModel = new TransactionDetailsModel();
                objModel.TranDetails = new TransactionDetails();
                using (objCurrencyDBContext = new CurrencyDBContext())
                {
                    objModel.lstCurrency = objCurrencyDBContext.DbCurrency.AsEnumerable().ToList();

                }
                if (!string.IsNullOrEmpty(id))
                {
                    using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                    {
                        objModel.TranDetails = objTransactionDetailsDbContext.DbTransactionDetails.Where(r => r.ID == int.Parse(id)).FirstOrDefault();
                    }
                }
                
                return PartialView("AddTransaction", objModel);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        [HttpPost]
        public ActionResult SaveTranDetails(TransactionDetailsModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.TranDetails.ID == null)
                    {
                        //Add
                        using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                        {
                            objTransactionDetailsDbContext.DbTransactionDetails.Add(model.TranDetails);
                            objTransactionDetailsDbContext.SaveChanges();
                            _message = "'" + model.TranDetails.ActNo + "' - Transaction Detail added successfully";
                        }
                    }
                    else
                    {
                        //Edit
                        using (objTransactionDetailsDbContext = new TransactionDetailsDbContext())
                        {
                            var modelFromDb = objTransactionDetailsDbContext.DbTransactionDetails.Where(r => r.ID == model.TranDetails.ID).FirstOrDefault();
                            modelFromDb.ActBranch = model.TranDetails.ActBranch;
                            modelFromDb.ActNo = model.TranDetails.ActNo;
                            modelFromDb.SortCode = model.TranDetails.SortCode;
                            modelFromDb.IBAN = model.TranDetails.IBAN;
                            modelFromDb.CurrCode = model.TranDetails.CurrCode;
                            modelFromDb.ActBal = model.TranDetails.ActBal;
                            modelFromDb.IntAmt = model.TranDetails.IntAmt;
                            modelFromDb.PaymentCode = model.TranDetails.PaymentCode;
                            modelFromDb.ActClsInd = model.TranDetails.ActClsInd;
                            modelFromDb.ActClsDt = model.TranDetails.ActClsDt;
                            modelFromDb.ActOpenDt = model.TranDetails.ActOpenDt;
                            modelFromDb.UndocumentedActInd = model.TranDetails.UndocumentedActInd;
                            modelFromDb.DorActInd = model.TranDetails.DorActInd;
                            modelFromDb.CustType = model.TranDetails.CustType;
                            modelFromDb.ActType = model.TranDetails.ActType;
                            modelFromDb.PCUSTID = model.TranDetails.PCUSTID;
                            modelFromDb.J1CUSTID = model.TranDetails.J1CUSTID;
                            modelFromDb.J2CUSTID = model.TranDetails.J2CUSTID;
                            modelFromDb.J3CUSTID = model.TranDetails.J3CUSTID;
                            modelFromDb.J4CUSTID = model.TranDetails.J4CUSTID;
                            modelFromDb.J5CUSTID = model.TranDetails.J5CUSTID;
                            modelFromDb.J6CUSTID = model.TranDetails.J6CUSTID;
                            modelFromDb.J7CUSTID = model.TranDetails.J7CUSTID;
                            modelFromDb.J8CUSTID = model.TranDetails.J8CUSTID;

                            objTransactionDetailsDbContext.DbTransactionDetails.Update(modelFromDb);
                            objTransactionDetailsDbContext.SaveChanges();
                            _message = "'" + model.TranDetails.ActNo + "' - Transaction Detail updated successfully";
                        }
                    }
                    return Json(new { success = true, message = _message });
                }
                else
                {
                    List<string> fieldOrder = new List<string>(new string[] {
                                 "TransactionDetails" })
                         .Select(f => f.ToLower()).ToList();

                    var _message1 = ModelState
                        .Select(m => new { Order = fieldOrder.IndexOf(m.Key.ToLower()), Error = m.Value })
                        .OrderBy(m => m.Order)
                        .SelectMany(m => m.Error.Errors.Select(e => e.ErrorMessage)).ToList();

                    _message = string.Join("<br/>", _message1);
                    return Json(new { success = false, message = _message });

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = _message });

            }
        }

    }
}