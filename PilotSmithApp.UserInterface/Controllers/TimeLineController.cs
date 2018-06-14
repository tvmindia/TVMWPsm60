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

namespace PilotSmithApp.UserInterface.Controllers
{
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
        [AuthSecurityFilter(ProjectObject = "TimeLine", Mode = "R")]
        public ActionResult GetTimeLine(String Id, string Type) {

            List<TimeLineViewModel> Result= Mapper.Map<List<TimeLine>, List<TimeLineViewModel>>(_commonBusiness.GetTimeLine(Guid.Parse(Id), Type));
            return PartialView("_TimeLine", Result);
          
        }
    }
}