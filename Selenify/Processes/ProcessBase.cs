﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Selenify.Processes
{
    public abstract class ProcessBase<T> : IProcessBase
    {
        public string ProcessName { get; set; } = string.Empty;
        public T State { get; set; } = default!;

        private string RunningProcessesDir = Directory.GetParent(
                Directory.GetCurrentDirectory()!
            )!.Parent!.Parent!.FullName + "\\RunningProcesses\\";

        public abstract void Run();

        public ProcessBase( string processName )
        {
            ProcessName = processName;

            CreateRunningProcessesDirectory();
            CreateStateFile();
            LoadState();
        }

        public void SaveState()
        {
            string stateFileName = ProcessName.Replace( " ", "" ) + ".state.json";
            File.WriteAllText(
                RunningProcessesDir + stateFileName,
                JsonSerializer.Serialize( State )
            );
        }

        public void LoadState()
        {
            string stateFilePath = RunningProcessesDir + ProcessName.Replace( " ", "" ) + ".state.json";
            string stateFile = File.ReadAllText( stateFilePath );

            State = Newtonsoft.Json.JsonConvert.DeserializeObject<T>( stateFile )!;
        }

        public void ResetState()
        {
            State = Activator.CreateInstance<T>();
            SaveState();
        }

        private void CreateStateFile()
        {
            string stateFileName = ProcessName.Replace( " ", "" ) + ".state.json";
            string stateFilePath = RunningProcessesDir + stateFileName;
            if (
                File.Exists( stateFilePath ) && new FileInfo( stateFilePath ).Length > 0)
            {
                return;
            }

            if (!File.Exists( stateFilePath )) {
                using (File.Create( stateFilePath )) { }
            }
            if (new FileInfo( stateFilePath).Length == 0)
            {
                State = Activator.CreateInstance<T>();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject( State );
                File.WriteAllText( stateFilePath, json );
            }
        }

        private void CreateRunningProcessesDirectory()
        {
            if (Directory.Exists( RunningProcessesDir ))
            {
                return;
            }
            Directory.CreateDirectory( RunningProcessesDir );
        }
    }
}