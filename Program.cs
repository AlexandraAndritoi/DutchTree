using DutchTree.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTree
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            RunSeeding(host);
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetUpConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SetUpConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            // Removing the default configuration options
            builder.Sources.Clear();

            builder.AddJsonFile("config.json", false, true)
                   .AddEnvironmentVariables();
        }

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
            using var scope = scopeFactory?.CreateScope();
            var seeder = scope?.ServiceProvider.GetService(typeof(DutchSeeder)) as DutchSeeder;
            seeder?.Seed().Wait();
        }
    }
}
