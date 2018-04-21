using AutoMapper;
using PilotSmithApp.UserInterface.Models;
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
            List<PSAUserViewModel> UserVMList= Mapper.Map<List<User>, List<PSAUserViewModel>>(_userBusiness.GetAllUsers());
           
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
    }
}