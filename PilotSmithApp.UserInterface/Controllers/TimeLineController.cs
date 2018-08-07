using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using AutoMapper;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class TimeLineController : Controller
    {
        // GET: TimeLine
        private ICommonBusiness _commonBusiness;
        public TimeLineController(ICommonBusiness commonBusiness) {

            _commonBusiness = commonBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetTimeLine(String Id, string Type) {

            List<TimeLineViewModel> Result = new List<TimeLineViewModel>();
            TimeLineViewModelList List = new TimeLineViewModelList();
            if (Id!=null && Id != "") {
                Result = Mapper.Map<List<TimeLine>, List<TimeLineViewModel>>(_commonBusiness.GetTimeLine(Guid.Parse(Id), Type));
                List.TimeLineList = Result;
                List.CurrentDocument = (from A in Result where (A.DocumentID == Guid.Parse(Id)) select A.DocumentNo.ToString()).FirstOrDefault();
            }
                      
            return PartialView("_TimeLine", List);
          
        }
    }
}