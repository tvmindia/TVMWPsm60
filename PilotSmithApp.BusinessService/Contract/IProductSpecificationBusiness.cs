using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IProductSpecificationBusiness
    {
        object InsertUpdateProductSpecification(ProductSpecification productSpecification);
        List<ProductSpecification> GetAllProductSpecification(ProductSpecificationAdvanceSearch productSpecificationAdvanceSearch);
        ProductSpecification GetProductSpecification(int code);
        bool CheckProductSpecificationExist(ProductSpecification productSpecification);
        object DeleteProductSpecification(int code);
        List<SelectListItem> GetProductSpecificationForSelectList();
    }
}
