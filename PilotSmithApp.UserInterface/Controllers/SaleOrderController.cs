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
        #region Constructor Injection
        public SaleOrderController(ISaleOrderBusiness saleOrderBusiness, IQuotationBusiness quotationBusiness, IEnquiryBusiness enquiryBusiness)
        {
            _saleOrderBusiness = saleOrderBusiness;
            _quotationBusiness = quotationBusiness;
            _enquiryBusiness = enquiryBusiness;
        }
        #endregion Constructor Injection
        // GET: SaleOrder
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region SaleOrderForm Form
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public ActionResult SaleOrderForm(Guid id, Guid? quoteID, Guid? enquiryID)
        {
            SaleOrderViewModel saleOrderVM = null;
            if (id!=Guid.Empty)
            {
                saleOrderVM = Mapper.Map <SaleOrder,SaleOrderViewModel >(_saleOrderBusiness.GetSaleOrder(id));
                saleOrderVM.IsUpdate = true;
                AppUA appUA = Session["AppUA"] as AppUA;
                saleOrderVM.IsDocLocked = saleOrderVM.DocumentOwners.Contains(appUA.UserName);
                if (saleOrderVM.EnquiryID!=null)
                {
                    saleOrderVM.DocumentType = "Enquiry";
                    saleOrderVM.EnquirySelectList = _enquiryBusiness.GetEnquiryForSelectList(enquiryID);
                }
                if(saleOrderVM.QuoteID!=null)
                {
                    saleOrderVM.DocumentType = "Quotation";
                    saleOrderVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quoteID);
                }
            }
            else if(id==Guid.Empty&&quoteID!=null)
            {
                saleOrderVM = new SaleOrderViewModel();
                QuotationViewModel quotationVM= Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation((Guid)quoteID));
                saleOrderVM.QuotationSelectList = _quotationBusiness.GetQuotationForSelectList(quoteID);
                saleOrderVM.CustomerID = quotationVM.CustomerID;
                saleOrderVM.QuoteID = quoteID;
                saleOrderVM.EnquirySelectList = new List<SelectListItem>();
                saleOrderVM.DocumentType = "Quotation";
                saleOrderVM.DocumentStatus = new DocumentStatusViewModel()
                {
                    Description = "OPEN",
                };
            }
            else if (id == Guid.Empty && enquiryID != null)
            {
                saleOrderVM = new SaleOrderViewModel();
                EnquiryViewModel enquiryVM = Mapper.Map<Enquiry, EnquiryViewModel>(_enquiryBusiness.GetEnquiry((Guid)enquiryID));
                saleOrderVM.EnquirySelectList= _enquiryBusiness.GetEnquiryForSelectList(enquiryID);
                saleOrderVM.CustomerID = enquiryVM.CustomerID;
                saleOrderVM.EnquiryID = enquiryID;
                saleOrderVM.QuotationSelectList = new List<SelectListItem>();
                saleOrderVM.DocumentType = "Enquiry";
                saleOrderVM.DocumentStatus = new DocumentStatusViewModel()
                {
                    Description = "OPEN",
                };
            }
            else
            {
                saleOrderVM = new SaleOrderViewModel();
                saleOrderVM.QuotationSelectList = new List<SelectListItem>();
                saleOrderVM.EnquirySelectList = new List<SelectListItem>();
                saleOrderVM.DocumentType = "Quotation";
                saleOrderVM.DocumentStatus = new DocumentStatusViewModel()
                {
                    Description = "OPEN",
                };
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
                int filteredResult = saleOrderVMList.Count;
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
                recordsFiltered = saleOrderVMList.Count,
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
                        SaleOrderID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        Qty = 0,
                        Rate=0,
                        CessAmt=0,
                        UnitCode = null,                      
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
        #region Get SaleOrder DetailList By SaleOrderID with Estimate
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string GetSaleOrderDetailListBySaleOrderIDWithEstimate(Guid estimateID)
        {
            try
            {
                List<SaleOrderDetailViewModel> saleOrderItemViewModelList = new List<SaleOrderDetailViewModel>();
                if (estimateID != Guid.Empty)
                {
                    List<EstimateDetailViewModel> estimateVMList = null;// Mapper.Map<List<EstimateDetail>, List<EstimateDetailViewModel>>(_estimateBusiness.GetEstimateDetailListByEstimateID(estimateID));
                    foreach (EstimateDetailViewModel estimateDetailVM in estimateVMList)
                    {
                        SaleOrderDetailViewModel saleOrderDetailVM = new SaleOrderDetailViewModel()
                        {
                            ID = Guid.Empty,
                            SaleOrderID = Guid.Empty,
                            ProductID = estimateDetailVM.ProductID,
                            ProductModelID = estimateDetailVM.ProductModelID,
                            ProductSpec = estimateDetailVM.ProductSpec,
                            Qty = estimateDetailVM.Qty,
                            UnitCode = estimateDetailVM.UnitCode,
                            Rate = estimateDetailVM.SellingRate,
                            Discount = 0,
                            Product = new ProductViewModel()
                            {
                                ID = (Guid)estimateDetailVM.ProductID,
                                Code = estimateDetailVM.Product.Code,
                                Name = estimateDetailVM.Product.Name,
                            },
                            ProductModel = new ProductModelViewModel()
                            {
                                ID = (Guid)estimateDetailVM.ProductModelID,
                                Name = estimateDetailVM.ProductModel.Name
                            },
                            Unit = new UnitViewModel()
                            {
                                Description = estimateDetailVM.Unit.Description,
                            },
                            TaxType = new TaxTypeViewModel()
                            {
                                ValueText = "",
                            }
                        };
                        saleOrderItemViewModelList.Add(saleOrderDetailVM);
                    }
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = saleOrderItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get SaleOrder DetailList By SaleOrderID with Estimate
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
        #region InsertUpdateSaleOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SaleOrder", Mode = "R")]
        public string InsertUpdateSaleOrder(SaleOrderViewModel saleOrderVM)
        {
            //object resultFromBusiness = null;

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                saleOrderVM.PSASysCommon = new PSASysCommonViewModel();
                saleOrderVM.PSASysCommon.CreatedBy = appUA.UserName;
                saleOrderVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                saleOrderVM.PSASysCommon.UpdatedBy = appUA.UserName;
                saleOrderVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(saleOrderVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                saleOrderVM.SaleOrderDetailList = JsonConvert.DeserializeObject<List<SaleOrderDetailViewModel>>(ReadableFormat);
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
            saleOrderVM.EmailFlag = emailFlag;
            @ViewBag.path = "http://" + HttpContext.Request.Url.Authority + "/Content/images/logo1.PNG";
            saleOrderVM.PDFTools = new PDFTools();
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
        public ActionResult ChangeButtonStyle(string actionType)
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

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteSaleOrder();";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailSaleOrder();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";
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