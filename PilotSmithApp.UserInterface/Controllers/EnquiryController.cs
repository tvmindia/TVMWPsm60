using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class EnquiryController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IEnquiryBusiness _enquiryBusiness;
        ICustomerBusiness _customerBusiness;
        IBranchBusiness _branchBusiness;
        IEnquiryGradeBusiness _enquiryGradeBusiness;
        ICommonBusiness _commonBusiness;
        IAreaBusiness _areaBusiness;
        IReferencePersonBusiness _referencePersonBusiness;
        IDocumentStatusBusiness _documentStatusBusiness;
        private IUserBusiness _userBusiness;
        SecurityFilter.ToolBarAccess _tool;

        public EnquiryController(IEnquiryBusiness enquiryBusiness, ICustomerBusiness customerBusiness,
            IBranchBusiness branchBusiness, IEnquiryGradeBusiness enquiryGradeBusiness,
            ICommonBusiness commonBusiness, IAreaBusiness areaBusiness,
            IReferencePersonBusiness referencePersonBusiness, IDocumentStatusBusiness documentStatusBusiness, 
            IUserBusiness userBusiness, SecurityFilter.ToolBarAccess tool)
        {
            _enquiryBusiness = enquiryBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
            _enquiryGradeBusiness = enquiryGradeBusiness;
            _commonBusiness = commonBusiness;
            _areaBusiness= areaBusiness;
            _referencePersonBusiness = referencePersonBusiness;
            _documentStatusBusiness = documentStatusBusiness;
            _userBusiness = userBusiness;
            _tool = tool;

        }
        // GET: Enquiry
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
        public ActionResult Index(string id)
        {
            ViewBag.ID = id;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            EnquiryAdvanceSearchViewModel enquiryAdvanceSearchVM = new EnquiryAdvanceSearchViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            enquiryAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            enquiryAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("ENQ");
            return View(enquiryAdvanceSearchVM);
        }
        #region Enquiry Form
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
        public ActionResult EnquiryForm(Guid id)
        {
            EnquiryViewModel enquiryVM = null;
            try
            {

                if (id != Guid.Empty)
                {
                    enquiryVM = Mapper.Map<Enquiry, EnquiryViewModel>(_enquiryBusiness.GetEnquiry(id));
                    enquiryVM.IsUpdate = true;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    enquiryVM.IsDocLocked = enquiryVM.DocumentOwners.Contains(appUA.UserName);
                    enquiryVM.Customer = new CustomerViewModel
                    {
                        Titles = new TitlesViewModel()
                        {
                            TitlesSelectList = _customerBusiness.GetTitleSelectList(),
                        },
                    };
                }
                else
                {
                    enquiryVM = new EnquiryViewModel();
                    enquiryVM.IsUpdate = false;
                    enquiryVM.ID = Guid.Empty;
                    enquiryVM.DocumentStatus = new DocumentStatusViewModel();
                    enquiryVM.DocumentStatus.Description = "-";
                    enquiryVM.Branch = new BranchViewModel();
                    enquiryVM.Branch.Description = "-";
                    enquiryVM.IsDocLocked = false;
                    enquiryVM.Customer = new CustomerViewModel
                    {
                        Titles = new TitlesViewModel()
                        {
                            TitlesSelectList = _customerBusiness.GetTitleSelectList(),
                        },
                    };
                }

                enquiryVM.EnquiryGrade = new EnquiryGradeViewModel()
                {
                    EnquiryGradeSelectList = _enquiryGradeBusiness.GetEnquiryGradeSelectList()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_EnquiryForm", enquiryVM);
        }
        #endregion Enquiry Form
        #region Enquiry Detail Add
        public ActionResult AddEnquiryDetail()
        {
            EnquiryDetailViewModel enquiryDetailVM = new EnquiryDetailViewModel();
            enquiryDetailVM.IsUpdate = false;
            return PartialView("_AddEnquiryDetail", enquiryDetailVM);
        }
        #endregion Enquiry Detail Add
        #region Get Enquiry DetailList By EnquiryID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
        public string GetEnquiryDetailListByEnquiryID(Guid enquiryID)
        {
            try
            {
                List<EnquiryDetailViewModel> enquiryItemViewModelList = new List<EnquiryDetailViewModel>();
                if (enquiryID == Guid.Empty)
                {
                    EnquiryDetailViewModel enquiryDetailVM = new EnquiryDetailViewModel()
                    {
                        ID = Guid.Empty,
                        EnquiryID = Guid.Empty,
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
                    };
                    enquiryItemViewModelList.Add(enquiryDetailVM);
                }
                else
                {
                    enquiryItemViewModelList = Mapper.Map<List<EnquiryDetail>, List<EnquiryDetailViewModel>>(_enquiryBusiness.GetEnquiryDetailListByEnquiryID(enquiryID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = enquiryItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Enquiry DetailList By EnquiryID
        #region Delete Enquiry
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "D")]
        public string DeleteEnquiry(Guid id)
        {

            try
            {
                object result = _enquiryBusiness.DeleteEnquiry(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete Enquiry
        #region Delete Enquiry Detail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "D")]
        public string DeleteEnquiryDetail(Guid id)
        {

            try
            {
                object result = _enquiryBusiness.DeleteEnquiryDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }


        }
        #endregion Delete Enquiry Detail
        #region GetAllEnquiry
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
        public JsonResult GetAllEnquiry(DataTableAjaxPostModel model, EnquiryAdvanceSearchViewModel EnquiryAdvanceSearchVM)
        {
            //setting options to our model
            EnquiryAdvanceSearchVM.DataTablePaging.Start = model.start;
            EnquiryAdvanceSearchVM.DataTablePaging.Length = (EnquiryAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : EnquiryAdvanceSearchVM.DataTablePaging.Length;

            //EnquiryAdvanceSearchVM.OrderColumn = model.order[0].column;
            //EnquiryAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<EnquiryViewModel> EnquiryVMList = Mapper.Map<List<Enquiry>, List<EnquiryViewModel>>(_enquiryBusiness.GetAllEnquiry(Mapper.Map<EnquiryAdvanceSearchViewModel, EnquiryAdvanceSearch>(EnquiryAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = EnquiryVMList.Count != 0 ? EnquiryVMList[0].TotalCount : 0,
                recordsFiltered = EnquiryVMList.Count != 0 ? EnquiryVMList[0].FilteredCount : 0,
                data = EnquiryVMList
            });
        }
        #endregion GetAllEnquiry
        #region InsertUpdateEnquiry
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "W")]
        public string InsertUpdateEnquiry(EnquiryViewModel enquiryVM)
        {
            //object resultFromBusiness = null;

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                enquiryVM.PSASysCommon = new PSASysCommonViewModel();
                enquiryVM.PSASysCommon.CreatedBy = appUA.UserName;
                enquiryVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                enquiryVM.PSASysCommon.UpdatedBy = appUA.UserName;
                enquiryVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(enquiryVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                enquiryVM.EnquiryDetailList = JsonConvert.DeserializeObject<List<EnquiryDetailViewModel>>(ReadableFormat);
                object result = _enquiryBusiness.InsertUpdateEnquiry(Mapper.Map<EnquiryViewModel, Enquiry>(enquiryVM));

                if (enquiryVM.ID == Guid.Empty)
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

        #endregion InsertUpdateEnquiry
        #region Get Enquiry SelectList On Demand
        public ActionResult GetEnquiryForSelectListOnDemand(string searchTerm)
        {
            List<EnquiryViewModel> enquiryVMList = string.IsNullOrEmpty(searchTerm) ? null : Mapper.Map<List<Enquiry>, List<EnquiryViewModel>>(_enquiryBusiness.GetEnquiryForSelectListOnDemand(searchTerm));
            var list = new List<Select2Model>();
            if (enquiryVMList != null)
            {
                foreach (EnquiryViewModel enquiryVM in enquiryVMList)
                {
                    list.Add(new Select2Model()
                    {
                        text = enquiryVM.EnquiryNo,
                        id = enquiryVM.ID.ToString()
                    });
                }
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion Get Enquiry SelectList On Demand
        #region EnquirySelectList
        public ActionResult EnquirySelectList(string required, Guid? id)
        {
            ViewBag.IsRequired = required;
            EnquiryViewModel enquiryVM = new EnquiryViewModel();
            enquiryVM.EnquirySelectList = _enquiryBusiness.GetEnquiryForSelectList(id);
            return PartialView("_EnquirySelectList", enquiryVM);
        }
        #endregion EnquirySelectList
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName,"Enquiry");
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddEnquiry();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportEnquiryData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetEnquiryList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddEnquiry();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveEnquiry();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetEnquiry();";

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('"+ id.ToString() + "','ENQ');";



                    if (_commonBusiness.CheckDocumentIsDeletable("ENQ", id))
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
                        toolboxVM.deletebtn.Event = "DeleteEnquiry();";
                    }
                   

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

                    toolboxVM.TimeLine.Visible = true;
                    toolboxVM.TimeLine.Text = "TimeLn";
                    toolboxVM.TimeLine.Title = "TimeLine";
                    toolboxVM.TimeLine.Event = "GetTimeLine('" + id.ToString() + "','ENQ');";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Disable = true;
                    toolboxVM.deletebtn.DisableReason = "Document Locked";
                    toolboxVM.deletebtn.Event = "";
                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveEnquiry();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetEnquiry();";

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