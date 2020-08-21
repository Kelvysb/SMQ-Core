using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SMQCoreManager.Business;
using SMQCoreManager.Business.Interfaces;
using SMQCoreManager.Services;
using SMQCoreManager.Services.Interfaces;

namespace SMQCoreManager
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton(async p =>
            {
                var httpClient = p.GetRequiredService<HttpClient>();
                return await httpClient.GetFromJsonAsync<Settings>("appsettings.json");
            });

            builder.Services.AddSingleton<IAppsService, AppsService>();
            builder.Services.AddSingleton<IUsersService, UsersService>();
            builder.Services.AddSingleton<IQueueService, QueueService>();
            builder.Services.AddSingleton<ISMQCoreBusiness, SMQCoreBusiness>();

            await builder.Build().RunAsync();
        }
    }
}