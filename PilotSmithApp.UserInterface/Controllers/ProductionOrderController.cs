﻿using Newtonsoft.Json;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ProductionOrderController : Controller
    {
        // GET: ProductOrder
        [AuthSecurityFilter(ProjectObject = "ProductOrder", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region ProductionOrderForm Form
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ProductionOrderForm()
        {
            ProductionOrderViewModel productionOrderVM = new ProductionOrderViewModel();
            return PartialView("_ProductionOrderForm", productionOrderVM);
        }
        #endregion ProductionOrderForm Form

        #region ProductionOrder Detail Add
        public ActionResult AddSaleOrderDetail()
        {
            ProductionOrderDetailViewModel productionOrderDetailVM = new ProductionOrderDetailViewModel();
            productionOrderDetailVM.IsUpdate = false;
            return PartialView("_AddProductionOrderDetail", productionOrderDetailVM);
        }
        #endregion ProductionOrder Detail Add

        #region GetAllProductionOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public JsonResult GetAllProductionOrder(DataTableAjaxPostModel model, ProductionOrderAdvanceSearchViewModel productionOrderAdvanceSearchVM)
        {
            //setting options to our model
            productionOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            productionOrderAdvanceSearchVM.DataTablePaging.Length = (productionOrderAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productionOrderAdvanceSearchVM.DataTablePaging.Length;

            List<ProductionOrderViewModel> productionOrderVMList = new List<ProductionOrderViewModel>();//Mapper.Map<List<SaleOrder>, List<SaleOrderViewModel>>(_saleOrderBusiness.(Mapper.Map<SaleOrderAdvanceSearchViewModel, SaleOrderAdvanceSearch>(saleOrderAdvanceSearchVM)));
            if (productionOrderAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = productionOrderVMList.Count != 0 ? productionOrderVMList[0].TotalCount : 0;
                int filteredResult = productionOrderVMList.Count != 0 ? productionOrderVMList[0].FilteredCount : 0;
                productionOrderVMList = productionOrderVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = productionOrderVMList.Count != 0 ? productionOrderVMList[0].TotalCount : 0,
                recordsFiltered = productionOrderVMList.Count != 0 ? productionOrderVMList[0].FilteredCount : 0,
                data = productionOrderVMList
            });
        }
        #endregion GetAllProductionOrder

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportProductionOrderData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionOrderList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProductionOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionOrder();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteProductionOrder();";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProductionOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionOrder();";

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