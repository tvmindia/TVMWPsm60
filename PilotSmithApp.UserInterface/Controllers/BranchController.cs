﻿using PilotSmithApp.BusinessService.Contract;
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
    public class BranchController : Controller
    {
        IBranchBusiness _branchBusiness;
        public BranchController(IBranchBusiness branchBusiness)
        {
            _branchBusiness = branchBusiness;
        }
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }
        #region Branch SelectList
        [AuthSecurityFilter(ProjectObject = "Branch", Mode = "R")]
        public ActionResult BranchSelectList(string required)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            ViewBag.IsRequired = required;
            BranchViewModel branchVM = new BranchViewModel();
            branchVM.BranchList = _branchBusiness.GetBranchForSelectList(appUA.UserName);
            return PartialView("_BranchSelectList", branchVM);
        }
        #endregion Branch SelectList
    }
}