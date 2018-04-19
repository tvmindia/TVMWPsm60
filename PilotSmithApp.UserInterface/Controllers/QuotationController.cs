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
    public class QuotationController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IQuotationBusiness _quotationBusiness;
        ICustomerBusiness _customerBusiness;
        IBranchBusiness _branchBusiness;
        public QuotationController(IQuotationBusiness quotationBusiness, ICustomerBusiness customerBusiness, IBranchBusiness branchBusiness)
        {
            _quotationBusiness = quotationBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
        }
        // GET: Quotation
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }
        #region Quotation Form
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult QuotationForm(Guid id)
        {
            QuotationViewModel quotationVM = null;
            try
            {

                if (id != Guid.Empty)
                {
                    quotationVM = Mapper.Map<Quotation, QuotationViewModel>(_quotationBusiness.GetQuotation(id));
                    quotationVM.IsUpdate = true;
                }
                else
                {
                    quotationVM = new QuotationViewModel();
                    quotationVM.IsUpdate = false;
                    quotationVM.ID = Guid.Empty;
                }
                quotationVM.Customer = new CustomerViewModel
                {
                    Titles = new TitlesViewModel()
                    {
                        TitlesSelectList = _customerBusiness.GetTitleSelectList(),
                    },
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_QuotationForm", quotationVM);
        }
        #endregion Quotation Form
        #region Quotation Detail Add
        public ActionResult AddQuotationDetail()
        {
            QuotationDetailViewModel quotationDetailVM = new QuotationDetailViewModel();
            quotationDetailVM.IsUpdate = false;
            return PartialView("_AddQuotationDetail", quotationDetailVM);
        }
        #endregion Quotation Detail Add
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
            if (QuotationAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = QuotationVMList.Count != 0 ? QuotationVMList[0].TotalCount : 0;
                int filteredResult = QuotationVMList.Count != 0 ? QuotationVMList[0].FilteredCount : 0;
                QuotationVMList = QuotationVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = QuotationVMList.Count != 0 ? QuotationVMList[0].TotalCount : 0,
                recordsFiltered = QuotationVMList.Count != 0 ? QuotationVMList[0].FilteredCount : 0,
                data = QuotationVMList
            });
        }
        #endregion GetAllQuotation
        #region InsertUpdateQuotation
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public string InsertUpdateQuotation(QuotationViewModel quotationVM)
        {
            //object resultFromBusiness = null;

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                quotationVM.PSASysCommon = new PSASysCommonViewModel();
                quotationVM.PSASysCommon.CreatedBy = appUA.UserName;
                quotationVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                quotationVM.PSASysCommon.UpdatedBy = appUA.UserName;
                quotationVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(quotationVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                quotationVM.QuotationDetailList = JsonConvert.DeserializeObject<List<QuotationDetailViewModel>>(ReadableFormat);
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
        #region Calculate GST
        public ActionResult GSTCalculatedFields(QuotationDetailViewModel quotationDetailVM)
        {
            quotationDetailVM = Mapper.Map<QuotationDetail, QuotationDetailViewModel>(_quotationBusiness.CalculateGST(Mapper.Map<QuotationDetailViewModel, QuotationDetail>(quotationDetailVM)));
            return PartialView("_GSTCalculatedFields", quotationDetailVM);
        }
        #endregion Calculate GST
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Quotation", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
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

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteQuotation();";

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
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}