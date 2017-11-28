using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CatAList.Services;
using Microsoft.Extensions.DependencyInjection;
using CatAList.Services.External;
using Microsoft.Extensions.Options;

namespace CatAList
{
    class Program
    {
        static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            // Load configurations
            BuildConfigurations();

            // Build our service provider
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            // Run our app
            serviceProvider.GetService<App>().RunAsync().Wait();
        }

        static void BuildConfigurations()
        {
            string environmentName = Environment.GetEnvironmentVariable("DOTNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                // No environment settings are used on this project, but this is a good practice.
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
            // Build all our configuration into a single Configuration object.
            Configuration = builder.Build();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Configure logger and add to services
            var loggerFactory = new LoggerFactory()
                .AddDebug();        
            services.AddSingleton(loggerFactory);
            services.AddLogging();

            // Get the "PeopleServiceInfo" from our config and provide it to anyone who needs it.
            services.Configure<PeopleServiceInfo>(options => Configuration.GetSection("PeopleServiceInfo").Bind(options));
            services.AddSingleton(sp => sp.GetService<IOptionsSnapshot<PeopleServiceInfo>>().Value);
            
            // Add our PeopleGetter and CatServices to our service collection
            services.AddSingleton<IPeopleGetter, PeopleGetter>();

            services.AddTransient<ICatService, CatService>();
            // Add console for input and output for our App
            services.AddSingleton(Console.Out);
            services.AddSingleton(Console.In);

            // Add our app
            services.AddTransient<App>();

        }

    }
}