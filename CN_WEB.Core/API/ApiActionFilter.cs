using CN_WEB.Core.API.Authentication;
using CN_WEB.Core.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CN_WEB.Core.API
{
    public class ApiActionFilter : IActionFilter
    {
        private readonly IAuthService _authService;
        private readonly IRedisCache _cache;

        public ApiActionFilter(IAuthService authService, IRedisCache cache)
        {
            _authService = authService;
            _cache = cache;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var cacheAttr = filterContext.GetAttributes<RedisCacheAttribute>();
            if (cacheAttr != null)
            {
                _cache.SetCacheAction(filterContext, cacheAttr);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.GetAttributes<AllowAnonymousAttribute>() == null)
            {
                //Authenticate(filterContext.HttpContext);
            }

            var cacheAttr = filterContext.GetAttributes<RedisCacheAttribute>();
            if (cacheAttr != null)
            {
                _cache.GetCacheAction(filterContext);
            }
        }

        private void Authenticate(HttpContext httpContext)
        {
            string idToken = httpContext.Request.Headers["id-token"];

            if (_authService.ValidateToken(idToken))
            {
                var userModel = _authService.GetUserFromIdToken(idToken);
                httpContext.User = new MyClaimsPrincipal(userModel);
            }
            else
            {
                ClearTokenId(httpContext);
                throw new UnauthorizedException();
            }
        }

        private void ClearTokenId(HttpContext httpContext)
        {
            var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddMonths(-1) };
            httpContext.Response.Cookies.Delete("id-token", cookieOptions);
        }
    }

    public static class ApiAction
    {
        public static TAttribute GetAttributes<TAttribute>(this ActionExecutingContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            var attr = descriptor.MethodInfo.GetCustomAttributes(typeof(TAttribute), false);
            if (attr.Length > 0)
            {
                return (TAttribute)attr[0];
            }
            else
            {
                return default;
            }
        }

        public static TAttribute GetAttributes<TAttribute>(this ActionExecutedContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            var attr = descriptor.MethodInfo.GetCustomAttributes(typeof(TAttribute), false);
            if (attr.Length > 0)
            {
                return (TAttribute)attr[0];
            } else
            {
                return default;
            }
        }
    }
}
