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
    public class DocumentApprovalController : Controller
    {
        private IDocumentApprovalBusiness _documentApprovalBusiness;
        private IDocumentTypeBusiness _documentTypeBusiness;
        PSASysCommon _pSASysCommon = new PSASysCommon();
        AppConst _appConst = new AppConst();
        SecurityFilter.ToolBarAccess _tool;
        public DocumentApprovalController(IDocumentApprovalBusiness documentApprovalBusiness, 
            IDocumentTypeBusiness documentTypeBusiness,SecurityFilter.ToolBarAccess tool)
        {
            _documentApprovalBusiness = documentApprovalBusiness;
            _documentTypeBusiness = documentTypeBusiness;
            _tool = tool;
        }

        // GET: DocumentApproval
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ViewPendingDocuments(string ID, string DocType, string DocID)
        {
            ViewBag.DocumentID = DocID;
            ViewBag.ApprovalLogID = ID;
            ViewBag.DocumentType = DocType;

            DocumentApprovalAdvanceSearchViewModel documentApprovalAdvanceSearchVM = new DocumentApprovalAdvanceSearchViewModel();
            documentApprovalAdvanceSearchVM.DocumentType = new DocumentTypeViewModel()
            {
                DocumentTypeSelectList = _documentTypeBusiness.GetDocumentTypeSelectList(),
            };          
            return View(documentApprovalAdvanceSearchVM);
        }
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ApproveDocument(string ID,string DocType,string DocID)
        {
            //ViewBag.SysModuleCode = code;
            ViewBag.DocumentID = DocID;
            ViewBag.ApprovalLogID = ID;
            ViewBag.DocumentType = DocType;
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ApprovalHistory()
        {
            ApprovalHistoryViewModel approvalHistoryVM = new ApprovalHistoryViewModel();
            return PartialView("_ApprovalHistory", approvalHistoryVM);
        }

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult DocumentSummary(DocumentSummaryViewModel documentSummaryVM)
        {
            documentSummaryVM.DataTable = _documentApprovalBusiness.GetDocumentSummary(documentSummaryVM.DocumentID, documentSummaryVM.DocumentTypeCode);
            return PartialView("_DocumentSummary", documentSummaryVM);
        }

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ViewApprovalHistory(string ID, string DocType, string DocID)
        {
            ViewBag.DocumentID = DocID;
            ViewBag.ApprovalLogID = ID;
            ViewBag.DocumentType = DocType;

            DocumentApprovalAdvanceSearchViewModel documentApprovalAdvanceSearchVM = new DocumentApprovalAdvanceSearchViewModel();
            documentApprovalAdvanceSearchVM.DocumentType = new DocumentTypeViewModel()
            {
                DocumentTypeSelectList = _documentTypeBusiness.GetDocumentTypeSelectList(),
            };
            return View(documentApprovalAdvanceSearchVM);
        }

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ApprovalHistoryList(string DocType, string DocID)
        {
            ViewBag.DocumentID = DocID;
            ViewBag.DocumentType = DocType;
            return PartialView("_ApprovalHistoryList");
        }

        #region GetAllApprovalHistory
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public JsonResult GetAllApprovalHistory(DataTableAjaxPostModel model, DocumentApprovalAdvanceSearchViewModel documentApprovalAdvanceSearchVM)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            documentApprovalAdvanceSearchVM.LoginName = appUA.UserName;
            documentApprovalAdvanceSearchVM.DataTablePaging.Start = model.start;
            documentApprovalAdvanceSearchVM.DataTablePaging.Length = (documentApprovalAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : documentApprovalAdvanceSearchVM.DataTablePaging.Length);
            List<DocumentApprovalViewModel> documentApprovalList = Mapper.Map<List<DocumentApproval>, List<DocumentApprovalViewModel>>(_documentApprovalBusiness.GetAllApprovalHistory(Mapper.Map<DocumentApprovalAdvanceSearchViewModel, DocumentApprovalAdvanceSearch>(documentApprovalAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            //if (documentApprovalAdvanceSearchVM.DataTablePaging.Length == -1)
            //{
            //    int totalResult = documentApprovalList.Count != 0 ? documentApprovalList[0].TotalCount : 0;
            //    int filteredResult = documentApprovalList.Count != 0 ? documentApprovalList[0].FilteredCount : 0;
            //    documentApprovalList = documentApprovalList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            //}
            return Json(new
            {
                draw = model.draw,
                recordsTotal = documentApprovalList.Count != 0 ? documentApprovalList[0].TotalCount : 0,
                recordsFiltered = documentApprovalList.Count != 0 ? documentApprovalList[0].FilteredCount : 0,
                data = documentApprovalList

            });
        }
        #endregion GetAllApprovalHistory

        #region Approvals
        //[AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult GetApprovers(string documentTypeCode)
        {
            DocumentApproverViewModel SendForApprovalVM = new DocumentApproverViewModel();
            SendForApprovalVM.SendForApprovalList = Mapper.Map<List<DocumentApprover>, List<DocumentApproverViewModel>>(_documentApprovalBusiness.GetApproversByDocType(documentTypeCode));
            SendForApprovalVM.ApproversCount = SendForApprovalVM.SendForApprovalList.Select(m => m.ApproverLevel).Distinct().Count();
            return PartialView("_SendForApproval", SendForApprovalVM);

        }
        #endregion Approvals
        #region GetAllDocumentApproval
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public JsonResult GetAllDocumentApproval(DataTableAjaxPostModel model, DocumentApprovalAdvanceSearchViewModel documentApprovalAdvanceSearchVM)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            documentApprovalAdvanceSearchVM.LoginName = appUA.UserName;
            documentApprovalAdvanceSearchVM.DataTablePaging.Start = model.start;
            documentApprovalAdvanceSearchVM.DataTablePaging.Length = (documentApprovalAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : documentApprovalAdvanceSearchVM.DataTablePaging.Length);
            List<DocumentApprovalViewModel> documentApprovalList = Mapper.Map<List<DocumentApproval>, List<DocumentApprovalViewModel>>(_documentApprovalBusiness.GetAllDocumentsPendingForApprovals(Mapper.Map<DocumentApprovalAdvanceSearchViewModel, DocumentApprovalAdvanceSearch>(documentApprovalAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = documentApprovalList.Count != 0 ? documentApprovalList[0].TotalCount : 0,
                recordsFiltered = documentApprovalList.Count != 0 ? documentApprovalList[0].FilteredCount : 0,
                data = documentApprovalList

            });
        }
        #endregion GetAllDocumentApproval

        #region GetApprovalHistory
        //[AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public string GetApprovalHistory(string documentID, string documentTypeCode)
            {
            try
            {
                List<ApprovalHistoryViewModel> approvalHistoryVMList = new List<ApprovalHistoryViewModel>();
                approvalHistoryVMList = Mapper.Map<List<ApprovalHistory>,List< ApprovalHistoryViewModel>>(_documentApprovalBusiness.GetApprovalHistory(Guid.Parse(documentID), documentTypeCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = approvalHistoryVMList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion GetApprovalHistory

        #region ApproveDocumentInsert
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public async Task<string> ApproveDocumentInsert(string ApprovalLogID, string DocumentID, string DocumentTypeCode, string Remarks)
        {
            try
            {
                DateTime approvalDate = _pSASysCommon.GetCurrentDateTime();
                var result = _documentApprovalBusiness.ApproveDocument(Guid.Parse(ApprovalLogID),Guid.Parse(DocumentID),DocumentTypeCode, approvalDate,Remarks);
                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(DocumentID), DocumentTypeCode);

                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion ApproveDocumentInsert 

        #region RejectDocument
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public async Task<string> RejectDocument(string ApprovalLogID, string DocumentID, string DocumentTypeCode,string Remarks)
        {
            try
            {
                DateTime rejectionDate = _pSASysCommon.GetCurrentDateTime();
                var result = _documentApprovalBusiness.RejectDocument(Guid.Parse(ApprovalLogID), Guid.Parse(DocumentID), DocumentTypeCode,Remarks, rejectionDate);
                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(DocumentID), DocumentTypeCode);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion RejectDocument

        #region ValidateDocumentsApprovalPermission
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public string ValidateDocumentsApprovalPermission( string DocumentID, string DocumentTypeCode)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string LoginName = appUA.UserName;
                var result = _documentApprovalBusiness.ValidateDocumentsApprovalPermission(LoginName, Guid.Parse(DocumentID), DocumentTypeCode);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion ValidateDocumentsApprovalPermission

        #region SendDocForApproval
        //[AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public async Task <string> SendDocForApproval(string documentID, string documentTypeCode,string approvers)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string createdBy = appUA.UserName;
                DateTime createdDate = _pSASysCommon.GetCurrentDateTime();

                var result = _documentApprovalBusiness.SendDocForApproval(Guid.Parse(documentID), documentTypeCode, approvers, createdBy, createdDate);

                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(documentID), documentTypeCode);

                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion SendDocForApproval

        #region ReSendDocForApproval
       // [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public async Task<string> ReSendDocForApproval(string documentID, string documentTypeCode, string latestApprovalID)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string createdBy = appUA.UserName;
                DateTime createdDate = _pSASysCommon.GetCurrentDateTime();

                var result = _documentApprovalBusiness.ReSendDocForApproval(Guid.Parse(documentID), documentTypeCode,Guid.Parse(latestApprovalID), createdBy, createdDate);
                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(documentID), documentTypeCode);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion ReSendDocForApproval

        #region RecallDocument
        public async Task <string> RecallDocument(string documentID,string documentTypeCode,string documentNo)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string createdBy = appUA.UserName;
                DateTime recallDate = _pSASysCommon.GetCurrentDateTime();
                DateTime createdDate = _pSASysCommon.GetCurrentDateTime();
                var result = _documentApprovalBusiness.RecallDocument(Guid.Parse(documentID),documentTypeCode,documentNo,recallDate,createdBy,createdDate);
                bool mailresult = await _documentApprovalBusiness.SendRecallMails(Guid.Parse(documentID),documentTypeCode);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion RecallDocument

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            Permission permission = _pSASysCommon.GetSecurityCode(appUA.UserName, "DocumentApproval");
            switch (actionType)
            {
                case "List":
                   
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPendingDocList();";
                    //----added for export button--------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportPendingDocs();";
                    //---------------------------------------
                    break;

                case "Back":
                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "Close();";//need to change function to rebind table

                    break;
                    
                case "Close":
                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    break;

                case "ApprovalHistory":
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetApprovalHistory();";
                    //---------------------------------------
                    toolboxVM.ExportBtn.Visible = true;
                    toolboxVM.ExportBtn.Text = "Export";
                    toolboxVM.ExportBtn.Title = "Export";
                    toolboxVM.ExportBtn.Event = "ExportApprovalHistory();";
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