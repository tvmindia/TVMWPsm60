﻿using AutoMapper;
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
    public class EnquiryFollowupController : Controller
    {
        AppConst _appConstant = new AppConst();
        PSASysCommon _pSASysCommon = new PSASysCommon();
        IEnquiryFollowupBusiness _enquiryFollowupBusiness;
        public EnquiryFollowupController(IEnquiryFollowupBusiness enquiryFollowupBusiness)
        {
            _enquiryFollowupBusiness = enquiryFollowupBusiness;
        }
        // GET: EnquiryFollowup
        public ActionResult Index()
        {
            EnquiryFollowupViewModel enquiryFollowupVM = new EnquiryFollowupViewModel();
            enquiryFollowupVM.EnquiryID = Guid.Parse("f00764f6-974a-40b5-8410-317c41d2e5bc");
            //enquiryVm.TotalCount = 11;
            ViewBag.Ispager = false;
            enquiryFollowupVM.DataTablePaging = new DataTablePagingViewModel()
            {
                Length = 3,
                Start = 0
            };
            enquiryFollowupVM.EnquiryFollowupList = Mapper.Map<List<EnquiryFollowup>, List<EnquiryFollowupViewModel>>(_enquiryFollowupBusiness.GetAllEnquiryFollowup(Mapper.Map<EnquiryFollowupViewModel, EnquiryFollowup>(enquiryFollowupVM)));
            return View(enquiryFollowupVM);
        }
        public ActionResult GetEnquiryFollowupList(EnquiryFollowupViewModel enquiryFollowupVM)
        {
            ViewBag.Ispager = false;
            if(enquiryFollowupVM.DataTablePaging==null)
            {
                enquiryFollowupVM.DataTablePaging = new DataTablePagingViewModel()
                {
                    Length = 3,
                    Start = 0
                };
            }
            else
            {
                enquiryFollowupVM.DataTablePaging.Length = 3;
                ViewBag.Ispager = true;
            }
            enquiryFollowupVM.EnquiryFollowupList = Mapper.Map<List<EnquiryFollowup>, List<EnquiryFollowupViewModel>>(_enquiryFollowupBusiness.GetAllEnquiryFollowup(Mapper.Map<EnquiryFollowupViewModel, EnquiryFollowup>(enquiryFollowupVM)));
            enquiryFollowupVM.TotalCount = enquiryFollowupVM.EnquiryFollowupList.Count > 0 ? enquiryFollowupVM.EnquiryFollowupList[0].TotalCount : 0;
            return PartialView("_EnquiryFollowupList", enquiryFollowupVM);
        }
        public ActionResult AddEnquiryFollowup(EnquiryFollowupViewModel enquiryFollowupVM)
        {
            enquiryFollowupVM.IsUpdate = false;
            if (enquiryFollowupVM.ID!=Guid.Empty)
            {
                enquiryFollowupVM = Mapper.Map<EnquiryFollowup, EnquiryFollowupViewModel>(_enquiryFollowupBusiness.GetEnquiryFollowup(enquiryFollowupVM.ID));
                enquiryFollowupVM.IsUpdate = true;
            }
            return PartialView("_AddEnquiryFollowup", enquiryFollowupVM);
        }
        #region InsertUpdate EnquiryFollowup
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "EnquiryFollowup", Mode = "R")]
        public string InsertUpdateEnquiryFollowup(EnquiryFollowupViewModel enquiryFollowupVM)
        {

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                enquiryFollowupVM.PSASysCommon = new PSASysCommonViewModel();
                enquiryFollowupVM.PSASysCommon.CreatedBy = appUA.UserName;
                enquiryFollowupVM.PSASysCommon.CreatedDate = _pSASysCommon.GetCurrentDateTime();
                enquiryFollowupVM.PSASysCommon.UpdatedBy = appUA.UserName;
                enquiryFollowupVM.PSASysCommon.UpdatedDate = _pSASysCommon.GetCurrentDateTime();
                
                object result = _enquiryFollowupBusiness.InsertUpdateEnquiryFollowup(Mapper.Map<EnquiryFollowupViewModel, EnquiryFollowup>(enquiryFollowupVM));

                if (enquiryFollowupVM.ID == Guid.Empty)
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

        #endregion InsertUpdate EnquiryFollowup
    }
}