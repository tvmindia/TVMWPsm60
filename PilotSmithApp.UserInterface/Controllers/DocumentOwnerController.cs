using AutoMapper;
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
    public class DocumentOwnerController : Controller
    {
        IUserBusiness _userBusiness;
        public DocumentOwnerController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }
        // GET: DocumentOwner
        [AuthSecurityFilter(ProjectObject = "DocumentOwner", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region Document Owner SelectList
        public ActionResult DocumentOwnerSelectList(string required)
        {
            ViewBag.IsRequired = required;
            PSAUserViewModel userVM = new PSAUserViewModel();
            userVM.UserSelectList = new List<SelectListItem>();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<PSAUserViewModel> UserVMList = Mapper.Map<List<User>, List<PSAUserViewModel>>(_userBusiness.GetAllUsers());

            if (UserVMList != null)
                foreach (PSAUserViewModel uVM in UserVMList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = uVM.UserName,
                        Value = uVM.ID.ToString(),
                        Selected = false
                    });
                }
            userVM.UserSelectList = selectListItem;
            return PartialView("_DocumentOwnerSelectList", userVM);
        }
        #endregion Document Owner SelectList

        #region Get Document Owner SelectList On Demand
        [HttpPost]
        public ActionResult GetDocumentOwnerForSelectListOnDemand(string searchTerm)
        {
            List<PSAUserViewModel> UserVMList = Mapper.Map<List<User>, List<PSAUserViewModel>>(_userBusiness.GetAllUsers());
            var list = UserVMList != null ? (from user in UserVMList.Where(x => x.UserName.ToLower().Contains(searchTerm.ToLower())).ToList()
                                                 select new Select2Model
                                                 {
                                                     text = user.UserName,
                                                     id = user.ID.ToString(),
                                                 }).ToList() : new List<Select2Model>();
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get Document Owner SelectList On Demand

    }
}