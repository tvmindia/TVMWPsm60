using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Service
{
    public class EnquiryBusiness:IEnquiryBusiness
    {
        IEnquiryRepository _enquiryRepository;
        public EnquiryBusiness(IEnquiryRepository enquiryRepository)
        {
            _enquiryRepository = enquiryRepository;
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
            return _enquiryRepository.InsertUpdateEnquiry(enquiry);
        }
    }
}
