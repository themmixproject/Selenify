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

        public static IProcessBase? SelectProcess() {
            List<IProcessBase> processes = GetProcesses();
            int processesCount = processes.Count;
            int currentOptionIndex = 0;
            
            int GetNextOptionIndex() {
                return (currentOptionIndex + 1) % processesCount;
            }

            int GetPreviousOptionIndex() {
                return (currentOptionIndex - 1) % processesCount;
            }

            bool userHasNotConfirmed = true;
            while( userHasNotConfirmed ) {
                DisplayUI(processes, currentOptionIndex);

                ConsoleKeyInfo key = Console.ReadKey();
                ConsoleKey keyCode = key.Key;

                if (keyCode == ConsoleKey.LeftArrow) {
                    currentOptionIndex = GetPreviousOptionIndex();
                }
                else if (keyCode == ConsoleKey.RightArrow) {
                    currentOptionIndex = GetNextOptionIndex();
                }
                else if (keyCode == ConsoleKey.Enter) {
                    ConsoleUI.Reset();
                    return processes[currentOptionIndex];
                }
                else if (keyCode == ConsoleKey.Escape ) {
                    ConsoleUI.Reset();
                    userHasNotConfirmed = false;
                }
            }

            return null;
        }

        private static void DisplayUI(List<IProcessBase> processes, int optionIndex) {
            int countStringLength = processes.Count.ToString().Length;

            string headerString = $"Select a process <{(optionIndex + 1)
                .ToString()
                .PadLeft( countStringLength )}/{processes.Count}> :\n";
            string processName = processes[optionIndex].ProcessName;
            string footer = "\n\nPress enter to continue.";
            string uiString = headerString + processName + footer;
            ConsoleUI.WriteLines( uiString );
        }
    }
}
