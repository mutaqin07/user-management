using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Controllers
{
    public class DashboardController : Controller
    {
        public DashboardController()
        {
            
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
