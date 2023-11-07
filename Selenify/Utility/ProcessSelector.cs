using OpenQA.Selenium.Internal;
using Selenify.Processes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Utility
{
    public static class ProcessSelector
    {
        public static List<IProcessBase> GetProcesses()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where( t => typeof( IProcessBase ).IsAssignableFrom( t ) && !t.IsAbstract )
            .ToList();

            var processes = new List<IProcessBase>();
            foreach (Type type in types)
            {
                var process = Activator.CreateInstance( type ) as IProcessBase;
                processes.Add( process! );
            }

            return processes;
        }

        private static int SelectedIndex = 0;
        private static List<IProcessBase> Processes = GetProcesses();
        private static readonly int ProcessCount = Processes.Count;

        private static int GetNextOptionIndex()
        {
            return (SelectedIndex + 1) % ProcessCount;
        }

        private static int GetPreviousOptionIndex()
        {
            int index = (SelectedIndex - 1) % ProcessCount;
            if (index >= 0)
            {
                return index;
            }
            else
            {
                return index + ProcessCount;
            }
        }

        public static IProcessBase? SelectProcess() {
            bool userHasNotConfirmed = true;
            while( userHasNotConfirmed ) {
                DisplayUI();

                ConsoleKeyInfo key = System.Console.ReadKey();
                ConsoleKey keyCode = key.Key;

                if (keyCode == ConsoleKey.LeftArrow) {
                    SelectedIndex = GetPreviousOptionIndex();
                }
                else if (keyCode == ConsoleKey.RightArrow) {
                    SelectedIndex = GetNextOptionIndex();
                }
                else if (keyCode == ConsoleKey.Enter) {
                    ConsoleUI.Reset();
                    return Processes[SelectedIndex];
                }
                else if (keyCode == ConsoleKey.Escape ) {
                    ConsoleUI.Reset();
                    userHasNotConfirmed = false;
                }
            }

            return null;
        }

        private static void DisplayUI() {
            int countStringLength = ProcessCount.ToString().Length;

            string headerString = $"Select a process <{(SelectedIndex + 1)
                .ToString()
                .PadLeft( countStringLength )}/{ProcessCount}> :\n";
            string processName = Processes[SelectedIndex].ProcessName;
            string footer = "\n\nPress enter to continue.\n" +
                "Use the arrow keys to select a proces.";
            string uiString = headerString + processName + footer;
            Console.UI.WriteLine( uiString );
        }
    }
}
