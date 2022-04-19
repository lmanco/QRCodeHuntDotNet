using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace QRCodeHuntDotNet
{
    public class Program
    {
        public const string AppName = "QRCodeHuntDotNet";
        public const string AppDisplayName = "QR Code Hunt";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
