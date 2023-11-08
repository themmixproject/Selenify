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
        IProcessBase selectedProcess = ProcessSelector.UserSelectProcess()!;
        selectedProcess.Run();
    }
}