using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
