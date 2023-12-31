﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Utility
{
    public static class ConfigurationLoader
    {
        public static AppSettingsSection LoadConfig(string configPath)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configPath,
            };

            Configuration configuration = System
                .Configuration
                .ConfigurationManager
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
