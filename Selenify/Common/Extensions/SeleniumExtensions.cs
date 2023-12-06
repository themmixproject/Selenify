using Microsoft.VisualBasic;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Extensions
{
    public static class SeleniumExtensions
    {
        public static bool ElementIsStale(IWebElement element)
        {
            try
            {
                bool _ = element.Displayed;
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
        }

        public static CookieContainer GetCookiesFromPredicate(IWebDriver driver, Func<OpenQA.Selenium.Cookie, bool> predicate)
        {
            CookieContainer cookieContainer = new CookieContainer();

            foreach (var cookies in driver.Manage().Cookies.AllCookies)
            {
                if (predicate(cookies))
                {
                    System.Net.Cookie netCookie = CookieExtensions.ToSystemCookie(cookies);
                    cookieContainer.Add(netCookie);
                }
            }

            return cookieContainer;
        }

        public static CookieContainer GetAllCookiesFromWebDriver (IWebDriver driver)
        {
            return GetCookiesFromPredicate(driver, cookie => true);
        }

        public static CookieContainer GetCookiesFromDomain(IWebDriver driver, string domain)
        {
            return GetCookiesFromPredicate(driver, cookie => cookie.Domain.Equals(domain));
        }

        public static CookieContainer GetCookiesFromCurrentDomain(IWebDriver driver)
        {
            string currentDomain = new Uri(driver.Url).Host;
            return GetCookiesFromPredicate(driver, cookie => cookie.Domain.Equals(currentDomain));
            }

        public static class CookieExtensions
        {
            public static System.Net.Cookie ToSystemCookie(OpenQA.Selenium.Cookie cookie)
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
}
