using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ReferencePersonController : Controller
    {
        IReferencePersonBusiness _referencePersonBusiness; 
        public ReferencePersonController(IReferencePersonBusiness referencePersonBusiness)
        {
            _referencePersonBusiness = referencePersonBusiness;
        }
        // GET: ReferencePerson
        public ActionResult Index()
        {
            return View();
        }
        #region ReferencePerson SelectList
        public ActionResult ReferencePersonSelectList(string required)
        {
            ViewBag.IsRequired = required;
            ReferencePersonViewModel referencePersonVM = new ReferencePersonViewModel();
            referencePersonVM.ReferencePersonSelectList = _referencePersonBusiness.GetReferencePersonSelectList();
            return PartialView("_ReferencePersonSelectList", referencePersonVM);
        }
        #endregion ReferencePerson SelectList
    }
}