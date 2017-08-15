using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EncryptAppConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            //EncryptAppConfigValue("App.config");
            var secretUri  = ConfigurationManager.AppSettings["VstsTestManagementPat.KeyVaultUri"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            Console.WriteLine("Complete");
        }

        static void EncryptAppConfigValue(string fileName)
        {
            Configuration config = null;
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var configSection = config.AppSettings;
                if (!configSection.ElementInformation.IsLocked && !configSection.SectionInformation.IsLocked)
                {
                    configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                }

                configSection.SectionInformation.ForceSave = true;
                config.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    
}
