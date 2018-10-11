using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.BusinessService.Contracts;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProductModelController : Controller
    {
        #region Constructor_Injection
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProductModelBusiness _productModelBusiness;
        IProductBusiness _productBusiness;
        //IUserBusiness _userBusiness;
        IFileUploadBusiness _fileUploadBusiness;
        SecurityFilter.ToolBarAccess _tool;
        public ProductModelController(IProductModelBusiness productModelBusiness,IProductBusiness productBusiness,
            IFileUploadBusiness fileUploadBusiness,SecurityFilter.ToolBarAccess tool)//, IUserBusiness userBusiness
        {
            _productModelBusiness = productModelBusiness;  
            _productBusiness = productBusiness;
            //_userBusiness = userBusiness;
            _fileUploadBusiness = fileUploadBusiness;
            _tool = tool;
        }
        #endregion Constructor_Injection
        // GET: ProductModel
        [AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductModelAdvanceSearchViewModel productModelAdvanceSearchVM = new ProductModelAdvanceSearchViewModel();
            return View();
        }

        #region InsertUpdateProductModel
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "W")]
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
        [AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "R")]
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
        //[HttpGet]
        //[AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "R")]
        public ActionResult MasterPartial(Guid masterCode)
        {
            ProductModelViewModel productModelVM = masterCode == Guid.Empty ? new ProductModelViewModel() : Mapper.Map<ProductModel, ProductModelViewModel>(_productModelBusiness.GetProductModel(masterCode));
            productModelVM.IsUpdate = masterCode == Guid.Empty ? false : true;
            return PartialView("_AddProductModel", productModelVM);
        }
        #endregion MasterPartial
        #region ProductModelSelectList
        public ActionResult ProductModelSelectList(string required, bool? disabled, Guid productID)
        {
            ViewBag.IsRequired = required;
            ViewBag.IsDisabled = disabled;
            ViewBag.HasAddPermission = false;
            ViewBag.propertydisable = disabled == null ? false : disabled;
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductModel");
            if (permission.SubPermissionList.Count>0)
            {
                if (permission.SubPermissionList.First(s => s.Name == "SelectListAddButton").AccessCode.Contains("R"))
                {
                    ViewBag.HasAddPermission = true;
                }
            }
            ProductModelViewModel productModelVM = new ProductModelViewModel();
            productModelVM.ProductModelSelectList = _productModelBusiness.GetProductModelForSelectList(productID);
            return PartialView("_ProductModelSelectList", productModelVM);
        }
        #endregion ProductModelSelectList
        #region ProductModel Basic Information
        //[AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "R")]
        public ActionResult ProductModelBasicInfo(Guid ID)
        {
            ProductModelViewModel productModelVM = new ProductModelViewModel();
            if (ID != Guid.Empty)
            {
                productModelVM = Mapper.Map<ProductModel, ProductModelViewModel>(_productModelBusiness.GetProductModel(ID));
            }
            return PartialView("_ProductModelBasicInfo", productModelVM);
        }
        #endregion ProductModel Basic Information
        #region GetAllProductModel
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "R")]
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
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "D")]
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

        [HttpPost]
        public string UploadImages()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                Guid FileID = Guid.NewGuid();
                FileUpload fileuploadObj = new FileUpload();
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        
                        HttpPostedFileBase file = files[i];
                        Random random = new Random();
                        string fname = "pilotpro" + random.Next() + ".png";                     
                        fileuploadObj.AttachmentURL = "Content/images/ProductImage/" + fname;                 
                        fname = Path.Combine(Server.MapPath("~/Content/images/ProductImage/"),fname);
                        file.SaveAs(fname);
                    }
                        //return _fileObj.AttachmentURL;
                        return JsonConvert.SerializeObject(new { Result = "OK", Record = fileuploadObj, Message = "File Uploaded Successfully!" });
                       

                }
                catch (Exception ex)
                {
                    //return "";
                    return JsonConvert.SerializeObject(new { Result = "Error", Message = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return "";
                //return Json(new { Result = "Error", Message = "No files selected." });
            }
        }

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductModel", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductModel");
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
            toolboxVM = _tool.SetToolbarAccess(toolboxVM, permission);
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}