using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SaleInvoiceController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ISaleInvoiceBusiness _saleInvoiceBusiness;      
        ISaleOrderBusiness _saleOrderBusiness;
        IQuotationBusiness _quotationBusiness;       
        ICommonBusiness _commonBusiness;       
        IDocumentStatusBusiness _documentStatusBusiness;
        SecurityFilter.ToolBarAccess _tool;
        IProformaInvoiceBusiness _proformaInvoiceBusiness;
        public SaleInvoiceController(ISaleInvoiceBusiness saleInvoiceBusiness,
           IQuotationBusiness quotationBusiness,
           ISaleOrderBusiness saleOrderBusiness, 
           ICommonBusiness commonBusiness,
           IDocumentStatusBusiness documenStatusBusiness,
           SecurityFilter.ToolBarAccess tool,IProformaInvoiceBusiness proformaInvoiceBusiness           
            )
        {
            _saleInvoiceBusiness = saleInvoiceBusiness;            
            _quotationBusiness = quotationBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _commonBusiness = commonBusiness;
            _documentStatusBusiness = documenStatusBusiness;
            _tool = tool;
            _proformaInvoiceBusiness = proformaInvoiceBusiness;       
        }
        // GET: SaleInvoice
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
            SaleInvoiceAdvanceSearchViewModel saleInvoiceAdvanceSearchVM = new SaleInvoiceAdvanceSearchViewModel();            
            saleInvoiceAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            saleInvoiceAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("SIV");           
            return View(saleInvoiceAdvanceSearchVM);
        }


        #region SaleInvoice Other Charge Detail 
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult SaleInvoiceOtherChargeDetail(bool update)
        {
            SaleInvoiceOtherChargeViewModel saleInvocieOtherChargeVM = new SaleInvoiceOtherChargeViewModel();
            saleInvocieOtherChargeVM.IsUpdate = update;
            return PartialView("_SaleInvoiceOtherCharge", saleInvocieOtherChargeVM);
        }
        #endregion SaleInvoice Other Charge Detail 

        #region SaleInvoice Form
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult SaleInvoiceForm(Guid id, Guid? saleorderID, Guid? quotationID,Guid? proformaInvoiceID)
        {
            SaleInvoiceViewModel saleInvoiceVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    saleInvoiceVM = Mapper.Map<SaleInvoice, SaleInvoiceViewModel>(_saleInvoiceBusiness.GetSaleInvoice(id));
                    saleInvoiceVM.IsUpdate = true;
                    saleInvoiceVM.PreparedBy = saleInvoiceVM.PreparedBy != Guid.Empty ? saleInvoiceVM.PreparedBy : null;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    saleInvoiceVM.IsDocLocked = saleInvoiceVM.DocumentOwners.Contains(appUA.UserName);

                    //if (saleInvoiceVM.QuoteID != Guid.Empty && saleInvoiceVM.QuoteID != null)
                    //{
                    //    saleInvoiceVM.DocumentType = "Quotation";
                    //    saleInvoiceVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quotationID);
                    //}
                    //if (saleInvoiceVM.SaleOrderID != null && saleInvoiceVM.SaleOrderID != Guid.Empty)
                    //{
                    //    saleInvoiceVM.DocumentType = "SaleOrder";
                    //    saleInvoiceVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleorderID);
                    //}
                    //if (saleInvoiceVM.ProfInvID != null && saleInvoiceVM.ProfInvID != Guid.Empty)
                    //{
                    //    saleInvoiceVM.DocumentType = "ProformaInvoice";
                    //    saleInvoiceVM.ProformaInvoiceSelectList = _proformaInvoiceBusiness.GetProformaInvoiceForSelectList(proformaInvoiceID);
                    //}
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
                    saleInvoiceVM.Discount = quotationVM.Discount;
                    saleInvoiceVM.SaleOrderSelectList = new List<SelectListItem>();
                    saleInvoiceVM.ProformaInvoiceSelectList = new List<SelectListItem>();
                    saleInvoiceVM.CustomerID = quotationVM.CustomerID;
                    saleInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-",
                    };
                    saleInvoiceVM.Branch = new BranchViewModel();
                    saleInvoiceVM.Branch.Description = "-";
                    saleInvoiceVM.Customer = quotationVM.Customer;
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
                    saleInvoiceVM.ProformaInvoiceSelectList = new List<SelectListItem>();
                    saleInvoiceVM.SaleOrderSelectList = _saleOrderBusiness.GetSaleOrderForSelectList(saleorderID);
                    saleInvoiceVM.SaleOrderID = saleorderID;
                    saleInvoiceVM.Discount = saleorderVM.Discount;
                    saleInvoiceVM.CustomerID = saleorderVM.CustomerID;
                    saleInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-",
                    };
                    saleInvoiceVM.Branch = new BranchViewModel();
                    saleInvoiceVM.Branch.Description = "-";
                    saleInvoiceVM.Customer = saleorderVM.Customer;
                    saleInvoiceVM.IsDocLocked = false;
                }
                else if(id==Guid.Empty && proformaInvoiceID!=null)
                {
                    ProformaInvoiceViewModel proformaInvoiceVM = Mapper.Map<ProformaInvoice, ProformaInvoiceViewModel>(_proformaInvoiceBusiness.GetProformaInvoice((Guid)proformaInvoiceID));
                    saleInvoiceVM = new SaleInvoiceViewModel();
                    saleInvoiceVM.IsUpdate = false;
                    saleInvoiceVM.ID = Guid.Empty;
                    saleInvoiceVM.DocumentType = "ProformaInvoice";
                    saleInvoiceVM.QuotationSelectList = new List<SelectListItem>();
                    saleInvoiceVM.SaleOrderSelectList = new List<SelectListItem>();
                    saleInvoiceVM.ProformaInvoiceSelectList = _proformaInvoiceBusiness.GetProformaInvoiceForSelectList(proformaInvoiceID);
                    saleInvoiceVM.ProfInvID = proformaInvoiceID;
                    saleInvoiceVM.Discount = proformaInvoiceVM.Discount;
                    saleInvoiceVM.CustomerID = proformaInvoiceVM.CustomerID;
                    saleInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-"
                    };
                    saleInvoiceVM.Branch = new BranchViewModel();
                    saleInvoiceVM.Branch.Description = "-";
                    saleInvoiceVM.Customer = proformaInvoiceVM.Customer;
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
                    saleInvoiceVM.ProformaInvoiceSelectList = new List<SelectListItem>();
                    saleInvoiceVM.DocumentStatus = new DocumentStatusViewModel()
                    {
                        Description = "-",
                    };
                    saleInvoiceVM.Branch = new BranchViewModel();
                    saleInvoiceVM.Branch.Description = "-";
                    saleInvoiceVM.Customer = new CustomerViewModel();
                    saleInvoiceVM.Customer.CompanyName = "-";
                    saleInvoiceVM.IsDocLocked = false;
                }
                //saleInvoiceVM.Customer = new CustomerViewModel
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
            return PartialView("_SaleInvoiceForm", saleInvoiceVM);
        }
        #endregion SaleInvoice Form

        #region SaleInvoice Detail Add
        public ActionResult AddSaleInvoiceDetail(bool update)
        {
            SaleInvoiceDetailViewModel saleInvoiceDetailVM = new SaleInvoiceDetailViewModel();
            saleInvoiceDetailVM.IsUpdate = update;
            return PartialView("_AddSaleInvoiceDetail", saleInvoiceDetailVM);
        }
        #endregion SaleInvoice Detail Add

        #region SaleInvoice Detail Add
        public ActionResult AddSaleInvoiceServiceBill(bool update)
        {
            SaleInvoiceDetailViewModel saleInvoiceDetailVM = new SaleInvoiceDetailViewModel();
            saleInvoiceDetailVM.IsUpdate = update;
            saleInvoiceDetailVM.Qty = 1;//by default one
            saleInvoiceDetailVM.UnitCode = 4;////by default select Nos as unit
            return PartialView("_SaleInvoiceServiceBill", saleInvoiceDetailVM);
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
                        CessAmt=0,
                        OtherChargeCode=null,
                        OtherCharge=new OtherChargeViewModel()
                        {
                        Description=null
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
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleInvoiceItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleInvoice DetailList By QuotationID From Quotation

        #region GetSaleInvoiceDetailListByProfInvIDFromProformaInvoice
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceDetailListByProfInvIDFromProformaInvoice(Guid proformaInvoiceID)
        {
            try
            {
                List<SaleInvoiceDetailViewModel> saleInvoiceItemViewModelList = new List<SaleInvoiceDetailViewModel>();
                if (proformaInvoiceID != Guid.Empty)
                {
                    List<ProformaInvoiceDetailViewModel> proformaInvoiceDetailVMList = Mapper.Map<List<ProformaInvoiceDetail>, List<ProformaInvoiceDetailViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceDetailListByProformaInvoiceID(proformaInvoiceID));
                    saleInvoiceItemViewModelList = (from proformaInvoiceDetailVM in proformaInvoiceDetailVMList
                                                    select new SaleInvoiceDetailViewModel
                                                    {
                                                        ID = Guid.Empty,
                                                        SaleInvID = Guid.Empty,
                                                        ProductID = proformaInvoiceDetailVM.ProductID,
                                                        ProductModelID = proformaInvoiceDetailVM.ProductModelID,
                                                        ProductSpec = proformaInvoiceDetailVM.ProductSpec,
                                                        Qty = proformaInvoiceDetailVM.Qty,
                                                        UnitCode = proformaInvoiceDetailVM.UnitCode,
                                                        Rate = proformaInvoiceDetailVM.Rate,
                                                        SpecTag = proformaInvoiceDetailVM.SpecTag,
                                                        Discount = proformaInvoiceDetailVM.Discount,
                                                        TaxTypeCode = proformaInvoiceDetailVM.TaxTypeCode,
                                                        CGSTPerc = proformaInvoiceDetailVM.CGSTPerc,
                                                        SGSTPerc = proformaInvoiceDetailVM.SGSTPerc,
                                                        IGSTPerc = proformaInvoiceDetailVM.IGSTPerc,
                                                        CessAmt = proformaInvoiceDetailVM.CessAmt,
                                                        CessPerc = proformaInvoiceDetailVM.CessPerc,
                                                        Product = proformaInvoiceDetailVM.Product,
                                                        ProductModel = proformaInvoiceDetailVM.ProductModel,
                                                        Unit = proformaInvoiceDetailVM.Unit,
                                                        TaxType = proformaInvoiceDetailVM.TaxType,
                                                        OtherCharge = proformaInvoiceDetailVM.OtherCharge,

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
        #endregion GetSaleInvoiceDetailListByProfInvIDFromProformaInvoice

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
                                                             OtherCharge = quotationOtherChargeVM.OtherCharge,
                                                             TaxType = quotationOtherChargeVM.TaxType,
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
                                                               OtherCharge = quotationOtherChargeVM.OtherCharge,
                                                               TaxType = quotationOtherChargeVM.TaxType,                                                            
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

        #region GetProformaInvoiceOtherChargesDetailListByProfInvIDFromProformaInvoice
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetProformaInvoiceOtherChargesDetailListByProfInvIDFromProformaInvoice(Guid proformaInvoiceID)
        {
            try
            {
                List<SaleInvoiceOtherChargeViewModel> saleInvoiceOtherChargeViewModelList = new List<SaleInvoiceOtherChargeViewModel>();
                if (proformaInvoiceID != Guid.Empty)
                {
                    List<ProformaInvoiceOtherChargeViewModel> proformaInvoiceOtherChargeVMList = Mapper.Map<List<ProformaInvoiceOtherCharge>, List<ProformaInvoiceOtherChargeViewModel>>(_proformaInvoiceBusiness.GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID(proformaInvoiceID));
                    saleInvoiceOtherChargeViewModelList = (from proformaInvoiceOtherChargeVM in proformaInvoiceOtherChargeVMList
                                                           select new SaleInvoiceOtherChargeViewModel
                                                           {
                                                               ID = Guid.Empty,
                                                               SaleInvID = Guid.Empty,
                                                               OtherChargeCode = proformaInvoiceOtherChargeVM.OtherChargeCode,
                                                               ChargeAmount = proformaInvoiceOtherChargeVM.ChargeAmount,
                                                               TaxTypeCode = proformaInvoiceOtherChargeVM.TaxTypeCode,
                                                               CGSTPerc = proformaInvoiceOtherChargeVM.CGSTPerc,
                                                               SGSTPerc = proformaInvoiceOtherChargeVM.SGSTPerc,
                                                               IGSTPerc = proformaInvoiceOtherChargeVM.IGSTPerc,
                                                               AddlTaxPerc = 0,
                                                               AddlTaxAmt = 0,
                                                               OtherCharge = proformaInvoiceOtherChargeVM.OtherCharge,
                                                               TaxType = proformaInvoiceOtherChargeVM.TaxType,
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
        #endregion GetProformaInvoiceOtherChargesDetailListByProfInvIDFromProformaInvoice

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
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "W")]
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
                if(saleInvoiceVM.InvoiceType=="RB")
                {
                ResultFromJS = JsonConvert.DeserializeObject(saleInvoiceVM.OtherChargesDetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                saleInvoiceVM.SaleInvoiceOtherChargeDetailList = JsonConvert.DeserializeObject<List<SaleInvoiceOtherChargeViewModel>>(ReadableFormat);
                }
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
        #region UpdateSaleInvoiceEmailInfo
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string UpdateSaleInvoiceEmailInfo(SaleInvoiceViewModel saleInvoiceVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                saleInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
                saleInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
                saleInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object result = _saleInvoiceBusiness.UpdateSaleInvoiceEmailInfo(Mapper.Map<SaleInvoiceViewModel, SaleInvoice>(saleInvoiceVM));

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

        #endregion UpdateSaleInvoiceEmailInfo

        #region Email SaleInvoice
        public ActionResult EmailSaleInvoice(SaleInvoiceViewModel saleInvoiceVM)
        {
            bool emailFlag = saleInvoiceVM.EmailFlag;
            //SaleInvoiceViewModel saleInvoiceVM = new SaleInvoiceViewModel();
            saleInvoiceVM = Mapper.Map<SaleInvoice, SaleInvoiceViewModel>(_saleInvoiceBusiness.GetSaleInvoice(saleInvoiceVM.ID));
            if (saleInvoiceVM.ShippingAddress != null && saleInvoiceVM.ShippingAddress != "")
            saleInvoiceVM.ShippingAddress=saleInvoiceVM.ShippingAddress.ToString().Replace(Environment.NewLine, "<br />");
            if (saleInvoiceVM.MailingAddress != null && saleInvoiceVM.MailingAddress != "")
                saleInvoiceVM.MailingAddress = saleInvoiceVM.MailingAddress.ToString().Replace(Environment.NewLine, "<br />");
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

        #region Print SaleInvoice
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult PrintSaleInvoice(SaleInvoiceViewModel saleInvoiceVM)
        {
            bool emailFlag = saleInvoiceVM.EmailFlag;
            saleInvoiceVM = Mapper.Map<SaleInvoice, SaleInvoiceViewModel>(_saleInvoiceBusiness.GetSaleInvoice(saleInvoiceVM.ID));
            saleInvoiceVM.SaleInvoiceDetailList = Mapper.Map<List<SaleInvoiceDetail>, List<SaleInvoiceDetailViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceDetailListBySaleInvoiceID(saleInvoiceVM.ID));
            saleInvoiceVM.SaleInvoiceOtherChargeDetailList = Mapper.Map<List<SaleInvoiceOtherCharge>, List<SaleInvoiceOtherChargeViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID(saleInvoiceVM.ID));
            saleInvoiceVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_PrintSaleInvoice", saleInvoiceVM);
        }
        #endregion Print SaleInvoice

        #region GetSaleInvoiceListForXMLGeneration
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceListForXMLGeneration(string invoiceType)
        {
            SaleInvoiceAdvanceSearchViewModel saleInvoiceAdvanceSearchVM=new SaleInvoiceAdvanceSearchViewModel();
            saleInvoiceAdvanceSearchVM.DataTablePaging = new DataTablePagingViewModel();
            saleInvoiceAdvanceSearchVM.DataTablePaging.Length = -1;
            List<SaleInvoiceViewModel> SaleInvoiceVMList= Mapper.Map<List<SaleInvoice>, List<SaleInvoiceViewModel>>(_saleInvoiceBusiness.GetAllSaleInvoice(Mapper.Map<SaleInvoiceAdvanceSearchViewModel, SaleInvoiceAdvanceSearch>(saleInvoiceAdvanceSearchVM)));
            switch (invoiceType)
            {
                case "ExpModi":
                    SaleInvoiceVMList = SaleInvoiceVMList.Where(x => x.TallyStatus == 0 || x.TallyStatus==2 ).ToList();
                    break;
                case "NExp":
                    SaleInvoiceVMList = SaleInvoiceVMList.Where(x => x.TallyStatus == 0).ToList();
                    break;
                case "Modified":
                    SaleInvoiceVMList = SaleInvoiceVMList.Where(x => x.TallyStatus == 2).ToList();
                    break;
            }
            
            return JsonConvert.SerializeObject(new { Status = "OK", Records = SaleInvoiceVMList, Message = "Success" });
        }
        #endregion GetSaleInvoiceListForXMLGeneration

        #region UpdateSaleInvoiceTallyStatus
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string UpdateSaleInvoiceTallyStatus(string ids)
        {
            try
            {
                
                object result = _saleInvoiceBusiness.UpdateSaleInvoiceTallyStatus(ids);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Updation successfull" });
            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetSaleInvoiceTallyStatus

        #region Get SaleInvoice By SaleInvoiceIDs
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceBySaleInvoiceIDs(string ids)
        {
            try
            {
                List<SaleInvoiceViewModel> SaleInvoiceViewModelList;
                SaleInvoiceViewModelList = Mapper.Map<List<SaleInvoice>, List<SaleInvoiceViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceByID(ids));
                for (var i = 0; i < SaleInvoiceViewModelList.Count; i++)
                {
                    SaleInvoiceViewModelList[i].SaleInvoiceDetailList = Mapper.Map<List<SaleInvoiceDetail>, List<SaleInvoiceDetailViewModel>>(_saleInvoiceBusiness.GetSaleInvoiceDetailListBySaleInvoiceID(SaleInvoiceViewModelList[i].ID));
                }
                    return JsonConvert.SerializeObject(new { Status = "OK", Records = SaleInvoiceViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleInvoice By SaleInvoiceIDs


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
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "SaleInvoice");
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

                    toolboxVM.XMLBtn.Visible = true;
                    toolboxVM.XMLBtn.Text = "XML";
                    toolboxVM.XMLBtn.Title = "Export to XML";
                    toolboxVM.XMLBtn.Event = "GetSaleInvoiceXML()";

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

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','SIV');";

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

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.Title = "Print Document";
                    toolboxVM.PrintBtn.Event = "PrintSaleInvoice()";

                    toolboxVM.SendForApprovalBtn.Visible = false;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','SIV');";
                    break;
                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSaleInvoice();";

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
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','SIV');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','SIV');";
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
            toolboxVM = _tool.SetToolbarAccess(toolboxVM, permission);
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}