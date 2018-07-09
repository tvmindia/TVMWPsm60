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
    public class ProductController : Controller
    {
        #region Constructor_Injection
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProductCategoryBusiness _productCategoryBusiness;
        IProductBusiness _productBusiness;
        ICompanyBusiness _companyBusiness;
        IUserBusiness _userBusiness;
        public ProductController(IProductCategoryBusiness productCategoryBusiness, IProductBusiness productBusiness,ICompanyBusiness companyBusiness, IUserBusiness userBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
            _productBusiness = productBusiness;
            _companyBusiness = companyBusiness;
            _userBusiness = userBusiness;
        }
        #endregion Constructor_Injection
        // GET: Product
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult Index()
        {
            ProductAdvanceSearchViewModel productAdvanceSearchViewModelVM = new ProductAdvanceSearchViewModel();
            return View();
        }
        #region InsertUpdateProduct
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "W")]
        public string InsertUpdateProduct(ProductViewModel productVM)
        {
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productVM.PSASysCommon = new PSASysCommonViewModel();
                productVM.PSASysCommon.CreatedBy = appUA.UserName;
                productVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                productVM.PSASysCommon.UpdatedBy = appUA.UserName;
                productVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                result = _productBusiness.InsertUpdateProduct(Mapper.Map<ProductViewModel, Product>(productVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateProduct
        #region Product Basic Information
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult ProductBasicInfo(Guid ID)
        {
            ProductViewModel productVM = new ProductViewModel();
            if (ID != Guid.Empty)
            {
                productVM = Mapper.Map<Product, ProductViewModel>(_productBusiness.GetProduct(ID));
            }
            return PartialView("_ProductBasicInfo", productVM);
        }
        #endregion Product Basic Information
        #region CheckProductCodeExist      
        [AcceptVerbs("Get", "Post")]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult CheckProductCodeExist(ProductViewModel productVM)
        {
            bool exists = _productBusiness.CheckProductCodeExist(Mapper.Map<ProductViewModel, Product>(productVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Prouct code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion CheckProductCodeExist

        #region MasterPartial
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult MasterPartial(Guid masterCode)
        {
            ProductViewModel productVM = masterCode == Guid.Empty ? new ProductViewModel() : Mapper.Map<Product, ProductViewModel>(_productBusiness.GetProduct(masterCode));
            productVM.IsUpdate = masterCode == Guid.Empty ? false : true; 
            if (productVM.IsUpdate==false)
            {
                productVM.Code = _productBusiness.GetProductCode();
            }       
            return PartialView("_AddProduct", productVM);
        }
        #endregion MasterPartial
        # region ProductSelectList
        public ActionResult ProductSelectList(string required, bool? disabled)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _userBusiness.GetSecurityCode(appUA.UserName, "Product");
            if (permission.SubPermissionList != null)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            ProductViewModel productVM = new ProductViewModel();
            productVM.ProductSelectList = _productBusiness.GetProductForSelectList();
            return PartialView("_ProductSelectList", productVM);
        }
        #endregion ProductSelectList

        #region GetAllProduct
        [HttpPost]        
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public JsonResult GetAllProduct(DataTableAjaxPostModel model, ProductAdvanceSearchViewModel productAdvanceSearchVM)
        {
            productAdvanceSearchVM.DataTablePaging.Start = model.start;
            productAdvanceSearchVM.DataTablePaging.Length = (productAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productAdvanceSearchVM.DataTablePaging.Length;
            List<ProductViewModel> productVMList = Mapper.Map<List<Product>, List<ProductViewModel>>(_productBusiness.GetAllProduct(Mapper.Map<ProductAdvanceSearchViewModel, ProductAdvanceSearch>(productAdvanceSearchVM)));
            if (productAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = productVMList.Count != 0 ? productVMList[0].TotalCount : 0;
                int filteredResult = productVMList.Count != 0 ? productVMList[0].FilteredCount : 0;
                productVMList = productVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = productVMList.Count != 0 ? productVMList[0].TotalCount : 0,
                recordsFiltered = productVMList.Count != 0 ? productVMList[0].FilteredCount : 0,
                data = productVMList
            });
        }
        #endregion GetAllProduct

        #region DeleteProduct
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "D")]
        public string DeleteProduct(Guid id)
        {
            try
            {
                var result = _productBusiness.DeleteProduct(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteProduct

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetProductList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportProductData();";
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