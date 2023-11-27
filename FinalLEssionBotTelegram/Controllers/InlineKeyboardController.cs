using FinalLEssionBotTelegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FinalLEssionBotTelegram.Controllers
{
    internal class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramBotClient;
        public InlineKeyboardController(IStorage memoryStorage, ITelegramBotClient telegramBotClient)
        {
            _memoryStorage = memoryStorage;
            _telegramBotClient = telegramBotClient;
        }
        public async Task Handle(CallbackQuery? callback, CancellationToken cancellation)
        {
            if(callback?.Data == null)
            {
                return;
            }
            _memoryStorage.GetSession(callback.From.Id).Info = callback.Data;
            string Info = callback.Data switch
            {
                "str" => "Кол-во символов в строке",
                "sum" => "Сумирование чисел",
                _ => String.Empty
            };
            await _telegramBotClient.SendTextMessageAsync(callback.From.Id, $"<b> Вы выбрали </b> " +
                $"{Environment.NewLine} {Info}", cancellationToken:cancellation, parseMode:ParseMode.Html);
        }
    }
}
