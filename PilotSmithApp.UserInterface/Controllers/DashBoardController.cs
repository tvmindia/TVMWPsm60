using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        [AuthSecurityFilter(ProjectObject = "DashBoard", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
    }
}