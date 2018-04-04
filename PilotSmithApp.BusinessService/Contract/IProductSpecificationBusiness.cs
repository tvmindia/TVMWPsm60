using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IProductSpecificationBusiness
    {
        object InsertUpdateProductSpecification(ProductSpecification productSpecification);
        List<ProductSpecification> GetAllProductSpecification(ProductSpecificationAdvanceSearch productSpecificationAdvanceSearch);
        ProductSpecification GetProductSpecification(int code);
        bool CheckProductSpecificationCodeExist(int code);
        object DeleteProductSpecification(int code);
        List<ProductSpecification> GetProductSpecificationForSelectList();
    }
}
