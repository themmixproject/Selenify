using SauceDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Processes
{

    public class DownloadSauceImages : IProcessBase
    {
        public string ProcessName { get; set; } = "Download Sauce Images";
        private class ProcessState {
        };
        private ProcessState state { get; set; } = new ProcessState();


        public void Run()
        {
            LoginUser();
        }

        public void LoginUser()
        {
            AppSettingsSection config = loadSecretsConfig();
            string siteUsername = config.Settings["site_username"].Value;
            string sitePassword = config.Settings["site_password"].Value;

            Console.WriteLine(siteUsername);
        }

        private static AppSettingsSection loadSecretsConfig()
        {
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap
                {
                    ExeConfigFilename = @".\Config\Secrets.config"
                },
                ConfigurationUserLevel.None
            );

            return configuration.AppSettings;
        }
    }
}
