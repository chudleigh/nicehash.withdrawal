using Microsoft.Extensions.Configuration;
using Nicehash.Withdrawal.Config.Vo;
using System.IO;

namespace Nicehash.Withdrawal.Config
{
    public static class Configuration
    {
        public static NiceHashConfig NiceHash => Instance.GetSection(NiceHashConfig.SectionName).Get<NiceHashConfig>();

        public static ApiConfig Api => Instance.GetSection(ApiConfig.SectionName).Get<ApiConfig>();

        static Configuration()
        {
            Instance = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }

        private static readonly IConfiguration Instance;
    }
}