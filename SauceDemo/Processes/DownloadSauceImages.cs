using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V116.Target;
using OpenQA.Selenium.Support.UI;
using SauceDemo.Interfaces;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            LoopThroughProducts();

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

        private void LoopThroughProducts()
        {
            ReadOnlyCollection<IWebElement> inventoryItems = GetInventoryItems();
            for (int i = 0; i <inventoryItems.Count; i++)
            {
                IWebElement inventoryItem = inventoryItems[i];

                if (ElementIsStale( inventoryItem ))
                {
                    inventoryItems = GetInventoryItems();
                    inventoryItem = inventoryItems[i];
                }

                IWebElement inventoryItemLink = inventoryItem
                    .FindElement( By.ClassName( "inventory_item_label" ) )
                    .FindElement( By.TagName( "a" ) );
                inventoryItemLink.Click();

                SaveInventoryItemImage();

                Driver.Navigate().Back();
            }
        }

        private void SaveInventoryItemImage()
        {
            IWebElement inventoryImage = Driver
                .FindElement( By.Id( "inventory_item_container" ) )
                .FindElement( By.ClassName( "inventory_details_img" ) );

            string imageSource = inventoryImage.GetAttribute( "src" );
            string imageName = Path.GetFileName( imageSource );

            CreateDownloadsFolder();

            string savePath = GetProjectDirectoryPath() + "\\Downloads";
            DownloadFileToDirectory( imageSource, savePath, imageName );
        }

        private void CreateDownloadsFolder()
        {
            string path = GetProjectDirectoryPath() + "\\Downloads";

            if(!Directory.Exists( path ))
            {
                Directory.CreateDirectory( path );
            }
        }

        private string GetProjectDirectoryPath()
        {
            return Directory.GetParent(
                Directory.GetCurrentDirectory()!
            )!.Parent!.Parent!.FullName;
        }

        private async void DownloadFileToDirectory(string fileUrl,  string path, string fileName )
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync( fileUrl );

                using (Stream memoryStream = await response.Content.ReadAsStreamAsync())
                {
                    string savePath = Path.Combine( path, fileName );
                    using (FileStream fileStream  = File.Create(savePath))
                    {
                        memoryStream.CopyTo( fileStream );
                    }
                }
            }
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

        private ReadOnlyCollection<IWebElement> GetInventoryItems()
        {
            return Driver
                .FindElement( By.Id( "inventory_container" ) )
                .FindElements( By.CssSelector( ".inventory_item" ) );
        }

        private bool ElementIsStale(IWebElement element )
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
