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
    }
}
