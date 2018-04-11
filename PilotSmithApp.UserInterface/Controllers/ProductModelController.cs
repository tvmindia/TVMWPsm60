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
    public class ProductModelController : Controller
    {
        #region Constructor_Injection
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProductModelBusiness _productModelBusiness;
        IProductBusiness _productBusiness;
        public ProductModelController(IProductModelBusiness productModelBusiness,IProductBusiness productBusiness)
        {
            _productModelBusiness = productModelBusiness;  
            _productBusiness = productBusiness;           
        }
        #endregion Constructor_Injection
        // GET: ProductModel
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductModelAdvanceSearchViewModel productModelAdvanceSearchVM = new ProductModelAdvanceSearchViewModel();
            return View();
        }

        #region InsertUpdateProductModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AuthSecurityFilter(ProjectObject = "Customers", Mode = "W")]
        public string InsertUpdateProductModel(ProductModelViewModel productModelVM)
        {
            object result = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productModelVM.PSASysCommon = new PSASysCommonViewModel();
                productModelVM.PSASysCommon.CreatedBy = appUA.UserName;
                productModelVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                productModelVM.PSASysCommon.UpdatedBy = appUA.UserName;
                productModelVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                result = _productModelBusiness.InsertUpdateProductModel(Mapper.Map<ProductModelViewModel, ProductModel>(productModelVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateProductModel

        #region CheckProductModelNameExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckProductModelNameExist(ProductModelViewModel productModelVM)
        {
            bool exists = _productModelBusiness.CheckProductModelNameExist(Mapper.Map<ProductModelViewModel, ProductModel>(productModelVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Prouct Model name is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion CheckProductModelName

        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(Guid masterCode)
        {
            ProductModelViewModel productModelVM = masterCode == Guid.Empty ? new ProductModelViewModel() : Mapper.Map<ProductModel, ProductModelViewModel>(_productModelBusiness.GetProductModel(masterCode));
            productModelVM.IsUpdate = masterCode == Guid.Empty ? false : true;
            return PartialView("_AddProductModel", productModelVM);
        }
        #endregion MasterPartial

        # region ProductModelSelectList
        public ActionResult ProductModelSelectList(string required)
        {
            ViewBag.IsRequired = required;
            ProductModelViewModel productModelVM = new ProductModelViewModel();
            productModelVM.ProductModelSelectList = _productModelBusiness.GetProductModelForSelectList();
            return PartialView("_ProductModelSelectList", productModelVM);
        }
        #endregion ProductModelSelectList

        #region GetAllProductModel
        public JsonResult GetAllProductModel(DataTableAjaxPostModel model, ProductModelAdvanceSearchViewModel productModelAdvanceSearchVM)
        {
            productModelAdvanceSearchVM.DataTablePaging.Start = model.start;
            productModelAdvanceSearchVM.DataTablePaging.Length = (productModelAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productModelAdvanceSearchVM.DataTablePaging.Length;
           List<ProductModelViewModel> productModelVMList = Mapper.Map<List<ProductModel>, List<ProductModelViewModel>>(_productModelBusiness.GetAllProductModel(Mapper.Map<ProductModelAdvanceSearchViewModel, ProductModelAdvanceSearch>(productModelAdvanceSearchVM)));
            if (productModelAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = productModelVMList.Count != 0 ? productModelVMList[0].TotalCount : 0;
                int filteredResult = productModelVMList.Count != 0 ? productModelVMList[0].FilteredCount : 0;
                productModelVMList = productModelVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = productModelVMList.Count != 0 ? productModelVMList[0].TotalCount : 0,
                recordsFiltered = productModelVMList.Count != 0 ? productModelVMList[0].FilteredCount : 0,
                data = productModelVMList
            });
        }
        #endregion GetAllProduct

        #region DeleteProductModel
        public string DeleteProductModel(Guid id)
        {
            try
            {
                var result = _productModelBusiness.DeleteProductModel(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteProductModel

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
                    toolboxVM.addbtn.Event = "AddProductModelMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetProductModelList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportProductModelData();";
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