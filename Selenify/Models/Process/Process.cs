using Selenify.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Selenify.Models.Process
{
    public abstract class Process<T> : IProcess
    {
        public string ProcessName { get; set; } = string.Empty;
        public T State { get; set; } = default!;

        private string RunningProcessesDir = Directory.GetCurrentDirectory()! + "\\RunningProcesses\\";

        public abstract void Run();

        public Process(string processName)
        {
            ProcessName = processName;

            FileManager.CreateDirectoryIfNotExists(RunningProcessesDir);

            CreateStateFile();
            LoadState();
        }

        public void SaveState()
        {
            string stateFileName = ProcessName.Replace(" ", "") + ".state.json";
            File.WriteAllText(
                RunningProcessesDir + stateFileName,
                JsonSerializer.Serialize(State)
            );
        }

        public void LoadState()
        {
            string stateFilePath = RunningProcessesDir + ProcessName.Replace(" ", "") + ".state.json";
            string stateFile = File.ReadAllText(stateFilePath);

            State = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stateFile)!;
        }

        public void ResetState()
        {
            State = Activator.CreateInstance<T>();
            SaveState();
        }

        private void CreateStateFile()
        {
            string stateFileName = ProcessName.Replace(" ", "") + ".state.json";
            string stateFilePath = RunningProcessesDir + stateFileName;

            FileManager.CreateFileIfNotExists(stateFilePath);

            if (new FileInfo(stateFilePath).Length == 0)
            {
                State = Activator.CreateInstance<T>();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(State);
                File.WriteAllText(stateFilePath, json);
            }
        }
    }
}
