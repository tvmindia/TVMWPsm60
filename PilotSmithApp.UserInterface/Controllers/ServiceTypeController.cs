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
    public class ServiceTypeController : Controller
    {
        #region ConstructorInjection
        IServiceTypeBusiness _serviceTypeBusiness;
        public ServiceTypeController(IServiceTypeBusiness serviceTypeBusiness)
        {
            _serviceTypeBusiness = serviceTypeBusiness;
        }
        #endregion ConstructorInjection

        // GET: ServiceType
        //public ActionResult Index()
        //{
        //    return View();
        //}

        #region ServiceType SelectList
        public ActionResult ServiceTypeSelectList(string required)
        {
            ViewBag.IsRequired = required;
            ServiceTypeViewModel serviceTypeVM = new ServiceTypeViewModel();
            serviceTypeVM.ServiceTypeSelectList = _serviceTypeBusiness.GetServiceTypeSelectList();
            return PartialView("_ServiceTypeSelectList", serviceTypeVM);
        }
        #endregion ServiceType SelectList

    }
}