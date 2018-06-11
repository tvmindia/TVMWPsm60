using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class SaleInvoiceController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ISaleInvoiceBusiness _saleInvoiceBusiness;      
        ISaleOrderBusiness _saleOrderBusiness;
        IQuotationBusiness _quotationBusiness;
        ICommonBusiness _commonBusiness;       
        IDocumentStatusBusiness _documentStatusBusiness;
        public SaleInvoiceController(ISaleInvoiceBusiness saleInvoiceBusiness,
           IQuotationBusiness quotationBusiness,
           ISaleOrderBusiness saleOrderBusiness, 
           ICommonBusiness commonBusiness,
           IDocumentStatusBusiness documenStatusBusiness           
            )
        {
            _saleInvoiceBusiness = saleInvoiceBusiness;            
            _quotationBusiness = quotationBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _commonBusiness = commonBusiness;
            _documentStatusBusiness = documenStatusBusiness;            
        }
        // GET: SaleInvoice
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult Index()
        {
            SaleInvoiceAdvanceSearchViewModel saleInvoiceAdvanceSearchVM = new SaleInvoiceAdvanceSearchViewModel();            
            saleInvoiceAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            saleInvoiceAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("SIV");           
            return View(saleInvoiceAdvanceSearchVM);
        }


        #region SaleInvoice Other Charge Detail 
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult SaleInvoiceOtherChargeDetail()
        {
            SaleInvoiceOtherChargeViewModel saleInvocieOtherChargeVM = new SaleInvoiceOtherChargeViewModel();
            saleInvocieOtherChargeVM.IsUpdate = false;
            return PartialView("_SaleInvoiceOtherCharge", saleInvocieOtherChargeVM);
        }
        #endregion SaleInvoice Other Charge Detail 

        #region SaleInvoice Form
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult SaleInvoiceForm(Guid id, Guid? saleorderID, Guid? quotationID)
        {
            SaleInvoiceViewModel saleInvoiceVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    saleInvoiceVM = Mapper.Map<SaleInvoice, SaleInvoiceViewModel>(_saleInvoiceBusiness.GetSaleInvoice(id));
                    saleInvoiceVM.IsUpdate = true;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    saleInvoiceVM.IsDocLocked = saleInvoiceVM.DocumentOwners.Contains(appUA.UserName);

                    if (saleInvoiceVM.QuoteID != Guid.Empty && saleInvoiceVM.QuoteID != null)
                    {
                        saleInvoiceVM.DocumentType = "Quotation";
                        saleInvoiceVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quotationID);
                    }
                    if (saleInvoiceVM.SaleOrderID != null && saleInvoiceVM.SaleOrderID != Guid.Empty)
                    {
                        saleInvoiceVM.DocumentType = "SaleOrder";
                        saleInvoiceVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleorderID);
                    }
                }
                else if (id == Guid.Empty && quotationID != null)
                {
                    QuotationViewModel quotationVM = Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation((Guid)quotationID));
                    saleInvoiceVM = new SaleInvoiceViewModel();
                    saleInvoiceVM.IsUpdate = false;
                    saleInvoiceVM.ID = Guid.Empty; 
                    saleInvoiceVM.DocumentType = "Quotation";
                    saleInvoiceVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quotationID);
                    saleInvoiceVM.QuoteID = quotationID;
                    saleInvoiceVM.SaleOrderSelectList = new List<SelectListItem>();
                    saleInvoiceVM.CustomerID = quotationVM.CustomerID;
                    saleInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "OPEN",
                    };
                    saleInvoiceVM.Branch = new BranchViewModel();
                    saleInvoiceVM.Branch.Description = "-";
                    saleInvoiceVM.IsDocLocked = false;
                }
                else if (id == Guid.Empty && saleorderID != null)
                {
                    SaleOrderViewModel saleorderVM = Mapper.Map<SaleOrder, SaleOrderViewModel>(_saleOrderBusiness.GetSaleOrder((Guid)saleorderID));
                    saleInvoiceVM = new SaleInvoiceViewModel();
                    saleInvoiceVM.IsUpdate = false;
                    saleInvoiceVM.ID = Guid.Empty;
                    saleInvoiceVM.DocumentType = "SaleOrder";
                    saleInvoiceVM.QuotationSelectList = new List<SelectListItem>();
                    saleInvoiceVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleorderID);
                    saleInvoiceVM.SaleOrderID = saleorderID;
                    saleInvoiceVM.CustomerID = saleorderVM.CustomerID;
                    saleInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "OPEN",
                    };
                    saleInvoiceVM.Branch = new BranchViewModel();
                    saleInvoiceVM.Branch.Description = "-";
                    saleInvoiceVM.IsDocLocked = false;
                }
                else   
                {
                    saleInvoiceVM = new SaleInvoiceViewModel();
                    saleInvoiceVM.IsUpdate = false;
                    saleInvoiceVM.ID = Guid.Empty;
                    saleInvoiceVM.DocumentType = "Quotation";
                    saleInvoiceVM.QuotationSelectList = new List<SelectListItem>();
                    saleInvoiceVM.SaleOrderSelectList = new List<SelectListItem>();
                    saleInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "OPEN",
                    };
                    saleInvoiceVM.Branch = new BranchViewModel();
                    saleInvoiceVM.Branch.Description = "-";
                    saleInvoiceVM.IsDocLocked = false;
                }
                saleInvoiceVM.Customer = new CustomerViewModel
                {
                    //Titles = new TitlesViewModel()
                    //{
                    //    TitlesSelectList = _customerBusiness.GetTitleSelectList(),
                    //},
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_SaleInvoiceForm", saleInvoiceVM);
        }
        #endregion SaleInvoice Form

        #region SaleInvoice Detail Add
        public ActionResult AddSaleInvoiceDetail()
        {
            SaleInvoiceDetailViewModel saleInvoiceDetailVM = new SaleInvoiceDetailViewModel();
            saleInvoiceDetailVM.IsUpdate = false;
            return PartialView("_AddSaleInvoiceDetail", saleInvoiceDetailVM);
        }
        #endregion SaleInvoice Detail Add

        #region Get SaleInvoice DetailList By SaleInvoiceID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            try
            {
                List<SaleInvoiceDetailViewModel> saleInvoiceItemViewModelList = new List<SaleInvoiceDetailViewModel>();
                if (saleInvoiceID == Guid.Empty)
                {
                    SaleInvoiceDetailViewModel saleInvoiceDetailVM = new SaleInvoiceDetailViewModel()
                    {
                        ID = Guid.Empty,
                        //QuoteID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        Qty = 0,
                        UnitCode = null,
                        Rate = 0,
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
                        Unit = new UnitViewModel()
                        {
                            Description = null,
                        },
                        TaxType = new TaxTypeViewModel()
                        {
                            ValueText = "",
                        }
                    };
                    saleInvoiceItemViewModelList.Add(saleInvoiceDetailVM);
                }
                else
                {
                    saleInvoiceItemViewModelList = Mapper.Map<List<SaleInvoiceDetail>, List<SaleInvoiceDetailViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceDetailListBySaleInvoiceID(saleInvoiceID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleInvoiceItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleInvoice DetailList By SaleInvoiceID

        #region Get SaleInvoice DetailList By SaleOrderID From saleOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceDetailListBySaleOrderIDFromSaleOrder(Guid saleOrderID)
        {
            try
            {
                List<SaleInvoiceDetailViewModel> saleInvoiceItemViewModelList = new List<SaleInvoiceDetailViewModel>();
                if (saleOrderID != Guid.Empty)
                {
                    List<SaleOrderDetailViewModel> saleOrderVMList = Mapper.Map<List<SaleOrderDetail>, List<SaleOrderDetailViewModel>>(_saleOrderBusiness.GetSaleOrderDetailListBySaleOrderID(saleOrderID));
                    foreach (SaleOrderDetailViewModel saleOrderDetailVM in saleOrderVMList)
                    {
                        SaleInvoiceDetailViewModel saleInvoiceDetailVM = new SaleInvoiceDetailViewModel()
                        {
                            ID = Guid.Empty,
                            //QuoteID = Guid.Empty,
                            ProductID = saleOrderDetailVM.ProductID,
                            ProductModelID = saleOrderDetailVM.ProductModelID,
                            ProductSpec = saleOrderDetailVM.ProductSpec,
                            Qty = saleOrderDetailVM.Qty,
                            UnitCode = saleOrderDetailVM.UnitCode,
                            Rate = saleOrderDetailVM.Rate,
                            CGSTPerc = saleOrderDetailVM.CGSTPerc,
                            SGSTPerc = saleOrderDetailVM.SGSTPerc,
                            IGSTPerc = saleOrderDetailVM.IGSTPerc,
                            Discount = saleOrderDetailVM.Discount,
                            CessAmt = saleOrderDetailVM.CessAmt,
                            CessPerc = saleOrderDetailVM.CessPerc,
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
                                Description = saleOrderDetailVM.Unit.Description,
                            },
                            TaxType = new TaxTypeViewModel()
                            {
                                ValueText = "",
                            }
                        };
                        saleInvoiceItemViewModelList.Add(saleInvoiceDetailVM);
                    }
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleInvoiceItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleInvoice DetailList By SaleOrderID From saleOrder

        #region Get SaleInvoice DetailList By QuotationID From Quotation
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceDetailListByQuotationIDFromQuotation(Guid quoteID)
        {
            try
            {
                List<SaleInvoiceDetailViewModel> saleInvoiceItemViewModelList = new List<SaleInvoiceDetailViewModel>();
                if (quoteID != Guid.Empty)
                {
                    List<QuotationDetailViewModel> quotationDetailVMList = Mapper.Map<List<QuotationDetail>, List<QuotationDetailViewModel>>(_quotationBusiness.GetQuotationDetailListByQuotationID(quoteID));
                    saleInvoiceItemViewModelList = (from quotationDetailVM in quotationDetailVMList
                                                  select new SaleInvoiceDetailViewModel
                                                  {
                                                      ID = Guid.Empty,
                                                      SaleInvID = Guid.Empty,
                                                      ProductID = quotationDetailVM.ProductID,
                                                      ProductModelID = quotationDetailVM.ProductModelID,
                                                      ProductSpec = quotationDetailVM.ProductSpec,
                                                      Qty = quotationDetailVM.Qty,
                                                      UnitCode = quotationDetailVM.UnitCode,
                                                      Rate = quotationDetailVM.Rate,
                                                     // SpecTag = quotationDetailVM.SpecTag,
                                                      Discount = quotationDetailVM.Discount,
                                                      TaxTypeCode = quotationDetailVM.TaxTypeCode,
                                                      CGSTPerc = quotationDetailVM.CGSTPerc,
                                                      SGSTPerc = quotationDetailVM.SGSTPerc,
                                                      IGSTPerc = quotationDetailVM.IGSTPerc,
                                                      CessAmt = 0,
                                                      CessPerc = 0,
                                                      Product = new ProductViewModel()
                                                      {
                                                          ID = (Guid)quotationDetailVM.ProductID,
                                                          Code = quotationDetailVM.Product.Code,
                                                          Name = quotationDetailVM.Product.Name,
                                                      },
                                                      ProductModel = new ProductModelViewModel()
                                                      {
                                                          ID = (Guid)quotationDetailVM.ProductModelID,
                                                          Name = quotationDetailVM.ProductModel.Name
                                                      }, 
                                                      Unit = new UnitViewModel()
                                                      {
                                                          Description = quotationDetailVM.Unit.Description
                                                      },
                                                      TaxType = new TaxTypeViewModel()
                                                      {
                                                          //Code=(int)quotationDetailVM.TaxTypeCode,
                                                          Description = quotationDetailVM.TaxType.Description,
                                                          ValueText = quotationDetailVM.TaxType.ValueText
                                                      },
                                                  }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleInvoiceItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleInvoice DetailList By QuotationID From Quotation

        #region Get Quotation OtherChargeList By QuotationID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetQuotationOtherChargesDetailListByQuotationID(Guid quotationID)
        {
            try
            {
                List<QuotationOtherChargeViewModel> quotationOtherChargeViewModelList = new List<QuotationOtherChargeViewModel>();
                if (quotationID == Guid.Empty)
                {
                    QuotationOtherChargeViewModel quotationOtherChargeVM = new QuotationOtherChargeViewModel()
                    {
                        ID = Guid.Empty,
                        QuoteID = Guid.Empty,
                        ChargeAmount = 0,
                        OtherCharge = new OtherChargeViewModel()
                        {
                            Description = "",
                        },
                        TaxType = new TaxTypeViewModel()
                        {
                            ValueText = "",
                        }
                    };
                    quotationOtherChargeViewModelList.Add(quotationOtherChargeVM);
                }
                else
                {
                    quotationOtherChargeViewModelList = Mapper.Map<List<QuotationOtherCharge>, List<QuotationOtherChargeViewModel>>(_quotationBusiness.GetQuotationOtherChargesDetailListByQuotationID(quotationID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = quotationOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Quotation OtherChargeList By QuotationID

        #region Get SaleInvoice OtherChargeList By SaleInvoiceID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(Guid saleInvoiceID)
        {
            try
            {
                List<SaleInvoiceOtherChargeViewModel> saleOrderOtherChargeViewModelList = new List<SaleInvoiceOtherChargeViewModel>();
                if (saleInvoiceID == Guid.Empty)
                {
                    SaleInvoiceOtherChargeViewModel saleOrderOtherChargeVM = new SaleInvoiceOtherChargeViewModel()
                    {
                        ID = Guid.Empty,
                        SaleInvID = Guid.Empty,
                        ChargeAmount = 0,
                        OtherCharge = new OtherChargeViewModel()
                        {
                            Description = "",
                        },
                        TaxType = new TaxTypeViewModel()
                        {
                            ValueText = "",
                        }
                    };
                    saleOrderOtherChargeViewModelList.Add(saleOrderOtherChargeVM);
                }
                else
                {
                    saleOrderOtherChargeViewModelList = Mapper.Map<List<SaleInvoiceOtherCharge>, List<SaleInvoiceOtherChargeViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(saleInvoiceID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleOrderOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleOrder OtherChargeList By SaleOrderID

        #region Get SaleInvoice OtherCharge DetailList By QuotationID From Quotation
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceOtherChargesDetailListByQuotationIDFromQuotation(Guid quoteID)
        {
            try
            {
                List<SaleInvoiceOtherChargeViewModel> saleInvoiceOtherChargeViewModelList = new List<SaleInvoiceOtherChargeViewModel>();
                if (quoteID != Guid.Empty)
                {
                    List<QuotationOtherChargeViewModel> quotationOtherChargeVMList = Mapper.Map<List<QuotationOtherCharge>, List<QuotationOtherChargeViewModel>>(_quotationBusiness.GetQuotationOtherChargesDetailListByQuotationID(quoteID));
                    saleInvoiceOtherChargeViewModelList = (from quotationOtherChargeVM in quotationOtherChargeVMList
                                                         select new SaleInvoiceOtherChargeViewModel
                                                         {
                                                             ID = Guid.Empty,
                                                             SaleInvID = Guid.Empty,
                                                             OtherChargeCode = quotationOtherChargeVM.OtherChargeCode,
                                                             ChargeAmount = quotationOtherChargeVM.ChargeAmount,
                                                             TaxTypeCode = quotationOtherChargeVM.TaxTypeCode,
                                                             CGSTPerc = quotationOtherChargeVM.CGSTPerc,
                                                             SGSTPerc = quotationOtherChargeVM.SGSTPerc,
                                                             IGSTPerc = quotationOtherChargeVM.IGSTPerc,
                                                             AddlTaxPerc = 0,
                                                             AddlTaxAmt = 0,
                                                             OtherCharge = new OtherChargeViewModel()
                                                             {
                                                                 Description = quotationOtherChargeVM.OtherCharge.Description
                                                             },
                                                             TaxType = new TaxTypeViewModel()
                                                             {
                                                                 Code = quotationOtherChargeVM.TaxType.Code,
                                                                 ValueText = quotationOtherChargeVM.TaxType.ValueText
                                                             },
                                                         }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleInvoiceOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleOrder DetailList By SaleOrderID with Quotation

        #region Get SaleInvoice OtherCharge DetailList By SaleOrderID From SaleOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceOtherChargesDetailListBySaleOrderIDFromSaleOrder(Guid saleOrderID)
        {
            try
            {
                List<SaleInvoiceOtherChargeViewModel> saleInvoiceOtherChargeViewModelList = new List<SaleInvoiceOtherChargeViewModel>();
                if (saleOrderID != Guid.Empty)
                {
                    List<SaleOrderOtherChargeViewModel> saleOrderOtherChargeVMList = Mapper.Map<List<SaleOrderOtherCharge>, List<SaleOrderOtherChargeViewModel>>(_saleOrderBusiness.GetSaleOrderOtherChargesDetailListBySaleOrderID(saleOrderID));
                    saleInvoiceOtherChargeViewModelList = (from quotationOtherChargeVM in saleOrderOtherChargeVMList
                                                           select new SaleInvoiceOtherChargeViewModel
                                                           {
                                                               ID = Guid.Empty,
                                                               SaleInvID = Guid.Empty,
                                                               OtherChargeCode = quotationOtherChargeVM.OtherChargeCode,
                                                               ChargeAmount = quotationOtherChargeVM.ChargeAmount,
                                                               TaxTypeCode = quotationOtherChargeVM.TaxTypeCode,
                                                               CGSTPerc = quotationOtherChargeVM.CGSTPerc,
                                                               SGSTPerc = quotationOtherChargeVM.SGSTPerc,
                                                               IGSTPerc = quotationOtherChargeVM.IGSTPerc,
                                                               AddlTaxPerc = 0,
                                                               AddlTaxAmt = 0,
                                                               OtherCharge = new OtherChargeViewModel()
                                                               {
                                                                   Description = quotationOtherChargeVM.OtherCharge.Description
                                                               },
                                                               TaxType = new TaxTypeViewModel()
                                                               {
                                                                   Code = quotationOtherChargeVM.TaxType.Code,
                                                                   ValueText = quotationOtherChargeVM.TaxType.ValueText
                                                               },
                                                           }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleInvoiceOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleOrder DetailList By SaleOrderID with Quotation

        #region Delete SaleInvoice
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "D")]
        public string DeleteSaleInvoice(Guid id)
        {

            try
            {
                object result = _saleInvoiceBusiness.DeleteSaleInvoice(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete SaleInvoice
        #region Delete SaleInvoice Detail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "D")]
        public string DeleteSaleInvoiceDetail(Guid id)
        {

            try
            {
                object result = _saleInvoiceBusiness.DeleteSaleInvoiceDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete SaleInvoice Detail

        #region Delete SaleInvoice OtherChargeDetail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "D")]
        public string DeleteSaleInvoiceOtherChargeDetail(Guid id)
        {

            try
            {
                object result = _saleInvoiceBusiness.DeleteSaleInvoiceOtherChargeDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion Delete SaleInvoice OtherChargeDetail

        #region GetAllSaleInvoice
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public JsonResult GetAllSaleInvoice(DataTableAjaxPostModel model, SaleInvoiceAdvanceSearchViewModel SaleInvoiceAdvanceSearchVM)
        {
            //setting options to our model
            SaleInvoiceAdvanceSearchVM.DataTablePaging.Start = model.start;
            SaleInvoiceAdvanceSearchVM.DataTablePaging.Length = (SaleInvoiceAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : SaleInvoiceAdvanceSearchVM.DataTablePaging.Length;

            //SaleInvoiceAdvanceSearchVM.OrderColumn = model.order[0].column;
            //SaleInvoiceAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<SaleInvoiceViewModel> SaleInvoiceVMList = Mapper.Map<List<SaleInvoice>, List<SaleInvoiceViewModel>>(_saleInvoiceBusiness.GetAllSaleInvoice(Mapper.Map<SaleInvoiceAdvanceSearchViewModel, SaleInvoiceAdvanceSearch>(SaleInvoiceAdvanceSearchVM)));
            if (SaleInvoiceAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = SaleInvoiceVMList.Count != 0 ? SaleInvoiceVMList[0].TotalCount : 0;
                int filteredResult = SaleInvoiceVMList.Count != 0 ? SaleInvoiceVMList[0].FilteredCount : 0;
                SaleInvoiceVMList = SaleInvoiceVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = SaleInvoiceVMList.Count != 0 ? SaleInvoiceVMList[0].TotalCount : 0,
                recordsFiltered = SaleInvoiceVMList.Count != 0 ? SaleInvoiceVMList[0].FilteredCount : 0,
                data = SaleInvoiceVMList
            });
        }
        #endregion GetAllSaleInvoice
        #region InsertUpdateSaleInvoice
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string InsertUpdateSaleInvoice(SaleInvoiceViewModel saleInvoiceVM)
        {
            //object resultFromBusiness = null;

            try
            {
                object ResultFromJS;
                string ReadableFormat;
                AppUA appUA = Session["AppUA"] as AppUA;
                saleInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
                saleInvoiceVM.PSASysCommon.CreatedBy = appUA.UserName;
                saleInvoiceVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                saleInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
                saleInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                ResultFromJS = JsonConvert.DeserializeObject(saleInvoiceVM.DetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                saleInvoiceVM.SaleInvoiceDetailList = JsonConvert.DeserializeObject<List<SaleInvoiceDetailViewModel>>(ReadableFormat);
                ResultFromJS = JsonConvert.DeserializeObject(saleInvoiceVM.OtherChargesDetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                saleInvoiceVM.SaleInvoiceOtherChargeDetailList = JsonConvert.DeserializeObject<List<SaleInvoiceOtherChargeViewModel>>(ReadableFormat);

                object result = _saleInvoiceBusiness.InsertUpdateSaleInvoice(Mapper.Map<SaleInvoiceViewModel, SaleInvoice>(saleInvoiceVM));

                if (saleInvoiceVM.ID == Guid.Empty)
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

        #endregion InsertUpdateSaleInvoice
        //#region UpdateSaleInvoiceEmailInfo
        //[HttpPost]
        //[AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        //public string UpdateSaleInvoiceEmailInfo(SaleInvoiceViewModel saleInvoiceVM)
        //{
        //    try
        //    {
        //        AppUA appUA = Session["AppUA"] as AppUA;
        //        saleInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
        //        saleInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
        //        saleInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
        //        object result = _saleInvoiceBusiness.UpdateSaleInvoiceEmailInfo(Mapper.Map<SaleInvoiceViewModel, SaleInvoice>(saleInvoiceVM));

        //        if (saleInvoiceVM.ID == Guid.Empty)
        //        {
        //            return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Insertion successfull" });
        //        }
        //        else
        //        {
        //            return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Updation successfull" });
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        AppConstMessage cm = _appConstant.GetMessage(ex.Message);
        //        return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
        //    }

        //}

        //#endregion UpdateSaleInvoiceEmailInfo

        //#region Email SaleInvoice
        //public ActionResult EmailSaleInvoice(SaleInvoiceViewModel saleInvoiceVM)
        //{
        //    bool emailFlag = saleInvoiceVM.EmailFlag;
        //    //SaleInvoiceViewModel saleInvoiceVM = new SaleInvoiceViewModel();
        //    saleInvoiceVM = Mapper.Map<SaleInvoice, SaleInvoiceViewModel>(_saleInvoiceBusiness.GetSaleInvoice(saleInvoiceVM.ID));
        //    saleInvoiceVM.SaleInvoiceDetailList = Mapper.Map<List<SaleInvoiceDetail>, List<SaleInvoiceDetailViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceDetailListBySaleInvoiceID(saleInvoiceVM.ID));
        //    saleInvoiceVM.EmailFlag = emailFlag;
        //    @ViewBag.path = "http://" + HttpContext.Request.Url.Authority + "/Content/images/logo1.PNG";
        //    saleInvoiceVM.PDFTools = new PDFTools();
        //    return PartialView("_EmailSaleInvoice", saleInvoiceVM);
        //}
        //#endregion Email SaleInvoice
        //#region EmailSent
        //[HttpPost, ValidateInput(false)]
        //[AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        //public async Task<string> SendSaleInvoiceEmail(SaleInvoiceViewModel saleInvoiceVM)
        //{
        //    try
        //    {
        //        object result = null;
        //        if (!string.IsNullOrEmpty(saleInvoiceVM.ID.ToString()))
        //        {
        //            AppUA appUA = Session["AppUA"] as AppUA;
        //            saleInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
        //            saleInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
        //            saleInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();

        //            bool sendsuccess = await _saleInvoiceBusiness.QuoteEmailPush(Mapper.Map<SaleInvoiceViewModel, SaleInvoice>(saleInvoiceVM));
        //            if (sendsuccess)
        //            {
        //                //1 is meant for mail sent successfully
        //                saleInvoiceVM.EmailSentYN = sendsuccess;
        //                result = _saleInvoiceBusiness.UpdateSaleInvoiceEmailInfo(Mapper.Map<SaleInvoiceViewModel, SaleInvoice>(saleInvoiceVM));
        //            }
        //            return JsonConvert.SerializeObject(new { Status = "OK", Record = result, MailResult = sendsuccess, Message = _appConstant.MailSuccess });
        //        }
        //        else
        //        {

        //            return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID is Missing" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppConstMessage cm = _appConstant.GetMessage(ex.Message);
        //        return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
        //    }
        //}
        //#endregion EmailSent
        //#region Get QUotation SelectList On Demand
        //public ActionResult GetSaleInvoiceSelectListOnDemand(string searchTerm)
        //{
        //    List<SaleInvoice> saleInvoiceList = string.IsNullOrEmpty(searchTerm) ? null : _saleInvoiceBusiness.GetSaleInvoiceForSelectListOnDemand(searchTerm);
        //    var list = new List<Select2Model>();
        //    if (saleInvoiceList != null)
        //    {
        //        foreach (SaleInvoice saleInvoice in saleInvoiceList)
        //        {
        //            list.Add(new Select2Model()
        //            {
        //                text = saleInvoice.QuoteNo,
        //                id = saleInvoice.ID.ToString()
        //            });
        //        }
        //    }
        //    return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        //}
        //#endregion Get SaleInvoice SelectList On Demand

        #region Email SaleInvoice
        public ActionResult EmailSaleInvoice(SaleInvoiceViewModel saleInvoiceVM)
        {
            bool emailFlag = saleInvoiceVM.EmailFlag;
            //SaleInvoiceViewModel saleInvoiceVM = new SaleInvoiceViewModel();
            saleInvoiceVM = Mapper.Map<SaleInvoice, SaleInvoiceViewModel>(_saleInvoiceBusiness.GetSaleInvoice(saleInvoiceVM.ID));
            saleInvoiceVM.SaleInvoiceDetailList = Mapper.Map<List<SaleInvoiceDetail>, List<SaleInvoiceDetailViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceDetailListBySaleInvoiceID(saleInvoiceVM.ID));
            saleInvoiceVM.SaleInvoiceOtherChargeDetailList = Mapper.Map<List<SaleInvoiceOtherCharge>, List<SaleInvoiceOtherChargeViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(saleInvoiceVM.ID));
            saleInvoiceVM.EmailFlag = emailFlag;
            @ViewBag.path = "http://" + HttpContext.Request.Url.Authority + "/Content/images/Pilot1.PNG";
            saleInvoiceVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_EmailSaleInvoice", saleInvoiceVM);
        }
        #endregion Email SaleInvoice

        #region EmailSent
        [HttpPost, ValidateInput(false)]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public async Task<string> SendSaleInvoiceEmail(SaleInvoiceViewModel saleInvoiceVM)
        {
            try
            {
                object result = null;
                if (!string.IsNullOrEmpty(saleInvoiceVM.ID.ToString()))
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    saleInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
                    saleInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
                    saleInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();

                    bool sendsuccess = await _saleInvoiceBusiness.QuoteEmailPush(Mapper.Map<SaleInvoiceViewModel, SaleInvoice>(saleInvoiceVM));
                    if (sendsuccess)
                    {
                        //1 is meant for mail sent successfully
                        saleInvoiceVM.EmailSentYN = sendsuccess;
                        result = _saleInvoiceBusiness.UpdateSaleInvoiceEmailInfo(Mapper.Map<SaleInvoiceViewModel, SaleInvoice>(saleInvoiceVM));
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
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSaleInvoice();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportSaleInvoiceData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleInvoiceList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSaleInvoice();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveSaleInvoice();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleInvoice();";

                    if (_commonBusiness.CheckDocumentIsDeletable("SIV", id))
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
                        toolboxVM.deletebtn.Event = "DeleteSaleInvoice();";
                    }

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailSaleInvoice();";

                    toolboxVM.SendForApprovalBtn.Visible = false;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveSaleInvoice();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleInvoice();";

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