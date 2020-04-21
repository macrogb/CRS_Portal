using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CRS_Portal.Entity;

namespace CRS_Portal.Controllers
{
    public class InitiateCRSController : Controller
    {
        EntityLookupDbContext entityLookupDbContext;
        string ErrorDesc;
        public IActionResult Index()
        {
            HttpContext.Session.SetString("BankName", "CANARA");

            return View();
        }

        public IActionResult TriggerAutomation()
        {
            return null;
        }

        private bool ValidateInputData()
        {
            /**Validating Entity Lookup**/

            entityLookupDbContext = new EntityLookupDbContext();
            var objentity=entityLookupDbContext.DbEntityLookup.FirstOrDefault(e => e.CREATEDDT.Contains(DateTime.Now.Year.ToString()));
            if (entityLookupDbContext.DbEntityLookup.Count() <= 0)
            {
                ErrorDesc = "The Entity Customer Details Missing";
            }
            if (objentity == null)
            {
                ErrorDesc = "The Entity Customer details seems old data.Please upload the " + (DateTime.Now.Year - 1).ToString() + " year entity customer details";
                return false;
            }


            return true;

        }
    }
}