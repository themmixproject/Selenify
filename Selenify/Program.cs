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

internal class Program {
    private static void Main( string[] args ) {
        StartUp();

        IProcess? selectedProcess = new DownloadSauceImages();

        if (selectedProcess != null)
        {
            selectedProcess.Run();
        }

        //System.Console.Write(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent);
        //DownloadManager.DownloadFile("https://lh4.googleusercontent.com/-MC8WtWcan0I/AAAAAAAAAAI/AAAAAAAAAQI/bSW8SUtaZDU/photo.jpg?sz=192", downloadsDir);
        //await DownloadManager.DownloadAsync(new HttpClient(), "https://th.bing.com/th/id/OIP.-Sf7ke25iuyCKqmNG664GwHaFJ?rs=1&pid=ImgDetMain", downloadsDir);
    }
}