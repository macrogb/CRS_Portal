using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CRS_Portal.Controllers
{
    public class AuditHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}