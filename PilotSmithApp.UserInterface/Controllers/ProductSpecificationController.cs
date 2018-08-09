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
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProductSpecificationController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IProductSpecificationBusiness _productSpecificationBusiness;
        SecurityFilter.ToolBarAccess _tool;
        // GET: ProductSpecification
        #region Constructor Injection
        public ProductSpecificationController(IProductSpecificationBusiness productSpecificationBusiness,SecurityFilter.ToolBarAccess tool)
        {
            _productSpecificationBusiness = productSpecificationBusiness;
            _tool = tool;
          
        }
        #endregion
        [AuthSecurityFilter(ProjectObject = "ProductSpecification", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductSpecificationAdvanceSearchViewModel productSpecificationAdvanceSearchVM = new ProductSpecificationAdvanceSearchViewModel();
            return View();
        }

        #region CheckProductSpecificationExist
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "ProductSpecification", Mode = "R")]
        public ActionResult CheckProductSpecificationExist(ProductSpecificationViewModel productSpecificationVM)
        {
            bool exists = productSpecificationVM.IsUpdate ? false : _productSpecificationBusiness.CheckProductSpecificationExist(Mapper.Map<ProductSpecificationViewModel, ProductSpecification>(productSpecificationVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Product Specification is already in use </span> <i class='fa fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region InsertUpdateProductSpecification
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "ProductSpecification", Mode = "W")]
        public string InsertUpdateProductSpecification(ProductSpecificationViewModel productSpecificationVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productSpecificationVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _productSpecificationBusiness.InsertUpdateProductSpecification(Mapper.Map<ProductSpecificationViewModel, ProductSpecification>(productSpecificationVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region MasterPartial
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "ProductSpecification", Mode = "R")]
        public ActionResult MasterPartial(int masterCode)
        {
            ProductSpecificationViewModel productSpecificationVM = masterCode==0 ? new ProductSpecificationViewModel() : Mapper.Map<ProductSpecification, ProductSpecificationViewModel>(_productSpecificationBusiness.GetProductSpecification(masterCode));
            productSpecificationVM.IsUpdate = masterCode==0 ? false : true;
            return PartialView("_AddProductSpecification", productSpecificationVM);
        }
        #endregion

        #region GetAllProductSpecification
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductSpecification", Mode = "R")]
        public JsonResult GetAllProductSpecification(DataTableAjaxPostModel model, ProductSpecificationAdvanceSearchViewModel productSpecificationAdvanceSearchVM)
        {
            productSpecificationAdvanceSearchVM.DataTablePaging.Start = model.start;
            productSpecificationAdvanceSearchVM.DataTablePaging.Length = (productSpecificationAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productSpecificationAdvanceSearchVM.DataTablePaging.Length;
            List<ProductSpecificationViewModel> productSpecificationVMList = Mapper.Map<List<ProductSpecification>, List<ProductSpecificationViewModel>>(_productSpecificationBusiness.GetAllProductSpecification(Mapper.Map<ProductSpecificationAdvanceSearchViewModel, ProductSpecificationAdvanceSearch>(productSpecificationAdvanceSearchVM)));
            if (productSpecificationAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = productSpecificationVMList.Count != 0 ? productSpecificationVMList[0].TotalCount : 0;
                int filteredResult = productSpecificationVMList.Count != 0 ? productSpecificationVMList[0].FilteredCount : 0;
                productSpecificationVMList = productSpecificationVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = productSpecificationVMList.Count != 0 ? productSpecificationVMList[0].TotalCount : 0,
                recordsFiltered = productSpecificationVMList.Count != 0 ? productSpecificationVMList[0].FilteredCount : 0,
                data = productSpecificationVMList
            });
        }
        #endregion

        #region DeleteProductSpecification
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductSpecification", Mode = "D")]
        public string DeleteProductSpecification(int code)
        {
            try
            {
                var result = _productSpecificationBusiness.DeleteProductSpecification(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region ProductSpecificationSelectList
        public ActionResult ProductSpecificationSelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "ProductSpecification");
            if (permission.SubPermissionList.Count>0)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            ProductSpecificationViewModel productSpecificationVM = new ProductSpecificationViewModel();
            productSpecificationVM.ProductSpecificationSelectList = _productSpecificationBusiness.GetProductSpecificationForSelectList();
            return PartialView("_ProductSpecificationSelectList", productSpecificationVM);
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductSpecification", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "ProductSpecification");
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductSpecificationMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetProductSpecificationList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportProductSpecificationData();";
                    //---------------------------------------
                    break;
                default:
                    return Content("Nochange");
            }
            toolboxVM = _tool.SetToolbarAccess(toolboxVM, permission);
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}