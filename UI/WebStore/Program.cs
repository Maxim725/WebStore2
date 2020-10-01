using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args)
           .Build()
           .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host => host
                   .UseStartup<Startup>()
                .ConfigureLogging((host, log) =>
                {
                    // log.ClearProviders() // удаление провайдеров конфигураций
                    // log.AddConsole(); // консольный провайдер
                    // log.AddConsole(opt => opt.IncludeScopes = true); // консольный провайдер
                    // log.AddProvider();
                    // log.AddFilter(level => level >= LogLevel.Information); // Фильтр по типам ошибки
                    // log.AddFilter("Microsoft", level => level >= LogLevel.Information); // По категориям
                }))
                .UseSerilog((host, log) =>
                    log.ReadFrom.Configuration(host.Configuration)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff zzz} {SourceContext} [{Level}]{NewLine}{Message}{NewLine}{Exception}")
                .WriteTo.RollingFile($@".\Log\WebStore[{DateTime.Now:yyyy-mm-ddTHH-mm-ss}].log")
                .WriteTo.File(new JsonFormatter(",", true, null), $@".\Log\WebStore[{DateTime.Now:yyyy-mm-ddTHH-mm-ss}].log.json")
                .WriteTo.Seq("http://localhost:5341/")
                );
    }
}
