using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemo.Interfaces;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SauceDemo.SeleniumUtilities.WebDriverManager;

namespace SauceDemo.Processes
{

    public class DownloadSauceImages : IProcessBase
    {
        public string ProcessName { get; set; } = "Download Sauce Images";
        private class ProcessState {
        };
        private ProcessState state { get; set; } = new ProcessState();
        public void Run()
        {
            Uri url = new Uri( "https://www.saucedemo.com/" );

            Driver.Navigate().GoToUrl( url );

            LoginUser();

            Driver.Quit();
        }

        private void LoginUser()
        {
            AppSettingsSection config = loadSecretsConfig();
            string siteUsername = config.Settings["site_username"].Value;
            string sitePassword = config.Settings["site_password"].Value;

            WebDriverWait loginWait = new WebDriverWait( Driver, TimeSpan.FromSeconds( 5 ) );
            loginWait.Until( ExpectedConditions.ElementIsVisible( By.Id( "user-name" ) ) );

            IWebElement usernameField = Driver.FindElement( By.Id( "user-name" ) );
            usernameField.SendKeys( siteUsername );

            IWebElement passwordField = Driver.FindElement(By.Id( "password" ) );
            passwordField.SendKeys( sitePassword );

            IWebElement loginButton = Driver.FindElement( By.Id( "login-button" ) );
            loginButton.Click();

            WebDriverWait inventoryHeaderWait = new WebDriverWait( Driver, TimeSpan.FromSeconds( 5 ) );
            inventoryHeaderWait.Until(
                ExpectedConditions.ElementExists(
                    By.XPath( "//*[@id='header_container']/div[1]/div[2]/div" )
                )
            );
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
    }
}
