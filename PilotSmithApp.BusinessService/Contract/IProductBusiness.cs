using PilotSmithApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Contract
{
    public interface IProductBusiness
    {
        object InsertUpdateProduct(Product product);
        List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch);
        Product GetProduct(Guid id);
        bool CheckProductCodeExist(Product product);
        object DeleteProduct(Guid id);
        List<SelectListItem> GetProductForSelectList();
        string GetProductCode();
    }
}
