using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TriviadorClient.Entities;

namespace TriviadorClient
{
    public class Program
    {
        public static void Start()
        {
            CreateServer().Start();
            //        WebHost.CreateDefaultBuilder().UseKestrel(x =>
            //        {
            //            x.ListenAnyIP(8080);
            //        }).UseStartup<Startup>()
            //.Build();
            CreateHostBuilder(null).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                { 
                    webBuilder.UseStartup<Startup>();
                });

        public static Server CreateServer()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            return new Server(loggerFactory.CreateLogger("LogLevel"));
        }
    }
}
