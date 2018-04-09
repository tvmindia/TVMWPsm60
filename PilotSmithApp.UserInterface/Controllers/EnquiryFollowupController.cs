using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class EnquiryFollowupController : Controller
    {
        // GET: EnquiryFollowup
        public ActionResult Index()
        {
            EnquiryFollowupViewModel enquiryVm = new EnquiryFollowupViewModel();
            enquiryVm.TotalCount = 11;
            ViewBag.Ispager = true;
            return View(enquiryVm);
        }
    }
}