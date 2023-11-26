﻿using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using Selenify.Common.Utility;
using Selenify.Models.Process;
using Selenify.Common.Helpers;

internal class Program {
    private static void Main( string[] args ) {
        //IProcess? selectedProcess = new UserProcessSelectionHandler()
        //    .UserSelectProcess();

        //if ( selectedProcess != null )
        //{
        //    selectedProcess.Run();
        //}

        System.Console.Write(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent);
        string downloadsDir = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName + "\\Downloads\\";
        FileHelper.CreateDirectoryIfNotExists( downloadsDir );
        //DownloadManager.DownloadFile("https://lh4.googleusercontent.com/-MC8WtWcan0I/AAAAAAAAAAI/AAAAAAAAAQI/bSW8SUtaZDU/photo.jpg?sz=192", downloadsDir);
        DownloadManager.DownloadFileAsync("https://th.bing.com/th/id/OIP.-Sf7ke25iuyCKqmNG664GwHaFJ?rs=1&pid=ImgDetMain", downloadsDir);
    }
}