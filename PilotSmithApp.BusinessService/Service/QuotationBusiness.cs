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
    public class QuotationBusiness:IQuotationBusiness
    {
        IQuotationRepository _quotationRepository;
        ICommonBusiness _commonBusiness;
        public QuotationBusiness(IQuotationRepository quotationRepository, ICommonBusiness commonBusiness)
        {
            _quotationRepository = quotationRepository;
            _commonBusiness = commonBusiness;
        }
        public List<Quotation> GetAllQuotation(QuotationAdvanceSearch quotationAdvanceSearch)
        {
            return _quotationRepository.GetAllQuotation(quotationAdvanceSearch);
        }
        public List<QuotationDetail> GetQuotationDetailListByQuotationID(Guid quoteID)
        {
            return _quotationRepository.GetQuotationDetailListByQuotationID(quoteID);
        }
        public object InsertUpdateQuotation(Quotation quotation)
        {
            if (quotation.QuotationDetailList.Count > 0)
            {
                quotation.DetailXML = _commonBusiness.GetXMLfromQuotationObject(quotation.QuotationDetailList, "ProductID");
            }
            return _quotationRepository.InsertUpdateQuotation(quotation);
        }
        public Quotation GetQuotation(Guid id)
        {
            return _quotationRepository.GetQuotation(id);
        }
        public object DeleteQuotation(Guid id)
        {
            return _quotationRepository.DeleteQuotation(id);
        }
        public object DeleteQuotationDetail(Guid id)
        {
            return _quotationRepository.DeleteQuotationDetail(id);
        }
    }
}
