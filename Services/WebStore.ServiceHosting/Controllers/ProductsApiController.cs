using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductData
    {
        private readonly IProductData products;

        public ProductsApiController(IProductData products)
        {
            this.products = products;
        }
        [HttpGet("brands")]

        public IEnumerable<BrandDTO> GetBrands()
        {
            return products.GetBrands();
        }

        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections()
        {
            return products.GetSections();
        }

        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id)
        {
            return products.GetProductById(id);
        }

        [HttpPost]
        public PageProductsDTO GetProducts([FromBody] ProductFilter Filter = null)
        {
            return products.GetProducts(Filter);
        }

        [HttpGet("sections/{id}")]
        public SectionDTO GetSectionById(int id)
        {
            return products.GetSectionById(id);
        }

        [HttpGet("brands/{id}")]
        public BrandDTO GetBrandById(int id)
        {
            return products.GetBrandById(id);
        }
    }
}
