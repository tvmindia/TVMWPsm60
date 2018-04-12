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
    public class EnquiryController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IEnquiryBusiness _enquiryBusiness;
        ICustomerBusiness _customerBusiness;
        IBranchBusiness _branchBusiness;
        IEnquiryGradeBusiness _enquiryGradeBusiness;
        public EnquiryController(IEnquiryBusiness enquiryBusiness, ICustomerBusiness customerBusiness,IBranchBusiness branchBusiness, IEnquiryGradeBusiness enquiryGradeBusiness)
        {
            _enquiryBusiness = enquiryBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
            _enquiryGradeBusiness = enquiryGradeBusiness;
        }
        // GET: Enquiry
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
        public ActionResult Index()
        {
            return View();
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
                }
                else
                {
                    enquiryVM = new EnquiryViewModel();
                    enquiryVM.IsUpdate = false;
                    enquiryVM.ID = Guid.Empty;
                }
                enquiryVM.Customer = new CustomerViewModel {
                    Titles=new TitlesViewModel() {
                        TitlesSelectList=_customerBusiness.GetTitleSelectList(),
                    }, 
                };
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
                if(enquiryID==Guid.Empty)
                {
                    EnquiryDetailViewModel enquiryDetailVM = new EnquiryDetailViewModel()
                    {
                        ID = Guid.Empty,
                        EnquiryID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        Qty = 0,
                        UnitCode = 1,
                        Rate=0,
                        Product=new ProductViewModel()
                        {
                            ID=Guid.Empty,
                            Code=string.Empty,
                            Name=string.Empty,
                        },
                        ProductModel=new ProductModelViewModel()
                        {
                            ID=Guid.Empty,
                            Name=string.Empty
                        },
                    };
                    enquiryItemViewModelList.Add(enquiryDetailVM);
                }
                else
                {
                    enquiryItemViewModelList = Mapper.Map<List<EnquiryDetail>, List<EnquiryDetailViewModel>>(_enquiryBusiness.GetEnquiryDetailListByEnquiryID(enquiryID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = enquiryItemViewModelList,Message="Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records ="" ,Message = cm.Message });
            }
        }
        #endregion Get Enquiry DetailList By EnquiryID
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
            if (EnquiryAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = EnquiryVMList.Count != 0 ? EnquiryVMList[0].TotalCount : 0;
                int filteredResult = EnquiryVMList.Count != 0 ? EnquiryVMList[0].FilteredCount : 0;
                EnquiryVMList = EnquiryVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = EnquiryVMList.Count != 0 ? EnquiryVMList[0].TotalCount : 0,
                recordsFiltered = EnquiryVMList.Count != 0 ? EnquiryVMList[0].FilteredCount : 0,
                data = EnquiryVMList
            });
        }
        #endregion GetAllEnquiry
        #region InsertUpdateEnquiry
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
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
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Insertion successfull" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Updation successfull" });
                }


            }
            catch (Exception ex)
            {

                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }

        #endregion InsertUpdateEnquiry
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Enquiry", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
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

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteEnquiry();";

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
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}