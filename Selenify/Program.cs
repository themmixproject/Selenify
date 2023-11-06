using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using Selenify.Processes;
using Selenify.Utility;

internal class Program {
    private static void Main( string[] args ) {
        //IProcessBase selectedProcess = ProcessSelector.SelectProcess()!;
        ConsoleUI.WriteLines("Line 1", "Line 2", "Line 3");
        Thread.Sleep(2000);
        ConsoleUI.UpdateLine(1, "Hello world!");
        Thread.Sleep(2000);
        var i = 0;
    }
}