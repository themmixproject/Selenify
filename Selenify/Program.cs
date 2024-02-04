using Selenify.Common.AutoIt;
using static Selenify.ApplicationInitializer;

internal class Program {
    private static void Main( string[] args ) {
        StartUp();

        //IProcess? selectedProcess = new DownloadSauceImages();

        //if (selectedProcess != null)
        //{
        //    selectedProcess.Run();
        //}

        System.Diagnostics.Process.Start("C:\\Windows\\notepad.exe");
        string sendText = AutoItXExtensions.PrepareSendText("HELLO{ENTER}HELLO");

        AutoIt.AutoItX.WinActivate("Untitled - Notepad");
        Thread.Sleep(1000);
        AutoIt.AutoItX.Send(sendText);
    }
}