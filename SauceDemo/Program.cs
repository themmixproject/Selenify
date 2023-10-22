using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SauceDemo.Factories;
using System.Collections.ObjectModel;

internal class Program {
    public static IWebDriver driver;
    private static void Main( string[] args ) {
        string driverPath = "C:\\Webdrivers\\msedgedriver.exe";
        AppSettingsSection secrets = loadSecretsConfig();
        string username = secrets.Settings["username"].Value;
        string profilePath = "C:\\Users\\" + username +"\\AppData\\Local\\Microsoft\\Edge\\Test User Data";
        driver = WebDriverFactory.GetEdgeDriver( profilePath, driverPath );

        string siteUsername = secrets.Settings["site_username"].Value;
        string sitePassword = secrets.Settings["site_password"].Value;
        loginUser( siteUsername, sitePassword );

        loopThroughProducts();

        driver.Quit();

        Console.WriteLine( "Hello, World!" );
    }

    private static AppSettingsSection loadSecretsConfig() {
        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = @"Config\Secrets.config"
        };
        
        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
            fileMap,
            ConfigurationUserLevel.None
        );

        return configuration.AppSettings;
    }

    public static void loopThroughProducts() {
        ReadOnlyCollection<IWebElement> inventoryItems = getInventoryItems();

        for ( int i = 0; i < inventoryItems.Count; i++) {
            IWebElement inventoryItem = inventoryItems[i];

            if (elementIsStale( inventoryItem ) ) {
                inventoryItems = getInventoryItems();
                inventoryItem = inventoryItems[i];
            }

            IWebElement inventoryItemLink = inventoryItem
                .FindElement( By.ClassName( "inventory_item_label" ) )
                .FindElement( By.TagName( "a" ) );
            inventoryItemLink.Click();

            driver.Navigate().Back();
        }
    }

    public static ReadOnlyCollection<IWebElement> getInventoryItems() {
        return driver
            .FindElement( By.Id( "inventory_container" ) )
            .FindElements( By.CssSelector( ".inventory_item" ) );
    }

    public static void loginUser(string username, string password ) {
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