using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<SectionDTO> GetSections() => _db.Sections.ToDTO();

        public SectionDTO GetSectionById(int id) => _db.Sections.Find(id).ToDTO();
        
        public IEnumerable<BrandDTO> GetBrands() => _db.Brands.Include(b => b.Products).ToDTO();

        public BrandDTO GetBrandById(int id) => _db.Brands
            .Include(b => b.Products)
            .FirstOrDefault()
            .ToDTO();

        public PageProductsDTO GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products
               .Include(product => product.Brand)
               .Include(product => product.Section);

            if (filter?.Ids?.Length > 0)
                query = query.Where(product => filter.Ids.Contains(product.Id));
            else
            {
                if (filter?.BrandId != null)
                    query = query.Where(product => product.BrandId == filter.BrandId);

                if (filter?.SectionId != null)
                    query = query.Where(product => product.SectionId == filter.SectionId);
            }

            var totalCount = query.Count();

            if (filter?.PageSize > 0)
                query = query
                    .Skip((filter.Page - 1) * (int)filter.PageSize)
                    .Take((int)filter.PageSize);

            return new PageProductsDTO
            {
                Products = query.ToDTO(),/*.ToArray()*/
                TotalCount = totalCount
            };
        }

        public ProductDTO GetProductById(int id) => _db.Products
           .Include(product => product.Brand)
           .Include(product => product.Section)
           .FirstOrDefault(product => product.Id == id)
            .ToDTO()    ;
    }
}
