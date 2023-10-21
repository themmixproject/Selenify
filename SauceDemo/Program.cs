using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;

internal class Program {
    public static IWebDriver driver;
    private static void Main( string[] args ) {
        string binaryPath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
        string driverPath = "C:\\Webdrivers\\msedgedriver.exe";

        string username = getUsernameFromSecretsConfig();
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

        driver.Quit();

        Console.WriteLine( "Hello, World!" );
    }

    private static string getUsernameFromSecretsConfig() {
        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = @"Config\Secrets.config"
        };
        
        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
            fileMap,
            ConfigurationUserLevel.None
        );

        return configuration.AppSettings.Settings["username"].Value;
    }
}