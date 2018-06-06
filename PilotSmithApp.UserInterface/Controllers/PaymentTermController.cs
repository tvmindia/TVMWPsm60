﻿using AutoMapper;
using Newtonsoft.Json;
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
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IPaymentTermBusiness _paymentTermBusiness;
        public PaymentTermController(IPaymentTermBusiness paymentTermBusiness)
        {
            _paymentTermBusiness = paymentTermBusiness;
        }
        // GET: PaymentTerm
        [AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "W")]
        public ActionResult Index()
        {
            PaymentTermAdvanceSearchViewModel paymentTermAdvanceSearchVM = new PaymentTermAdvanceSearchViewModel();
            return View();
        }
        #region InsertUpdatePaymentTerm
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "W")]
        public string InsertUpdatePaymentTerm(PaymentTermViewModel paymentTermVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                paymentTermVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _paymentTermBusiness.InsertUpdatePaymentTerm(Mapper.Map<PaymentTermViewModel, PaymentTerm>(paymentTermVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region CheckPaymentTermCodeExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "R")]
        public ActionResult CheckPaymentTermCodeExist(PaymentTermViewModel paymentTermVM)
        {
            bool exists = _paymentTermBusiness.CheckPaymentTermNameExist(Mapper.Map<PaymentTermViewModel, PaymentTerm>(paymentTermVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Code already is in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MasterPartial
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            PaymentTermViewModel paymentTermVM = masterCode == "" ? new PaymentTermViewModel() : Mapper.Map<PaymentTerm, PaymentTermViewModel>(_paymentTermBusiness.GetPaymentTerm(masterCode));
            paymentTermVM.IsUpdate = masterCode == "" ? false : true;
            return PartialView("_AddPaymentTerm", paymentTermVM);
        }
        #endregion MasterPartial

        #region GetAllPaymentTerm
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "R")]
        public JsonResult GetAllPaymentTerm(DataTableAjaxPostModel model, PaymentTermAdvanceSearchViewModel paymentTermAdvanceSearchVM)
        {
            paymentTermAdvanceSearchVM.DataTablePaging.Start = model.start;
            paymentTermAdvanceSearchVM.DataTablePaging.Length = (paymentTermAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : paymentTermAdvanceSearchVM.DataTablePaging.Length;
            List<PaymentTermViewModel> paymentTermVMList = Mapper.Map<List<PaymentTerm>, List<PaymentTermViewModel>>(_paymentTermBusiness.GetAllPayTerm(Mapper.Map<PaymentTermAdvanceSearchViewModel, PaymentTermAdvanceSearch>(paymentTermAdvanceSearchVM)));
            if (paymentTermAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = paymentTermVMList.Count != 0 ? paymentTermVMList[0].TotalCount : 0;
                int filteredResult = paymentTermVMList.Count != 0 ? paymentTermVMList[0].FilteredCount : 0;
                paymentTermVMList = paymentTermVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = paymentTermVMList.Count != 0 ? paymentTermVMList[0].TotalCount : 0,
                recordsFiltered = paymentTermVMList.Count != 0 ? paymentTermVMList[0].FilteredCount : 0,
                data = paymentTermVMList
            });
        }
        #endregion

        #region DeletePaymentTerm
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "D")]
        public string DeletePaymentTerm(string code)
        {
            try
            {
                var result = _paymentTermBusiness.DeletePaymentTerm(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region PaymentTerm SelectList
        public ActionResult PaymentTermSelectList(string required)
        {
            ViewBag.IsRequired = required;
            PaymentTermViewModel paymentTermVM = new PaymentTermViewModel();
            paymentTermVM.PaymentTermSelectList = _paymentTermBusiness.GetPaymentTermForSelectList(); 
            return PartialView("_PaymentTermSelectList", paymentTermVM);
        }
        #endregion PaymentTerm SelectList

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PaymentTerm", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddPaymentTermMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPaymentTermList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportPaymentTermData();";
                    //---------------------------------------
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion ButtonStyling

    }
}