using AutoMapper;
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
    public class DashBoardController : Controller
    {

        ISalesInvoieBusiness _salesInvoiceBusiness;
        ICommonBusiness _commonBusiness;
        public DashBoardController(ISalesInvoieBusiness salesInvoiceBusiness, ICommonBusiness commonBusiness) {
            _salesInvoiceBusiness = salesInvoiceBusiness;
            _commonBusiness = commonBusiness;
        }


        // GET: DashBoard
        [AuthSecurityFilter(ProjectObject = "DashBoard", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult SalesSummary()
        {

            SalesSummaryList salesList = new SalesSummaryList();
            salesList.SalesSummaryVMList = Mapper.Map<List<SalesSummary>, List<SalesSummaryViewModel>>(_salesInvoiceBusiness.GetSalesSummary());
            return PartialView("_SalesSummary", salesList);
        }

    }
}