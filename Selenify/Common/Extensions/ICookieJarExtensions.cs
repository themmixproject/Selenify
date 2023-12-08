using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Selenify.Common.Extensions
{
    public static class ICookieJarExtensions
    {
        public static CookieContainer ToCookieContainer(this ICookieJar cookieJar)
        {
            CookieContainer cookieContainer = new CookieContainer();
            foreach (OpenQA.Selenium.Cookie cookie in cookieJar.AllCookies)
            {
                System.Net.Cookie netCookie = cookie.ToSystemCookie();
                cookieContainer.Add(netCookie);
            }

            return cookieContainer;
        }

        public static ICookieJar GetCookiesFromPredicate(this ICookieJar cookieJar, Func<OpenQA.Selenium.Cookie, bool> predicate)
        {
            ICookieJar retrievedCookies = Activator.CreateInstance<ICookieJar>();

            foreach (var cookie in cookieJar.AllCookies)
            {
                if (predicate(cookie))
                {
                    retrievedCookies.AddCookie(cookie);
                }
            }

            return retrievedCookies;
        }

        public static ICookieJar AllFromDomain(this ICookieJar cookieJar, string domain)
        {
            return GetCookiesFromPredicate(cookieJar, cookie => cookie.Domain.Equals(domain));
        }
    }
}
