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
    public class EmployeeController : Controller
    {
        AppConst _appConstant = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        IEmployeeBusiness _employeeBusiness;
        public EmployeeController(IEmployeeBusiness employeeBusiness)
        {
            _employeeBusiness = employeeBusiness;
        }
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        #region InsertUpdateEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "W")]
        public string InsertUpdateEmployee(EmployeeViewModel employeeVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                employeeVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _employeeBusiness.InsertUpdateEmployee(Mapper.Map<EmployeeViewModel, Employee>(employeeVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateEmployee
        #region GetAllEmployee
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public JsonResult GetAllEmployee(DataTableAjaxPostModel model, EmployeeAdvanceSearchViewModel employeeAdvanceSearchVM)
        {
            //setting options to our model
            employeeAdvanceSearchVM.DataTablePaging.Start = model.start;
            employeeAdvanceSearchVM.DataTablePaging.Length = (employeeAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : employeeAdvanceSearchVM.DataTablePaging.Length;

            //CustomerAdvanceSearchVM.OrderColumn = model.order[0].column;
            //CustomerAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<EmployeeViewModel> employeeVMList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetAllEmployee(Mapper.Map<EmployeeAdvanceSearchViewModel, EmployeeAdvanceSearch>(employeeAdvanceSearchVM)));
            if (employeeAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = employeeVMList.Count != 0 ? employeeVMList[0].TotalCount : 0;
                int filteredResult = employeeVMList.Count != 0 ? employeeVMList[0].FilteredCount : 0;
                employeeVMList = employeeVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = employeeVMList.Count != 0 ? employeeVMList[0].TotalCount : 0,
                recordsFiltered = employeeVMList.Count != 0 ? employeeVMList[0].FilteredCount : 0,
                data = employeeVMList
            });
        }
        #endregion GetAllEmployee
        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(Guid masterCode)
        {
            EmployeeViewModel employeeVM = masterCode == Guid.Empty ? new EmployeeViewModel() : Mapper.Map<Employee, EmployeeViewModel>(_employeeBusiness.GetEmployee(masterCode));
            employeeVM.IsUpdate = masterCode == Guid.Empty ? false : true;

            return PartialView("_EmployeeForm", employeeVM);
        }
        #endregion MasterPartial
        #region Employee SelectList
        public ActionResult EmployeeSelectList(string required)
        {
            ViewBag.IsRequired = required;
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_EmployeeSelectList", employeeVM);
        }
        #endregion Employee SelectList
        #region ResponsiblePerson SelectList
        public ActionResult ResponsiblePersonSelectList(string required)
        {
            ViewBag.IsRequired = required;
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_ResponsiblePersonSelectList", employeeVM);
        }
        #endregion ResponsiblePerson SelectList
        #region AttendedBy SelectList
        public ActionResult AttendedBySelectList(string required)
        {
            ViewBag.IsRequired = required;
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_AttendedBySelectList", employeeVM);
        }
        #endregion AttendedBy SelectList
        #region PreparedBy SelectList
        public ActionResult PreparedBySelectList(string required)
        {
            ViewBag.IsRequired = required;
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_PreparedBySelectList", employeeVM);
        }
        #endregion PreparedBy SelectList

        #region DeleteEmployee
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "D")]
        public string DeleteEmployee(Guid id)
        {
            try
            {
                var result = _employeeBusiness.DeleteEmployee(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteEmployee
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Company", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddEmployeeMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetEmployeeList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportEmployeeData();";
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