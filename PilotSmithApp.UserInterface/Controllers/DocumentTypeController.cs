using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
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
    public class DocumentTypeController : Controller
    {
        #region Constructor_Injection 

        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IDocumentTypeBusiness _documentTypeBusiness;
        public DocumentTypeController(IDocumentTypeBusiness documentTypeBusiness)
        {
            _documentTypeBusiness = documentTypeBusiness;
        }
        #endregion Constructor_Injection
        // GET: DocumentType
        [AuthSecurityFilter(ProjectObject = "DocumentType", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region DocumentType SelectList
        public ActionResult DocumentTypeSelectList(string required)
        {
            ViewBag.IsRequired = required;
            DocumentTypeViewModel documentTypeVM = new DocumentTypeViewModel();
            documentTypeVM.DocumentTypeSelectList = _documentTypeBusiness.GetDocumentTypeSelectList();
            return PartialView("_DocumentTypeSelectList", documentTypeVM);
        }
        #endregion DocumentType SelectList
    }
}