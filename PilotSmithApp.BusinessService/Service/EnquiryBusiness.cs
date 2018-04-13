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
                enquiry.DetailXML = _commonBusiness.GetXMLfromEnquiryObject(enquiry.EnquiryDetailList, "ProductID");
            }
            return _enquiryRepository.InsertUpdateEnquiry(enquiry);
        }
        public Enquiry GetEnquiry(Guid id)
        {
           return _enquiryRepository.GetEnquiry(id);
        }
    }
}
