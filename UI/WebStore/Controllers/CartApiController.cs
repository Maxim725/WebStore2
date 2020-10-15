using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    //[Route("api/cart")] // http://localhost:5000/api/cart
    //[ApiController]
    //public class CartApiController : ControllerBase
    //{
    //    private readonly ICartService _cartService;
    //    public CartApiController(ICartService cartService)
    //    {
    //        _cartService = cartService;
    //    }

    //    [HttpGet("add/{id}")]
    //    [HttpGet("imcrement/{id}")]
    //    public IActionResult AddToCart(int id)
    //    {
    //        _cartService.AddToCart(id);
    //        // Если наследуемся от Controller (то есть не API)
    //        //return Json(new {  id, message = $"Товар с id:{id} был добавлен в корзину"}) 

    //        // Если API, то
    //        return new JsonResult(new { id, message = $"Товар с id:{id} был добавлен в корзину" });
    //    }

    //    [HttpGet("decrement/{id}")]
    //    public IActionResult DecrementFromCart(int id)
    //    {
    //        _cartService.DecrementFromCart(id);
    //        return Ok();
    //    }

    //    [HttpGet("remove/{id]")]
    //    public IActionResult RemoveFromCart(int id)
    //    {
    //        _cartService.RemoveFromCart(id);
    //        return Ok();
    //    }

    //    [HttpGet("clear")]
    //    public IActionResult Clear()
    //    {
    //        _cartService.Clear();
    //        return Ok();
    //    }
    //}
}
