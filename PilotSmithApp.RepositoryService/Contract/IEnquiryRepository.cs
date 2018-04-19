using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.RepositoryService.Contract
{
    public interface IEnquiryRepository
    {
        List<Enquiry> GetAllEnquiry(EnquiryAdvanceSearch enquiryAdvanceSearch);
        List<EnquiryDetail> GetEnquiryDetailListByEnquiryID(Guid enquiryID);
        Enquiry GetEnquiry(Guid id);
        object InsertUpdateEnquiry(Enquiry enquiry);
        object DeleteEnquiry(Guid id);
        object DeleteEnquiryDetail(Guid id);
        List<Enquiry> GetEnquiryForSelectList();
        List<EnquiryValueFolloupSummary> GetEnquiryValueVsFollowupCountSummary();
    }
}
