﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(string brandId)
        {
            return View(new SelectableBrandsViewModel
            {
                Brands = GetBrands(),
                CurrentBrandId = int.TryParse(brandId, out var id) ? id : (int?) null
            }) ;
        }

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands()
               .Select(brand => new BrandViewModel
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Order = brand.Order,
                    ProductsCount = brand.ProductsCount 
                })
               .OrderBy(brand => brand.Order);
    }
}
