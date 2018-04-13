using System.Collections.Generic;
using System.Configuration;

namespace InventoryKeeper.Data.Helpers
{
    public static class AppSettingsHelper
    {
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];
                if (string.IsNullOrWhiteSpace(result))
                    throw new KeyNotFoundException(key + " Not found");
                else
                    return result;
            }
            catch (ConfigurationErrorsException)
            {
                return "Error reading app settings";
            }
        }
    }
}
