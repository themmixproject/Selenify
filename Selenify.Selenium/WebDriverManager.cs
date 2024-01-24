using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Selenify.Common.Console;
using Selenify.Common.Utility;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Selenify.Selenium
{
    public static class WebDriverManager
    {
        private static IWebDriver? _driver;
        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    LoadEdgeDriver();
                }

                return _driver!;
            }
        }

        public static void LoadEdgeDriver(EdgeOptions? options = null)
        {
            Common.Console.Console.UI.WriteLine("Loading Edge driver . . .");

            if (options == null)
            {
                options = GetDefaultEdgeOptions();
            }

            string driverPath = System
                .Configuration
                .ConfigurationManager
                .AppSettings["edgeDriverPath"]!;
            EdgeDriverService driverService = EdgeDriverService.CreateDefaultService(driverPath);
            driverService.HideCommandPromptWindow = true;

            _driver = new EdgeDriver(driverService, options);

            Common.Console.Console.UI.WriteLine("Edge driver loaded");
            Common.Console.Console.UI.Stop();
        }

        private static EdgeOptions GetDefaultEdgeOptions()
        {
            EdgeOptions options = new EdgeOptions
            {
                BinaryLocation = System
                .Configuration
                .ConfigurationManager
                .AppSettings["edgeBinaryPath"]
            };
            options.AddArgument("--no-sandbox"); // If running in a restricted environment
            options.AddArgument("--disable-dev-shm-usage"); // If running in a restricted environment
            options.AddArgument("--disable-gpu"); // If you encounter GPU-related issues
            options.AddArgument("--disable-features=msUndersideButton");
            options.AddArgument("--disable-features=msShowSignInIndicator");
            options.AddArgument("--log-level=3");

            AppSettingsSection secretsConfig = ConfigurationLoader.LoadSecretsConfig();

            string profilePath = secretsConfig.Settings["edge_profile_path"].Value;
            options.AddArgument("user-data-dir=" + profilePath);

            return options;
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}
