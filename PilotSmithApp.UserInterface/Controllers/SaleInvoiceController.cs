using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class SaleInvoiceController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        ISaleInvoiceBusiness _saleInvoiceBusiness;
        ICustomerBusiness _customerBusiness;
        IBranchBusiness _branchBusiness;
        IEstimateBusiness _estimateBusiness;
        public SaleInvoiceController(ISaleInvoiceBusiness saleInvoiceBusiness, ICustomerBusiness customerBusiness, IBranchBusiness branchBusiness, IEstimateBusiness estimateBusiness)
        {
            _saleInvoiceBusiness = saleInvoiceBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
            _estimateBusiness = estimateBusiness;
        }
        // GET: SaleInvoice
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region SaleInvoice Form
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult SaleInvoiceForm(Guid id, Guid? estimateID)
        {
            SaleInvoiceViewModel saleInvoiceVM = null;
            try
            {
                if (id != Guid.Empty)
                {
                    saleInvoiceVM = Mapper.Map<SaleInvoice, SaleInvoiceViewModel>(_saleInvoiceBusiness.GetSaleInvoice(id));
                    saleInvoiceVM.IsUpdate = true;
                }
                else if (id == Guid.Empty && estimateID == null)
                {
                    saleInvoiceVM = new SaleInvoiceViewModel();
                    saleInvoiceVM.IsUpdate = false;
                    saleInvoiceVM.ID = Guid.Empty;
                    //saleInvoiceVM.EstimateID = null;
                    saleInvoiceVM.DocumentStatusCode = 5;
                }
                else if (id == Guid.Empty && estimateID != null)
                {
                    EstimateViewModel estimateVM = Mapper.Map<Estimate, EstimateViewModel>(_estimateBusiness.GetEstimate((Guid)estimateID));
                    saleInvoiceVM = new SaleInvoiceViewModel();
                    saleInvoiceVM.IsUpdate = false;
                    saleInvoiceVM.ID = Guid.Empty;
                    //saleInvoiceVM.CustomerID = estimateVM.CustomerID;
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
        #region Get SaleInvoice DetailList By SaleInvoiceID with Estimate
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public string GetSaleInvoiceDetailListBySaleInvoiceIDWithEstimate(Guid estimateID)
        {
            try
            {
                List<SaleInvoiceDetailViewModel> saleInvoiceItemViewModelList = new List<SaleInvoiceDetailViewModel>();
                if (estimateID != Guid.Empty)
                {
                    List<EstimateDetailViewModel> estimateVMList = Mapper.Map<List<EstimateDetail>, List<EstimateDetailViewModel>>(_estimateBusiness.GetEstimateDetailListByEstimateID(estimateID));
                    foreach (EstimateDetailViewModel estimateDetailVM in estimateVMList)
                    {
                        SaleInvoiceDetailViewModel saleInvoiceDetailVM = new SaleInvoiceDetailViewModel()
                        {
                            ID = Guid.Empty,
                            //QuoteID = Guid.Empty,
                            ProductID = estimateDetailVM.ProductID,
                            ProductModelID = estimateDetailVM.ProductModelID,
                            ProductSpec = estimateDetailVM.ProductSpec,
                            Qty = estimateDetailVM.Qty,
                            UnitCode = estimateDetailVM.UnitCode,
                            Rate = estimateDetailVM.SellingRate,
                            //CGSTAmt = 0,
                            //IGSTAmt = 0,
                            //SGSTAmt = 0,
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
        #endregion Get SaleInvoice DetailList By SaleInvoiceID with Estimate
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
                AppUA appUA = Session["AppUA"] as AppUA;
                saleInvoiceVM.PSASysCommon = new PSASysCommonViewModel();
                saleInvoiceVM.PSASysCommon.CreatedBy = appUA.UserName;
                saleInvoiceVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                saleInvoiceVM.PSASysCommon.UpdatedBy = appUA.UserName;
                saleInvoiceVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(saleInvoiceVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                saleInvoiceVM.SaleInvoiceDetailList = JsonConvert.DeserializeObject<List<SaleInvoiceDetailViewModel>>(ReadableFormat);
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
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SaleInvoice", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
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

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteSaleInvoice();";

                    toolboxVM.EmailBtn.Visible = true;
                    toolboxVM.EmailBtn.Text = "Email";
                    toolboxVM.EmailBtn.Title = "Email";
                    toolboxVM.EmailBtn.Event = "EmailSaleInvoice();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
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