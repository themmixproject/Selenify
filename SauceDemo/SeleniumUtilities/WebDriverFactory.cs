using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace SauceDemo.SeleniumUtilities;

public static class WebDriverFactory
{
    public static IWebDriver GetEdgeDriver(string profilePath, string driverPath)
    {
        EdgeOptions options = new EdgeOptions
        {
            BinaryLocation = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe"
        };
        options.AddArgument("--no-sandbox"); // If running in a restricted environment
        options.AddArgument("--disable-dev-shm-usage"); // If running in a restricted environment
        options.AddArgument("--disable-gpu"); // If you encounter GPU-related issues
        options.AddArgument("--disable-features=msUndersideButton");
        options.AddArgument("--disable-features=msShowSignInIndicator");
        options.AddArgument("--log-level=3");
        options.AddArgument("user-data-dir=" + profilePath);

        EdgeDriver _driver = new EdgeDriver(driverPath, options);

        return _driver;
    }
}
