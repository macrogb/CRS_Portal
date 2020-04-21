using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRS_Portal.Entity;
using CRS_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRS_Portal.Controllers
{
    public class EntityLookupController : Controller
    {
        EntityLookupDbContext objEntitytDbContext;

        public EntityLookupController()
        {
            
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
    }
}