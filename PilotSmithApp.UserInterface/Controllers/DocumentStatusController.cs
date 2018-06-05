using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class DocumentStatusController : Controller
    {
        IDocumentStatusBusiness _documentStatusBusiness;
        public DocumentStatusController(IDocumentStatusBusiness documentStatusBusiness)
        {
            _documentStatusBusiness = documentStatusBusiness;
        }
        // GET: DocumentStatus
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

    }
}