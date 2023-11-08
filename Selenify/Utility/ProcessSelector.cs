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

        private static int? SelectedIndex { get; set; } = 0;
        private static List<IProcessBase> Processes { get; set; } = GetProcesses();
        private static readonly int ProcessCount = Processes.Count;
        private static bool UserHasNotConfirmed { get; set; } = true;

        public static IProcessBase? SelectProcess() {
            while( UserHasNotConfirmed ) {
                DisplayUI();

                ConsoleKeyInfo key = System.Console.ReadKey();
                ConsoleKey keyCode = key.Key;

                SelectedIndex = GetNewSelectedIndex(keyCode);
                UserHasNotConfirmed = HandleUserConfirmation(keyCode);

                if ( !UserHasNotConfirmed )
                {
                    Console.UI.Reset();
                    if (SelectedIndex == null) {
                        return null;
                    }

                    return Processes[SelectedIndex!.Value];
                }
            }

            return null;
        }

        private static int? GetNewSelectedIndex(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow)
            {
                SelectedIndex = GetPreviousOptionIndex();
            }
            else if (key == ConsoleKey.RightArrow)
            {
                SelectedIndex = GetNextOptionIndex();
            }
            else if ( key == ConsoleKey.Escape)
            {
                SelectedIndex = null;
            }

            return SelectedIndex;
        }

        private static int GetNextOptionIndex()
        {
            return (SelectedIndex!.Value + 1) % ProcessCount;
        }

        private static int GetPreviousOptionIndex()
        {
            int index = (SelectedIndex!.Value - 1) % ProcessCount;
            if (index >= 0)
            {
                return index;
            }
            else
            {
                return index + ProcessCount;
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

        private static void DisplayUI() {
            int countStringLength = ProcessCount.ToString().Length;

            string headerString = $"Select a process <{(SelectedIndex!.Value + 1)
                .ToString()
                .PadLeft( countStringLength )}/{ProcessCount}> :\n";
            string processName = Processes[SelectedIndex!.Value].ProcessName;
            string footer = "\n\nPress enter to continue.\n" +
                "Use the arrow keys to select a proces.";
            string uiString = headerString + processName + footer;
            Console.UI.WriteLine( uiString );
        }
    }
}
