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
    public class ProductionOrderController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProductionOrderBusiness _productionOrderBusiness;
        ISaleOrderBusiness _saleOrderBusiness;
        ICommonBusiness _commonBusiness;
        IDocumentStatusBusiness _documentStatusBusiness;
        SecurityFilter.ToolBarAccess _tool;
        ICurrencyBusiness _currencyBusiness;
		IApproverBusiness _approverBusiness;
		public ProductionOrderController(IProductionOrderBusiness productionOrderBusiness,
            ISaleOrderBusiness saleOrderBusiness,
            ICommonBusiness commonBusiness,
            IDocumentStatusBusiness documentStatusBusiness, SecurityFilter.ToolBarAccess tool,
            ICurrencyBusiness currencyBusiness, IApproverBusiness approverBusiness
			)
        {
            _productionOrderBusiness = productionOrderBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _commonBusiness = commonBusiness;
            _documentStatusBusiness = documentStatusBusiness;
            _tool = tool;
            _currencyBusiness = currencyBusiness;
			_approverBusiness = approverBusiness;
		}
        // GET: ProductOrder
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
			AppUA appUA = Session["AppUA"] as AppUA;
			bool IsDocumentApprover = _approverBusiness.CheckIsDocumentApprover("POD", appUA.UserName);
			ViewBag.IsDocumentApprover = IsDocumentApprover;
			ProductionOrderAdvanceSearchViewModel productionOrderAdvanceSearchVM = new ProductionOrderAdvanceSearchViewModel();
            productionOrderAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            productionOrderAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("POD");
            return View(productionOrderAdvanceSearchVM);
        }

        #region ProductionOrderForm Form
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ProductionOrderForm(Guid id, Guid? saleOrderID, string isDocumentApprover = "False")
        {
            ProductionOrderViewModel productionOrderVM = null;
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                if (id != Guid.Empty)
                {
                    productionOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder(id));
                    productionOrderVM.IsUpdate = true;
                    //AppUA appUA = Session["AppUA"] as AppUA;
                    productionOrderVM.IsDocLocked = productionOrderVM.DocumentOwners.Contains(appUA.UserName);
                    productionOrderVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleOrderID);
                    productionOrderVM.Currency = new CurrencyViewModel();
					productionOrderVM.IsDocumentApprover = isDocumentApprover == "True" ? true : false;
					//productionOrderVM.IsDistributor = appUA.RolesCSV.Contains("Production Distributor") ? true : false;
					Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductionOrder");
                    if (permission.SubPermissionList.Count > 0)
                    {
                        string access = "";
                        if (permission.SubPermissionList.Where(a => a.Name == "IsDistributor").ToList().Count() > 0) {
                            access = permission.SubPermissionList.First(li => li.Name == "IsDistributor") != null ? permission.SubPermissionList.First(li => li.Name == "IsDistributor").AccessCode : "";
                        }
                        if (access.Contains("R") || access.Contains("W"))
                        {
                            productionOrderVM.IsDistributor = true;
                        }
                        else
                        {
                            productionOrderVM.IsDistributor = false;
                        }

                        string milestoneaccess = "";
                        if (permission.SubPermissionList.Where(a => a.Name == "Milestone").ToList().Count() > 0)
                        {
                            milestoneaccess = permission.SubPermissionList.First(li => li.Name == "Milestone") != null ? permission.SubPermissionList.First(li => li.Name == "Milestone").AccessCode : "";

                        }
                        if (milestoneaccess.Contains("R") || milestoneaccess.Contains("W"))
                        {
                            productionOrderVM.IsMilestoneUpdate = true;
                        }
                        else
                        {
                            productionOrderVM.IsMilestoneUpdate = false;
                        }
                    }

                }
                else if (id == Guid.Empty && saleOrderID == null)
                {
                    productionOrderVM = new ProductionOrderViewModel();
                    productionOrderVM.IsUpdate = false;
                    productionOrderVM.ID = Guid.Empty;
                    productionOrderVM.SaleOrderID = null;
                    productionOrderVM.SaleOrderSelectList = new List<SelectListItem>();
                    productionOrderVM.DocumentStatus = new DocumentStatusViewModel();
                    productionOrderVM.DocumentStatus.Description = "-";
                    productionOrderVM.Branch = new BranchViewModel();
                    productionOrderVM.Branch.Description = "-";
                    //productionOrderVM.Customer = new CustomerViewModel();
                    //productionOrderVM.Customer.CompanyName = "-";
                    productionOrderVM.IsDocLocked = false;
                    productionOrderVM.CurrencyCode = "INR";
                    productionOrderVM.CurrencyRate = 1;
                    productionOrderVM.Currency = new CurrencyViewModel()
                    {
                        CurrencyList = Mapper.Map<List<Currency>, List<CurrencyViewModel>>(_currencyBusiness.GetCurrencyForSelectList())
                    };
                }
                else if (id == Guid.Empty && saleOrderID != null)
                {
                    SaleOrderViewModel saleOrderVM = Mapper.Map<SaleOrder, SaleOrderViewModel>(_saleOrderBusiness.GetSaleOrder((Guid)saleOrderID));
                    productionOrderVM = new ProductionOrderViewModel();
                    productionOrderVM.IsUpdate = false;
                    productionOrderVM.ID = Guid.Empty;
                    productionOrderVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleOrderID);
                    productionOrderVM.SaleOrderID = saleOrderID;
                    productionOrderVM.CustomerID = saleOrderVM.CustomerID;
                    productionOrderVM.DocumentStatus = new DocumentStatusViewModel();
                    productionOrderVM.DocumentStatus.Description = "-";
                    productionOrderVM.Branch = new BranchViewModel();
                    productionOrderVM.IsDocLocked = false;
                    productionOrderVM.Branch.Description = "-";
                    productionOrderVM.Customer = saleOrderVM.Customer;
                    productionOrderVM.CurrencyCode = saleOrderVM.CurrencyCode;
                    productionOrderVM.CurrencyRate = saleOrderVM.CurrencyRate;
                    productionOrderVM.Currency = new CurrencyViewModel()
                    {
                        CurrencyList = Mapper.Map<List<Currency>, List<CurrencyViewModel>>(_currencyBusiness.GetCurrencyForSelectList())
                    };

                }
                //AppUA appUA = Session["AppUA"] as AppUA;
                //Session["pro"] = 1;
                Permission _permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "SellingPrice");
                string p = _permission.AccessCode;
                if ((p.Contains("R") || p.Contains("W")))
                {
                    productionOrderVM.ShowRate = true;
                }
                else
                {
                    productionOrderVM.ShowRate = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_ProductionOrderForm", productionOrderVM);
        }
        #endregion ProductionOrderForm Form

        #region ProductionOrder Detail Add
        public ActionResult AddProductionOrderDetail(bool update, int IsMilestoneUpdate = 0)
        {
            //ProductionOrderDetailViewModel productionOrderDetailVM = null;
            //List<ProductionOrderDetailViewModel> productionOrderDetailVM = new List<ProductionOrderDetailViewModel>();

            ProductionOrderDetailViewModel productionOrderDetailVM = new ProductionOrderDetailViewModel();
            productionOrderDetailVM = new ProductionOrderDetailViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "SellingPrice");

            if (permission != null)
            {
                string p = permission.AccessCode;
                if (p.Contains("R") || p.Contains("W"))
                {
                    productionOrderDetailVM.ShowRate = true;
                    // productionOrderDetailVM.ShowAmount = true;
                }
                else
                {
                    productionOrderDetailVM.ShowRate = false;
                    // productionOrderDetailVM.ShowAmount = false;
                }
            }
            if (IsMilestoneUpdate == 1)
                productionOrderDetailVM.IsMilestoneUpdate = true;
            else
                productionOrderDetailVM.IsMilestoneUpdate = false;
            productionOrderDetailVM.IsUpdate = update;
            productionOrderDetailVM.OrderQty = 0;
            productionOrderDetailVM.ProducedQty = 0;
            productionOrderDetailVM.PrevProdOrderQty = 0;
            productionOrderDetailVM.PrevProducedQty = 0;
            productionOrderDetailVM.SaleOrderQty = 0;
            productionOrderDetailVM.Rate = 0;
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

            List<ProductionOrderViewModel> productionOrderVMList = Mapper.Map<List<ProductionOrder>, List<ProductionOrderViewModel>>(_productionOrderBusiness.GetAllProductionOrder(Mapper.Map<ProductionOrderAdvanceSearchViewModel, ProductionOrderAdvanceSearch>(productionOrderAdvanceSearchVM)));
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

        #region Get ProductionOrder SelectList On Demand
        public ActionResult GetProductionOrderSelectListOnDemand(string searchTerm)
        {
            List<ProductionOrderViewModel> productionOrderVMList = string.IsNullOrEmpty(searchTerm) ? null : Mapper.Map<List<ProductionOrder>, List<ProductionOrderViewModel>>(_productionOrderBusiness.GetProductionOrderForSelectListOnDemand(searchTerm));
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
                        ProducedQty = 0,
                        PrevProdOrderQty = 0,
                        PrevProducedQty = 0,
                        PrevDelQty = 0,
                        SaleOrderQty = 0,
                        SaleOrderDetailID = Guid.Empty,
                        // TotalProdusedQty = 0,
                        Unit = new UnitViewModel()
                        {
                            Code = 0,
                            Description = string.Empty,
                        },
                        Rate = 0,
                        //MileStone1FcFinishDt= _pSASysCommon.GetCurrentDateTime(),
                        //MileStone1FcFinishDtFormatted = "-",
                        //MileStone1AcTFinishDtFormatted = "-",
                        //MileStone2FcFinishDtFormatted = "-",
                        //MileStone2AcTFinishDtFormatted = "-",
                        //MileStone3FcFinishDtFormatted = "-",
                        //MileStone3AcTFinishDtFormatted = "-",
                        //MileStone4FcFinishDtFormatted = "-",
                        //MileStone4AcTFinishDtFormatted = "-",
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
                    AppUA appUA = Session["AppUA"] as AppUA;
                    Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductionOrder");
                    if (permission.SubPermissionList.Count > 0)
                    {
                        string p = permission.SubPermissionList.First(li => li.Name == "Rate").AccessCode;
                        string q = permission.SubPermissionList.First(li => li.Name == "Amount").AccessCode;
                        foreach (var Item in productionOrderItemViewModelList)
                        {
                            if ((p.Contains("R") || p.Contains("W")) || (q.Contains("R") || q.Contains("W")))
                            {
                                Item.ShowRate = true;
                                Item.ShowAmount = true;
                            }
                            else
                            {
                                Item.ShowRate = false;
                                Item.ShowAmount = false;
                            }
                        }
                    }
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

        #region Get ProductionOrder DetailList By ProductionOrderID with SaleOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public string GetProductionOrderDetailListByProductionOrderIDWithSaleOrder(Guid saleOrderID)
        {
            try
            {
                List<ProductionOrderDetailViewModel> productionOrderItemViewModelList = new List<ProductionOrderDetailViewModel>();
                if (saleOrderID != Guid.Empty)
                {
                    List<SaleOrderDetailViewModel> saleOrderDetailVMList = Mapper.Map<List<SaleOrderDetail>, List<SaleOrderDetailViewModel>>(_saleOrderBusiness.GetSaleOrderDetailListBySaleOrderID(saleOrderID, false).Where(x => x.Qty != x.PrevProdOrderQty).ToList());
                    productionOrderItemViewModelList = (from saleOrderDetailVM in saleOrderDetailVMList
                                                        select new ProductionOrderDetailViewModel
                                                        {
                                                            ID = Guid.Empty,
                                                            SaleOrderDetailID = saleOrderDetailVM.ID,
                                                            ProdOrderID = Guid.Empty,
                                                            ProductID = saleOrderDetailVM.ProductID,
                                                            ProductModelID = saleOrderDetailVM.ProductModelID,
                                                            ProductSpec = saleOrderDetailVM.ProductSpec,
                                                            SaleOrderQty = saleOrderDetailVM.Qty,
                                                            UnitCode = saleOrderDetailVM.UnitCode,
                                                            ProducedQty = 0,    //saleOrderDetailVM.Qty-saleOrderDetailVM.PrevProduceQty,
                                                            OrderQty = saleOrderDetailVM.Qty - saleOrderDetailVM.PrevProdOrderQty,
                                                            // PrevProducedQty=saleOrderDetailVM.PrevProduceQty,
                                                            PrevProdOrderQty = saleOrderDetailVM.PrevProdOrderQty,
                                                            Rate = saleOrderDetailVM.Rate == null ? 0 : saleOrderDetailVM.Rate,
                                                            Amount = 0,
                                                            SpecTag = saleOrderDetailVM.SpecTag,
                                                            Product = saleOrderDetailVM.Product,
                                                            ProductModel = saleOrderDetailVM.ProductModel,
                                                            Unit = saleOrderDetailVM.Unit,
                                                        }).ToList();

                }
                AppUA appUA = Session["AppUA"] as AppUA;
                Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductionOrder");
                if (permission.SubPermissionList.Count > 0)
                {
                    string p = permission.SubPermissionList.First(li => li.Name == "Rate").AccessCode;
                    string q = permission.SubPermissionList.First(li => li.Name == "Amount").AccessCode;
                    foreach (var Item in productionOrderItemViewModelList)
                    {
                        if ((p.Contains("R") || p.Contains("W")) || (q.Contains("R") || q.Contains("W")))
                        {
                            Item.ShowRate = true;
                            Item.ShowAmount = true;
                        }
                        else
                        {
                            Item.ShowRate = false;
                            Item.ShowAmount = false;
                        }
                    }
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = productionOrderItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get ProductionOrder DetailList By ProductionOrderID with SaleOrder

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
                AppUA appUA = Session["AppUA"] as AppUA;
                object result = _productionOrderBusiness.DeleteProductionOrderDetail(id,appUA.UserName, _pSASysCommon.GetCurrentDateTime());
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion DeleteProductionOrderDetail

        #region UpdateProductionOrderEmailInfo
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public string UpdateProductionOrderEmailInfo(ProductionOrderViewModel productionOrderVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productionOrderVM.PSASysCommon = new PSASysCommonViewModel();
                productionOrderVM.PSASysCommon.UpdatedBy = appUA.UserName;
                productionOrderVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object result = _productionOrderBusiness.UpdateProductionOrderEmailInfo(Mapper.Map<ProductionOrderViewModel, ProductionOrder>(productionOrderVM),0);

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

        #endregion UpdateProductionOrderEmailInfo

        #region Email ProductionOrder
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult EmailProductionOrder(ProductionOrderViewModel productionOrderVM)
        {
            bool emailFlag = productionOrderVM.EmailFlag;

            productionOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder(productionOrderVM.ID));
            productionOrderVM.ProductionOrderDetailList = Mapper.Map<List<ProductionOrderDetail>, List<ProductionOrderDetailViewModel>>(_productionOrderBusiness.GetProductionOrderDetailListByProductionOrderID(productionOrderVM.ID));
            productionOrderVM.EmailFlag = emailFlag;
            @ViewBag.path = "http://" + HttpContext.Request.Url.Authority + "/Content/images/logo1.PNG";
            productionOrderVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_EmailProductionOrder", productionOrderVM);
        }
        #endregion Email ProductionOrder

        #region EmailSent
        [HttpPost, ValidateInput(false)]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public async Task<string> SendProductionOrderEmail(ProductionOrderViewModel productionOrderVM)
        {
            try
            {
                object result = null;
                if (!string.IsNullOrEmpty(productionOrderVM.ID.ToString()))
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    productionOrderVM.PSASysCommon = new PSASysCommonViewModel();
                    productionOrderVM.PSASysCommon.UpdatedBy = appUA.UserName;
                    productionOrderVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();

                    bool sendsuccess = await _productionOrderBusiness.ProductionOrderEmailPush(Mapper.Map<ProductionOrderViewModel, ProductionOrder>(productionOrderVM));
                    if (sendsuccess)
                    {
                        //1 is meant for mail sent successfully
                        productionOrderVM.EmailSentYN = sendsuccess;
                        result = _productionOrderBusiness.UpdateProductionOrderEmailInfo(Mapper.Map<ProductionOrderViewModel, ProductionOrder>(productionOrderVM),1);
                    }
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, MailResult = sendsuccess, Message = _appConstant.MailSuccess });
                }
                else
                {

                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID is Missing" });
                }
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion EmailSent

        #region Print ProductionOrder
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult PrintProductionOrder(ProductionOrderViewModel productionOrderVM)
        {
            bool emailFlag = productionOrderVM.EmailFlag;
            productionOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder(productionOrderVM.ID));
            productionOrderVM.ProductionOrderDetailList = Mapper.Map<List<ProductionOrderDetail>, List<ProductionOrderDetailViewModel>>(_productionOrderBusiness.GetProductionOrderDetailListByProductionOrderID(productionOrderVM.ID));
            productionOrderVM.PDFTools = new PDFToolsViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductionOrder");
            //Session["ProdSession"] = 1;
            if (permission.SubPermissionList.Count > 0)
            {
                string p = permission.SubPermissionList.First(li => li.Name == "Rate").AccessCode;
                if (p.Contains("R") || p.Contains("W"))
                {
                    productionOrderVM.ShowRate = true;
                    // productionOrderDetailVM.ShowAmount = true;
                }
                else
                {
                    productionOrderVM.ShowRate = false;
                    // productionOrderDetailVM.ShowAmount = false;
                }
            }
            return PartialView("_PrintProductionOrder", productionOrderVM);
        }
        #endregion Print ProductionOrder 

        #region CheckProducedQty
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckProducedQty(ProductionOrderDetailViewModel prodOrderDetailVM)
        {
            //prodOrderDetailVM = new ProductionOrderDetailViewModel();
            if (prodOrderDetailVM.SaleOrderQty != 0)
            {
                if (prodOrderDetailVM.ProducedQty > prodOrderDetailVM.OrderQty)
                {
                    return Json("<p><span style='vertical-align: 2px'>Produced qty greater than Order qty </span> <i class='fa fa-exclamation' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
                }
            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion CheckProducedQty

        #region CheckOrderQty
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckOrderQty(ProductionOrderDetailViewModel prodOrderDetailVM)
        {
            ProductionOrderDetailViewModel prodOrderDetail = Mapper.Map<ProductionOrderDetail, ProductionOrderDetailViewModel>(_productionOrderBusiness.ValidateProductionOrderDetailOrderQty(prodOrderDetailVM.SaleOrderDetailID, prodOrderDetailVM.ID));
            if (prodOrderDetail.SaleOrderQty != 0 && prodOrderDetail.TotalProdOrderQty < prodOrderDetail.SaleOrderQty)
            {
                var Total = (prodOrderDetailVM.OrderQty + prodOrderDetail.TotalProdOrderQty);
                if (Total > prodOrderDetail.SaleOrderQty)
                {
                    return Json("<p><span style='vertical-align: 2px'>Order qty greater than SaleOrder qty </span> <i class='fa fa-exclamation' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion CheckOrderQty
        #region Preview ProductionOrder
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult PreviewProductionOrder(Guid id)
        {
            ProductionOrderViewModel productionOrderVM = new ProductionOrderViewModel();
            productionOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder(id));
            productionOrderVM.ProductionOrderDetailList = Mapper.Map<List<ProductionOrderDetail>, List<ProductionOrderDetailViewModel>>(_productionOrderBusiness.GetProductionOrderDetailListByProductionOrderID(id));
            return PartialView("_PreviewProductionOrder", productionOrderVM);
        }
        #endregion Email ProductionOrder
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProductionOrder");
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

                case "Draft":
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

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    if (_commonBusiness.CheckDocumentIsDeletable("POD", id))
                    {
                        toolboxVM.deletebtn.Visible = true;
                        toolboxVM.deletebtn.Disable = true;
                        toolboxVM.deletebtn.Text = "Delete";
                        toolboxVM.deletebtn.Title = "Delete";
                        toolboxVM.deletebtn.DisableReason = "Document Used";
                        toolboxVM.deletebtn.Event = "";
                    }
                    else
                    {
                        toolboxVM.deletebtn.Visible = true;
                        toolboxVM.deletebtn.Text = "Delete";
                        toolboxVM.deletebtn.Title = "Delete";
                        toolboxVM.deletebtn.Event = "DeleteProductionOrder();";
                    }

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.Title = "Print Document";
                    toolboxVM.PrintBtn.Event = "PrintProductionOrder()";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    //toolboxVM.RecallBtn.Visible = true;
                    //toolboxVM.RecallBtn.Text = "Recall";
                    //toolboxVM.RecallBtn.Title = "Document Recall";
                    //toolboxVM.RecallBtn.Event = "RecallDocumentItem('POD');";
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

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    if (_commonBusiness.CheckDocumentIsDeletable("POD", id))
                    {
                        toolboxVM.deletebtn.Visible = true;
                        toolboxVM.deletebtn.Disable = true;
                        toolboxVM.deletebtn.Text = "Delete";
                        toolboxVM.deletebtn.Title = "Delete";
                        toolboxVM.deletebtn.DisableReason = "Document Used";
                        toolboxVM.deletebtn.Event = "";
                    }
                    else
                    {
                        toolboxVM.deletebtn.Visible = true;
                        toolboxVM.deletebtn.Text = "Delete";
                        toolboxVM.deletebtn.Title = "Delete";
                        toolboxVM.deletebtn.Event = "DeleteProductionOrder();";
                    }

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    //toolboxVM.RecallBtn.Visible = true;
                    //toolboxVM.RecallBtn.Text = "Recall";
                    //toolboxVM.RecallBtn.Title = "Document Recall";
                    //toolboxVM.RecallBtn.Event = "RecallDocumentItem('POD');";
                    break;

                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

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

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Disable = true;
                    toolboxVM.EmailBtn.DisableReason = "Document Locked";
                    toolboxVM.EmailBtn.Event = "";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Approval History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    //toolboxVM.RecallBtn.Visible = true;
                    //toolboxVM.RecallBtn.Text = "Recall";
                    //toolboxVM.RecallBtn.Title = "Document Recall";
                    //toolboxVM.RecallBtn.Event = "";
                    break;
                case "DistributorApproveDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

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

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.Title = "Print Document";
                    toolboxVM.PrintBtn.Event = "PrintProductionOrder()";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    break;
                case "Approved":
                case "DistributerMileStoneApprove":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

                    //toolboxVM.savebtn.Visible = true;
                    //toolboxVM.savebtn.Text = "Save";
                    //toolboxVM.savebtn.Title = "Save";
                    //toolboxVM.savebtn.Disable = true;
                    //toolboxVM.savebtn.DisableReason = "Document Locked";
                    //toolboxVM.savebtn.Event = "";
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
                    toolboxVM.resetbtn.Disable = true;
                    toolboxVM.resetbtn.DisableReason = "Document Locked";
                    toolboxVM.resetbtn.Event = "";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Disable = true;
                    toolboxVM.deletebtn.DisableReason = "Document Locked";
                    toolboxVM.deletebtn.Event = "";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.Title = "Print Document";
                    toolboxVM.PrintBtn.Event = "PrintProductionOrder()";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    break;
                case "ClosedForApproval":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

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

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";


                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    toolboxVM.RecallBtn.Visible = true;
                    toolboxVM.RecallBtn.Text = "Recall";
                    toolboxVM.RecallBtn.Title = "Document Recall";
                    toolboxVM.RecallBtn.Event = "RecallDocumentItem('POD');";
                    break;
                case "MilestoneApproveDocument":
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
                    toolboxVM.resetbtn.Disable = true;
                    toolboxVM.resetbtn.DisableReason = "Document Locked";
                    toolboxVM.resetbtn.Event = "";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Disable = true;
                    toolboxVM.deletebtn.DisableReason = "Document Locked";
                    toolboxVM.deletebtn.Event = "";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Disable = true;
                    toolboxVM.EmailBtn.DisableReason = "Document Locked";
                    toolboxVM.EmailBtn.Event = "";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Approval History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    //toolboxVM.RecallBtn.Visible = true;
                    //toolboxVM.RecallBtn.Text = "Recall";
                    //toolboxVM.RecallBtn.Title = "Document Recall";
                    //toolboxVM.RecallBtn.Event = "";
                    break;
                case "Recalled":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductionOrder();";

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

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProductionOrder();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";

                    toolboxVM.RecallBtn.Visible = true;
                    toolboxVM.RecallBtn.Text = "Recall";
                    toolboxVM.RecallBtn.Title = "Document Recall";
                    toolboxVM.RecallBtn.Event = "RecallDocumentItem('POD');";
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

				case "DocumentApproverEdit":
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
					//toolboxVM.resetbtn.Disable = true;
					//toolboxVM.resetbtn.DisableReason = "Document Locked";
					toolboxVM.resetbtn.Event = "ResetProductionOrder();";

					toolboxVM.deletebtn.Visible = true;
					toolboxVM.deletebtn.Text = "Delete";
					toolboxVM.deletebtn.Title = "Delete";
					toolboxVM.deletebtn.Disable = true;
					toolboxVM.deletebtn.DisableReason = "Document Locked";
					toolboxVM.deletebtn.Event = "";

					toolboxVM.EmailBtn.Visible = true;
					toolboxVM.EmailBtn.Text = "Email";
					toolboxVM.EmailBtn.Title = "Email";
					toolboxVM.EmailBtn.Disable = true;
					toolboxVM.EmailBtn.DisableReason = "Document Locked";
					toolboxVM.EmailBtn.Event = "";

					toolboxVM.SendForApprovalBtn.Visible = true;
					toolboxVM.SendForApprovalBtn.Text = "Send";
					toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
					toolboxVM.SendForApprovalBtn.Disable = true;
					toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
					toolboxVM.SendForApprovalBtn.Event = "";

					toolboxVM.TimeLine.Visible = true;
					toolboxVM.TimeLine.Text = "TimeLn";
					toolboxVM.TimeLine.Title = "TimeLine";
					toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

					toolboxVM.HistoryBtn.Visible = true;
					toolboxVM.HistoryBtn.Text = "History";
					toolboxVM.HistoryBtn.Title = "Document History";
					toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";


					toolboxVM.PrintBtn.Visible = true;
					toolboxVM.PrintBtn.Text = "Print";
					toolboxVM.PrintBtn.Title = "Print Document";
					toolboxVM.PrintBtn.Disable = true;
					toolboxVM.PrintBtn.DisableReason = "Document Locked";
					toolboxVM.PrintBtn.Event = "PrintProductionOrder()";

					toolboxVM.RecallBtn.Visible = true;
					toolboxVM.RecallBtn.Text = "Recall";
					toolboxVM.RecallBtn.Title = "Document Recall";
					toolboxVM.RecallBtn.Disable = true;
					toolboxVM.PrintBtn.DisableReason = "Document Not Approved";
					toolboxVM.RecallBtn.Event = "";
					break;
				case "ClosedForApprovalApproverEdit":
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
					//toolboxVM.resetbtn.Disable = true;
					//toolboxVM.resetbtn.DisableReason = "Document Locked";
					toolboxVM.resetbtn.Event = "ResetProductionOrder();";

					toolboxVM.deletebtn.Visible = true;
					toolboxVM.deletebtn.Text = "Delete";
					toolboxVM.deletebtn.Title = "Delete";
					toolboxVM.deletebtn.Disable = true;
					toolboxVM.deletebtn.DisableReason = "Document Locked";
					toolboxVM.deletebtn.Event = "";

					toolboxVM.EmailBtn.Visible = true;
					toolboxVM.EmailBtn.Text = "Email";
					toolboxVM.EmailBtn.Title = "Email";
					toolboxVM.EmailBtn.Disable = true;
					toolboxVM.EmailBtn.DisableReason = "Document Locked";
					toolboxVM.EmailBtn.Event = "";

					toolboxVM.SendForApprovalBtn.Visible = true;
					toolboxVM.SendForApprovalBtn.Text = "Send";
					toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
					toolboxVM.SendForApprovalBtn.Disable = true;
					toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
					toolboxVM.SendForApprovalBtn.Event = "";

					toolboxVM.TimeLine.Visible = true;
					toolboxVM.TimeLine.Text = "TimeLn";
					toolboxVM.TimeLine.Title = "TimeLine";
					toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','POD');";

					toolboxVM.HistoryBtn.Visible = true;
					toolboxVM.HistoryBtn.Text = "History";
					toolboxVM.HistoryBtn.Title = "Document History";
					toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','POD');";


					toolboxVM.PrintBtn.Visible = true;
					toolboxVM.PrintBtn.Text = "Print";
					toolboxVM.PrintBtn.Title = "Print Document";
					toolboxVM.PrintBtn.Disable = true;
					toolboxVM.PrintBtn.DisableReason = "Document Locked";
					toolboxVM.PrintBtn.Event = "PrintProductionOrder()";

					toolboxVM.RecallBtn.Visible = true;
					toolboxVM.RecallBtn.Text = "Recall";
					toolboxVM.RecallBtn.Title = "Document Recall";
					toolboxVM.RecallBtn.Event = "RecallDocumentItem('POD');";
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