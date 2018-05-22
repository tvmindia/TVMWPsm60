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
    public class UserInBranchController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        private IUserBusiness _userBusiness;
        private IUserInBranchBusiness _userInBranchBusiness;

        public UserInBranchController(IUserBusiness userBusiness, IUserInBranchBusiness userInBranchBusiness)
        {
            _userBusiness = userBusiness;
            _userInBranchBusiness = userInBranchBusiness;
        }
        // GET: UserBranch
        public ActionResult Index()
        {
            UserInBranchViewModel userInBranchVM = new UserInBranchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<PSAUserViewModel> PSAUserVMList = Mapper.Map<List<SAMTool.DataAccessObject.DTO.User>, List<PSAUserViewModel>>(_userBusiness.GetAllUsers());

            if (PSAUserVMList != null)
                foreach (PSAUserViewModel PSAuVM in PSAUserVMList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = PSAuVM.UserName,
                        Value = PSAuVM.ID.ToString(),
                        Selected = false
                    });
                }
            userInBranchVM.PSAUser = new PSAUserViewModel();
            userInBranchVM.PSAUser.UserSelectList = selectListItem;
            return View(userInBranchVM);
        }

        #region GetAllUserInBranchByUserId
        [AuthSecurityFilter(ProjectObject = "UserInBranch", Mode = "R")]
        [HttpGet]
        public string GetAllUserInBranchByUserId(Guid userId)
        {
            try
            {
                List<UserInBranchViewModel> userList = Mapper.Map<List<UserInBranch>, List<UserInBranchViewModel>>(_userInBranchBusiness.GetAllUserInBranchByUserId(userId));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = userList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllUserInBranchByUserId

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "UserInBranch", Mode = "W")]
        public string InserUpdateUserInBranch(string userId,string hasAccess, string isDefault)
        {
            object result = null;
            try
            {
                UserInBranchViewModel userInBranchVM = new UserInBranchViewModel();
                userInBranchVM.UserID = Guid.Parse(userId.ToString());
                userInBranchVM.DefaultBranch = isDefault;
                userInBranchVM.HasAccessBranch = hasAccess;
               
                    AppUA appUA = Session["AppUA"] as AppUA;
                    userInBranchVM.PSASysCommon = new PSASysCommonViewModel();
                    userInBranchVM.PSASysCommon.CreatedBy = appUA.UserName;
                    userInBranchVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                    result = _userInBranchBusiness.InsertUpdateUserInBranch(Mapper.Map<UserInBranchViewModel, UserInBranch>(userInBranchVM));
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConstant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
           }
        }

        #region ButtonStyling
        [AuthSecurityFilter(ProjectObject = "UserInBranch", Mode = "R")]
        [HttpGet]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            Permission _permission = Session["UserRights"] as Permission;
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "Default":

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.savebtn.Visible = true;
                    }
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Event = "SaveChanges()";

                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goHome()";

                    break;
               
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion

    }
}