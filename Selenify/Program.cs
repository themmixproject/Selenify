using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using Selenify.Common.Utility;
using Selenify.Models.Process;
using Selenify.Common.Helpers;
using static Selenify.ApplicationInitializer;
using Selenify.Processes;
using Selenify.Common.Http;

internal class Program {
    private static void Main( string[] args ) {
        StartUp();

        IProcess? selectedProcess = new DownloadSauceImages();

        if (selectedProcess != null)
        {
            selectedProcess.Run();
        }
    }
}