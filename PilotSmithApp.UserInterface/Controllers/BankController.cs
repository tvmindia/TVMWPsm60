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
    public class BankController : Controller
    {
        #region Global Declaration
        private IBankBusiness _bankBusiness;
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        #endregion Global Declaration
        #region Construction Injection
        public BankController(IBankBusiness bankBusiness)
        {
            _bankBusiness = bankBusiness;
        }
        #endregion Construction Injection
        // GET: Bank
        #region Index
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            BankAdvanceSearchViewModel bankAdvanceSearchVM = new BankAdvanceSearchViewModel();
            return View(bankAdvanceSearchVM);
        }
        #endregion Index
        #region CheckBankCodeExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckCodeExist(BankViewModel bankVM)
        {
            bool exists = bankVM.IsUpdate ? false : _bankBusiness.CheckCodeExist(bankVM.Code);
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Bank code already in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion CheckBankCodeExist
        #region GetAllBank
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public JsonResult GetAllBank(DataTableAjaxPostModel model, BankAdvanceSearchViewModel bankAdvanceSearchVM)
        {
            //setting options to our model
            bankAdvanceSearchVM.DataTablePaging.Start = model.start;
            bankAdvanceSearchVM.DataTablePaging.Length = (bankAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : bankAdvanceSearchVM.DataTablePaging.Length;

            //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
            //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<BankViewModel> bankVMList = Mapper.Map<List<Bank>, List<BankViewModel>>(_bankBusiness.GetAllBank(Mapper.Map<BankAdvanceSearchViewModel, BankAdvanceSearch>(bankAdvanceSearchVM)));
            if (bankAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = bankVMList.Count != 0 ? bankVMList[0].TotalCount : 0;
                int filteredResult = bankVMList.Count != 0 ? bankVMList[0].FilteredCount : 0;
                bankVMList = bankVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = bankVMList.Count != 0 ? bankVMList[0].TotalCount : 0,
                recordsFiltered = bankVMList.Count != 0 ? bankVMList[0].FilteredCount : 0,
                data = bankVMList
            });
        }
        #endregion GetAllBank
        #region InsertUpdateBank
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public string InsertUpdateBank(BankViewModel bankVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                bankVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _bankBusiness.InsertUpdateBank(Mapper.Map<BankViewModel, Bank>(bankVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }
        #endregion InsertUpdateBank
        #region DeleteBank
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public string DeleteBank(string code)
        {
            try
            {
                var result = _bankBusiness.DeleteBank(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteBank
        #region BankSelctList
        public ActionResult BankSelectList(string required)
        {
            ViewBag.IsRequired = required;
            BankViewModel bankVM = new BankViewModel();
            bankVM.BankCode = bankVM.Code;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            bankVM.SelectList = new List<SelectListItem>();
            bankVM.SelectList = _bankBusiness.GetBankForSelectList();
            return PartialView("_BankSelectList", bankVM);
        }
        #endregion BankSelectList
        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            BankViewModel bankVM = string.IsNullOrEmpty(masterCode) ? new BankViewModel() : Mapper.Map<Bank, BankViewModel>(_bankBusiness.GetBank(masterCode));
            bankVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddBankPartial", bankVM);
        }

        #endregion
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddBankMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetBankList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportBankData();";
                    //---------------------------------------
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}