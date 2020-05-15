using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RancherNodeAutoScaler
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            // Run with console or service
            var asService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<NodeAutoScaler>();
            });
            //builder.UseEnvironment(asService ? EnvironmentName.Production : EnvironmentName.Development);
            if (asService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }
    }
}
