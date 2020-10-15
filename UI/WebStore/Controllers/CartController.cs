using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService CartService) => _cartService = CartService;

        public IActionResult Details() => View(new CartOrderViewModel { Cart = _cartService.TransformFromCart() });

        public IActionResult AddToCart(int id)
        {
            _cartService.AddToCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Clear()
        {
            _cartService.Clear();
            return RedirectToAction(nameof(Details));
        }

        [Authorize]
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new CartOrderViewModel
                {
                    Cart = _cartService.TransformFromCart(),
                    Order = OrderModel
                });

            var orderModel = new CreateOrderModel
            {
                Order = OrderModel,
                Items = _cartService.TransformFromCart().Items
                            .Select(i => new OrderItemDTO
                            {
                                Id = i.Product.Id,
                                Price = i.Product.Price,
                                Quantity = i.Quantity
                            }).ToList()
            };
            var order = await OrderService.CreateOrder(User.Identity.Name, orderModel);

            _cartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region API
        public IActionResult AddToCartAPI(int id)
        {
            _cartService.AddToCart(id);
            // Если наследуемся от Controller (то есть не API)
            //return Json(new {  id, message = $"Товар с id:{id} был добавлен в корзину"}) 

            // Если API, то
            return new JsonResult(new { id, message = $"Товар с id:{id} был добавлен в корзину" });
        }

        public IActionResult DecrementFromCartAPI(int id)
        {
            _cartService.DecrementFromCart(id);
            return Ok();
        }

        public IActionResult RemoveFromCartAPI(int id)
        {
            _cartService.RemoveFromCart(id);
            return Ok();
        }

        public IActionResult ClearAPI()
        {
            _cartService.Clear();
            return Ok();
        }

        public IActionResult GetCartView() => ViewComponent("Cart");
        #endregion
    }
}
