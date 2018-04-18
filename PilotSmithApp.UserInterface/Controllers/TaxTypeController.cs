using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class TaxTypeController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private ITaxTypeBusiness _taxTypeBusiness;
        #region Constructor Injection
        public TaxTypeController(ITaxTypeBusiness taxTypeBusiness)
        {
            _taxTypeBusiness = taxTypeBusiness;
        }
        #endregion
        // GET: TaxType
        public ActionResult Index()
        {
            return View();
        }
        #region TaxType SelectList
        [AuthSecurityFilter(ProjectObject = "TaxType", Mode = "R")]
        public ActionResult TaxTypeSelectList(string required)
        {
            ViewBag.IsRequired = required;
            TaxTypeViewModel taxTypeVM = new TaxTypeViewModel();
            taxTypeVM.TaxTypeSelectList = _taxTypeBusiness.GetTaxTypeForSelectList();
            return PartialView("_TaxTypeSelectList", taxTypeVM);
        }
        #endregion TaxType SelectList
    }
}