using Microsoft.Extensions.Configuration;

namespace SMQCore.Tests.Helpers
{
    internal static class ConfigurationMock
    {
        public static IConfiguration Build()
        {
            return new ConfigurationBuilder().AddJsonFile("appSettings.Test.json").Build();
        }
    }
}