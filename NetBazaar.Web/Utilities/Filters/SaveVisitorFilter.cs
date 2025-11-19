using Azure.Core;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NetBazaar.Application.DTOs.Visitor;
using NetBazaar.Application.Interfaces.Visitor;
using NetBazaar.Domain.Entities.VisitorContext;
using UAParser;

namespace NetBazaar.Web.EndPoint.Utilities.Filters
{
    public class SaveVisitorFilter : IActionFilter
    {
        private readonly ISaveVisitorInfoService _saveVisitorInfoService;
        private readonly Parser _uaParser;

        public SaveVisitorFilter(ISaveVisitorInfoService saveVisitorInfoService)
        {
            _saveVisitorInfoService = saveVisitorInfoService;
            _uaParser = Parser.GetDefault();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Intentionally left empty - no post-execution logic needed
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var visitorInfo = CreateVisitorInfoDto(context);
                _saveVisitorInfoService.Execute(visitorInfo);
            }
            catch (Exception ex)
            {
                // Log the exception but don't break the request flow
                // Consider using ILogger here in a real scenario
                System.Diagnostics.Debug.WriteLine($"Failed to save visitor info: {ex.Message}");
            }
        }

        private RequestSaveVisitorInfoDTO CreateVisitorInfoDto(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            var clientInfo = _uaParser.Parse(userAgent);

            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            return new RequestSaveVisitorInfoDTO
            {
                IpAddress = GetIpAddress(httpContext),
                Browser = CreateBrowserInfo(clientInfo),
                VisitorId = GetOrCreateVisitorId(httpContext),

                Device = CreateDeviceInfo(clientInfo),
                OperatingSystem = CreateOsInfo(clientInfo),
                
                PhysicalPath = $"{actionDescriptor.ControllerName}/{actionDescriptor.ActionName}",
                CurrentUrl = request.Path,
                ReferrerUrl = httpContext.Request.Headers["Referer"].ToString(),
                HttpMethod = request.Method,
                Protocol = request.Protocol
            };
        }

        private string GetOrCreateVisitorId(HttpContext httpContext)
        {
            const string cookieName = "VisitorId";

            // Try to get existing visitor ID
            if (httpContext.Request.Cookies.TryGetValue(cookieName, out var existingVisitorId)
                && !string.IsNullOrEmpty(existingVisitorId))
            {
                return existingVisitorId;
            }

            // Create new visitor ID
            var newVisitorId = Guid.NewGuid().ToString();

            var cookieOptions = new CookieOptions
            {
                Path = "/",                          // The cookie applies to the entire site
                HttpOnly = true,                       // Not accessible via JavaScript (mitigates XSS)
                Secure = httpContext.Request.IsHttps, // Only sent over secure HTTPS connections
                SameSite = SameSiteMode.Lax,          // CSRF protection for most cross-site requests
                Expires = DateTimeOffset.UtcNow.AddDays(30), // Expires in 30 days
                IsEssential = true                     // Cookie is essential for the app's functionality
            };

            httpContext.Response.Cookies.Append(cookieName, newVisitorId, cookieOptions);
            return newVisitorId;
        }
        private static string GetIpAddress(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private static VisitorVersionDto CreateBrowserInfo(ClientInfo clientInfo)
        {
            return new VisitorVersionDto
            {
                Family = clientInfo.UA.Family,
                Version = FormatVersion(clientInfo.UA.Major, clientInfo.UA.Minor, clientInfo.UA.Patch)
            };
        }

        private static VisitorDeviceDto CreateDeviceInfo(ClientInfo clientInfo)
        {
            return new VisitorDeviceDto
            {
                Family = clientInfo.Device.Family,
                Brand = clientInfo.Device.Brand,
                Model = clientInfo.Device.Model,
                IsBot = clientInfo.Device.IsSpider
            };
        }

        private static VisitorVersionDto CreateOsInfo(ClientInfo clientInfo)
        {
            return new VisitorVersionDto
            {
                Family = clientInfo.OS.Family,
                Version = FormatVersion(clientInfo.OS.Major, clientInfo.OS.Minor, clientInfo.OS.Patch)
            };
        }

        private static string FormatVersion(string major, string minor, string patch)
        {
            return $"{major}.{minor}.{patch}";
        }
    }
}