using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SMQCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    string url = "http://*:3002";
                    if (args.Length > 0)
                    {
                        url = args[0];
                    }

                    webBuilder.UseStartup<Startup>()
                    .UseUrls(url);
                });
    }
}