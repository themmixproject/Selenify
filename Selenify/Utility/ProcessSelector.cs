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
using static System.Runtime.CompilerServices.RuntimeHelpers;

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

        private static List<IProcessBase> Processes { get; set; } = GetProcesses();
        private static readonly int ProcessCount = Processes.Count;
        public static IProcessBase? SelectProcess() {
            bool userHasNotConfirmed = true;
            int? selectedIndex = 0;
            
            while( userHasNotConfirmed ) {
                DisplayUI(selectedIndex);

                ConsoleKey key = System.Console.ReadKey().Key;
                selectedIndex = GetNewSelectedIndex(key, selectedIndex);
                userHasNotConfirmed = HandleUserConfirmation(key);

                if ( !userHasNotConfirmed )
                {
                    Console.UI.Reset();
                    if (selectedIndex == null)
                    {
                        return null;
                    }

                    return Processes[selectedIndex!.Value];
                }
            }

            return null;
        }

        private static int? GetNewSelectedIndex(ConsoleKey key, int? oldIndex)
        {
            int? newIndex = 0;

            if (key == ConsoleKey.LeftArrow)
            {
                newIndex = GetPreviousOptionIndex(oldIndex);
            }
            else if (key == ConsoleKey.RightArrow)
            {
                newIndex = GetNextOptionIndex(oldIndex);
            }
            else if ( key == ConsoleKey.Escape)
            {
                newIndex = null;
            }

            return newIndex;
        }

        private static int GetNextOptionIndex(int? index)
        {
            return (index!.Value + 1) % ProcessCount;
        }

        private static int GetPreviousOptionIndex(int? index)
        {
            int newIndex = (index!.Value - 1) % ProcessCount;
            if (newIndex >= 0)
            {
                return newIndex;
            }
            else
            {
                return newIndex + ProcessCount;
            }
        }

        private static bool HandleUserConfirmation(ConsoleKey key)
        {
            if (key == ConsoleKey.Enter || key == ConsoleKey.Escape)
            {
                return false;
            }

            return true;
        }

        private static void DisplayUI(int? selectedIndex) {
            int countStringLength = ProcessCount.ToString().Length;

            string headerString = $"Select a process <{(selectedIndex!.Value + 1)
                .ToString()
                .PadLeft( countStringLength )}/{ProcessCount}> :\n";
            string processName = Processes[selectedIndex!.Value].ProcessName;
            string footer = "\n\nPress enter to continue.\n" +
                "Use the arrow keys to select a proces.";
            string uiString = headerString + processName + footer;
            Console.UI.WriteLine( uiString );
        }
    }
}
