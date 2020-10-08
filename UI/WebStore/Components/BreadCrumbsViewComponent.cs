using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    { 
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbViewModel();

            if (int.TryParse(Request.Query["sectionId"], out var sectionId))
                model.Section = _productData.GetSectionById(sectionId).FromDTO();

            if (int.TryParse(Request.Query["brandId"], out var brandId))
                model.Brand = _productData.GetBrandById(brandId).FromDTO();

            if (int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var productId))
            {
                var product = _productData.GetProductById(productId);

                if (product != null)
                    model.Product = product.Name;
            }
            return View(model);
        }
    }
}
