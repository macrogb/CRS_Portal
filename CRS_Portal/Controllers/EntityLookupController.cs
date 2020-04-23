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
    public class EntityLookupController : Controller
    {
        EntityLookupDbContext objEntitytDbContext;
        CountryDbContext objCountryDbContext;
        private IHostingEnvironment _hostingEnv;
        string _message = string.Empty;

        public EntityLookupController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public IActionResult Index()
        {
            EntityLookupModel objModel = new EntityLookupModel();
            objModel.entity = new EntityLookupDetailDto();
            using (objCountryDbContext = new CountryDbContext())
            {
                objModel.lstcountry = objCountryDbContext.DbCountry.AsEnumerable().ToList();

            }
            return View(objModel);
        }

        public ActionResult LoadEntityLookupDetails()
        {
            
            try
            {
                using (objEntitytDbContext = new EntityLookupDbContext())
                {
                    var _lstmodel = objEntitytDbContext.DbEntityLookup.Select(p => new EntityLookupSummaryDto()
                    {
                        ID = p.ID,
                        EntityID = p.CustID,
                        EntityName = p.EntityName,
                        EntityType = p.AccHoldType
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

        public ActionResult DownloadCRSEntityCustomerTemplate()
        {
            try
            {
                string path = _hostingEnv.WebRootPath + "\\TemplateFiles\\CRS_Entity_Lookup_Template.xlsx";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                string fileName = "CRS_Entity_Lookup_Template.xlsx";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return File("", System.Net.Mime.MediaTypeNames.Application.Octet, "");
            }
        }

        [HttpGet]
        public ActionResult LoadEntityDetailsByID(string id)
        {
            try
            {
                EntityLookupModel objModel = new EntityLookupModel();
                objModel.entity = new EntityLookupDetailDto();
                using (objCountryDbContext = new CountryDbContext())
                {
                    objModel.lstcountry = objCountryDbContext.DbCountry.AsEnumerable().ToList();

                }

                if (!string.IsNullOrEmpty(id))
                {
                    using (objEntitytDbContext = new EntityLookupDbContext())
                    {
                        objModel.entity = objEntitytDbContext.DbEntityLookup.Where(r => r.ID == long.Parse(id)).FirstOrDefault();
                    }
                }
                return PartialView("AddEntity", objModel);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        [HttpPost]
        public ActionResult SaveEntityDetails(EntityLookupModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(model.entity.ID==null)
                    {
                        //Add
                        using (objEntitytDbContext = new EntityLookupDbContext())
                        {
                            //model.entity.CreatedBy = long.Parse(HttpContext.Session.GetString("UserID"));
                            model.entity.CreatedDT = DateTime.Today.ToString("yyyyMMdd");
                            model.entity.CreatedTM = DateTime.Now.ToString("HHmmss");

                            objEntitytDbContext.DbEntityLookup.Add(model.entity);
                            objEntitytDbContext.SaveChanges();
                            _message = "'" + model.entity.EntityName + "' - Entity added successfully";
                        }
                    }
                    else
                    {
                        //Edit
                        using (objEntitytDbContext = new EntityLookupDbContext())
                        {
                            var modelFromDb = objEntitytDbContext.DbEntityLookup.Where(r => r.ID == model.entity.ID).FirstOrDefault();

                            modelFromDb.EntityName = model.entity.EntityName;
                            modelFromDb.BuildingIdentifier = model.entity.BuildingIdentifier;
                            modelFromDb.StreetName = model.entity.StreetName;
                            modelFromDb.DistrictName = model.entity.DistrictName;
                            modelFromDb.City = model.entity.City;
                            modelFromDb.PostCode = model.entity.PostCode;
                            modelFromDb.CountryCode = model.entity.CountryCode;
                            modelFromDb.EmailID = model.entity.EmailID;
                            modelFromDb.AccHoldType = model.entity.AccHoldType;
                            modelFromDb.ResCountryCode = model.entity.ResCountryCode;
                            modelFromDb.PCountryIdentityType = model.entity.PCountryIdentityType;
                            modelFromDb.PCountryIdentityNo = model.entity.PCountryIdentityNo;
                            modelFromDb.PIdentityIssuedBy = model.entity.PIdentityIssuedBy;
                            modelFromDb.secondResCountryCode = model.entity.secondResCountryCode;
                            modelFromDb.SecondCountryIdentityType = model.entity.SecondCountryIdentityType;
                            modelFromDb.SecondCountryIdentityNo = model.entity.SecondCountryIdentityNo;
                            modelFromDb.SecondIdentityIssuedBy = model.entity.SecondIdentityIssuedBy;
                            modelFromDb.ThirdResCountryCode = model.entity.ThirdResCountryCode;
                            modelFromDb.ThirdCountryIdentityType = model.entity.ThirdCountryIdentityType;
                            modelFromDb.ThirdCountryIdentityNo = model.entity.ThirdCountryIdentityNo;
                            modelFromDb.ThirdIdentityIssuedBy = model.entity.ThirdIdentityIssuedBy;

                            //modelFromDb.ModifiedBY = long.Parse(HttpContext.Session.GetString("UserID"));
                            modelFromDb.ModifiedDT = DateTime.Today.ToString("yyyyMMdd");
                            modelFromDb.ModifiedTM = DateTime.Now.ToString("HHmmss");

                            objEntitytDbContext.DbEntityLookup.Update(modelFromDb);
                            objEntitytDbContext.SaveChanges();
                            _message = "'" + model.entity.EntityName + "' - Entity updated successfully";
                        }
                    }
                    return Json(new { success = true, message = _message });
                }
                else
                {
                    List<string> fieldOrder = new List<string>(new string[] {
                                 "EntityLookup" })
                         .Select(f => f.ToLower()).ToList();

                    var _message1 = ModelState
                        .Select(m => new { Order = fieldOrder.IndexOf(m.Key.ToLower()), Error = m.Value })
                        .OrderBy(m => m.Order)
                        .SelectMany(m => m.Error.Errors.Select(e => e.ErrorMessage)).ToList();

                    _message = string.Join("<br/>", _message1.Distinct());
                    return Json(new { success = false, message = _message });

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = _message });

            }
        }

        [HttpPost]
        public IActionResult DeleteEntityDetails(string id)
        {
            try
            {
                using (objEntitytDbContext = new EntityLookupDbContext())
                {
                    var modelFromDb = objEntitytDbContext.DbEntityLookup.Where(r => r.ID == long.Parse(id)).FirstOrDefault();
                    if (modelFromDb != null)
                    {
                        objEntitytDbContext.Remove(modelFromDb);
                        objEntitytDbContext.SaveChanges();
                    }
                    _message = "'" + modelFromDb.EntityName + "' - Entity has been deleted";
                    return Json(new { success = true, message = _message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }

        [HttpPost]
        public IActionResult DeleteAllEntityDetails()
        {
            try
            {
                using (objEntitytDbContext = new EntityLookupDbContext())
                {
                    List<EntityLookupDetailDto> lstentityDetails = objEntitytDbContext.DbEntityLookup.Where(o => true).ToList();
                    objEntitytDbContext.RemoveRange(lstentityDetails);
                    objEntitytDbContext.SaveChanges();

                }
                _message = "All entity records has been deleted";
                return Json(new { success = true, message = _message });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "The server has encountered an unexpected internal error. Please try again later." });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EntityDetailsFileUpload(IList<IFormFile> files)
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

                        using (FileStream output = System.IO.File.Create(Helper.GetPathAndFilename(filename, "EntityDetails", _hostingEnv)))
                        {
                            await files[i].CopyToAsync(output);
                            filePath = Path.GetDirectoryName(output.Name);
                            output.Dispose();
                        }
                        _message = BulkEntityDetailsUpload(filePath + @"\" + filename, HttpContext.Session.GetString("UserID"));
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

        private string BulkEntityDetailsUpload(string fileNamewithPath, string _userID)
        {
            try
            {
                DataTable dtContent = Helper.GetDataTableFromExcel(fileNamewithPath);
                string query = string.Empty;
                List<EntityLookupDetailDto> objlstEntityDetails = new List<EntityLookupDetailDto>();
                foreach (DataRow item in dtContent.Rows)
                {
                    if (item.ItemArray.Length == 22)
                    {
                        EntityLookupDetailDto objEntityDetails = new EntityLookupDetailDto();
                        objEntityDetails.CustID = item.ItemArray[0].ToString();
                        objEntityDetails.EntityName = item.ItemArray[1].ToString();
                        objEntityDetails.BuildingIdentifier = item.ItemArray[2].ToString();
                        objEntityDetails.StreetName = item.ItemArray[3].ToString();
                        objEntityDetails.DistrictName = item.ItemArray[4].ToString();
                        objEntityDetails.City = item.ItemArray[5].ToString();
                        objEntityDetails.PostCode = item.ItemArray[6].ToString();
                        objEntityDetails.CountryCode = item.ItemArray[7].ToString();
                        objEntityDetails.EmailID = item.ItemArray[8].ToString();
                        objEntityDetails.AccHoldType = item.ItemArray[9].ToString();
                        objEntityDetails.ResCountryCode = item.ItemArray[10].ToString();
                        objEntityDetails.PCountryIdentityType = item.ItemArray[11].ToString();
                        objEntityDetails.PCountryIdentityNo = item.ItemArray[12].ToString();
                        objEntityDetails.PIdentityIssuedBy = item.ItemArray[13].ToString();
                        objEntityDetails.secondResCountryCode = item.ItemArray[14].ToString();
                        objEntityDetails.SecondCountryIdentityType = item.ItemArray[15].ToString();
                        objEntityDetails.SecondCountryIdentityNo = item.ItemArray[16].ToString();
                        objEntityDetails.SecondIdentityIssuedBy = item.ItemArray[17].ToString();
                        objEntityDetails.ThirdResCountryCode = item.ItemArray[18].ToString();
                        objEntityDetails.ThirdCountryIdentityType = item.ItemArray[19].ToString();
                        objEntityDetails.ThirdCountryIdentityNo = item.ItemArray[20].ToString();
                        objEntityDetails.ThirdIdentityIssuedBy = item.ItemArray[21].ToString();
                       
                        objlstEntityDetails.Add(objEntityDetails);

                        if (objlstEntityDetails.Count == 1000)
                        {
                            using (objEntitytDbContext = new EntityLookupDbContext())
                            {
                                objEntitytDbContext.DbEntityLookup.AddRange(objlstEntityDetails);
                                objEntitytDbContext.SaveChanges();
                            }
                            objlstEntityDetails = new List<EntityLookupDetailDto>();
                        }
                    }
                    else
                    {
                        //To Do - Import correct records and inform few records are not in correct format
                        return "false";
                    }
                }
                if (objlstEntityDetails.Count > 0)
                {
                    using (objEntitytDbContext = new EntityLookupDbContext())
                    {
                        objEntitytDbContext.DbEntityLookup.AddRange(objlstEntityDetails);
                        objEntitytDbContext.SaveChanges();
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