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
                    // log.ClearProviders() // �������� ����������� ������������
                    // log.AddConsole(); // ���������� ���������
                    // log.AddConsole(opt => opt.IncludeScopes = true); // ���������� ���������
                    // log.AddProvider();
                    // log.AddFilter(level => level >= LogLevel.Information); // ������ �� ����� ������
                    // log.AddFilter("Microsoft", level => level >= LogLevel.Information); // �� ����������
                }));
    }
}
