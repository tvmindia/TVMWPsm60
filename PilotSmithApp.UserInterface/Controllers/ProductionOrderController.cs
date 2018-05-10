using AutoMapper;
using Newtonsoft.Json;
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
    public class ProductionOrderController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProductionOrderBusiness _productionOrderBusiness;
        public ProductionOrderController(IProductionOrderBusiness productionOrderBusiness)
        {
            _productionOrderBusiness = productionOrderBusiness;
        }
        // GET: ProductOrder
        [AuthSecurityFilter(ProjectObject = "ProductOrder", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region ProductionOrderForm Form
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ProductionOrderForm(Guid id)
        {
            ProductionOrderViewModel productionOrderVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    productionOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder(id));
                    productionOrderVM.IsUpdate = true;
                }
                else if (id == Guid.Empty)
                {
                    productionOrderVM = new ProductionOrderViewModel();
                    productionOrderVM.IsUpdate = false;
                    productionOrderVM.ID = Guid.Empty;
                    productionOrderVM.DocumentStatusCode = 7;
                }                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return PartialView("_ProductionOrderForm", productionOrderVM);
        }
        #endregion ProductionOrderForm Form

        #region ProductionOrder Detail Add
        public ActionResult AddProductionOrderDetail()
        {
            ProductionOrderDetailViewModel productionOrderDetailVM = new ProductionOrderDetailViewModel();
            productionOrderDetailVM.IsUpdate = false;
            return PartialView("_AddProductionOrderDetail", productionOrderDetailVM);
        }
        #endregion ProductionOrder Detail Add

        #region InsertUpdateProductionOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "W")]
        public string InsertUpdateProductionOrder(ProductionOrderViewModel productionOrderVM)
        {
            //object resultFromBusiness = null;

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productionOrderVM.PSASysCommon = new PSASysCommonViewModel();
                productionOrderVM.PSASysCommon.CreatedBy = appUA.UserName;
                productionOrderVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                productionOrderVM.PSASysCommon.UpdatedBy = appUA.UserName;
                productionOrderVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(productionOrderVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                productionOrderVM.ProductionOrderDetailList = JsonConvert.DeserializeObject<List<ProductionOrderDetailViewModel>>(ReadableFormat);
                object result = _productionOrderBusiness.InsertUpdateProductionOrder(Mapper.Map<ProductionOrderViewModel, ProductionOrder>(productionOrderVM));

                if (productionOrderVM.ID == Guid.Empty)
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

        #endregion InsertUpdateProductionOrder

        #region GetAllProductionOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public JsonResult GetAllProductionOrder(DataTableAjaxPostModel model, ProductionOrderAdvanceSearchViewModel productionOrderAdvanceSearchVM)
        {
            //setting options to our model
            productionOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            productionOrderAdvanceSearchVM.DataTablePaging.Length = (productionOrderAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productionOrderAdvanceSearchVM.DataTablePaging.Length;

            List<ProductionOrderViewModel> productionOrderVMList =Mapper.Map<List<ProductionOrder>, List<ProductionOrderViewModel>>(_productionOrderBusiness.GetAllProductionOrder(Mapper.Map<ProductionOrderAdvanceSearchViewModel, ProductionOrderAdvanceSearch>(productionOrderAdvanceSearchVM)));
            if (productionOrderAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = productionOrderVMList.Count != 0 ? productionOrderVMList[0].TotalCount : 0;
                int filteredResult = productionOrderVMList.Count != 0 ? productionOrderVMList[0].FilteredCount : 0;
                productionOrderVMList = productionOrderVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = productionOrderVMList.Count != 0 ? productionOrderVMList[0].TotalCount : 0,
                recordsFiltered = productionOrderVMList.Count != 0 ? productionOrderVMList[0].FilteredCount : 0,
                data = productionOrderVMList
            });
        }
        #endregion GetAllProductionOrder

        #region Get QUotation SelectList On Demand
        public ActionResult GetProductionOrderSelectListOnDemand(string searchTerm)
        {
            List<ProductionOrderViewModel> productionOrderVMList = string.IsNullOrEmpty(searchTerm) ? null : Mapper.Map<List<ProductionOrder>,List<ProductionOrderViewModel>>(_productionOrderBusiness.GetProductionOrderForSelectListOnDemand(searchTerm));
            var list = new List<Select2Model>();
            if (productionOrderVMList != null)
            {
                foreach (ProductionOrderViewModel productionOrderVM in productionOrderVMList)
                {
                    list.Add(new Select2Model()
                    {
                        text = productionOrderVM.ProdOrderNo,
                        id = productionOrderVM.ID.ToString()
                    });
                }
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get ProductionOrder SelectList On Demand

        #region GetProductionOrderDetailListByProductionOrderID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public string GetProductionOrderDetailListByProductionOrderID(Guid productionOrderID)
        {
            try
            {
                List<ProductionOrderDetailViewModel> productionOrderItemViewModelList = new List<ProductionOrderDetailViewModel>();
                if (productionOrderID == Guid.Empty)
                {
                    ProductionOrderDetailViewModel productionOrderDetailVM = new ProductionOrderDetailViewModel()
                    {
                        ID = Guid.Empty,
                        ProdOrderID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        OrderQty = 0,
                        ProducedQty=0,
                        Unit=new UnitViewModel()
                        {
                            Code=0,
                            Description=string.Empty,
                        },
                        Rate=0,
                        //MileStone1FcFinishDt= _pSASysCommon.GetCurrentDateTime(),
                        //MileStone1FcFinishDtFormatted = _pSASysCommon.GetCurrentDateTime().ToString("dd-MMM-yyyy"),
                        Product = new ProductViewModel()
                        {
                            ID = Guid.Empty,
                            Code = string.Empty,
                            Name = string.Empty,
                        },
                        ProductModel = new ProductModelViewModel()
                        {
                            ID = Guid.Empty,
                            Name = string.Empty
                        },
                    };
                    productionOrderItemViewModelList.Add(productionOrderDetailVM);
                }
                else
                {
                    productionOrderItemViewModelList = Mapper.Map<List<ProductionOrderDetail>, List<ProductionOrderDetailViewModel>>(_productionOrderBusiness.GetProductionOrderDetailListByProductionOrderID(productionOrderID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = productionOrderItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetProductionOrderDetailListByProductionOrderID

        #region DeleteProductionOrder       
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "D")]
        public string DeleteProductionOrder(Guid id)
        {

            try
            {
                object result = _productionOrderBusiness.DeleteProductionOrder(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion DeleteProductionOrder

        #region DeleteProductionOrderDetail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "D")]
        public string DeleteProductionOrderDetail(Guid id)
        {

            try
            {
                object result = _productionOrderBusiness.DeleteProductionOrderDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion DeleteProductionOrderDetail

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportProductionOrderData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionOrderList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProductionOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionOrder();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteProductionOrder();";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProductionOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProductionOrder();";

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