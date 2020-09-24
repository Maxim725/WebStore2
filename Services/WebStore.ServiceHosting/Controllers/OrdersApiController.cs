using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _orderServicee;

        public OrdersApiController(IOrderService orderService)
        {
            _orderServicee = orderService;
        }

        [HttpGet("user/{userName}")]
        public Task<IEnumerable<OrderDTO>> GetUserOrders(string userName)
        {
            return _orderServicee.GetUserOrders(userName);
        }

        [HttpGet("{id}")]
        public Task<OrderDTO> GetOrderById(int id)
        {
            return _orderServicee.GetOrderById(id);
        }

        [HttpPost("{userName}")]
        public Task<OrderDTO> CreateOrder(string userName, [FromBody] CreateOrderModel model)
        {
            return _orderServicee.CreateOrder(userName, model);
        }


    }
}
