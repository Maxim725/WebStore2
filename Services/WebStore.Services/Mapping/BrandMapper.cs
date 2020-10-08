using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.Services.Mapping
{
    public static class BrandMapper
    {
        public static BrandDTO ToDTO(this Brand brand) => brand is null ? null : new BrandDTO
        {
            Id = brand.Id,
            Name = brand.Name,
            Order = brand.Order,
            ProductsCount = brand.Products.Count()
        };

        public static Brand FromDTO(this BrandDTO brand) => brand is null ? null : new Brand
        {
            Id = brand.Id,
            Name = brand.Name,
            Order = brand.Order
        };

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> sections) => sections.Select(ToDTO);
        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> sections) => sections.Select(FromDTO);
    }
}
