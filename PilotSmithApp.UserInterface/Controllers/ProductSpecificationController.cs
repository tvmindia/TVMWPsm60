using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class ProductSpecificationController : Controller
    {
        AppConst _appConst = new AppConst();
        private PSASysCommon _psaSysCommon = new PSASysCommon();
        private IProductSpecificationBusiness _productSpecificationBusiness;
        // GET: ProductSpecification
        #region Constructor Injection
        public ProductSpecificationController(IProductSpecificationBusiness productSpecificationBusiness)
        {
            _productSpecificationBusiness = productSpecificationBusiness;
        }
        #endregion
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductSpecificationAdvanceSearchViewModel productSpecificationAdvanceSearchVM = new ProductSpecificationAdvanceSearchViewModel();
            return View();
        }

        #region CheckProductSpecificationExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckProductSpecificationExist(ProductSpecificationViewModel productSpecificationVM)
        {
            bool exists = productSpecificationVM.IsUpdate ? false : _productSpecificationBusiness.CheckProductSpecificationExist(Mapper.Map<ProductSpecificationViewModel, ProductSpecification>(productSpecificationVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Product Specification is already in use </span> <i class='fas fa-times' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region InsertUpdateProductSpecification
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
        [HttpGet]
        public ActionResult MasterPartial(int masterCode)
        {
            ProductSpecificationViewModel productSpecificationVM = masterCode==0 ? new ProductSpecificationViewModel() : Mapper.Map<ProductSpecification, ProductSpecificationViewModel>(_productSpecificationBusiness.GetProductSpecification(masterCode));
            productSpecificationVM.IsUpdate = masterCode==0 ? false : true;
            return PartialView("_AddProductSpecification", productSpecificationVM);
        }
        #endregion

        #region GetAllProductSpecification
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
        public ActionResult ProductSpecificationSelectList(string required)
        {
            ViewBag.IsRequired = required;
            ProductSpecificationViewModel productSpecificationVM = new ProductSpecificationViewModel();
            productSpecificationVM.ProductSpecificationSelectList = _productSpecificationBusiness.GetProductSpecificationForSelectList();
            return PartialView("_ProductSpecificationSelectList", productSpecificationVM);
        }
        #endregion

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
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}