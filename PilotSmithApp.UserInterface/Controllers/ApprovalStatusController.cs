using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PilotSmithApp.UserInterface.Controllers
{
    public class ApprovalStatusController : Controller
    {
        IApprovalStatusBusiness _approvalStatusBusiness;
        public ApprovalStatusController(IApprovalStatusBusiness approvalStatusBusiness)
        {
            _approvalStatusBusiness = approvalStatusBusiness;
        }


        // GET: ApprovalStatus
        public ActionResult Index()
        {
            return View();
        }
    }
}