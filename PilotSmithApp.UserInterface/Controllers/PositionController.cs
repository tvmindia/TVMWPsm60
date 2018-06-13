using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class PositionController : Controller
    {
        IPositionBusiness _positionBusiness;
        public PositionController(IPositionBusiness positionBusiness)
        {
            _positionBusiness = positionBusiness;
        }
        // GET: Position
        public ActionResult Index()
        {
            return View();
        }

        #region Get Position SelectList On Demand
        [HttpPost]
        public ActionResult GetPositionForSelectListOnDemand(string searchTerm)
        {
            List<SelectListItem> positionSelectList = _positionBusiness.GetPositionSelectList();
            var list = positionSelectList != null ? (from SelectListItem in positionSelectList.Where(x => x.Text.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                       select new Select2Model
                                                       {
                                                           text = SelectListItem.Text,
                                                           id = SelectListItem.Value,
                                                       }).ToList() : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get Position SelectList On Demand
    }
}