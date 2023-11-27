using FinalLEssionBotTelegram.Controllers;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FinalLEssionBotTelegram
{
    internal class Bot : BackgroundService
    {
        private ITelegramBotClient telegramBotClient;
        private TextMessageController _textMessageController;
        private InlineKeyboardController _keyboardController;
        public Bot(ITelegramBotClient botClient, TextMessageController textMessageController, InlineKeyboardController keyboardController)
        {
            telegramBotClient = botClient;
            _textMessageController = textMessageController;
            _keyboardController = keyboardController;
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if(update.Type == UpdateType.CallbackQuery)
            {
                await _keyboardController.Handle(update.CallbackQuery, cancellationToken);
            }   
            if(update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                       await _textMessageController.Handle(update.Message, cancellationToken);
                        break;
                }
            }
        }
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) 
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API error {apiRequestException.ErrorCode}" +
                $" {apiRequestException.Message}", _ => exception.ToString()
            };
            Console.WriteLine(errorMessage);
            Console.WriteLine("Задержка перед повторным подключение 10сек");
            Thread.Sleep(10000);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            telegramBotClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, cancellationToken: stoppingToken);
            Console.WriteLine("Бот запущен");
        }
    }
}
