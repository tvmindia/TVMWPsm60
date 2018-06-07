using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
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
        IUserBusiness _userBusiness;
        IEmployeeBusiness _employeeBusiness;
        IDepartmentBusiness _departmentBusiness;
        IPositionBusiness _positionBusiness;

        public EmployeeController(IEmployeeBusiness employeeBusiness, IUserBusiness userBusiness, IDepartmentBusiness departmentBusiness, IPositionBusiness positionBusiness)
        {
            _employeeBusiness = employeeBusiness;
            _userBusiness = userBusiness;
            _departmentBusiness = departmentBusiness;
            _positionBusiness =positionBusiness;
        }
        // GET: Employee
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
        public ActionResult Index()
        {
            EmployeeAdvanceSearchViewModel employeeAdvanceSearchVM = new EmployeeAdvanceSearchViewModel();
            employeeAdvanceSearchVM.Department = new DepartmentViewModel();
            employeeAdvanceSearchVM.Department.DepartmentSelectList = _departmentBusiness.GetDepartmentSelectList();
            employeeAdvanceSearchVM.Position = new PositionViewModel();
            employeeAdvanceSearchVM.Position.PositionSelectList = _positionBusiness.GetPositionSelectList();
            return View(employeeAdvanceSearchVM);
        }
        #region InsertUpdateEmployee
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "W")]
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
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
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
            if (masterCode == Guid.Empty)
            {
                employeeVM.IsActive = true;
            }
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
        #region ServicedBy SelectList
        public ActionResult ServicedBySelectList(string required)
        {
            ViewBag.IsRequired = required;
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_ServicedBySelectList", employeeVM);
        }
        #endregion ServicedBy SelectList
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
        #region QCBy SelectList
        public ActionResult QCBySelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "Employee");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_QCBySelectList", employeeVM);
        }
        #endregion QCBy SelectList
        #region DeleteEmployee
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "D")]
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

        #region CheckEmployeeCodeExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
        public ActionResult CheckEmployeeCodeExist(EmployeeViewModel employeeVM)
        {
            bool exists = _employeeBusiness.CheckEmployeeCodeExist(Mapper.Map<EmployeeViewModel, Employee>(employeeVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Employee Code already is in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
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