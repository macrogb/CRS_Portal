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
        private IHostingEnvironment _hostingEnv;

        public EntityLookupController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public IActionResult Index()
        {
            EntityLookupModel objModel = new EntityLookupModel();
            objModel.entity = new EntityLookupDetailDto();
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
                if(!string.IsNullOrEmpty(id))
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
    }
}