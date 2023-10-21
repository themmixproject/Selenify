using System;
using System.Configuration;

internal class Program {
    private static void Main( string[] args ) {
        string username = getUsernameFromSecretsConfig();

        Console.WriteLine( "Hello, World!" );
    }

    private static string getUsernameFromSecretsConfig() {
        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = @"Config\Secrets.config"
        };
        
        Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
            fileMap,
            ConfigurationUserLevel.None
        );

        return configuration.AppSettings.Settings["username"].Value;
    }
}