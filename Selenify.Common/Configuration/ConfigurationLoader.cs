using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Configuration
{
    public static class ConfigurationLoader
    {
        public static AppSettingsSection LoadConfig(string configPath)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configPath,
            };

            System.Configuration.Configuration configuration = ConfigurationManager
                .OpenMappedExeConfiguration(
                fileMap,
                ConfigurationUserLevel.None);

            return configuration.AppSettings;
        }

        public static AppSettingsSection LoadSecretsConfig()
        {
            return LoadConfig(@".\Configurations\Secrets.config");
        }
    }
}
