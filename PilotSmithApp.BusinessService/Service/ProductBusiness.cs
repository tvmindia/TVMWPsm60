﻿using PilotSmithApp.BusinessService.Contract;
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
            List<SelectListItem> selectListItem = null;
            List<Product> productList = _productRepository.GetProductForSelectList();
            return selectListItem = productList!=null?(from product in productList select new SelectListItem
                              {
                                  Text = product.Code + " - " + product.Name,
                                  Value = product.ID.ToString(),
                                  Selected = false
                              }).ToList():new List<SelectListItem>();       
        }
        public string GetProductCode()
        {
            return _productRepository.GetProductCode();
        }
    }
}
