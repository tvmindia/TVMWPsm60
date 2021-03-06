﻿using AutoMapper;
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
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DeliveryChallanController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IDeliveryChallanBusiness _deliveryChallanBusiness;
        IProductionOrderBusiness _productionOrderBusiness;
        ISaleOrderBusiness _saleOrderBusiness;
        SecurityFilter.ToolBarAccess _tool;

        public DeliveryChallanController(IDeliveryChallanBusiness deliveryChallanBusiness,
            IProductionOrderBusiness productionOrderBusiness,
            ISaleOrderBusiness saleOrderBusiness, SecurityFilter.ToolBarAccess tool

            )
        {
            _deliveryChallanBusiness = deliveryChallanBusiness;
            _productionOrderBusiness = productionOrderBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _tool = tool;
        }
        // GET: DeliveryChallan
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
            DeliveryChallanAdvanceSearchViewModel deliveryChallanVM = new DeliveryChallanAdvanceSearchViewModel();            
            return View(deliveryChallanVM);
        }

        #region DeliveryChallan Form
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public ActionResult DeliveryChallanForm(Guid id, Guid? saleOrderID, Guid? prodOrderID)
        {
            DeliveryChallanViewModel deliveryChallanVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    deliveryChallanVM = Mapper.Map<DeliveryChallan, DeliveryChallanViewModel>(_deliveryChallanBusiness.GetDeliveryChallan(id));
                    deliveryChallanVM.IsUpdate = true;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    deliveryChallanVM.IsDocLocked = deliveryChallanVM.DocumentOwners.Contains(appUA.UserName);
                    if (deliveryChallanVM.SaleOrderID != null)
                    {
                        deliveryChallanVM.DocumentType = "SaleOrder";
                        deliveryChallanVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleOrderID);
                    }
                    if (deliveryChallanVM.ProdOrderID != null)
                    {
                        deliveryChallanVM.DocumentType = "ProductionOrder";
                        deliveryChallanVM.ProductionOrderSelectList = _productionOrderBusiness.GetProductionOrderForSelectList(prodOrderID);
                    }

                }
                else if (id == Guid.Empty && saleOrderID != null)
                {
                    SaleOrderViewModel saleOrderVM = Mapper.Map<SaleOrder, SaleOrderViewModel>(_saleOrderBusiness.GetSaleOrder((Guid)saleOrderID));
                    deliveryChallanVM = new DeliveryChallanViewModel();
                    deliveryChallanVM.IsUpdate = false;
                    deliveryChallanVM.ID = Guid.Empty;
                    deliveryChallanVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleOrderID);
                    deliveryChallanVM.SaleOrderID = saleOrderID;
                    deliveryChallanVM.CustomerID = saleOrderVM.CustomerID;
                    deliveryChallanVM.ProdOrderID = null;
                    deliveryChallanVM.DocumentType = "SaleOrder";
                    deliveryChallanVM.ProductionOrderSelectList = new List<SelectListItem>();
                    deliveryChallanVM.Customer = saleOrderVM.Customer;
                    deliveryChallanVM.IsDocLocked = false;
                }
                else if (id == Guid.Empty && prodOrderID != null)
                {
                    ProductionOrderViewModel productionOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder((Guid)prodOrderID));
                    deliveryChallanVM = new DeliveryChallanViewModel();
                    deliveryChallanVM.IsUpdate = false;
                    deliveryChallanVM.ID = Guid.Empty;
                    deliveryChallanVM.ProductionOrderSelectList = _productionOrderBusiness.GetProductionOrderForSelectList(prodOrderID);
                    deliveryChallanVM.ProdOrderID = prodOrderID;
                    deliveryChallanVM.CustomerID = productionOrderVM.CustomerID;
                    deliveryChallanVM.SaleOrderID = null;
                    deliveryChallanVM.DocumentType = "ProductionOrder";
                    deliveryChallanVM.SaleOrderSelectList = new List<SelectListItem>();
                    deliveryChallanVM.Customer = productionOrderVM.Customer;
                    deliveryChallanVM.IsDocLocked = false;
                }
                else
                {
                    deliveryChallanVM = new DeliveryChallanViewModel();
                    deliveryChallanVM.SaleOrderSelectList = new List<SelectListItem>();
                    deliveryChallanVM.ProductionOrderSelectList = new List<SelectListItem>();
                    deliveryChallanVM.SaleOrderID = null;
                    deliveryChallanVM.ProdOrderID = null;
                    deliveryChallanVM.DocumentType = "SaleOrder";
                    deliveryChallanVM.IsDocLocked = false;
                    deliveryChallanVM.IsUpdate = false;
                    deliveryChallanVM.ID = Guid.Empty;
                    deliveryChallanVM.Customer = new CustomerViewModel();
                    deliveryChallanVM.Customer.CompanyName = "-";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_DeliveryChallanForm", deliveryChallanVM);
        }
        #endregion DeliveryChallan Form

        #region DeliveryChallan Detail Add
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public ActionResult AddDeliveryChallanDetail()
        {
            DeliveryChallanDetailViewModel deliveryChallanDetailVM = new DeliveryChallanDetailViewModel();
            deliveryChallanDetailVM.IsUpdate = false;
            deliveryChallanDetailVM.OrderQty = 0;
            deliveryChallanDetailVM.DelvQty = 0;
            deliveryChallanDetailVM.PrevDelQty = 0;
            return PartialView("_AddDeliveryChallanDetail", deliveryChallanDetailVM);
        }
        #endregion DeliveryChallan Detail Add

        #region InsertUpdateDeliveryChallan
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "W")]
        public string InsertUpdateDeliveryChallan(DeliveryChallanViewModel deliveryChallanVM)
        {
            //object resultFromBusiness = null;

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                deliveryChallanVM.PSASysCommon = new PSASysCommonViewModel();
                deliveryChallanVM.PSASysCommon.CreatedBy = appUA.UserName;
                deliveryChallanVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                deliveryChallanVM.PSASysCommon.UpdatedBy = appUA.UserName;
                deliveryChallanVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(deliveryChallanVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                deliveryChallanVM.DeliveryChallanDetailList = JsonConvert.DeserializeObject<List<DeliveryChallanDetailViewModel>>(ReadableFormat);
                object result = _deliveryChallanBusiness.InsertUpdateDeliveryChallan(Mapper.Map<DeliveryChallanViewModel, DeliveryChallan>(deliveryChallanVM));

                if (deliveryChallanVM.ID == Guid.Empty)
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

        #endregion InsertUpdateDeliveryChallan

        #region GetAllDeliveryChallan
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public JsonResult GetAllDeliveryChallan(DataTableAjaxPostModel model, DeliveryChallanAdvanceSearchViewModel deliveryChallanAdvanceSearchVM)
        {
            //setting options to our model
            deliveryChallanAdvanceSearchVM.DataTablePaging.Start = model.start;
            deliveryChallanAdvanceSearchVM.DataTablePaging.Length = (deliveryChallanAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : deliveryChallanAdvanceSearchVM.DataTablePaging.Length;

            List<DeliveryChallanViewModel> deliveryChallanVMList = Mapper.Map<List<DeliveryChallan>, List<DeliveryChallanViewModel>>(_deliveryChallanBusiness.GetAllDeliveryChallan(Mapper.Map<DeliveryChallanAdvanceSearchViewModel, DeliveryChallanAdvanceSearch>(deliveryChallanAdvanceSearchVM)));
           
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = deliveryChallanVMList.Count != 0 ? deliveryChallanVMList[0].TotalCount : 0,
                recordsFiltered = deliveryChallanVMList.Count != 0 ? deliveryChallanVMList[0].FilteredCount : 0,
                data = deliveryChallanVMList
            });
        }
        #endregion GetAllDeliveryChallan

        #region Get DeliveryChallan SelectList On Demand

        public ActionResult GetDeliveryChallanForSelectListOnDemand(string searchTerm)
        {
            List<DeliveryChallanViewModel> deliveryChallanVMList = string.IsNullOrEmpty(searchTerm) ? null : Mapper.Map<List<DeliveryChallan>, List<DeliveryChallanViewModel>>(_deliveryChallanBusiness.GetDeliveryChallanForSelectListOnDemand(searchTerm));
            var list = new List<Select2Model>();
            if (deliveryChallanVMList != null)
            {
                foreach (DeliveryChallanViewModel deliveryChallanVM in deliveryChallanVMList)
                {
                    list.Add(new Select2Model()
                    {
                        text = deliveryChallanVM.DelvChallanNo,
                        id = deliveryChallanVM.ID.ToString()
                    });
                }
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get DeliveryChallan SelectList On Demand

        #region GetDeliveryChallanDetailListByDeliveryChallanID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public string GetDeliveryChallanDetailListByDeliveryChallanID(Guid deliveryChallanID)
        {
            try
            {
                List<DeliveryChallanDetailViewModel> deliveryChallanItemViewModelList = new List<DeliveryChallanDetailViewModel>();
                if (deliveryChallanID == Guid.Empty)
                {
                    DeliveryChallanDetailViewModel deliveryChallanDetailVM = new DeliveryChallanDetailViewModel()
                    {
                        ID = Guid.Empty,
                        DelvChallanID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        OrderQty = 0,
                        DelvQty = 0,
                        PrevDelQty=0,
                        Unit = new UnitViewModel()
                        {
                            Code = 0,
                            Description = string.Empty,
                        },

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
                    deliveryChallanItemViewModelList.Add(deliveryChallanDetailVM);
                }
                else
                {
                    deliveryChallanItemViewModelList = Mapper.Map<List<DeliveryChallanDetail>, List<DeliveryChallanDetailViewModel>>(_deliveryChallanBusiness.GetDeliveryChallanDetailListByDeliveryChallanID(deliveryChallanID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = deliveryChallanItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetDeliveryChallanDetailListByDeliveryChallanID

        #region Get DeliveryChallan DetailList By DEliveryChallanID with ProductionOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public string GetDeliveryChallanDetailListByDeliveryChallanIDWithProductionOrder(Guid prodOrderID)
        {
            try
            {
                List<DeliveryChallanDetailViewModel> deliveryChallanItemViewModelList = new List<DeliveryChallanDetailViewModel>();
                if (prodOrderID != Guid.Empty)
                {
                    List<ProductionOrderDetailViewModel> productionOrderDetailVMList = Mapper.Map<List<ProductionOrderDetail>, List<ProductionOrderDetailViewModel>>(_productionOrderBusiness.GetProductionOrderDetailListByProductionOrderID(prodOrderID).Where(x=>x.OrderQty!=x.PrevDelQty).ToList());

                    deliveryChallanItemViewModelList = (from productionOrderDetailVM in productionOrderDetailVMList
                                                        select new DeliveryChallanDetailViewModel
                                                        {

                                                            ID = Guid.Empty,
                                                            DelvChallanID = Guid.Empty,
                                                            ProductID = productionOrderDetailVM.ProductID,
                                                            ProductModelID = productionOrderDetailVM.ProductModelID,
                                                            ProductSpec = productionOrderDetailVM.ProductSpec,
                                                            OrderQty = productionOrderDetailVM.OrderQty,
                                                            UnitCode = productionOrderDetailVM.UnitCode,
                                                            DelvQty = productionOrderDetailVM.OrderQty-productionOrderDetailVM.PrevDelQty,
                                                            SpecTag = productionOrderDetailVM.SpecTag,
                                                            PrevDelQty=productionOrderDetailVM.PrevDelQty,
                                                            Product = productionOrderDetailVM.Product,
                                                            ProductModel = productionOrderDetailVM.ProductModel,
                                                            Unit = productionOrderDetailVM.Unit,
                                                        }).ToList();

                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = deliveryChallanItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get DeliveryChallan DetailList By DEliveryChallanID with ProductionOrder

        #region Get DeliveryChallan DetailList By DeliveryChallanID with SaleOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public string GetDeliveryChallanDetailListByDeliveryChallanIDWithSaleOrder(Guid saleOrderID)
        {
            try
            {
                List<DeliveryChallanDetailViewModel> deliveryChallanItemViewModelList = new List<DeliveryChallanDetailViewModel>();
                if (saleOrderID != Guid.Empty)
                {
                    List<SaleOrderDetailViewModel> saleOrderDetailVMList = Mapper.Map<List<SaleOrderDetail>, List<SaleOrderDetailViewModel>>(_saleOrderBusiness.GetSaleOrderDetailListBySaleOrderID(saleOrderID,false).Where(x=>x.Qty!=x.PrevDelQty).ToList());
                    deliveryChallanItemViewModelList = (from saleOrderDetailVM in saleOrderDetailVMList
                                                        select new DeliveryChallanDetailViewModel
                                                        {

                                                            ID = Guid.Empty,
                                                            DelvChallanID = Guid.Empty,
                                                            ProductID = saleOrderDetailVM.ProductID,
                                                            ProductModelID = saleOrderDetailVM.ProductModelID,
                                                            ProductSpec = saleOrderDetailVM.ProductSpec,
                                                            OrderQty = saleOrderDetailVM.Qty,
                                                            UnitCode = saleOrderDetailVM.UnitCode,
                                                            DelvQty = saleOrderDetailVM.Qty-saleOrderDetailVM.PrevDelQty,
                                                            SpecTag = saleOrderDetailVM.SpecTag,
                                                            PrevDelQty=saleOrderDetailVM.PrevDelQty,
                                                            Product = saleOrderDetailVM.Product,
                                                            ProductModel = saleOrderDetailVM.ProductModel,
                                                            Unit = saleOrderDetailVM.Unit,
                                                        }).ToList();


                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = deliveryChallanItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get DeliveryChallan DetailList By DeliveryChallanID with SaleOrder

        #region DeleteDelievryChallan  
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "D")]
        public string DeleteDeliveryChallan(Guid id)
        {

            try
            {
                object result = _deliveryChallanBusiness.DeleteDeliveryChallan(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion DeleteDelievryChallan

        #region DeleteDeliveryChallanDetail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "D")]
        public string DeleteDeliveryChallanDetail(Guid id)
        {

            try
            {
                object result = _deliveryChallanBusiness.DeleteDeliveryChallanDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion DeleteDeliveryChallanDetail

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DeliveryChallan", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "DeliveryChallan");
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddDeliveryChallan();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportDeliveryChallanData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetDeliveryChallanList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddDeliveryChallan();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveDeliveryChallan();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetDeliveryChallan();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteDeliveryChallan();";

                    //toolboxVM.EmailBtn.Visible = true;
                    //toolboxVM.EmailBtn.Text = "Email";
                    //toolboxVM.EmailBtn.Title = "Email";
                    //toolboxVM.EmailBtn.Event = "EmailDeliveryChallan();";

                    //toolboxVM.SendForApprovalBtn.Visible = true;
                    //toolboxVM.SendForApprovalBtn.Text = "Send";
                    //toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    //toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','DLC');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','DLC');";
                    break;

                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddDeliveryChallan();";

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
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','DLC');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','DLC');";
                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveDeliveryChallan();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetDeliveryChallan();";

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