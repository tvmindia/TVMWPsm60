using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IEnquiryBusiness
    {
        List<Enquiry> GetAllEnquiry(EnquiryAdvanceSearch enquiryAdvanceSearch);
        List<EnquiryDetail> GetEnquiryDetailListByEnquiryID(Guid enquiryID);
        object InsertUpdateEnquiry(Enquiry enquiry);

    }
}
