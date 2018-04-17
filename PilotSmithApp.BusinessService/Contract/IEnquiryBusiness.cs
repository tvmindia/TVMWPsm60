using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IEnquiryBusiness
    {
        List<Enquiry> GetAllEnquiry(EnquiryAdvanceSearch enquiryAdvanceSearch);
        Enquiry GetEnquiry(Guid id);
        List<EnquiryDetail> GetEnquiryDetailListByEnquiryID(Guid enquiryID);
        object InsertUpdateEnquiry(Enquiry enquiry);
        object DeleteEnquiry(Guid id);
        object DeleteEnquiryDetail(Guid id);
        List<SelectListItem> GetEnquiryForSelectList();
    }
}
