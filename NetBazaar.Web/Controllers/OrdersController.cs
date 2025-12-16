using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.Interfaces.Basket;
using NetBazaar.Application.Interfaces.Order;
using NetBazaar.Application.Interfaces.User;
using NetBazaar.Domain.Entities;
using NetBazaar.Domain.Enums;
using NetBazaar.EndPoint.Controllers;
using NetBazaar.Web.EndPoint.ViewModels.Order;
using System.Security.Claims;

[Authorize]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;
    private readonly IUserAddressService _userAddressService;

    public OrdersController(IOrderService orderService, IBasketService basketService, IUserAddressService userAddressService)
    {
        _orderService = orderService;
        _basketService = basketService;
        _userAddressService = userAddressService;
    }

    [HttpGet("/checkout")]
    public async Task<IActionResult> Checkout()
    {
        var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var basket = await _basketService.GetBasketForUserAsync(buyerId);
        var addresses = await _userAddressService.GetAddressesAsync(buyerId);

        var vm = new CheckoutViewModel
        {
            Basket = basket,
            Addresses = addresses
        };

        return View(vm);
    }

    // نمایش جزئیات یک سفارش
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var order = await _orderService.GetOrderByIdAsync(id, buyerId);

        if (order == null)
        {
            TempData[OperationErrorKey] = "سفارش یافت نشد.";
            return RedirectToAction("Index");
        }

        return View(order); // این View باید در مسیر Views/Orders/Details.cshtml باشد
    }
    // نمایش لیست سفارش‌های کاربر
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = await _orderService.GetOrdersForUserAsync(buyerId);

        return View(orders); // این View باید در مسیر Views/Orders/Index.cshtml باشد
    }
    [HttpPost("/checkout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (buyerId == null)
        {
            TempData[OperationErrorKey] = "شناسه ای برای خریدار پیدا نشد!";
            return View(vm);
        }

        int orderId;
        try
        {
            orderId = await _orderService.CreateOrderAsync(
            buyerId,
            vm.SelectedAddressId,
            vm.PaymentMethod
        );
        }
        catch (Exception ex)
        {

            throw;
        }
        

        if (vm.PaymentMethod == PaymentMethod.CashOnDelivery)
            return RedirectToAction("Index", "Orders");

        return RedirectToAction("Index", "Payment", new { orderId });
    }
}