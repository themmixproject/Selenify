using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.SeleniumUtilities
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
                    _driver = GetEdgeDriver();
                }

                return _driver;
            }
        }

        public static IWebDriver GetEdgeDriver()
        {
            string driverPath = "C:\\Webdrivers\\msedgedriver.exe";
            EdgeOptions options = GetEdgeOptions();

            return new EdgeDriver(driverPath, options);
        }

        public static void LoadEdgeDriver()
        {
            string driverPath = ConfigurationManager.AppSettings["edgeDriverPath"]!;

            EdgeOptions options = GetEdgeOptions();

            _driver = new EdgeDriver(driverPath, options);
        }

        private static EdgeOptions GetEdgeOptions()
        {
            AppSettingsSection secrets = loadSecretsConfig();
            string username = secrets.Settings["username"].Value;
            string profilePath = "C:\\Users\\" + username + "\\AppData\\Local\\Microsoft\\Edge\\Test User Data";

            EdgeOptions options = new EdgeOptions {
                BinaryLocation = ConfigurationManager.AppSettings["edgeBinaryPath"]
            };
            options.AddArgument("--no-sandbox"); // If running in a restricted environment
            options.AddArgument("--disable-dev-shm-usage"); // If running in a restricted environment
            options.AddArgument("--disable-gpu"); // If you encounter GPU-related issues
            options.AddArgument("--disable-features=msUndersideButton");
            options.AddArgument("--disable-features=msShowSignInIndicator");
            options.AddArgument("--log-level=3");
            options.AddArgument("user-data-dir=" + profilePath);

            return options;
        }

        private static AppSettingsSection loadSecretsConfig()
        {
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = @".\Config\Secrets.config"
                },
                ConfigurationUserLevel.None
            );

            return configuration.AppSettings;
        }

        public static void QuitDriver()
        {
            if(_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}
