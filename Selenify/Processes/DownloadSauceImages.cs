using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Security.Policy;
using System.Threading;
using static Selenify.Selenium.WebDriverManager;
using Selenify.Selenium.Extensions;
using Selenify.Common.Http;
using Selenify.Common.Utility;
using Selenify.Base.Models.Process;
using Console = Selenify.Common.Console;

namespace Selenify.Processes
{

    public class DownloadSauceImages : Process<DownloadSauceImages.ProcessState>
    {
        public class ProcessState
        {
            public bool isLoggedIn { get; set; }
            public int i { get; set; } = 0;
        };

        public DownloadSauceImages() : base("Download Sauce Images") { }

        public override void Run()
        {
            Uri url = new Uri("https://www.saucedemo.com/");
            Driver.Navigate().GoToUrl(url);

            LoginUser();

            State.isLoggedIn = true;
            SaveState();

            LoopThroughProducts();

            Driver.Quit();

            ResetState();
        }

        private void LoginUser()
        {
            AppSettingsSection secretsConfig = ConfigurationLoader.LoadSecretsConfig();
            string siteUsername = secretsConfig.Settings["site_username"].Value;
            string sitePassword = secretsConfig.Settings["site_password"].Value;

            WebDriverWait loginWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            loginWait.Until(ExpectedConditions.ElementIsVisible(By.Id("user-name")));

            IWebElement usernameField = Driver.FindElement(By.Id("user-name"));
            usernameField.SendKeys(siteUsername);

            IWebElement passwordField = Driver.FindElement(By.Id("password"));
            passwordField.SendKeys(sitePassword);

            IWebElement loginButton = Driver.FindElement(By.Id("login-button"));
            loginButton.Click();

            WebDriverWait inventoryHeaderWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            inventoryHeaderWait.Until(
                ExpectedConditions.ElementExists(
                    By.XPath("//*[@id='header_container']/div[1]/div[2]/div")
                )
            );
        }

        private void LoopThroughProducts()
        {
            ReadOnlyCollection<IWebElement> inventoryItems = GetInventoryItems();

            for (; State.i < inventoryItems.Count; State.i++)
            {
                IWebElement inventoryItem = inventoryItems[State.i];

                if (inventoryItem.IsStale())
                {
                    inventoryItems = GetInventoryItems();
                    inventoryItem = inventoryItems[State.i];
                }

                IWebElement inventoryItemLink = inventoryItem
                    .FindElement(By.ClassName("inventory_item_label"))
                    .FindElement(By.TagName("a"));
                inventoryItemLink.Click();

                SaveInventoryItemImageAsync();

                Driver.Navigate().Back();

                Thread.Sleep(500);

                SaveState();
            }
        }

        private async void SaveInventoryItemImageAsync()
        {
            IWebElement inventoryImage = Driver
                .FindElement(By.Id("inventory_item_container"))
                .FindElement(By.ClassName("inventory_details_img"));

            string imageSource = inventoryImage.GetAttribute("src");
            string imageName = Path.GetFileName(imageSource);

            string downloadsDirPath = GetProjectDirectoryPath() + "\\Downloads\\";
            Directory.CreateDirectory(downloadsDirPath);

            await HttpClientManager.Client.DownloadToTempFolder(imageSource, imageName);
        }

        private string GetProjectDirectoryPath()
        {
            return Directory.GetParent(
                Directory.GetCurrentDirectory()!
            )!.Parent!.Parent!.FullName;
        }

        private ReadOnlyCollection<IWebElement> GetInventoryItems()
        {
            return Driver
                .FindElement(By.Id("inventory_container"))
                .FindElements(By.CssSelector(".inventory_item"));
        }
    }
}
