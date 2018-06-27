using AutoMapper;
using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.UserInterface.Models;
using PilotSmithApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
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
        IEnquiryBusiness _enquiryBusiness;
        ISaleOrderBusiness _saleOrderBusiness;
        IProductionOrderBusiness _productionOrderBusiness;
        IQuotationBusiness _quotationBusiness;
        ICommonBusiness _commonBusiness;
        public DashBoardController(ISalesInvoieBusiness salesInvoiceBusiness, ICommonBusiness commonBusiness, IEnquiryBusiness enquiryBusiness,
            ISaleOrderBusiness saleOrderBusiness,IProductionOrderBusiness productionOrderBusiness,IQuotationBusiness quotationBusiness) {
            _salesInvoiceBusiness = salesInvoiceBusiness;
            _commonBusiness = commonBusiness;
            _enquiryBusiness = enquiryBusiness;
            _quotationBusiness = quotationBusiness;
            _saleOrderBusiness = saleOrderBusiness;
            _productionOrderBusiness = productionOrderBusiness;
        }


        // GET: DashBoard
        [AuthSecurityFilter(ProjectObject = "UserDashboard", Mode = "R")]
        public ActionResult Index(bool isUser = false)
        {
            Permission _permission = Session["UserRights"] as Permission;
            if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "AdminView").AccessCode : string.Empty).Contains("R"))
            {//AdminView subObject given to check if user has admin access.
                if (isUser)
                {
                    return View();
                }
                else
                {
                    return View("AdminDashboard");
                }
            }
            else
            {
                return View();
            }
        }

        [AuthSecurityFilter(ProjectObject = "UserDashboard", Mode = "R")]
        public ActionResult RecentDocument()
        {
            RecentDocumentViewModel recentDocument = new RecentDocumentViewModel();
            AppUA appUA = Session["AppUA"] as AppUA;
            recentDocument.RecentDocumentList = Mapper.Map<List<RecentDocument>, List<RecentDocumentViewModel>>(_commonBusiness.GetRecentDocument(appUA.UserName));
            return PartialView("_RecentDocument", recentDocument);
        }

        [AuthSecurityFilter(ProjectObject = "UserDashboard", Mode = "R")]
        public ActionResult SearchDocument(string searchTerm = null)
        {
            RecentDocumentViewModel recentDocument = new RecentDocumentViewModel();
            if (searchTerm != null)
            {
                recentDocument.RecentDocumentList = Mapper.Map<List<RecentDocument>, List<RecentDocumentViewModel>>(_commonBusiness.GetRecentDocument(null,searchTerm));
            }
            return PartialView("_SearchDocument", recentDocument);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult AdminDashboard()
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

        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult EnquiryValueVsFollowupCountSummary()
        {

            EnquiryFollowupSummaryList enqFollowupSummary = new EnquiryFollowupSummaryList();
            enqFollowupSummary.EnquiryFollowupSummaryVMList = Mapper.Map<List<EnquiryValueFolloupSummary>, List<EnquiryValueFolloupSummaryViewModel>>(_enquiryBusiness.GetEnquiryValueVsFollowupCountSummary());
            return PartialView("_EnquiryFollowupSummary", enqFollowupSummary);
        }



        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult DocumentSummary()
        {
            EnquirySummaryViewModel enquirySummary = Mapper.Map<EnquirySummary, EnquirySummaryViewModel>(_enquiryBusiness.GetEnquirySummaryCount());
            QuotationSummaryViewModel quotationSummary = Mapper.Map<QuotationSummary, QuotationSummaryViewModel>(_quotationBusiness.GetQuotationSummaryCount());
            SaleOrderSummaryViewModel saleOrderSummary = Mapper.Map<SaleOrderSummary, SaleOrderSummaryViewModel>(_saleOrderBusiness.GetSaleOrderSummaryCount());
            ProductionOrderSummaryViewModel productionOrderSummary = Mapper.Map<ProductionOrderSummary, ProductionOrderSummaryViewModel>(_productionOrderBusiness.GetProductionOrderSummaryCount());
            ViewBag.EnquiryTotal = enquirySummary.TotalEnquiryCount;
            ViewBag.ConvertedEnquiry = enquirySummary.ConvertedEnquiryCount;
            ViewBag.QuotationTotal = quotationSummary.TotalQuotationCount;
            ViewBag.ConvertedQuotation = quotationSummary.ConvertedQuotationCount;
            ViewBag.LostQuotation = quotationSummary.LostQuotationCount;
            ViewBag.ProductionOrderTotal = productionOrderSummary.TotalProductionOrderCount;
            ViewBag.OpenProductionOrder = productionOrderSummary.OpenProductionOrderCount;
            ViewBag.ClosedProductionOrder = productionOrderSummary.ClosedProductionOrderCount;
            ViewBag.InProgressProductionOrder = productionOrderSummary.InProgressProductionOrderCount;
            ViewBag.SaleOrderTotal = saleOrderSummary.TotalSaleOrderCount;
            ViewBag.OpenSaleOrder = saleOrderSummary.OpenSaleOrderCount;
            return PartialView("_DocumentSummary");
        }

    }
}