using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Utility
{
    public static class WebDriverManager {
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

        public static void LoadEdgeDriver( EdgeOptions? options = null ) {
            if (options == null) {
                options = GetDefaultEdgeOptions();
            }

            string driverPath = System
                .Configuration
                .ConfigurationManager
                .AppSettings["edgeDriverPath"]!;
            _driver = new EdgeDriver( driverPath, options );
        }

        private static EdgeOptions GetDefaultEdgeOptions()
        {
            EdgeOptions options = new EdgeOptions {
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
            
            AppSettingsSection secrets = Utility.ConfigurationManager.Secrets;
            string profilePath = secrets.Settings["edge_profile_path"].Value;
            options.AddArgument("user-data-dir=" + profilePath);

            return options;
        }

        public static void QuitDriver()
        {
            if(_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}
