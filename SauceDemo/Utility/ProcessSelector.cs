using SauceDemo.Processes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Utility
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
                int selectedIndex = currentOptionIndex;
                selectedIndex += 1;

                if (selectedIndex > processesCount - 1) {
                    selectedIndex = 0;
                }

                return selectedIndex;
            }

            int GetPreviousOptionIndex() {
                int selectedIndex = currentOptionIndex;
                selectedIndex -= 1;

                if (selectedIndex < 0) {
                    selectedIndex = processesCount - 1;
                }

                return selectedIndex;
            }

            int countStringLength = processes.Count.ToString()
                .Length;
            bool userHasNotConfirmed = true;
            while( userHasNotConfirmed ) {
                string headerString = "Select a process <" + 
                    (currentOptionIndex + 1)
                        .ToString()
                        .PadLeft(countStringLength) +
                    "/" + 
                    processesCount + "> :\n";
                string processName = processes[currentOptionIndex].ProcessName;
                string footer = "\n\nPress enter to continue.";
                string uiString = headerString + processName + footer;
                ConsoleUI.Write( uiString );

                ConsoleKeyInfo key = Console.ReadKey();
                ConsoleKey keyCode = key.Key;


                if (keyCode == ConsoleKey.LeftArrow) {
                    currentOptionIndex = GetPreviousOptionIndex();
                }
                else if (keyCode == ConsoleKey.RightArrow) {
                    currentOptionIndex = GetNextOptionIndex();
                }
                else if (keyCode == ConsoleKey.Enter) {
                    userHasNotConfirmed = false;
                    ConsoleUI.Clear();
                    ConsoleUI.Stop();
                    return processes[currentOptionIndex];
                }
                else if (keyCode == ConsoleKey.Escape ) {
                    ConsoleUI.Clear();
                    ConsoleUI.Stop();
                    userHasNotConfirmed = false;
                }
            }

            return null;
        }
    }
}
