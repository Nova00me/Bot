using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FinalLEssionBotTelegram.Controllers
{
    internal class TextMessageController
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IStorage _storage;
        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage storage)
        {
            _telegramBotClient = telegramBotClient;
            _storage = storage;
        }
        public async Task Handle(Message message, CancellationToken cancellationToken)
        {
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Количество сим в строке", $"str"),
                        InlineKeyboardButton.WithCallbackData($"Сумма чисел", $"sum")
                    });
                    await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"<b> Меню </b> {Environment.NewLine}" +
                        $" {Environment.NewLine}", cancellationToken: cancellationToken,
                        parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    if (_storage.GetSession(message.Chat.Id).Info == "str")
                    {
                        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"{message.Text.Length} символов {Environment.NewLine} " +
                            $"Введите  /start для выхода в главное меню", cancellationToken: cancellationToken);
                    }
                    if (_storage.GetSession(message.Chat.Id).Info == "sum")
                    {
                        string[] ints = message.Text.Split(' ');
                        int sum = 0;
                        try
                        {
                            for (int i = 0; i < ints.Length; i++)
                            {
                                sum += int.Parse(ints[i]);
                            }
                            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"{sum}" +
                                $"{Environment.NewLine}Введите  /start для выхода в главное меню", cancellationToken: cancellationToken);
                        }
                        catch
                        {
                            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Не корректный формат {Environment.NewLine}" +
                                $"Водите число число, через пробел");
                        }
                    }
                    break;
            }
        }
    }
}
