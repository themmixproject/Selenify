using OpenQA.Selenium;

namespace Selenify.Selenium.Extensions
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
