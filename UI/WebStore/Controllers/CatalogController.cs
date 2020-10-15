
using System.Linq;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using System.Collections;
using System.Collections.Generic;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _configuration;
        private const string _pageSize = "PageSize";
        public CatalogController(IProductData ProductData, IConfiguration configuration)
        {
            _ProductData = ProductData;
            _configuration = configuration;
        }

        public IActionResult Shop(int? brandId, int? sectionId, int page = 1)
        {
            var pageSize = int.TryParse(_configuration[_pageSize], out var size)
                ? size
                : (int?)null;

            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = pageSize
            };

            var products = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = brandId,
                BrandId = sectionId,
                Products = products.Products.FromDTO().ToView().OrderBy(p => p.Order),
                Page = new PageViewModel
                {
                    PageSize = pageSize ?? 0,
                    PageNumber = page,
                    TotalItems = products.TotalCount
                }
            });
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
                return NotFound();

            return View(product.FromDTO().ToView());
        }


        #region API
        public IActionResult GetCatalogHTML(int? brandId, int? sectionId, int page = 1)
        {
            // Partial / _FeaturesItems
            return View("Partial/_FeaturesItems", GetProducts(brandId, sectionId, page));
        }
        private IEnumerable<ProductViewModel> GetProducts(int? brandId, int? sectionId, in int page)
        {
            return _ProductData.GetProducts(new ProductFilter
            {
                SectionId = sectionId,
                BrandId = brandId,
                Page = page,
                PageSize = int.Parse(_configuration[_pageSize])
            }).Products
              .FromDTO()
              .ToView()
              .OrderBy(p => p.Order);
        }
        #endregion
    }
}
