
using System.Linq;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _configuration;

        public CatalogController(IProductData ProductData, IConfiguration configuration)
        {
            _ProductData = ProductData;
            _configuration = configuration;
        }

        public IActionResult Shop(int? brandId, int? sectionId, int page = 1)
        {
            var pageSize = int.TryParse(_configuration["PageSize"], out var size)
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
    }
}
