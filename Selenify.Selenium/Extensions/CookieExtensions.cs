using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Selenium.Extensions
{
    public static class CookieExtensions
    {
        public static System.Net.Cookie ToSystemCookie(this OpenQA.Selenium.Cookie cookie)
        {
            System.Net.Cookie netCookie = new System.Net.Cookie()
            {
                Domain = cookie.Domain,
                HttpOnly = cookie.IsHttpOnly,
                Name = cookie.Name,
                Path = cookie.Path,
                Secure = cookie.Secure,
                Value = cookie.Value
            };
            if (cookie.Expiry.HasValue)
            {
                netCookie.Expires = cookie.Expiry.Value;
            }

            return netCookie;
        }
    }
}
