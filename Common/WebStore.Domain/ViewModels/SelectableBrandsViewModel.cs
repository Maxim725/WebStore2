using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.ViewModels
{
    public class SelectableBrandsViewModel
    {
        public IEnumerable<BrandViewModel> Brands { get; set; }

        public int? CurrentBrandId { get; set; }
    }
}
