using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.BusinessServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PilotSmithApp.UserInterface.Controllers
{
    public class EstimateController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IEstimateBusiness _estimateBusiness;
        ICustomerBusiness _customerBusiness;
        IBranchBusiness _branchBusiness;
        IEnquiryBusiness _enquiryBusiness;
        ICommonBusiness _commonBusiness;
        IAreaBusiness _areaBusiness;
        IReferencePersonBusiness _referencePersonBusiness;
        IDocumentStatusBusiness _documentStatusBusiness;
        private IUserBusiness _userBusiness;

        public EstimateController(IEstimateBusiness estimateBusiness, ICustomerBusiness customerBusiness,
            IBranchBusiness branchBusiness, IEnquiryBusiness enquiryBusiness, ICommonBusiness commonBusiness,
            IAreaBusiness areaBusiness,IReferencePersonBusiness referencePersonBusiness,
            IDocumentStatusBusiness documentStatusBusiness, IUserBusiness userBusiness)
        {
            _estimateBusiness = estimateBusiness;
            _customerBusiness = customerBusiness;
            _branchBusiness = branchBusiness;
            _enquiryBusiness = enquiryBusiness;
            _commonBusiness = commonBusiness;
            _areaBusiness = areaBusiness;
            _referencePersonBusiness = referencePersonBusiness;
            _documentStatusBusiness = documentStatusBusiness;
            _userBusiness = userBusiness;
        }
        // GET: Estimate
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public ActionResult Index()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            EstimateAdvanceSearchViewModel estimateAdvanceSearchVM = new EstimateAdvanceSearchViewModel();
            estimateAdvanceSearchVM.Area = new AreaViewModel();
            estimateAdvanceSearchVM.Area.AreaSelectList = selectListItem;
            estimateAdvanceSearchVM.Customer = new CustomerViewModel();
            estimateAdvanceSearchVM.Customer.CustomerSelectList = selectListItem;
            estimateAdvanceSearchVM.ReferencePerson = new ReferencePersonViewModel();
            estimateAdvanceSearchVM.ReferencePerson.ReferencePersonSelectList = selectListItem;
            estimateAdvanceSearchVM.Branch = new BranchViewModel();
            estimateAdvanceSearchVM.Branch.BranchList = selectListItem;
            estimateAdvanceSearchVM.DocumentStatus = new DocumentStatusViewModel();
            estimateAdvanceSearchVM.DocumentStatus.DocumentStatusSelectList = _documentStatusBusiness.GetSelectListForDocumentStatus("EST"); ;
            estimateAdvanceSearchVM.PSAUser = new PSAUserViewModel();
            estimateAdvanceSearchVM.PSAUser.UserSelectList = selectListItem;
            return View(estimateAdvanceSearchVM);
        } 

        #region GetEstimateForm
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public ActionResult EstimateForm(Guid id,Guid? enquiryID)
        {
            EstimateViewModel estimateVM = null;
           try
            {
                if (id != Guid.Empty)
                {
                    estimateVM = Mapper.Map<Estimate, EstimateViewModel>(_estimateBusiness.GetEstimate(id));
                    estimateVM.IsUpdate = true;
                    AppUA appUA = Session["AppUA"] as AppUA;
                    estimateVM.IsDocLocked = estimateVM.DocumentOwners.Contains(appUA.UserName);
                    estimateVM.EnquirySelectList = _enquiryBusiness.GetEnquiryForSelectList(enquiryID);
                }
                else if(id==Guid.Empty && enquiryID==null)
                {
                    estimateVM = new EstimateViewModel();
                    estimateVM.IsUpdate = false;
                    estimateVM.ID = Guid.Empty;
                    estimateVM.EnquiryID = null;
                    estimateVM.EnquirySelectList = new List<SelectListItem>();
                    estimateVM.DocumentStatus = new DocumentStatusViewModel();
                    estimateVM.DocumentStatus.Description = "OPEN";
                    estimateVM.Branch = new BranchViewModel();
                    estimateVM.Branch.Description = "-";
                }
                else if(id==Guid.Empty && enquiryID!=null)
                {
                    EnquiryViewModel enquiryVM = Mapper.Map<Enquiry, EnquiryViewModel>(_enquiryBusiness.GetEnquiry((Guid)enquiryID));
                    estimateVM = new EstimateViewModel();
                    estimateVM.IsUpdate = false;
                    estimateVM.ID = Guid.Empty;
                    estimateVM.CustomerID = enquiryVM.CustomerID;
                    estimateVM.BranchCode = enquiryVM.BranchCode;
                    estimateVM.EnquirySelectList = _enquiryBusiness.GetEnquiryForSelectList(enquiryID);
                    estimateVM.EnquiryID = enquiryID;
                    estimateVM.DocumentStatus = new DocumentStatusViewModel();
                    estimateVM.DocumentStatus.Description = "OPEN";
                    estimateVM.Branch = new BranchViewModel();
                    estimateVM.Branch.Description = "-";
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("_EstimateForm", estimateVM);
        }
        #endregion GetEstimateForm      

        #region Estimate Detail Add
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public ActionResult AddEstimateDetail()
         {
            EstimateDetailViewModel estimateDetailVM = new EstimateDetailViewModel();
            estimateDetailVM.IsUpdate = false;
            return PartialView("_AddEstimateDetail", estimateDetailVM);
        }
        #endregion Estimate Detail Add

        #region GetAllEstimate       
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public JsonResult GetAllEstimate(DataTableAjaxPostModel model, EstimateAdvanceSearchViewModel estimateAdvanceSearchVM)
        {
            //setting options to our model
            estimateAdvanceSearchVM.DataTablePaging.Start = model.start;
            estimateAdvanceSearchVM.DataTablePaging.Length = (estimateAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : estimateAdvanceSearchVM.DataTablePaging.Length;

           

            // action inside a standard controller
            List<EstimateViewModel> estimateVMList = Mapper.Map<List<Estimate>, List<EstimateViewModel>>(_estimateBusiness.GetAllEstimate(Mapper.Map<EstimateAdvanceSearchViewModel, EstimateAdvanceSearch>(estimateAdvanceSearchVM)));
            if (estimateAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = estimateVMList.Count != 0 ? estimateVMList[0].TotalCount : 0;
                int filteredResult = estimateVMList.Count != 0 ? estimateVMList[0].FilteredCount : 0;
                estimateVMList = estimateVMList.Skip(0).Take(filteredResult > 1000 ? 1000 : filteredResult).ToList();
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
                recordsTotal = estimateVMList.Count != 0 ? estimateVMList[0].TotalCount : 0,
                recordsFiltered = estimateVMList.Count != 0 ? estimateVMList[0].FilteredCount : 0,
                data = estimateVMList
            });
        }
        #endregion GetAllEstimate

        #region InsertUpdateEstimate
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "W")]
        public string InsertUpdateEstimate(EstimateViewModel estimateVM)
        {
            //object resultFromBusiness = null;

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                estimateVM.PSASysCommon = new PSASysCommonViewModel();
                estimateVM.PSASysCommon.CreatedBy = appUA.UserName;
                estimateVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                estimateVM.PSASysCommon.UpdatedBy = appUA.UserName;
                estimateVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                object ResultFromJS = JsonConvert.DeserializeObject(estimateVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                estimateVM.EstimateDetailList = JsonConvert.DeserializeObject<List<EstimateDetailViewModel>>(ReadableFormat);
                object result = _estimateBusiness.InsertUpdateEstimate(Mapper.Map<EstimateViewModel, Estimate>(estimateVM));

                if (estimateVM.ID == Guid.Empty)
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

        #endregion InsertUpdateEstimate

        # region EstimateSelectList
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public ActionResult EstimateSelectList(string required, Guid? id)
        {
            ViewBag.IsRequired = required;
            EstimateViewModel estimateVM = new EstimateViewModel();
            estimateVM.EstimateSelectList = _estimateBusiness.GetEstimateForSelectList(id==Guid.Empty?null:id);
            return PartialView("_EstimateSelectList", estimateVM);
        }
        #endregion EstimateSelectList

        #region GetEstimateForSelectListOnDemand
        public ActionResult GetEstimateForSelectListOnDemand(string searchTerm)
        {
            List<EstimateViewModel> estimateVMList = string.IsNullOrEmpty(searchTerm) ? null : Mapper.Map<List<Estimate>, List<EstimateViewModel>>(_estimateBusiness.GetEstimateForSelectListOnDemand(searchTerm));
            var list = new List<Select2Model>();
            if(estimateVMList!=null)
            {
                foreach(EstimateViewModel estimateVM in estimateVMList)
                {
                    list.Add(new Select2Model()
                    {
                        text=estimateVM.EstimateNo,
                        id=estimateVM.ID.ToString()
                    });

                }
            }
            return Json(new { items = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion GetEstimateForSelectListOnDemand

        #region Get Estimate DetailList By EstimateID
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public string GetEstimateDetailListByEstimateID(Guid estimateID)
        {
            try
            {
                List<EstimateDetailViewModel> estimateItemViewModelList = new List<EstimateDetailViewModel>();
                if (estimateID == Guid.Empty)
                {
                    EstimateDetailViewModel estimateDetailVM = new EstimateDetailViewModel()
                    {
                        ID = Guid.Empty,
                        EstimateID = Guid.Empty,
                        ProductID = Guid.Empty,
                        ProductModelID = Guid.Empty,
                        ProductSpec = string.Empty,
                        Qty = 0,
                        UnitCode = null,
                        CostRate = 0,
                        SellingRate=0,
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
                    estimateItemViewModelList.Add(estimateDetailVM);
                }
                else
                {
                    estimateItemViewModelList = Mapper.Map<List<EstimateDetail>, List<EstimateDetailViewModel>>(_estimateBusiness.GetEstimateDetailListByEstimateID(estimateID));
                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = estimateItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Estimate DetailList By EstimateID

        #region Get Estimate DetailList By EstimateID with Enquiry
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public string GetEstimateDetailListByEstimateIDWithEnquiry(Guid enquiryID)
        {
            try
            {
                List<EstimateDetailViewModel> estimateItemViewModelList = new List<EstimateDetailViewModel>();
                if (enquiryID != Guid.Empty)
                {
                    List<EnquiryDetailViewModel> enquiryDetailVMList = Mapper.Map<List<EnquiryDetail>, List<EnquiryDetailViewModel>>(_enquiryBusiness.GetEnquiryDetailListByEnquiryID(enquiryID));
                    estimateItemViewModelList = (from enquiryDetailVM in enquiryDetailVMList
                                          select new EstimateDetailViewModel
                                          {
                                              ID = Guid.Empty,
                                              EstimateID = Guid.Empty,
                                              ProductID = enquiryDetailVM.ProductID,
                                              ProductModelID = enquiryDetailVM.ProductModelID,
                                              ProductSpec = enquiryDetailVM.ProductSpec,
                                              Qty = enquiryDetailVM.Qty,
                                              UnitCode = enquiryDetailVM.UnitCode,
                                              CostRate = enquiryDetailVM.ProductModel.CostPrice,
                                              SellingRate = enquiryDetailVM.Rate,
                                              SpecTag = enquiryDetailVM.SpecTag,


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
                                          }).ToList();

                }
                return JsonConvert.SerializeObject(new { Status = "OK", Records = estimateItemViewModelList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion Get Estimate DetailList By EstimateID with Enquiry

        #region DeleteEstimate
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "D")]
        public string DeleteEstimate(Guid id)
        {
            try
            {
                object result = _estimateBusiness.DeleteEstimate(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteEstimate

        #region DeleteEstimateDetail
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "D")]
        public string DeleteEstimateDetail(Guid id)
        {
            try
            {
                object result = _estimateBusiness.DeleteEstimateDetail(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Sucess" });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteEstimateDetail

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Estimate", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType, Guid? id)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddEstimate();";

                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export to excel";
                    toolboxVM.ExportBtn.Event = "ExportEstimateData()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetEstimateList();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddEstimate();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "SaveEstimate();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetEstimate();";

                    if (_commonBusiness.CheckDocumentIsDeletable("EST", id))
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
                        toolboxVM.deletebtn.Event = "DeleteEstimate();";
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
                    toolboxVM.savebtn.Event = "SaveEstimate();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "ResetEstimate();";

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