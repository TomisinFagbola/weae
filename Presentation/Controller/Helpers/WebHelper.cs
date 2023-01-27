
using Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace API.Helpers
{
    public class WebHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static ClaimsPrincipal CurrentUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
                {
                    return _httpContextAccessor.HttpContext.User;
                }

                return _httpContextAccessor.HttpContext.User;
            }
        }

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }

        public static Guid UserId
        {
            get
            {
                Guid id;

                var userId = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypeHelper.UserId).FirstOrDefault()?.Value ?? "";

                Guid.TryParse(userId, out id);

                return id;
            }
        }
        public static string FirstName =>
            _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.FirstName).FirstOrDefault()?.Value ?? "";
        public static string LastName =>
           _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.LastName).FirstOrDefault()?.Value ?? "";
        public static string Email =>
           _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.Email).FirstOrDefault()?.Value ?? "";

        public static string Country =>
            _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypeHelper.Country).FirstOrDefault()?.Value ?? "";
        public Guid GetUserId()
        {
            return UserId;
        }
    }
}
