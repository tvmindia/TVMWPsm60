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
    public class ProductionQCBusiness: IProductionQCBusiness
    {
        IProductionQCRepository _productionQCRepository;
        ICommonBusiness _commonBusiness;
        public ProductionQCBusiness(IProductionQCRepository productionQCRepository, ICommonBusiness commonBusiness)
        {
            _productionQCRepository = productionQCRepository;
            _commonBusiness = commonBusiness;
        }
        public List<ProductionQC> GetAllProductionQC(ProductionQCAdvanceSearch productionQCAdvanceSearch)
        {
            return _productionQCRepository.GetAllProductionQC(productionQCAdvanceSearch);
        }
        public List<ProductionQCDetail> GetProductionQCDetailListByProductionQCID(Guid productionQCID)
        {
            return _productionQCRepository.GetProductionQCDetailListByProductionQCID(productionQCID);
        }
        public object InsertUpdateProductionQC(ProductionQC productionQC)
        {
            if (productionQC.ProductionQCDetailList.Count > 0)
            {
                productionQC.DetailXML = _commonBusiness.GetXMLfromProductionQCObject(productionQC.ProductionQCDetailList, "ProductID,ProductModelID,QCBy,QCDateFormatted");
            }
            return _productionQCRepository.InsertUpdateProductionQC(productionQC);
        }
        public ProductionQC GetProductionQC(Guid id)
        {
            return _productionQCRepository.GetProductionQC(id);
        }
        public object DeleteProductionQC(Guid id)
        {
            return _productionQCRepository.DeleteProductionQC(id);
        }
        public object DeleteProductionQCDetail(Guid id)
        {
            return _productionQCRepository.DeleteProductionQCDetail(id);
        }
    }
}
