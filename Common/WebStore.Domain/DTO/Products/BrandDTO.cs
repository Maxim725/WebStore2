﻿using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Products
{
    public class BrandDTO : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Order { get; set; }
    }
}