using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DocumentStatusController : Controller
    {
        IDocumentStatusBusiness _documentStatusBusiness;
        public DocumentStatusController(IDocumentStatusBusiness documentStatusBusiness)
        {
            _documentStatusBusiness = documentStatusBusiness;
        }
        // GET: DocumentStatus
        [AuthSecurityFilter(ProjectObject = "DocumentStatus", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region DocumentStatus SelectList
        public ActionResult DocumentStatusSelectList(string required,string code)
        {
            ViewBag.IsRequired = required;
            DocumentStatusViewModel documentStatusVM = new DocumentStatusViewModel();
            documentStatusVM.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus(code);
            return PartialView("_DocumentStatusSelectList", documentStatusVM);
        }
        #endregion DocumentStatus SelectList

        #region ServiceStatus SelectList
        public ActionResult ServiceStatusSelectList(string required, string code)
        {
            ViewBag.IsRequired = required;
            DocumentStatusViewModel documentStatusVM = new DocumentStatusViewModel();
            documentStatusVM.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus(code);
            return PartialView("_ServiceStatusSelectList", documentStatusVM);
        }
        #endregion ServiceStatus SelectList

    }
}