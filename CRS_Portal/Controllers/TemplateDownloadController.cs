using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRS_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRS_Portal.Controllers
{
    public class TemplateDownloadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadTemplateGrid()
        {
            try
            {
                List<TemplateDownloadModel> lstModel = new List<TemplateDownloadModel>();

                TemplateDownloadModel t1Model = new TemplateDownloadModel();
                //t1Model.ID = 1;
                //t1Model.IsActive = true;
                //t1Model.Title = "Individual Lookup";
                //t1Model.Description = "Download Individual Lookup file";
                //t1Model.


                var sa = new JsonSerializerSettings();
                return Json(lstModel, sa);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}