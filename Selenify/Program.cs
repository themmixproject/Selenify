using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using Selenify.Common.Utility;
using Selenify.Models.Process;

internal class Program {
    private static void Main( string[] args ) {
        IProcess? selectedProcess = new UserProcessSelectionHandler()
            .UserSelectProcess();

        if ( selectedProcess != null )
        {
            selectedProcess.Run();
        }
    }
}