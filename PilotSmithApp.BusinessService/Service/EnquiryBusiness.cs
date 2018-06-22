using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class EnquiryBusiness:IEnquiryBusiness
    {
        IEnquiryRepository _enquiryRepository;
        ICommonBusiness _commonBusiness;
        public EnquiryBusiness(IEnquiryRepository enquiryRepository, ICommonBusiness commonBusiness)
        {
            _enquiryRepository = enquiryRepository;
            _commonBusiness = commonBusiness;
        }
        public List<Enquiry> GetAllEnquiry(EnquiryAdvanceSearch enquiryAdvanceSearch)
        {
            return _enquiryRepository.GetAllEnquiry(enquiryAdvanceSearch);
        }
        public List<EnquiryDetail> GetEnquiryDetailListByEnquiryID(Guid enquiryID)
        {
            return _enquiryRepository.GetEnquiryDetailListByEnquiryID(enquiryID);
        }
        public object InsertUpdateEnquiry(Enquiry enquiry)
        {
            if(enquiry.EnquiryDetailList.Count>0)
            {
                enquiry.DetailXML = _commonBusiness.GetXMLfromEnquiryObject(enquiry.EnquiryDetailList, "ProductID, Qty, UnitCode, Rate");
            }
            return _enquiryRepository.InsertUpdateEnquiry(enquiry);
        }
        public Enquiry GetEnquiry(Guid id)
        {
           return _enquiryRepository.GetEnquiry(id);
        }
        public object DeleteEnquiry(Guid id)
        {
            return _enquiryRepository.DeleteEnquiry(id);
        }
        public object DeleteEnquiryDetail(Guid id)
        {
            return _enquiryRepository.DeleteEnquiryDetail(id);
        }
        public List<SelectListItem> GetEnquiryForSelectList(Guid? id)
        {
            List<SelectListItem> selectListItem = null;
            List<Enquiry> enquiryList = _enquiryRepository.GetEnquiryForSelectList(id);
            return selectListItem = enquiryList!=null?(from enquiry in enquiryList
                                     select new SelectListItem
                                     {
                                         Text = enquiry.EnquiryNo + "-" + enquiry.Customer.CompanyName,
                                         Value = enquiry.ID.ToString(),
                                         Selected = false
                                     }).ToList():new List<SelectListItem>();
        }
        public List<Enquiry> GetEnquiryForSelectListOnDemand(string searchTerm)
        {
            return _enquiryRepository.GetEnquiryForSelectListOnDemand(searchTerm);
        }
        public List<EnquiryValueFolloupSummary> GetEnquiryValueVsFollowupCountSummary() {

            return _enquiryRepository.GetEnquiryValueVsFollowupCountSummary();
        }
        public EnquirySummary GetEnquirySummaryCount()
        {
            return _enquiryRepository.GetEnquirySummaryCount();
        }

    }
}
