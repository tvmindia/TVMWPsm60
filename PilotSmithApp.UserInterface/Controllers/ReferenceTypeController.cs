using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ReferenceTypeController : Controller
    {
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IReferenceTypeBusiness _referenceTypeBusiness;
        IUserBusiness _userBusiness;
        public ReferenceTypeController(IReferenceTypeBusiness referenceTypeBusiness, IUserBusiness userBusiness)
        {
            _referenceTypeBusiness = referenceTypeBusiness;
            _userBusiness = userBusiness;
        }
        // GET: ReferenceType
        [AuthSecurityFilter(ProjectObject = "ReferenceType", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region ReferenceType SelectList
        public ActionResult ReferenceTypeSelectList(string required)
        {
            ViewBag.IsRequired = required;
            ReferenceTypeViewModel referenceTypeVM = new ReferenceTypeViewModel();
            referenceTypeVM.ReferenceTypeSelectList = _referenceTypeBusiness.GetReferenceTypeSelectList();
            return PartialView("_ReferenceTypeSelectList", referenceTypeVM);
        }
        #endregion

    }
}