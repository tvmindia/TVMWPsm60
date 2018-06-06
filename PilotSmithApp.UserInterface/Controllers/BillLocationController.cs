using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class BillLocationController : Controller
    {

        IBillLocationBusiness _billLocationBusiness;
        public BillLocationController(IBillLocationBusiness billLocationBusiness)
        {
            _billLocationBusiness = billLocationBusiness;
        }
        // GET: BillLocation
        public ActionResult Index()
        {
            return View();
        }

        #region BillLocation SelectList
        public ActionResult BillLocationSelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            BillLocationViewModel billLocationVM = new BillLocationViewModel();
            billLocationVM.BillLocationList = _billLocationBusiness.GetBillLocationForSelectList();
            return PartialView("_BillLocationSelectList", billLocationVM);
        }
        #endregion BillLocation SelectList
    }
}