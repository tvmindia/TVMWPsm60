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
    public class ProductSpecificationBusiness:IProductSpecificationBusiness
    {
        private IProductSpecificationRepository _productSpecificationRepository;
        public ProductSpecificationBusiness(IProductSpecificationRepository productSpecificationRepository)
        {
            _productSpecificationRepository = productSpecificationRepository;
        }
        public object InsertUpdateProductSpecification(ProductSpecification productSpecification)
        {
            return _productSpecificationRepository.InsertUpdateProductSpecification(productSpecification);
        }
        public List<ProductSpecification> GetAllProductSpecification(ProductSpecificationAdvanceSearch productSpecificationAdvanceSearch)
        {
            return _productSpecificationRepository.GetAllProductSpecification(productSpecificationAdvanceSearch);
        }
        public ProductSpecification GetProductSpecification(int code)
        {
            return _productSpecificationRepository.GetProductSpecification(code);
        }
        public bool CheckProductSpecificationCodeExist(int code)
        {
            return _productSpecificationRepository.CheckProductSpecificationCodeExist(code);
        }
        public object DeleteProductSpecification(int code)
        {
            return _productSpecificationRepository.DeleteProductSpecification(code);
        }
        public List<ProductSpecification> GetProductSpecificationForSelectList()
        {
            return _productSpecificationRepository.GetProductSpecificationForSelectList();
        }
    }
}
