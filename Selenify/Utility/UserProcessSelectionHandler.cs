using OpenQA.Selenium.Internal;
using Selenify.ProcessOutlines;
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
    public class UserProcessSelectionHandler
    {
        private List<IProcess> Processes;
        private readonly int ProcessCount;

        public UserProcessSelectionHandler()
        {
            Processes = GetProcesses();
            ProcessCount = Processes.Count;
        }

        private List<IProcess> GetProcesses()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where( t => typeof( IProcess ).IsAssignableFrom( t ) && !t.IsAbstract )
            .ToList();

            var processes = new List<IProcess>();
            foreach (Type type in types)
            {
                var process = Activator.CreateInstance( type ) as IProcess;
                processes.Add( process! );
            }

            return processes;
        }

        public IProcess? UserSelectProcess() {
            bool userHasConfirmed = false;
            int? selectedIndex = 0;
            
            while( !userHasConfirmed ) {
                DisplayUI(selectedIndex);

                ConsoleKey key = System.Console.ReadKey().Key;
                selectedIndex = GetNewSelectedIndex(key, selectedIndex);
                userHasConfirmed = IsUserSelectionConfirmed(key);

                if (userHasConfirmed)
                {
                    Console.UI.Reset();
                    return GetSelectedProcess(selectedIndex);
                }
            }

            return null;
        }

        private void DisplayUI(int? selectedIndex)
        {
            int countStringLength = ProcessCount.ToString().Length;

            string headerString = $"Select a process <{(selectedIndex!.Value + 1)
                .ToString()
                .PadLeft(countStringLength)}/{ProcessCount}> :\n";
            string processName = Processes[selectedIndex!.Value].ProcessName;
            string footer = "\n\nPress enter to continue.\n" +
                "Use the arrow keys to select a proces.";
            string uiString = headerString + processName + footer;
            Console.UI.WriteLine(uiString);
        }

        private int? GetNewSelectedIndex(ConsoleKey key, int? oldIndex)
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

        private int GetNextOptionIndex(int? index)
        {
            return (index!.Value + 1) % ProcessCount;
        }

        private int GetPreviousOptionIndex(int? index)
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

        private bool IsUserSelectionConfirmed(ConsoleKey key)
        {
            if (key == ConsoleKey.Enter || key == ConsoleKey.Escape)
            {
                return true;
            }

            return false;
        }

        private IProcess? GetSelectedProcess(int? selectedIndex)
        {
            if (selectedIndex == null)
            {
                return null;
            }

            return Processes[selectedIndex!.Value];
        }

    }
}
