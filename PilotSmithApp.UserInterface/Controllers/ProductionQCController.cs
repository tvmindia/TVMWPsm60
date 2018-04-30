using AutoMapper;
using Newtonsoft.Json;
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
    public class ProductionQCController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        public ProductionQCController()
        {
        }
        // GET: ProductionQ
        public ActionResult Index()
        {
            ProductionQCAdvanceSearchViewModel productionQCVM = new ProductionQCAdvanceSearchViewModel();
            return View(productionQCVM);
        }
        #region ProductionQC Form
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public ActionResult ProductionQCForm(Guid id)
        {
            ProductionQCViewModel productionQCVM = null;
            try
            {

                if (id != Guid.Empty)
                {
                    productionQCVM = new ProductionQCViewModel();// Mapper.Map<ProductionQC, ProductionQCViewModel>(_productionQCBusiness.GetProductionQC(id));
                    productionQCVM.IsUpdate = true;
                }
                else
                {
                    productionQCVM = new ProductionQCViewModel();
                    productionQCVM.IsUpdate = false;
                    productionQCVM.ID = Guid.Empty;
                    productionQCVM.DocumentStatusCode = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_ProductionQCForm", productionQCVM);
        }
        #endregion ProductionQC Form
        #region ProductionQC Detail Add
        public ActionResult AddProductionQCDetail()
        {
            ProductionQCDetailViewModel productionQCDetailVM = new ProductionQCDetailViewModel();
            productionQCDetailVM.IsUpdate = false;
            return PartialView("_AddProductionQCDetail", productionQCDetailVM);
        }
        #endregion ProductionQC Detail Add
        #region Get ProductionQC DetailList By ProductionQCID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public string GetProductionQCDetailListByProductionQCID(Guid productionQCID)
        {
            try
            {
                List<ProductionQCDetailViewModel> productionQCItemViewModelList = new List<ProductionQCDetailViewModel>();
                if (productionQCID == Guid.Empty)
                {
                    ProductionQCDetailViewModel productionQCDetailVM = new ProductionQCDetailViewModel()
                    {
                        //ID = Guid.Empty,
                        //ProductionQCID = Guid.Empty,
                        //ProductID = Guid.Empty,
                        //ProductModelID = Guid.Empty,
                        //ProductSpec = string.Empty,
                        //Qty = 0,
                        //UnitCode = null,
                        //Rate = 0,
                        //Product = new ProductViewModel()
                        //{
                        //    ID = Guid.Empty,
                        //    Code = string.Empty,
                        //    Name = string.Empty,
                        //},
                        //ProductModel = new ProductModelViewModel()
                        //{
                        //    ID = Guid.Empty,
                        //    Name = string.Empty
                        //},
                        //Unit = new UnitViewModel()
                        //{
                        //    Description = null,
                        //},
                    };
                    productionQCItemViewModelList.Add(productionQCDetailVM);
                }
                else
                {
                    productionQCItemViewModelList = null;// Mapper.Map<List<ProductionQCDetail>, List<ProductionQCDetailViewModel>>(_productionQCBusiness.GetProductionQCDetailListByProductionQCID(productionQCID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = productionQCItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get ProductionQC DetailList By ProductionQCID
        #region Delete ProductionQC
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "D")]
        public string DeleteProductionQC(Guid id)
        {

            try
            {
                object result = null;// _productionQCBusiness.DeleteProductionQC(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete ProductionQC
        #region Delete ProductionQC Detail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "D")]
        public string DeleteProductionQCDetail(Guid id)
        {

            try
            {
                object result = null;// _productionQCBusiness.DeleteProductionQCDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete ProductionQC Detail
        #region GetAllProductionQC
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public JsonResult GetAllProductionQC(DataTableAjaxPostModel model, ProductionQCAdvanceSearchViewModel ProductionQCAdvanceSearchVM)
        {
            //setting options to our model
            ProductionQCAdvanceSearchVM.DataTablePaging.Start = model.start;
            ProductionQCAdvanceSearchVM.DataTablePaging.Length = (ProductionQCAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : ProductionQCAdvanceSearchVM.DataTablePaging.Length;

            //ProductionQCAdvanceSearchVM.OrderColumn = model.order[0].column;
            //ProductionQCAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<ProductionQCViewModel> ProductionQCVMList = new List<ProductionQCViewModel>();// Mapper.Map<List<ProductionQC>, List<ProductionQCViewModel>>(_productionQCBusiness.GetAllProductionQC(Mapper.Map<ProductionQCAdvanceSearchViewModel, ProductionQCAdvanceSearch>(ProductionQCAdvanceSearchVM)));
            if (ProductionQCAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = ProductionQCVMList.Count != 0 ? ProductionQCVMList[0].TotalCount : 0;
                int filteredResult = ProductionQCVMList.Count != 0 ? ProductionQCVMList[0].FilteredCount : 0;
                ProductionQCVMList = ProductionQCVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = ProductionQCVMList.Count != 0 ? ProductionQCVMList[0].TotalCount : 0,
                recordsFiltered = ProductionQCVMList.Count != 0 ? ProductionQCVMList[0].FilteredCount : 0,
                data = ProductionQCVMList
            });
        }
        #endregion GetAllProductionQC
        #region InsertUpdateProductionQC
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public string InsertUpdateProductionQC(ProductionQCViewModel productionQCVM)
        {
            //object resultFromBusiness = null;

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productionQCVM.PSASysCommon = new PSASysCommonViewModel();
                productionQCVM.PSASysCommon.CreatedBy = appUA.UserName;
                productionQCVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                productionQCVM.PSASysCommon.UpdatedBy = appUA.UserName;
                productionQCVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(productionQCVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                productionQCVM.ProductionQCDetailList = JsonConvert.DeserializeObject<List<ProductionQCDetailViewModel>>(ReadableFormat);
                object result = null;// _productionQCBusiness.InsertUpdateProductionQC(Mapper.Map<ProductionQCViewModel, ProductionQC>(productionQCVM));

                if (productionQCVM.ID == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Insertion successfull" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Updation successfull" });
                }


            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }

        #endregion InsertUpdateProductionQC

        //#region ProductionQCSelectList
        //public ActionResult ProductionQCSelectList(string required)
        //{
        //    ViewBag.IsRequired = required;
        //    ProductionQCViewModel productionQCVM = new ProductionQCViewModel();
        //    productionQCVM.ProductionQCSelectList = _productionQCBusiness.GetProductionQCForSelectList();
        //    return PartialView("_ProductionQCSelectList", productionQCVM);
        //}
        //#endregion ProductionQCSelectList

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionQC();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportProductionQCData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionQCList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionQC();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProductionQC();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionQC();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteProductionQC();";

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProductionQC();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionQC();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}