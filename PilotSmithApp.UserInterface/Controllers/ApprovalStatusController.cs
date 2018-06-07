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
        #region Get ApprovalStatus SelectList On Demand
        [HttpPost]
        public ActionResult GetApprovalStatusSelectListOnDemand(string searchTerm)
        {
            List<SelectListItem> approvalStatusSelectList = _approvalStatusBusiness.GetSelectListForApprovalStatus();
            var list = approvalStatusSelectList != null ? (from SelectListItem in approvalStatusSelectList.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                     select new Select2Model
                                                     {
                                                         text = SelectListItem.Text,
                                                         id = SelectListItem.Value,
                                                     }).ToList() : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get ApprovalStatus SelectList On Demand
    }
}