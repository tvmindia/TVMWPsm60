using AutoMapper;
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
    public class SaleOrderController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ISaleOrderBusiness _saleOrderBusiness;

        #region Constructor Injection
        public SaleOrderController(ISaleOrderBusiness saleOrderBusiness)
        {
            _saleOrderBusiness = saleOrderBusiness;
        }
        #endregion Constructor Injection
        // GET: SaleOrder
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region SaleOrderForm Form
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult SaleOrderForm()
        {
            SaleOrderViewModel saleOrderVM = new SaleOrderViewModel();        
            return PartialView("_SaleOrderForm", saleOrderVM);
        }
        #endregion SaleOrderForm Form

        #region SaleOrder Detail Add
        public ActionResult AddSaleOrderDetail()
        {
            SaleOrderDetailViewModel saleOrderDetailVM = new SaleOrderDetailViewModel();
            saleOrderDetailVM.IsUpdate = false;
            return PartialView("_AddSaleOrderDetail", saleOrderDetailVM);
        }
        #endregion SaleOrder Detail Add

        #region GetAllSaleOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public JsonResult GetAllSaleOrder(DataTableAjaxPostModel model, SaleOrderAdvanceSearchViewModel saleOrderAdvanceSearchVM)
        {
            //setting options to our model
            saleOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            saleOrderAdvanceSearchVM.DataTablePaging.Length = (saleOrderAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : saleOrderAdvanceSearchVM.DataTablePaging.Length;

            List<SaleOrderViewModel> saleOrderVMList = new List<SaleOrderViewModel>();//Mapper.Map<List<SaleOrder>, List<SaleOrderViewModel>>(_saleOrderBusiness.(Mapper.Map<SaleOrderAdvanceSearchViewModel, SaleOrderAdvanceSearch>(saleOrderAdvanceSearchVM)));
            if (saleOrderAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = saleOrderVMList.Count != 0 ? saleOrderVMList[0].TotalCount : 0;
                int filteredResult = saleOrderVMList.Count != 0 ? saleOrderVMList[0].FilteredCount : 0;
                saleOrderVMList = saleOrderVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = saleOrderVMList.Count != 0 ? saleOrderVMList[0].TotalCount : 0,
                recordsFiltered = saleOrderVMList.Count != 0 ? saleOrderVMList[0].FilteredCount : 0,
                data = saleOrderVMList
            });
        }
        #endregion GetAllSaleOrder
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSaleOrder();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportSaleOrderData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleOrderList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSaleOrder();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveSaleOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleOrder();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteSaleOrder();";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailSaleOrder();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveSaleOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleOrder();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}