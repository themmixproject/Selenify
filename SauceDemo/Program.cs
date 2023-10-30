using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using SauceDemo.Processes;
using SauceDemo.Utility;

internal class Program {
    public static IWebDriver driver;
    private static void Main( string[] args ) {
        WebDriverManager.LoadEdgeDriver();

        DownloadSauceImages process = new DownloadSauceImages();
        process.Run();
        Console.WriteLine( "Hello, World!" );
    }
}