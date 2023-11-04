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
    private static void Main( string[] args ) {
        //WebDriverManager.LoadEdgeDriver();

        // ProcessSelector.GetProcesses();


        //DownloadSauceImages process = new DownloadSauceImages();
        //process.Run();
        Console.WriteLine( "hello world!" );
        ConsoleUI.Write( "Hello\nWorld!\nI am Steven!" );
        System.Threading.Thread.Sleep( 2000 );
        ConsoleUI.Write( "Banana,\nMango,\nApples" );
        System.Threading.Thread.Sleep( 2000 );
        ConsoleUI.Stop();
        Console.WriteLine( "This line won't be cleared by ConsoleUI." );
        System.Threading.Thread.Sleep( 2000 );
        ConsoleUI.Write( "Test1" );
        System.Threading.Thread.Sleep( 2000 );
        ConsoleUI.Write( "Test2" );
    }
}