using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SauceDemo.Factories;
using System.Collections.ObjectModel;
using System.Net;
using System.Security.Policy;
using OpenQA.Selenium.DevTools.V116.Emulation;

internal class Program {
    public static IWebDriver driver;
    private static void Main( string[] args ) {
        try {
            string driverPath = "C:\\Webdrivers\\msedgedriver.exe";
            AppSettingsSection secrets = loadSecretsConfig();
            string username = secrets.Settings["username"].Value;
            string profilePath = "C:\\Users\\" + username + "\\AppData\\Local\\Microsoft\\Edge\\Test User Data";
            driver = WebDriverFactory.GetEdgeDriver( profilePath, driverPath );

            string siteUsername = secrets.Settings["site_username"].Value;
            string sitePassword = secrets.Settings["site_password"].Value;
            LoginUser( siteUsername, sitePassword );
            
            loopThroughProducts();
        }
        catch ( Exception ex ) {
            driver.Quit();

            throw new Exception(ex.Message );
        }

        driver.Quit();

        Console.WriteLine( "Hello, World!" );
    }

    private static AppSettingsSection loadSecretsConfig() {
        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
            new ExeConfigurationFileMap {
                ExeConfigFilename = @"Config\Secrets.config"
            },
            ConfigurationUserLevel.None
        );

        return configuration.AppSettings;
    }

    public static void loopThroughProducts() {
        ReadOnlyCollection<IWebElement> inventoryItems = GetInventoryItems();

        for ( int i = 0; i < inventoryItems.Count; i++) {
            IWebElement inventoryItem = inventoryItems[i];

            if (elementIsStale( inventoryItem ) ) {
                inventoryItems = GetInventoryItems();
                inventoryItem = inventoryItems[i];
            }

            IWebElement inventoryItemLink = inventoryItem
                .FindElement( By.ClassName( "inventory_item_label" ) )
                .FindElement( By.TagName( "a" ) );
            inventoryItemLink.Click();

            SaveInventoryItemImage();

            driver.Navigate().Back();
        }
    }

    public static ReadOnlyCollection<IWebElement> GetInventoryItems() {
        return driver
            .FindElement( By.Id( "inventory_container" ) )
            .FindElements( By.CssSelector( ".inventory_item" ) );
    }

    public static void SaveInventoryItemImage() {
        IWebElement inventoryImage = driver
            .FindElement( By.Id( "inventory_item_container" ) )
            .FindElement( By.ClassName( "inventory_details_img" ) );

        string imageSource = inventoryImage.GetAttribute( "src" );
        string imageName = Path.GetFileName(imageSource );

        CreateDownloadsFolder();

        string savePath = GetProjectDirectoryPath() + "\\Downloads";
        DownloadFileToDirectory( imageSource, savePath, imageName );
    }

    public static string GetProjectDirectoryPath() {
        return Directory.GetParent(
            Directory.GetCurrentDirectory()!
        )!.Parent!.Parent!.FullName;
    }

    public static async void DownloadFileToDirectory(
        string imageSource,
        string path,
        string fileName
    ) {
        using (var client = new HttpClient()) {
            var response = await client.GetAsync( imageSource );

            using (var memoryStream = await response.Content.ReadAsStreamAsync()) {
                string savePath = Path.Combine(path, fileName);
                using (var fileStream = File.Create( savePath )) {
                    memoryStream.CopyTo( fileStream );
                }
            }
        }
    }

    public static void CreateDownloadsFolder() {
        string path = GetProjectDirectoryPath() + "\\Downloads";

        if (!Directory.Exists( path )) {
            Directory.CreateDirectory( path );
        }
    }

    public static void LoginUser(string username, string password ) {
        Uri url = new Uri( "https://www.saucedemo.com/" );

        driver!.Navigate().GoToUrl( url );

        WebDriverWait loginWait = new WebDriverWait( driver, TimeSpan.FromSeconds( 5 ) );

        loginWait.Until( ExpectedConditions.ElementIsVisible( By.Id( "user-name" ) ) );

        IWebElement usernameField = driver.FindElement( By.Id( "user-name" ) );
        usernameField.SendKeys( username );

        IWebElement passwordField = driver.FindElement( By.Id( "password" ) );
        passwordField.SendKeys( password );

        IWebElement loginButton = driver.FindElement( By.Id( "login-button" ) );
        loginButton.Click();

        WebDriverWait inventoryHeaderWait = new WebDriverWait( driver, TimeSpan.FromSeconds( 2 ) );
        inventoryHeaderWait.Until(
            ExpectedConditions.ElementExists(
                By.XPath( "//*[@id='header_container']/div[1]/div[2]/div" )
            )
        );
    }
    public static bool elementIsStale( IWebElement element ) {
        try {
            bool _ = element.Displayed;
            return false;
        }
        catch ( StaleElementReferenceException) {
            return true;
        }
    }

}