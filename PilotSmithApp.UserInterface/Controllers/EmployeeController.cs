﻿using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class EmployeeController : Controller
    {
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
    }
}