using System.Security.Claims;

namespace NetBazaar.Web.EndPoint.Utilities
{
    public static class BasketHelper
    {
        public const string BasketBuyerIdCookieName = "BasketBuyerId";

        public static string GetOrCreateBuyerId(HttpContext httpContext, ClaimsPrincipal user)
        {
            // اگر کاربر لاگین کرده باشد
            if (user?.Identity?.IsAuthenticated == true)
            {
                return user.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            // اگر کاربر لاگین نکرده باشد
            var buyerId = httpContext.Request.Cookies[BasketBuyerIdCookieName];

            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                };
                httpContext.Response.Cookies.Append(BasketBuyerIdCookieName, buyerId, cookieOptions);
            }

            return buyerId;
        }

        public static void DeleteBuyerIdCookie(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete(BasketBuyerIdCookieName);
        }
    }
}