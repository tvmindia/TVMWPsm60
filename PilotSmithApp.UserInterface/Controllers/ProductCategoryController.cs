using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.BusinessService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PilotSmithApp.UserInterface.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ProductCategoryController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IProductCategoryBusiness _productCategoryBusiness;
        // GET: ProductCategory
        #region Constructor Injection
        public ProductCategoryController(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }
        #endregion
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductCategoryAdvanceSearchViewModel productCategoryAvanceSearchVM = new ProductCategoryAdvanceSearchViewModel();
            return View(productCategoryAvanceSearchVM);
        }

        #region CheckProductCategoryCodeExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckProductCategoryCodeExist(ProductCategoryViewModel productCategoryVM)
        {
            bool exists = productCategoryVM.IsUpdate ? false : _productCategoryBusiness.CheckProductCategoryCodeExist(productCategoryVM.Code);
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Product Category Code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region InsertUpdateProductCategory
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
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion

        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(string masterCode)
        {
            ProductCategoryViewModel productCategoryVM = masterCode=="0" ? new ProductCategoryViewModel() : Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryBusiness.GetProductCategory(int.Parse(masterCode)));
            productCategoryVM.IsUpdate = masterCode=="0" ? false : true;
            return PartialView("_AddProductCategory", productCategoryVM);
        }
        #endregion

        #region GetAllProductCategory
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
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
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
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}