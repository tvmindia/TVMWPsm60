using PilotSmithApp.BusinessService.Contract;
using PilotSmithApp.DataAccessObject.DTO;
using PilotSmithApp.RepositoryService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PilotSmithApp.BusinessService.Service
{
    public class ProductBusiness:IProductBusiness
    {
        private IProductRepository _productRepository;
        public ProductBusiness(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public object InsertUpdateProduct(Product product)
        {
            return _productRepository.InsertUpdateProduct(product);
        }
        public List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch)
        {
            return _productRepository.GetAllProduct(productAdvanceSearch);
        }
        public Product GetProduct(Guid id)
        {
            return _productRepository.GetProduct(id);
        }
        public bool CheckProductCodeExist(Product product)
        {
            return _productRepository.CheckProductCodeExist(product);
        }
        public object DeleteProduct(Guid id)
        {
            return _productRepository.DeleteProduct(id);
        }
        public List<SelectListItem> GetProductForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Product> productList = _productRepository.GetProductForSelectList();
            if (productList != null)
                foreach (Product product in productList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = product.Code+" | "+product.Name,
                        Value = product.ID.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;         
        }
    }
}
