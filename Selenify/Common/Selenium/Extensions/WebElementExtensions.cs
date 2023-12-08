using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Selenium.Extensions
{
    public static class WebElementExtensions
    {
        public static bool IsStale(this IWebElement element)
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
    }
}
