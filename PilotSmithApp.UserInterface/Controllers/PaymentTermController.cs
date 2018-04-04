using AutoMapper;
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
    public class PaymentTermController : Controller
    {
        IPaymentTermBusiness _paymentTermBusiness;
        public PaymentTermController(IPaymentTermBusiness paymentTermBusiness)
        {
            _paymentTermBusiness = paymentTermBusiness;
        }
        // GET: PaymentTerm
        [AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "W")]
        public ActionResult Index()
        {
            return View();
        }
    }
}