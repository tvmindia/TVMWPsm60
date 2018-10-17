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
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ProformaInvoiceController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IProformaInvoiceBusiness _proformaInvoiceBusiness;
        ISaleOrderBusiness _saleOrderBusiness;
        IQuotationBusiness _quotationBusiness;
        ICommonBusiness _commonBusiness;
        IDocumentStatusBusiness _documentStatusBusiness;
        SecurityFilter.ToolBarAccess _tool;
        public ProformaInvoiceController(IProformaInvoiceBusiness proformaInvoiceBusiness,
            ISaleOrderBusiness saleOrderBusiness,
            IQuotationBusiness quotationBusiness,
            ICommonBusiness commonBusiness,
            IDocumentStatusBusiness documentBusiness,SecurityFilter.ToolBarAccess tool
            )
        {
            _proformaInvoiceBusiness = proformaInvoiceBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _quotationBusiness = quotationBusiness;
            _commonBusiness = commonBusiness;
            _documentStatusBusiness = documentBusiness;
            _tool = tool;
        }
        // GET: ProformaInvoice
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
            ProformaInvoiceAdvanceSearchViewModel proformaInvoiceAdvanceSearchVM = new ProformaInvoiceAdvanceSearchViewModel();
            proformaInvoiceAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            proformaInvoiceAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("PIV");
            return View(proformaInvoiceAdvanceSearchVM);
        }

        #region ProformaInvoice Other Charge Detail 
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public ActionResult ProformaInvoiceOtherChargeDetail(bool update)
        {
            ProformaInvoiceOtherChargeViewModel proformaInvocieOtherChargeVM = new ProformaInvoiceOtherChargeViewModel();
            proformaInvocieOtherChargeVM.IsUpdate = update;
            return PartialView("_ProformaInvoiceOtherCharge", proformaInvocieOtherChargeVM);
        }
        #endregion ProformaInvoice Other Charge Detail 

        #region ProformaInvoice Form
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public ActionResult ProformaInvoiceForm(Guid id, Guid? saleorderID, Guid? quotationID)
        {
            ProformaInvoiceViewModel proformaInvoiceVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    proformaInvoiceVM = Mapper.Map<ProformaInvoice, ProformaInvoiceViewModel>(_proformaInvoiceBusiness.GetProformaInvoice(id));
                    proformaInvoiceVM.IsUpdate = true;
                    proformaInvoiceVM.PreparedBy = proformaInvoiceVM.PreparedBy != Guid.Empty ? proformaInvoiceVM.PreparedBy : null;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    proformaInvoiceVM.IsDocLocked = proformaInvoiceVM.DocumentOwners.Contains(appUA.UserName);

                    if (proformaInvoiceVM.QuoteID != Guid.Empty && proformaInvoiceVM.QuoteID != null)
                    {
                        proformaInvoiceVM.DocumentType = "Quotation";
                        proformaInvoiceVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quotationID);
                    }
                    if (proformaInvoiceVM.SaleOrderID != null && proformaInvoiceVM.SaleOrderID != Guid.Empty)
                    {
                        proformaInvoiceVM.DocumentType = "SaleOrder";
                        proformaInvoiceVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleorderID);
                    }
                }
                else if (id == Guid.Empty && quotationID != null)
                {
                    QuotationViewModel quotationVM = Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation((Guid)quotationID));
                    proformaInvoiceVM = new ProformaInvoiceViewModel();
                    proformaInvoiceVM.IsUpdate = false;
                    proformaInvoiceVM.ID = Guid.Empty;
                    proformaInvoiceVM.DocumentType = "Quotation";
                    proformaInvoiceVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quotationID);
                    proformaInvoiceVM.QuoteID = quotationID;
                    proformaInvoiceVM.Discount = quotationVM.Discount;
                    proformaInvoiceVM.SaleOrderSelectList = new List<SelectListItem>();
                    proformaInvoiceVM.CustomerID = quotationVM.CustomerID;
                    proformaInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-",
                    };
                    proformaInvoiceVM.Branch = new BranchViewModel();
                    proformaInvoiceVM.Branch.Description = "-";
                    proformaInvoiceVM.Customer = quotationVM.Customer;
                    proformaInvoiceVM.IsDocLocked = false;
                }
                else if (id == Guid.Empty && saleorderID != null)
                {
                    SaleOrderViewModel saleorderVM = Mapper.Map<SaleOrder, SaleOrderViewModel>(_saleOrderBusiness.GetSaleOrder((Guid)saleorderID));
                    proformaInvoiceVM = new ProformaInvoiceViewModel();
                    proformaInvoiceVM.IsUpdate = false;
                    proformaInvoiceVM.ID = Guid.Empty;
                    proformaInvoiceVM.DocumentType = "SaleOrder";
                    proformaInvoiceVM.QuotationSelectList = new List<SelectListItem>();
                    proformaInvoiceVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleorderID);
                    proformaInvoiceVM.SaleOrderID = saleorderID;
                    proformaInvoiceVM.Discount = saleorderVM.Discount;
                    proformaInvoiceVM.CustomerID = saleorderVM.CustomerID;
                    proformaInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-",
                    };
                    proformaInvoiceVM.Branch = new BranchViewModel();
                    proformaInvoiceVM.Branch.Description = "-";
                    proformaInvoiceVM.Customer = saleorderVM.Customer;
                    proformaInvoiceVM.IsDocLocked = false;
                }
                else
                {
                    proformaInvoiceVM = new ProformaInvoiceViewModel();
                    proformaInvoiceVM.IsUpdate = false;
                    proformaInvoiceVM.ID = Guid.Empty;
                    proformaInvoiceVM.DocumentType = "Quotation";
                    proformaInvoiceVM.QuotationSelectList = new List<SelectListItem>();
                    proformaInvoiceVM.SaleOrderSelectList = new List<SelectListItem>();
                    proformaInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-",
                    };
                    proformaInvoiceVM.Branch = new BranchViewModel();
                    proformaInvoiceVM.Branch.Description = "-";
                    proformaInvoiceVM.Customer = new CustomerViewModel();
                    proformaInvoiceVM.Customer.CompanyName = "-";
                    proformaInvoiceVM.IsDocLocked = false;
                }
                //proformaInvoiceVM.Customer = new CustomerViewModel
                //{
                //    //Titles = new TitlesViewModel()
                //    //{
                //    //    TitlesSelectList = _customerBusiness.GetTitleSelectList(),
                //    //},
                //};
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_ProformaInvoiceForm", proformaInvoiceVM);
        }
        #endregion ProformaInvoice Form

        #region ProformaInvoice Detail Add
        public ActionResult AddProformaInvoiceDetail(bool update)
        {
            ProformaInvoiceDetailViewModel proformaInvoiceDetailVM = new ProformaInvoiceDetailViewModel();
            proformaInvoiceDetailVM.IsUpdate = update;
            return PartialView("_AddProformaInvoiceDetail", proformaInvoiceDetailVM);
        }
        #endregion ProformaInvoice Detail Add

        #region ProformaInvoice ServiceBill Add
        public ActionResult AddProformaInvoiceServiceBill(bool update)
        {
            ProformaInvoiceDetailViewModel proformaInvoiceDetailVM = new ProformaInvoiceDetailViewModel();
            proformaInvoiceDetailVM.IsUpdate = update;
            proformaInvoiceDetailVM.Qty = 1;//by default one
            proformaInvoiceDetailVM.UnitCode = 4;////by default select Nos as unit
            return PartialView("_ProformaInvoiceServiceBill", proformaInvoiceDetailVM);
        }
        #endregion ProformaInvoice ServiceBill Add

        #region Get ProformaInvoice DetailList By ProformaInvoiceID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public string GetProformaInvoiceDetailListByProformaInvoiceID(Guid proformaInvoiceID)
        {
            try
            {
                List<ProformaInvoiceDetailViewModel> proformaInvoiceItemViewModelList = new List<ProformaInvoiceDetailViewModel>();
                if (proformaInvoiceID == Guid.Empty)
                {
                    ProformaInvoiceDetailViewModel proformaInvoiceDetailVM = new ProformaInvoiceDetailViewModel()
                    {
                        ID = Guid.Empty,
                        //QuoteID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        Qty = 0,
                        UnitCode = null,
                        Rate = 0,
                        CessAmt = 0,
                        OtherChargeCode = null,
                        OtherCharge = new OtherChargeViewModel()
                        {
                            Description = null
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
                        Unit = new UnitViewModel()
                        {
                            Description = null,
                        },
                        TaxType = new TaxTypeViewModel()
                        {
                            ValueText = "",
                        }
                    };
                    proformaInvoiceItemViewModelList.Add(proformaInvoiceDetailVM);
                }
                else
                {
                    proformaInvoiceItemViewModelList = Mapper.Map<List<ProformaInvoiceDetail>, List<ProformaInvoiceDetailViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceDetailListByProformaInvoiceID(proformaInvoiceID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = proformaInvoiceItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get ProformaInvoice DetailList By ProformaInvoiceID

        #region Get ProformaInvoice DetailList By SaleOrderID From saleOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public string GetProformaInvoiceDetailListBySaleOrderIDFromSaleOrder(Guid saleOrderID)
        {
            try
            {
                List<ProformaInvoiceDetailViewModel> proformaInvoiceItemViewModelList = new List<ProformaInvoiceDetailViewModel>();
                if (saleOrderID != Guid.Empty)
                {
                    List<SaleOrderDetailViewModel> saleOrderVMList = Mapper.Map<List<SaleOrderDetail>, List<SaleOrderDetailViewModel>>(_saleOrderBusiness.GetSaleOrderDetailListBySaleOrderID(saleOrderID));
                    foreach (SaleOrderDetailViewModel saleOrderDetailVM in saleOrderVMList)
                    {
                        ProformaInvoiceDetailViewModel proformaInvoiceDetailVM = new ProformaInvoiceDetailViewModel()
                        {
                            ID = Guid.Empty,
                            //QuoteID = Guid.Empty,
                            ProductID = saleOrderDetailVM.ProductID,
                            ProductModelID = saleOrderDetailVM.ProductModelID,
                            ProductSpec = saleOrderDetailVM.ProductSpec,
                            Qty = saleOrderDetailVM.Qty,
                            UnitCode = saleOrderDetailVM.UnitCode,
                            Rate = saleOrderDetailVM.Rate,
                            TaxTypeCode = saleOrderDetailVM.TaxTypeCode,
                            CGSTPerc = saleOrderDetailVM.CGSTPerc,
                            SGSTPerc = saleOrderDetailVM.SGSTPerc,
                            IGSTPerc = saleOrderDetailVM.IGSTPerc,
                            Discount = saleOrderDetailVM.Discount,
                            CessAmt = saleOrderDetailVM.CessAmt,
                            CessPerc = saleOrderDetailVM.CessPerc,
                            Product = saleOrderDetailVM.Product,
                            ProductModel = saleOrderDetailVM.ProductModel,
                            Unit = saleOrderDetailVM.Unit,
                            TaxType = saleOrderDetailVM.TaxType,
                            SpecTag=saleOrderDetailVM.SpecTag
                        };
                        proformaInvoiceItemViewModelList.Add(proformaInvoiceDetailVM);
                    }
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = proformaInvoiceItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get ProformaInvoice DetailList By SaleOrderID From saleOrder

        #region Get ProformaInvoice DetailList By QuotationID From Quotation
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public string GetProformaInvoiceDetailListByQuotationIDFromQuotation(Guid quoteID)
        {
            try
            {
                List<ProformaInvoiceDetailViewModel> proformaInvoiceItemViewModelList = new List<ProformaInvoiceDetailViewModel>();
                if (quoteID != Guid.Empty)
                {
                    List<QuotationDetailViewModel> quotationDetailVMList = Mapper.Map<List<QuotationDetail>, List<QuotationDetailViewModel>>(_quotationBusiness.GetQuotationDetailListByQuotationID(quoteID));
                    proformaInvoiceItemViewModelList = (from quotationDetailVM in quotationDetailVMList
                                                    select new ProformaInvoiceDetailViewModel
                                                    {
                                                        ID = Guid.Empty,
                                                        ProfInvID = Guid.Empty,
                                                        ProductID = quotationDetailVM.ProductID,
                                                        ProductModelID = quotationDetailVM.ProductModelID,
                                                        ProductSpec = quotationDetailVM.ProductSpec,
                                                        Qty = quotationDetailVM.Qty,
                                                        UnitCode = quotationDetailVM.UnitCode,
                                                        Rate = quotationDetailVM.Rate,
                                                        SpecTag = quotationDetailVM.SpecTag,
                                                        Discount = quotationDetailVM.Discount,
                                                        TaxTypeCode = quotationDetailVM.TaxTypeCode,
                                                        CGSTPerc = quotationDetailVM.CGSTPerc,
                                                        SGSTPerc = quotationDetailVM.SGSTPerc,
                                                        IGSTPerc = quotationDetailVM.IGSTPerc,
                                                        CessAmt = 0,
                                                        CessPerc = 0,
                                                        Product = quotationDetailVM.Product,
                                                        ProductModel = quotationDetailVM.ProductModel,
                                                        Unit = quotationDetailVM.Unit,
                                                        TaxType = quotationDetailVM.TaxType,
                                                    }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = proformaInvoiceItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get ProformaInvoice DetailList By QuotationID From Quotation

        #region Get Quotation OtherChargeList By QuotationID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
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

        #region Get Proforma Invoice OtherChargeList By SaleInvoiceID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public string GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(Guid proformaInvoiceID)
        {
            try
            {
                List<ProformaInvoiceOtherChargeViewModel> proformaInvoiceOtherChargeViewModelList = new List<ProformaInvoiceOtherChargeViewModel>();
                if (proformaInvoiceID == Guid.Empty)
                {
                    ProformaInvoiceOtherChargeViewModel proformaInvoiceOtherChargeVM = new ProformaInvoiceOtherChargeViewModel()
                    {
                        ID = Guid.Empty,
                        ProfInvID = Guid.Empty,
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
                    proformaInvoiceOtherChargeViewModelList.Add(proformaInvoiceOtherChargeVM);
                }
                else
                {
                    proformaInvoiceOtherChargeViewModelList = Mapper.Map<List<ProformaInvoiceOtherCharge>, List<ProformaInvoiceOtherChargeViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(proformaInvoiceID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = proformaInvoiceOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Proforma Invoice OtherChargeList By SaleOrderID

        #region Get Proforma Invoice OtherCharge DetailList By QuotationID From Quotation
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public string GetProformaInvoiceOtherChargesDetailListByQuotationIDFromQuotation(Guid quoteID)
        {
            try
            {
                List<ProformaInvoiceOtherChargeViewModel> proformaInvoiceOtherChargeViewModelList = new List<ProformaInvoiceOtherChargeViewModel>();
                if (quoteID != Guid.Empty)
                {
                    List<QuotationOtherChargeViewModel> quotationOtherChargeVMList = Mapper.Map<List<QuotationOtherCharge>, List<QuotationOtherChargeViewModel>>(_quotationBusiness.GetQuotationOtherChargesDetailListByQuotationID(quoteID));
                    proformaInvoiceOtherChargeViewModelList = (from quotationOtherChargeVM in quotationOtherChargeVMList
                                                           select new ProformaInvoiceOtherChargeViewModel
                                                           {
                                                               ID = Guid.Empty,
                                                               ProfInvID = Guid.Empty,
                                                               OtherChargeCode = quotationOtherChargeVM.OtherChargeCode,
                                                               ChargeAmount = quotationOtherChargeVM.ChargeAmount,
                                                               TaxTypeCode = quotationOtherChargeVM.TaxTypeCode,
                                                               CGSTPerc = quotationOtherChargeVM.CGSTPerc,
                                                               SGSTPerc = quotationOtherChargeVM.SGSTPerc,
                                                               IGSTPerc = quotationOtherChargeVM.IGSTPerc,
                                                               AddlTaxPerc = 0,
                                                               AddlTaxAmt = 0,
                                                               OtherCharge = quotationOtherChargeVM.OtherCharge,
                                                               TaxType = quotationOtherChargeVM.TaxType,
                                                           }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = proformaInvoiceOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Proforma Invoice OtherCharge DetailList By QuotationID From Quotation

        #region Get Proforma Invoice OtherCharge DetailList By SaleOrderID From SaleOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public string GetProformaInvoiceOtherChargesDetailListBySaleOrderIDFromSaleOrder(Guid saleOrderID)
        {
            try
            {
                List<ProformaInvoiceOtherChargeViewModel> proformaInvoiceOtherChargeViewModelList = new List<ProformaInvoiceOtherChargeViewModel>();
                if (saleOrderID != Guid.Empty)
                {
                    List<SaleOrderOtherChargeViewModel> saleOrderOtherChargeVMList = Mapper.Map<List<SaleOrderOtherCharge>, List<SaleOrderOtherChargeViewModel>>(_saleOrderBusiness.GetSaleOrderOtherChargesDetailListBySaleOrderID(saleOrderID));
                    proformaInvoiceOtherChargeViewModelList = (from quotationOtherChargeVM in saleOrderOtherChargeVMList
                                                           select new ProformaInvoiceOtherChargeViewModel
                                                           {
                                                               ID = Guid.Empty,
                                                               ProfInvID = Guid.Empty,
                                                               OtherChargeCode = quotationOtherChargeVM.OtherChargeCode,
                                                               ChargeAmount = quotationOtherChargeVM.ChargeAmount,
                                                               TaxTypeCode = quotationOtherChargeVM.TaxTypeCode,
                                                               CGSTPerc = quotationOtherChargeVM.CGSTPerc,
                                                               SGSTPerc = quotationOtherChargeVM.SGSTPerc,
                                                               IGSTPerc = quotationOtherChargeVM.IGSTPerc,
                                                               AddlTaxPerc = 0,
                                                               AddlTaxAmt = 0,
                                                               OtherCharge = quotationOtherChargeVM.OtherCharge,
                                                               TaxType = quotationOtherChargeVM.TaxType,
                                                           }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = proformaInvoiceOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Proforma Invoice OtherCharge DetailList By SaleOrderID From SaleOrder

        #region Delete Proforma Invoice
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "D")]
        public string DeleteProformaInvoice(Guid id)
        {

            try
            {
                object result = _proformaInvoiceBusiness.DeleteProformaInvoice(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete Proforma Invoice

        #region Delete Proforma Invoice Detail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "D")]
        public string DeleteProformaInvoiceDetail(Guid id)
        {

            try
            {
                object result = _proformaInvoiceBusiness.DeleteProformaInvoiceDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete Proforma Invoice Detail

        #region Delete ProformaInvoice OtherChargeDetail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "D")]
        public string DeleteProformaInvoiceOtherChargeDetail(Guid id)
        {

            try
            {
                object result = _proformaInvoiceBusiness.DeleteProformaInvoiceOtherChargeDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion Delete ProformaInvoice OtherChargeDetail

        #region GetAllProformaInvoice
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public JsonResult GetAllProformaInvoice(DataTableAjaxPostModel model, ProformaInvoiceAdvanceSearchViewModel proformaInvoiceAdvanceSearchVM)
        {
            //setting options to our model
            proformaInvoiceAdvanceSearchVM.DataTablePaging.Start = model.start;
            proformaInvoiceAdvanceSearchVM.DataTablePaging.Length = (proformaInvoiceAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : proformaInvoiceAdvanceSearchVM.DataTablePaging.Length;

            //SaleInvoiceAdvanceSearchVM.OrderColumn = model.order[0].column;
            //SaleInvoiceAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<ProformaInvoiceViewModel> proformaInvoiceVMList = Mapper.Map<List<ProformaInvoice>, List<ProformaInvoiceViewModel>>(_proformaInvoiceBusiness.GetAllProformaInvoice(Mapper.Map<ProformaInvoiceAdvanceSearchViewModel, ProformaInvoiceAdvanceSearch>(proformaInvoiceAdvanceSearchVM)));

            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = proformaInvoiceVMList.Count != 0 ? proformaInvoiceVMList[0].TotalCount : 0,
                recordsFiltered = proformaInvoiceVMList.Count != 0 ? proformaInvoiceVMList[0].FilteredCount : 0,
                data = proformaInvoiceVMList
            });
        }
        #endregion GetAllProformaInvoice

        #region InsertUpdateProformaInvoice
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "W")]
        public string InsertUpdateProformaInvoice(ProformaInvoiceViewModel proformaInvoiceVM)
        {
            //object resultFromBusiness = null;

            try
            {
                object ResultFromJS;
                string ReadableFormat;
                AppUA appUA = Session["AppUA"] as AppUA;
                proformaInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
                proformaInvoiceVM.PSASysCommon.CreatedBy = appUA.UserName;
                proformaInvoiceVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                proformaInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
                proformaInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                ResultFromJS = JsonConvert.DeserializeObject(proformaInvoiceVM.DetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                proformaInvoiceVM.ProformaInvoiceDetailList = JsonConvert.DeserializeObject<List<ProformaInvoiceDetailViewModel>>(ReadableFormat);
                if (proformaInvoiceVM.InvoiceType == "RB")
                {
                    ResultFromJS = JsonConvert.DeserializeObject(proformaInvoiceVM.OtherChargesDetailJSON);
                    ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    proformaInvoiceVM.ProformaInvoiceOtherChargeDetailList = JsonConvert.DeserializeObject<List<ProformaInvoiceOtherChargeViewModel>>(ReadableFormat);
                }
                object result = _proformaInvoiceBusiness.InsertUpdateProformaInvoice(Mapper.Map<ProformaInvoiceViewModel, ProformaInvoice>(proformaInvoiceVM));

                if (proformaInvoiceVM.ID == Guid.Empty)
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

        #endregion InsertUpdateProformaInvoice

        #region UpdateProformaEmailInfo
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public string UpdateProformaInvoiceEmailInfo(ProformaInvoiceViewModel proformaInvoiceVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                proformaInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
                proformaInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
                proformaInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object result = _proformaInvoiceBusiness.UpdateProformaInvoiceEmailInfo(Mapper.Map<ProformaInvoiceViewModel, ProformaInvoice>(proformaInvoiceVM));

                if (proformaInvoiceVM.ID == Guid.Empty)
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

        #endregion UpdateSaleOrderEmailInfo

        #region Email ProformaInvoice
        public ActionResult EmailProformaInvoice(ProformaInvoiceViewModel proformaInvoiceVM)
        {
            bool emailFlag = proformaInvoiceVM.EmailFlag;
            //SaleInvoiceViewModel saleInvoiceVM = new SaleInvoiceViewModel();
            proformaInvoiceVM = Mapper.Map<ProformaInvoice, ProformaInvoiceViewModel>(_proformaInvoiceBusiness.GetProformaInvoice(proformaInvoiceVM.ID));
            if (proformaInvoiceVM.ShippingAddress != null && proformaInvoiceVM.ShippingAddress != "")
                proformaInvoiceVM.ShippingAddress = proformaInvoiceVM.ShippingAddress.ToString().Replace(Environment.NewLine, "<br />");
            if (proformaInvoiceVM.MailingAddress != null && proformaInvoiceVM.MailingAddress != "")
                proformaInvoiceVM.MailingAddress = proformaInvoiceVM.MailingAddress.ToString().Replace(Environment.NewLine, "<br />");
            proformaInvoiceVM.ProformaInvoiceDetailList = Mapper.Map<List<ProformaInvoiceDetail>, List<ProformaInvoiceDetailViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceDetailListByProformaInvoiceID(proformaInvoiceVM.ID));
            proformaInvoiceVM.ProformaInvoiceOtherChargeDetailList = Mapper.Map<List<ProformaInvoiceOtherCharge>, List<ProformaInvoiceOtherChargeViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(proformaInvoiceVM.ID));
            proformaInvoiceVM.EmailFlag = emailFlag;
            @ViewBag.path = "http://" + HttpContext.Request.Url.Authority + "/Content/images/Pilot1.PNG";
            proformaInvoiceVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_EmailProformaInvoiceForm", proformaInvoiceVM);
        }
        #endregion Email ProformaInvoice

        #region EmailSent
        [HttpPost, ValidateInput(false)]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public async Task<string> SendProformaInvoiceEmail(ProformaInvoiceViewModel proformaInvoiceVM)
        {
            try
            {
                object result = null;
                if (!string.IsNullOrEmpty(proformaInvoiceVM.ID.ToString()))
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    proformaInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
                    proformaInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
                    proformaInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();

                    bool sendsuccess = await _proformaInvoiceBusiness.ProformaInvoiceEmailPush(Mapper.Map<ProformaInvoiceViewModel, ProformaInvoice>(proformaInvoiceVM));
                    if (sendsuccess)
                    {
                        //1 is meant for mail sent successfully
                        proformaInvoiceVM.EmailSentYN = sendsuccess;
                        result = _proformaInvoiceBusiness.UpdateProformaInvoiceEmailInfo(Mapper.Map<ProformaInvoiceViewModel, ProformaInvoice>(proformaInvoiceVM));
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

        #region GetProformaInvoiceForSelectListOnDemand
        public ActionResult GetProformaInvoiceForSelectListOnDemand(string searchTerm)
        {
            List<ProformaInvoice> proformaInvoiceList = string.IsNullOrEmpty(searchTerm) ? null : _proformaInvoiceBusiness.GetProformaInvoiceForSelectListOnDemand(searchTerm);
            var list = new List<Select2Model>();
            if (proformaInvoiceList != null)
            {
                foreach (ProformaInvoice proformaInvoice in proformaInvoiceList)
                {
                    list.Add(new Select2Model()
                    {
                        text = proformaInvoice.ProfInvNo,
                        id = proformaInvoice.ID.ToString()
                    });
                }
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        # endregion GetProformaInvoiceForSelectListOnDemand

        #region Print ProformaInvoice
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public ActionResult PrintProformaInvoice(ProformaInvoiceViewModel proformaInvoiceVM)
        {
            bool emailFlag = proformaInvoiceVM.EmailFlag;
            proformaInvoiceVM = Mapper.Map<ProformaInvoice, ProformaInvoiceViewModel>(_proformaInvoiceBusiness.GetProformaInvoice(proformaInvoiceVM.ID));
            proformaInvoiceVM.ProformaInvoiceDetailList = Mapper.Map<List<ProformaInvoiceDetail>, List<ProformaInvoiceDetailViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceDetailListByProformaInvoiceID(proformaInvoiceVM.ID));
            proformaInvoiceVM.ProformaInvoiceOtherChargeDetailList = Mapper.Map<List<ProformaInvoiceOtherCharge>, List<ProformaInvoiceOtherChargeViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(proformaInvoiceVM.ID));
            proformaInvoiceVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_PrintProformaInvoice", proformaInvoiceVM);
        }
        #endregion Print ProformaInvoice

        #region CheckQty
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckQty(decimal Qty)
        {

            if (Qty == 0)
            {

                return Json("<p><span style='vertical-align: 2px'>Value could not be zero!</span></p>", JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion CheckQty
        #region CheckRate
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckRate(decimal Rate)
        {

            if (Rate == 0)
            {

                return Json("<p><span style='vertical-align: 2px'>Value could not be zero!</span></p>", JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion CheckRate
        


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProformaInvoice", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "ProformaInvoice");
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProformaInvoice();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportProformaInvoiceData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProformaInvoiceList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProformaInvoice();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProformaInvoice();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProformaInvoice();";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','PIV');";

                    if (_commonBusiness.CheckDocumentIsDeletable("PIV", id))
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
                        toolboxVM.deletebtn.Event = "DeleteProformaInvoice();";
                    }

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailProformaInvoice();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.Title = "Print Document";
                    toolboxVM.PrintBtn.Event = "PrintProformaInvoice()";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','PIV');";

                    //toolboxVM.SendForApprovalBtn.Visible = false;
                    //toolboxVM.SendForApprovalBtn.Text = "Send";
                    //toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    //toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('PIV');";
                    break;
                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProformaInvoice();";

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

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','PIV');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','PIV');";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveProformaInvoice();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetProformaInvoice();";

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