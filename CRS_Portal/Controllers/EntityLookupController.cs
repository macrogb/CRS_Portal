using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRS_Portal.Entity;
using CRS_Portal.Models;
using Microsoft.AspNetCore.Hosting;
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
                        objModel.entity = objEntitytDbContext.DbEntityLookup.Where(r => r.CustID == id).FirstOrDefault();
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

                    _message = string.Join("<br/>", _message1);
                    return Json(new { success = false, message = _message });

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = _message });

            }
        }
    }
}