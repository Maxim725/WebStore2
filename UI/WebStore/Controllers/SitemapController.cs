using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index([FromServices]IProductData productData)
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("Blogs", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Shop", "Home")),
                new SitemapNode(Url.Action("Index", "WebAPITest")),
            };

            nodes.AddRange(productData
                .GetSections()
                .Select(s => new SitemapNode(Url.Action("Shop", "Catalog", new { SectionId = s.Id }))));

            foreach(var brand in productData.GetBrands())
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new { BrandId = brand.Id })));

            foreach (var products in productData.GetProducts().Products)
                nodes.Add(new SitemapNode(Url.Action("Details", "Catalog", new { products.Id })));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
