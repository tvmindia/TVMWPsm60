using Newtonsoft.Json;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ServiceCallController : Controller
    {
        // GET: ServiceCall
        public ActionResult Index()
        {
            return View();
        }

        #region ServiceCall Form
        //[AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ServiceCallForm()
        {
            ServiceCallViewModel serviceCallVM = new ServiceCallViewModel();
            return PartialView("_ServiceCallForm", serviceCallVM);
        }
        #endregion ServiceCall Form

        #region ServiceCall Detail Add
        public ActionResult AddServiceCallDetail()
        {
            ServiceCallDetailViewModel serviceCallDetailVM = new ServiceCallDetailViewModel();
            serviceCallDetailVM.IsUpdate = false;
            return PartialView("_AddServiceCallDetail", serviceCallDetailVM);
        }
        #endregion ServiceCall Detail Add

        #region GetAllServiceCall
        [HttpPost]
       // [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public JsonResult GetAllServiceCall(DataTableAjaxPostModel model, ServiceCallAdvanceSearchViewModel serviceCallAdvanceSearchVM)
        {
            //setting options to our model
            serviceCallAdvanceSearchVM.DataTablePaging.Start = model.start;
            serviceCallAdvanceSearchVM.DataTablePaging.Length = (serviceCallAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : serviceCallAdvanceSearchVM.DataTablePaging.Length;

            List<ServiceCallViewModel> serviceCallVMList = new List<ServiceCallViewModel>();//Mapper.Map<List<SaleOrder>, List<SaleOrderViewModel>>(_saleOrderBusiness.(Mapper.Map<SaleOrderAdvanceSearchViewModel, SaleOrderAdvanceSearch>(saleOrderAdvanceSearchVM)));
            if (serviceCallAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = serviceCallVMList.Count != 0 ? serviceCallVMList[0].TotalCount : 0;
                int filteredResult = serviceCallVMList.Count != 0 ? serviceCallVMList[0].FilteredCount : 0;
                serviceCallVMList = serviceCallVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = serviceCallVMList.Count != 0 ? serviceCallVMList[0].TotalCount : 0,
                recordsFiltered = serviceCallVMList.Count != 0 ? serviceCallVMList[0].FilteredCount : 0,
                data = serviceCallVMList
            });
        }
        #endregion GetAllServiceCall

        #region ButtonStyling
        [HttpGet]
       // [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddServiceCall();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportServiceCallData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetServiceCallList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddServiceCall();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveServiceCall();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetServiceCall();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteServiceCall();";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailServiceCall();";

                    //toolboxVM.SendForApprovalBtn.Visible = true;
                    //toolboxVM.SendForApprovalBtn.Text = "Send";
                    //toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    //toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveServiceCall();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetServiceCall();";

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