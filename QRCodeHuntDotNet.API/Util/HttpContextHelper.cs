using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QRCodeHuntDotNet.API.Util
{
    public interface IHttpContextHelper
    {
        long GetCurrentUserId(HttpContext httpContext);
        string GetCurrentUserRole(HttpContext httpContext);
        Task SignInAsync(HttpContext httpContext, IEnumerable<Claim> claims);
        string GetSiteUrl(HttpContext httpContext);
    }

    public class HttpContextHelper : IHttpContextHelper
    {
        public long GetCurrentUserId(HttpContext httpContext)
        {
            return long.Parse(httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        }

        public string GetCurrentUserRole(HttpContext httpContext)
        {
            return httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
        }

        public async Task SignInAsync(HttpContext httpContext, IEnumerable<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        public string GetSiteUrl(HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}{Uri.SchemeDelimiter}{httpContext.Request.Host}";
        }
    }
}
