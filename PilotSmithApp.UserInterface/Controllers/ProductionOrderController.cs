using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace PilotSmithApp.UserInterface.Controllers
{
    public class ProductionOrderController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProductionOrderBusiness _productionOrderBusiness;
        ISaleOrderBusiness _saleOrderBusiness;
        ICommonBusiness _commonBusiness;
        IBranchBusiness _branchBusiness;
        IAreaBusiness _areaBusiness;
        ICustomerBusiness _customerBusiness;
        IDocumentStatusBusiness _documentStatusBusiness;
        IApprovalStatusBusiness _approvalStatusBusiness;
        private IUserBusiness _userBusiness;

        public ProductionOrderController(IProductionOrderBusiness productionOrderBusiness,ISaleOrderBusiness saleOrderBusiness,ICommonBusiness commonBusiness,
            IAreaBusiness areaBusiness,
            IReferencePersonBusiness referencePersonBusiness, IDocumentStatusBusiness documentStatusBusiness, IUserBusiness userBusiness, IApprovalStatusBusiness approvalStatusBusiness, ICustomerBusiness customerBusiness, IBranchBusiness branchBusiness)
        {
            _productionOrderBusiness = productionOrderBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _commonBusiness = commonBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
            _areaBusiness = areaBusiness;           
            _documentStatusBusiness = documentStatusBusiness;
            _userBusiness = userBusiness;
            _approvalStatusBusiness = approvalStatusBusiness;
        }
        // GET: ProductOrder
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult Index(string id,string saleOrderID)
        {
            ViewBag.ID = id;
            ViewBag.SaleOrderID = saleOrderID;
            ProductionOrderAdvanceSearchViewModel productionOrderAdvanceSearchVM = new ProductionOrderAdvanceSearchViewModel();
            productionOrderAdvanceSearchVM.Area = new AreaViewModel();
            productionOrderAdvanceSearchVM.Area.AreaSelectList = _areaBusiness.GetAreaForSelectList();
            productionOrderAdvanceSearchVM.Customer = new CustomerViewModel();
            productionOrderAdvanceSearchVM.Customer.CustomerSelectList = _customerBusiness.GetCustomerSelectList();            
            productionOrderAdvanceSearchVM.Branch = new BranchViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            productionOrderAdvanceSearchVM.Branch.BranchList = _branchBusiness.GetBranchForSelectList(appUA.UserName);
            productionOrderAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            productionOrderAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("POD");
            productionOrderAdvanceSearchVM.ApprovalStatus = new ApprovalStatusViewModel();
            productionOrderAdvanceSearchVM.ApprovalStatus.ApprovalStatusSelectList = _approvalStatusBusiness.GetSelectListForApprovalStatus();
            productionOrderAdvanceSearchVM.PSAUser = new PSAUserViewModel();
            //List<SelectListItem> selectListItem = new List<SelectListItem>();
           // List<SelectListItem> selectListItem = null;
            List<PSAUserViewModel> PSAUserVMList = Mapper.Map<List<SAMTool.DataAccessObject.DTO.User>, List<PSAUserViewModel>>(_userBusiness.GetAllUsers());
            productionOrderAdvanceSearchVM.PSAUser.UserSelectList = PSAUserVMList != null ? (from PSAuserVM in PSAUserVMList
                                                      select new SelectListItem
                                                      {
                                                          Text = PSAuserVM.UserName,
                                                          Value = PSAuserVM.ID.ToString(),
                                                          Selected = false
                                                      }).ToList() : new List<SelectListItem>();           

            return View(productionOrderAdvanceSearchVM);
        }

        #region ProductionOrderForm Form
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ProductionOrderForm(Guid id,Guid? saleOrderID)
        {
            ProductionOrderViewModel productionOrderVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    productionOrderVM = Mapper.Map<ProductionOrder, ProductionOrderViewModel>(_productionOrderBusiness.GetProductionOrder(id));
                    productionOrderVM.IsUpdate = true;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    productionOrderVM.IsDocLocked = productionOrderVM.DocumentOwners.Contains(appUA.UserName);
                    productionOrderVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleOrderID);
                }
                else if (id == Guid.Empty && saleOrderID==null)
                {
                    productionOrderVM = new ProductionOrderViewModel();
                    productionOrderVM.IsUpdate = false;
                    productionOrderVM.ID = Guid.Empty;                   
                    productionOrderVM.SaleOrderID = null;
                    productionOrderVM.SaleOrderSelectList = new List<SelectListItem>();
                    productionOrderVM.DocumentStatus = new DocumentStatusViewModel();
                    productionOrderVM.DocumentStatus.Description = "OPEN";
                    productionOrderVM.Branch = new BranchViewModel();
                    productionOrderVM.Branch.Description = "-";
                    productionOrderVM.IsDocLocked = false;
                }     
                else if(id==Guid.Empty && saleOrderID!=null)
                {
                    SaleOrderViewModel saleOrderVM = Mapper.Map<SaleOrder, SaleOrderViewModel>(_saleOrderBusiness.GetSaleOrder((Guid)saleOrderID));
                    productionOrderVM = new ProductionOrderViewModel();
                    productionOrderVM.IsUpdate = false;
                    productionOrderVM.ID = Guid.Empty;                  
                    productionOrderVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleOrderID);
                    productionOrderVM.SaleOrderID = saleOrderID;
                    productionOrderVM.CustomerID = saleOrderVM.CustomerID;
                    productionOrderVM.DocumentStatus = new DocumentStatusViewModel();
                    productionOrderVM.DocumentStatus.Description = "OPEN";
                    productionOrderVM.Branch = new BranchViewModel();
                    productionOrderVM.IsDocLocked = false;
                    productionOrderVM.Branch.Description = "-";
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
            productionOrderDetailVM.OrderQty = 0;
            productionOrderDetailVM.ProducedQty = 0;
            productionOrderDetailVM.PrevProducedQty = 0;        
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

        #region Get ProductionOrder SelectList On Demand
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
                        PrevProducedQty=0,
                        PrevDelQty=0,
                        Unit=new UnitViewModel()
                        {
                            Code=0,
                            Description=string.Empty,
                        },
                        Rate=0,
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
                    List<SaleOrderDetailViewModel> saleOrderDetailVMList = Mapper.Map<List<SaleOrderDetail>, List<SaleOrderDetailViewModel>>(_saleOrderBusiness.GetSaleOrderDetailListBySaleOrderID(saleOrderID));
                    productionOrderItemViewModelList = (from saleOrderDetailVM in saleOrderDetailVMList
                                                        select new ProductionOrderDetailViewModel
                                                        {
                                                            ID = Guid.Empty,
                                                            ProdOrderID = Guid.Empty,
                                                            ProductID = saleOrderDetailVM.ProductID,
                                                            ProductModelID = saleOrderDetailVM.ProductModelID,
                                                            ProductSpec = saleOrderDetailVM.ProductSpec,
                                                            OrderQty = saleOrderDetailVM.Qty,
                                                            UnitCode = saleOrderDetailVM.UnitCode,
                                                            ProducedQty = 0,
                                                            PrevProducedQty = saleOrderDetailVM.PrevProduceQty,
                                                            Rate = saleOrderDetailVM.Rate == null ? 0 : saleOrderDetailVM.Rate,
                                                            Amount = 0,
                                                            SpecTag=saleOrderDetailVM.SpecTag,
                                                            Product = new ProductViewModel()
                                                            {
                                                                ID = (Guid)saleOrderDetailVM.ProductID,
                                                                Code = saleOrderDetailVM.Product.Code,
                                                                Name = saleOrderDetailVM.Product.Name,
                                                            },
                                                            ProductModel = new ProductModelViewModel()
                                                            {
                                                                ID = (Guid)saleOrderDetailVM.ProductModelID,
                                                                Name = saleOrderDetailVM.ProductModel.Name
                                                            },
                                                            Unit = new UnitViewModel()
                                                            {
                                                                Description = saleOrderDetailVM.Unit.Description
                                                            },
                                                        }).ToList();
                             
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
                object result = _productionOrderBusiness.UpdateProductionOrderEmailInfo(Mapper.Map<ProductionOrderViewModel, ProductionOrder>(productionOrderVM));

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
            productionOrderVM.PDFTools = new PDFTools();
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
                        result = _productionOrderBusiness.UpdateProductionOrderEmailInfo(Mapper.Map<ProductionOrderViewModel, ProductionOrder>(productionOrderVM));
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

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionOrder", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType,Guid? id)
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

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('POD');";
                    break;

                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Disable = true;
                    toolboxVM.addbtn.DisableReason = "Document Locked";
                    toolboxVM.addbtn.Event = "";

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

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";
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