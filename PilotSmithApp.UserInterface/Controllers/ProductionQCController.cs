using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace PilotSmithApp.UserInterface.Controllers
{
    public class ProductionQCController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProductionQCBusiness _productionQCBusiness;
        IProductionOrderBusiness _productionOrderBusiness;       
        IDocumentStatusBusiness _documentStatusBusiness;
        SecurityFilter.ToolBarAccess _tool;        

        public ProductionQCController(IProductionQCBusiness productionQCBusiness, 
          IProductionOrderBusiness productionOrderBusiness,         
          IDocumentStatusBusiness documentStatusBusiness,SecurityFilter.ToolBarAccess tool          
          )
        {
            _productionQCBusiness = productionQCBusiness;
            _productionOrderBusiness = productionOrderBusiness;            
            _documentStatusBusiness = documentStatusBusiness;
            _tool = tool;       
        }
        // GET: ProductionQ
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
            ProductionQCAdvanceSearchViewModel productionQCVM = new ProductionQCAdvanceSearchViewModel();                   
            productionQCVM.DocumentStatus = new DocumentStatusViewModel();
            productionQCVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("PQC");        
            return View(productionQCVM);
        }
        #region ProductionQC Form
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public ActionResult ProductionQCForm(Guid id, Guid? productionOrderID)
        {
            ProductionQCViewModel productionQCVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    productionQCVM = Mapper.Map<ProductionQC, ProductionQCViewModel>(_productionQCBusiness.GetProductionQC(id));
                    productionQCVM.IsUpdate = true;
                    productionQCVM.ProdOrderSelectList = _productionOrderBusiness.GetProductionOrderForSelectList(productionOrderID);
                    AppUA appUA = Session["AppUA"] as AppUA;
                    productionQCVM.IsDocLocked = productionQCVM.DocumentOwners.Contains(appUA.UserName);
                }
                else if (id == Guid.Empty && productionOrderID == null)
                {
                    productionQCVM = new ProductionQCViewModel();
                    productionQCVM.IsUpdate = false;
                    productionQCVM.ID = Guid.Empty;
                    productionQCVM.ProdOrderID = null;
                    productionQCVM.ProdOrderSelectList = new List<SelectListItem>();
                    productionQCVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description="-",
                    };
                    //productionQCVM.Customer = new CustomerViewModel();
                    //productionQCVM.Customer.CompanyName = "-";
                    productionQCVM.IsDocLocked = false;
                }
                else if (id == Guid.Empty && productionOrderID != null)
                {
                    ProductionOrderViewModel productioOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder((Guid)productionOrderID));
                    productionQCVM = new ProductionQCViewModel();
                    productionQCVM.IsUpdate = false;
                    productionQCVM.ID = Guid.Empty;
                    productionQCVM.ProdOrderSelectList = _productionOrderBusiness.GetProductionOrderForSelectList(productionOrderID);
                    productionQCVM.ProdOrderID = productionOrderID;
                    productionQCVM.CustomerID = productioOrderVM.CustomerID;
                    productionQCVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-",
                    };
                    productionQCVM.Customer = productioOrderVM.Customer;
                    productionQCVM.IsDocLocked = false;
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
                        ID = Guid.Empty,
                        ProdQCID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        QCQty = 0,
                        QCDate = _pSASysCommon.GetCurrentDateTime(),
                        QCDateFormatted = _pSASysCommon.GetCurrentDateTime().ToString("dd-MMM-yyyy"),
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
                    productionQCItemViewModelList.Add(productionQCDetailVM);
                }
                else
                {
                    productionQCItemViewModelList = Mapper.Map<List<ProductionQCDetail>, List<ProductionQCDetailViewModel>>(_productionQCBusiness.GetProductionQCDetailListByProductionQCID(productionQCID));
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
                object result = _productionQCBusiness.DeleteProductionQC(id);
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
                object result = _productionQCBusiness.DeleteProductionQCDetail(id);
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
            List<ProductionQCViewModel> ProductionQCVMList = Mapper.Map<List<ProductionQC>, List<ProductionQCViewModel>>(_productionQCBusiness.GetAllProductionQC(Mapper.Map<ProductionQCAdvanceSearchViewModel, ProductionQCAdvanceSearch>(ProductionQCAdvanceSearchVM)));
           
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
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "W")]
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
                object result = _productionQCBusiness.InsertUpdateProductionQC(Mapper.Map<ProductionQCViewModel, ProductionQC>(productionQCVM));

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
        #region Get ProductionQC DetailList By ProductionOrderID with ProductionOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionQC", Mode = "R")]
        public string GetProductionQCDetailListByProductionOrderIDWithProductionOrder(Guid productionOrderID)
        {
            try
            {
                List<ProductionQCDetailViewModel> productionQCDetailVMList = new List<ProductionQCDetailViewModel>();
                if (productionOrderID != Guid.Empty)
                {
                    List<ProductionOrderDetailViewModel> productionOrderVMList = Mapper.Map<List<ProductionOrderDetail>, List<ProductionOrderDetailViewModel>>(_productionOrderBusiness.GetProductionOrderDetailListByProductionOrderID(productionOrderID).Where(x => x.ProducedQty != x.QCCompletedQty).ToList());
                    productionQCDetailVMList = (from prodOrderVM in productionOrderVMList
                                                select new ProductionQCDetailViewModel
                                                {
                                                    ID = Guid.Empty,
                                                    ProdQCID = Guid.Empty,
                                                    ProductID = prodOrderVM.ProductID,
                                                    Product = prodOrderVM.Product,
                                                    ProductModelID = prodOrderVM.ProductModelID,
                                                    ProductModel = prodOrderVM.ProductModel,
                                                    ProductSpec = prodOrderVM.ProductSpec,
                                                    ProducedQty = prodOrderVM.ProducedQty != null ? prodOrderVM.ProducedQty : 0,
                                                    QCQtyPrevious = prodOrderVM.QCCompletedQty,
                                                    QCQty = (prodOrderVM.ProducedQty - prodOrderVM.QCCompletedQty) > 0 ? (prodOrderVM.ProducedQty - prodOrderVM.QCCompletedQty) : 0,
                                                    Unit = prodOrderVM.Unit,
                                                    SpecTag = prodOrderVM.SpecTag,
                                                    QCDate = _pSASysCommon.GetCurrentDateTime(),
                                                    QCDateFormatted = _pSASysCommon.GetCurrentDateTime().ToString("dd-MMM-yyyy"),
                                                    Employee = new EmployeeViewModel() { Name = "" }
                                                }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = productionQCDetailVMList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get ProductionQC DetailList By ProductionOrderID with ProductionOrder
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
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductionQC");
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

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','PQC');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','PQC');";

                    break;

                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionQC();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Disable = true;
                    toolboxVM.savebtn.DisableReason = "Document Locked";
                    toolboxVM.savebtn.Event = "";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Disable = true;
                    toolboxVM.resetbtn.DisableReason = "Document Locked";
                    toolboxVM.resetbtn.Event = "";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Disable = true;
                    toolboxVM.deletebtn.DisableReason = "Document Locked";
                    toolboxVM.deletebtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','PQC');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','PQC');";
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
            toolboxVM = _tool.SetToolbarAccess(toolboxVM, permission);
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}