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
    public class CustomerCategoryController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private ICustomerCategoryBusiness _customerCategoryBusiness;
        // GET: CustomerCategory
        #region Constructor Injection
        public CustomerCategoryController(ICustomerCategoryBusiness customerCategoryBusiness)
        {
            _customerCategoryBusiness = customerCategoryBusiness;
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        #region CustomerCategorySelectList
        //[AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult CustomerCategorySelectList(string required)
        {
            ViewBag.IsRequired = required;
            CustomerCategoryViewModel customerCategoryVM = new CustomerCategoryViewModel();
            customerCategoryVM.CustomerCategorySelectList = _customerCategoryBusiness.GetCustomerCategoryForSelectList();
            return PartialView("_CustomerCategorySelectList", customerCategoryVM);
        }
        #endregion CustomerCategorySelectList

        #region ButtonStyling
        [HttpGet]
       // [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddCustomerCategoryMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetCustomerCategoryList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportCustomerCategoryData();";
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