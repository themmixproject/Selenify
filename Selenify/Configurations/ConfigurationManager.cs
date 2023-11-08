using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Configurations
{
    public static class ConfigurationManager
    {
        private static AppSettingsSection? _secrets;

        public static AppSettingsSection Secrets
        {
            get
            {
                if (_secrets == null)
                {
                    _secrets = LoadSecretsConfig();
                }

                return _secrets!;
            }
        }

        public static AppSettingsSection LoadSecretsConfig()
        {
            Configuration configuration = System
                .Configuration
                .ConfigurationManager
                .OpenMappedExeConfiguration(
                    new ExeConfigurationFileMap
                    {
                        ExeConfigFilename = @".\Config\Secrets.config"
                    },
                    ConfigurationUserLevel.None
                );

            return configuration.AppSettings;
        }

        public static void UnloadSecretsConfig()
        {
            _secrets = null;
        }
    }
}
