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
            return View();
        }

        public ActionResult LoadEntityLookupDetails()
        {
            
            try
            {
                using (objEntitytDbContext = new EntityLookupDbContext())
                {
                    var _lstmodel = objEntitytDbContext.DbEntityLookup.Select(p => new EntityLookupSummaryDto()
                    {
                        EntityID = p.CUSTID,
                        EntityName = p.ENTITYNAME,
                        EntityType = p.ACCHOLDTYPE
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
    }
}