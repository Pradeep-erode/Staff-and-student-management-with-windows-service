using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Staff_and_student.Utilities
{
    public class Utility
    {
        static class ConfigurationManager
        {
            public static IConfiguration AppSetting { get; }
            static ConfigurationManager()
            {
                AppSetting = new ConfigurationBuilder()
                     .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                     .AddJsonFile("AppSettings.json")
                     .Build();
            }
        }
        public static string GetAppSettings(string key)
        {
            var value = ConfigurationManager.AppSetting.GetSection("AppSettings:" + key).Value;
            if (value != null)
            {
                return value.ToString();
            }
            return string.Empty;
        }
    }
}
