﻿using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.BusinessService.Contracts;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class DynamicUIController : Controller
    {
        // GET: DynamicUI
        private IDynamicUIBusiness _dynamicUIBusiness;
        public DynamicUIController(IDynamicUIBusiness dynamicUIBusiness)
        {
            _dynamicUIBusiness = dynamicUIBusiness;

        }

        public ActionResult _MenuNavBar()
        {
            List<PSASysMenu> menulist = _dynamicUIBusiness.GetAllMenues();
            DynamicUIViewModel dUIObj = new DynamicUIViewModel();
            dUIObj.PSASSysMenuViewModelList = Mapper.Map<List<PSASysMenu>, List<PSASysMenuViewModel>>(menulist);
            return View(dUIObj);
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UnderConstruction() {
            return View();
        }
        public ActionResult PopUpUnderConstruction()
        {
            return PartialView("PopUpUnderConstruction");
        }

    }
}