using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.Interfaces.Payment;
using NetBazaar.EndPoint.Controllers;

public class PaymentController : BaseController
{
    private readonly IPaymentService _paymentService;
    private readonly IZarinPalService _zarinPal;
    private readonly IConfiguration _configuration;

    public PaymentController(IPaymentService paymentService, IZarinPalService zarinPal, IConfiguration configuration)
    {
        _paymentService = paymentService;
        _zarinPal = zarinPal;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int orderId)
    {
        var payment = await _paymentService.CreatePaymentAsync(orderId);

        var callbackUrl = Url.Action("Verify", "Payment",
            new { paymentId = payment.PaymentId }, protocol: Request.Scheme);

        var request = new ZarinPalPaymentRequest
        {
            MerchantId = _configuration["ZarinPal:MerchantId"]!,
            Amount = (int)payment.Amount,
            Description = payment.Description,
            Email = payment.UserEmail,
            Mobile = payment.UserMobile,
            CallbackUrl = callbackUrl!
        };

        try
        {
            var result = await _zarinPal.CreatePaymentRequestAsync(request);
            if (result.Status == 100)
                return Redirect($"https://zarinpal.com/pg/StartPay/{result.Authority}");
        }
        catch
        {
            TempData[OperationErrorKey] = "خطا در اتصال به درگاه پرداخت";

        }
        return RedirectToAction("Index", "Orders");
    }

    [HttpGet]
    public async Task<IActionResult> Verify(Guid paymentId, string authority, string status)
    {
        if (string.IsNullOrEmpty(status) || status.ToLower() != "ok")
            return RedirectToAction("Index", "Orders");

        var verified = await _paymentService.VerifyPaymentAsync(paymentId, authority);
        TempData[verified ? "Success" : "Error"] =
            verified ? "پرداخت با موفقیت انجام شد" : "تأیید پرداخت ناموفق بود";

        return RedirectToAction("Index", "Orders");
    }
}