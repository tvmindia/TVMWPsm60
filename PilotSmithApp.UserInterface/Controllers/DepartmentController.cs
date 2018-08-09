using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DepartmentController : Controller
    {
        IDepartmentBusiness _departmentBusiness;
        public DepartmentController(IDepartmentBusiness departmentBusiness)
        {
            _departmentBusiness = departmentBusiness;
        }
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }
        #region Get Department SelectList On Demand
        [HttpPost]
        public ActionResult GetDepartmentForSelectListOnDemand(string searchTerm)
        {
            List<SelectListItem> departmentSelectList = _departmentBusiness.GetDepartmentSelectList();
            var list = departmentSelectList != null ? (from SelectListItem in departmentSelectList.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                 select new Select2Model
                                                 {
                                                     text = SelectListItem.Text,
                                                     id = SelectListItem.Value,
                                                 }).ToList() : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get Department SelectList On Demand
    }
}