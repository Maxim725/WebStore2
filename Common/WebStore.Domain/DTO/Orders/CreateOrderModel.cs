using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO.Orders
{
    public class CreateOrderModel
    {
        public OrderViewModel Order { get; set; }

        public List<OrderItemDTO> Items { get; set; }
    }
}
