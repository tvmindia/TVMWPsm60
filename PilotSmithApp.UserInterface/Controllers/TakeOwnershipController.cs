﻿using AutoMapper;
using Newtonsoft.Json;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class TakeOwnershipController : Controller
    {
        AppConst _appConst = new AppConst();
        PSASysCommon _psaSysCommon = new PSASysCommon();
        private ITakeOwnershipBusiness _takeOwnershipBusiness;

        public TakeOwnershipController(ITakeOwnershipBusiness takeOwnershipBusiness)
        {
            _takeOwnershipBusiness = takeOwnershipBusiness;
        }
        // GET: TakeOwnership
        public ActionResult Index()
        {
            return View();
        }

        #region InsertUpdateTakeOwnership
        public string InsertTakeOwnership(DocumentLogViewModel documentLogVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                documentLogVM.PSASysCommon = new PSASysCommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _psaSysCommon.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _psaSysCommon.GetCurrentDateTime(),
                };
                var result = _takeOwnershipBusiness.InsertTakeOwnership(Mapper.Map<DocumentLogViewModel, DocumentLog>(documentLogVM));

                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion
        #region GetOwnershipHistory
        public string GetOwnershipHistory(string DocumentID, string DocumentTypeCode)
        {
            try
            {
                List<DocumentLogViewModel> ownershipHistoryVMList = new List<DocumentLogViewModel>();
                ownershipHistoryVMList = Mapper.Map<List<DocumentLog>, List<DocumentLogViewModel>>(_takeOwnershipBusiness.GetOwnershipHistory(Guid.Parse(DocumentID), DocumentTypeCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ownershipHistoryVMList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion GetOwnershipHistory
    }
}