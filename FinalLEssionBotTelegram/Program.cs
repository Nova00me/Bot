using FinalLEssionBotTelegram;
using FinalLEssionBotTelegram.Configuration;
using FinalLEssionBotTelegram.Controllers;
using FinalLEssionBotTelegram.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;

namespace FinalEssionBotTelegram
{
    class Program
    {
        public static async Task Main() //И это тоже ))
        {
            Console.OutputEncoding = Encoding.Unicode;
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();
            Console.WriteLine("Сервис запущен");
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }
        private static void ConfigureServices(IServiceCollection services) // Не понимаю как это работает
        {
            AppSettings appSettings = BuildSettings();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddHostedService<Bot>();
        }
        private static AppSettings BuildSettings()
        {
            return new AppSettings()
            {
                BotToken = ""
            };
        }

    }
}