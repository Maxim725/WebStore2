using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                }));
    }
}
