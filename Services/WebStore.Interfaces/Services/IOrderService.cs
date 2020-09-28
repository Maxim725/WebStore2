using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetUserOrders(string userName);

        Task<OrderDTO> GetOrderById(int id);

        Task<OrderDTO> CreateOrder(string userName, CreateOrderModel model);
    }
}
