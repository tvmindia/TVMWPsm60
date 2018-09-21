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
    public class QuotationController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IQuotationBusiness _quotationBusiness;       
        IEstimateBusiness _estimateBusiness;
        ICommonBusiness _commonBusiness;       
        IDocumentStatusBusiness _documentStatusBusiness;        
        SecurityFilter.ToolBarAccess _tool;

        public QuotationController(IQuotationBusiness quotationBusiness,           
            IEstimateBusiness estimateBusiness,
            ICommonBusiness commonBusiness,            
            IDocumentStatusBusiness documentStatusBusiness,SecurityFilter.ToolBarAccess tool
            )
        {
            _quotationBusiness = quotationBusiness;           
            _estimateBusiness = estimateBusiness;
            _commonBusiness = commonBusiness;            
            _documentStatusBusiness = documentStatusBusiness;           
            _tool = tool;   
           
        }
        // GET: Quotation
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
            QuotationAdvanceSearchViewModel quotationAdvanceSearchVM = new QuotationAdvanceSearchViewModel();
            quotationAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            quotationAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("QUO");         
            return View(quotationAdvanceSearchVM);           
        }
        #region Quotation Form
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult QuotationForm(Guid id,Guid? estimateID)
        {
            QuotationViewModel quotationVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    quotationVM = Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation(id));
                    quotationVM.IsUpdate = true;
                    
                    AppUA appUA = Session["AppUA"] as AppUA;
                    quotationVM.IsDocLocked = quotationVM.DocumentOwners.Contains(appUA.UserName);
                    quotationVM.EstimateSelectList = _estimateBusiness.GetEstimateForSelectList(estimateID);

                }
                else if(id==Guid.Empty&&estimateID==null)
                {
                    quotationVM = new QuotationViewModel();
                    quotationVM.IsUpdate = false;
                    quotationVM.ID = Guid.Empty;
                    quotationVM.EstimateID = null;
                    quotationVM.EstimateSelectList = new List<SelectListItem>();
                    quotationVM.DocumentStatus = new DocumentStatusViewModel();
                    quotationVM.DocumentStatus.Description = "-";
                    quotationVM.Branch = new BranchViewModel();
                    quotationVM.Branch.Description = "-";
                    //quotationVM.Customer = new CustomerViewModel();
                    //quotationVM.Customer.CompanyName = "-";
                    quotationVM.IsDocLocked = false;
                }
                else if(id == Guid.Empty && estimateID != null)
                {
                    EstimateViewModel estimateVM = Mapper.Map<Estimate,EstimateViewModel>(_estimateBusiness.GetEstimate((Guid)estimateID));
                    quotationVM = new QuotationViewModel();
                    quotationVM.IsUpdate = false;
                    quotationVM.ID = Guid.Empty;
                    quotationVM.EstimateID = estimateID;
                    quotationVM.CustomerID = estimateVM.CustomerID;
                    quotationVM.EstimateSelectList = _estimateBusiness.GetEstimateForSelectList(estimateID);
                    quotationVM.DocumentStatus = new DocumentStatusViewModel();
                    quotationVM.DocumentStatus.Description = "-";
                    quotationVM.Branch = new BranchViewModel();
                    quotationVM.Branch.Description = "-";
                    quotationVM.Customer = estimateVM.Customer;
                    quotationVM.IsDocLocked = false;
                }
                //quotationVM.Customer = new CustomerViewModel
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
            return PartialView("_QuotationForm", quotationVM);
        }
        #endregion Quotation Form
        #region Quotation Detail Add
        public ActionResult AddQuotationDetail(bool update)
        {
            QuotationDetailViewModel quotationDetailVM = new QuotationDetailViewModel();
            quotationDetailVM.IsUpdate = update;
            return PartialView("_AddQuotationDetail", quotationDetailVM);
        }
        #endregion Quotation Detail Add
        #region QuotationOtherCharge Detail 
        public ActionResult QuotationOtherChargeDetail()
        {
            QuotationOtherChargeViewModel quotationOtherChargeVM = new QuotationOtherChargeViewModel();
            quotationOtherChargeVM.IsUpdate = false;
            return PartialView("_QuotationOtherCharge", quotationOtherChargeVM);
        }
        #endregion QuotationOtherCharge Detail Add

        #region Get Quotation DetailList By QuotationID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public string GetQuotationDetailListByQuotationID(Guid quotationID)
        {
            try
            {
                List<QuotationDetailViewModel> quotationItemViewModelList = new List<QuotationDetailViewModel>();
                if (quotationID == Guid.Empty)
                {
                    QuotationDetailViewModel quotationDetailVM = new QuotationDetailViewModel()
                    {
                        ID = Guid.Empty,
                        QuoteID = Guid.Empty,
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
                        TaxType=new TaxTypeViewModel()
                        {
                            ValueText = "",
                        }
                    };
                    quotationItemViewModelList.Add(quotationDetailVM);
                }
                else
                {
                    quotationItemViewModelList = Mapper.Map<List<QuotationDetail>, List<QuotationDetailViewModel>>(_quotationBusiness.GetQuotationDetailListByQuotationID(quotationID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = quotationItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Quotation DetailList By QuotationID

        #region Get Quotation OtherChargeList By QuotationID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
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
                        ChargeAmount=0,
                        OtherCharge=new OtherChargeViewModel()
                        {
                            Description= "",
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

        #region Get Quotation DetailList By QuotationID with Estimate
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public string GetQuotationDetailListByQuotationIDWithEstimate(Guid estimateID)
        {
            try
            {
                List<QuotationDetailViewModel> quotationItemViewModelList = new List<QuotationDetailViewModel>();
                if (estimateID != Guid.Empty)
                {
                    List<EstimateDetailViewModel> estimateVMList = Mapper.Map<List<EstimateDetail>,List<EstimateDetailViewModel>>(_estimateBusiness.GetEstimateDetailListByEstimateID(estimateID));
                    quotationItemViewModelList=(from estimateDetailVM in estimateVMList
                                                select new QuotationDetailViewModel {
                                                    ID = Guid.Empty,
                                                    QuoteID = Guid.Empty,
                                                    ProductID = estimateDetailVM.ProductID,
                                                    ProductModelID = estimateDetailVM.ProductModelID,
                                                    ProductSpec = estimateDetailVM.ProductSpec,
                                                    Qty = estimateDetailVM.Qty,
                                                    UnitCode = estimateDetailVM.UnitCode,
                                                    Rate = estimateDetailVM.SellingRate,
                                                    CGSTPerc = 0,
                                                    IGSTPerc = 0,
                                                    SGSTPerc = 0,
                                                    Discount = 0,
                                                    SpecTag = estimateDetailVM.SpecTag,
                                                    Product = estimateDetailVM.Product,
                                                    ProductModel = estimateDetailVM.ProductModel,
                                                    Unit = estimateDetailVM.Unit,
                                                    TaxType = new TaxTypeViewModel()
                                                    {
                                                        ValueText = "",
                                                    }
                                                }).ToList();
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = quotationItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Quotation DetailList By QuotationID with Estimate
        #region Delete Quotation
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "D")]
        public string DeleteQuotation(Guid id)
        {

            try
            {
                object result = _quotationBusiness.DeleteQuotation(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete Quotation
        #region Delete Quotation Detail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "D")]
        public string DeleteQuotationDetail(Guid id)
        {

            try
            {
                object result = _quotationBusiness.DeleteQuotationDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete Quotation Detail
        #region Delete Quotation OtherCharge
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "D")]
        public string DeleteQuotationOtherChargeDetail(Guid id)
        {

            try
            {
                object result = _quotationBusiness.DeleteQuotationOtherChargeDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete Quotation OtherCharge
        #region GetAllQuotation
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public JsonResult GetAllQuotation(DataTableAjaxPostModel model, QuotationAdvanceSearchViewModel QuotationAdvanceSearchVM)
        {
            //setting options to our model
            QuotationAdvanceSearchVM.DataTablePaging.Start = model.start;
            QuotationAdvanceSearchVM.DataTablePaging.Length = (QuotationAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : QuotationAdvanceSearchVM.DataTablePaging.Length;

            //QuotationAdvanceSearchVM.OrderColumn = model.order[0].column;
            //QuotationAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<QuotationViewModel> QuotationVMList = Mapper.Map<List<Quotation>, List<QuotationViewModel>>(_quotationBusiness.GetAllQuotation(Mapper.Map<QuotationAdvanceSearchViewModel, QuotationAdvanceSearch>(QuotationAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = QuotationVMList.Count != 0 ? QuotationVMList[0].TotalCount : 0,
                recordsFiltered = QuotationVMList.Count != 0 ? QuotationVMList[0].FilteredCount : 0,
                data = QuotationVMList
            });
        }
        #endregion GetAllQuotation
        #region InsertUpdateQuotation
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "W")]
        public string InsertUpdateQuotation(QuotationViewModel quotationVM)
        {
            //object resultFromBusiness = null;

            try
            {
                object ResultFromJS;
                string ReadableFormat;
                AppUA appUA = Session["AppUA"] as AppUA;
                quotationVM.PSASysCommon = new PSASysCommonViewModel();
                quotationVM.PSASysCommon.CreatedBy = appUA.UserName;
                quotationVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                quotationVM.PSASysCommon.UpdatedBy = appUA.UserName;
                quotationVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                ResultFromJS = JsonConvert.DeserializeObject(quotationVM.DetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                quotationVM.QuotationDetailList = JsonConvert.DeserializeObject<List<QuotationDetailViewModel>>(ReadableFormat);
                ResultFromJS = JsonConvert.DeserializeObject(quotationVM.OtherChargesDetailJSON);
                ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                quotationVM.QuotationOtherChargeList= JsonConvert.DeserializeObject<List<QuotationOtherChargeViewModel>>(ReadableFormat);
                object result = _quotationBusiness.InsertUpdateQuotation(Mapper.Map<QuotationViewModel, Quotation>(quotationVM));

                if (quotationVM.ID == Guid.Empty)
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

        #endregion InsertUpdateQuotation
        #region UpdateQuotationEmailInfo
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public string UpdateQuotationEmailInfo(QuotationViewModel quotationVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                quotationVM.PSASysCommon = new PSASysCommonViewModel();
                quotationVM.PSASysCommon.UpdatedBy = appUA.UserName;
                quotationVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object result = _quotationBusiness.UpdateQuotationEmailInfo(Mapper.Map<QuotationViewModel, Quotation>(quotationVM));

                if (quotationVM.ID == Guid.Empty)
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

        #endregion UpdateQuotationEmailInfo
        #region Email Quotation
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult EmailQuotation(QuotationViewModel quotationVM)
        {
            bool emailFlag = quotationVM.EmailFlag;
            bool ImageCheck = quotationVM.ImageCheck;
            //QuotationViewModel quotationVM = new QuotationViewModel();
            quotationVM = Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation(quotationVM.ID));
            quotationVM.QuotationDetailList = Mapper.Map<List<QuotationDetail>,List <QuotationDetailViewModel>>(_quotationBusiness.GetQuotationDetailListByQuotationID(quotationVM.ID));
            quotationVM.QuotationOtherChargeList = Mapper.Map<List<QuotationOtherCharge>, List<QuotationOtherChargeViewModel>>(_quotationBusiness.GetQuotationOtherChargesDetailListByQuotationID(quotationVM.ID));
            quotationVM.EmailFlag = emailFlag;
            quotationVM.ImageCheck = ImageCheck;
            ViewBag.path = "http://" + HttpContext.Request.Url.Authority + "/Content/images/logo1.PNG";
            ViewBag.ImgURL = "http://" + HttpContext.Request.Url.Authority + "/";
            quotationVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_EmailQuotation",quotationVM);
        }
        #endregion Email Quotation
        #region Print Quotation
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult PrintQuotation(QuotationViewModel quotationVM)
        {
            bool emailFlag = quotationVM.EmailFlag;
            //QuotationViewModel quotationVM = new QuotationViewModel();
            quotationVM = Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation(quotationVM.ID));
            quotationVM.QuotationDetailList = Mapper.Map<List<QuotationDetail>, List<QuotationDetailViewModel>>(_quotationBusiness.GetQuotationDetailListByQuotationID(quotationVM.ID));
            quotationVM.QuotationOtherChargeList = Mapper.Map<List<QuotationOtherCharge>, List<QuotationOtherChargeViewModel>>(_quotationBusiness.GetQuotationOtherChargesDetailListByQuotationID(quotationVM.ID));
            ViewBag.ImgURL = "http://" + HttpContext.Request.Url.Authority + "/";
            quotationVM.PDFTools = new PDFToolsViewModel();
            return PartialView("_PrintQuotation", quotationVM);
        }
        #endregion Print Quotation
        #region EmailSent
        [HttpPost, ValidateInput(false)]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public async Task<string> SendQuotationEmail(QuotationViewModel quotationVM)
        {
            try
            {
                object result = null;
                if (!string.IsNullOrEmpty(quotationVM.ID.ToString()))
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    quotationVM.PSASysCommon = new PSASysCommonViewModel();
                    quotationVM.PSASysCommon.UpdatedBy = appUA.UserName;
                    quotationVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                    bool sendsuccess = await _quotationBusiness.QuoteEmailPush(Mapper.Map<QuotationViewModel, Quotation>(quotationVM));
                    if (sendsuccess)
                    {
                        //1 is meant for mail sent successfully
                        quotationVM.EmailSentYN = sendsuccess;
                        result = _quotationBusiness.UpdateQuotationEmailInfo(Mapper.Map<QuotationViewModel, Quotation>(quotationVM));
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
                return JsonConvert.SerializeObject(new { Status = "ERROR",Record="", Message = cm.Message });
            }
        }
        #endregion EmailSent
        #region Get QUotation SelectList On Demand
        public ActionResult GetQuotationSelectListOnDemand(string searchTerm)
        {
            List<Quotation> quotationList = string.IsNullOrEmpty(searchTerm)?null:_quotationBusiness.GetQuotationForSelectListOnDemand(searchTerm);
            var list = new List<Select2Model>();
            if (quotationList!=null)
            {
                foreach (Quotation quotation in quotationList)
            {
                list.Add(new Select2Model()
                {
                    text = quotation.QuoteNo,
                    id = quotation.ID.ToString()
                });
            }
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get Quotation SelectList On Demand

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
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "Quotation");                  
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddQuotation();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportQuotationData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetQuotationList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddQuotation();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveQuotation();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetQuotation();";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','QUO');";


                    if (_commonBusiness.CheckDocumentIsDeletable("QUO", id))
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
                        toolboxVM.deletebtn.Event = "DeleteQuotation();";
                    }

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailQuotation();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','QUO');";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";

                    //toolboxVM.RecallBtn.Visible = true;
                    //toolboxVM.RecallBtn.Text = "Recall";
                    //toolboxVM.RecallBtn.Title = "Document Recall";
                    //toolboxVM.RecallBtn.Event = "RecallDocumentItem('QUO');";
                    break;
                case "Draft":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddQuotation();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveQuotation();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetQuotation();";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','QUO');";

                    if (_commonBusiness.CheckDocumentIsDeletable("QUO", id))
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
                        toolboxVM.deletebtn.Event = "DeleteQuotation();";
                    }

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailQuotation();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('QUO');";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','QUO');";

                    //toolboxVM.RecallBtn.Visible = true;
                    //toolboxVM.RecallBtn.Text = "Recall";
                    //toolboxVM.RecallBtn.Title = "Document Recall";
                    //toolboxVM.RecallBtn.Event = "RecallDocumentItem('QUO');";
                    break;
                case "LockDocument":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddQuotation();";

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
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','QUO');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','QUO');";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Disable = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.DisableReason = "Not Approved";
                    toolboxVM.PrintBtn.Event = "";

                   
                    break;

                case "Approved":                                        
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddQuotation();";

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
                    toolboxVM.EmailBtn.Event = "EmailQuotation();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','QUO');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','QUO');";


                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.Title = "Print Document";
                    toolboxVM.PrintBtn.Event = "PrintQuotation()";
                 
                    //toolboxVM.RecallBtn.Visible = true;
                    //toolboxVM.RecallBtn.Text = "Recall";
                    //toolboxVM.RecallBtn.Title = "Document Recall";
                    //toolboxVM.SendForApprovalBtn.Disable = true;
                    //toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    //toolboxVM.RecallBtn.Event = "";
                    
                    break;

                case "Recalled":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddQuotation();";

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
                    toolboxVM.EmailBtn.Event = "EmailQuotation();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Disable = true;
                    toolboxVM.SendForApprovalBtn.DisableReason = "Document Locked";
                    toolboxVM.SendForApprovalBtn.Event = "";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','QUO');";

                    toolboxVM.HistoryBtn.Visible = true;
                    toolboxVM.HistoryBtn.Text = "History";
                    toolboxVM.HistoryBtn.Title = "Document History";
                    toolboxVM.HistoryBtn.Event = "ApprovalHistoryList('" + id.ToString() + "','QUO');";


                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Print";
                    toolboxVM.PrintBtn.Title = "Print Document";
                    toolboxVM.PrintBtn.Event = "PrintQuotation()";

                    toolboxVM.RecallBtn.Visible = true;
                    toolboxVM.RecallBtn.Text = "Recall";
                    toolboxVM.RecallBtn.Title = "Document Recall";
                    toolboxVM.RecallBtn.Event = "RecallDocumentItem('QUO');";
                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveQuotation();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetQuotation();";

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