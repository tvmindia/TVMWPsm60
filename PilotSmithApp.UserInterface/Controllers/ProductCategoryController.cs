﻿using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.BusinessService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PilotSmithApp.UserInterface.Models;
using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
using SAMTool.BusinessServices.Contracts;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProductCategoryController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IProductCategoryBusiness _productCategoryBusiness;
        SecurityFilter.ToolBarAccess _tool;
        // GET: ProductCategory
        #region Constructor Injection
        public ProductCategoryController(IProductCategoryBusiness productCategoryBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _productCategoryBusiness = productCategoryBusiness;
            _tool = tool;
        }
        #endregion
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductCategoryAdvanceSearchViewModel productCategoryAvanceSearchVM = new ProductCategoryAdvanceSearchViewModel();
            return View(productCategoryAvanceSearchVM);
        }

        #region CheckProductCategoryExist        
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult CheckProductCategoryExist(ProductCategoryViewModel productCategoryVM)
        {
            bool exists = productCategoryVM.IsUpdate ? false : _productCategoryBusiness.CheckProductCategoryExist(Mapper.Map<ProductCategoryViewModel,ProductCategory>(productCategoryVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Product Category is already in use </span> <i class='fa fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region InsertUpdateProductCategory
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "W")]
        public string InsertUpdateProductCategory(ProductCategoryViewModel productCategoryVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productCategoryVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _productCategoryBusiness.InsertUpdateProductCategory(Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVM));
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
        //[AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult MasterPartial(int masterCode)
        {
            ProductCategoryViewModel productCategoryVM = masterCode==0 ? new ProductCategoryViewModel() : Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryBusiness.GetProductCategory(masterCode));
            productCategoryVM.IsUpdate = masterCode==0 ? false : true;
            return PartialView("_AddProductCategory", productCategoryVM);
        }
        #endregion

        #region GetAllProductCategory
        [HttpPost]      
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public JsonResult GetAllProductCategory(DataTableAjaxPostModel model,ProductCategoryAdvanceSearchViewModel productCategoryAdvanceSearchVM)
        {
            productCategoryAdvanceSearchVM.DataTablePaging.Start = model.start;
            productCategoryAdvanceSearchVM.DataTablePaging.Length = (productCategoryAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productCategoryAdvanceSearchVM.DataTablePaging.Length;
            List<ProductCategoryViewModel>productCategoryVMList= Mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(_productCategoryBusiness.GetAllProductCategory(Mapper.Map<ProductCategoryAdvanceSearchViewModel, ProductCategoryAdvanceSearch>(productCategoryAdvanceSearchVM)));
            if (productCategoryAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = productCategoryVMList.Count != 0 ? productCategoryVMList[0].TotalCount : 0;
                int filteredResult = productCategoryVMList.Count != 0 ? productCategoryVMList[0].FilteredCount : 0;
                productCategoryVMList = productCategoryVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = productCategoryVMList.Count != 0 ? productCategoryVMList[0].TotalCount : 0,
                recordsFiltered = productCategoryVMList.Count != 0 ? productCategoryVMList[0].FilteredCount : 0,
                data = productCategoryVMList
            });
        }
        #endregion

        #region DeleteProductCategory
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "D")]
        public string DeleteProductCategory(int code)
        {
            try
            {
                var result = _productCategoryBusiness.DeleteProductCategory(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        # region ProductCategorySelectList
        public ActionResult ProductCategorySelectList(string required,bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "ProductCategory");
            if (permission.SubPermissionList.Count>0)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            ProductCategoryViewModel productCategoryVM = new ProductCategoryViewModel();
            productCategoryVM.ProductCategorySelectList = _productCategoryBusiness.GetProductCategoryForSelectList();
            return PartialView("_ProductCategorySelectList", productCategoryVM);
        }
        #endregion ProductCategorySelectList

        //#region ProductCategoryDropDown
        //public ActionResult ProductCategoryDropDown(ProductCategoryViewModel productCategoryVM)
        //{
        //    productCategoryVM.ProductCategoryCode = productCategoryVM.Code;
        //    List<SelectListItem> selectListItem = new List<SelectListItem>();
        //    productCategoryVM.SelectList = new List<SelectListItem>();
        //    List<ProductCategoryViewModel> productCategoryList = Mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(_productCategoryBusiness.GetProductCategoryForSelectList());
        //    if(productCategoryList!=null)
        //        foreach(ProductCategoryViewModel productCategory in productCategoryList)
        //        {
        //            selectListItem.Add(new SelectListItem
        //            {
        //                Text = productCategory.Description,
        //                Value = productCategory.Code.ToString(),
        //                Selected = false
        //            });  
        //        }
        //    productCategoryVM.SelectList = selectListItem;
        //    return PartialView("_ProductCategoryDropDown", productCategoryVM);
        //}
        //#endregion

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _psaSysCommon.GetSecurityCode(appUA.UserName, "ProductCategory");
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductCategoryMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetProductCategoryList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportProductCategoryData();";
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