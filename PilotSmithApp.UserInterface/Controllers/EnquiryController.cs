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
        IEnquiryBusiness _enquiryBusiness;
        ICustomerBusiness _customerBusiness;
        IBranchBusiness _branchBusiness;
        public EnquiryController(IEnquiryBusiness enquiryBusiness, ICustomerBusiness customerBusiness,IBranchBusiness branchBusiness)
        {
            _enquiryBusiness = enquiryBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
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
                    //customerVM = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomer(id));
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_EnquiryForm", enquiryVM);
        }
        #endregion Enquiry Form
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