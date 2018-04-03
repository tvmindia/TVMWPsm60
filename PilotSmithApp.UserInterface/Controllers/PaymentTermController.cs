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
        #region ProductDropdown
        public ActionResult PaymentTermDropdown(PaymentTermViewModel paymentTermVM)
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            paymentTermVM.SelectList = new List<SelectListItem>();
            List<PaymentTermViewModel> paymentTermList = Mapper.Map<List<PaymentTerm>, List<PaymentTermViewModel>>(_paymentTermBusiness.GetPaymentTermForSelectList());
            if (paymentTermList != null)
                foreach (PaymentTermViewModel paymentTerm in paymentTermList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = paymentTerm.Code +" || "+ paymentTerm.Description,
                        Value = paymentTerm.Code.ToString(),
                        Selected = false
                    });
                }
            paymentTermVM.SelectList = selectListItem;
            ViewBag.PostingProperty = "PaymentTermCode";
            ViewBag.SelectList = selectListItem;
            return PartialView("_PaymentTermDropdown", paymentTermVM);
        }
        #endregion ProductDropdown
    }
}