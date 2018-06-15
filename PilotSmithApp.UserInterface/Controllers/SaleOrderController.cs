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
    public class SaleOrderController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ISaleOrderBusiness _saleOrderBusiness;
        IQuotationBusiness _quotationBusiness;
        IEnquiryBusiness _enquiryBusiness;       
        ICommonBusiness _commonBusiness;      
        IDocumentStatusBusiness _documentStatusBusiness;      
       
        #region Constructor Injection
        public SaleOrderController(ISaleOrderBusiness saleOrderBusiness, IQuotationBusiness quotationBusiness, IEnquiryBusiness enquiryBusiness, ICommonBusiness commonBusiness,
            IDocumentStatusBusiness documentStatusBusiness)
        {
            _saleOrderBusiness = saleOrderBusiness;
            _quotationBusiness = quotationBusiness;
            _enquiryBusiness = enquiryBusiness;
            _commonBusiness = commonBusiness;            
            _documentStatusBusiness = documentStatusBusiness;           
        }
        #endregion Constructor Injection
        // GET: SaleOrder
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult Index()
        {
            SaleOrderAdvanceSearchViewModel saleOrderAdvanceSearchVM = new SaleOrderAdvanceSearchViewModel();           
            saleOrderAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            saleOrderAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("SOD");           
            return View(saleOrderAdvanceSearchVM);
        }
        #region SaleOrderForm Form
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult SaleOrderForm(Guid id, Guid? quoteID, Guid? enquiryID)
        {
            SaleOrderViewModel saleOrderVM = null;
            if (id != Guid.Empty)
            {
                saleOrderVM = Mapper.Map<SaleOrder, SaleOrderViewModel>(_saleOrderBusiness.GetSaleOrder(id));
                saleOrderVM.IsUpdate = true;
                AppUA appUA = Session["AppUA"] as AppUA;
                saleOrderVM.IsDocLocked = saleOrderVM.DocumentOwners.Contains(appUA.UserName);
                if (saleOrderVM.EnquiryID != null)
                {
                    saleOrderVM.DocumentType = "Enquiry";
                    saleOrderVM.EnquirySelectList = _enquiryBusiness.GetEnquiryForSelectList(enquiryID);
                }
                if (saleOrderVM.QuoteID != null)
                {
                    saleOrderVM.DocumentType = "Quotation";
                    saleOrderVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quoteID);
                }
            }
            else if (id == Guid.Empty && quoteID != null)
            {
                saleOrderVM = new SaleOrderViewModel();
                QuotationViewModel quotationVM = Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation((Guid)quoteID));
                saleOrderVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quoteID);
                saleOrderVM.CustomerID = quotationVM.CustomerID;
                saleOrderVM.QuoteID = quoteID;
                saleOrderVM.EnquirySelectList = new List<SelectListItem>();
                saleOrderVM.DocumentType = "Quotation";
                saleOrderVM.DocumentStatus = new DocumentStatusViewModel()
                {
                    Description = "-",
                };
                saleOrderVM.Branch = new BranchViewModel();
                saleOrderVM.Branch.Description = "-";
                saleOrderVM.IsDocLocked = false;
            }
            else if (id == Guid.Empty && enquiryID != null)
            {
                saleOrderVM = new SaleOrderViewModel();
                EnquiryViewModel enquiryVM = Mapper.Map<Enquiry, EnquiryViewModel>(_enquiryBusiness.GetEnquiry((Guid)enquiryID));
                saleOrderVM.EnquirySelectList = _enquiryBusiness.GetEnquiryForSelectList(enquiryID);
                saleOrderVM.CustomerID = enquiryVM.CustomerID;
                saleOrderVM.EnquiryID = enquiryID;
                saleOrderVM.QuotationSelectList = new List<SelectListItem>();
                saleOrderVM.DocumentType = "Enquiry";
                saleOrderVM.DocumentStatus = new DocumentStatusViewModel()
                {
                    Description = "-",
                };
                saleOrderVM.Branch = new BranchViewModel();
                saleOrderVM.Branch.Description = "-";
                saleOrderVM.IsDocLocked = false;
            }
            else
            {
                saleOrderVM = new SaleOrderViewModel();
                saleOrderVM.QuotationSelectList = new List<SelectListItem>();
                saleOrderVM.EnquirySelectList = new List<SelectListItem>();
                saleOrderVM.DocumentType = "Quotation";
                saleOrderVM.DocumentStatus = new DocumentStatusViewModel()
                {
                    Description = "-",
                };
                saleOrderVM.Branch = new BranchViewModel();
                saleOrderVM.Branch.Description = "-";
                saleOrderVM.IsDocLocked = false;
            }
            return PartialView("_SaleOrderForm", saleOrderVM);
        }
        #endregion SaleOrderForm Form
        #region Get SaleOrder SelectList On Demand
        public ActionResult GetSaleOrderSelectListOnDemand(string searchTerm)
        {
            List<SaleOrderViewModel> saleOrderVMList = string.IsNullOrEmpty(searchTerm) ? null : Mapper.Map<List<SaleOrder>, List<SaleOrderViewModel>>(_saleOrderBusiness.GetSaleOrderForSelectListOnDemand(searchTerm));
            var list = new List<Select2Model>();
            if (saleOrderVMList != null)
            {
                foreach (SaleOrderViewModel saleOrderVM in saleOrderVMList)
                {
                    list.Add(new Select2Model()
                    {
                        text = saleOrderVM.SaleOrderNo,
                        id = saleOrderVM.ID.ToString()
                    });
                }
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get SaleOrder SelectList On Demand
        #region SaleOrder Detail Add
        public ActionResult AddSaleOrderDetail()
        {
            SaleOrderDetailViewModel saleOrderDetailVM = new SaleOrderDetailViewModel();
            saleOrderDetailVM.IsUpdate = false;
            return PartialView("_AddSaleOrderDetail", saleOrderDetailVM);
        }
        #endregion SaleOrder Detail Add
        #region QuotationOtherCharge Detail 
        public ActionResult SaleOrderOtherChargeDetail()
        {
            SaleOrderOtherChargeViewModel saleOrderOtherChargeVM = new SaleOrderOtherChargeViewModel();
            saleOrderOtherChargeVM.IsUpdate = false;
            return PartialView("_SaleOrderOtherCharge", saleOrderOtherChargeVM);
        }
        #endregion QuotationOtherCharge Detail Add
        #region GetAllSaleOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public JsonResult GetAllSaleOrder(DataTableAjaxPostModel model, SaleOrderAdvanceSearchViewModel saleOrderAdvanceSearchVM)
        {
            //setting options to our model
            saleOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            saleOrderAdvanceSearchVM.DataTablePaging.Length = (saleOrderAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : saleOrderAdvanceSearchVM.DataTablePaging.Length;

            List<SaleOrderViewModel> saleOrderVMList = Mapper.Map<List<SaleOrder>, List<SaleOrderViewModel>>(_saleOrderBusiness.GetAllSaleOrder(Mapper.Map<SaleOrderAdvanceSearchViewModel, SaleOrderAdvanceSearch>(saleOrderAdvanceSearchVM)));
            if (saleOrderAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = saleOrderVMList.Count != 0 ? saleOrderVMList[0].TotalCount : 0;
                //int filteredResult = saleOrderVMList.Count;
                int filteredResult = saleOrderVMList.Count != 0 ? saleOrderVMList[0].FilteredCount : 0;
                saleOrderVMList = saleOrderVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = saleOrderVMList.Count != 0 ? saleOrderVMList[0].TotalCount : 0,
                //recordsFiltered = saleOrderVMList.Count,
                recordsFiltered = saleOrderVMList.Count != 0 ? saleOrderVMList[0].FilteredCount : 0,
                data = saleOrderVMList
            });
        }
        #endregion GetAllSaleOrder

        #region GetSaleOrderDetailListBySaleOrderID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string GetSaleOrderDetailListBySaleOrderID(Guid saleOrderID)
        {
            try
            {
                List<SaleOrderDetailViewModel> saleOrderItemViewModelList = new List<SaleOrderDetailViewModel>();
                if (saleOrderID == Guid.Empty)
                {
                    SaleOrderDetailViewModel saleOrderDetailVM = new SaleOrderDetailViewModel()
                    {
                        ID = Guid.Empty,
                        SaleOrderID = saleOrderID,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        Qty = 0,
                        Rate = 0,
                        Discount = 0,
                        SGSTPerc = 0,
                        CGSTPerc = 0,
                        IGSTPerc = 0,
                        CessAmt = 0,
                        CessPerc = 0,
                        UnitCode = null,
                        PrevProduceQty=0,
                        PrevDelQty=0,
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
                            Description = "",
                        }
                    };
                    saleOrderItemViewModelList.Add(saleOrderDetailVM);
                }
                else
                {
                    saleOrderItemViewModelList = Mapper.Map<List<SaleOrderDetail>, List<SaleOrderDetailViewModel>>(_saleOrderBusiness.GetSaleOrderDetailListBySaleOrderID(saleOrderID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleOrderItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetSaleOrderDetailListBySaleOrderID
        #region Get SaleOrder DetailList By QuotationID with Quotation
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string GetSaleOrderDetailListByQuotationIDFromQuotation(Guid quoteID)
        {
            try
            {
                List<SaleOrderDetailViewModel> saleOrderItemViewModelList = new List<SaleOrderDetailViewModel>();
                if (quoteID != Guid.Empty)
                {
                    List<QuotationDetailViewModel> quotationDetailVMList = Mapper.Map<List<QuotationDetail>, List<QuotationDetailViewModel>>(_quotationBusiness.GetQuotationDetailListByQuotationID(quoteID));
                    saleOrderItemViewModelList = (from quotationDetailVM in quotationDetailVMList
                                                  select new SaleOrderDetailViewModel
                                                  {
                                                      ID = Guid.Empty,
                                                      SaleOrderID = Guid.Empty,
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
                                                          Code=quotationDetailVM.TaxType.Code,
                                                          ValueText = quotationDetailVM.TaxType.ValueText
                                                      },
                                                  }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleOrderItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleOrder DetailList By SaleOrderID with Quotation
        #region Get SaleOrder DetailList By EnquiryID From Enquiry
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string GetSaleOrderDetailListByEnquiryIDFromEnquiry(Guid enquiryID)
        {
            try
            {
                List<SaleOrderDetailViewModel> saleOrderItemViewModelList = new List<SaleOrderDetailViewModel>();
                if (enquiryID != Guid.Empty)
                {
                    List<EnquiryDetailViewModel> enquiryDetailVMList = Mapper.Map<List<EnquiryDetail>, List<EnquiryDetailViewModel>>(_enquiryBusiness.GetEnquiryDetailListByEnquiryID(enquiryID));
                    saleOrderItemViewModelList = (from enquiryDetailVM in enquiryDetailVMList
                                                  select new SaleOrderDetailViewModel
                                                  {
                                                      ID = Guid.Empty,
                                                      SaleOrderID = Guid.Empty,
                                                      ProductID = enquiryDetailVM.ProductID,
                                                      ProductModelID = enquiryDetailVM.ProductModelID,
                                                      ProductSpec = enquiryDetailVM.ProductSpec,
                                                      Qty = enquiryDetailVM.Qty,
                                                      UnitCode = enquiryDetailVM.UnitCode,
                                                      Rate = enquiryDetailVM.Rate,
                                                      SpecTag = enquiryDetailVM.SpecTag,
                                                      TaxTypeCode = null,
                                                      SGSTPerc = 0,
                                                      CGSTPerc = 0,
                                                      IGSTPerc = 0,
                                                      CessAmt = 0,
                                                      CessPerc = 0,
                                                      Discount = 0,
                                                      Product = new ProductViewModel()
                                                      {
                                                          ID = (Guid)enquiryDetailVM.ProductID,
                                                          Code = enquiryDetailVM.Product.Code,
                                                          Name = enquiryDetailVM.Product.Name,
                                                      },
                                                      ProductModel = new ProductModelViewModel()
                                                      {
                                                          ID = (Guid)enquiryDetailVM.ProductModelID,
                                                          Name = enquiryDetailVM.ProductModel.Name
                                                      },
                                                      Unit = new UnitViewModel()
                                                      {
                                                          Description = enquiryDetailVM.Unit.Description
                                                      },
                                                      TaxType = new TaxTypeViewModel() { },
                                                  }).ToList();

                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleOrderItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleOrder DetailList By EnquiryID From Enquiry

        #region Get SaleOrder OtherChargeList By SaleOrderID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string GetSaleOrderOtherChargesDetailListBySaleOrderID(Guid saleOrderID)
        {
            try
            {
                List<SaleOrderOtherChargeViewModel> saleOrderOtherChargeViewModelList = new List<SaleOrderOtherChargeViewModel>();
                if (saleOrderID == Guid.Empty)
                {
                    SaleOrderOtherChargeViewModel saleOrderOtherChargeVM = new SaleOrderOtherChargeViewModel()
                    {
                        ID = Guid.Empty,
                        SaleOrderID = Guid.Empty,
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
                    saleOrderOtherChargeViewModelList = Mapper.Map<List<SaleOrderOtherCharge>, List<SaleOrderOtherChargeViewModel>>(_saleOrderBusiness.GetSaleOrderOtherChargesDetailListBySaleOrderID(saleOrderID));
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

        #region Get SaleOrder OtherCharge DetailList By QuotationID From Quotation
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string GetSaleOrderOtherChargesDetailListByQuotationIDFromQuotation(Guid quoteID)
        {
            try
            {
                List<SaleOrderOtherChargeViewModel> saleOrderOtherChargeViewModelList = new List<SaleOrderOtherChargeViewModel>();
                if (quoteID != Guid.Empty)
                {
                    List<QuotationOtherChargeViewModel> quotationOtherChargeVMList = Mapper.Map<List<QuotationOtherCharge>, List<QuotationOtherChargeViewModel>>(_quotationBusiness.GetQuotationOtherChargesDetailListByQuotationID(quoteID));
                    saleOrderOtherChargeViewModelList = (from quotationOtherChargeVM in quotationOtherChargeVMList
                                                         select new SaleOrderOtherChargeViewModel
                                                  {
                                                      ID = Guid.Empty,
                                                      SaleOrderID = Guid.Empty,
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
                                                          Code= quotationOtherChargeVM.TaxType.Code,
                                                          ValueText = quotationOtherChargeVM.TaxType.ValueText
                                                      },
                                                  }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleOrderOtherChargeViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleOrder DetailList By SaleOrderID with Quotation

        #region Delete SaleOrder
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "D")]
        public string DeleteSaleOrder(Guid id)
        {

            try
            {
                object result = _saleOrderBusiness.DeleteSaleOrder(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete SaleOrder
        #region Delete SaleOrder Detail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "D")]
        public string DeleteSaleOrderDetail(Guid id)
        {

            try
            {
                object result = _saleOrderBusiness.DeleteSaleOrderDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete SaleOrder Detail
        #region Delete SaleOrder OtherCharge
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "D")]
        public string DeleteSaleOrderOtherChargeDetail(Guid id)
        {

            try
            {
                object result = _saleOrderBusiness.DeleteSaleOrderOtherChargeDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete SaleOrder OtherCharge
        #region InsertUpdateSaleOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string InsertUpdateSaleOrder(SaleOrderViewModel saleOrderVM)
        {
            //object resultFromBusiness = null;

            try
            {
                object ResultFromJS;
                string ReadableFormat;
                AppUA appUA = Session["AppUA"] as AppUA;
                saleOrderVM.PSASysCommon = new PSASysCommonViewModel();
                saleOrderVM.PSASysCommon.CreatedBy = appUA.UserName;
                saleOrderVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                saleOrderVM.PSASysCommon.UpdatedBy = appUA.UserName;
                saleOrderVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                ResultFromJS = JsonConvert.DeserializeObject(saleOrderVM.DetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                saleOrderVM.SaleOrderDetailList = JsonConvert.DeserializeObject<List<SaleOrderDetailViewModel>>(ReadableFormat);
                ResultFromJS = JsonConvert.DeserializeObject(saleOrderVM.OtherChargesDetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                saleOrderVM.SaleOrderOtherChargeList = JsonConvert.DeserializeObject<List<SaleOrderOtherChargeViewModel>>(ReadableFormat);
                object result = _saleOrderBusiness.InsertUpdateSaleOrder(Mapper.Map<SaleOrderViewModel, SaleOrder>(saleOrderVM));

                if (saleOrderVM.ID == Guid.Empty)
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

        #endregion InsertUpdateSaleOrder
        #region UpdateSaleOrderEmailInfo
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string UpdateSaleOrderEmailInfo(SaleOrderViewModel saleOrderVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                saleOrderVM.PSASysCommon = new PSASysCommonViewModel();
                saleOrderVM.PSASysCommon.UpdatedBy = appUA.UserName;
                saleOrderVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object result = _saleOrderBusiness.UpdateSaleOrderEmailInfo(Mapper.Map<SaleOrderViewModel, SaleOrder>(saleOrderVM));

                if (saleOrderVM.ID == Guid.Empty)
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
        #region Email SaleOrder
        public ActionResult EmailSaleOrder(SaleOrderViewModel saleOrderVM)
        {
            bool emailFlag = saleOrderVM.EmailFlag;
            //SaleOrderViewModel saleOrderVM = new SaleOrderViewModel();
            saleOrderVM = Mapper.Map<SaleOrder, SaleOrderViewModel>(_saleOrderBusiness.GetSaleOrder(saleOrderVM.ID));
            saleOrderVM.SaleOrderDetailList = Mapper.Map<List<SaleOrderDetail>, List<SaleOrderDetailViewModel>>(_saleOrderBusiness.GetSaleOrderDetailListBySaleOrderID(saleOrderVM.ID));
            saleOrderVM.SaleOrderOtherChargeList= Mapper.Map<List<SaleOrderOtherCharge>, List<SaleOrderOtherChargeViewModel>>(_saleOrderBusiness.GetSaleOrderOtherChargesDetailListBySaleOrderID(saleOrderVM.ID));
            saleOrderVM.EmailFlag = emailFlag;
            @ViewBag.path = "http://" + HttpContext.Request.Url.Authority + "/Content/images/logo1.PNG";
            saleOrderVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_EmailSaleOrder", saleOrderVM);
        }
        #endregion Email SaleOrder
        #region EmailSent
        [HttpPost, ValidateInput(false)]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public async Task<string> SendSaleOrderEmail(SaleOrderViewModel saleOrderVM)
        {
            try
            {
                object result = null;
                if (!string.IsNullOrEmpty(saleOrderVM.ID.ToString()))
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    saleOrderVM.PSASysCommon = new PSASysCommonViewModel();
                    saleOrderVM.PSASysCommon.UpdatedBy = appUA.UserName;
                    saleOrderVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();

                    bool sendsuccess = await _saleOrderBusiness.QuoteEmailPush(Mapper.Map<SaleOrderViewModel, SaleOrder>(saleOrderVM));
                    if (sendsuccess)
                    {
                        //1 is meant for mail sent successfully
                        saleOrderVM.EmailSentYN = sendsuccess;
                        result = _saleOrderBusiness.UpdateSaleOrderEmailInfo(Mapper.Map<SaleOrderViewModel, SaleOrder>(saleOrderVM));
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
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSaleOrder();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportSaleOrderData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleOrderList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSaleOrder();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveSaleOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleOrder();";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','SOD');";

                    if (_commonBusiness.CheckDocumentIsDeletable("SOD", id))
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
                        toolboxVM.deletebtn.Event = "DeleteSaleOrder();";
                    }

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailSaleOrder();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('SOD');";
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
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','SOD');";
                    break;

                case "Approved":
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

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailSaleOrder();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','SOD');";

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveSaleOrder();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetSaleOrder();";

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