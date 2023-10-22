using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

internal class Program {
    public static IWebDriver? driver;
    private static void Main( string[] args ) {
        string binaryPath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
        string driverPath = "C:\\Webdrivers\\msedgedriver.exe";

        Configuration secretsConfig = loadSecretsConfig();

        string username = secretsConfig.AppSettings.Settings["username"].Value;
        string profilePath = "C:\\Users\\" + username +"\\AppData\\Local\\Microsoft\\Edge\\Test User Data";

        EdgeOptions options = new EdgeOptions {
            BinaryLocation = binaryPath
        };
        options.AddArgument( "--no-sandbox" ); // If running in a restricted environment
        options.AddArgument( "--disable-dev-shm-usage" ); // If running in a restricted environment
        options.AddArgument( "--disable-gpu" ); // If you encounter GPU-related issues
        options.AddArgument( "--disable-features=msUndersideButton" );
        options.AddArgument( "--disable-features=msShowSignInIndicator" );
        options.AddArgument( "--log-level=3" );
        options.AddArgument( "user-data-dir=" + profilePath );

        driver = new EdgeDriver( driverPath, options );

        string siteUsername = secretsConfig.AppSettings.Settings["site_username"].Value;
        string sitePassword = secretsConfig.AppSettings.Settings["site_password"].Value;
        loginUser( siteUsername, sitePassword );

        driver.Quit();

        Console.WriteLine( "Hello, World!" );
    }

    private static Configuration loadSecretsConfig() {
        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = @"Config\Secrets.config"
        };
        
        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
            fileMap,
            ConfigurationUserLevel.None
        );

        return configuration;
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
}